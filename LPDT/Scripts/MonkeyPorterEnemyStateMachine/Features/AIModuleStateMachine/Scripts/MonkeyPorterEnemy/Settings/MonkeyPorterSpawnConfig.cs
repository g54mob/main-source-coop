using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings
{
	[CreateAssetMenu(fileName = "MonkeyPorterSpawnConfig_Default", menuName = "Configurations/AIModuleStateMachine/MonkeyPorter/MonkeyPorterSpawnConfig")]
	public class MonkeyPorterSpawnConfig : ScriptableObject
	{
		[field: SerializeField]
		public NetworkPrefabRef MonkeyPrefab { get; private set; }

		[field: SerializeField]
		public NetworkPrefabRef CartPrefab { get; private set; }

		[Tooltip("Debug only. Spawns the porter on every level regardless of whether it was bought in the shop.")]
		[field: SerializeField]
		public bool AlwaysSpawnWithoutPurchase { get; private set; }
	}
}
