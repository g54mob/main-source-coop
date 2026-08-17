using EvilCore.Inputs;
using Rewired;

namespace NomadDrive.Features.Inputs
{
	public static class PlacingObjectInputs
	{
		private const string RotateObject = "RotateObject";

		private const string FinishObjectPlacing = "FinishObjectPlacing";

		private const string MoveObjectHorizontal = "MoveObjectHorizontal";

		private const string MoveObjectVertical = "MoveObjectVertical";

		private const string MoveObjectDirect = "MoveObjectDirect";

		private static Rewired.Player Player => InputHelper<PlacingObjectInputsMarker>.Player;

		static PlacingObjectInputs()
		{
			InputHelper<PlacingObjectInputsMarker>.SetMapId(5);
		}

		public static void Enable()
		{
			InputHelper<PlacingObjectInputsMarker>.EnableMap();
		}

		public static void Disable()
		{
			InputHelper<PlacingObjectInputsMarker>.DisableMap();
		}

		public static bool GetRotateObjectButton()
		{
			return Player.GetButton("RotateObject");
		}

		public static bool IsFinishObjectPlacingButtonDown()
		{
			return Player.GetButtonDown("FinishObjectPlacing");
		}

		public static float GetMoveObjectHorizontalValue()
		{
			return Player.GetAxis("MoveObjectHorizontal");
		}

		public static float GetMoveObjectVerticalValue()
		{
			return Player.GetAxis("MoveObjectVertical");
		}

		public static float GetMoveObjectDirectValue()
		{
			return Player.GetAxis("MoveObjectDirect");
		}
	}
}
