using UnityEngine;

namespace Features.MultiplayerSessionServices.Scripts
{
	[CreateAssetMenu(fileName = "MultiplayerSessionConfig_Default", menuName = "Configurations/NetworkServices/MultiplayerSessionConfig")]
	public class MultiplayerSessionConfig : ScriptableObject
	{
		[field: SerializeField]
		[field: Min(1f)]
		public int RoomCodeLength { get; private set; } = 5;

		[field: SerializeField]
		[field: Range(2f, 255f)]
		public int MaxPlayersInRoom { get; private set; } = 4;

		[field: SerializeField]
		public GameObject NetworkRunnerPrefab { get; private set; }

		[field: SerializeField]
		[field: Min(1f)]
		public int PlayerNameMaxLength { get; private set; } = 10;

		[field: SerializeField]
		[field: Min(1f)]
		public int MaxRegionPing { get; private set; } = 100;

		[field: SerializeField]
		public bool UseCustomEnvironment { get; private set; }

		[field: SerializeField]
		public string CustomEnvironment { get; private set; }
	}
}
