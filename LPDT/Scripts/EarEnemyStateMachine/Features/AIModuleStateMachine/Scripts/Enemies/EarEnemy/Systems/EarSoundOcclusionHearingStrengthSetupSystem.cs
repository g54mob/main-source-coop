using Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.States;
using Features.EntitiesSoundOcclusionModule.Scripts.EnemiesSoundOcclusion;
using Fusion;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class EarSoundOcclusionHearingStrengthSetupSystem : SoundOcclusionHearingStrengthSetupSystemBase<EarStateId>
	{
		[SerializeField]
		private Global.SerializableDictionary.SerializableDictionary<EarStateId, float> _hearingStrengths;

		[SerializeField]
		private float _defaultValue = 1f;

		protected override float DefaultValue => _defaultValue;

		protected override bool TryGetValue(EarStateId stateId, out float value)
		{
			if (_hearingStrengths.ContainsKey(stateId))
			{
				value = _hearingStrengths[stateId];
				return true;
			}
			value = 0f;
			return false;
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
