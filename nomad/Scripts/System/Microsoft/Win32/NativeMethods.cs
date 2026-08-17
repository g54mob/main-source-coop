using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Win32
{
	internal static class NativeMethods
	{
		public static bool DuplicateHandle(HandleRef hSourceProcessHandle, SafeHandle hSourceHandle, HandleRef hTargetProcess, out SafeWaitHandle targetHandle, int dwDesiredAccess, bool bInheritHandle, int dwOptions)
		{
			bool success = false;
			try
			{
				hSourceHandle.DangerousAddRef(ref success);
				IntPtr target_handle;
				MonoIOError error;
				bool result = MonoIO.DuplicateHandle(hSourceProcessHandle.Handle, hSourceHandle.DangerousGetHandle(), hTargetProcess.Handle, out target_handle, dwDesiredAccess, bInheritHandle ? 1 : 0, dwOptions, out error);
				if (error != MonoIOError.ERROR_SUCCESS)
				{
					throw MonoIO.GetException(error);
				}
				targetHandle = new SafeWaitHandle(target_handle, ownsHandle: true);
				return result;
			}
			finally
			{
				if (success)
				{
					hSourceHandle.DangerousRelease();
				}
			}
		}

		public static bool DuplicateHandle(HandleRef hSourceProcessHandle, HandleRef hSourceHandle, HandleRef hTargetProcess, out SafeProcessHandle targetHandle, int dwDesiredAccess, bool bInheritHandle, int dwOptions)
		{
			IntPtr target_handle;
			MonoIOError error;
			bool result = MonoIO.DuplicateHandle(hSourceProcessHandle.Handle, hSourceHandle.Handle, hTargetProcess.Handle, out target_handle, dwDesiredAccess, bInheritHandle ? 1 : 0, dwOptions, out error);
			if (error != MonoIOError.ERROR_SUCCESS)
			{
				throw MonoIO.GetException(error);
			}
			targetHandle = new SafeProcessHandle(target_handle, ownsHandle: true);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetCurrentProcess();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetExitCodeProcess(IntPtr processHandle, out int exitCode);

		public static bool GetExitCodeProcess(SafeProcessHandle processHandle, out int exitCode)
		{
			bool success = false;
			try
			{
				processHandle.DangerousAddRef(ref success);
				return GetExitCodeProcess(processHandle.DangerousGetHandle(), out exitCode);
			}
			finally
			{
				if (success)
				{
					processHandle.DangerousRelease();
				}
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool TerminateProcess(IntPtr processHandle, int exitCode);

		public static bool TerminateProcess(SafeProcessHandle processHandle, int exitCode)
		{
			bool success = false;
			try
			{
				processHandle.DangerousAddRef(ref success);
				return TerminateProcess(processHandle.DangerousGetHandle(), exitCode);
			}
			finally
			{
				if (success)
				{
					processHandle.DangerousRelease();
				}
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetCurrentProcessId();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CloseProcess(IntPtr handle);
	}
}
