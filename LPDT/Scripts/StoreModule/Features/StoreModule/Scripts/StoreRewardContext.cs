using UnityEngine;

namespace Features.StoreModule.Scripts
{
	public class StoreRewardContext
	{
		public int TargetPlayerId;

		public Vector3 SpawnPosition;

		public Quaternion SpawnRotation;

		public StoreCardData CardData;

		public Color SpawnColor = Color.white;
	}
}
