namespace TeaTime.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using Contracts.Data.Repository;
    using Models;

    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<User> GetUser(string id)
        {
            return await _repository.Find(id);
        }

        public async Task<User> CreateUser(User user)
        {
            return await _repository.Create(user);
        }

        public async Task<User> GetOrCreateUser(string userId, string name)
        {
            var user = await this.GetUser(userId);
            if (user != null)
                return user;

           return await this.CreateUser(new User {UserId = userId, Name = name});
        }

        public Task<IEnumerable<User>> GetManyById(IEnumerable<ulong> ids)
        {
            return _repository.GetManyById(ids);
        }

        public async Task<User> Get(ulong id)
        {
            var users = await _repository.GetManyById(new[] {id});
            return users.FirstOrDefault();
        }
    }
}