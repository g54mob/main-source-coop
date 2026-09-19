using Features.BeachInteractableCommonModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.BarBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(2)]
	public class BarMusicButtonInteractReactor : ButtonInteractReactor
	{
		[SerializeField]
		private BarMusicController _barMusicController;

		protected override void OnButtonInteracted(bool isSwitchedOn)
		{
			BarMusicController barMusicController = ResolveMusicController();
			if (!(barMusicController == null))
			{
				barMusicController.SetPlaying(isSwitchedOn);
			}
		}

		public void SyncFromMusicController()
		{
			BarMusicController barMusicController = ResolveMusicController();
			if (!(barMusicController == null))
			{
				SyncSwitchedOnFromExternal(barMusicController.IsPlaying);
			}
		}

		private BarMusicController ResolveMusicController()
		{
			if (_barMusicController != null)
			{
				return _barMusicController;
			}
			_barMusicController = GetComponentInParent<BarMusicController>(includeInactive: true);
			if (_barMusicController != null)
			{
				return _barMusicController;
			}
			_barMusicController = UnityEngine.Object.FindFirstObjectByType<BarMusicController>();
			return _barMusicController;
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
