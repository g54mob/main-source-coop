using System;

namespace Features.MainMenuModule.Scripts
{
	public class WantToLeaveLobbyPopupModel
	{
		public bool IsOpen { get; private set; }

		public event Action<bool> OnOpenChanged;

		public void SetOpen(bool isOpen)
		{
			if (IsOpen != isOpen)
			{
				IsOpen = isOpen;
				this.OnOpenChanged?.Invoke(IsOpen);
			}
		}
	}
}
