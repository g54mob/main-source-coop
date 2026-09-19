using Features.GrabModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using Zenject;

namespace Features.PlayersEyeFocusModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerEyesGrabableTargetRegister : NetworkBehaviour
	{
		private SimplePointGrabable _simplePointGrabable;

		private MultiplayerModel _multiplayerModel;

		private PlayerEyesTargetsModel _eyesTargetsModel;

		private EyesTargetsPriorityConfiguration _priorityConfiguration;

		private EyesTargetData _lastEyesTargetData;

		[Inject]
		private void InjectDependencies(MultiplayerModel multiplayerModel, PlayerEyesTargetsModel eyesTargetsModel, EyesTargetsPriorityConfiguration priorityConfiguration)
		{
			_multiplayerModel = multiplayerModel;
			_eyesTargetsModel = eyesTargetsModel;
			_priorityConfiguration = priorityConfiguration;
		}

		public override void Spawned()
		{
			_simplePointGrabable = GetComponent<SimplePointGrabable>();
			_simplePointGrabable.OnGrabbedPlayersChanged += ProcessGrabbedPlayersChanged;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_simplePointGrabable.OnGrabbedPlayersChanged -= ProcessGrabbedPlayersChanged;
		}

		private void ProcessGrabbedPlayersChanged()
		{
			if (_lastEyesTargetData != null)
			{
				_eyesTargetsModel.UnregisterTarget(_lastEyesTargetData);
				_lastEyesTargetData = null;
			}
			if (_simplePointGrabable.GrabbedByPlayers.Count > 0)
			{
				_lastEyesTargetData = (_simplePointGrabable.GrabbedByPlayers.Contains(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId) ? new EyesTargetData(_priorityConfiguration.EyesTargetsPriority[EyesTarget.ItemTakeByPlayer], base.gameObject.transform) : new EyesTargetData(_priorityConfiguration.EyesTargetsPriority[EyesTarget.ItemTakeByOther], base.gameObject.transform));
				_eyesTargetsModel.RegisterTarget(_lastEyesTargetData);
			}
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
