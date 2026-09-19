using Fusion;

namespace Features.AIModule.Scripts
{
	public sealed class LocalEnemySpawnPointOccupancy : IEnemySpawnPointOccupancy
	{
		private NetworkId _occupantId;

		public bool IsOccupied => _occupantId != default(NetworkId);

		public NetworkId OccupantId => _occupantId;

		public bool TryOccupy(NetworkObject occupant)
		{
			if (occupant == null || !occupant.IsValid)
			{
				return false;
			}
			NetworkId id = occupant.Id;
			if (_occupantId != default(NetworkId) && _occupantId != id)
			{
				return false;
			}
			_occupantId = id;
			return true;
		}

		public void Release(NetworkObject occupant)
		{
			if (!(occupant == null) && occupant.IsValid)
			{
				Release(occupant.Id);
			}
		}

		public void Release(NetworkId occupantId)
		{
			if (!(_occupantId == default(NetworkId)) && !(_occupantId != occupantId))
			{
				_occupantId = default(NetworkId);
			}
		}
	}
}
