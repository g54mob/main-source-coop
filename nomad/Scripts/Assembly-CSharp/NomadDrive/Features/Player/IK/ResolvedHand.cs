using RootMotion.FinalIK;

namespace NomadDrive.Features.Player.IK
{
	public readonly struct ResolvedHand
	{
		public readonly FullBodyBipedEffector PrimaryEffector;

		public readonly InteractionObject PrimaryInteractionObject;

		public readonly FullBodyBipedEffector SecondaryEffector;

		public readonly InteractionObject SecondaryInteractionObject;

		public readonly bool UseBothHands;

		public readonly bool IsValid;

		public static readonly ResolvedHand Invalid;

		public ResolvedHand(FullBodyBipedEffector effector, InteractionObject interactionObject)
		{
			PrimaryEffector = effector;
			PrimaryInteractionObject = interactionObject;
			SecondaryEffector = FullBodyBipedEffector.Body;
			SecondaryInteractionObject = null;
			UseBothHands = false;
			IsValid = interactionObject != null;
		}

		public ResolvedHand(FullBodyBipedEffector primaryEffector, InteractionObject primaryObject, FullBodyBipedEffector secondaryEffector, InteractionObject secondaryObject)
		{
			PrimaryEffector = primaryEffector;
			PrimaryInteractionObject = primaryObject;
			SecondaryEffector = secondaryEffector;
			SecondaryInteractionObject = secondaryObject;
			UseBothHands = true;
			IsValid = primaryObject != null || secondaryObject != null;
		}
	}
}
