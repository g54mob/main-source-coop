using Cysharp.Threading.Tasks;
using Features.AIModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Features.SessionManagementModule.Models;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using Features.ViewSystemModule.Scripts.Windows;
using Fusion;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.BootstrapModule.Scripts.Bootstraps
{
	[NetworkBehaviourWeaved(0)]
	public class TutorialSceneBootstrap : SessionSceneBootstrap
	{
		private ISessionRunStarter _sessionRunStarter;

		private IMultiplayerService _multiplayerService;

		private SessionStateMachine _sessionStateMachine;

		private ILoadingScreenService _loadingScreenService;

		private ITutorialStartupService _tutorialStartupService;

		private EnemySpawnConfigurationModel _enemySpawnConfigurationModel;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private IWindowsService _windowsService;

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		[Inject]
		public void InjectDependencies(ISessionRunStarter sessionRunStarter, IMultiplayerService multiplayerService, SessionStateMachine sessionStateMachine, ILoadingScreenService loadingScreenService, ITutorialStartupService tutorialStartupService, EnemySpawnConfigurationModel enemySpawnConfigurationModel, SpawnedPlayersModel spawnedPlayersModel, IWindowsService windowsService, SpawnedEntityStatsModel spawnedEntityStatsModel)
		{
			_sessionRunStarter = sessionRunStarter;
			_multiplayerService = multiplayerService;
			_sessionStateMachine = sessionStateMachine;
			_loadingScreenService = loadingScreenService;
			_tutorialStartupService = tutorialStartupService;
			_enemySpawnConfigurationModel = enemySpawnConfigurationModel;
			_spawnedPlayersModel = spawnedPlayersModel;
			_windowsService = windowsService;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_enemySpawnConfigurationModel.CurrentSpawnConfiguration = EnemySpawnConfigurationType.Tutorial;
			_loadingScreenService.Show(LoadingScreenShowType.ShowScreenWithFade, AutoStartTutorialRun);
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= InitPlayerStats;
		}

		private async void AutoStartTutorialRun()
		{
			if (base.Runner.IsSharedModeMasterClient)
			{
				await UniTask.WaitUntil(() => _sessionStateMachine.IsAuthority && _sessionStateMachine.IsActive);
				_sessionRunStarter.RequestStart();
				_multiplayerService.SetSessionPublic(base.Runner, isPublic: false);
				await UniTask.WaitUntil(() => _spawnedPlayersModel.Players.ContainsKey(base.Runner.LocalPlayer));
				await _windowsService.OpenWindowAsync<TutorialWindow>();
				_tutorialStartupService.StartBaseTutorial();
				if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(base.Runner.LocalPlayer.PlayerId))
				{
					InitPlayerStats(base.Runner.LocalPlayer.PlayerId);
				}
				else
				{
					_spawnedEntityStatsModel.OnPlayerStatRegistered += InitPlayerStats;
				}
			}
		}

		private void InitPlayerStats(int playerId)
		{
			if (base.Runner.LocalPlayer.PlayerId == playerId)
			{
				_spawnedEntityStatsModel.PlayerStats[playerId].GetStat(EntityStatType.Invisibility).OverrideValue(1f);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
