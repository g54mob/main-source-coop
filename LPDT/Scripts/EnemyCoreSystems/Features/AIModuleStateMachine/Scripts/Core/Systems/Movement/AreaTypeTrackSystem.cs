using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.NavigationModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Movement
{
	[NetworkBehaviourWeaved(0)]
	public class AreaTypeTrackSystem : MonoSystem
	{
		private IMovementContext _context;

		[SerializeField]
		private Transform _footRayCast;

		private INavigationService _navigationService;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(IMovementContext context, INavigationService navigationService)
		{
			_context = context;
			_navigationService = navigationService;
		}

		public override void Enable()
		{
			_isEnabled = true;
		}

		public override void Disable()
		{
			_isEnabled = false;
			Clear();
		}

		private void Update()
		{
			if (!base.Initialized || !_isEnabled)
			{
				return;
			}
			if (_navigationService.IsPointOnNavMeshProjected(_footRayCast.position, _context.NavMeshAgent, out var hit))
			{
				if (_context.CurrentAreaType != hit.mask)
				{
					_context.SetCurrentAreaType(hit.mask);
				}
			}
			else
			{
				int areaFromName = NavMesh.GetAreaFromName("Walkable");
				_context.SetCurrentAreaType(areaFromName);
			}
		}

		private void OnDrawGizmos()
		{
			if (_context != null)
			{
				Gizmos.DrawSphere(_context.TargetPosition, 0.5f);
			}
		}

		public override void Clear()
		{
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
