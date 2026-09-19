using System.Collections.Generic;
using Features.GrabModule.Scripts.PhysGrab.CartGrabber;
using Fusion;
using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	[NetworkBehaviourWeaved(0)]
	public class ObjectToGrabOn : GrabObjectBase
	{
		public override Rigidbody Rigidbody { get; }

		public override List<PhysGrabber> Grabbers { get; } = new List<PhysGrabber>();

		public override float GrabStrengthMultiplier { get; set; }

		public override ICartItemsContainer CartItemsGrabber => null;

		public override bool GrabbingPhysicsBlocked { get; set; }

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
