using Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Settings;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy
{
	public class HeadcrabEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private HeadcrabEnemySettings _enemySettings;

		[SerializeField]
		private HeadcrabChaseSettings _chaseSettings;

		[SerializeField]
		private HeadcrabAttachSettings _attachSettings;

		[SerializeField]
		private HeadcrabEnemyContext _context;

		[SerializeField]
		private HeadcrabEnemy _enemy;

		public override void InstallBindings()
		{
			base.Container.Bind<HeadcrabEnemySettings>().FromInstance(_enemySettings).AsSingle();
			base.Container.Bind<HeadcrabChaseSettings>().FromInstance(_chaseSettings).AsSingle();
			base.Container.Bind<HeadcrabAttachSettings>().FromInstance(_attachSettings).AsSingle();
			base.Container.Bind<HeadcrabEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<HeadcrabEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<HeadcrabTargetSensor>().AsSingle();
			base.Container.Bind<HeadcrabResnapSensor>().AsSingle();
		}
	}
}
