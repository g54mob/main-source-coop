using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.NavigationModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class CrabHomeRoamSystem : MonoSystem
	{
		[SerializeField]
		private float _roamRadius = 6f;

		private IMovementContext _movementContext;

		private INavigationService _navigationService;

		private bool _enabled;

		private bool _hasHome;

		private Vector3 _homePosition;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(IMovementContext movementContext, INavigationService navigationService)
		{
			_movementContext = movementContext;
			_navigationService = navigationService;
		}

		public override void Enable()
		{
			_enabled = true;
			if (!_hasHome)
			{
				_homePosition = base.transform.position;
				_hasHome = true;
			}
		}

		public void SetHome(Vector3 homePosition, float roamRadius)
		{
			_homePosition = homePosition;
			_roamRadius = roamRadius;
			_hasHome = true;
		}

		public override void Disable()
		{
			_enabled = false;
			Clear();
		}

		public override void Clear()
		{
			_movementContext.NeedToFindTargetPosition = false;
		}

		private void Update()
		{
			if (base.Initialized && _enabled && _movementContext.NeedToFindTargetPosition && _movementContext.NavMeshAgent.isActiveAndEnabled && _movementContext.NavMeshAgent.isOnNavMesh && _navigationService.TryGetRandomNavmeshPosition(_homePosition, _roamRadius, out var position))
			{
				NavMeshPath navMeshPath = new NavMeshPath();
				if (_movementContext.NavMeshAgent.CalculatePath(position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
				{
					_movementContext.SetTargetPosition(position);
					_movementContext.NeedToFindTargetPosition = false;
					_movementContext.SetTargetPositionCompleted(isCompleted: false);
				}
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
