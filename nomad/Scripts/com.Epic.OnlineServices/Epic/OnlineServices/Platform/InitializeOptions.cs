using System;

namespace Epic.OnlineServices.Platform
{
	public struct InitializeOptions
	{
		public IntPtr AllocateMemoryFunction { get; set; }

		public IntPtr ReallocateMemoryFunction { get; set; }

		public IntPtr ReleaseMemoryFunction { get; set; }

		public Utf8String ProductName { get; set; }

		public Utf8String ProductVersion { get; set; }

		public IntPtr Reserved { get; }

		public IntPtr SystemInitializeOptions { get; }

		public InitializeThreadAffinity? OverrideThreadAffinity { get; set; }
	}
}
