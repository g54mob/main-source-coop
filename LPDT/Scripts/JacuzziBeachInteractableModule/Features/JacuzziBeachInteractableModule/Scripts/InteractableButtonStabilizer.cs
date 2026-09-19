using Features.GrabModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.JacuzziBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class InteractableButtonStabilizer : NetworkBehaviour
	{
		[SerializeField]
		private SimplePointGrabable _buttonGrabbable;

		[SerializeField]
		private float _stabilizationForce;

		private void FixedUpdate()
		{
			if (base.HasStateAuthority && _buttonGrabbable.GrabbedByPlayersCount == 0)
			{
				_buttonGrabbable.Rigidbody.AddForce(Vector3.up * _stabilizationForce, ForceMode.Force);
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
