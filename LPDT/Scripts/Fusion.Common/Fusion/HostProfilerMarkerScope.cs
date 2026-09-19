using System;
using System.Runtime.CompilerServices;

namespace Fusion
{
	public readonly struct HostProfilerMarkerScope : IDisposable
	{
		public HostProfilerMarkerScope(HostProfilerMarker marker)
		{
			_003Cmarker_003EP = marker;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			_003Cmarker_003EP.End();
		}
	}
}
