using Features.BeachInteractableCommonModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.JacuzziBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(2)]
	public class MusicJacuzziButtonInteractReactor : ButtonInteractReactor
	{
		[SerializeField]
		private JacuzziBeachInteractableBehaviour _jacuzziBeachInteractableBehaviour;

		protected override void OnButtonInteracted(bool isSwitchedOn)
		{
			_jacuzziBeachInteractableBehaviour.SwitchMusic(isSwitchedOn);
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
