using System;
using System.Runtime.CompilerServices;
using Unity.Profiling.LowLevel.Unsafe;

namespace Fusion
{
	public readonly struct HostProfilerMarker
	{
		private readonly IntPtr _handle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public HostProfilerMarkerScope Begin()
		{
			return Start();
		}

		internal HostProfilerMarker(IntPtr handle)
		{
			_handle = handle;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public HostProfilerMarkerScope Start()
		{
			if (_handle != (IntPtr)0)
			{
				ProfilerUnsafeUtility.BeginSample(_handle);
			}
			return new HostProfilerMarkerScope(this);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void End()
		{
			if (_handle != (IntPtr)0)
			{
				ProfilerUnsafeUtility.EndSample(_handle);
			}
		}
	}
}
