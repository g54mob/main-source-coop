using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Setup;
using Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.States;
using Fusion;
using Global.SerializableDictionary;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class AnchorMoveSpeedSetupSystem : PerStateValueSetupSystemBase<AnchorStateId>
	{
		[SerializeField]
		private Global.SerializableDictionary.SerializableDictionary<AnchorStateId, float> _moveSpeeds;

		[SerializeField]
		private float _defaultValue = 4f;

		private AnchorEnemyContext _context;

		protected override float DefaultValue => _defaultValue;

		[Inject]
		private void InjectDependencies(AnchorEnemyContext context, ICurrentStateProvider<AnchorStateId> stateProvider)
		{
			_context = context;
			SetStateProvider(stateProvider);
		}

		protected override bool TryGetValue(AnchorStateId stateId, out float value)
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
			if (!(_context.NavMeshAgent == null))
			{
				_context.NavMeshAgent.speed = value;
				if (_context.NavMeshAgent.isActiveAndEnabled && _context.NavMeshAgent.isOnNavMesh)
				{
					_context.NavMeshAgent.isStopped = value <= 0.01f;
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
