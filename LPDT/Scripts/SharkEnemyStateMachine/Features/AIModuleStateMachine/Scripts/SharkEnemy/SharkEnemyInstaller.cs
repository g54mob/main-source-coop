using Features.AIModuleStateMachine.Scripts.Core.Sensors;
using Features.AIModuleStateMachine.Scripts.SharkEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.SharkEnemy.Settings;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.SharkEnemy
{
	public class SharkEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private SharkMovementSettings _movementSettings;

		[SerializeField]
		private SharkTargetingSettings _targetingSettings;

		[SerializeField]
		private SharkAttackSettings _attackSettings;

		[SerializeField]
		private SharkAudioSettings _audioSettings;

		[SerializeField]
		private SharkEnemyContext _context;

		[SerializeField]
		private SharkEnemy _enemy;

		[SerializeField]
		private EnemyPovDetector _povDetector;

		[SerializeField]
		private EnemyLineOfSightDetector _lineOfSightDetector;

		public override void InstallBindings()
		{
			base.Container.Bind<SharkMovementSettings>().FromInstance(_movementSettings).AsSingle();
			base.Container.Bind<SharkTargetingSettings>().FromInstance(_targetingSettings).AsSingle();
			base.Container.Bind<SharkAttackSettings>().FromInstance(_attackSettings).AsSingle();
			base.Container.Bind<SharkAudioSettings>().FromInstance(_audioSettings).AsSingle();
			base.Container.Bind<SharkEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<SharkEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<EnemyPovDetector>().FromInstance(_povDetector).AsSingle();
			base.Container.Bind<EnemyLineOfSightDetector>().FromInstance(_lineOfSightDetector).AsSingle();
			base.Container.Bind<SharkWaterTargetSensor>().AsSingle();
		}
	}
}
