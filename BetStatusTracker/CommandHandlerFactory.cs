using System;
using Paramore.Brighter;

namespace BetStatusTracker
{
    internal class CommandHandlerFactory : IAmAHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IHandleRequests Create(Type handlerType)
        {
            var requestHandler = (IHandleRequests)_serviceProvider.GetService(handlerType);
            return requestHandler;
        }

        public void Release(IHandleRequests handler)
        {
        }
    }
}