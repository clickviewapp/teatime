namespace TeaTime.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts.Data;
    using Contracts.Data.Repositories;
    using Contracts.Data.Repository;
    using Models.Data;

    public class ModuleRepository : BaseRepository, IModuleRepository
    {
        public ModuleRepository(IUnitOfWork unitOfWork) : base(unitOfWork) {}

        public async Task<Module> GetModule(string command)
        {
            var sql = $"SELECT * FROM `{Strings.Tables.Modules}` `m` WHERE `m`.`Command` = @command;";
            return await this.QuerySingle<Module>(sql, new {command});
        }

        public async Task<IEnumerable<InventoryItem>> GetInventory(string command)
        {
            var mod =  await this.GetModule(command);

            if(mod == null)
                return null;

            return await this.GetInventory(mod.Id);
        }

        public async Task<IEnumerable<InventoryItem>> GetInventory(ulong moduleId)
        {
            var s = $@"SELECT * FROM `{Strings.Tables.InventoryItems}` `i` WHERE `i`.`ModuleId` = @moduleId;";

            var s1 = $@"SELECT 
	                        `o`.*, `i`.`Id` AS `InventoryItemId`
                        FROM
	                        `{Strings.Tables.InventoryItems}` `i`
                        JOIN 
	                        `{Strings.Tables.InventoryItemsOptions}` `o` ON `o`.`InventoryItemId` = `i`.`Id`
                        WHERE
                            `i`.`ModuleId` = @moduleId;";


            Action<InventoryItem, IEnumerable<InventoryItemOption>> map = (item, options) =>
            {
                item.Options.AddRange(options);
            };
            
            return await this.GetManyToMany(s + s1, new { moduleId }, option => option.InventoryItemId, map);
        }
    }
}