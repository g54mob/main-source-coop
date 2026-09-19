using System;
using UnityEngine;

namespace Features.CameraModuleRotation.Scripts
{
	public class HeadBobbingModel
	{
		private float _headBobbingIntensityNormalized;

		public float HeadBobbingIntensityNormalized
		{
			get
			{
				return _headBobbingIntensityNormalized;
			}
			set
			{
				_headBobbingIntensityNormalized = Mathf.Clamp01(value);
				this.OnHeadBobbingIntensityChanged?.Invoke(_headBobbingIntensityNormalized);
			}
		}

		public event Action<float> OnHeadBobbingIntensityChanged;
	}
}
