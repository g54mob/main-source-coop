using Features.GrabModule.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.SnakeModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class SnakeSegmentGrabKinematicsDetector : AuthorityKinematicsDetector
	{
		[SerializeField]
		private SimplePointGrabable _grabable;

		public override bool IsPhysicsKinematics()
		{
			if (_grabable == null || !_grabable.Initialized)
			{
				return true;
			}
			return _grabable.GrabbedByPlayers.Count == 0;
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
