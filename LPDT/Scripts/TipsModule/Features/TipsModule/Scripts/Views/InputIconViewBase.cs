using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Features.TipsModule.Scripts.Views
{
	public class InputIconViewBase : ViewBehaviour
	{
		[SerializeField]
		private TMP_Text _keyBinding;

		[SerializeField]
		private Image _imageKeyBinding;

		[SerializeField]
		private HorizontalLayoutGroup _background;

		[SerializeField]
		private RectTransform _rebuildLayoutTarget;

		[SerializeField]
		protected InputActionReference _inputAction;

		internal InputAction InputAction;

		private bool _isIconActive;

		private bool _isTextActive;

		[field: SerializeField]
		public bool IsDisableOnKeyboard { get; private set; }

		[field: SerializeField]
		public bool IsEnableInputKeyTextOnMobile { get; private set; }

		public bool IsIconActive => _isIconActive;

		public bool IsTextActive => _isTextActive;

		public event Action InputActionChanged;

		public event Action<bool> IconActive;

		public event Action<bool> TextActive;

		protected override void OnEnable()
		{
			base.OnEnable();
			if (_inputAction != null)
			{
				InputAction = _inputAction.action;
			}
		}

		public void SetInputKeyText(string bindingKey)
		{
			if (_imageKeyBinding != null)
			{
				_imageKeyBinding.gameObject.SetActive(value: false);
			}
			if (_keyBinding != null && _background != null)
			{
				_background.gameObject.SetActive(value: true);
				_keyBinding.SetText(bindingKey);
				LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_background.transform);
			}
			this.TextActive?.Invoke(obj: true);
			_isTextActive = true;
			this.IconActive?.Invoke(obj: false);
			_isIconActive = false;
			if (_rebuildLayoutTarget != null)
			{
				Canvas.ForceUpdateCanvases();
				LayoutRebuilder.ForceRebuildLayoutImmediate(_rebuildLayoutTarget);
			}
		}

		public void SetInputKeySprite(Sprite sprite, float scale)
		{
			if (_background != null)
			{
				_background.gameObject.SetActive(value: false);
			}
			if (_imageKeyBinding != null)
			{
				_imageKeyBinding.gameObject.SetActive(value: true);
				_imageKeyBinding.transform.localScale = Vector3.one * scale;
				_imageKeyBinding.sprite = sprite;
			}
			this.IconActive?.Invoke(obj: true);
			_isIconActive = true;
			this.TextActive?.Invoke(obj: false);
			_isTextActive = false;
			if (_rebuildLayoutTarget != null)
			{
				Canvas.ForceUpdateCanvases();
				LayoutRebuilder.ForceRebuildLayoutImmediate(_rebuildLayoutTarget);
			}
		}

		public void SetInputActive(bool isActive)
		{
			if (_background != null)
			{
				_background.gameObject.SetActive(isActive);
				LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_background.transform);
			}
			if (_imageKeyBinding != null)
			{
				_imageKeyBinding.gameObject.SetActive(isActive);
			}
			this.IconActive?.Invoke(obj: false);
			_isIconActive = false;
			this.TextActive?.Invoke(obj: false);
			_isTextActive = false;
			if (_rebuildLayoutTarget != null)
			{
				Canvas.ForceUpdateCanvases();
				LayoutRebuilder.ForceRebuildLayoutImmediate(_rebuildLayoutTarget);
			}
		}

		public void OverrideInputAction(InputAction inputAction)
		{
			InputAction = inputAction;
			this.InputActionChanged?.Invoke();
		}
	}
}
