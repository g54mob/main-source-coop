using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial
{
	public class PirateGameObjectInstaller : MonoInstaller
	{
		[SerializeField]
		private PirateEnemy _pirateEnemyPrefab;

		[SerializeField]
		private PirateEnemyContext _pirateEnemyContextPrefab;

		public override void InstallBindings()
		{
			base.Container.Bind<PirateEnemy>().FromInstance(_pirateEnemyPrefab).AsSingle();
			base.Container.Bind<PirateEnemyContext>().FromInstance(_pirateEnemyContextPrefab).AsSingle();
		}
	}
}
