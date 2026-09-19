namespace Features.KnifeThrowingModule.Scripts
{
	public interface IKnifeThrowingTableService
	{
		void RegisterObject(KnifeThrowingTableObjectTeleport teleportable);

		void ConfigureObjectSpawning(KnifeThrowingTableObjectSpawner spawner);

		void ClearTableSessionState();

		void TryResetObjectsPosition();
	}
}
