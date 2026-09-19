using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Setup;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class SleeperMoveSpeedSetupSystem : PerStateValueSetupSystemBase<SleeperStateId>
	{
		[SerializeField]
		private SerializableDictionary<SleeperStateId, float> _moveSpeeds;

		[SerializeField]
		private float _defaultValue = 3f;

		private SleeperEnemyContext _context;

		protected override float DefaultValue => _defaultValue;

		[Inject]
		private void InjectDependencies(SleeperEnemyContext context, ICurrentStateProvider<SleeperStateId> stateProvider)
		{
			_context = context;
			SetStateProvider(stateProvider);
		}

		protected override bool TryGetValue(SleeperStateId stateId, out float value)
		{
			if (_moveSpeeds.ContainsKey(stateId))
			{
				value = _moveSpeeds[stateId];
				return true;
			}
			value = 0f;
			return false;
		}

		protected override void Apply(float value)
		{
			_context.SetMoveSpeed(value);
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
