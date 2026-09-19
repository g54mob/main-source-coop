#define DEBUG
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Fusion
{
	internal class NativeDynamicLibrary : SafeHandleZeroOrMinusOneIsInvalid
	{
		private static class WindowsNative
		{
			private const string DllName = "kernel32";

			[DllImport("kernel32", SetLastError = true)]
			public static extern IntPtr LoadLibrary(string fileName);

			[DllImport("kernel32", SetLastError = true)]
			public static extern IntPtr GetProcAddress(IntPtr module, string procName);

			[DllImport("kernel32", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool FreeLibrary(IntPtr module);
		}

		private static class UnixNative
		{
			private const string DllName = "libdl";

			public const int RTLD_LAZY = 1;

			public const int RTLD_NOW = 2;

			[DllImport("libdl", SetLastError = true)]
			public static extern IntPtr dlopen(string fileName, int flags);

			[DllImport("libdl", SetLastError = true)]
			public static extern IntPtr dlsym(IntPtr handle, string symbol);

			[DllImport("libdl", SetLastError = true)]
			public static extern int dlclose(IntPtr handle);

			[DllImport("libdl", SetLastError = true)]
			public static extern IntPtr dlerror();
		}

		public static bool TryLoadByPath(string path, out NativeDynamicLibrary library)
		{
			library = null;
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				return TryLoadWindows(path, out library);
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				return TryLoadUnix(path, out library);
			}
			return TryLoadUnix(path, out library);
		}

		public static bool TryLoadByName(string nameWithoutExtension, out NativeDynamicLibrary library)
		{
			library = null;
			if (string.IsNullOrEmpty(nameWithoutExtension))
			{
				return false;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				string loadedPath = FusionPlatform.GetLoadedPath(Assembly.GetExecutingAssembly());
				string directoryName = Path.GetDirectoryName(loadedPath);
				if (TryLoadWindows(directoryName + "/" + nameWithoutExtension + ".dll", out library))
				{
					return true;
				}
				if (TryLoadWindows(nameWithoutExtension + ".dll", out library))
				{
					return true;
				}
				if (TryLoadWindows(nameWithoutExtension ?? "", out library))
				{
					return true;
				}
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				if (TryLoadUnix("lib" + nameWithoutExtension + ".dylib", out library))
				{
					return true;
				}
				if (TryLoadUnix(nameWithoutExtension + ".dylib", out library))
				{
					return true;
				}
			}
			else
			{
				if (TryLoadUnix("lib" + nameWithoutExtension + ".so", out library))
				{
					return true;
				}
				if (TryLoadUnix(nameWithoutExtension + ".so", out library))
				{
					return true;
				}
			}
			library = null;
			return false;
		}

		private static bool TryLoadWindows(string path, out NativeDynamicLibrary library)
		{
			IntPtr intPtr = LoadWindowsLibrary(path);
			if (intPtr != IntPtr.Zero)
			{
				library = new NativeDynamicLibrary(intPtr);
				return true;
			}
			library = null;
			return false;
		}

		private static bool TryLoadUnix(string path, out NativeDynamicLibrary library)
		{
			IntPtr intPtr = LoadUnixLibrary(path);
			if (intPtr != IntPtr.Zero)
			{
				library = new NativeDynamicLibrary(intPtr);
				return true;
			}
			library = null;
			return false;
		}

		private static IntPtr LoadWindowsLibrary(string path)
		{
			return WindowsNative.LoadLibrary(path);
		}

		private static IntPtr LoadUnixLibrary(string path)
		{
			return UnixNative.dlopen(path, 2);
		}

		private NativeDynamicLibrary(IntPtr ptr)
			: base(ownsHandle: true)
		{
			if (ptr == IntPtr.Zero)
			{
				throw new ArgumentOutOfRangeException("ptr");
			}
			SetHandle(ptr);
		}

		[return: NotNull]
		public T CreateDelegate<T>([NotNull] string name) where T : Delegate
		{
			return (T)CreateDelegate(name, typeof(T));
		}

		[return: NotNull]
		public Delegate CreateDelegate([NotNull] string name, [NotNull] Type delegateType)
		{
			Assert.Check(!IsInvalid, "!IsInvalid");
			Assert.Check(!base.IsClosed, "!IsClosed");
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentOutOfRangeException("name");
			}
			IntPtr functionPointer = GetFunctionPointer(name);
			if (functionPointer == IntPtr.Zero)
			{
				throw new InvalidOperationException("No function was found with the name " + name + ".");
			}
			Delegate delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer(functionPointer, delegateType);
			if ((object)delegateForFunctionPointer == null)
			{
				throw new InvalidOperationException("Failed to create a delegate for " + name);
			}
			return delegateForFunctionPointer;
		}

		public IntPtr GetFunctionPointer(string functionName)
		{
			return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? GetWindowsFunctionPointer(functionName) : GetUnixFunctionPointer(functionName);
		}

		private IntPtr GetWindowsFunctionPointer(string functionName)
		{
			return WindowsNative.GetProcAddress(handle, functionName);
		}

		private IntPtr GetUnixFunctionPointer(string functionName)
		{
			UnixNative.dlerror();
			return UnixNative.dlsym(handle, functionName);
		}

		protected sealed override bool ReleaseHandle()
		{
			Assert.Check(!IsInvalid, "!IsInvalid");
			FreeLibrary();
			return true;
		}

		private void FreeLibrary()
		{
			Assert.Check(!IsInvalid, "!IsInvalid");
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				FreeWindowsLibrary();
			}
			else
			{
				FreeUnixLibrary();
			}
		}

		private void FreeWindowsLibrary()
		{
			if (!WindowsNative.FreeLibrary(handle))
			{
				throw new InvalidOperationException($"Failed to free native library with error {Marshal.GetLastWin32Error()}");
			}
		}

		private void FreeUnixLibrary()
		{
			if (UnixNative.dlclose(handle) != 0)
			{
				throw new InvalidOperationException("Failed to free native library with error " + Marshal.PtrToStringAnsi(UnixNative.dlerror()));
			}
		}
	}
}
