using Cysharp.Threading.Tasks;
using Features.GrabModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.StrechArmsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class ArmSpawner : NetworkBehaviour
	{
		[SerializeField]
		private Arm _armOrientation;

		[SerializeField]
		private GameObject _armPrefab;

		[SerializeField]
		private GameObject _armEndPrefab;

		[SerializeField]
		private NetworkObject _lookPoint;

		[SerializeField]
		private Transform _idlePoseTransform;

		[SerializeField]
		private Transform _idlePoseStoreTransform;

		[SerializeField]
		private Transform _startPoseTransform;

		[SerializeField]
		private LineArmController _lineArmController;

		private MultiplayerModel _multiplayerModel;

		private ArmStartsModel _armStartsModel;

		private PlayersArmsModel _playersArmsModel;

		private NetworkObject _spawnedArmEnd;

		private NetworkObject _spawnedArm;

		private PlayerRef _ownerRef;

		[Inject]
		private void InjectDependencies(MultiplayerModel multiplayerModel, ArmStartsModel armStartsModel, PlayersArmsModel playersArmsModel)
		{
			_multiplayerModel = multiplayerModel;
			_armStartsModel = armStartsModel;
			_playersArmsModel = playersArmsModel;
		}

		public override async void Spawned()
		{
			_ownerRef = base.Object.StateAuthority;
			_armStartsModel.AddArmStart(_ownerRef, _startPoseTransform, _armOrientation);
			_armStartsModel.AddIdlePose(_ownerRef, _idlePoseTransform, _armOrientation);
			_armStartsModel.AddIdleStorePose(_ownerRef, _idlePoseStoreTransform, _armOrientation);
			if (base.Object.HasStateAuthority)
			{
				await SpawnArm();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_armStartsModel.RemoveForPlayer(_ownerRef, _armOrientation);
			DespawnIfValid(runner, ref _spawnedArmEnd);
			DespawnIfValid(runner, ref _spawnedArm);
		}

		private async UniTask SpawnArm()
		{
			_spawnedArmEnd = await _multiplayerModel.NetworkRunner.SpawnAsync(_armEndPrefab, base.transform.position, Quaternion.identity, _multiplayerModel.NetworkRunner.LocalPlayer, InitArmEndBeforeSpawn);
			_spawnedArm = await _multiplayerModel.NetworkRunner.SpawnAsync(_armPrefab, base.transform.position, Quaternion.identity, _multiplayerModel.NetworkRunner.LocalPlayer, InitArmVisualBeforeSpawn);
			_playersArmsModel.AddPlayerArm(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, _armOrientation);
		}

		private static void DespawnIfValid(NetworkRunner runner, ref NetworkObject networkObject)
		{
			if (networkObject != null && networkObject.IsValid && runner != null && runner.IsRunning)
			{
				runner.Despawn(networkObject);
			}
			networkObject = null;
		}

		private void InitArmEndBeforeSpawn(NetworkRunner runner, NetworkObject obj)
		{
			ArmEndController component = obj.GetComponent<ArmEndController>();
			component.SetLookPoint(_lookPoint);
			component.SetArmOrientation(_armOrientation);
		}

		private void InitArmVisualBeforeSpawn(NetworkRunner runner, NetworkObject obj)
		{
			ArmVisualsController component = obj.GetComponent<ArmVisualsController>();
			component.SetArmOrientation(_armOrientation);
			component.SetLineArmController(LineArmType.RightArmDefault);
			component.SetArmEndGrabbable(_spawnedArmEnd.GetComponentInChildren<IPointGrabable>());
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
