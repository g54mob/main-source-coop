namespace NomadDrive.Features.LiquidTransferSystem
{
	public interface ITransferDrinkLock
	{
		string UseActionPromptId { get; }

		void SetTransferLocked(bool locked);
	}
}
