using Fusion;

namespace Features.InteractModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public abstract class InteractableBase : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface
	{
		public bool IsInteractable = true;

		public virtual void Interact()
		{
			_ = IsInteractable;
		}

		public virtual void OnInteractEnd()
		{
		}

		protected void ClearLocalPendingInteractionUnlessAuthority(ref bool pendingInteraction)
		{
			if (!base.HasStateAuthority)
			{
				pendingInteraction = false;
			}
		}

		public virtual void StateAuthorityChanged()
		{
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
