using Fusion;
using UnityEngine;

namespace Features.RagdollModule.Scripts
{
	public struct RagdollPhysData
	{
		public Rigidbody RigidBody;

		public Collider Collider;

		public INetworkTRSPTeleport TransformSynchronizer;
	}
}
