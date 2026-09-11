using System;

namespace Photon.Voice.Unity
{
	[Serializable]
	public struct AndroidAudioInParameters
	{
		public bool EnableAEC;

		public bool EnableAGC;

		public bool EnableNS;

		public static AndroidAudioInParameters Default = new AndroidAudioInParameters
		{
			EnableAEC = true,
			EnableAGC = true,
			EnableNS = true
		};
	}
}
