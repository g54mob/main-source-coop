using System.Collections.Generic;
using Features.GrabModule.Scripts.PhysGrab.CartGrabber;
using Fusion;
using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	[NetworkBehaviourWeaved(0)]
	public class NullGrabObject : GrabObjectBase
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private List<PhysGrabber> _playerGrabbing = new List<PhysGrabber>();

		[SerializeField]
		private float _grabStrengthMultiplier = 1f;

		[SerializeField]
		private CartItemsGrabber _cartItemsGrabber;

		public override Rigidbody Rigidbody => _rigidbody;

		public override List<PhysGrabber> Grabbers => _playerGrabbing;

		public override ICartItemsContainer CartItemsGrabber => _cartItemsGrabber;

		public override bool GrabbingPhysicsBlocked { get; set; }

		public override float GrabStrengthMultiplier
		{
			get
			{
				return _grabStrengthMultiplier;
			}
			set
			{
				_grabStrengthMultiplier = value;
			}
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
