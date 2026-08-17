using NomadDrive.Features.Objectives;
using NomadDrive.Features.Player;
using UnityEngine;

namespace NomadDrive.Features.Consumables
{
	public class Medicine : Consumable
	{
		[SerializeField]
		private MedicineConfig medicineConfig;

		[Tooltip("Medicine'in kullanılabilmesi için gereken minimum hasar (damage). Bu kadar damage ve aktif zehir yokken kullanılamaz.")]
		[SerializeField]
		private float minDamageToUse = 1f;

		public override void OnUseButtonDown()
		{
			PlayerStatsManager statsManager = playerService.StatsManager;
			if (!(statsManager.CurrentDamage < minDamageToUse) || statsManager.IsPoisoned)
			{
				if (medicineConfig != null)
				{
					statsManager.ApplyMedicine(medicineConfig.poisonHealRatePerMinute, medicineConfig.instantPoisonReduction);
				}
				base.OnUseButtonDown();
				ObjectivesEventBus.Raise(ObjectiveSignal.PlayerCured, this);
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
