using UnityEngine;

namespace Features.ChestScreamerModule.Scripts.Systems
{
	public interface IChestScreamerGameplay
	{
		bool HasPreset(ChestScreamerType type);

		bool CanTrigger(ChestScreamerType type, float openDeltaDegrees, float grabHoldSeconds, bool isLidGrabbed);

		float GetUnblockGrabTimer(ChestScreamerType type);

		bool IsBlockedGrabbleAfterScreamer(ChestScreamerType type);

		float GetOpenImpulsePerMass(ChestScreamerType type);

		float GetOpenImpulseForwardBias(ChestScreamerType type);

		float GetHoldOpenAngleDegrees(ChestScreamerType type);

		void PlayFeedback(ChestScreamerType type, Vector3 position, Quaternion rotation, int soundSourceId);

		void RestorePersistedScreamerVisual(ChestScreamerType type, Vector3 position, Quaternion rotation);
	}
}
