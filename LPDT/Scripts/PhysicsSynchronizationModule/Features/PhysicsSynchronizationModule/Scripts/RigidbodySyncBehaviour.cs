using Fusion;
using UnityEngine;

namespace Features.PhysicsSynchronizationModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class RigidbodySyncBehaviour : NetworkBehaviour
	{
		[SerializeField]
		private Rigidbody _rigidBody;

		private void Start()
		{
			if (!base.Object.HasStateAuthority)
			{
				_rigidBody.isKinematic = true;
			}
		}

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
