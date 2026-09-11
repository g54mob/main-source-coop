using System;

namespace Photon.Voice.Unity.UtilityScripts
{
	public class MicAmplifierFloat : IProcessor<float>, IDisposable
	{
		public float AmplificationFactor { get; set; }

		public bool Disabled { get; set; }

		public MicAmplifierFloat(float amplificationFactor)
		{
			AmplificationFactor = amplificationFactor;
		}

		public float[] Process(float[] buf)
		{
			if (Disabled)
			{
				return buf;
			}
			for (int i = 0; i < buf.Length; i++)
			{
				buf[i] *= AmplificationFactor;
			}
			return buf;
		}

		public void Dispose()
		{
		}
	}
}
