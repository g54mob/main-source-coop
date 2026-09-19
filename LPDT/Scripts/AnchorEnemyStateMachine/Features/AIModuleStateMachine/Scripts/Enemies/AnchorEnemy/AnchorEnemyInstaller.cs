using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.States;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	public class AnchorEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private AnchorEnemyContext _context;

		[SerializeField]
		private AnchorEnemy _enemy;

		public override void InstallBindings()
		{
			base.Container.BindInterfacesAndSelfTo<AnchorEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<AnchorEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<ICurrentStateProvider<AnchorStateId>>().FromInstance(_enemy).AsSingle();
		}
	}
}
