using System;
using Features.DeadPartsModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core
{
	[Serializable]
	public class EnemyLootEntry
	{
		[Tooltip("Relative chance of this entry inside the configuration. Weights do not have to sum to 1.")]
		[Min(0f)]
		public float Weight = 1f;

		public EnemyLootKind Kind = EnemyLootKind.Item;

		public NetworkBehaviour Item;

		[Min(1f)]
		public int ItemCount = 1;

		public DeadPartType DeadPartType = DeadPartType.DefaultButt;

		[Tooltip("Charges already spent on the spawned dead part. 1 means a fresh part with one use consumed.")]
		[Min(0f)]
		public int DeadPartUsageCount = 1;

		[Tooltip("Spreads items around the spawn point and adds a random spawn impulse (dead parts only get the impulse).")]
		public bool UseSpread;

		public bool IsValid => Kind switch
		{
			EnemyLootKind.Item => Item != null && ItemCount > 0, 
			EnemyLootKind.DeadPart => DeadPartType != DeadPartType.None, 
			_ => false, 
		};
	}
}
