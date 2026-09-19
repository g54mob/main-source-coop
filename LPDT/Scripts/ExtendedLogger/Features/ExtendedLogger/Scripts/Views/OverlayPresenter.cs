using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.ExtendedLogger.Scripts.Views
{
	[PublicAPI]
	public class OverlayPresenter : PresenterBehaviour<OverlayViewBase>
	{
		private readonly OverlayModel _overlayModel;

		public OverlayPresenter(OverlayModel overlayModel)
		{
			_overlayModel = overlayModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			_overlayModel.OnOverlaySwitched += ChangeOverlayText;
			foreach (DebugFilterType key in _overlayModel.CurrentOverlays.Keys)
			{
				ChangeOverlayText(key);
			}
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_overlayModel.OnOverlaySwitched -= ChangeOverlayText;
		}

		private void ChangeOverlayText(DebugFilterType debugFilterType)
		{
			base.View.OverlaysByFilters[debugFilterType].SetText(_overlayModel.CurrentOverlays[debugFilterType]);
		}
	}
}
