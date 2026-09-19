using System;
using Features.ProgressSavingModule.Scripts.Implementation;
using Zenject;

namespace Features.LevelModule.Scripts.Systems
{
	public class ChapterProgressSaveSystem : IInitializable, IDisposable
	{
		private readonly LevelModel _levelModel;

		private readonly ChapterProgressModel _chapterProgressModel;

		private readonly ChapterCompletedNetworkEvent _chapterCompletedNetworkEvent;

		private readonly ISavingService _savingService;

		public ChapterProgressSaveSystem(LevelModel levelModel, ChapterProgressModel chapterProgressModel, ChapterCompletedNetworkEvent chapterCompletedNetworkEvent, ISavingService savingService)
		{
			_levelModel = levelModel;
			_chapterProgressModel = chapterProgressModel;
			_chapterCompletedNetworkEvent = chapterCompletedNetworkEvent;
			_savingService = savingService;
		}

		public void Initialize()
		{
			_chapterCompletedNetworkEvent.OnNetworkEventSend += OnChapterCompleted;
			_levelModel.OnLevelLoaded += OnLevelLoaded;
		}

		public void Dispose()
		{
			_chapterCompletedNetworkEvent.OnNetworkEventSend -= OnChapterCompleted;
			_levelModel.OnLevelLoaded -= OnLevelLoaded;
		}

		private void OnChapterCompleted(ChapterCompletedNetworkEvent networkEvent)
		{
			_chapterProgressModel.MarkChapterPassed(_levelModel.SelectedSequenceSet, networkEvent.ChapterIndex);
			_savingService.SaveDataForGroup(SavingGroup.ChapterProgress);
		}

		private void OnLevelLoaded(LevelType _)
		{
			_chapterProgressModel.RecordReachedSubLevel(_levelModel.SelectedSequenceSet, _levelModel.CurrentChapterIndex, _levelModel.CurrentLevelInChapterIndex);
			_savingService.SaveDataForGroup(SavingGroup.ChapterProgress);
		}
	}
}
