using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.DebugModule.Scripts
{
	public class ToggleDebugWindowView : ToggleDebugWindowViewBase
	{
		[SerializeField]
		private Button _button;

		private IWindowsService _windowsService;

		protected override void OnEnable()
		{
			base.OnEnable();
			_button.onClick.AddListener(HandleOnButtonClicked);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_button.onClick.RemoveListener(HandleOnButtonClicked);
		}

		private void HandleOnButtonClicked()
		{
			OnEnableDebugButtonClick?.Invoke();
		}
	}
}
