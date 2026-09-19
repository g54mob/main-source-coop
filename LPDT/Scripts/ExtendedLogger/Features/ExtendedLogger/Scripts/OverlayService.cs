using Features.ViewSystemModule.Scripts.Windows;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.ExtendedLogger.Scripts
{
	public class OverlayService : IOverlayService
	{
		private readonly OverlayModel _overlayModel;

		private readonly OverlayGlobalWindow _overlayWindow;

		public OverlayService(OverlayModel overlayModel, OverlayGlobalWindow overlayWindow)
		{
			_overlayModel = overlayModel;
			_overlayWindow = overlayWindow;
		}

		public void SwitchOverlay(DebugFilterType debugFilterType, object message)
		{
			_overlayModel.CurrentOverlays[debugFilterType] = message.ToString();
			_overlayModel.InvokeOnOverlaySwitched(debugFilterType);
		}

		public void SetOverlayActive(bool isActive)
		{
			if (isActive)
			{
				if (_overlayWindow.WindowStatus == WindowStatus.Closed)
				{
					_overlayWindow.Open();
				}
				else if (_overlayWindow.WindowStatus == WindowStatus.Hidden)
				{
					_overlayWindow.Show();
				}
			}
			else if (_overlayWindow.WindowStatus == WindowStatus.Showed)
			{
				_overlayWindow.Close();
			}
		}
	}
}
