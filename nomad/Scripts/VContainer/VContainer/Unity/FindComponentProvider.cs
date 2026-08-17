using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Internal;

namespace VContainer.Unity
{
	internal sealed class FindComponentProvider : IInstanceProvider
	{
		private readonly Type componentType;

		private readonly IReadOnlyList<IInjectParameter> customParameters;

		private ComponentDestination destination;

		private Scene scene;

		public FindComponentProvider(Type componentType, IReadOnlyList<IInjectParameter> customParameters, in Scene scene, in ComponentDestination destination)
		{
			this.componentType = componentType;
			this.customParameters = customParameters;
			this.scene = scene;
			this.destination = destination;
		}

		public object SpawnInstance(IObjectResolver resolver)
		{
			Component component = null;
			Transform parent = destination.GetParent(resolver);
			if (parent != null)
			{
				component = parent.GetComponentInChildren(componentType, includeInactive: true);
				if (component == null)
				{
					throw new VContainerException(componentType, $"{componentType} is not in the parent {parent.name} : {this}");
				}
			}
			else
			{
				if (!scene.IsValid())
				{
					throw new VContainerException(componentType, $"Invalid Component find target {this}");
				}
				List<GameObject> buffer;
				using (ListPool<GameObject>.Get(out buffer))
				{
					scene.GetRootGameObjects(buffer);
					foreach (GameObject item in buffer)
					{
						component = item.GetComponentInChildren(componentType, includeInactive: true);
						if (component != null)
						{
							break;
						}
					}
				}
				if (component == null)
				{
					throw new VContainerException(componentType, $"{componentType} is not in this scene {scene.path} : {this}");
				}
			}
			if (component is MonoBehaviour monoBehaviour)
			{
				InjectorCache.GetOrBuild(monoBehaviour.GetType()).Inject(monoBehaviour, resolver, customParameters);
			}
			destination.ApplyDontDestroyOnLoadIfNeeded(component);
			return component;
		}
	}
}
