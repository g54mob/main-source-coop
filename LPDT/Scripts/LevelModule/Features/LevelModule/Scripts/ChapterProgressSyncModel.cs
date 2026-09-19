using System;
using System.Collections.Generic;
using Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer;
using UnityEngine;
using Zenject;

namespace Features.LevelModule.Scripts
{
	[Serializable]
	public class ChapterProgressSyncModel : DataStreamSynchronizableBase<ChapterProgressSyncModel>
	{
		[SerializeField]
		private LevelSequenceSet _sequenceSet = LevelSequenceSet.Default;

		[SerializeField]
		private int _chaptersCount;

		[SerializeField]
		private List<int> _passedChapters = new List<int>();

		[SerializeField]
		private List<ChapterSubLevelProgress> _subLevelProgress = new List<ChapterSubLevelProgress>();

		private ChaptersConfiguration _chaptersConfiguration;

		private LevelsConfiguration _levelsConfiguration;

		public override bool IsNeedToSynchronizeOnSpawn => true;

		public LevelSequenceSet SequenceSet => _sequenceSet;

		public int ChaptersCount => _chaptersCount;

		public event Action OnChanged;

		[Inject]
		private void InjectDependencies(ChaptersConfiguration chaptersConfiguration, LevelsConfiguration levelsConfiguration)
		{
			_chaptersConfiguration = chaptersConfiguration;
			_levelsConfiguration = levelsConfiguration;
		}

		public bool IsChapterPassed(int chapterIndex)
		{
			if (_passedChapters != null)
			{
				return _passedChapters.Contains(chapterIndex);
			}
			return false;
		}

		public bool IsChapterAvailable(int chapterIndex)
		{
			if (chapterIndex < 0 || chapterIndex >= _chaptersCount)
			{
				return false;
			}
			if (!_chaptersConfiguration.IsWithChaptersLocking)
			{
				return true;
			}
			if (!_levelsConfiguration.ChapterSequences.TryGetValue(_sequenceSet, out var value))
			{
				return false;
			}
			return ChapterAvailabilityResolver.IsChapterAvailable(value, IsChapterPassed, chapterIndex);
		}

		public int GetReachedSubLevelCount(int chapterIndex)
		{
			if (_subLevelProgress == null)
			{
				return 0;
			}
			for (int i = 0; i < _subLevelProgress.Count; i++)
			{
				if (_subLevelProgress[i].ChapterIndex == chapterIndex)
				{
					return _subLevelProgress[i].MaxReachedSubLevelCount;
				}
			}
			return 0;
		}

		public void Publish(LevelSequenceSet sequenceSet, int chaptersCount, IReadOnlyList<int> passedChapters, IReadOnlyList<ChapterSubLevelProgress> subLevelProgress)
		{
			_sequenceSet = sequenceSet;
			_chaptersCount = chaptersCount;
			_passedChapters = ((passedChapters != null) ? new List<int>(passedChapters) : new List<int>());
			_subLevelProgress = new List<ChapterSubLevelProgress>();
			if (subLevelProgress != null)
			{
				for (int i = 0; i < subLevelProgress.Count; i++)
				{
					_subLevelProgress.Add(new ChapterSubLevelProgress
					{
						ChapterIndex = subLevelProgress[i].ChapterIndex,
						MaxReachedSubLevelCount = subLevelProgress[i].MaxReachedSubLevelCount
					});
				}
			}
			this.OnChanged?.Invoke();
			Synchronize();
		}

		protected override void SetNewValues(ChapterProgressSyncModel synchronizable)
		{
			_sequenceSet = synchronizable._sequenceSet;
			_chaptersCount = synchronizable._chaptersCount;
			_passedChapters = synchronizable._passedChapters ?? new List<int>();
			_subLevelProgress = synchronizable._subLevelProgress ?? new List<ChapterSubLevelProgress>();
			this.OnChanged?.Invoke();
		}
	}
}
