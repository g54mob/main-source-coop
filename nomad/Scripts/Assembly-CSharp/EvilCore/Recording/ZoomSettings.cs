using System;

namespace EvilCore.Recording
{
	[Serializable]
	public class ZoomSettings
	{
		public float startFOV = 60f;

		public float endFOV = 20f;

		public float duration = 3f;

		public float distance = 8f;

		public float height = 3f;

		public float angle;

		public float followSmoothTime = 0.3f;

		public float lookAtSmoothTime = 0.1f;

		public float lookAtHeightOffset = 1f;

		public bool lookAtTarget = true;

		public float shakeAmplitude;

		public float shakeFrequency = 0.5f;
	}
}
