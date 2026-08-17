using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.ObjectSpawning
{
	public class LootPlacementContext
	{
		private readonly List<PlacedItemRecord> _placedItems = new List<PlacedItemRecord>();

		private const float DuplicatePositionEpsilon = 0.1f;

		private readonly List<Vector3> _reservedPositions = new List<Vector3>();

		public IReadOnlyList<PlacedItemRecord> PlacedItems => _placedItems;

		public void RegisterPlacedItem(Vector3 boundsCenter, Vector3 boundsHalfExtents)
		{
			_placedItems.Add(new PlacedItemRecord(boundsCenter, boundsHalfExtents));
		}

		public bool TryReservePosition(Vector3 worldPos)
		{
			float num = 0.010000001f;
			for (int i = 0; i < _reservedPositions.Count; i++)
			{
				if ((_reservedPositions[i] - worldPos).sqrMagnitude < num)
				{
					return false;
				}
			}
			_reservedPositions.Add(worldPos);
			return true;
		}

		public void Clear()
		{
			_placedItems.Clear();
			_reservedPositions.Clear();
		}
	}
}
