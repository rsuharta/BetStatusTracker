using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Paramore.Brighter;
using Paramore.Brighter.CommandStore.MsSql;
using Paramore.Brighter.Eventsourcing.Handlers;

namespace BetStatusTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandStore =
                new MsSqlCommandStore(new MsSqlCommandStoreConfiguration("Server=localhost;Database=BetStatusTracker;Trusted_Connection=True;", "Command"));

            var serviceProvider = BuildServiceProvider(commandStore);

            var registry = new SubscriberRegistry();
            registry.Register<BetRegistrationCommand, BetRegistrationCommandHandler>();

            var builder = CommandProcessorBuilder.With()
                .Handlers(new HandlerConfiguration(
                    subscriberRegistry: registry,
                    handlerFactory: new CommandHandlerFactory(serviceProvider)
                ))
                .DefaultPolicy()
                .NoTaskQueues()
                .RequestContextFactory(new InMemoryRequestContextFactory());
            
            var commandProcessor = builder.Build();
            var command = new BetRegistrationCommand();
            commandProcessor.Send(command);

            var retrievedCommand = commandStore.Get<BetRegistrationCommand>(command.Id);

            Console.WriteLine($"Retrieved command Id: {retrievedCommand.Id}");

            Console.ReadLine();
        }

        private static IServiceProvider BuildServiceProvider(IAmACommandStore commandStore)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<BetRegistrationCommandHandler>();
            
            var commandSourcingHandler = new CommandSourcingHandler<BetRegistrationCommand>(commandStore);
            serviceCollection.Add(new ServiceDescriptor(typeof(CommandSourcingHandler<BetRegistrationCommand>), p => commandSourcingHandler, ServiceLifetime.Transient));

            return serviceCollection.BuildServiceProvider();
        }
    }
}
