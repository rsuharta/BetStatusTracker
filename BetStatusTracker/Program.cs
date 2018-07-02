using System;
using Microsoft.Extensions.DependencyInjection;
using Paramore.Brighter;
using Paramore.Brighter.CommandStore.MsSql;

namespace BetStatusTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();

            var registry = new SubscriberRegistry();
            registry.Register<BetRegistrationCommand, BetRegisteredCommandHandler>();

            var builder = CommandProcessorBuilder.With()
                .Handlers(new HandlerConfiguration(
                    subscriberRegistry: registry,
                    handlerFactory: new CommandHandlerFactory(serviceProvider)
                ))
                .DefaultPolicy()
                .NoTaskQueues()
                .RequestContextFactory(new InMemoryRequestContextFactory());

            var commandStore =
                new MsSqlCommandStore(new MsSqlCommandStoreConfiguration("", "Command"));

            var commandProcessor = builder.Build();
            commandProcessor.Send(new BetRegistrationCommand());

            Console.ReadLine();
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<BetRegisteredHandler>();
            return serviceCollection.BuildServiceProvider();
        }
    }
}
