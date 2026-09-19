using System;

namespace Features.EntitiesSoundOcclusionModule.Scripts.EnemiesSoundOcclusion
{
	public interface IEnemySoundOcclusionModel
	{
		float HearingStrength { get; set; }

		event Action<HeardSound> OnEnemyTriggeredBySound;

		void TriggerEnemyBySound(HeardSound heardSound);
	}
}
