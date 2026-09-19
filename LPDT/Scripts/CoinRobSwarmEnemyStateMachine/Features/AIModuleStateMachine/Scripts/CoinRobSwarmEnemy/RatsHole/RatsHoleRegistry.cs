using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole
{
	public class RatsHoleRegistry : IReadyHoleLocator
	{
		private readonly List<RatsHole> _holes = new List<RatsHole>();

		public IReadOnlyList<RatsHole> Holes => _holes;

		public void Register(RatsHole hole)
		{
			if (!(hole == null) && !_holes.Contains(hole))
			{
				_holes.Add(hole);
			}
		}

		public void Unregister(RatsHole hole)
		{
			if (!(hole == null))
			{
				_holes.Remove(hole);
			}
		}

		public bool TryGetNearest(Vector3 worldPosition, out RatsHole nearestHole)
		{
			nearestHole = null;
			float num = float.MaxValue;
			for (int i = 0; i < _holes.Count; i++)
			{
				RatsHole ratsHole = _holes[i];
				if (!(ratsHole == null) && ratsHole.IsReady)
				{
					float sqrMagnitude = (ratsHole.AbsorbWorldPosition - worldPosition).sqrMagnitude;
					if (!(sqrMagnitude >= num))
					{
						num = sqrMagnitude;
						nearestHole = ratsHole;
					}
				}
			}
			return nearestHole != null;
		}

		public bool TryGetNearestReadyHolePosition(Vector3 worldPosition, out Vector3 holePosition)
		{
			holePosition = default(Vector3);
			if (!TryGetNearest(worldPosition, out var nearestHole))
			{
				return false;
			}
			holePosition = nearestHole.AbsorbWorldPosition;
			return true;
		}

		public bool HasReadyHoleNearSpawnPosition(Vector3 spawnPosition, float radius)
		{
			float num = radius * radius;
			for (int i = 0; i < _holes.Count; i++)
			{
				RatsHole ratsHole = _holes[i];
				if (!(ratsHole == null) && ratsHole.IsReady && (ratsHole.transform.position - spawnPosition).sqrMagnitude <= num)
				{
					return true;
				}
			}
			return false;
		}
	}
}
