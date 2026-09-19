#define FUSION_LOGLEVEL_TRACE
using System;
using System.Reflection;
using UnityEngine;

namespace Fusion
{
	public class NetworkObjectProviderDefault : Behaviour, INetworkObjectProvider
	{
		[InlineHelp]
		public bool DelayIfSceneManagerIsBusy = true;

		private bool? _isAcquirePrefabInstanceOverriden;

		public virtual NetworkObjectAcquireResult AcquireInstance(NetworkRunner runner, in NetworkObjectAcquireContext context, out NetworkObject instance)
		{
			instance = null;
			if (DelayIfSceneManagerIsBusy && runner.SceneManager.IsBusy)
			{
				return NetworkObjectAcquireResult.Retry;
			}
			if (context.TypeId.IsSceneObject)
			{
				if ((bool)context.AttachableInstance)
				{
					instance = context.AttachableInstance;
					return NetworkObjectAcquireResult.Success;
				}
				return NetworkObjectAcquireResult.Retry;
			}
			if (!context.TypeId.IsPrefab)
			{
				return NetworkObjectAcquireResult.Failed;
			}
			if (IsAcquirePrefabInstanceOverriden())
			{
				return AcquirePrefabInstance(runner, new NetworkPrefabAcquireContext(in context), out instance);
			}
			try
			{
				instance = CreatePrefabInstance(runner, context.TypeId.AsPrefabId, context.IsSynchronous, context.DontDestroyOnLoad);
			}
			catch (Exception arg)
			{
				Log.Error($"Failed to load prefab: {arg}");
				return NetworkObjectAcquireResult.Failed;
			}
			if (!instance)
			{
				return NetworkObjectAcquireResult.Retry;
			}
			return NetworkObjectAcquireResult.Success;
		}

		protected NetworkObject CreatePrefabInstance(NetworkRunner runner, NetworkPrefabId prefabId, bool synchronous, bool dontDestroyOnLoad)
		{
			NetworkPrefabTable prefabs = runner.Prefabs;
			NetworkObject networkObject = prefabs.Load(prefabId, synchronous);
			if (!networkObject)
			{
				return null;
			}
			NetworkObject networkObject2 = InstantiatePrefab(runner, networkObject);
			if (dontDestroyOnLoad)
			{
				runner.MakeDontDestroyOnLoad(networkObject2.gameObject);
			}
			else
			{
				runner.MoveToRunnerScene(networkObject2.gameObject);
			}
			prefabs.AddInstance(prefabId);
			return networkObject2;
		}

		public virtual void ReleaseInstance(NetworkRunner runner, in NetworkObjectReleaseContext context)
		{
			NetworkObject instance = context.Object;
			if (!context.IsBeingDestroyed)
			{
				switch (context.TypeId.Kind)
				{
				case NetworkTypeIdKind.Prefab:
					DestroyPrefabInstance(runner, context.TypeId.AsPrefabId, instance);
					break;
				case NetworkTypeIdKind.PrefabNested:
					var (prefabId, index) = context.TypeId.AsNestedPrefabId;
					DestroyPrefabNestedObject(runner, prefabId, index, instance);
					break;
				case NetworkTypeIdKind.SceneObject:
					DestroySceneObject(runner, context.TypeId.AsSceneObjectId, instance);
					break;
				default:
					throw new NotImplementedException($"Unknown type id {context.TypeId}");
				}
			}
			if (context.TypeId.IsPrefab)
			{
				runner.Prefabs.RemoveInstance(context.TypeId.AsPrefabId);
			}
		}

		void INetworkObjectProvider.Shutdown(NetworkRunner runner)
		{
			NetworkPrefabTable prefabs = runner.Prefabs;
			if (prefabs != null && prefabs.Options.UnloadUnusedPrefabsOnShutdown)
			{
				prefabs.UnloadUnreferenced(includeIncompleteLoads: true);
			}
		}

		public NetworkPrefabId GetPrefabId(NetworkRunner runner, NetworkObjectGuid prefabGuid)
		{
			return runner.Prefabs.GetId(prefabGuid);
		}

		public NetworkPrefabId GetPrefabId(NetworkRunner runner, string prefabName)
		{
			return runner.Prefabs.GetId(prefabName);
		}

		protected virtual NetworkObject InstantiatePrefab(NetworkRunner runner, NetworkObject prefab)
		{
			return UnityEngine.Object.Instantiate(prefab);
		}

		protected virtual void DestroyPrefabInstance(NetworkRunner runner, NetworkPrefabId prefabId, NetworkObject instance)
		{
			UnityEngine.Object.Destroy(instance.gameObject);
		}

		protected virtual void DestroyPrefabNestedObject(NetworkRunner runner, NetworkPrefabId prefabId, int index, NetworkObject instance)
		{
			DestroyPrefabNestedObject(runner, instance);
		}

		[Obsolete("Use the overload with prefabId and index instead")]
		protected virtual void DestroyPrefabNestedObject(NetworkRunner runner, NetworkObject instance)
		{
			UnityEngine.Object.Destroy(instance.gameObject);
		}

		protected virtual void DestroySceneObject(NetworkRunner runner, NetworkSceneObjectId sceneObjectId, NetworkObject instance)
		{
			UnityEngine.Object.Destroy(instance.gameObject);
		}

		[Obsolete("Override AcquireInstance instead. It deals with scene objects as well, due to context.AttachableInstance field.")]
		public virtual NetworkObjectAcquireResult AcquirePrefabInstance(NetworkRunner runner, in NetworkPrefabAcquireContext context, out NetworkObject instance)
		{
			if (DelayIfSceneManagerIsBusy && runner.SceneManager.IsBusy)
			{
				instance = null;
				return NetworkObjectAcquireResult.Retry;
			}
			try
			{
				instance = CreatePrefabInstance(runner, context.PrefabId, context.IsSynchronous, context.DontDestroyOnLoad);
			}
			catch (Exception arg)
			{
				Log.Error($"Failed to load prefab: {arg}");
				instance = null;
				return NetworkObjectAcquireResult.Failed;
			}
			if (!instance)
			{
				return NetworkObjectAcquireResult.Retry;
			}
			return NetworkObjectAcquireResult.Success;
		}

		private bool IsAcquirePrefabInstanceOverriden()
		{
			if (_isAcquirePrefabInstanceOverriden.HasValue)
			{
				return _isAcquirePrefabInstanceOverriden.Value;
			}
			MethodInfo method = GetType().GetMethod("AcquirePrefabInstance", BindingFlags.Instance | BindingFlags.Public);
			_isAcquirePrefabInstanceOverriden = method.DeclaringType != typeof(NetworkObjectProviderDefault);
			return _isAcquirePrefabInstanceOverriden.Value;
		}
	}
}
