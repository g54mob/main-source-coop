using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using PlayEveryWare.EpicOnlineServices.Utility;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices
{
	public class DLLHandle : SafeHandle
	{
		public override bool IsInvalid => handle == IntPtr.Zero;

		[Conditional("ENABLE_DLLHANDLE_PRINT")]
		private static void Log(string toPrint)
		{
			Debug.Log(toPrint);
		}

		public static List<string> GetPathsToPlugins()
		{
			string item = FileSystemUtility.CombinePaths(Application.dataPath, "Plugins");
			string fullPath = FileSystemUtility.GetFullPath(FileSystemUtility.CombinePaths("Packages", "com.playeveryware.eos", "Runtime"));
			List<string> pluginPaths = new List<string>();
			pluginPaths.Add(item);
			pluginPaths.Add(fullPath);
			if (EOSManagerPlatformSpecificsSingleton.Instance != null)
			{
				EOSManagerPlatformSpecificsSingleton.Instance.AddPluginSearchPaths(ref pluginPaths);
			}
			for (int num = pluginPaths.Count - 1; num >= 0; num--)
			{
				_ = pluginPaths[num];
			}
			for (int num2 = pluginPaths.Count - 1; num2 >= 0; num2--)
			{
				if (!FileSystemUtility.DirectoryExists(pluginPaths[num2]))
				{
					pluginPaths.RemoveAt(num2);
				}
			}
			return pluginPaths;
		}

		public static string GetVersionForLibrary(string libraryName)
		{
			FileVersionInfo libraryVersionInfo = GetLibraryVersionInfo(libraryName);
			if (libraryVersionInfo != null)
			{
				return $"{libraryVersionInfo.FileMajorPart}.{libraryVersionInfo.FileMinorPart}.{libraryVersionInfo.FileBuildPart}";
			}
			return null;
		}

		public static string GetProductVersionForLibrary(string libraryName)
		{
			return GetLibraryVersionInfo(libraryName)?.ProductVersion;
		}

		private static FileVersionInfo GetLibraryVersionInfo(string libraryName)
		{
			string pathForLibrary = GetPathForLibrary(libraryName);
			if (pathForLibrary != null)
			{
				return FileVersionInfo.GetVersionInfo(pathForLibrary);
			}
			return null;
		}

		public static string GetPathForLibrary(string libraryName)
		{
			List<string> pathsToPlugins = GetPathsToPlugins();
			string text = ((EOSManagerPlatformSpecificsSingleton.Instance != null) ? EOSManagerPlatformSpecificsSingleton.Instance.GetDynamicLibraryExtension() : ".dll");
			foreach (string item in pathsToPlugins)
			{
				using IEnumerator<string> enumerator2 = FileSystemUtility.GetFileSystemEntries(item, libraryName + text).GetEnumerator();
				if (enumerator2.MoveNext())
				{
					return enumerator2.Current;
				}
			}
			return null;
		}

		public static DLLHandle LoadDynamicLibrary(string libraryName)
		{
			string pathForLibrary = GetPathForLibrary(libraryName);
			if (pathForLibrary == null)
			{
				return null;
			}
			IntPtr intPtr = SystemDynamicLibrary.Instance.LoadLibraryAtPath(pathForLibrary);
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception("Could not load dynamic library from \"" + pathForLibrary + "\".");
			}
			return new DLLHandle(intPtr);
		}

		public DLLHandle(IntPtr intPtr)
			: base(intPtr, ownsHandle: true)
		{
			SetHandle(intPtr);
		}

		protected override bool ReleaseHandle()
		{
			if (handle == IntPtr.Zero)
			{
				return true;
			}
			bool result = SystemDynamicLibrary.Instance.UnloadLibrary(handle);
			SetHandle(IntPtr.Zero);
			return result;
		}

		public Delegate LoadFunctionAsDelegate(Type functionType, string functionName)
		{
			return LoadFunctionAsDelegate(handle, functionType, functionName);
		}

		public IntPtr LoadFunctionAsIntPtr(string functionName)
		{
			return SystemDynamicLibrary.Instance.LoadFunctionWithName(handle, functionName);
		}

		public void ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(Type clazz, Type delegateType, string functionName)
		{
			ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(handle, clazz, delegateType, functionName);
		}

		private static void ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(IntPtr libraryHandle, Type clazz, Type delegateType, string functionName)
		{
			Delegate value = LoadFunctionAsDelegate(libraryHandle, delegateType, functionName);
			clazz.GetField(functionName).SetValue(null, value);
		}

		public static Delegate LoadFunctionAsDelegate(IntPtr libraryHandle, Type functionType, string functionName)
		{
			if (libraryHandle == IntPtr.Zero)
			{
				throw new Exception("libraryHandle is null");
			}
			if (functionType == null)
			{
				throw new Exception("null function type?");
			}
			IntPtr intPtr = SystemDynamicLibrary.Instance.LoadFunctionWithName(libraryHandle, functionName);
			if (intPtr == IntPtr.Zero)
			{
				throw new Exception("Function not found: " + functionName);
			}
			return Marshal.GetDelegateForFunctionPointer(intPtr, functionType);
		}
	}
}
