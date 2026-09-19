using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Setup;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class CompleteDistanceSetupSystem : PerStateValueSetupSystemBase<MimicStateId>
	{
		[SerializeField]
		private SerializableDictionary<MimicStateId, float> _completeDistances;

		[SerializeField]
		private float _defaultValue;

		private IMovementContext _context;

		protected override float DefaultValue => _defaultValue;

		[Inject]
		private void InjectDependencies(IMovementContext context, ICurrentStateProvider<MimicStateId> stateProvider)
		{
			_context = context;
			SetStateProvider(stateProvider);
		}

		protected override bool TryGetValue(MimicStateId stateId, out float value)
		{
			if (_completeDistances.ContainsKey(stateId))
			{
				value = _completeDistances[stateId];
				return true;
			}
			value = 0f;
			return false;
		}

		protected override void Apply(float value)
		{
			_context.CompletePointMinDistance = value;
		}

		public override void Clear()
		{
			_context.CompletePointMinDistance = _defaultValue;
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
