using System;

namespace Features.GamePhasesModule.Scripts.Data
{
	[Serializable]
	public class GamePhasesData
	{
		public float GamePhaseTime;

		public QuotaPercentForSubtractData QuotaPercentForSubtract;

		public float TimerSubtractByDeath;

		public float TimerMultiplierByHiddenPlayer;
	}
}
