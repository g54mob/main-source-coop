using System.IO;
using Cysharp.Threading.Tasks;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.NetworkRandomModule.Scripts;
using Features.SessionManagementModule.Models;
using UnityEngine.SceneManagement;

namespace Features.SessionManagementModule.Installers
{
	public sealed class SessionNetworkedSceneLoader : INetworkedSceneLoader
	{
		private readonly ILevelService _levelService;

		private readonly Features.LevelModule.Scripts.LevelModel _levelModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly INetworkRandomInitializationService _networkRandomInitializationService;

		private bool _advanceOnNextLoad;

		public SessionNetworkedSceneLoader(ILevelService levelService, Features.LevelModule.Scripts.LevelModel levelModel, MultiplayerModel multiplayerModel, INetworkRandomInitializationService networkRandomInitializationService)
		{
			_levelService = levelService;
			_levelModel = levelModel;
			_multiplayerModel = multiplayerModel;
			_networkRandomInitializationService = networkRandomInitializationService;
		}

		public void RequestAdvanceOnNextLoad()
		{
			_advanceOnNextLoad = true;
		}

		public bool HasNextLevel()
		{
			return _levelService.IsNextLevelLoadAvailable();
		}

		public async UniTask UnloadAsync()
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_advanceOnNextLoad = false;
				await _levelService.UnloadCurrentLevel();
			}
		}

		public async UniTask LoadSceneAsync(string levelName, int transitionEpoch)
		{
			if (!_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			bool advanceOnNextLoad = _advanceOnNextLoad;
			_advanceOnNextLoad = false;
			if (transitionEpoch <= _levelModel.LastLoadTransitionEpoch)
			{
				return;
			}
			if (_levelModel.CurrentLevel == LevelType.None)
			{
				LevelType firstLevelOfChapter = _levelService.GetFirstLevelOfChapter(_levelModel.SelectedChapterIndex);
				if (IsLevelSceneLoaded(_levelService.GetLevelNameByType(firstLevelOfChapter)))
				{
					if (_levelModel.SelectedSequenceSet == LevelSequenceSet.None)
					{
						_levelModel.SetSelectedSequenceSet(LevelSequenceSet.Default, sync: false);
					}
					_levelModel.SetChapterCursor(_levelModel.SelectedChapterIndex, 1, sync: false);
					_levelModel.SetLastLoadTransitionEpoch(transitionEpoch, sync: false);
					_levelService.RefreshSequenceLevelNumber();
					_levelModel.SetCurrentLevel(firstLevelOfChapter);
					return;
				}
			}
			if (_levelModel.CurrentLevel != LevelType.None)
			{
				if (advanceOnNextLoad)
				{
					_levelModel.SetLastLoadTransitionEpoch(transitionEpoch, sync: false);
					await _levelService.LoadNextLevel(pauseGame: false);
				}
				return;
			}
			_networkRandomInitializationService.InjectNetworkRandoms();
			if (_levelModel.SelectedSequenceSet == LevelSequenceSet.None)
			{
				_levelModel.SetSelectedSequenceSet(LevelSequenceSet.Default, sync: false);
			}
			_levelModel.SetChapterCursor(_levelModel.SelectedChapterIndex, 0, sync: false);
			_levelModel.SetLastLoadTransitionEpoch(transitionEpoch, sync: false);
			await _levelService.LoadNextLevel(pauseGame: false);
		}

		private static bool IsLevelSceneLoaded(string levelName)
		{
			Scene sceneByName = SceneManager.GetSceneByName(Path.GetFileNameWithoutExtension(levelName));
			if (sceneByName.IsValid())
			{
				return sceneByName.isLoaded;
			}
			return false;
		}
	}
}
