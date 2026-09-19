using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.NavigationModule.Scripts;
using Fusion;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class AnchorAreaRoamSystem : MonoSystem
	{
		private AnchorEnemyContext _context;

		private IMovementContext _movementContext;

		private INavigationService _navigationService;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(AnchorEnemyContext context, IMovementContext movementContext, INavigationService navigationService)
		{
			_context = context;
			_movementContext = movementContext;
			_navigationService = navigationService;
		}

		public override void Enable()
		{
			_enabled = true;
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
			if (base.Initialized && _enabled && _movementContext.NeedToFindTargetPosition && _context.HasAreaPosition && _movementContext.NavMeshAgent.isActiveAndEnabled && _movementContext.NavMeshAgent.isOnNavMesh && _navigationService.TryGetRandomNavmeshPosition(_context.AreaPosition, _context.AreaRoamRadius, out var position))
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
