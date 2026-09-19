namespace Features.SessionManagementModule.Models
{
	public interface IShopSeatHealthObservation
	{
		bool IsStoreTablePresent { get; }

		bool TryVerifyLocalSeatSpendable(out string problem);

		bool TryVerifyAllSeatedAvatarsInShop(out string problem);

		bool TryVerifyLocalSeatedAvatarPresent(out string problem);
	}
}
