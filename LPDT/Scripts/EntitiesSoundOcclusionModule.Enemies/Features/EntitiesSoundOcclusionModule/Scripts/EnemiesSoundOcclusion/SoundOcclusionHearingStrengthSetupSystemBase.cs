using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.Systems.Setup;
using Fusion;
using Zenject;

namespace Features.EntitiesSoundOcclusionModule.Scripts.EnemiesSoundOcclusion
{
	[NetworkBehaviourWeaved(0)]
	public abstract class SoundOcclusionHearingStrengthSetupSystemBase<TStateId> : PerStateValueSetupSystemBase<TStateId>
	{
		private IEnemySoundOcclusionModel _enemySoundOcclusionModel;

		[Inject]
		public void InjectDependencies(IEnemySoundOcclusionModel enemySoundOcclusionModel, ICurrentStateProvider<TStateId> stateProvider)
		{
			_enemySoundOcclusionModel = enemySoundOcclusionModel;
			SetStateProvider(stateProvider);
		}

		protected override void Apply(float value)
		{
			_enemySoundOcclusionModel.HearingStrength = value;
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
