using System;

namespace HeathenEngineering
{
	public static class CommandLine
	{
		public static class CommonCommands
		{
			public static string SteamLobbyConnect = "+connect_lobby";

			public static string SteamStartWindowed = "-windowed";

			public static string SteamAutoConfig = "-autoconfig";

			public static string UnityForceD3D11 = "-force-d3d11";

			public static string UnityForceGlCore = "-force-glcore";

			public static string UnityForceVulkan = "-force-vulkan";

			public static string UnityScreenQuality = "-screen-quality";

			public static string UnityScreenWidth = "-screen-height";

			public static string UnityScreenHeight = "-screen-width";
		}

		public static string[] GetArguments()
		{
			return Environment.GetCommandLineArgs();
		}

		public static string GetArgumentLine()
		{
			return Environment.CommandLine;
		}

		public static ulong GetSteamLobbyInvite()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			ulong result = 0uL;
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i] == CommonCommands.SteamLobbyConnect && i + 1 < commandLineArgs.Length && ulong.TryParse(commandLineArgs[i + 1], out result))
				{
					return result;
				}
			}
			return result;
		}

		public static bool GetAutoConfig()
		{
			string[] arguments = GetArguments();
			for (int i = 0; i < arguments.Length; i++)
			{
				if (arguments[i] == CommonCommands.SteamAutoConfig)
				{
					return true;
				}
			}
			return false;
		}
	}
}
