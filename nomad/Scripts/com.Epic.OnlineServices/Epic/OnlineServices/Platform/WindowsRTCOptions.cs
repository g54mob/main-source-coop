using System;

namespace Epic.OnlineServices.Platform
{
	public struct WindowsRTCOptions
	{
		public WindowsRTCOptionsPlatformSpecificOptions? PlatformSpecificOptions { get; set; }

		public RTCBackgroundMode BackgroundMode { get; }

		public IntPtr Reserved { get; }
	}
}
