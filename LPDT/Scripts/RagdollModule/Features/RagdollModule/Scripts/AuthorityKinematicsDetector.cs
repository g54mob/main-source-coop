using Fusion;

namespace Features.RagdollModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public abstract class AuthorityKinematicsDetector : NetworkBehaviour
	{
		public abstract bool IsPhysicsKinematics();

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
