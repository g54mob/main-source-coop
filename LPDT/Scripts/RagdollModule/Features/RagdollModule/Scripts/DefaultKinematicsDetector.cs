using Fusion;

namespace Features.RagdollModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class DefaultKinematicsDetector : AuthorityKinematicsDetector
	{
		public override bool IsPhysicsKinematics()
		{
			return false;
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
