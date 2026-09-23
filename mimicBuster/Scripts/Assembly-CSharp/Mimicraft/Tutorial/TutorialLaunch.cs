using UnityEngine;

namespace Mimicraft.Tutorial
{
	public static class TutorialLaunch
	{
		public static bool Requested { get; private set; }

		public static bool IsTutorialSession { get; private set; }

		public static void Request()
		{
			Requested = true;
			IsTutorialSession = true;
		}

		public static void Clear()
		{
			Requested = false;
			IsTutorialSession = false;
		}

		public static bool Consume()
		{
			bool requested = Requested;
			Requested = false;
			return requested;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			Requested = false;
			IsTutorialSession = false;
		}
	}
}
