using System;
using System.Collections.Generic;
using UnityEngine;

namespace VContainer.Unity
{
	internal sealed class NewGameObjectProvider : IInstanceProvider
	{
		private readonly Type componentType;

		private readonly IInjector injector;

		private readonly IReadOnlyList<IInjectParameter> customParameters;

		private readonly string newGameObjectName;

		private ComponentDestination destination;

		public NewGameObjectProvider(Type componentType, IInjector injector, IReadOnlyList<IInjectParameter> customParameters, in ComponentDestination destination, string newGameObjectName = null)
		{
			this.componentType = componentType;
			this.customParameters = customParameters;
			this.injector = injector;
			this.destination = destination;
			this.newGameObjectName = newGameObjectName;
		}

		public object SpawnInstance(IObjectResolver resolver)
		{
			GameObject gameObject = new GameObject(string.IsNullOrEmpty(newGameObjectName) ? componentType.Name : newGameObjectName);
			gameObject.SetActive(value: false);
			Transform parent = destination.GetParent(resolver);
			if (parent != null)
			{
				gameObject.transform.SetParent(parent);
			}
			Component component = gameObject.AddComponent(componentType);
			injector.Inject(component, resolver, customParameters);
			destination.ApplyDontDestroyOnLoadIfNeeded(component);
			component.gameObject.SetActive(value: true);
			return component;
		}
	}
}
