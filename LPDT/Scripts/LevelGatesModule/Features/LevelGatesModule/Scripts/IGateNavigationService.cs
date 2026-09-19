using System.Collections.Generic;
using UnityEngine;

namespace Features.LevelGatesModule.Scripts
{
	public interface IGateNavigationService
	{
		void ConstructPathToGate(Vector3 startingPoint, IGate gate, bool projectPoints, List<GatePathData> pathGates);

		bool TryGetClosestExitGate(Vector3 startingPoint, out IGate closestGate);

		bool PathContainsGate(List<GatePathData> pathGates, IGate gate);
	}
}
