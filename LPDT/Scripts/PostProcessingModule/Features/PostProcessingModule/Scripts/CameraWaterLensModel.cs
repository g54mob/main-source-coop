using System;
using Features.GameCycle.Scripts.SessionCleanup;
using UnityEngine;

namespace Features.PostProcessingModule.Scripts
{
	public class CameraWaterLensModel : ISessionCleanup
	{
		public const float MaxWetness = 1f;

		public float Wetness { get; private set; }

		public Transform LocalCameraTransform { get; private set; }

		public event Action<float> OnWetnessChanged;

		public void RegisterLocalCamera(Transform cameraTransform)
		{
			LocalCameraTransform = cameraTransform;
		}

		public void AddWetness(float amount)
		{
			if (!(amount <= 0f))
			{
				float num = Mathf.Clamp(Wetness + amount, 0f, 1f);
				if (!Mathf.Approximately(num, Wetness))
				{
					Wetness = num;
					this.OnWetnessChanged?.Invoke(Wetness);
				}
			}
		}

		public void Decay(float deltaTime, float decayPerSecond)
		{
			if (!(Wetness <= 0f))
			{
				float num = Mathf.Max(0f, Wetness - decayPerSecond * deltaTime);
				if (!Mathf.Approximately(num, Wetness))
				{
					Wetness = num;
					this.OnWetnessChanged?.Invoke(Wetness);
				}
			}
		}

		public void Cleanup()
		{
			LocalCameraTransform = null;
			if (!(Wetness <= 0f))
			{
				Wetness = 0f;
				this.OnWetnessChanged?.Invoke(Wetness);
			}
		}
	}
}
