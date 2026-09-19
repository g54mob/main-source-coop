using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy
{
	public class MonkeyPorterEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private MonkeyPorterSettings _settings;

		[SerializeField]
		private MonkeyPorterContext _context;

		[SerializeField]
		private MonkeyPorterEnemy _enemy;

		public override void InstallBindings()
		{
			base.Container.Bind<MonkeyPorterSettings>().FromInstance(_settings).AsSingle();
			base.Container.Bind<MonkeyPorterContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<MonkeyPorterEnemy>().FromInstance(_enemy).AsSingle();
		}
	}
}
