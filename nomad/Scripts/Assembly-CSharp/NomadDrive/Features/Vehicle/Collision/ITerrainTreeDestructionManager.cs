using UnityEngine;

namespace NomadDrive.Features.Vehicle.Collision
{
	public interface ITerrainTreeDestructionManager
	{
		TerrainTreeDestructionConfig Config { get; }

		float TreeSearchRadius { get; }

		void ReportDestruction(Vector3 worldPosition, Vector3 impactVelocity);
	}
}
