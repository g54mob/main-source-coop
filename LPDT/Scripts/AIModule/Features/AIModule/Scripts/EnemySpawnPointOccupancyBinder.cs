using Fusion;

namespace Features.AIModule.Scripts
{
	public sealed class EnemySpawnPointOccupancyBinder
	{
		private IEnemySpawnPointOccupancy _occupancy;

		private NetworkId _ownerId;

		public IEnemySpawnPointOccupancy Occupancy => _occupancy;

		public void Bind(IEnemySpawnPointOccupancy occupancy, NetworkObject owner)
		{
			if (occupancy != null && !(owner == null) && owner.IsValid)
			{
				_occupancy = occupancy;
				_ownerId = owner.Id;
			}
		}

		public void ReleaseBound()
		{
			if (_occupancy != null && _ownerId != default(NetworkId))
			{
				_occupancy.Release(_ownerId);
			}
			_occupancy = null;
			_ownerId = default(NetworkId);
		}
	}
}
