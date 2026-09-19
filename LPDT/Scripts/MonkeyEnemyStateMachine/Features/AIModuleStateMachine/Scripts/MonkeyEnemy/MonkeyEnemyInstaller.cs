using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Sensors;
using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy
{
	public class MonkeyEnemyInstaller : MonoInstaller
	{
		[SerializeField]
		private MonkeyEnemySettings _enemySettings;

		[SerializeField]
		private MonkeyMovementSettings _movementSettings;

		[SerializeField]
		private MonkeyPlayerInteractionSettings _playerInteractionSettings;

		[SerializeField]
		private MonkeyCombatSettings _combatSettings;

		[SerializeField]
		private MonkeyItemInteractionSettings _itemInteractionSettings;

		[SerializeField]
		private MonkeyItemGiftSettings _itemGiftSettings;

		[SerializeField]
		private MonkeyFearSettings _fearSettings;

		[SerializeField]
		private MonkeyStunSettings _stunSettings;

		[SerializeField]
		private MonkeyCauldronSettings _cauldronSettings;

		[SerializeField]
		private MonkeyLastChanceSettings _lastChanceSettings;

		[SerializeField]
		private MonkeyEnemyContext _context;

		[SerializeField]
		private MonkeyEnemy _enemy;

		public override void InstallBindings()
		{
			base.Container.Bind<MonkeyEnemySettings>().FromInstance(_enemySettings).AsSingle();
			base.Container.Bind<MonkeyMovementSettings>().FromInstance(_movementSettings).AsSingle();
			base.Container.Bind<MonkeyPlayerInteractionSettings>().FromInstance(_playerInteractionSettings).AsSingle();
			base.Container.Bind<MonkeyCombatSettings>().FromInstance(_combatSettings).AsSingle();
			base.Container.Bind<MonkeyItemInteractionSettings>().FromInstance(_itemInteractionSettings).AsSingle();
			base.Container.Bind<MonkeyItemGiftSettings>().FromInstance(_itemGiftSettings).AsSingle();
			base.Container.Bind<MonkeyFearSettings>().FromInstance(_fearSettings).AsSingle();
			base.Container.Bind<MonkeyStunSettings>().FromInstance(_stunSettings).AsSingle();
			base.Container.Bind<MonkeyCauldronSettings>().FromInstance((_cauldronSettings != null) ? _cauldronSettings : ScriptableObject.CreateInstance<MonkeyCauldronSettings>()).AsSingle();
			base.Container.Bind<MonkeyLastChanceSettings>().FromInstance(_lastChanceSettings).AsSingle();
			base.Container.Bind<MonkeyEnemyContext>().FromInstance(_context).AsSingle();
			base.Container.Bind<MonkeyEnemy>().FromInstance(_enemy).AsSingle();
			base.Container.Bind<MonkeyItemClaimModel>().AsSingle();
			base.Container.Bind<MonkeyItemClaimService>().AsSingle();
			base.Container.Bind<MonkeyItemSensor>().AsSingle();
		}
	}
}
