using Features.AIModuleStateMachine.Scripts.Core;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.PlayerTutorialGuideEnemy
{
	public class PlayerTutorialGuideEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private PlayerTutorialGuideEnemyContext _context;

		[SerializeField]
		private PlayerTutorialGuideEnemy _enemy;

		[SerializeField]
		private EnemyStatsConfiguration _enemyStatsConfiguration;

		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<PlayerTutorialGuideEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<PlayerTutorialGuideEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<ICurrentStateProvider<PlayerTutorialGuideStateId>>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<EnemyStatsConfiguration>().FromInstance(_enemyStatsConfiguration).AsSingle();
		}
	}
}
