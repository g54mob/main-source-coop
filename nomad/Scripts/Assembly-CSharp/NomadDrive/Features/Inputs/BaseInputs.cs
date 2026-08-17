using EvilCore.Inputs;
using Rewired;

namespace NomadDrive.Features.Inputs
{
	public static class BaseInputs
	{
		private const string LookHorizontal = "LookHorizontal";

		private const string LookVertical = "LookVertical";

		private const string CameraZoom = "CameraZoom";

		private const string PrimaryInteraction = "PrimaryInteraction";

		private const string SecondaryInteraction = "SecondaryInteraction";

		private const string VoiceChat = "VoiceChat";

		private const string ToggleObjectivesPanel = "ToggleObjectivesPanel";

		private const string ObjectivesPanelPreviousPage = "ObjectivesPanelPreviousPage";

		private const string ObjectivesPanelNextPage = "ObjectivesPanelNextPage";

		private const string ObjectivesPanelLock = "ObjectivesPanelLock";

		private const string Respawn = "Respawn";

		private static Rewired.Player Player => InputHelper<BaseInputsMarker>.Player;

		static BaseInputs()
		{
			InputHelper<BaseInputsMarker>.SetMapId(1);
		}

		public static void Enable()
		{
			InputHelper<BaseInputsMarker>.EnableMap();
		}

		public static void Disable()
		{
			InputHelper<BaseInputsMarker>.DisableMap();
		}

		public static float GetLookHorizontal()
		{
			return Player.GetAxis("LookHorizontal");
		}

		public static float GetLookVertical()
		{
			return Player.GetAxis("LookVertical");
		}

		public static bool IsCameraZoomButtonDown()
		{
			return Player.GetButtonDown("CameraZoom");
		}

		public static bool IsCameraZoomButtonUp()
		{
			return Player.GetButtonUp("CameraZoom");
		}

		public static bool IsPrimaryInteractionButtonDown()
		{
			return Player.GetButtonDown("PrimaryInteraction");
		}

		public static bool IsPrimaryInteractionButtonUp()
		{
			return Player.GetButtonUp("PrimaryInteraction");
		}

		public static bool IsSecondaryInteractionButtonDown()
		{
			return Player.GetButtonDown("SecondaryInteraction");
		}

		public static bool IsSecondaryInteractionButtonUp()
		{
			return Player.GetButtonUp("SecondaryInteraction");
		}

		public static bool IsVoiceChatButtonDown()
		{
			return Player.GetButtonDown("VoiceChat");
		}

		public static bool IsVoiceChatButtonUp()
		{
			return Player.GetButtonUp("VoiceChat");
		}

		public static bool IsToggleObjectivesPanelButtonDown()
		{
			return Player.GetButtonDown("ToggleObjectivesPanel");
		}

		public static bool IsToggleObjectivesPanelButtonUp()
		{
			return Player.GetButtonUp("ToggleObjectivesPanel");
		}

		public static bool IsObjectivesPanelPreviousPageButtonDown()
		{
			return Player.GetButtonDown("ObjectivesPanelPreviousPage");
		}

		public static bool IsObjectivesPanelNextPageButtonDown()
		{
			return Player.GetButtonDown("ObjectivesPanelNextPage");
		}

		public static bool IsObjectivesPanelLockButtonDown()
		{
			return Player.GetButtonDown("ObjectivesPanelLock");
		}

		public static bool IsRespawnButtonDown()
		{
			return Player.GetButtonDown("Respawn");
		}
	}
}
