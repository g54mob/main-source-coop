namespace NomadDrive.Features.Inputs
{
	public interface IActionMapsManager
	{
		void EnterEquippingMode();

		void ExitEquippingMode();

		void EnterDrivingMode();

		void ExitDrivingMode();

		void EnterSittingMode();

		void ExitSittingMode();

		void EnterPlacingObjectMode();

		void ExitPlacingObjectMode();
	}
}
