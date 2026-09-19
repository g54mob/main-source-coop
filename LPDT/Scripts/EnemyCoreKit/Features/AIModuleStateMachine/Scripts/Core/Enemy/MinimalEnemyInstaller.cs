using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Enemy
{
	public class MinimalEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private EnemyContextBase _context;

		[SerializeField]
		private MinimalEnemy _enemy;

		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<EnemyContextBase>().FromInstance(_context).AsSingle();
			base.Container.Bind<MinimalEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<ICurrentStateProvider<SimpleEnemyStateId>>().FromInstance(_enemy).AsSingle();
		}
	}
}
