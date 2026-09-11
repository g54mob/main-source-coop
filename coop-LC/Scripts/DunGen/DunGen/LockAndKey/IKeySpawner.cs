namespace DunGen.LockAndKey
{
	public interface IKeySpawner
	{
		bool CanSpawnKey(KeyManager keyManager, Key key);

		void SpawnKey(KeySpawnParameters keySpawnParameters);
	}
}
