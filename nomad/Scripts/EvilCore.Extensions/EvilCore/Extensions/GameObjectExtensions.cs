using System;
using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;
using VContainer;

namespace EvilCore.Extensions
{
	public static class GameObjectExtensions
	{
		private static IObjectResolver _cachedContainer;

		private static readonly HashSet<int> _injectedThisFrame = new HashSet<int>();

		private static int _injectedFrame = -1;

		public static MeshRenderer GetMeshRenderer(this GameObject gameObject)
		{
			if (!gameObject.TryGetComponentInChildren<MeshRenderer>(out var component))
			{
				return null;
			}
			return component;
		}

		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			if (gameObject.TryGetComponent<T>(out var component))
			{
				return component;
			}
			return gameObject.AddComponent<T>();
		}

		public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component, bool includeInactive = false) where T : Component
		{
			return component = gameObject.GetComponentInChildren<T>(includeInactive);
		}

		public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component, bool includeInactive = false) where T : Component
		{
			return component = gameObject.GetComponentInParent<T>(includeInactive);
		}

		public static Collider[] GetColliders(this GameObject gameObject)
		{
			return gameObject.GetComponentsInChildren<Collider>();
		}

		public static Collider[] GetRigidColliders(this GameObject gameObject)
		{
			return Array.FindAll(gameObject.GetComponentsInChildren<Collider>(), (Collider c) => !c.isTrigger);
		}

		public static Collider[] GetTriggerColliders(this GameObject gameObject)
		{
			return Array.FindAll(gameObject.GetComponentsInChildren<Collider>(), (Collider c) => c.isTrigger);
		}

		public static void SetIgnoreCollisions(this Collider col, Collider[] targetColliders, bool ignore)
		{
			foreach (Collider collider in targetColliders)
			{
				Physics.IgnoreCollision(col, collider, ignore);
			}
		}

		public static void SetContainerReference(IObjectResolver container)
		{
			_cachedContainer = container;
		}

		public static void InjectGameObject(this GameObject gameObject)
		{
			if (_cachedContainer == null)
			{
				EvilLogger.LogError("[DI] Container not available for injection on '" + gameObject.name + "'.", "InjectGameObject", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Extensions\\GameObjectExtensions.cs", 87);
				return;
			}
			int frameCount = Time.frameCount;
			if (frameCount != _injectedFrame)
			{
				_injectedFrame = frameCount;
				_injectedThisFrame.Clear();
			}
			try
			{
				InjectGameObjectDeduped(gameObject);
			}
			catch (VContainerException ex)
			{
				EvilLogger.LogError("[DI] Injection failed on '" + gameObject.name + "': " + ex.Message, "InjectGameObject", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Extensions\\GameObjectExtensions.cs", 104);
			}
		}

		private static void InjectGameObjectDeduped(GameObject current)
		{
			if (current == null || !_injectedThisFrame.Add(current.GetInstanceID()))
			{
				return;
			}
			List<MonoBehaviour> list = new List<MonoBehaviour>();
			current.GetComponents(list);
			for (int i = 0; i < list.Count; i++)
			{
				MonoBehaviour monoBehaviour = list[i];
				if (monoBehaviour != null)
				{
					_cachedContainer.Inject(monoBehaviour);
				}
			}
			Transform transform = current.transform;
			for (int j = 0; j < transform.childCount; j++)
			{
				InjectGameObjectDeduped(transform.GetChild(j).gameObject);
			}
		}

		public static T AddComponentWithInjection<T>(this GameObject gameObject) where T : Component
		{
			T val = gameObject.AddComponent<T>();
			if (_cachedContainer != null)
			{
				_cachedContainer.Inject(val);
			}
			else
			{
				EvilLogger.LogError("Container is null, cannot inject into component!", "AddComponentWithInjection", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Extensions\\GameObjectExtensions.cs", 141);
			}
			return val;
		}
	}
}
