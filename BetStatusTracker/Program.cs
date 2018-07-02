using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            commandProcessor.Send(new BetRegistrationCommand());

            Console.ReadLine();
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<BetRegistrationCommandHandler>();

            var commandStore =
                new MsSqlCommandStore(new MsSqlCommandStoreConfiguration("", "Command"));

            serviceCollection.Add(new ServiceDescriptor(typeof(IAmACommandStore), p => commandStore, ServiceLifetime.Singleton));
            serviceCollection.Add(new ServiceDescriptor(typeof(IAmACommandStoreAsync), p => commandStore, ServiceLifetime.Singleton));
            
            return serviceCollection.BuildServiceProvider();
        }
    }
}
