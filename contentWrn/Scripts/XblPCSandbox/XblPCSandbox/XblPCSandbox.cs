using System;

namespace XblPCSandbox
{
	public class XblPCSandbox
	{
		public static bool IsValidSandbox(string sandbox)
		{
			return SandboxHelper.IsValidSandbox(sandbox);
		}

		public static string GetSandbox()
		{
			return SandboxHelper.GetSandbox();
		}

		public static void SetSandbox(string sandbox)
		{
			if (!IsValidSandbox(sandbox))
			{
				throw new ArgumentException("Sandbox isn't valid. A valid sandbox adheres to [a-zA-Z0-9.]+.");
			}
			SandboxHelper.SetSandbox(sandbox);
		}

		public static void RestartApps()
		{
			SandboxHelper.OpenAppsInSandboxMode(onlyLaunchIfOpen: false);
		}
	}
}
