using System.Collections.Generic;
using Features.LevelModule.Scripts;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core.SafeZones
{
	public class EnemySafeZoneInteractionPointsModel : ILevelCleanup
	{
		private readonly Dictionary<Transform, EnemySafeZoneInteractionPointOccupant> _occupants = new Dictionary<Transform, EnemySafeZoneInteractionPointOccupant>();

		public IReadOnlyDictionary<Transform, EnemySafeZoneInteractionPointOccupant> Occupants => _occupants;

		public bool IsOccupiedByAnother(Transform point, int ownerType, int ownerInstanceId)
		{
			if (point != null && _occupants.TryGetValue(point, out var value))
			{
				return !value.IsOwnedBy(ownerType, ownerInstanceId);
			}
			return false;
		}

		public bool TryOccupy(Transform point, int ownerType, int ownerInstanceId)
		{
			if (point == null)
			{
				return false;
			}
			if (_occupants.TryGetValue(point, out var value) && !value.IsOwnedBy(ownerType, ownerInstanceId))
			{
				return false;
			}
			_occupants[point] = new EnemySafeZoneInteractionPointOccupant(ownerType, ownerInstanceId);
			return true;
		}

		public void ReleaseOccupiedByOwner(int ownerType, int ownerInstanceId)
		{
			List<Transform> list = null;
			foreach (KeyValuePair<Transform, EnemySafeZoneInteractionPointOccupant> occupant in _occupants)
			{
				if (occupant.Value.IsOwnedBy(ownerType, ownerInstanceId))
				{
					if (list == null)
					{
						list = new List<Transform>();
					}
					list.Add(occupant.Key);
				}
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					_occupants.Remove(list[i]);
				}
			}
		}

		public void Cleanup()
		{
			_occupants.Clear();
		}
	}
}
