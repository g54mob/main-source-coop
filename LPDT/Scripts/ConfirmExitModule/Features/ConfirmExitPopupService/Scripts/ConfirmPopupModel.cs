using System;

namespace Features.ConfirmExitPopupService.Scripts
{
	public class ConfirmPopupModel
	{
		private Action _onCancel;

		public bool IsOpen { get; private set; }

		public ConfirmExitData Data { get; private set; }

		public event Action OnOpenChanged;

		public event Action OnConfirm;

		public void RequestShow(ConfirmExitData data, Action onYes, Action onNo = null)
		{
			Data = data;
			this.OnConfirm = onYes;
			_onCancel = onNo;
			ChangeData(isOpen: true);
		}

		public void ConfirmYes()
		{
			this.OnConfirm?.Invoke();
			ClearCallbacks();
			ChangeData(isOpen: false);
		}

		public void Cancel()
		{
			_onCancel?.Invoke();
			ClearCallbacks();
			ChangeData(isOpen: false);
		}

		private void ClearCallbacks()
		{
			this.OnConfirm = null;
			_onCancel = null;
		}

		private void ChangeData(bool isOpen)
		{
			IsOpen = isOpen;
			this.OnOpenChanged?.Invoke();
		}
	}
}
