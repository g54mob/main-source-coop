using RootMotion.FinalIK;
using UnityEngine;

namespace NomadDrive.Features.Interaction.IK
{
	public class InteractableIKSetup : MonoBehaviour
	{
		[Header("Hand Selection")]
		[SerializeField]
		private IKHandSelection handSelection = IKHandSelection.DominantHand;

		[Header("IK Targets")]
		[SerializeField]
		private InteractionObject leftHandInteractionObject;

		[SerializeField]
		private InteractionObject rightHandInteractionObject;

		[Header("Interaction Animation")]
		[SerializeField]
		private bool playAnimationOnInteract = true;

		[SerializeField]
		[Range(0.1f, 3f)]
		private float interactionSpeed = 1f;

		[SerializeField]
		[Range(0f, 2f)]
		private float hoverReEngageDelay = 0.4f;

		public IKHandSelection HandSelection => handSelection;

		public InteractionObject LeftHandInteractionObject => leftHandInteractionObject;

		public InteractionObject RightHandInteractionObject => rightHandInteractionObject;

		public bool PlayAnimationOnInteract => playAnimationOnInteract;

		public float InteractionSpeed => interactionSpeed;

		public float HoverReEngageDelay => hoverReEngageDelay;

		public InteractionObject GetInteractionObjectForEffector(FullBodyBipedEffector effector)
		{
			return effector switch
			{
				FullBodyBipedEffector.LeftHand => leftHandInteractionObject, 
				FullBodyBipedEffector.RightHand => rightHandInteractionObject, 
				_ => null, 
			};
		}
	}
}
