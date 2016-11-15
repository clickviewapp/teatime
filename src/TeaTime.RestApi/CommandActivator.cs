namespace TeaTime.RestApi
{
    using System;
    using TeaTime.Commands.Activation;

    public class CommandActivator : ICommandActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object Create(Type type)
        {
            return _serviceProvider.GetService(type);
        }
    }
}
