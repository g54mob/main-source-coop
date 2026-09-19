using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.AIModule.Data;
using Features.GamePauseModule.Scripts;
using Features.LevelModule.Scripts.RoomVariations;
using Features.ProgressSavingModule.Scripts.Implementation;
using Features.SceneManagement;

namespace Features.LevelModule.Scripts
{
	public class LevelService : ILevelService
	{
		private readonly LevelModel _levelModel;

		private readonly LevelsConfiguration _levelsConfiguration;

		private readonly INetworkSceneLoaderServiceFacade _networkSceneLoaderService;

		private readonly BeforeLevelChangeNetworkEvent _beforeLevelChangeNetworkEvent;

		private readonly ChapterCompletedNetworkEvent _chapterCompletedNetworkEvent;

		private readonly GameGlobalNetworkingPause _gameGlobalNetworkingPause;

		private readonly SpawnedRoomsModel _spawnedRoomsModel;

		private readonly NavigationModel _navigationModel;

		private readonly ChapterProgressModel _chapterProgressModel;

		private readonly ISavingService _savingService;

		public LevelService(LevelModel levelModel, LevelsConfiguration levelsConfiguration, INetworkSceneLoaderServiceFacade networkSceneLoaderService, BeforeLevelChangeNetworkEvent beforeLevelChangeNetworkEvent, ChapterCompletedNetworkEvent chapterCompletedNetworkEvent, GameGlobalNetworkingPause gameGlobalNetworkingPause, SpawnedRoomsModel spawnedRoomsModel, NavigationModel navigationModel, ChapterProgressModel chapterProgressModel, ISavingService savingService)
		{
			_levelModel = levelModel;
			_levelsConfiguration = levelsConfiguration;
			_networkSceneLoaderService = networkSceneLoaderService;
			_beforeLevelChangeNetworkEvent = beforeLevelChangeNetworkEvent;
			_chapterCompletedNetworkEvent = chapterCompletedNetworkEvent;
			_gameGlobalNetworkingPause = gameGlobalNetworkingPause;
			_spawnedRoomsModel = spawnedRoomsModel;
			_navigationModel = navigationModel;
			_chapterProgressModel = chapterProgressModel;
			_savingService = savingService;
		}

		public string GetSelectedLevelName()
		{
			return _levelsConfiguration.LevelsPool[_levelModel.SelectedLevel];
		}

		public string GetLevelNameByType(LevelType levelType)
		{
			return _levelsConfiguration.LevelsPool[levelType];
		}

		public LevelType GetFirstLevelOfChapter(int chapterIndex)
		{
			LevelSequenceSet key = ((_levelModel.SelectedSequenceSet == LevelSequenceSet.None) ? LevelSequenceSet.Default : _levelModel.SelectedSequenceSet);
			if (_levelsConfiguration.ChapterSequences.TryGetValue(key, out var value) && value.Count > 0)
			{
				int index = ((chapterIndex >= 0) ? ((chapterIndex >= value.Count) ? (value.Count - 1) : chapterIndex) : 0);
				if (value[index].Levels.Count > 0)
				{
					return value[index].Levels[0];
				}
			}
			return LevelType.Level1;
		}

		public async UniTask LoadLevel(LevelType levelType, bool pauseGame)
		{
			bool pausedBeforeLoading = _gameGlobalNetworkingPause.IsGlobalPausedEnable;
			if (pauseGame)
			{
				_gameGlobalNetworkingPause.SetGlobalPaused(isPaused: true);
			}
			_beforeLevelChangeNetworkEvent.SendEvent(_levelModel.CurrentLevel, levelType);
			if (_levelModel.CurrentLevel != LevelType.None)
			{
				LevelType currentLevel = _levelModel.CurrentLevel;
				_levelModel.SetCurrentLevel(levelType, sync: false);
				await _networkSceneLoaderService.UnloadSceneAsync(GetLevelNameByType(currentLevel), NetworkSceneLoadingFlags.SyncLoading);
			}
			else
			{
				_levelModel.SetCurrentLevel(levelType, sync: false);
			}
			await _networkSceneLoaderService.LoadSceneAsync(GetLevelNameByType(levelType), NetworkSceneLoadingFlags.SyncLoading);
			await UniTask.WaitUntil(() => _spawnedRoomsModel.IsAllTasksCompleted);
			_levelModel.CurrentLevelNumber++;
			RefreshSequenceLevelNumber();
			_levelModel.Synchronize();
			if (pauseGame)
			{
				_gameGlobalNetworkingPause.SetGlobalPaused(pausedBeforeLoading);
			}
			_levelModel.InvokeOnBeforeLevelLoaded(levelType);
			_levelModel.InvokeOnLevelLoaded(levelType);
		}

		public async UniTask<bool> LoadNextLevel(bool pauseGame)
		{
			if (!TryGetCurrentChapter(out var chapters, out var chapter))
			{
				return false;
			}
			if (_levelModel.CurrentLevelInChapterIndex >= chapter.Levels.Count)
			{
				_chapterCompletedNetworkEvent.SendEvent(_levelModel.CurrentChapterIndex);
				if (!TryGetNextNonEmptyChapter(chapters, out var nextChapterIndex))
				{
					return false;
				}
				_levelModel.SetChapterCursor(nextChapterIndex, 0, sync: false);
				chapter = chapters[nextChapterIndex];
			}
			LevelType levelType = chapter.Levels[_levelModel.CurrentLevelInChapterIndex];
			_levelModel.SetChapterCursor(_levelModel.CurrentChapterIndex, _levelModel.CurrentLevelInChapterIndex + 1, sync: false);
			await LoadLevel(levelType, pauseGame);
			return true;
		}

		public async UniTask UnloadCurrentLevel()
		{
			if (_levelModel.CurrentLevel != LevelType.None)
			{
				LevelType currentLevel = _levelModel.CurrentLevel;
				_beforeLevelChangeNetworkEvent.SendEvent(currentLevel, LevelType.None);
				_levelModel.SetCurrentLevel(LevelType.None, sync: false);
				await _networkSceneLoaderService.UnloadSceneAsync(GetLevelNameByType(currentLevel), NetworkSceneLoadingFlags.SyncLoading);
				_spawnedRoomsModel.Reset();
				_levelModel.SetChapterCursor(0, 0, sync: false);
				_levelModel.CurrentSequenceLevelNumber = 0;
				_levelModel.Synchronize();
			}
		}

		public async UniTask<bool> LoadChapter(int chapterIndex, bool pauseGame)
		{
			if (!_levelsConfiguration.ChapterSequences.TryGetValue(_levelModel.SelectedSequenceSet, out var value))
			{
				return false;
			}
			if (chapterIndex < 0 || chapterIndex >= value.Count)
			{
				return false;
			}
			if (value[chapterIndex].Levels.Count == 0)
			{
				return false;
			}
			UnlockChaptersUpTo(chapterIndex);
			_levelModel.SetSelectedChapter(chapterIndex, sync: false);
			_levelModel.SetChapterCursor(chapterIndex, 0, sync: false);
			return await LoadNextLevel(pauseGame);
		}

		public void RefreshSequenceLevelNumber()
		{
			_levelModel.CurrentSequenceLevelNumber = _levelsConfiguration.GetSequenceLevelNumber(_levelModel.SelectedSequenceSet, _levelModel.CurrentChapterIndex, _levelModel.CurrentLevelInChapterIndex);
		}

		private void UnlockChaptersUpTo(int chapterIndex)
		{
			if (!_levelsConfiguration.ChapterSequences.TryGetValue(_levelModel.SelectedSequenceSet, out var value))
			{
				return;
			}
			for (int i = 0; i < chapterIndex; i++)
			{
				if (!value[i].IsOptional)
				{
					_chapterProgressModel.MarkChapterPassed(_levelModel.SelectedSequenceSet, i);
				}
			}
			_savingService.SaveDataForGroup(SavingGroup.ChapterProgress);
		}

		public bool IsNextLevelLoadAvailable()
		{
			if (!TryGetCurrentChapter(out var chapters, out var chapter))
			{
				return false;
			}
			if (_levelModel.CurrentLevelInChapterIndex < chapter.Levels.Count)
			{
				return true;
			}
			int nextChapterIndex;
			return TryGetNextNonEmptyChapter(chapters, out nextChapterIndex);
		}

		public bool TryPeekNextLevelType(out LevelType levelType)
		{
			levelType = LevelType.None;
			if (!TryGetCurrentChapter(out var chapters, out var chapter))
			{
				return false;
			}
			int num = _levelModel.CurrentLevelInChapterIndex;
			if (num >= chapter.Levels.Count)
			{
				if (!TryGetNextNonEmptyChapter(chapters, out var nextChapterIndex))
				{
					return false;
				}
				chapter = chapters[nextChapterIndex];
				num = 0;
			}
			if (num < 0 || num >= chapter.Levels.Count)
			{
				return false;
			}
			levelType = chapter.Levels[num];
			return true;
		}

		private bool TryGetCurrentChapter(out List<Chapter> chapters, out Chapter chapter)
		{
			chapter = null;
			if (!_levelsConfiguration.ChapterSequences.TryGetValue(_levelModel.SelectedSequenceSet, out chapters))
			{
				return false;
			}
			if (_levelModel.CurrentChapterIndex < 0 || _levelModel.CurrentChapterIndex >= chapters.Count)
			{
				return false;
			}
			chapter = chapters[_levelModel.CurrentChapterIndex];
			return true;
		}

		private bool TryGetNextNonEmptyChapter(List<Chapter> chapters, out int nextChapterIndex)
		{
			bool flag = _levelModel.CurrentChapterIndex >= 0 && _levelModel.CurrentChapterIndex < chapters.Count && chapters[_levelModel.CurrentChapterIndex].IsOptional;
			for (int i = _levelModel.CurrentChapterIndex + 1; i < chapters.Count; i++)
			{
				if (chapters[i].Levels.Count != 0 && (!chapters[i].IsOptional || flag))
				{
					nextChapterIndex = i;
					return true;
				}
			}
			nextChapterIndex = -1;
			return false;
		}
	}
}
