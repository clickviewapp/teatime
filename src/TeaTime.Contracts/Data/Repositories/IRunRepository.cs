namespace TeaTime.Contracts.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRunRepository
    {
        Task<Run> Find(string channelId);
        Task<Run> Create(Run newRun);
        Task<Order> AddOrder(Order order);
        Task<IEnumerable<Order>> GetOrders(ulong runId);
        Task<bool> UpdateRun(Run run);
    }
}