using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.Movement.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerKinematicsDetector : AuthorityKinematicsDetector
	{
		[SerializeField]
		private RagdollEntity _ragdollEntity;

		[SerializeField]
		private PlayerCharacterMovableBase _playerCharacterMovableBase;

		public override bool IsPhysicsKinematics()
		{
			if (!_playerCharacterMovableBase.IsKinematic)
			{
				return _ragdollEntity.IsSimulated;
			}
			return true;
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
