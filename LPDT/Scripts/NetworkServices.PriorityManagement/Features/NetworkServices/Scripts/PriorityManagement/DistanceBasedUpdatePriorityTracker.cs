using Features.CameraModelModule;
using Features.LevelGatesModule.Data;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.NetworkServices.Scripts.PriorityManagement
{
	[NetworkBehaviourWeaved(0)]
	public class DistanceBasedUpdatePriorityTracker : UpdatePriorityTracker
	{
		[SerializeField]
		private DistancePriorityConfiguration _distancePriorityConfiguration;

		private CameraModel _cameraModel;

		private MultiplayerModel _multiplayerModel;

		private PlayersGatesModelSynchronizedModel _playersGatesModelSynchronizedModel;

		[Inject]
		public void InjectDependencies(CameraModel cameraModel, MultiplayerModel multiplayerModel, PlayersGatesModelSynchronizedModel playersGatesModelSynchronizedModel)
		{
			_cameraModel = cameraModel;
			_multiplayerModel = multiplayerModel;
			_playersGatesModelSynchronizedModel = playersGatesModelSynchronizedModel;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
