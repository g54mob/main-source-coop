using System;

namespace Photon.Voice.Unity.UtilityScripts
{
	public class MicrophoneValue : IProcessor<short>, IDisposable
	{
		private int m_samplingRate;

		public bool Disabled { get; set; }

		public MicrophoneValue(int SamlpingRate)
		{
			m_samplingRate = SamlpingRate;
		}

		public short[] Process(short[] buf)
		{
			return buf;
		}

		public void Dispose()
		{
		}
	}
}
