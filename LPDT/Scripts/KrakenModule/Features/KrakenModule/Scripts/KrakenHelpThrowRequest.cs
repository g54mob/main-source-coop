using Fusion;
using UnityEngine;

namespace Features.KrakenModule.Scripts
{
	public readonly struct KrakenHelpThrowRequest
	{
		public PlayerRef AliveTarget { get; }

		public Vector3 TargetPosition { get; }

		public Vector3 SpawnPosition { get; }

		public KrakenHelpThrowRequest(PlayerRef aliveTarget, Vector3 targetPosition, Vector3 spawnPosition)
		{
			AliveTarget = aliveTarget;
			TargetPosition = targetPosition;
			SpawnPosition = spawnPosition;
		}
	}
}
