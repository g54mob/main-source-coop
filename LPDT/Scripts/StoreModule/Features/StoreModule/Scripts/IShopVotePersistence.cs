namespace Features.StoreModule.Scripts
{
	public interface IShopVotePersistence
	{
		void SaveVote(bool hasVoted);

		bool HasVoteForCurrentShop();
	}
}
