using Features.AIModuleStateMachine.Scripts.SirenEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.SirenEnemy.Settings;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.SirenEnemy
{
	public class SirenEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private SirenEnemySettings _enemySettings;

		[SerializeField]
		private SirenIdleSettings _idleSettings;

		[SerializeField]
		private SirenChaseSettings _chaseSettings;

		[SerializeField]
		private SirenAttackSettings _attackSettings;

		[SerializeField]
		private SirenStunSettings _stunSettings;

		[SerializeField]
		private SirenEnemyContext _context;

		[SerializeField]
		private SirenEnemy _enemy;

		public override void InstallBindings()
		{
			base.Container.BindInstance(_enemySettings).AsSingle();
			base.Container.BindInstance(_idleSettings).AsSingle();
			base.Container.BindInstance(_chaseSettings).AsSingle();
			base.Container.BindInstance(_attackSettings).AsSingle();
			base.Container.BindInstance(_stunSettings).AsSingle();
			base.Container.Bind<SirenTargetSensor>().AsSingle();
			base.Container.BindInstance(_context).AsSingle();
			base.Container.BindInstance(_enemy).AsSingle();
		}
	}
}
