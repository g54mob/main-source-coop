using Fusion;
using UnityEngine;

namespace Features.KrakenModule.Scripts.Data
{
	public readonly struct KrakenDeadPartThrowTarget
	{
		public PlayerRef PlayerRef { get; }

		public Vector3 TargetPosition { get; }

		public KrakenDeadPartThrowTarget(PlayerRef playerRef, Vector3 targetPosition)
		{
			PlayerRef = playerRef;
			TargetPosition = targetPosition;
		}
	}
}
