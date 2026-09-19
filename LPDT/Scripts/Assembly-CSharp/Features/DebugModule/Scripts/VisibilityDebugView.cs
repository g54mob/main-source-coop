using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.DebugModule.Scripts
{
	public class VisibilityDebugView : VisibilityDebugViewBase
	{
		[SerializeField]
		private Toggle _armsVisibilityToggle;

		[SerializeField]
		private Toggle _uiVisibilityToggle;

		public override event Action<bool> OnArmsActiveSwitched;

		public override event Action<bool> OnUIActiveSwitched;

		protected override void OnEnable()
		{
			base.OnEnable();
			_armsVisibilityToggle.onValueChanged.AddListener(InvokeOnArmsActiveSwitched);
			_uiVisibilityToggle.onValueChanged.AddListener(InvokeOnUiActiveSwitched);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_armsVisibilityToggle.onValueChanged.RemoveListener(InvokeOnArmsActiveSwitched);
			_uiVisibilityToggle.onValueChanged.RemoveListener(InvokeOnUiActiveSwitched);
		}

		private void InvokeOnArmsActiveSwitched(bool isActive)
		{
			OnArmsActiveSwitched?.Invoke(isActive);
		}

		private void InvokeOnUiActiveSwitched(bool isActive)
		{
			OnUIActiveSwitched?.Invoke(isActive);
		}

		public override void RefreshUiToggle(bool isActive)
		{
			_uiVisibilityToggle.isOn = isActive;
		}

		public override void RefreshArmsToggle(bool isActive)
		{
			_armsVisibilityToggle.isOn = isActive;
		}
	}
}
