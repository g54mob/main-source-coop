using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.NavigationModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class AnchorAreaRelocateSystem : MonoSystem
	{
		private AnchorEnemyContext _context;

		private IMovementContext _movementContext;

		private INavigationService _navigationService;

		private bool _enabled;

		private float _timer;

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
			_timer = 0f;
			if (!_context.HasAreaPosition)
			{
				_context.SetAreaPosition(base.transform.position);
			}
		}

		public override void Disable()
		{
			_enabled = false;
			Clear();
		}

		public override void Clear()
		{
			_timer = 0f;
		}

		private void Update()
		{
			if (base.Initialized && _enabled && _context.HasAreaPosition)
			{
				_timer += Time.deltaTime;
				if (!(_timer < _context.ChangeAreaFrequency))
				{
					_timer = 0f;
					RelocateArea();
				}
			}
		}

		private void RelocateArea()
		{
			if (_navigationService.TryGetRandomNavmeshPosition(_context.AreaPosition, _context.AreaRelocateRadius, out var position))
			{
				_context.SetAreaPosition(position);
				_movementContext.NeedToFindTargetPosition = true;
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
