using System;
using UnityEngine;

namespace Obi
{
	public class VoxelPathFinder
	{
		public struct TargetVoxel : IEquatable<TargetVoxel>, IComparable<TargetVoxel>
		{
			public Vector3Int coordinates;

			public float distance;

			public float heuristic;

			public float cost => distance + heuristic;

			public TargetVoxel(Vector3Int coordinates, float distance, float heuristic)
			{
				this.coordinates = coordinates;
				this.distance = distance;
				this.heuristic = heuristic;
			}

			public bool Equals(TargetVoxel other)
			{
				return coordinates.Equals(other.coordinates);
			}

			public int CompareTo(TargetVoxel other)
			{
				return cost.CompareTo(other.cost);
			}
		}

		private MeshVoxelizer voxelizer;

		private bool[,,] closed;

		private PriorityQueue<TargetVoxel> open;

		public VoxelPathFinder(MeshVoxelizer voxelizer)
		{
			this.voxelizer = voxelizer;
			closed = new bool[voxelizer.resolution.x, voxelizer.resolution.y, voxelizer.resolution.z];
			open = new PriorityQueue<TargetVoxel>();
		}

		private TargetVoxel AStar(in Vector3Int start, Func<TargetVoxel, bool> termination, Func<Vector3Int, float> heuristic)
		{
			Array.Clear(closed, 0, closed.Length);
			open.Clear();
			open.Enqueue(new TargetVoxel(start, 0f, 0f));
			while (open.Count() != 0)
			{
				TargetVoxel targetVoxel = open.Dequeue();
				if (termination(targetVoxel))
				{
					return targetVoxel;
				}
				closed[targetVoxel.coordinates.x, targetVoxel.coordinates.y, targetVoxel.coordinates.z] = true;
				for (int i = 0; i < MeshVoxelizer.fullNeighborhood.Length; i++)
				{
					Vector3Int coords = targetVoxel.coordinates + MeshVoxelizer.fullNeighborhood[i];
					if (!voxelizer.VoxelExists(in coords) || voxelizer[coords.x, coords.y, coords.z] == MeshVoxelizer.Voxel.Outside || closed[coords.x, coords.y, coords.z])
					{
						continue;
					}
					TargetVoxel targetVoxel2 = new TargetVoxel(coords, targetVoxel.distance + voxelizer.GetDistanceToNeighbor(i), heuristic(coords));
					int num = -1;
					for (int j = 0; j < open.Count(); j++)
					{
						if (open.data[j].coordinates == coords)
						{
							num = j;
							break;
						}
					}
					if (num < 0)
					{
						open.Enqueue(targetVoxel2);
					}
					else if (targetVoxel2.distance < open.data[num].distance)
					{
						open.data[num] = targetVoxel2;
					}
				}
			}
			return new TargetVoxel(Vector3Int.zero, -1f, -1f);
		}

		public TargetVoxel FindClosestNonEmptyVoxel(in Vector3Int start)
		{
			if (voxelizer == null)
			{
				return new TargetVoxel(Vector3Int.zero, -1f, -1f);
			}
			if (!voxelizer.VoxelExists(in start))
			{
				return new TargetVoxel(Vector3Int.zero, -1f, -1f);
			}
			if (voxelizer[start.x, start.y, start.z] != MeshVoxelizer.Voxel.Outside)
			{
				return new TargetVoxel(start, 0f, 0f);
			}
			Array.Clear(closed, 0, closed.Length);
			return AStar(in start, (TargetVoxel v) => voxelizer[v.coordinates.x, v.coordinates.y, v.coordinates.z] != MeshVoxelizer.Voxel.Outside, (Vector3Int c) => 0f);
		}

		public TargetVoxel FindPath(in Vector3Int start, Vector3Int end)
		{
			if (voxelizer == null)
			{
				return new TargetVoxel(Vector3Int.zero, -1f, -1f);
			}
			if (!voxelizer.VoxelExists(in start) || !voxelizer.VoxelExists(in end))
			{
				return new TargetVoxel(Vector3Int.zero, -1f, -1f);
			}
			if (voxelizer[start.x, start.y, start.z] == MeshVoxelizer.Voxel.Outside || voxelizer[end.x, end.y, end.z] == MeshVoxelizer.Voxel.Outside)
			{
				return new TargetVoxel(Vector3Int.zero, -1f, -1f);
			}
			return AStar(in start, (TargetVoxel v) => v.coordinates == end, (Vector3Int c) => Vector3.Distance(c, end) * voxelizer.voxelSize);
		}
	}
}
