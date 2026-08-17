using RootMotion.FinalIK;
using UnityEngine;

namespace NomadDrive.Features.Interaction.IK
{
	public class HeldItemIKSetup : MonoBehaviour
	{
		[Header("Hand Selection")]
		[SerializeField]
		private IKHandSelection gripHandSelection = IKHandSelection.BothHands;

		[Header("Left Hand")]
		[SerializeField]
		private InteractionObject leftHandInteraction;

		[Header("Right Hand")]
		[SerializeField]
		private InteractionObject rightHandInteraction;

		public IKHandSelection GripHandSelection => gripHandSelection;

		public InteractionObject LeftHandInteraction => leftHandInteraction;

		public InteractionObject RightHandInteraction => rightHandInteraction;
	}
}
