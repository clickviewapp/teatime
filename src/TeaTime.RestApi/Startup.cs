namespace TeaTime.RestApi
{
    using Contracts;
    using Newtonsoft.Json.Serialization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Services;
    using TeaTime.Commands.Activation;
    using TeaTime.Commands.Core;
    using TeaTime.Commands.Loader;
    using System.Linq;
    using Contracts.Data;
    using Contracts.Data.Repositories;
    using Contracts.Data.Repository;
    using Data;
    using Data.Repositories;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IConnectionFactory>(f => new ConnectionFactory("***Removed***"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IRunRepository, RunRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IRunService, RunService>();

            // Add framework services.
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            
            RegisterCommands(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.UseMvc();
        }

        private static void RegisterCommands(IServiceCollection services)
        {
            var commands = CommandLoader.GetCommands().ToList();

            foreach (var command in commands)
            {
                services.AddTransient(command);
            }

            services.AddSingleton<ICommandActivator, CommandActivator>();
            services.AddSingleton<ICommandRunner, CommandRunner>();

            services.AddSingleton(a =>
            {
                var table = new CommandTable(a.GetService<ICommandActivator>());
                table.LoadCommands(commands);
                return table;
            });
        }
    }
}
