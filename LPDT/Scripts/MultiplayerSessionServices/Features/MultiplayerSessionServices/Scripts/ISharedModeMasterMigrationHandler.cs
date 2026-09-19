using Fusion;

namespace Features.MultiplayerSessionServices.Scripts
{
	public interface ISharedModeMasterMigrationHandler
	{
		int SharedModeMasterMigrationHandleOrder { get; }

		void OnSharedModeMasterMigrationCompleted(NetworkRunner runner);
	}
}
