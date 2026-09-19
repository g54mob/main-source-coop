using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Settings;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy
{
	public class SleeperEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private SleeperEnemyContext _context;

		[SerializeField]
		private SleeperEnemy _enemy;

		[SerializeField]
		private SleeperEnemySettings _settings;

		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<SleeperEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<SleeperEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<ICurrentStateProvider<SleeperStateId>>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<SleeperEnemySettings>().FromInstance(_settings).AsSingle();
		}
	}
}
