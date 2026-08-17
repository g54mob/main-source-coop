using NomadDrive.Features.Objectives;
using NomadDrive.Features.Player;
using UnityEngine;

namespace NomadDrive.Features.Consumables
{
	public class FirstAidKit : Consumable
	{
		[Tooltip("Kit'in kullanılabilmesi için gereken minimum hasar (damage). Bu kadar damage ve aktif zehir yokken kullanılamaz.")]
		[SerializeField]
		private float minDamageToUse = 1f;

		public override void OnUseButtonDown()
		{
			PlayerStatsManager statsManager = playerService.StatsManager;
			if (!(statsManager.CurrentDamage < minDamageToUse) || statsManager.IsPoisoned)
			{
				base.OnUseButtonDown();
				ObjectivesEventBus.Raise(ObjectiveSignal.PlayerHealed, this);
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
