using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts
{
	public class ThankPlayingPopupViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public Selectable FirstSelectable { get; private set; }

		[field: SerializeField]
		public Selectable ExitSelectable { get; private set; }

		[field: SerializeField]
		public GameObject ContentContainer { get; private set; }

		public event Action OnCloseClicked;

		protected void InvokeCloseClicked()
		{
			this.OnCloseClicked?.Invoke();
		}
	}
}
