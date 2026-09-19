using System;

namespace Features.ConfirmExitPopupService.Scripts
{
	public interface IConfirmExitService
	{
		void ShowPopup(Action onConfirmAction, ConfirmExitData confirmExitData, Action onCancelAction = null);
	}
}
