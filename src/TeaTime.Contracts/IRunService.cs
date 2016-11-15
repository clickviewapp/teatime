namespace TeaTime.Contracts
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IRunService
    {
        Task<bool> Start(SlashCommand command);
        Task<Order> AddOrder(SlashCommand command, string text);
        Task<bool> End(string channelId);
        Task<Run> Get(string channelId);
        Task<IEnumerable<OrderViewModel>> GetOrders(Run run);
    }
}