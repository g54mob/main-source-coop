using System;
using UnityEngine;

namespace Features.PostProcessingModule.Scripts
{
	public class FlickerAdjustingModel
	{
		public float CurrentBlend { get; private set; }

		public event Action<float> OnBlendChanged;

		public void ChangeBlend(float blend)
		{
			float num = Mathf.Clamp01(blend);
			if (!Mathf.Approximately(CurrentBlend, num))
			{
				CurrentBlend = num;
				this.OnBlendChanged?.Invoke(CurrentBlend);
			}
		}
	}
}
