using UnityEngine;

namespace Features.RagdollModule.Scripts
{
	public class RagdollBoneData
	{
		public Transform Bone;

		public ConfigurableJoint Joint;

		public bool HasJoint;

		public bool IsPhysical;

		public bool IsRotationOnlySync;
	}
}
