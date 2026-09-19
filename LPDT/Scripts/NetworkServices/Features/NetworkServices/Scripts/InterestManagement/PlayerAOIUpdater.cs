using Features.CameraModelModule;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.NetworkServices.Scripts.InterestManagement
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerAOIUpdater : NetworkBehaviour
	{
		[SerializeField]
		private float _extends = 32f;

		[SerializeField]
		private AOILayer _activeAOILayer;

		private ActiveAOILayerModel _activeAOILayerModel;

		private CameraModel _cameraModel;

		[Inject]
		public void InjectDependencies(ActiveAOILayerModel activeAOILayerModel, CameraModel cameraModel)
		{
			_activeAOILayerModel = activeAOILayerModel;
			_cameraModel = cameraModel;
		}

		public override void Spawned()
		{
			_activeAOILayerModel.OnActiveAOILayerUpdated += ClearPlayerAOI;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_activeAOILayerModel.OnActiveAOILayerUpdated -= ClearPlayerAOI;
		}

		public override void FixedUpdateNetwork()
		{
			if (IsValid())
			{
				base.Runner.AddPlayerAreaOfInterest(base.Object.InputAuthority, _cameraModel.CameraObject.transform.position, _extends);
			}
		}

		private void ClearPlayerAOI(AOILayer activeAOILayer)
		{
			if (activeAOILayer != _activeAOILayerModel.ActiveAOILayer && base.HasStateAuthority && !base.Object.InputAuthority.IsNone)
			{
				base.Runner.ClearPlayerAreaOfInterest(base.Object.InputAuthority);
			}
		}

		private new bool IsValid()
		{
			if (base.HasStateAuthority && _activeAOILayer == _activeAOILayerModel.ActiveAOILayer)
			{
				return !base.Object.InputAuthority.IsNone;
			}
			return false;
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
