using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.PlayersEyeFocusModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class EyesTargetAutoRegister : NetworkBehaviour
	{
		[SerializeField]
		private Transform _target;

		[SerializeField]
		private EyesTarget _eyesTarget;

		[SerializeField]
		private bool _registerByAuthority;

		private PlayerEyesTargetsModel _eyesTargetsModel;

		private EyesTargetsPriorityConfiguration _priorityConfiguration;

		private MultiplayerModel _multiplayerModel;

		private EyesTargetData _eyesTargetData;

		[Inject]
		private void InjectDependencies(PlayerEyesTargetsModel eyesTargetsModel, EyesTargetsPriorityConfiguration priorityConfiguration, MultiplayerModel multiplayerModel)
		{
			_eyesTargetsModel = eyesTargetsModel;
			_priorityConfiguration = priorityConfiguration;
			_multiplayerModel = multiplayerModel;
		}

		public override void Spawned()
		{
			if (!_registerByAuthority || !(base.Object.InputAuthority == _multiplayerModel.NetworkRunner.LocalPlayer))
			{
				_eyesTargetData = (_priorityConfiguration.EyesTargetsPriority.ContainsKey(_eyesTarget) ? new EyesTargetData(_priorityConfiguration.EyesTargetsPriority[_eyesTarget], _target) : new EyesTargetData(0, _target));
				_eyesTargetsModel.RegisterTarget(_eyesTargetData);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (!_registerByAuthority || !(base.Object.InputAuthority == _multiplayerModel.NetworkRunner.LocalPlayer))
			{
				_eyesTargetsModel.UnregisterTarget(_eyesTargetData);
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
