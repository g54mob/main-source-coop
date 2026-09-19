using System.Collections.Generic;
using Features.GrabModule.Scripts.PhysGrab.CartGrabber;
using Fusion;
using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	[NetworkBehaviourWeaved(0)]
	public abstract class GrabObjectBase : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface
	{
		public abstract Rigidbody Rigidbody { get; }

		public abstract List<PhysGrabber> Grabbers { get; }

		public abstract float GrabStrengthMultiplier { get; set; }

		public abstract ICartItemsContainer CartItemsGrabber { get; }

		public abstract bool GrabbingPhysicsBlocked { get; set; }

		public virtual void StateAuthorityChanged()
		{
			if (base.HasStateAuthority && Rigidbody != null)
			{
				Rigidbody.isKinematic = false;
				Rigidbody.WakeUp();
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
