using UnityEngine;

namespace Mimicraft.Cameras
{
	public class AimPunchSpring
	{
		private const float Damping = 9f;

		private const float SpringConstant = 65f;

		private const float ImpulseScale = 20f;

		private const float RestThreshold = 0.001f;

		private Vector2 punch;

		private Vector2 velocity;

		public Vector2 Value => punch;

		public void Add(Vector2 degrees)
		{
			velocity += degrees * 20f;
		}

		public void Clear()
		{
			punch = Vector2.zero;
			velocity = Vector2.zero;
		}

		public void Tick(float deltaTime)
		{
			if (punch.sqrMagnitude <= 0.001f && velocity.sqrMagnitude <= 0.001f)
			{
				punch = Vector2.zero;
				velocity = Vector2.zero;
				return;
			}
			punch += velocity * deltaTime;
			float num = Mathf.Max(0f, 1f - 9f * deltaTime);
			velocity *= num;
			float num2 = Mathf.Clamp(65f * deltaTime, 0f, 2f);
			velocity -= punch * num2;
		}
	}
}
