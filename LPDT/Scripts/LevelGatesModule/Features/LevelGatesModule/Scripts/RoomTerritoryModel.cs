using System.Collections.Generic;
using UnityEngine;

namespace Features.LevelGatesModule.Scripts
{
	public class RoomTerritoryModel
	{
		private readonly List<RoomTerritoryFootprint> _footprints = new List<RoomTerritoryFootprint>();

		public void Register(RoomTerritoryFootprint footprint)
		{
			if (!(footprint == null) && !_footprints.Contains(footprint))
			{
				_footprints.Add(footprint);
			}
		}

		public void Unregister(RoomTerritoryFootprint footprint)
		{
			if (!(footprint == null))
			{
				_footprints.Remove(footprint);
			}
		}

		public bool IsInsideInteriorTerritory(Vector3 worldPoint)
		{
			RoomTerritoryFootprint footprint;
			return TryGetContainingInterior(worldPoint, out footprint);
		}

		public bool TryGetContainingInterior(Vector3 worldPoint, out RoomTerritoryFootprint footprint)
		{
			for (int i = 0; i < _footprints.Count; i++)
			{
				RoomTerritoryFootprint roomTerritoryFootprint = _footprints[i];
				if (!(roomTerritoryFootprint == null) && roomTerritoryFootprint.Kind == RoomTerritoryKind.Interior && roomTerritoryFootprint.Contains(worldPoint))
				{
					footprint = roomTerritoryFootprint;
					return true;
				}
			}
			footprint = null;
			return false;
		}
	}
}
