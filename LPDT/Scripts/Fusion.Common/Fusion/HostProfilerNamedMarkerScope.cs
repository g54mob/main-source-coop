using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	[Obsolete("No longer used")]
	public struct HostProfilerNamedMarkerScope : IDisposable
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
		}
	}
}
