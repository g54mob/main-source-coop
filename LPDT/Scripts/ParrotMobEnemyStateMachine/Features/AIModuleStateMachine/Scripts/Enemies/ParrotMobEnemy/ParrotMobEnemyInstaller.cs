using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Settings;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	public class ParrotMobEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private ParrotMobEnemyContext _context;

		[SerializeField]
		private ParrotMobEnemy _enemy;

		[SerializeField]
		private ParrotMobAudioController _audioController;

		[SerializeField]
		private ParrotMobEnemySettings _enemySettings;

		[SerializeField]
		private ParrotMobIdleSettings _idleSettings;

		[SerializeField]
		private ParrotMobAlertSettings _alertSettings;

		[SerializeField]
		private ParrotMobScreamSettings _screamSettings;

		public override void InstallBindings()
		{
			base.Container.Bind<ParrotMobEnemySettings>().FromInstance(_enemySettings).AsSingle();
			base.Container.Bind<ParrotMobIdleSettings>().FromInstance(_idleSettings).AsSingle();
			base.Container.Bind<ParrotMobAlertSettings>().FromInstance(_alertSettings).AsSingle();
			base.Container.Bind<ParrotMobScreamSettings>().FromInstance(_screamSettings).AsSingle();
			base.Container.BindInterfacesAndSelfTo<ParrotMobEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<ParrotMobEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<ParrotMobAudioController>().FromInstance(_audioController).AsSingle();
			base.Container.Bind<ICurrentStateProvider<ParrotMobStateId>>().FromInstance(_enemy).AsSingle();
		}
	}
}
