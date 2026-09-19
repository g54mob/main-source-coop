using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class AnchorMeleeDecisionSystem : MonoSystem
	{
		private AnchorEnemyContext _context;

		private bool _isEnabled;

		public bool CanMelee { get; private set; }

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(AnchorEnemyContext context)
		{
			_context = context;
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

		public override void Clear()
		{
			CanMelee = false;
		}

		private void Update()
		{
			if (!base.Initialized || !_isEnabled || !base.HasStateAuthority)
			{
				CanMelee = false;
				return;
			}
			if (_context.MeleeCooldown > 0f)
			{
				_context.MeleeCooldown -= Time.deltaTime;
			}
			CanMelee = Evaluate();
		}

		private bool Evaluate()
		{
			if (_context.MeleeCooldown > 0f)
			{
				return false;
			}
			if (!_context.TryGetAttackTargetPosition(out var position))
			{
				return false;
			}
			return Vector3.Distance(_context.transform.position, position) <= _context.MeleeRange;
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
