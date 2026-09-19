using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Features.RagdollModule.Scripts
{
	public interface IRagdollEntity
	{
		NetworkObject NetworkObject { get; }

		HashSet<RagdollSimulationReasonEnum> SimulationReasons { get; }

		bool IsActiveRagdoll { get; set; }

		bool IsSimulated { get; }

		RagdollPhysData RootPhysData { get; }

		bool IsInitialized { get; }

		event Action<IRagdollEntity> OnSimulationStarted;

		event Action<IRagdollEntity> OnSimulationStopped;

		event Action<RagdollSimulationReasonEnum> OnRagdollSimulationReasonAdded;

		event Action<RagdollSimulationReasonEnum> OnRagdollSimulationReasonRemoved;

		event Action<Vector3, Vector3> OnTeleported;

		void AddSimulationReason(RagdollSimulationReasonEnum reason);

		void AddSimulationReason(RagdollSimulationReasonEnum reason, Transform poseBlueprint);

		void RemoveSimulationReason(RagdollSimulationReasonEnum reason);

		void RemoveAllSimulationReasons();

		void ForceRecoverInPlace();

		bool HasSimulationReason(RagdollSimulationReasonEnum reason);

		void ApplyPose(Transform poseBlueprint);

		void SwitchSelfCollisions(bool collisionsEnabled);

		void ResetPose();

		void Teleport(Vector3 position, Quaternion? rotation = null);

		void SetInterpolation(RigidbodyInterpolation interpolation);

		void SetPose(Transform poseBlueprint);
	}
}
