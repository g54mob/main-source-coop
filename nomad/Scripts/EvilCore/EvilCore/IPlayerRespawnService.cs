namespace EvilCore
{
	public interface IPlayerRespawnService
	{
		bool CanRespawn { get; }

		void RequestRespawn();
	}
}
