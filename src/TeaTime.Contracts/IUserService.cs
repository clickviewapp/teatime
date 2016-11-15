namespace TeaTime.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IUserService
    {
        Task<User> GetUser(string id);
        Task<User> CreateUser(User user);
        Task<User> GetOrCreateUser(string userId, string name);
        Task<IEnumerable<User>> GetManyById(IEnumerable<ulong> ids);
        Task<User> Get(ulong id);
    }
}
