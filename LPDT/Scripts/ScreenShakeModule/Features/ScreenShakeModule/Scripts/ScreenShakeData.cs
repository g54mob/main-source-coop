using System;

namespace Features.ScreenShakeModule.Scripts
{
	[Serializable]
	public class ScreenShakeData
	{
		public float Force;

		public float InTime;

		public float OutTime;

		public float FrequencyGain;

		public ScreenShakeData(float force, float inTime, float outTime, float frequencyGain)
		{
			Force = force;
			InTime = inTime;
			OutTime = outTime;
			FrequencyGain = frequencyGain;
		}

		public ScreenShakeData(ScreenShakeData screenShakeData)
		{
			Force = screenShakeData.Force;
			InTime = screenShakeData.InTime;
			OutTime = screenShakeData.OutTime;
			FrequencyGain = screenShakeData.FrequencyGain;
		}
	}
}
