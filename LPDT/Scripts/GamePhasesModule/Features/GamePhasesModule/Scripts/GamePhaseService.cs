using Features.AIModule.Scripts;
using Features.EnemyFearingModule.Scripts;
using Features.GamePhasesModule.Scripts.Data;
using Features.LevelModule.Scripts;
using Features.QuotaModule.Scripts;
using UnityEngine;

namespace Features.GamePhasesModule.Scripts
{
	public class GamePhaseService : IGamePhaseService
	{
		private readonly GamePhasesModel _gamePhasesModel;

		private readonly IEnemyFearService _enemyFearService;

		private readonly QuotaSynchronizedModel _quotaSynchronizedModel;

		private readonly EnemiesPoolModel _enemiesPoolModel;

		private readonly EnemySpawnPointsModel _enemySpawnPointsModel;

		public GamePhaseService(GamePhasesModel gamePhasesModel, IEnemyFearService enemyFearService, QuotaSynchronizedModel quotaSynchronizedModel, EnemiesPoolModel enemiesPoolModel, EnemySpawnPointsModel enemySpawnPointsModel)
		{
			_gamePhasesModel = gamePhasesModel;
			_enemyFearService = enemyFearService;
			_quotaSynchronizedModel = quotaSynchronizedModel;
			_enemiesPoolModel = enemiesPoolModel;
			_enemySpawnPointsModel = enemySpawnPointsModel;
		}

		public void SubtractTime(float time)
		{
			if (!_gamePhasesModel.InTransition)
			{
				_gamePhasesModel.CurrentPhaseTime -= time;
				if (_gamePhasesModel.CurrentPhaseTime <= 0f)
				{
					ActivateNextGamePhaseInSequence();
				}
			}
		}

		public void ChangeSubtractByTimeMultiplier(float timeMultiplier)
		{
			_gamePhasesModel.TimeMultiplier = timeMultiplier;
		}

		public void ActivateGamePhaseLoop(GamePhasesData gamePhasesData, LevelType levelType)
		{
			_gamePhasesModel.InvokeBeforeGamePhaseLoopActivated();
			_enemyFearService.CancelFear();
			_enemySpawnPointsModel.ClearOneTimeSpawnPoints();
			_gamePhasesModel.IsSomePlayerMovedFromSpawn = false;
			_gamePhasesModel.GamePhasesData = gamePhasesData;
			_gamePhasesModel.SetCurrentPhaseCount(0);
			ActivateNextGamePhaseInSequence();
		}

		public void ResumeAfterMigration(GamePhasesData gamePhasesData)
		{
			_gamePhasesModel.GamePhasesData = gamePhasesData;
			_gamePhasesModel.CurrentPhaseTime = gamePhasesData.GamePhaseTime;
			_gamePhasesModel.InTransition = false;
			_gamePhasesModel.QuotaOnStartPhase = _quotaSynchronizedModel.CurrentQuota.Value;
		}

		public void ActivateNextGamePhaseInSequence()
		{
			if (_gamePhasesModel.GamePhasesData != null && !_gamePhasesModel.InTransition)
			{
				_gamePhasesModel.InTransition = true;
				TransitionToNextPhase();
			}
		}

		private async void TransitionToNextPhase()
		{
			_gamePhasesModel.SetCurrentPhaseCount(_gamePhasesModel.CurrentPhaseCount + 1);
			_gamePhasesModel.InvokeBeforeFearAllEnemies();
			foreach (EnemyTypeWithTransform item in await _enemyFearService.FearAllEnemy(_enemiesPoolModel.ChosenEnemyTypes))
			{
				_enemySpawnPointsModel.RegisterSpawnPoint(item.EnemyType, new EnemySpawnPointData(item.Position, Quaternion.identity, item.Position, isOneTimeSpawnPoint: true));
			}
			_gamePhasesModel.CurrentPhaseTime = _gamePhasesModel.GamePhasesData.GamePhaseTime;
			_gamePhasesModel.SetIsActive(value: true);
			_gamePhasesModel.InTransition = false;
			_gamePhasesModel.QuotaOnStartPhase = _quotaSynchronizedModel.CurrentQuota.Value;
			_gamePhasesModel.InvokeOnGamePhaseActivated();
		}
	}
}
