using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.BarBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class BarBottleSpawner : NetworkBehaviour
	{
		private MultiplayerModel _multiplayerModel;

		[Inject]
		private void InjectDependencies(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public async UniTask<BarBottleServeDriver> SpawnBottleAsync(NetworkObject prefab, Vector3 spawnPosition, Quaternion spawnRotation)
		{
			if (prefab == null)
			{
				return null;
			}
			if (!TryGetHostRunner(out var runner))
			{
				return null;
			}
			NetworkObject networkObject = await runner.SpawnAsync(prefab, spawnPosition, spawnRotation, runner.LocalPlayer);
			if (networkObject == null)
			{
				return null;
			}
			if (!networkObject.TryGetComponent<BarBottleServeDriver>(out var component))
			{
				Debug.LogError("[BarBottleSpawner] Spawned '" + prefab.name + "' without BarBottleServeDriver. Despawning.");
				runner.Despawn(networkObject);
				return null;
			}
			return component;
		}

		private bool TryGetHostRunner(out NetworkRunner runner)
		{
			runner = _multiplayerModel?.NetworkRunner;
			if (runner == null || !runner.IsRunning || !runner.IsSharedModeMasterClient)
			{
				return false;
			}
			return true;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
