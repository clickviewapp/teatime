namespace TeaTime.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Contracts.Data;
    using Contracts.Data.Repositories;
    using Models;

    public class RunRepository : BaseRepository, IRunRepository
    {
        public RunRepository(IUnitOfWork unitOfWork) : base(unitOfWork) {}

        public async Task<Run> Find(string channelId)
        {
            var sql = $"SELECT * FROM `{Strings.Tables.Runs}` `r` WHERE `r`.`ChannelId` = @channelId And Ended = 0;";
            return await this.QuerySingle<Run>(sql, new { channelId });
        }

        public async Task<Run> Create(Run run)
        {
            var sql = $"INSERT INTO {Strings.Tables.Runs} (Name, UserId, ModuleId, ChannelId, DateCreated) VALUES (@name, @userId, @moduleId, @channelId, @dateCreated);";

            var query = sql + "SELECT LAST_INSERT_ID() AS LastInsertedId;";
            run.DateCreated = DateTime.UtcNow;

            run.Id = await this.QueryScalarAsync<ulong>(query, run);

            return run;
        }

        public async Task<Order> AddOrder(Order order)
        {
            var sql = $"INSERT INTO {Strings.Tables.Orders} (Text, UserId, RunId, DateCreated) VALUES (@text, @userId, @runId, @dateCreated) ON DUPLICATE KEY UPDATE DateModified=@dateCreated, Text=@text;";

            var query = sql + "SELECT LAST_INSERT_ID() AS LastInsertedId;";
            order.DateCreated = DateTime.UtcNow;

            order.Id = await this.QueryScalarAsync<ulong>(query, order);

            return order;
        }

        public Task<IEnumerable<Order>> GetOrders(ulong runId)
        {
            var sql = $"SELECT * FROM `{Strings.Tables.Orders}` WHERE RunId = @runId;";
            return this.QueryMany<Order>(sql, new { runId });
        }

        public Task<bool> UpdateRun(Run run)
        {
            var sql = $"UPDATE `{Strings.Tables.Runs}` SET Ended=@ended,DateModified=@dateModified WHERE Id=@id";

            run.DateModified = DateTime.UtcNow;

            return this.Execute(sql, run);
        }
    }
}