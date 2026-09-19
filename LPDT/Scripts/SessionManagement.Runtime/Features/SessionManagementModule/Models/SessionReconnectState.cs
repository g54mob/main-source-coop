using UnityEngine;

namespace Features.SessionManagementModule.Models
{
	public readonly struct SessionReconnectState
	{
		public readonly bool HasValue;

		public readonly int LevelId;

		public readonly Vector3 Position;

		public readonly float Yaw;

		public readonly bool IsCrouching;

		public readonly float Health;

		public SessionReconnectState(int levelId, Vector3 position, float yaw, bool isCrouching, float health)
		{
			HasValue = true;
			LevelId = levelId;
			Position = position;
			Yaw = yaw;
			IsCrouching = isCrouching;
			Health = health;
		}
	}
}
