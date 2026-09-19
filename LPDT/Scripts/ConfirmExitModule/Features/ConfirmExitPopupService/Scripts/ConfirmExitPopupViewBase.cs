using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.ConfirmExitPopupService.Scripts
{
	public abstract class ConfirmExitPopupViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public Selectable FirstButtonToSelect { get; private set; }

		[field: SerializeField]
		public RectTransform AnimationTarget { get; private set; }

		public event Action OnYesClicked;

		public event Action OnNoClicked;

		public abstract void SetVisible(bool isVisible);

		public abstract void SetTitleText(string text);

		public abstract void SetDescriptionText(string text);

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
