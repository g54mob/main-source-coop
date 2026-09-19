using System;
using UnityEngine;

namespace Features.ScreenShakeModule.Scripts
{
	public class CameraLocalScreenShakeModel
	{
		private float _screenShakeIntensityNormalized;

		public float ScreenShakeIntensityNormalized
		{
			get
			{
				return _screenShakeIntensityNormalized;
			}
			set
			{
				_screenShakeIntensityNormalized = Mathf.Clamp01(value);
				this.OnScreenShakeIntensityChanged?.Invoke(_screenShakeIntensityNormalized);
			}
		}

		public event Action<ScreenShakeData> OnScreenShakeTriggered;

		public event Action<float> OnScreenShakeIntensityChanged;

		public void TriggerScreenShake(ScreenShakeData screenShakeData)
		{
			this.OnScreenShakeTriggered?.Invoke(screenShakeData);
		}
	}
}
