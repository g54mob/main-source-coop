using System;
using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.Extensions;
using EvilCore.Inputs;
using EvilCore.Localization;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.Settings.Tabs
{
	public class InputRemappingRow : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		[SerializeField]
		private TextMeshProUGUI actionNameText;

		[SerializeField]
		private Image bindingIcon;

		[SerializeField]
		private TextMeshProUGUI bindingNameText;

		[SerializeField]
		private Button rebindButton;

		[Header("Context Badge")]
		[SerializeField]
		private Image contextBadgeIcon;

		[SerializeField]
		private TextMeshProUGUI contextBadgeLabel;

		[Header("Protected")]
		[SerializeField]
		private GameObject lockIcon;

		[Header("Conflict Colors")]
		[SerializeField]
		private Color normalBindingColor = Color.white;

		[SerializeField]
		private Color conflictBindingColor = Color.red;

		[Header("Sound")]
		[SerializeField]
		private SoundID hoverSound;

		[Header("Hover")]
		[SerializeField]
		private Image backgroundImage;

		[SerializeField]
		private Color hoverBackgroundColor = new Color(1f, 1f, 1f, 0.1f);

		[SerializeField]
		private Color hoverTextColor = Color.white;

		[SerializeField]
		private float hoverFadeDuration = 0.15f;

		[Inject]
		private IAudioManager _audioManager;

		[Inject]
		private ILocalizationService _localizationService;

		private InputRemappingActionData _data;

		private Action<InputRemappingActionData> _onRebindRequested;

		private string _friendlyBindingName;

		private bool _isProtected;

		private Color _defaultBackgroundColor;

		private Color _defaultActionTextColor;

		private Color _bindingBaseColor;

		private bool _isConflicting;

		private bool _isHovered;

		private bool _defaultsCaptured;

		private Tween _bgTween;

		private Tween _actionTween;

		private Tween _iconTween;

		private Tween _textTween;

		public InputRemappingActionData Data => _data;

		public int ActionId => _data?.ActionId ?? (-1);

		public string CurrentBindingName => _data?.CurrentBindingName;

		public int MapCategoryId => _data?.MapCategoryId ?? (-1);

		private void Awake()
		{
			base.gameObject.InjectGameObject();
			_bindingBaseColor = normalBindingColor;
			CaptureDefaultColors();
		}

		public void Setup(InputRemappingActionData data, Action<InputRemappingActionData> onRebindRequested, Sprite bindingSprite, string actionLabel, string friendlyBindingName)
		{
			_onRebindRequested = onRebindRequested;
			UpdateBinding(data, bindingSprite, actionLabel, friendlyBindingName);
			rebindButton?.onClick.RemoveAllListeners();
			rebindButton?.onClick.AddListener(OnRebindClicked);
			SetConflictHighlight(conflicting: false);
		}

		public void UpdateBinding(InputRemappingActionData data, Sprite bindingSprite, string actionLabel, string friendlyBindingName)
		{
			_data = data;
			_friendlyBindingName = friendlyBindingName;
			if (actionNameText != null)
			{
				actionNameText.text = ((!string.IsNullOrEmpty(actionLabel)) ? actionLabel : (string.IsNullOrEmpty(data.ActionDescriptiveName) ? data.ActionName : data.ActionDescriptiveName));
			}
			SetBindingSprite(bindingSprite);
		}

		public void SetContext(string label, Sprite sprite)
		{
			if (contextBadgeLabel != null)
			{
				contextBadgeLabel.text = label ?? string.Empty;
			}
			if (contextBadgeIcon != null)
			{
				bool flag = sprite != null;
				contextBadgeIcon.gameObject.SetActive(flag);
				if (flag)
				{
					contextBadgeIcon.sprite = sprite;
				}
			}
		}

		public void SetProtected(bool isProtected)
		{
			_isProtected = isProtected;
			if (lockIcon != null)
			{
				lockIcon.SetActive(isProtected);
			}
			if (rebindButton != null)
			{
				rebindButton.interactable = !isProtected;
			}
		}

		public void SetBindingSprite(Sprite sprite)
		{
			bool flag = sprite != null;
			if (bindingIcon != null)
			{
				bindingIcon.gameObject.SetActive(flag);
				if (flag)
				{
					bindingIcon.sprite = sprite;
				}
			}
			if (bindingNameText != null)
			{
				bindingNameText.gameObject.SetActive(!flag);
				if (!flag)
				{
					if (string.IsNullOrEmpty(_data?.CurrentBindingName))
					{
						bindingNameText.text = ((_localizationService != null) ? _localizationService.Localize("@settings.unbound") : "(unbound)");
					}
					else
					{
						bindingNameText.text = ((!string.IsNullOrEmpty(_friendlyBindingName)) ? _friendlyBindingName : _data.CurrentBindingName);
					}
				}
			}
			ApplyBindingColor(animate: false);
		}

		private void OnDestroy()
		{
			rebindButton?.onClick.RemoveAllListeners();
		}

		private void OnRebindClicked()
		{
			_onRebindRequested?.Invoke(_data);
		}

		public void SetInteractable(bool interactable)
		{
			if (rebindButton != null)
			{
				rebindButton.interactable = interactable && !_isProtected;
			}
		}

		public void SetConflictHighlight(bool conflicting)
		{
			_isConflicting = conflicting;
			_bindingBaseColor = (conflicting ? conflictBindingColor : normalBindingColor);
			ApplyBindingColor(_isHovered);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_isHovered = true;
			FadeHover(hover: true);
			PlaySound(hoverSound);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_isHovered = false;
			FadeHover(hover: false);
		}

		private void CaptureDefaultColors()
		{
			if (!_defaultsCaptured)
			{
				_defaultsCaptured = true;
				_defaultBackgroundColor = ((backgroundImage != null) ? backgroundImage.color : Color.clear);
				_defaultActionTextColor = ((actionNameText != null) ? actionNameText.color : Color.white);
			}
		}

		private void FadeHover(bool hover)
		{
			float duration = hoverFadeDuration;
			if (backgroundImage != null)
			{
				_bgTween.Stop();
				_bgTween = Tween.Color(backgroundImage, hover ? hoverBackgroundColor : _defaultBackgroundColor, duration, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
			if (actionNameText != null)
			{
				_actionTween.Stop();
				_actionTween = Tween.Color(actionNameText, hover ? hoverTextColor : _defaultActionTextColor, duration, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
			ApplyBindingColor(animate: true);
		}

		private void ApplyBindingColor(bool animate)
		{
			Color color = ((_isHovered && !_isConflicting) ? hoverTextColor : _bindingBaseColor);
			float num = (animate ? hoverFadeDuration : 0f);
			if (bindingIcon != null)
			{
				_iconTween.Stop();
				if (num > 0f)
				{
					_iconTween = Tween.Color(bindingIcon, color, num, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
				}
				else
				{
					bindingIcon.color = color;
				}
			}
			if (bindingNameText != null)
			{
				_textTween.Stop();
				if (num > 0f)
				{
					_textTween = Tween.Color(bindingNameText, color, num, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
				}
				else
				{
					bindingNameText.color = color;
				}
			}
		}

		private void PlaySound(SoundID sound)
		{
			if (sound.IsValid())
			{
				_audioManager?.PlayOneShotUI(sound);
			}
		}
	}
}
