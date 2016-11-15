namespace TeaTime.Contracts.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Data;

    public interface IModuleRepository
    {
        Task<Module> GetModule(string command);
        Task<IEnumerable<InventoryItem>> GetInventory(string command);
        Task<IEnumerable<InventoryItem>> GetInventory(ulong moduleId);
    }
}