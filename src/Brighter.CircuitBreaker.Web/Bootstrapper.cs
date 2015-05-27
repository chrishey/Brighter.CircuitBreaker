using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Brighter.CircuitBreaker.Service;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Polly;

namespace Brighter.CircuitBreaker.Web
{
	public class Bootstrapper : DefaultNancyBootstrapper
	{
		protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
		{
			var logger = LogProvider.For<Processor>();
			container.Register(logger);

			var circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreaker(1, TimeSpan.FromSeconds(10));

			var subscriberRegistry = new SubscriberRegistry();
			var handlerFactory = new TinyIoCHandlerFactory(container);
			var requestContextFactory = new InMemoryRequestContextFactory();
			var policyRegistry = new PolicyRegistry {{"circuitBreaker", circuitBreakerPolicy}};
			var commandProcessor = new CommandProcessor(subscriberRegistry, handlerFactory, requestContextFactory, policyRegistry,
				logger);
		}
	}

	public class TinyIoCHandlerFactory : IAmAHandlerFactory
	{
		private readonly TinyIoCContainer _container;

		public TinyIoCHandlerFactory(TinyIoCContainer container)
		{
			_container = container;
		}

		public IHandleRequests Create(Type handlerType)
		{
			return (IHandleRequests) _container.Resolve(handlerType);
		}

		public void Release(IHandleRequests handler)
		{
			var disposableHandler = handler as IDisposable;
			if (disposableHandler != null)
				disposableHandler.Dispose();

			handler = null;
		}
	}
}