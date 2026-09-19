using Features.CollectingModule.Scripts.New;
using Features.InteractModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.CollectingModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class CollectInteractable : InteractableBase
	{
		[SerializeField]
		private PhysicsItemUpVectorLimiter _physicsItemUpVectorLimiter;

		private bool _isPendingInteraction;

		public override void Interact()
		{
			if (IsInteractable)
			{
				if (base.Object.HasStateAuthority)
				{
					ExecuteInteractionLogic();
					return;
				}
				_isPendingInteraction = true;
				base.Object.RequestStateAuthority();
			}
		}

		public override void StateAuthorityChanged()
		{
			ClearLocalPendingInteractionUnlessAuthority(ref _isPendingInteraction);
			base.StateAuthorityChanged();
			if (base.HasStateAuthority && _isPendingInteraction)
			{
				_isPendingInteraction = false;
				ExecuteInteractionLogic();
			}
		}

		private void ExecuteInteractionLogic()
		{
			_physicsItemUpVectorLimiter.SetTargetUpVector(-_physicsItemUpVectorLimiter.CurrentUpVector);
		}

		public override void OnInteractEnd()
		{
			_physicsItemUpVectorLimiter.SetTargetUpVector(Vector3.up);
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
