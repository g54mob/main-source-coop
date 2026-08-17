using System;
using NomadDrive.Features.Rope;

namespace NomadDrive.Features.EvilRoads.Cable
{
	public interface ICableManager
	{
		int CableCount { get; }

		float SwingMultiplier { get; set; }

		float SlackMultiplier { get; set; }

		event Action<CrossChunkCable> OnCrossChunkCableCreated;

		event Action<CrossChunkCable> OnCrossChunkCableDestroyed;

		void RegisterCable(NomadDrive.Features.Rope.Rope rope, CableConnectionConfig config);

		void UnregisterCable(NomadDrive.Features.Rope.Rope rope);

		void ApplyMultipliers();

		void RegisterEdgeObject(EdgeObjectInfo edgeInfo);

		void UnregisterEdgeObjects(EvilRoad road);
	}
}
