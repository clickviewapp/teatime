namespace TeaTime.Data.Repositories
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Contracts.Data;
    using Contracts.Data.Repository;
    using Models;

    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) {}

        public async Task<User> Find(string id)
        {
            var sql = $"SELECT * FROM {Strings.Tables.Users} `u` WHERE u.UserId = @userId";
            return await this.QuerySingle<User>(sql, new {userId = id});
        }

        public async Task<IEnumerable<User>> GetManyById(IEnumerable<ulong> ids)
        {
            var sql = $"SELECT * FROM {Strings.Tables.Users} WHERE Id IN @ids";
            return await this.QueryMany<User>(sql, new { ids });
        }

        public async Task<User> Create(User user)
        {
            try
            {
                var sql =
                    $"INSERT INTO {Strings.Tables.Users} (Name, UserId, DateCreated) VALUES (@name, @userId, @dateCreated);";

                var query = sql + "SELECT LAST_INSERT_ID() AS LastInsertedId;";
                user.DateCreated = DateTime.UtcNow;

                user.Id = await this.QueryScalarAsync<ulong>(query, user);

                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
