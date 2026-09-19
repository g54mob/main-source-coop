using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.NavigationModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Navigation
{
	[NetworkBehaviourWeaved(0)]
	public class FindRandomSafePositionSystem : MonoSystem
	{
		private IMovementContext _movementContext;

		[SerializeField]
		private float _searchRadius;

		[SerializeField]
		private int _attempts;

		private INavigationService _navigationService;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		public void InjectDependencies(IMovementContext movementContext, INavigationService navigationService)
		{
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
			if (base.Initialized && _enabled && _movementContext.NeedToFindTargetPosition && _navigationService.TryGetRandomSafeNavmeshPosition(base.transform.position, _searchRadius, _attempts, out var position))
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
