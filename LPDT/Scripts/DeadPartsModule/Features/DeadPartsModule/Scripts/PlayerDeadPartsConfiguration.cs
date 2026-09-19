using Fusion;
using UnityEngine;

namespace Features.DeadPartsModule.Scripts
{
	[CreateAssetMenu(fileName = "PlayerDeadPartsConfiguration_Default", menuName = "Configurations/Player/PlayerDeadPartsConfiguration")]
	public class PlayerDeadPartsConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<DeadPartType, NetworkPrefabRef> ButtPool { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<DeadPartType, int> MaxDeadPartUsageCount { get; private set; }

		[field: SerializeField]
		public DeadPartType InitialDeadPartType { get; private set; }

		[field: SerializeField]
		public float PlayerDeadPartSpawnForce { get; private set; }

		[field: SerializeField]
		public float PlayerDeadPartSpawnTorque { get; private set; }

		[Header("Resurrection Settings")]
		[field: SerializeField]
		public float ResurrectionMinTime { get; private set; }

		[field: SerializeField]
		public float TargetResurrectionDistance { get; private set; }

		[field: SerializeField]
		public float ResurrectionMaxDistance { get; private set; }

		[field: SerializeField]
		public float ResurrectionMaxAngle { get; private set; }
	}
}
