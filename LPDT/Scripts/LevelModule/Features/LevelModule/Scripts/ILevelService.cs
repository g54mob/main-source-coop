using Cysharp.Threading.Tasks;

namespace Features.LevelModule.Scripts
{
	public interface ILevelService
	{
		string GetSelectedLevelName();

		string GetLevelNameByType(LevelType levelType);

		LevelType GetFirstLevelOfChapter(int chapterIndex);

		bool IsNextLevelLoadAvailable();

		bool TryPeekNextLevelType(out LevelType levelType);

		UniTask LoadLevel(LevelType levelType, bool pauseGame);

		UniTask<bool> LoadNextLevel(bool pauseGame);

		UniTask UnloadCurrentLevel();

		UniTask<bool> LoadChapter(int chapterIndex, bool pauseGame);

		void RefreshSequenceLevelNumber();
	}
}
