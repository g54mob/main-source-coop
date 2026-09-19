using Features.ItemsModule.Scripts;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings
{
	[CreateAssetMenu(fileName = "MonkeyEnemySettings_Default", menuName = "Configurations/AIModuleStateMachine/Monkey/MonkeyEnemySettings")]
	public class MonkeyEnemySettings : ScriptableObject
	{
		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float StartHungerPercent { get; private set; } = 0.2f;

		[field: SerializeField]
		public float MaxHunger { get; private set; } = 100f;

		[field: SerializeField]
		public float HungerIncreaseRate { get; private set; } = 2f;

		[field: SerializeField]
		public float FullHungerThreshold { get; private set; } = 40f;

		[field: SerializeField]
		public float StarvingHungerThreshold { get; private set; } = 100f;

		[field: SerializeField]
		public ItemType PreferredItemType { get; private set; } = ItemType.Coin;

		[field: SerializeField]
		public float ForceStrength { get; private set; } = 20f;

		[field: SerializeField]
		public float SpeedAnimationLerpSpeed { get; private set; } = 5f;
	}
}
