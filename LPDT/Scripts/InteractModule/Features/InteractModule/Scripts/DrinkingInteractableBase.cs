using System;
using Fusion;

namespace Features.InteractModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public abstract class DrinkingInteractableBase : InteractableBase
	{
		public abstract bool IsToggledOn { get; }

		public abstract bool IsConsumed { get; protected set; }

		public event Action OnToggleChanged;

		public event Action OnConsumed;

		protected void RaiseToggleChanged()
		{
			this.OnToggleChanged?.Invoke();
		}

		protected void RaiseConsumed()
		{
			this.OnConsumed?.Invoke();
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
