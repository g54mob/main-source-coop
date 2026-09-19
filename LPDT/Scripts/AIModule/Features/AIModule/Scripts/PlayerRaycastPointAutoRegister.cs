using Features.NavigationModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerRaycastPointAutoRegister : NetworkBehaviour
	{
		[SerializeField]
		private PlayerRaycastPoint _raycastPointType;

		private PlayerRaycastPointsModel _playerRaycastPointsModel;

		[Inject]
		public void InjectDependencies(PlayerRaycastPointsModel playerRaycastPointsModel)
		{
			_playerRaycastPointsModel = playerRaycastPointsModel;
		}

		public override void Spawned()
		{
			_playerRaycastPointsModel.RegisterRaycastPoint(base.Object.InputAuthority, _raycastPointType, base.transform);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_playerRaycastPointsModel.UnregisterRaycastPoint(base.Object.InputAuthority, _raycastPointType);
			base.Despawned(runner, hasState);
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
