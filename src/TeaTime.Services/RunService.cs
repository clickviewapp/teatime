namespace TeaTime.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Contracts;
    using Contracts.Data.Repositories;
    using Models;
    using System.Linq;

    public class RunService : IRunService
    {
        private readonly IUserService _userService;
        private readonly IRunRepository _runRepository;
        private readonly IModuleRepository _moduleRepository;

        public RunService(IUserService userService, IRunRepository runRepository, IModuleRepository moduleRepository)
        {
            _userService = userService;
            _runRepository = runRepository;
            _moduleRepository = moduleRepository;
        }

        public async Task<bool> Start(SlashCommand command)
        {
            var run = await _runRepository.Find(command.channel_id);

            if (run != null && !run.Ended)
                return false;

            var owner = await _userService.GetOrCreateUser(command.user_id, command.user_name);

            var module = await _moduleRepository.GetModule(command.text);

            if (module == null)
                return false;

            var newRun = new Run
            {
                UserId = owner.Id,
                ModuleId = module.Id,
                ChannelId = command.channel_id
            };

            var success = await _runRepository.Create(newRun);

            return true;
        }

        public async Task<Order> AddOrder(SlashCommand command, string text)
        {
            var teaTime = await this.Get(command.channel_id);

            if (teaTime == null)
            {
                return null;
            }

            var owner = await _userService.GetOrCreateUser(command.user_id, command.user_name);
            
            return await _runRepository.AddOrder(new Order
            {
                UserId = owner.Id,
                RunId = teaTime.Id,
                Text = text
            });
        }

        public async Task<bool> End(string channelId)
        {
            var teaTime = await this.Get(channelId);
            if (teaTime == null)
            {
                return false;
            }

            teaTime.Ended = true;
            return await _runRepository.UpdateRun(teaTime);
        }

        public async Task<Run> Get(string channelId)
        {
            return await _runRepository.Find(channelId);
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrders(Run run)
        {
            var orders = (await _runRepository.GetOrders(run.Id)).ToList();

            if(!orders.Any())
                return new List<OrderViewModel>();

            var userIds = orders.Select(o => o.UserId);

            var users = (await _userService.GetManyById(userIds)).ToList();

            return orders.Select(order => new OrderViewModel(order)
            {
                User = users.FirstOrDefault(u => u.Id == order.UserId)
            });
        }
    }
}