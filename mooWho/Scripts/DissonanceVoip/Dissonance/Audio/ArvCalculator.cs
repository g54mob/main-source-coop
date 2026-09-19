using System;

namespace Dissonance.Audio
{
	internal struct ArvCalculator
	{
		public float ARV { get; private set; }

		public void Reset()
		{
			ARV = 0f;
		}

		public void Update(ReadOnlySpan<float> samples)
		{
			float num = 0f;
			for (int i = 0; i < samples.Length; i++)
			{
				num += Math.Abs(samples[i]);
			}
			ARV = num / (float)Math.Max(1, samples.Length);
		}
	}
}
