using System;

namespace Photon.Voice.Unity.UtilityScripts
{
	public class MicAmplifierShort : IProcessor<short>, IDisposable
	{
		public float AmplificationFactor { get; set; }

		public bool Disabled { get; set; }

		public MicAmplifierShort(float amplificationFactor)
		{
			AmplificationFactor = amplificationFactor;
		}

		public short[] Process(short[] buf)
		{
			if (Disabled)
			{
				return buf;
			}
			for (int i = 0; i < buf.Length; i++)
			{
				buf[i] = (short)((float)buf[i] * AmplificationFactor);
			}
			return buf;
		}

		public void Dispose()
		{
		}
	}
}
