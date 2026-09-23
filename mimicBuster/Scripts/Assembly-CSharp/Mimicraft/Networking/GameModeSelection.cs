using UnityEngine;

namespace Mimicraft.Networking
{
	public static class GameModeSelection
	{
		public static string SelectedModeId { get; private set; } = "";

		public static void Select(string modeId)
		{
			SelectedModeId = modeId ?? "";
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			SelectedModeId = "";
		}
	}
}
