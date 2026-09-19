using Fusion;

namespace Features.AIModule.Scripts
{
	public interface IEnemySpawnPointOccupancy
	{
		bool IsOccupied { get; }

		NetworkId OccupantId { get; }

		bool TryOccupy(NetworkObject occupant);

		void Release(NetworkObject occupant);

		void Release(NetworkId occupantId);
	}
}
