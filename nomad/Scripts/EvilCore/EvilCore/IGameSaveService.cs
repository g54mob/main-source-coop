using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCore
{
	public interface IGameSaveService
	{
		bool IsRestoring { get; }

		bool IsLoadedWorld { get; }

		int LoadedSeed { get; }

		bool HasLoadedLootLedger { get; }

		IReadOnlyList<int> LoadedLootSeeds { get; }

		bool HasSaveForActiveSlot { get; }

		bool CanSaveNow { get; }

		string ActiveSlotId { get; }

		event Action OnRestoreComplete;

		event Action OnSaveStarted;

		event Action OnSaveCompleted;

		SaveSlotInfo[] GetSaveSlots();

		void DeleteSaveSlot(string slotId);

		void MarkNewGame(string worldName);

		void SelectSlotForContinue(string slotId);

		void NotifyHostStarted();

		void RequestSave();

		void SaveNow();

		void UpdateRemotePlayerRecord(string puid, string displayName, byte[] survivalBlob, string equippedItemGuid);

		bool TryGetRemotePlayerRecord(string puid, out byte[] survivalBlob, out string equippedItemGuid, out bool isDowned, out Vector3 downedPosition, out Vector3 downedEulerAngles);
	}
}
