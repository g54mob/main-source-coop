using System;
using UnityEngine;
using Zenject;

namespace Infrastructure.Injection.Factories
{
	internal sealed class CustomInjectedComponentFactory : IFactory<Type, GameObject, Component>, IFactory
	{
		private readonly DiContainer _diContainer;

		public CustomInjectedComponentFactory(DiContainer diContainer)
		{
			_diContainer = diContainer;
		}

		public Component Create(Type componentType, GameObject gameObject)
		{
			if (!typeof(MonoBehaviour).IsAssignableFrom(componentType))
			{
				throw new ArgumentException("Type argument componentType must derive from MonoBehaviour.");
			}
			return _diContainer.InstantiateComponent(componentType, gameObject);
		}
	}
}
