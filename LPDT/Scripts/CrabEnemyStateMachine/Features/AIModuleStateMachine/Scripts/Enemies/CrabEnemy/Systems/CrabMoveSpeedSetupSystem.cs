using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Setup;
using Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.States;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class CrabMoveSpeedSetupSystem : PerStateValueSetupSystemBase<CrabStateId>
	{
		[SerializeField]
		private SerializableDictionary<CrabStateId, float> _moveSpeeds;

		[SerializeField]
		private float _defaultValue = 4f;

		private CrabEnemyContext _context;

		protected override float DefaultValue => _defaultValue;

		[Inject]
		private void InjectDependencies(CrabEnemyContext crabEnemyContext, ICurrentStateProvider<CrabStateId> stateProvider)
		{
			_context = crabEnemyContext;
			SetStateProvider(stateProvider);
		}

		protected override bool TryGetValue(CrabStateId stateId, out float value)
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
			_context.SetTargetMoveSpeed(value);
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
