using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.ConfirmExitPopupService.Scripts
{
	public class ConfirmExitService : IConfirmExitService
	{
		private readonly ConfirmationPopupWindow _window;

		private readonly ConfirmPopupModel _confirmPopupModel;

		public ConfirmExitService(ConfirmationPopupWindow window, ConfirmPopupModel confirmPopupModel)
		{
			_window = window;
			_confirmPopupModel = confirmPopupModel;
		}

		public void ShowPopup(Action onConfirmAction, ConfirmExitData confirmExitData, Action onCancelAction = null)
		{
			SetWindowActive(isActive: true);
			_confirmPopupModel.RequestShow(confirmExitData, onConfirmAction, onCancelAction);
			_confirmPopupModel.OnOpenChanged += OnModelOpenChanged;
		}

		private void HidePopup()
		{
			_confirmPopupModel.OnOpenChanged -= OnModelOpenChanged;
			SetWindowActive(isActive: false);
		}

		private void OnModelOpenChanged()
		{
			if (!_confirmPopupModel.IsOpen)
			{
				HidePopup();
			}
		}

		private void SetWindowActive(bool isActive)
		{
			if (isActive)
			{
				if (_window.WindowStatus == WindowStatus.Closed)
				{
					_window.Open();
				}
				else if (_window.WindowStatus == WindowStatus.Hidden)
				{
					_window.Show();
				}
			}
			else if (_window.WindowStatus == WindowStatus.Showed)
			{
				_window.Close();
			}
		}
	}
}
