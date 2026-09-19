using Fusion;
using UnityEngine;

namespace Features.RagdollModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class NonRagdollAuthorityKinematicsDetector : AuthorityKinematicsDetector
	{
		[SerializeField]
		private RagdollEntity _ragdollEntity;

		public override bool IsPhysicsKinematics()
		{
			return _ragdollEntity.IsSimulated;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
