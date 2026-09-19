using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.NavigationModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Navigation
{
	[NetworkBehaviourWeaved(0)]
	public class FindTargetPlayerPositionSystem : MonoSystem
	{
		private IMovementContext _movementContext;

		private IDetectionContext _detectionContext;

		private PlayerRaycastPointsModel _playerRaycastPointsModel;

		private INavigationService _navigationService;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(IMovementContext movementContext, IDetectionContext detectionContext, PlayerRaycastPointsModel playerRaycastPointsModel, INavigationService navigationService)
		{
			_movementContext = movementContext;
			_detectionContext = detectionContext;
			_playerRaycastPointsModel = playerRaycastPointsModel;
			_navigationService = navigationService;
		}

		public override void Enable()
		{
			_enabled = true;
			ProcessPriorityPlayerChanged();
		}

		public override void Disable()
		{
			_enabled = false;
			Clear();
		}

		public override void Clear()
		{
		}

		private void ProcessPriorityPlayerChanged()
		{
			if (base.Initialized && _enabled && _detectionContext.PriorityPlayer != null)
			{
				PlayerRef inputAuthority = _detectionContext.PriorityPlayer.NetworkObject.InputAuthority;
				Transform point;
				if (_navigationService.TryGetPlayerTrackingPosition(inputAuthority, out var position))
				{
					_movementContext.SetTargetPosition(position);
					_movementContext.SetTargetPositionCompleted(isCompleted: false);
				}
				else if (_playerRaycastPointsModel.TryGetRaycastPoint(inputAuthority, PlayerRaycastPoint.MiddleFoot, out point))
				{
					_movementContext.SetTargetPosition(point.position);
					_movementContext.SetTargetPositionCompleted(isCompleted: false);
				}
			}
		}

		private void Update()
		{
			if (base.Initialized && _enabled)
			{
				ProcessPriorityPlayerChanged();
			}
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
