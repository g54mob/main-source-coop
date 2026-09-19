using System.Linq;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class FindInterestPointPositionResetSystem : MonoSystem
	{
		private MimicEnemyContext _context;

		[SerializeField]
		private float _minChangeTime;

		private bool _enabled;

		private float _cooldown;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(MimicEnemyContext context)
		{
			_context = context;
		}

		public override void Enable()
		{
			_enabled = true;
			_context.OnMimicInterestPointsChanged += OnMimicInterestPointsChanged;
		}

		public override void Disable()
		{
			_enabled = false;
			_context.OnMimicInterestPointsChanged -= OnMimicInterestPointsChanged;
			Clear();
		}

		public override void Clear()
		{
		}

		private void Update()
		{
			if (base.Initialized && _enabled)
			{
				if (_cooldown >= 0f)
				{
					_cooldown -= Time.deltaTime;
				}
				else if (_context.TargetPositionCompleted)
				{
					_context.NeedToFindTargetPosition = true;
					_cooldown = _minChangeTime;
				}
			}
		}

		private void OnMimicInterestPointsChanged()
		{
			if (_context.MimicInterestPoints.Any((MimicInterestPoint point) => point != null && point.PointPriority > _context.CurrentTargetInterestPriority))
			{
				_context.NeedToFindTargetPosition = true;
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
