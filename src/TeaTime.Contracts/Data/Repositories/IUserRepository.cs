namespace TeaTime.Contracts.Data.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IUserRepository
    {
        Task<User> Find(string id);
        Task<User> Create(User user);
        Task<IEnumerable<User>> GetManyById(IEnumerable<ulong> ids);
    }
}