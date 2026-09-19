#define FUSION_LOGLEVEL_TRACE
using System;
using Fusion;
using Zenject;

namespace NetworkServices.ObjectsProvider
{
	public class InjectedNetworkObjectProvider : NetworkObjectProviderDefault, IForbiddableObjectProvider
	{
		private bool _isAcquireInstanceAllowed = true;

		public bool IsAcquireInstanceAllowed => _isAcquireInstanceAllowed;

		public override NetworkObjectAcquireResult AcquirePrefabInstance(NetworkRunner runner, in NetworkPrefabAcquireContext context, out NetworkObject instance)
		{
			instance = null;
			if (!_isAcquireInstanceAllowed)
			{
				return NetworkObjectAcquireResult.Ignore;
			}
			if (DelayIfSceneManagerIsBusy && runner.SceneManager.IsBusy)
			{
				return NetworkObjectAcquireResult.Retry;
			}
			NetworkObject networkObject;
			try
			{
				networkObject = runner.Prefabs.Load(context.PrefabId, context.IsSynchronous);
			}
			catch (Exception arg)
			{
				Log.Error($"Failed to load prefab: {arg}");
				return NetworkObjectAcquireResult.Failed;
			}
			if (networkObject == null)
			{
				return NetworkObjectAcquireResult.Retry;
			}
			if (networkObject.TryGetComponent<NetworkObjectSpawnData>(out var component) && component.Injectable)
			{
				DiContainer diContainer = NetworkObjectInjectionContainer.Resolve();
				bool activeSelf = networkObject.gameObject.activeSelf;
				networkObject.gameObject.SetActive(value: false);
				try
				{
					instance = InstantiatePrefab(runner, networkObject);
					diContainer.InjectGameObject(instance.gameObject);
					instance.gameObject.SetActive(activeSelf);
				}
				catch (Exception arg2)
				{
					networkObject.gameObject.SetActive(activeSelf);
					Log.Error($"Failed to load prefab: {arg2}");
					return NetworkObjectAcquireResult.Failed;
				}
				networkObject.gameObject.SetActive(activeSelf);
			}
			else
			{
				instance = InstantiatePrefab(runner, networkObject);
			}
			if (context.DontDestroyOnLoad)
			{
				runner.MakeDontDestroyOnLoad(instance.gameObject);
			}
			else
			{
				runner.MoveToRunnerScene(instance.gameObject);
			}
			runner.Prefabs.AddInstance(context.PrefabId);
			return NetworkObjectAcquireResult.Success;
		}

		public void ForbidAcquireInstance()
		{
			_isAcquireInstanceAllowed = false;
		}

		public void AllowAcquireInstance()
		{
			_isAcquireInstanceAllowed = true;
		}
	}
}
