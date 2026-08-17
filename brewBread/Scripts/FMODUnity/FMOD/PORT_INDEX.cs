using System.Runtime.InteropServices;

namespace FMOD
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct PORT_INDEX
	{
		public const ulong NONE = ulong.MaxValue;

		public const ulong FLAG_VR_CONTROLLER = 1152921504606846976uL;
	}
}
