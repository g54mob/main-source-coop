using System;

namespace Rewired.Libraries.SharpDX.RawInput
{
	public delegate void ForwardRawInputEventsToUnityDelegate(IntPtr rawInputHeaderIndices, IntPtr rawInputDataIndices, uint indicesCount, IntPtr rawInputData, uint rawInputDataSize);
}
