using System.Collections.Generic;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.ProgressSavingModule.Scripts.Implementation;
using Features.QuotaModule.Scripts;
using Features.SessionManagementModule.Models;
using Fusion;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.DebugModule.Scripts
{
	[PublicAPI]
	public class ChapterDebugPresenter : PresenterBehaviour<ChapterDebugViewBase>
	{
		private const int CountdownAlreadyElapsedTicks = 100000;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly Features.LevelModule.Scripts.LevelModel _levelModel;

		private readonly LevelsConfiguration _levelsConfiguration;

		private readonly ChapterProgressModel _chapterProgressModel;

		private readonly ChapterProgressSyncModel _chapterProgressSyncModel;

		private readonly QuotaCompletionModel _quotaCompletionModel;

		private readonly Features.SessionManagementModule.Models.LevelModel _sessionLevelModel;

		private readonly ISavingService _savingService;

		private readonly ISessionRunStarter _sessionRunStarter;

		private readonly SessionStateMachine _sessionStateMachine;

		public ChapterDebugPresenter(MultiplayerModel multiplayerModel, Features.LevelModule.Scripts.LevelModel levelModel, LevelsConfiguration levelsConfiguration, ChapterProgressModel chapterProgressModel, ChapterProgressSyncModel chapterProgressSyncModel, QuotaCompletionModel quotaCompletionModel, Features.SessionManagementModule.Models.LevelModel sessionLevelModel, ISavingService savingService, ISessionRunStarter sessionRunStarter, SessionStateMachine sessionStateMachine)
		{
			_multiplayerModel = multiplayerModel;
			_levelModel = levelModel;
			_levelsConfiguration = levelsConfiguration;
			_chapterProgressModel = chapterProgressModel;
			_chapterProgressSyncModel = chapterProgressSyncModel;
			_quotaCompletionModel = quotaCompletionModel;
			_sessionLevelModel = sessionLevelModel;
			_savingService = savingService;
			_sessionRunStarter = sessionRunStarter;
			_sessionStateMachine = sessionStateMachine;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.OpenChapterButton.onClick.AddListener(OpenChapter);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.OpenChapterButton.onClick.RemoveListener(OpenChapter);
		}

		private void OpenChapter()
		{
			if (!int.TryParse(base.View.ChapterNumberInputField.text, out var result))
			{
				Debug.LogWarning("Select Chapter debug action requires a numeric chapter number.");
				return;
			}
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				Debug.LogWarning("Select Chapter debug action requires an active network session.");
				return;
			}
			if (!networkRunner.IsSharedModeMasterClient)
			{
				Debug.LogWarning("Select Chapter debug action is host-only because session start is state-authoritative.");
				return;
			}
			int num = result - 1;
			LevelSequenceSet selectedSequenceSet = GetSelectedSequenceSet();
			if (TryGetChapters(selectedSequenceSet, out var chapters))
			{
				if (num < 0 || num >= chapters.Count)
				{
					Debug.LogWarning($"Select Chapter debug action rejected chapter {result}; valid range is 1..{chapters.Count}.");
					return;
				}
				if (chapters[num].Levels.Count == 0)
				{
					Debug.LogWarning($"Select Chapter debug action rejected chapter {result}; chapter has no levels.");
					return;
				}
				DebugUnlockChaptersUpTo(selectedSequenceSet, chapters, num);
				SelectChapter(num, networkRunner);
			}
		}

		private void SelectChapter(int chapterIndex, NetworkRunner runner)
		{
			if (_sessionStateMachine.Current == SessionState.Lobby && _sessionStateMachine.IsActive)
			{
				_levelModel.SetSelectedChapter(chapterIndex);
				_sessionRunStarter.RequestStart();
			}
			else if (_sessionStateMachine.Current == SessionState.Level && _sessionStateMachine.IsActive)
			{
				_levelModel.SetSelectedChapter(chapterIndex, sync: false);
				_levelModel.SetChapterCursor(chapterIndex, 0, sync: false);
				CompleteCurrentLevel(runner);
			}
			else
			{
				Debug.LogWarning("Select Chapter debug action must be used from an active Lobby or Level state.");
			}
		}

		private void CompleteCurrentLevel(NetworkRunner runner)
		{
			_quotaCompletionModel.IsQuotaCompleted.Value = true;
			_sessionLevelModel.CountdownStartTick.Value = (int)runner.Tick - 100000;
			_quotaCompletionModel.IsBellActivated.Value = true;
		}

		private LevelSequenceSet GetSelectedSequenceSet()
		{
			if (_levelModel.SelectedSequenceSet != LevelSequenceSet.None)
			{
				return _levelModel.SelectedSequenceSet;
			}
			_levelModel.SetSelectedSequenceSet(LevelSequenceSet.Default, sync: false);
			return LevelSequenceSet.Default;
		}

		private bool TryGetChapters(LevelSequenceSet sequenceSet, out List<Chapter> chapters)
		{
			if (_levelsConfiguration.ChapterSequences.TryGetValue(sequenceSet, out chapters))
			{
				return true;
			}
			Debug.LogWarning($"Select Chapter debug action rejected sequence set {sequenceSet}; no chapter sequence configured.");
			return false;
		}

		private void DebugUnlockChaptersUpTo(LevelSequenceSet sequenceSet, List<Chapter> chapters, int chapterIndex)
		{
			for (int i = 0; i < chapterIndex; i++)
			{
				if (!chapters[i].IsOptional)
				{
					_chapterProgressModel.MarkChapterPassed(sequenceSet, i);
				}
			}
			_savingService.SaveDataForGroup(SavingGroup.ChapterProgress);
			_chapterProgressSyncModel.Publish(sequenceSet, chapters.Count, _chapterProgressModel.GetPassedChapters(sequenceSet), _chapterProgressModel.GetSubLevelProgress(sequenceSet));
		}
	}
}
