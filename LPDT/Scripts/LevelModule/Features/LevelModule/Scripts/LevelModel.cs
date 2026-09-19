using System;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using UnityEngine;

namespace Features.LevelModule.Scripts
{
	[Serializable]
	public class LevelModel : JsonSynchronizableBase<LevelModel>
	{
		[SerializeField]
		private LevelType _selectedLevel = LevelType.CoreLoopScene;

		[SerializeField]
		private LevelType _currentLevel;

		[SerializeField]
		private int _currentLevelNumber;

		[SerializeField]
		private int _currentSequenceLevelNumber;

		[SerializeField]
		private LevelSequenceSet _selectedSequenceSet = LevelSequenceSet.Default;

		[SerializeField]
		private ChapterType _selectedChapterType;

		[SerializeField]
		private int _selectedChapterIndex;

		[SerializeField]
		private int _currentChapterIndex;

		[SerializeField]
		private int _currentLevelInChapterIndex;

		[SerializeField]
		private int _lastLoadTransitionEpoch = -1;

		private LevelType _lastLoadedLevel;

		public override RPCType RPCType => RPCType.FromStateAuthorityToAll;

		public override bool IsNeedToSynchronizeOnSpawn => true;

		public LevelType SelectedLevel
		{
			get
			{
				return _selectedLevel;
			}
			private set
			{
				_selectedLevel = value;
			}
		}

		public LevelType CurrentLevel
		{
			get
			{
				return _currentLevel;
			}
			private set
			{
				if (_currentLevel != value)
				{
					_currentLevel = value;
					this.OnCurrentLevelChanged?.Invoke(_currentLevel);
				}
			}
		}

		public LevelType LastLoadedLevel
		{
			get
			{
				return _lastLoadedLevel;
			}
			set
			{
				_lastLoadedLevel = value;
				this.OnLastLoadedLevelChanged?.Invoke(_lastLoadedLevel);
			}
		}

		public int CurrentLevelNumber
		{
			get
			{
				return _currentLevelNumber;
			}
			set
			{
				_currentLevelNumber = value;
				this.OnCurrentLevelNumberChanged?.Invoke(_currentLevelNumber);
			}
		}

		public int CurrentSequenceLevelNumber
		{
			get
			{
				return _currentSequenceLevelNumber;
			}
			set
			{
				_currentSequenceLevelNumber = value;
				this.OnCurrentSequenceLevelNumberChanged?.Invoke(_currentSequenceLevelNumber);
			}
		}

		public LevelType ParentalLevel { get; set; }

		public LevelSequenceSet SelectedSequenceSet => _selectedSequenceSet;

		public ChapterType SelectedChapterType => _selectedChapterType;

		public int SelectedChapterIndex => _selectedChapterIndex;

		public int CurrentChapterIndex => _currentChapterIndex;

		public int CurrentLevelInChapterIndex => _currentLevelInChapterIndex;

		public int LastLoadTransitionEpoch => _lastLoadTransitionEpoch;

		public event Action<LevelType> OnLevelLoaded;

		public event Action<LevelType> OnBeforeLevelLoaded;

		public event Action<LevelType> OnLastLoadedLevelChanged;

		public event Action<LevelType> OnCurrentLevelChanged;

		public event Action<int> OnCurrentLevelNumberChanged;

		public event Action<int> OnCurrentSequenceLevelNumberChanged;

		public event Action<int> OnCurrentChapterIndexChanged;

		public event Action<int> OnSelectedChapterChanged;

		public event Action<ChapterType> OnSelectedChapterTypeChanged;

		public void SetSelectedLevel(LevelType level)
		{
			SelectedLevel = level;
			Synchronize();
		}

		public void SetSelectedSequenceSet(LevelSequenceSet sequenceSet, bool sync = true)
		{
			_selectedSequenceSet = sequenceSet;
			if (sync)
			{
				Synchronize();
			}
		}

		public void SetSelectedChapterType(ChapterType chapterType, bool sync = true)
		{
			bool num = _selectedChapterType != chapterType;
			_selectedChapterType = chapterType;
			if (num)
			{
				this.OnSelectedChapterTypeChanged?.Invoke(_selectedChapterType);
			}
			if (sync)
			{
				Synchronize();
			}
		}

		public void SetSelectedChapter(int chapterIndex, bool sync = true)
		{
			bool num = _selectedChapterIndex != chapterIndex;
			_selectedChapterIndex = chapterIndex;
			if (num)
			{
				this.OnSelectedChapterChanged?.Invoke(_selectedChapterIndex);
			}
			if (sync)
			{
				Synchronize();
			}
		}

		public void SetLastLoadTransitionEpoch(int epoch, bool sync = true)
		{
			_lastLoadTransitionEpoch = epoch;
			if (sync)
			{
				Synchronize();
			}
		}

		public void SetChapterCursor(int chapterIndex, int levelInChapterIndex, bool sync = true)
		{
			bool num = _currentChapterIndex != chapterIndex;
			_currentChapterIndex = chapterIndex;
			_currentLevelInChapterIndex = levelInChapterIndex;
			if (num)
			{
				this.OnCurrentChapterIndexChanged?.Invoke(_currentChapterIndex);
			}
			if (sync)
			{
				Synchronize();
			}
		}

		public void SetCurrentLevel(LevelType level, bool sync = true)
		{
			CurrentLevel = level;
			if (sync)
			{
				Synchronize();
			}
		}

		protected override void SetNewValues(LevelModel synchronizable, bool isSynchronizedOnStart)
		{
			SelectedLevel = synchronizable.SelectedLevel;
			CurrentLevel = synchronizable.CurrentLevel;
			_selectedSequenceSet = synchronizable._selectedSequenceSet;
			bool num = _selectedChapterIndex != synchronizable._selectedChapterIndex;
			_selectedChapterIndex = synchronizable._selectedChapterIndex;
			bool num2 = _selectedChapterType != synchronizable._selectedChapterType;
			_selectedChapterType = synchronizable._selectedChapterType;
			if (num2)
			{
				this.OnSelectedChapterTypeChanged?.Invoke(_selectedChapterType);
			}
			if (num)
			{
				this.OnSelectedChapterChanged?.Invoke(_selectedChapterIndex);
			}
			if (isSynchronizedOnStart)
			{
				CurrentLevelNumber = synchronizable.CurrentLevelNumber;
				CurrentSequenceLevelNumber = synchronizable.CurrentSequenceLevelNumber;
				SetChapterCursor(synchronizable.CurrentChapterIndex, synchronizable.CurrentLevelInChapterIndex, sync: false);
			}
			else if (synchronizable.CurrentLevel == LevelType.None)
			{
				CurrentLevelNumber = synchronizable.CurrentLevelNumber;
				CurrentSequenceLevelNumber = synchronizable.CurrentSequenceLevelNumber;
				SetChapterCursor(synchronizable.CurrentChapterIndex, synchronizable.CurrentLevelInChapterIndex, sync: false);
			}
			else
			{
				if (synchronizable.CurrentLevelNumber > CurrentLevelNumber)
				{
					CurrentLevelNumber = synchronizable.CurrentLevelNumber;
				}
				if (synchronizable.CurrentSequenceLevelNumber > CurrentSequenceLevelNumber)
				{
					CurrentSequenceLevelNumber = synchronizable.CurrentSequenceLevelNumber;
				}
				if (synchronizable.CurrentChapterIndex > CurrentChapterIndex || (synchronizable.CurrentChapterIndex == CurrentChapterIndex && synchronizable.CurrentLevelInChapterIndex > CurrentLevelInChapterIndex))
				{
					SetChapterCursor(synchronizable.CurrentChapterIndex, synchronizable.CurrentLevelInChapterIndex, sync: false);
				}
			}
			if (synchronizable._lastLoadTransitionEpoch > _lastLoadTransitionEpoch)
			{
				_lastLoadTransitionEpoch = synchronizable._lastLoadTransitionEpoch;
			}
		}

		public void InvokeOnLevelLoaded(LevelType levelType)
		{
			this.OnLevelLoaded?.Invoke(levelType);
		}

		public void InvokeOnBeforeLevelLoaded(LevelType levelType)
		{
			this.OnBeforeLevelLoaded?.Invoke(levelType);
		}
	}
}
