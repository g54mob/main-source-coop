using NomadDrive.Features.Inputs;

namespace NomadDrive.Features.ObjectPlacement
{
	public static class PlacementInputHandler
	{
		public static float GetMoveInput()
		{
			return PlacingObjectInputs.GetMoveObjectDirectValue();
		}

		public static bool IsFinishButtonDown()
		{
			return PlacingObjectInputs.IsFinishObjectPlacingButtonDown();
		}

		public static bool IsRotateButtonHeld()
		{
			return PlacingObjectInputs.GetRotateObjectButton();
		}

		public static float GetRotationInput()
		{
			return PlacingObjectInputs.GetMoveObjectHorizontalValue();
		}
	}
}
