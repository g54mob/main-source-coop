using UnityEngine;

namespace Features.BootstrapModule.Scripts.Bootstraps
{
	public static class ProjectLaunchSessionState
	{
		public static bool IsGameLaunchRegistered { get; private set; }

		public static void MarkGameLaunchRegistered()
		{
			IsGameLaunchRegistered = true;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Reset()
		{
			IsGameLaunchRegistered = false;
		}
	}
}
