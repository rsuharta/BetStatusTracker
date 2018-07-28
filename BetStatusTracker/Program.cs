namespace BetStatusTracker
{
    using System;
    using System.IO;

    using BetStatusTracker.Command.BetRegistration;
    using BetStatusTracker.Repositories;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using Paramore.Brighter;
    using Paramore.Brighter.CommandStore.MsSql;
    using Paramore.Brighter.Eventsourcing.Handlers;

    class Program
    {
        static void Main(string[] args)
        {
            var commandStore =
                new MsSqlCommandStore(new MsSqlCommandStoreConfiguration("Server=localhost;Database=BetStatusTracker;Trusted_Connection=True;", "Command"));

            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json").Build();

            var serviceProvider = BuildServiceProvider(commandStore, configuration);

            var registry = new SubscriberRegistry();
            registry.Register<BetRegistrationCommand, BetRegistrationCommandHandler>();

            var builder = CommandProcessorBuilder.With()
                .Handlers(new HandlerConfiguration(
                    subscriberRegistry: registry,
                    handlerFactory: new CommandHandlerFactory(serviceProvider))).DefaultPolicy().NoTaskQueues()
                .RequestContextFactory(new InMemoryRequestContextFactory());
            
            var commandProcessor = builder.Build();
            var command = new BetRegistrationCommand
                              {
                                  BetClientRef = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                                  SequenceNo = 1,
                                  BetSubmittedTime = DateTimeOffset.Now,
                                  CustomerId = "1234",
                                  MessageTimeStamp = DateTimeOffset.Now,
                                  StakeAmount = 2.1m
                              };

            commandProcessor.Send(command);

            var retrievedCommand = commandStore.Get<BetRegistrationCommand>(command.Id);

            Console.WriteLine($"Retrieved command Id: {retrievedCommand.Id}");

            Console.ReadLine();
        }

        private static IServiceProvider BuildServiceProvider(IAmACommandStore commandStore, IConfiguration Configuration)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<BetRegistrationCommandHandler>();
            
            var commandSourcingHandler = new CommandSourcingHandler<BetRegistrationCommand>(commandStore);
            serviceCollection.Add(new ServiceDescriptor(typeof(CommandSourcingHandler<BetRegistrationCommand>), p => commandSourcingHandler, ServiceLifetime.Transient));
            serviceCollection.AddDbContext<BetStatusTrackerContext>(options => options.UseLazyLoadingProxies()
                .UseSqlServer(Configuration.GetConnectionString("DbConnection")));

            return serviceCollection.BuildServiceProvider();
        }
        
    }
}
