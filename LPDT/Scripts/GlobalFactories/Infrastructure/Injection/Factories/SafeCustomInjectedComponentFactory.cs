using System;
using UnityEngine;
using Zenject;

namespace Infrastructure.Injection.Factories
{
	internal sealed class SafeCustomInjectedComponentFactory : IFactory<Type, GameObject, Component>, IFactory
	{
		private readonly InjectedComponentFactory _injectedComponentFactory;

		public SafeCustomInjectedComponentFactory(InjectedComponentFactory injectedComponentFactory)
		{
			_injectedComponentFactory = injectedComponentFactory;
		}

		public Component Create(Type componentType, GameObject gameObject)
		{
			Component component = gameObject.GetComponent(componentType);
			if (!(component == null))
			{
				return component;
			}
			return _injectedComponentFactory.Create(componentType, gameObject);
		}
	}
}
