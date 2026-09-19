namespace Features.MultiplayerSessionServices.Scripts
{
	public class EnemySpawnGate : IEnemySpawnGate
	{
		public bool IsSpawningAllowed { get; private set; } = true;

		public void SetSpawningAllowed(bool isAllowed)
		{
			IsSpawningAllowed = isAllowed;
		}
	}
}
