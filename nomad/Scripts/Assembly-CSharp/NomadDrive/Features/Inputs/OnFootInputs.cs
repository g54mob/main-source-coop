using EvilCore.Inputs;
using Rewired;

namespace NomadDrive.Features.Inputs
{
	public static class OnFootInputs
	{
		private const string MoveHorizontal = "MoveHorizontal";

		private const string MoveVertical = "MoveVertical";

		private const string Sprint = "Sprint";

		private const string Jump = "Jump";

		private const string Crouch = "Crouch";

		private const string RescueVehicle = "RescueVehicle";

		private const string RespawnPlayer = "RespawnPlayer";

		private static Rewired.Player Player => InputHelper<OnFootInputsMarker>.Player;

		static OnFootInputs()
		{
			InputHelper<OnFootInputsMarker>.SetMapId(2);
		}

		public static void Enable()
		{
			InputHelper<OnFootInputsMarker>.EnableMap();
		}

		public static void Disable()
		{
			InputHelper<OnFootInputsMarker>.DisableMap();
		}

		public static float GetMoveHorizontal()
		{
			return Player.GetAxis("MoveHorizontal");
		}

		public static float GetMoveVertical()
		{
			return Player.GetAxis("MoveVertical");
		}

		public static bool IsSprintButton()
		{
			return Player.GetButton("Sprint");
		}

		public static bool IsJumpButton()
		{
			return Player.GetButton("Jump");
		}

		public static bool IsJumpButtonDown()
		{
			return Player.GetButtonDown("Jump");
		}

		public static bool IsJumpButtonUp()
		{
			return Player.GetButtonUp("Jump");
		}

		public static bool IsCrouchButtonDown()
		{
			return Player.GetButtonDown("Crouch");
		}

		public static bool IsCrouchButtonUp()
		{
			return Player.GetButtonUp("Crouch");
		}

		public static bool GetRescueVehicleButton()
		{
			return Player.GetButton("RescueVehicle");
		}

		public static bool GetRespawnButton()
		{
			return Player.GetButton("RespawnPlayer");
		}
	}
}
