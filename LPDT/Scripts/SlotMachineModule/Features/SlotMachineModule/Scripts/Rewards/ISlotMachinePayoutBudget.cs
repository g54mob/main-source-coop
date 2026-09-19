namespace Features.SlotMachineModule.Scripts.Rewards
{
	public interface ISlotMachinePayoutBudget
	{
		int RemainingItemCount { get; }

		int ConsumeItemCount(int requestedCount);
	}
}
