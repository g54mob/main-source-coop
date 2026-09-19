namespace Features.MultiplayerSessionServices.Scripts
{
	public interface IEnemySpawnGate
	{
		bool IsSpawningAllowed { get; }

		void SetSpawningAllowed(bool isAllowed);
	}
}
