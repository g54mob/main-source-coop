using System;
using Cysharp.Threading.Tasks;

namespace EvilCore.EvilSave
{
	public interface IEvilSaveManager
	{
		string ActiveSlot { get; set; }

		bool IsLoading { get; }

		bool IsSaving { get; }

		event Action OnBeforeSave;

		event Action OnAfterSave;

		event Action OnBeforeLoad;

		event Action OnAfterLoad;

		void SaveGame();

		void LoadGame();

		UniTask SaveGameAsync();

		UniTask LoadGameAsync();

		void ApplyLoadedSaveables();
	}
}
