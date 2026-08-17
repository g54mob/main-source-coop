using System;
using System.Collections.Generic;
using UnityEngine;

namespace VContainer.Unity
{
	internal sealed class PrefabComponentProvider : IInstanceProvider
	{
		private readonly IInjector injector;

		private readonly IReadOnlyList<IInjectParameter> customParameters;

		private readonly Func<IObjectResolver, Component> prefabFinder;

		private ComponentDestination destination;

		public PrefabComponentProvider(Func<IObjectResolver, Component> prefabFinder, IInjector injector, IReadOnlyList<IInjectParameter> customParameters, in ComponentDestination destination)
		{
			this.injector = injector;
			this.customParameters = customParameters;
			this.prefabFinder = prefabFinder;
			this.destination = destination;
		}

		public object SpawnInstance(IObjectResolver resolver)
		{
			Component component = prefabFinder(resolver);
			Transform parent = destination.GetParent(resolver);
			bool activeSelf = component.gameObject.activeSelf;
			using (new ObjectResolverUnityExtensions.PrefabDirtyScope(component.gameObject))
			{
				if (activeSelf)
				{
					component.gameObject.SetActive(value: false);
				}
				Component component2 = ((parent != null) ? UnityEngine.Object.Instantiate(component, parent) : UnityEngine.Object.Instantiate(component));
				if (VContainerSettings.Instance != null && VContainerSettings.Instance.RemoveClonePostfix)
				{
					component2.name = component.name;
				}
				try
				{
					injector.Inject(component2, resolver, customParameters);
					destination.ApplyDontDestroyOnLoadIfNeeded(component2);
				}
				finally
				{
					if (activeSelf)
					{
						component.gameObject.SetActive(value: true);
						component2.gameObject.SetActive(value: true);
					}
				}
				return component2;
			}
		}
	}
}
