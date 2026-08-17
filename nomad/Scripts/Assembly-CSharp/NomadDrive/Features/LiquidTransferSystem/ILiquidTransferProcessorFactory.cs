namespace NomadDrive.Features.LiquidTransferSystem
{
	public interface ILiquidTransferProcessorFactory
	{
		LiquidTransferProcessor Create(ILiquidContainer source, ILiquidContainer target);

		LiquidTransferProcessor Create(ILiquidContainer target, float fillingSpeed);
	}
}
