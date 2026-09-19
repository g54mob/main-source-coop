using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Setup;
using Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.States;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class CrabSearchRangeSetupSystem : PerStateValueSetupSystemBase<CrabStateId>
	{
		[SerializeField]
		private SerializableDictionary<CrabStateId, float> _searchRanges;

		[SerializeField]
		private float _defaultValue = 25f;

		private IDetectionContext _context;

		protected override float DefaultValue => _defaultValue;

		[Inject]
		private void InjectDependencies(IDetectionContext context, ICurrentStateProvider<CrabStateId> stateProvider)
		{
			_context = context;
			SetStateProvider(stateProvider);
		}

		protected override bool TryGetValue(CrabStateId stateId, out float value)
		{
			if (_searchRanges.ContainsKey(stateId))
			{
				value = _searchRanges[stateId];
				return true;
			}
			value = 0f;
			return false;
		}

		protected override void Apply(float value)
		{
			_context.TargetSearchRange = value;
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
