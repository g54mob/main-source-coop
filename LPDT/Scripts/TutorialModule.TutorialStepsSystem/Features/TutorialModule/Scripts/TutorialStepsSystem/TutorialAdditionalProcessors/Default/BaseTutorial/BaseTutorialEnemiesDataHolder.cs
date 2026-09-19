using System;
using Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial;
using Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialEnemiesDataHolder
	{
		private Vector3? _guideRunAwayPosition;

		private PlayerTutorialGuideEnemy _playerTutorialGuideEnemy;

		private PirateEnemy _pirateEnemy;

		public PlayerTutorialGuideEnemy PlayerTutorialGuideEnemy
		{
			get
			{
				return _playerTutorialGuideEnemy;
			}
			set
			{
				_playerTutorialGuideEnemy = value;
				this.OnPlayerTutorialGuideEnemyChanged?.Invoke(value);
			}
		}

		public PirateEnemy PirateEnemy
		{
			get
			{
				return _pirateEnemy;
			}
			set
			{
				_pirateEnemy = value;
				this.OnPirateEnemyChanged?.Invoke(value);
			}
		}

		public Vector3? GuideRunAwayPosition
		{
			get
			{
				return _guideRunAwayPosition;
			}
			set
			{
				_guideRunAwayPosition = value;
				this.OnGuideRunAwayPositionChanged?.Invoke(value);
			}
		}

		public event Action<PlayerTutorialGuideEnemy> OnPlayerTutorialGuideEnemyChanged;

		public event Action<PirateEnemy> OnPirateEnemyChanged;

		public event Action<Vector3?> OnGuideRunAwayPositionChanged;
	}
}
