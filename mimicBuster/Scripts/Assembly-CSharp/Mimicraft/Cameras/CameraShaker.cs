using Mimicraft.Settings;
using UnityEngine;

namespace Mimicraft.Cameras
{
	public class CameraShaker
	{
		private const float TraumaDecayPerSecond = 2.2f;

		private const float MaxPositionOffset = 0.06f;

		private const float MaxRotationDegrees = 3.5f;

		private const float NoiseFrequency = 22f;

		private const float seedX = 0f;

		private const float seedY = 37.21f;

		private const float seedZ = 91.83f;

		private float trauma;

		public void AddTrauma(float amount)
		{
			trauma = Mathf.Clamp01(trauma + amount);
		}

		public void Tick(float deltaTime, out Vector3 positionOffset, out Vector3 rotationEulerOffset)
		{
			trauma = Mathf.Max(0f, trauma - 2.2f * deltaTime);
			float num = trauma * trauma * GameSettings.ScreenShake;
			float y = Time.time * 22f;
			positionOffset = new Vector3((Mathf.PerlinNoise(0f, y) * 2f - 1f) * 0.06f * num, (Mathf.PerlinNoise(37.21f, y) * 2f - 1f) * 0.06f * num, 0f);
			rotationEulerOffset = new Vector3(0f, 0f, (Mathf.PerlinNoise(91.83f, y) * 2f - 1f) * 3.5f * num);
		}
	}
}
