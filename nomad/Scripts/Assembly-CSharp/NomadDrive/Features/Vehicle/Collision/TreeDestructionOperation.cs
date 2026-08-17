using System;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Collision
{
	[Serializable]
	public struct TreeDestructionOperation : IEquatable<TreeDestructionOperation>
	{
		public Vector3 worldPosition;

		public Vector3 impactVelocity;

		public int chunkX;

		public int chunkZ;

		public Vector2Int ChunkCoord => new Vector2Int(chunkX, chunkZ);

		public TreeDestructionOperation(Vector3 worldPosition, Vector3 impactVelocity, int chunkX, int chunkZ)
		{
			this.worldPosition = worldPosition;
			this.impactVelocity = impactVelocity;
			this.chunkX = chunkX;
			this.chunkZ = chunkZ;
		}

		public bool Equals(TreeDestructionOperation other)
		{
			if (worldPosition.Equals(other.worldPosition) && impactVelocity.Equals(other.impactVelocity) && chunkX == other.chunkX)
			{
				return chunkZ == other.chunkZ;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is TreeDestructionOperation other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(worldPosition, impactVelocity, chunkX, chunkZ);
		}
	}
}
