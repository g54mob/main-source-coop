namespace NomadDrive.Features.LiquidTransferSystem
{
	public interface ILiquidTransferActions
	{
		void SetLiquidTransferStartedActions(ILiquidContainer other);

		void SetLiquidTransferCompletedActions();

		void SetLiquidTransferCancelledActions()
		{
		}
	}
}
