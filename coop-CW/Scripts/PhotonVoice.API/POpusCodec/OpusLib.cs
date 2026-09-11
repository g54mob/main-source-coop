using System.Runtime.InteropServices;

namespace POpusCodec
{
	public static class OpusLib
	{
		public static string Version
		{
			get
			{
				string text = Marshal.PtrToStringAnsi(Wrapper.opus_get_version_string());
				return ((text == null || text == "") ? "?" : text) ?? "";
			}
		}
	}
}
