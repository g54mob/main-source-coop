using System;
using UnityEngine;

namespace VContainer.Unity
{
	public readonly struct EntryPointsBuilder
	{
		private readonly IContainerBuilder containerBuilder;

		private readonly Lifetime lifetime;

		public static void EnsureDispatcherRegistered(IContainerBuilder containerBuilder)
		{
			if (!containerBuilder.Exists(typeof(EntryPointDispatcher)))
			{
				containerBuilder.Register<EntryPointDispatcher>(Lifetime.Scoped);
				containerBuilder.RegisterEntryPointExceptionHandler(Debug.LogException);
				containerBuilder.RegisterBuildCallback(delegate(IObjectResolver container)
				{
					container.Resolve<EntryPointDispatcher>().Dispatch();
				});
			}
		}

		public EntryPointsBuilder(IContainerBuilder containerBuilder, Lifetime lifetime)
		{
			this.containerBuilder = containerBuilder;
			this.lifetime = lifetime;
		}

		public RegistrationBuilder Add<T>()
		{
			return containerBuilder.Register<T>(lifetime).AsImplementedInterfaces();
		}

		public void OnException(Action<Exception> exceptionHandler)
		{
			containerBuilder.RegisterEntryPointExceptionHandler(exceptionHandler);
		}
	}
}
