using UnityEngine;

namespace Features.LevelGatesModule.Scripts
{
	public interface IGate
	{
		Vector3 Position { get; }

		Vector3 Forward { get; }

		GateType GateType { get; }

		GateGroup GateGroup { get; }

		bool IsActivated { get; }

		void ActivateGate(Vector3 observationPoint, bool immediate = false);

		void DeactivateGate(bool immediate = false);
	}
}
