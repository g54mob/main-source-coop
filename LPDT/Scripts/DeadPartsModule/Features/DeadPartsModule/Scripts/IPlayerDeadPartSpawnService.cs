using System.Threading.Tasks;
using PlayerCustomization;
using UnityEngine;

namespace Features.DeadPartsModule.Scripts
{
	public interface IPlayerDeadPartSpawnService
	{
		void SpawnPlayerDeadPartForCurrentPlayer();

		Task<PlayerDeadPart> SpawnPlayerDeadPart(DeadPartType type, Vector3 position, int usageCound, PlayerCustomizationSlotData slotData, bool addForce = false);

		Task<PlayerDeadPart> SpawnPlayerDeadPart(DeadPartType type, Vector3 position, int usageCound, bool addForce = false);

		void ReapplySavedDeadPartBooster();
	}
}
