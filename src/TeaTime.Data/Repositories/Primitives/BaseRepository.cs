namespace TeaTime.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts.Data;
    using Dapper;
    using Models.Data.Primitives;

    public abstract class BaseRepository
    {
        protected readonly IUnitOfWork UnitOfWork;

        protected BaseRepository(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        protected async Task<TReturnType> Read<TReturnType>(Func<IDbConnection, Task<TReturnType>> func)
        {
            return await func(GetConnection());
        }

        protected async Task Read(Func<IDbConnection, Task> func)
        {
            await func(GetConnection());
        }

        protected async Task<TObject> Write<TObject>(Func<IDbConnection, Task<TObject>> func)
        {
            return await func(GetWriteConnection());
        }

        public async Task<ulong> InsertSingle(string sql, object parameters)
        {
            return await this.Write(async connection => (await connection.QueryAsync<ulong>(sql, parameters, UnitOfWork.GetTransaction())).FirstOrDefault());

        }

        public async Task<int> Insert(string sql, object parameters)
        {
            return await this.Write(async connection => (await connection.ExecuteAsync(sql, parameters, UnitOfWork.GetTransaction())));
        }

        public async Task<TObject> QuerySingle<TObject>(string sql, object parameters)
        {
            return (await this.QueryMany<TObject>(sql, parameters)).FirstOrDefault();
        }

        public async Task<TObject> QuerySingle<TObject>(string sql)
        {
            return (await this.QueryMany<TObject>(sql, new { })).FirstOrDefault();
        }

        public async Task<object> QuerySingle(Type type, string sql, object parameters)
        {
            return (await this.QueryMany(type, sql, parameters)).FirstOrDefault();
        }

        public async Task<IEnumerable<TObject>> QueryMany<TObject>(string sql, object parameters)
        {
            return await this.Read(async con => await con.QueryAsync<TObject>(sql, parameters, UnitOfWork.GetTransaction(false)));
        }

        public async Task<IEnumerable<TObject>> QueryMany<TObject>(string sql)
        {
            return await this.Read(async con => await con.QueryAsync<TObject>(sql, transaction: UnitOfWork.GetTransaction(false)));
        }

        public async Task<IEnumerable<object>> QueryMany(Type type, string sql, object parameters)
        {
            return await this.Read(async con => await con.QueryAsync(type, sql, parameters, UnitOfWork.GetTransaction(false)));
        }

        public async Task<bool> Execute(string sql, object parameters)
        {
            return await this.Write(async connection => await connection.ExecuteAsync(sql, parameters)) > 0;
        }

        public async Task<Tuple<IEnumerable<object>, long>> QueryPagedCollection(Type type, string sql, object parameters)
        {
            var conn = GetConnection();
            var reader = await conn.QueryMultipleAsync(sql, parameters, UnitOfWork.GetTransaction(false));

            var objects = reader.Read(type).ToList();
            var count = reader.Read<long>().Single();

            return new Tuple<IEnumerable<object>, long>(objects, count);
        }

        protected async Task<IEnumerable<TFirst>> GetManyToMany<TFirst, TSecond>(string sql, object obj, Func<TSecond, ulong> secondKey, Action<TFirst, IEnumerable<TSecond>> addChildren) where TFirst : BaseEntity where TSecond : BaseEntity
        {
            return (await GetConnection()
                              .QueryMultipleAsync(sql, obj))
                              .Map<TFirst, TSecond, ulong>
                              (
                                  first => first.Id,
                                  secondKey,
                                  addChildren
                              );
        }

        protected async Task<TFirst> GetOneToMany<TFirst, TSecond>(string sql, object obj, Func<TSecond, ulong> secondKey, Action<TFirst, IEnumerable<TSecond>> addChildren) where TFirst : BaseEntity where TSecond : BaseEntity
        {
            return (await GetConnection()
                              .QueryMultipleAsync(sql, obj))
                              .Map<TFirst, TSecond, ulong>
                              (
                                  first => first.Id,
                                  secondKey,
                                  addChildren
                              ).FirstOrDefault();
        }

        protected async Task<TType> QueryScalarAsync<TType>(string sql, object parameters) where TType : struct
        {
            return await this.ScalarAsync(async con => await con.ExecuteScalarAsync<TType>(sql, parameters));
        }

        private async Task<TType> ScalarAsync<TType>(Func<IDbConnection, Task<TType>> scalarAction)
        {
            using (var con = GetConnection())
            {
                return await scalarAction(con);
            }
        }

        private IDbConnection GetConnection()
        {
            return UnitOfWork.GetReadOnlyConnection();
        }

        private IDbConnection GetWriteConnection()
        {
            return UnitOfWork.GetWriteConnection();
        }
    }

    public static class DapperExtensions
    {
        public static IEnumerable<TFirst> Map<TFirst, TSecond, TKey>(this SqlMapper.GridReader reader, Func<TFirst, TKey> firstKey, Func<TSecond, TKey> secondKey, Action<TFirst, IEnumerable<TSecond>> addChildren)
        {
            var first = reader.Read<TFirst>().ToList();

            var childMap = reader.Read<TSecond>()
                .GroupBy(secondKey)
                .ToDictionary(g => g.Key, g => g.AsEnumerable());

            foreach (var item in first)
            {
                IEnumerable<TSecond> children;
                if (childMap.TryGetValue(firstKey(item), out children))
                {
                    addChildren(item, children);
                }
            }

            return first;
        }
    }
}