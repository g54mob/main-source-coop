namespace NomadDrive.Features.LiquidTransferSystem
{
	public enum TransferRequestResult
	{
		SourceEmpty = 0,
		TargetFull = 1,
		IncompatibleLiquid = 2,
		Error = 3,
		Allowed = 4,
		SourceOutputNotAllowed = 5,
		TargetInputNotAllowed = 6
	}
}
