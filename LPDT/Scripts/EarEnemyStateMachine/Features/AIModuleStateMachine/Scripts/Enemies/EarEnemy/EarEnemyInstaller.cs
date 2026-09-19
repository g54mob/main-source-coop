using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Settings;
using Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.States;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy
{
	public class EarEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private EarEnemyContext _context;

		[SerializeField]
		private EarEnemy _enemy;

		[SerializeField]
		private EarEnemySettings _settings;

		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<EarEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<EarEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<ICurrentStateProvider<EarStateId>>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<EarEnemySettings>().FromInstance(_settings).AsSingle();
			base.Container.Bind<EarSoundDestinationResolver>().AsSingle();
			base.Container.Bind<PlayerSoundSourceGateService>().AsSingle();
		}
	}
}
