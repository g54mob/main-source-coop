using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.SessionManagementModule.Models;
using UnityEngine;

namespace Features.SessionManagementModule.Installers
{
	public sealed class SessionRunStarter : ISessionRunStarter
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly ChapterProgressSyncModel _chapterProgressSyncModel;

		private readonly Features.LevelModule.Scripts.LevelModel _levelModel;

		private readonly LevelsConfiguration _levelsConfiguration;

		private readonly ISessionStateContext _sessionStateContext;

		private readonly IMultiplayerService _multiplayerService;

		public SessionRunStarter(MultiplayerModel multiplayerModel, ChapterProgressSyncModel chapterProgressSyncModel, Features.LevelModule.Scripts.LevelModel levelModel, LevelsConfiguration levelsConfiguration, ISessionStateContext sessionStateContext, IMultiplayerService multiplayerService)
		{
			_multiplayerModel = multiplayerModel;
			_chapterProgressSyncModel = chapterProgressSyncModel;
			_levelModel = levelModel;
			_levelsConfiguration = levelsConfiguration;
			_sessionStateContext = sessionStateContext;
			_multiplayerService = multiplayerService;
		}

		public void RequestStart()
		{
			if (!_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			int num = _levelModel.SelectedChapterIndex;
			LevelSequenceSet levelSequenceSet = ((_levelModel.SelectedSequenceSet == LevelSequenceSet.None) ? LevelSequenceSet.Default : _levelModel.SelectedSequenceSet);
			if (_levelsConfiguration.ChapterSequences.TryGetValue(levelSequenceSet, out var value) && value.Count > 0 && num >= value.Count)
			{
				Debug.LogWarning($"[SessionLevel] Selected chapter {num} does not exist in sequence set {levelSequenceSet}; clamping to {value.Count - 1}.");
				num = value.Count - 1;
				_levelModel.SetSelectedChapter(num);
			}
			int chaptersCount = _chapterProgressSyncModel.ChaptersCount;
			if (chaptersCount > 0 && (num < 0 || num >= chaptersCount))
			{
				Debug.LogError($"[SessionLevel] Selected chapter {num} is out of range (valid 0..{chaptersCount - 1}); aborting run start.");
				return;
			}
			int num2 = ResolveAvailableChapter(num);
			if (num2 != num)
			{
				Debug.LogWarning($"[SessionLevel] Selected chapter {num} is locked for the host; starting on available chapter {num2} instead (SPEC-003 LS2).");
				_levelModel.SetSelectedChapter(num2);
			}
			_multiplayerService.MarkSessionStarted(_multiplayerModel.NetworkRunner);
			_sessionStateContext.Transition((num2 > 0) ? SessionState.Shop : SessionState.Level);
		}

		private int ResolveAvailableChapter(int requestedChapter)
		{
			if (_chapterProgressSyncModel.ChaptersCount <= 0)
			{
				return requestedChapter;
			}
			for (int num = requestedChapter; num > 0; num--)
			{
				if (_chapterProgressSyncModel.IsChapterAvailable(num))
				{
					return num;
				}
			}
			return 0;
		}
	}
}
