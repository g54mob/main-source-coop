using System.Collections.Generic;
using Features.ProgressSavingModule.Scripts.Implementation;
using RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts;

namespace Features.LevelModule.Scripts
{
	public class ChapterProgressModel : ISavable
	{
		private const string SAVE_ID = "ChapterProgress";

		private readonly ISavingManager _savingManager;

		private readonly ChaptersConfiguration _chaptersConfiguration;

		private readonly LevelsConfiguration _levelsConfiguration;

		private ChapterProgressDataHolder _dataHolder = new ChapterProgressDataHolder();

		public SavingGroup SavingGroup => SavingGroup.ChapterProgress;

		public ChapterProgressModel(ISavingManager savingManager, ChaptersConfiguration chaptersConfiguration, LevelsConfiguration levelsConfiguration)
		{
			_savingManager = savingManager;
			_chaptersConfiguration = chaptersConfiguration;
			_levelsConfiguration = levelsConfiguration;
		}

		public bool IsChapterPassed(LevelSequenceSet sequenceSet, int chapterIndex)
		{
			return FindEntry(sequenceSet)?.PassedChapters.Contains(chapterIndex) ?? false;
		}

		public bool IsChapterAvailable(LevelSequenceSet sequenceSet, int chapterIndex)
		{
			if (!_levelsConfiguration.ChapterSequences.TryGetValue(sequenceSet, out var value))
			{
				return false;
			}
			if (!_chaptersConfiguration.IsWithChaptersLocking)
			{
				if (chapterIndex >= 0)
				{
					return chapterIndex < value.Count;
				}
				return false;
			}
			return ChapterAvailabilityResolver.IsChapterAvailable(value, (int index) => IsChapterPassed(sequenceSet, index), chapterIndex);
		}

		public void MarkChapterPassed(LevelSequenceSet sequenceSet, int chapterIndex)
		{
			ChapterProgressEntry chapterProgressEntry = FindEntry(sequenceSet);
			if (chapterProgressEntry == null)
			{
				chapterProgressEntry = new ChapterProgressEntry
				{
					Set = sequenceSet
				};
				_dataHolder.Entries.Add(chapterProgressEntry);
			}
			if (!chapterProgressEntry.PassedChapters.Contains(chapterIndex))
			{
				chapterProgressEntry.PassedChapters.Add(chapterIndex);
			}
		}

		public void RecordReachedSubLevel(LevelSequenceSet sequenceSet, int chapterIndex, int reachedSubLevelCount)
		{
			ChapterProgressEntry chapterProgressEntry = FindEntry(sequenceSet);
			if (chapterProgressEntry == null)
			{
				chapterProgressEntry = new ChapterProgressEntry
				{
					Set = sequenceSet
				};
				_dataHolder.Entries.Add(chapterProgressEntry);
			}
			ChapterProgressEntry chapterProgressEntry2 = chapterProgressEntry;
			if (chapterProgressEntry2.SubLevelProgress == null)
			{
				chapterProgressEntry2.SubLevelProgress = new List<ChapterSubLevelProgress>();
			}
			for (int i = 0; i < chapterProgressEntry.SubLevelProgress.Count; i++)
			{
				if (chapterProgressEntry.SubLevelProgress[i].ChapterIndex == chapterIndex)
				{
					if (reachedSubLevelCount > chapterProgressEntry.SubLevelProgress[i].MaxReachedSubLevelCount)
					{
						chapterProgressEntry.SubLevelProgress[i].MaxReachedSubLevelCount = reachedSubLevelCount;
					}
					return;
				}
			}
			chapterProgressEntry.SubLevelProgress.Add(new ChapterSubLevelProgress
			{
				ChapterIndex = chapterIndex,
				MaxReachedSubLevelCount = reachedSubLevelCount
			});
		}

		public IReadOnlyList<int> GetPassedChapters(LevelSequenceSet sequenceSet)
		{
			return FindEntry(sequenceSet)?.PassedChapters ?? new List<int>();
		}

		public IReadOnlyList<ChapterSubLevelProgress> GetSubLevelProgress(LevelSequenceSet sequenceSet)
		{
			return FindEntry(sequenceSet)?.SubLevelProgress ?? new List<ChapterSubLevelProgress>();
		}

		public int GetReachedSubLevelCount(LevelSequenceSet sequenceSet, int chapterIndex)
		{
			ChapterProgressEntry chapterProgressEntry = FindEntry(sequenceSet);
			if (chapterProgressEntry?.SubLevelProgress == null)
			{
				return 0;
			}
			for (int i = 0; i < chapterProgressEntry.SubLevelProgress.Count; i++)
			{
				if (chapterProgressEntry.SubLevelProgress[i].ChapterIndex == chapterIndex)
				{
					return chapterProgressEntry.SubLevelProgress[i].MaxReachedSubLevelCount;
				}
			}
			return 0;
		}

		public void LoadData()
		{
			ChapterProgressDataHolder chapterProgressDataHolder = _savingManager.LoadDataForID<ChapterProgressDataHolder>("ChapterProgress");
			if (chapterProgressDataHolder != null)
			{
				_dataHolder = chapterProgressDataHolder;
			}
			EnsureEntriesInitialized();
			MigrateLegacyHighestUnlocked();
		}

		public void SaveData()
		{
			_savingManager.SaveDataWithID("ChapterProgress", _dataHolder, SavingGroup.ToString());
		}

		private ChapterProgressEntry FindEntry(LevelSequenceSet sequenceSet)
		{
			EnsureEntriesInitialized();
			for (int i = 0; i < _dataHolder.Entries.Count; i++)
			{
				if (_dataHolder.Entries[i].Set == sequenceSet)
				{
					return _dataHolder.Entries[i];
				}
			}
			return null;
		}

		private void MigrateLegacyHighestUnlocked()
		{
			for (int i = 0; i < _dataHolder.Entries.Count; i++)
			{
				ChapterProgressEntry chapterProgressEntry = _dataHolder.Entries[i];
				ChapterProgressEntry chapterProgressEntry2 = chapterProgressEntry;
				if (chapterProgressEntry2.PassedChapters == null)
				{
					chapterProgressEntry2.PassedChapters = new List<int>();
				}
				chapterProgressEntry2 = chapterProgressEntry;
				if (chapterProgressEntry2.SubLevelProgress == null)
				{
					chapterProgressEntry2.SubLevelProgress = new List<ChapterSubLevelProgress>();
				}
				if (chapterProgressEntry.PassedChapters.Count <= 0 && chapterProgressEntry.HighestUnlockedChapterIndex > 0)
				{
					for (int j = 0; j <= chapterProgressEntry.HighestUnlockedChapterIndex; j++)
					{
						chapterProgressEntry.PassedChapters.Add(j);
					}
				}
			}
		}

		private void EnsureEntriesInitialized()
		{
			ChapterProgressDataHolder dataHolder = _dataHolder;
			if (dataHolder.Entries == null)
			{
				dataHolder.Entries = new List<ChapterProgressEntry>();
			}
		}
	}
}
