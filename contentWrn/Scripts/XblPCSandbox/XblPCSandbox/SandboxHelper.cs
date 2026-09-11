using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;

namespace XblPCSandbox
{
	internal class SandboxHelper
	{
		[Flags]
		private enum RegSAM : uint
		{
			QueryValue = 1u,
			SetValue = 2u,
			WOW64_64Key = 0x100u,
			Set64 = 0x102u,
			Query64 = 0x101u
		}

		private static class RegHive
		{
			public static UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(2147483650u);

			public static UIntPtr HKEY_CURRENT_USER = new UIntPtr(2147483649u);
		}

		private static class RegWOW6432
		{
			[DllImport("Advapi32.dll")]
			private static extern uint RegCreateKeyEx(UIntPtr hKey, string lpSubKey, uint reserved, string lpClass, uint dwOptions, uint samDesired, uint lpSecurityAttributes, out uint phkResult, out uint lpdwDisposition);

			[DllImport("Advapi32.dll")]
			private static extern uint RegCloseKey(uint hKey);

			[DllImport("Advapi32.dll")]
			public static extern uint RegQueryValueEx(uint hKey, string lpValueName, uint lpReserved, ref uint lpType, StringBuilder lpData, ref uint lpcbData);

			[DllImport("Advapi32.dll")]
			public static extern uint RegSetValueExA(uint hKey, string lpValueName, uint lpReserved, uint dwType, byte[] lpData, uint cbData);

			[DllImport("Advapi32.dll")]
			private static extern uint RegOpenKeyEx(UIntPtr hKey, string lpSubKey, uint ulOptions, uint samDesired, out uint phkResult);

			[DllImport("Advapi32.dll")]
			public static extern uint RegDeleteValueA(uint hKey, string lpValueName);

			[DllImport("Kernel32.dll")]
			public static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, StringBuilder lpBuffer, uint nSize, IntPtr arguments);

			internal static string FormatMessage(uint dwMessageId)
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				FormatMessage(4096u, IntPtr.Zero, dwMessageId, 0u, stringBuilder, 1024u, IntPtr.Zero);
				return stringBuilder.ToString();
			}

			internal static string GetRegKey(UIntPtr inHive, string inKeyName, string inPropertyName)
			{
				uint phkResult = 0u;
				uint num = 0u;
				try
				{
					num = RegCreateKeyEx(inHive, inKeyName, 0u, null, 0u, 257u, 0u, out phkResult, out var _);
					if (num != 0)
					{
						Console.WriteLine($"Create/OpenKey (Query) failed {num}: {FormatMessage(num)}");
						return null;
					}
					uint lpType = 0u;
					uint lpcbData = 1024u;
					StringBuilder stringBuilder = new StringBuilder(1024);
					num = RegQueryValueEx(phkResult, inPropertyName, 0u, ref lpType, stringBuilder, ref lpcbData);
					if (num != 0)
					{
						if (2 != num)
						{
							Console.WriteLine($"QueryKey failed {num}: {FormatMessage(num)}");
						}
						return null;
					}
					return stringBuilder.ToString();
				}
				catch (Exception ex)
				{
					Console.WriteLine("Failed to get key: " + ex.Message);
					return null;
				}
				finally
				{
					if (phkResult != 0)
					{
						num = RegCloseKey(phkResult);
						if (num != 0)
						{
							Console.WriteLine($"CloseKey (Query) failed {num}: {FormatMessage(num)}");
						}
					}
				}
			}

			internal static uint SetRegKey(UIntPtr inHive, string inKeyName, string inPropertyName, string inPropertyValue)
			{
				uint phkResult = 0u;
				uint num = 0u;
				try
				{
					num = RegCreateKeyEx(inHive, inKeyName, 0u, null, 0u, 258u, 0u, out phkResult, out var _);
					if (num != 0)
					{
						Console.WriteLine($"Create/OpenKey (Set) failed {num}: {FormatMessage(num)}");
						return num;
					}
					byte[] bytes = Encoding.Default.GetBytes(inPropertyValue);
					byte[] array = new byte[bytes.Length + 1];
					Array.Copy(bytes, 0, array, 0, bytes.Length);
					num = RegSetValueExA(phkResult, inPropertyName, 0u, 1u, array, (uint)array.Length);
					if (num != 0)
					{
						Console.WriteLine($"SetKey failed {num}: {FormatMessage(num)}");
						return num;
					}
				}
				catch (Exception arg)
				{
					Console.WriteLine($"Failed to set key: {arg}");
					return 1u;
				}
				finally
				{
					if (phkResult != 0)
					{
						num = RegCloseKey(phkResult);
						if (num != 0)
						{
							Console.WriteLine($"CloseKey (Set) failed {num}: {FormatMessage(num)}");
						}
					}
				}
				return num;
			}

			internal static uint DeleteRegValue(UIntPtr inHive, string inKeyName, string inValueName)
			{
				uint phkResult = 0u;
				uint num = 0u;
				try
				{
					num = RegOpenKeyEx(inHive, inKeyName, 0u, 258u, out phkResult);
					if (num != 0)
					{
						if (2 == num)
						{
							return 0u;
						}
						Console.WriteLine($"OpenKey (Delete) failed {num}: {FormatMessage(num)}");
						return num;
					}
					num = RegDeleteValueA(phkResult, inValueName);
					if (num != 0 && 2 != num)
					{
						Console.WriteLine($"DeleteValue failed {num}: {FormatMessage(num)}");
					}
				}
				catch (Exception arg)
				{
					Console.WriteLine($"Failed to delete key: {arg}");
					return 1u;
				}
				finally
				{
					if (phkResult != 0)
					{
						num = RegCloseKey(phkResult);
						if (num != 0)
						{
							Console.WriteLine($"CloseKey (Delete) failed {num}: {FormatMessage(num)}");
						}
					}
				}
				return num;
			}
		}

		private const string SANDBOX_REGISTRY_KEY = "SOFTWARE\\Microsoft\\XboxLive";

		private const uint FORMAT_MESSAGE_FROM_SYSTEM = 4096u;

		internal static string GetSandbox()
		{
			string text = RegWOW6432.GetRegKey(RegHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\XboxLive", "Sandbox");
			if (string.IsNullOrEmpty(text))
			{
				text = "RETAIL";
			}
			return text;
		}

		internal static bool IsValidSandbox(string sandbox)
		{
			if (string.IsNullOrEmpty(sandbox))
			{
				return false;
			}
			MatchCollection matchCollection = Regex.Matches(sandbox, "[a-zA-Z0-9.]+");
			if (matchCollection.Count > 0)
			{
				return matchCollection[0].Value.Equals(sandbox);
			}
			return false;
		}

		internal static bool SetSandbox(string sandboxId)
		{
			Console.WriteLine("Setting the Sandbox");
			if (sandboxId.Equals("RETAIL", StringComparison.CurrentCultureIgnoreCase))
			{
				if (RegWOW6432.DeleteRegValue(RegHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\XboxLive", "Sandbox") != 0)
				{
					Console.WriteLine("Unable to switch sandbox to " + sandboxId + ". Aborting.");
					return false;
				}
			}
			else if (RegWOW6432.SetRegKey(RegHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\XboxLive", "Sandbox", sandboxId) != 0)
			{
				Console.WriteLine("Unable to switch sandbox to " + sandboxId + ". Aborting.");
				return false;
			}
			try
			{
				ServiceController serviceController = new ServiceController("XblAuthManager");
				if (serviceController.Status == ServiceControllerStatus.Running)
				{
					Console.WriteLine("Stopping XblAuthManager");
					serviceController.Stop();
					serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
					Console.WriteLine("Stopped XblAuthManager");
					Console.WriteLine("Starting XblAuthManager");
					serviceController.Start();
					serviceController.WaitForStatus(ServiceControllerStatus.Running);
					Console.WriteLine("Started XblAuthManager");
				}
				ServiceController serviceController2 = new ServiceController("DiagTrack");
				if (serviceController.Status == ServiceControllerStatus.Running)
				{
					Console.WriteLine("Stopping DiagTrack");
					serviceController2.Stop();
					serviceController2.WaitForStatus(ServiceControllerStatus.Stopped);
					Console.WriteLine("Stopped DiagTrack");
					Console.WriteLine("Starting DiagTrack");
					serviceController2.Start();
					serviceController2.WaitForStatus(ServiceControllerStatus.Running);
					Console.WriteLine("Started DiagTrack");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed to restart services: " + ex.Message);
			}
			return true;
		}

		internal static void OpenAppsInSandboxMode(bool onlyLaunchIfOpen)
		{
			string sandbox = GetSandbox();
			string text = (!sandbox.Equals("RETAIL", StringComparison.CurrentCultureIgnoreCase)).ToString().ToLower();
			OpenApp("Windows Store App", "start \"\" \"ms-windows-store:sandbox?enabled=" + text + "&id=" + sandbox + "\"", killProcess: true, "WinStore.App", onlyLaunchIfOpen);
			OpenApp("Xbox App", "start \"\" \"msxbox://\"", killProcess: true, "XboxPcApp", onlyLaunchIfOpen);
		}

		private static bool OpenApp(string friendlyName, string command)
		{
			return OpenApp(friendlyName, command, killProcess: false, "", onlyLaunchIfOpen: false);
		}

		private static bool OpenApp(string friendlyName, string command, bool killProcess, string processName, bool onlyLaunchIfOpen)
		{
			try
			{
				Process[] processesByName = Process.GetProcessesByName(processName);
				if (processesByName.Length == 0 && onlyLaunchIfOpen)
				{
					return false;
				}
				Process[] array;
				if (killProcess && processesByName.Length != 0)
				{
					Console.WriteLine("Closing " + friendlyName);
					array = processesByName;
					foreach (Process process in array)
					{
						try
						{
							process.Kill();
						}
						catch (InvalidOperationException)
						{
						}
						catch (SystemException ex2)
						{
							Console.WriteLine("Failed to close the " + process.ProcessName + " process: " + ex2.Message);
							return false;
						}
					}
					Console.WriteLine("Closed " + friendlyName);
				}
				array = processesByName;
				foreach (Process process2 in array)
				{
					try
					{
						process2.WaitForExit(1000);
					}
					catch (Exception)
					{
					}
				}
				Console.WriteLine("Starting " + friendlyName);
				RunProcess(command);
				Console.WriteLine("Started " + friendlyName);
				return true;
			}
			catch (Exception ex4)
			{
				Console.WriteLine("Failed to restart app (" + friendlyName + "): " + ex4.Message);
				return false;
			}
		}

		private static void RunProcess(string command)
		{
			Process process = new Process();
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				FileName = "cmd.exe",
				Arguments = "/C " + command
			};
			process.StartInfo = startInfo;
			try
			{
				process.Start();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed to start process: " + ex.Message);
			}
		}
	}
}
