using EvilCore.Inputs;
using Rewired;

namespace NomadDrive.Features.Inputs
{
	public static class DrivingInputs
	{
		private const string Throttle = "Throttle";

		private const string Brakes = "Brakes";

		private const string Steering = "Steering";

		private const string Handbrake = "Handbrake";

		private const string Horn = "Horn";

		private const string SwitchCamera = "SwitchCamera";

		private const string ToggleHeadlights = "ToggleHeadlights";

		private const string ToggleWipers = "ToggleWipers";

		private const string ToggleIgnition = "ToggleIgnition";

		private const string ToggleCabinLight = "ToggleCabinLight";

		private const string ToggleHandbrake = "ToggleHandbrake";

		private static Rewired.Player Player => InputHelper<DrivingInputsMarker>.Player;

		static DrivingInputs()
		{
			InputHelper<DrivingInputsMarker>.SetMapId(3);
		}

		public static void Enable()
		{
			InputHelper<DrivingInputsMarker>.EnableMap();
		}

		public static void Disable()
		{
			InputHelper<DrivingInputsMarker>.DisableMap();
		}

		public static float GetThrottle()
		{
			return Player.GetAxis("Throttle");
		}

		public static float GetBrakes()
		{
			return Player.GetAxis("Brakes");
		}

		public static float GetSteering()
		{
			return Player.GetAxis("Steering");
		}

		public static float GetHandbrake()
		{
			return Player.GetAxis("Handbrake");
		}

		public static bool IsHornButton()
		{
			return Player.GetButton("Horn");
		}

		public static bool IsSwitchCameraButtonDown()
		{
			return Player.GetButtonDown("SwitchCamera");
		}

		public static bool IsToggleHeadlightsButtonDown()
		{
			return Player.GetButtonDown("ToggleHeadlights");
		}

		public static bool IsToggleWipersButtonDown()
		{
			return Player.GetButtonDown("ToggleWipers");
		}

		public static bool IsToggleIgnitionButtonDown()
		{
			return Player.GetButtonDown("ToggleIgnition");
		}

		public static bool IsToggleCabinLightButtonDown()
		{
			return Player.GetButtonDown("ToggleCabinLight");
		}

		public static bool IsToggleHandbrakeButtonDown()
		{
			return Player.GetButtonDown("ToggleHandbrake");
		}
	}
}
