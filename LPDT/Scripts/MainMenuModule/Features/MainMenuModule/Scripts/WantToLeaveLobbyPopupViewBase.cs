using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.MainMenuModule.Scripts
{
	public abstract class WantToLeaveLobbyPopupViewBase : ViewBehaviour
	{
		public event Action OnYesClicked;

		public event Action OnNoClicked;

		public abstract void SetVisible(bool isVisible);

		protected void InvokeYesClicked()
		{
			this.OnYesClicked?.Invoke();
		}

		protected void InvokeNoClicked()
		{
			this.OnNoClicked?.Invoke();
		}
	}
}
