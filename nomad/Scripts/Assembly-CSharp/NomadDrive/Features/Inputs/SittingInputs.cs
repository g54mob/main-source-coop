using EvilCore.Inputs;
using Rewired;

namespace NomadDrive.Features.Inputs
{
	public static class SittingInputs
	{
		private const string Standup = "Standup";

		private const string SwitchCamera = "SwitchCamera";

		private static Rewired.Player Player => InputHelper<SittingInputsMarker>.Player;

		static SittingInputs()
		{
			InputHelper<SittingInputsMarker>.SetMapId(6);
		}

		public static void Enable()
		{
			InputHelper<SittingInputsMarker>.EnableMap();
		}

		public static void Disable()
		{
			InputHelper<SittingInputsMarker>.DisableMap();
		}

		public static bool IsStandupButton()
		{
			return Player.GetButton("Standup");
		}

		public static bool IsStandupButtonDown()
		{
			return Player.GetButtonDown("Standup");
		}

		public static bool IsSwitchCameraButtonDown()
		{
			return Player.GetButtonDown("SwitchCamera");
		}
	}
}
