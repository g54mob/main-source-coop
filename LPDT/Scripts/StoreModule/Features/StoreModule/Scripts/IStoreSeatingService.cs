namespace Features.StoreModule.Scripts
{
	public interface IStoreSeatingService
	{
		int GetSeatIndex(int playerId);

		int GetPlayerIdBySeat(int seatIndex);

		string GetHandle(int playerId);
	}
}
