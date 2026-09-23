using Mimicraft.Gameplay;
using Mimicraft.Networking;
using Mimicraft.Tutorial;
using UnityEngine;

namespace Mimicraft.UI
{
	public static class PracticeSession
	{
		public const string DefaultModeId = "practice";

		public static bool Start(bool tutorial, string modeId = "practice")
		{
			if (GameModeCatalog.Find(modeId) == null)
			{
				Report("'" + modeId + "' oyun modu bulunamadi - Resources/GameModes altinda bu id'ye sahip bir GameModeDefinition var mi?");
				return false;
			}
			LobbyView lobbyView = Object.FindFirstObjectByType<LobbyView>(FindObjectsInactive.Include);
			if (lobbyView == null)
			{
				Report("Menude LobbyView yok - practice oturumu baslatilamadi.");
				return false;
			}
			GameModeSelection.Select(modeId);
			if (tutorial)
			{
				TutorialLaunch.Request();
			}
			else
			{
				TutorialLaunch.Clear();
			}
			lobbyView.CreateLobby();
			return true;
		}

		private static void Report(string message)
		{
			if (ToastView.Instance != null)
			{
				ToastView.Instance.Show(message);
			}
			Debug.LogError("[PracticeSession] " + message);
		}
	}
}
