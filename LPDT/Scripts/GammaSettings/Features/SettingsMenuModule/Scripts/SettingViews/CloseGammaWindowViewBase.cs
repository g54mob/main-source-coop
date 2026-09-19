using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class CloseGammaWindowViewBase : ViewBehaviour
	{
		[SerializeField]
		private Button _closeButton;

		public event Action OnButtonClicked;

		protected override void OnEnable()
		{
			base.OnEnable();
			_closeButton.onClick.AddListener(OnCloseButtonClicked);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_closeButton.onClick.RemoveListener(OnCloseButtonClicked);
		}

		private void OnCloseButtonClicked()
		{
			this.OnButtonClicked?.Invoke();
		}
	}
}
