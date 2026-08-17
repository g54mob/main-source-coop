using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.Platform;

namespace Epic.OnlineServices
{
	public static class WindowsBindings
	{
		[DllImport("EOSSDK-Win64-Shipping", CallingConvention = CallingConvention.Cdecl, EntryPoint = "EOS_Platform_Create")]
		internal static extern IntPtr EOS_Platform_Create_Windows(ref WindowsOptionsInternal options);
	}
}
