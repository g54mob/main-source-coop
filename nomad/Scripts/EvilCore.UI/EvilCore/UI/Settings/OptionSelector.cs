using System;
using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.Audio;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.Settings
{
	public class OptionSelector : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		private enum SelectorMode
		{
			Discrete = 0,
			Range = 1,
			Text = 2
		}

		[SerializeField]
		private Button leftButton;

		[SerializeField]
		private Button rightButton;

		[SerializeField]
		private TextMeshProUGUI valueText;

		[SerializeField]
		private TextMeshProUGUI labelText;

		[SerializeField]
		private Image backgroundImage;

		[SerializeField]
		private bool applyImmediately = true;

		[Header("Sound")]
		[SerializeField]
		private SoundID hoverSound;

		[SerializeField]
		private SoundID changeSound;

		[Header("Hover")]
		[SerializeField]
		private Color hoverBackgroundColor = new Color(1f, 1f, 1f, 0.1f);

		[SerializeField]
		private Color hoverTextColor = Color.white;

		[SerializeField]
		private Color hoverArrowColor = Color.white;

		[SerializeField]
		private Color hoverDotColor = Color.white;

		[SerializeField]
		private Color hoverFillColor = Color.white;

		[SerializeField]
		private float hoverFadeDuration = 0.15f;

		[Header("Dot Indicators (Discrete Mode)")]
		[SerializeField]
		private RectTransform dotContainer;

		[SerializeField]
		private Sprite dotEmpty;

		[SerializeField]
		private Sprite dotFilled;

		[SerializeField]
		private float dotSize = 8f;

		[SerializeField]
		private Color dotColor = Color.white;

		[Header("Fill Bar (Range Mode)")]
		[SerializeField]
		private RectTransform fillBarContainer;

		[SerializeField]
		private Image fillBarImage;

		[Header("Text Input Mode")]
		[SerializeField]
		private TMP_InputField inputField;

		[SerializeField]
		private GameObject arrowsGroup;

		[Header("Animation")]
		[SerializeField]
		private float transitionDuration = 0.12f;

		[SerializeField]
		private float dotPulseScale = 1.4f;

		[Header("Value Text")]
		[SerializeField]
		private float minFontSize = 10f;

		private List<string> _options = new List<string>();

		private int _currentIndex;

		private SelectorMode _mode;

		private float _rangeMin;

		private float _rangeMax;

		private float _rangeStep;

		private float _rangeValue;

		private string _rangeSuffix = "";

		private int _rangeDecimals;

		private int _appliedIndex;

		private float _appliedRangeValue;

		private string _textValue = "";

		private string _appliedTextValue = "";

		private Color _defaultBackgroundColor;

		private Color _defaultValueTextColor;

		private Color _defaultLabelTextColor;

		private Color _defaultLeftArrowColor;

		private Color _defaultRightArrowColor;

		private Color _defaultDotColor;

		private Color _defaultFillColor;

		private bool _defaultsCaptured;

		private float _defaultFontSize;

		private bool _isHovered;

		private Tween _bgTween;

		private Tween _valueTween;

		private Tween _labelTween;

		private Tween _leftArrowTween;

		private Tween _rightArrowTween;

		private Tween _fillTween;

		[Inject]
		private IAudioManager _audioManager;

		private readonly List<Image> _dotImages = new List<Image>();

		private bool IsRangeMode => _mode == SelectorMode.Range;

		private bool IsTextMode => _mode == SelectorMode.Text;

		public bool ApplyImmediately => applyImmediately;

		public bool IsDirty => _mode switch
		{
			SelectorMode.Range => !Mathf.Approximately(_rangeValue, _appliedRangeValue), 
			SelectorMode.Text => _textValue != _appliedTextValue, 
			_ => _currentIndex != _appliedIndex, 
		};

		public int CurrentIndex => _currentIndex;

		public float RangeValue => _rangeValue;

		public string CurrentText => _textValue;

		public string CurrentValue => _mode switch
		{
			SelectorMode.Range => FormatRangeValue(), 
			SelectorMode.Text => _textValue, 
			_ => (_currentIndex >= 0 && _currentIndex < _options.Count) ? _options[_currentIndex] : "", 
		};

		public event Action<int> OnValueChanged;

		public event Action<float> OnRangeValueChanged;

		public event Action<string> OnTextChanged;

		public event Action OnDirtyStateChanged;

		private void Awake()
		{
			leftButton?.onClick.AddListener(Previous);
			rightButton?.onClick.AddListener(Next);
			CaptureDefaultColors();
			if (valueText != null)
			{
				_defaultFontSize = valueText.fontSize;
			}
			if (inputField != null)
			{
				inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
			}
		}

		private void OnDestroy()
		{
			leftButton?.onClick.RemoveAllListeners();
			rightButton?.onClick.RemoveAllListeners();
			if (inputField != null)
			{
				inputField.onValueChanged.RemoveListener(OnInputFieldValueChanged);
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_isHovered = true;
			FadeHoverColors(hover: true);
			PlaySound(hoverSound);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_isHovered = false;
			FadeHoverColors(hover: false);
		}

		public void SetOptions(List<string> options)
		{
			_mode = SelectorMode.Discrete;
			_options = options ?? new List<string>();
			_currentIndex = Mathf.Clamp(_currentIndex, 0, Mathf.Max(0, _options.Count - 1));
			SetInputFieldActive(active: false);
			SetArrowsActive(active: true);
			SetFillBarActive(active: false);
			SetDotContainerActive(active: true);
			RebuildDots();
			UpdateDisplay();
		}

		public void SetRange(float min, float max, float step, string suffix = "", int decimals = 0)
		{
			_mode = SelectorMode.Range;
			_rangeMin = min;
			_rangeMax = max;
			_rangeStep = Mathf.Max(step, 0.001f);
			_rangeSuffix = suffix;
			_rangeDecimals = decimals;
			_rangeValue = Mathf.Clamp(_rangeValue, min, max);
			ClearDots();
			SetInputFieldActive(active: false);
			SetArrowsActive(active: true);
			SetDotContainerActive(active: false);
			SetFillBarActive(active: true);
			UpdateDisplay();
		}

		public void SetTextInput(string initialValue = "", int maxLength = 0, string placeholder = null, bool masked = false, bool numeric = false)
		{
			_mode = SelectorMode.Text;
			_textValue = initialValue ?? "";
			_appliedTextValue = _textValue;
			ClearDots();
			SetDotContainerActive(active: false);
			SetFillBarActive(active: false);
			SetArrowsActive(active: false);
			SetInputFieldActive(active: true);
			if (inputField != null)
			{
				inputField.characterLimit = Mathf.Max(0, maxLength);
				inputField.contentType = (masked ? TMP_InputField.ContentType.Password : (numeric ? TMP_InputField.ContentType.IntegerNumber : TMP_InputField.ContentType.Standard));
				inputField.SetTextWithoutNotify(_textValue);
				if (placeholder != null && inputField.placeholder is TMP_Text tMP_Text)
				{
					tMP_Text.text = placeholder;
				}
				inputField.ForceLabelUpdate();
			}
		}

		public void SetValueWithoutNotify(int index)
		{
			if (_mode == SelectorMode.Discrete)
			{
				_currentIndex = Mathf.Clamp(index, 0, Mathf.Max(0, _options.Count - 1));
				_appliedIndex = _currentIndex;
				UpdateDisplay();
			}
		}

		public void SetRangeValueWithoutNotify(float value)
		{
			if (IsRangeMode)
			{
				_rangeValue = Mathf.Clamp(value, _rangeMin, _rangeMax);
				_appliedRangeValue = _rangeValue;
				UpdateDisplay();
			}
		}

		public void SetPlaceholder(string placeholder)
		{
			if (inputField != null && inputField.placeholder is TMP_Text tMP_Text)
			{
				tMP_Text.text = placeholder ?? "";
			}
		}

		public void SetTextValueWithoutNotify(string value)
		{
			if (IsTextMode)
			{
				_textValue = value ?? "";
				_appliedTextValue = _textValue;
				if (inputField != null)
				{
					inputField.SetTextWithoutNotify(_textValue);
				}
			}
		}

		public void FocusTextInput()
		{
			if (!(inputField == null))
			{
				inputField.Select();
				inputField.ActivateInputField();
			}
		}

		public void SetInteractable(bool interactable)
		{
			if (leftButton != null)
			{
				leftButton.interactable = interactable;
			}
			if (rightButton != null)
			{
				rightButton.interactable = interactable;
			}
			if (valueText != null)
			{
				valueText.alpha = (interactable ? 1f : 0.5f);
			}
			if (inputField != null)
			{
				inputField.interactable = interactable;
			}
		}

		public void ForceNotify()
		{
			switch (_mode)
			{
			case SelectorMode.Range:
				_appliedRangeValue = _rangeValue;
				this.OnRangeValueChanged?.Invoke(_rangeValue);
				break;
			case SelectorMode.Text:
				_appliedTextValue = _textValue;
				this.OnTextChanged?.Invoke(_textValue);
				break;
			default:
				_appliedIndex = _currentIndex;
				this.OnValueChanged?.Invoke(_currentIndex);
				break;
			}
			this.OnDirtyStateChanged?.Invoke();
		}

		private void Next()
		{
			if (IsTextMode)
			{
				return;
			}
			if (IsRangeMode)
			{
				float num = Mathf.Min(_rangeValue + _rangeStep, _rangeMax);
				if (!Mathf.Approximately(num, _rangeValue))
				{
					_rangeValue = num;
					PlaySound(changeSound);
					UpdateDisplay();
					if (applyImmediately)
					{
						_appliedRangeValue = _rangeValue;
						this.OnRangeValueChanged?.Invoke(_rangeValue);
					}
					else
					{
						this.OnDirtyStateChanged?.Invoke();
					}
				}
			}
			else if (_options.Count != 0)
			{
				int currentIndex = _currentIndex;
				_currentIndex = (_currentIndex + 1) % _options.Count;
				PlaySound(changeSound);
				UpdateDisplayWithDotTransition(currentIndex);
				if (applyImmediately)
				{
					_appliedIndex = _currentIndex;
					this.OnValueChanged?.Invoke(_currentIndex);
				}
				else
				{
					this.OnDirtyStateChanged?.Invoke();
				}
			}
		}

		private void Previous()
		{
			if (IsTextMode)
			{
				return;
			}
			if (IsRangeMode)
			{
				float num = Mathf.Max(_rangeValue - _rangeStep, _rangeMin);
				if (!Mathf.Approximately(num, _rangeValue))
				{
					_rangeValue = num;
					PlaySound(changeSound);
					UpdateDisplay();
					if (applyImmediately)
					{
						_appliedRangeValue = _rangeValue;
						this.OnRangeValueChanged?.Invoke(_rangeValue);
					}
					else
					{
						this.OnDirtyStateChanged?.Invoke();
					}
				}
			}
			else if (_options.Count != 0)
			{
				int currentIndex = _currentIndex;
				_currentIndex = (_currentIndex - 1 + _options.Count) % _options.Count;
				PlaySound(changeSound);
				UpdateDisplayWithDotTransition(currentIndex);
				if (applyImmediately)
				{
					_appliedIndex = _currentIndex;
					this.OnValueChanged?.Invoke(_currentIndex);
				}
				else
				{
					this.OnDirtyStateChanged?.Invoke();
				}
			}
		}

		private void UpdateDisplay()
		{
			if (!IsTextMode)
			{
				UpdateValueText();
				if (IsRangeMode)
				{
					AnimateFillBar();
				}
				else
				{
					UpdateDotsImmediate();
				}
			}
		}

		private void UpdateDisplayWithDotTransition(int previousIndex)
		{
			UpdateValueText();
			AnimateDotTransition(previousIndex, _currentIndex);
		}

		private void UpdateValueText()
		{
			if (!(valueText == null))
			{
				valueText.text = (IsRangeMode ? FormatRangeValue() : ((_currentIndex >= 0 && _currentIndex < _options.Count) ? _options[_currentIndex] : "-"));
				AutoFitFontSize();
			}
		}

		private string FormatRangeValue()
		{
			string text = ((_rangeDecimals <= 0) ? Mathf.RoundToInt(_rangeValue).ToString() : _rangeValue.ToString($"F{_rangeDecimals}"));
			if (!string.IsNullOrEmpty(_rangeSuffix))
			{
				return text + _rangeSuffix;
			}
			return text;
		}

		private void AutoFitFontSize()
		{
			if (!(valueText == null) && !(_defaultFontSize <= 0f))
			{
				valueText.fontSize = _defaultFontSize;
				valueText.ForceMeshUpdate();
				if (valueText.isTextOverflowing && minFontSize < _defaultFontSize)
				{
					valueText.enableAutoSizing = true;
					valueText.fontSizeMin = minFontSize;
					valueText.fontSizeMax = _defaultFontSize;
					valueText.ForceMeshUpdate();
					float fontSize = valueText.fontSize;
					valueText.enableAutoSizing = false;
					valueText.fontSize = fontSize;
				}
			}
		}

		private void RebuildDots()
		{
			ClearDots();
			if (!(dotContainer == null) && !(dotEmpty == null) && !(dotFilled == null) && _options.Count > 1)
			{
				for (int i = 0; i < _options.Count; i++)
				{
					GameObject obj = new GameObject($"Dot_{i}", typeof(RectTransform), typeof(Image));
					obj.transform.SetParent(dotContainer, worldPositionStays: false);
					obj.GetComponent<RectTransform>().sizeDelta = new Vector2(dotSize, dotSize);
					Image component = obj.GetComponent<Image>();
					component.sprite = ((i == _currentIndex) ? dotFilled : dotEmpty);
					component.color = (_isHovered ? hoverDotColor : dotColor);
					component.raycastTarget = false;
					_dotImages.Add(component);
				}
			}
		}

		private void UpdateDotsImmediate()
		{
			for (int i = 0; i < _dotImages.Count; i++)
			{
				if (!(_dotImages[i] == null))
				{
					_dotImages[i].sprite = ((i == _currentIndex) ? dotFilled : dotEmpty);
				}
			}
		}

		private void AnimateDotTransition(int fromIndex, int toIndex)
		{
			if (_dotImages.Count == 0)
			{
				return;
			}
			float d = transitionDuration;
			if (fromIndex >= 0 && fromIndex < _dotImages.Count && _dotImages[fromIndex] != null)
			{
				_dotImages[fromIndex].sprite = dotEmpty;
				Tween.Scale(_dotImages[fromIndex].GetComponent<RectTransform>(), Vector3.one, d, Ease.OutQuad, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
			if (toIndex >= 0 && toIndex < _dotImages.Count && _dotImages[toIndex] != null)
			{
				_dotImages[toIndex].sprite = dotFilled;
				RectTransform component = _dotImages[toIndex].GetComponent<RectTransform>();
				Tween.Scale(component, Vector3.one * dotPulseScale, d * 0.5f, Ease.OutQuad, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true).OnComplete(component, delegate(RectTransform rt)
				{
					Tween.Scale(rt, Vector3.one, d * 0.5f, Ease.InQuad, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
				});
			}
		}

		private void ClearDots()
		{
			foreach (Image dotImage in _dotImages)
			{
				if (dotImage != null)
				{
					UnityEngine.Object.Destroy(dotImage.gameObject);
				}
			}
			_dotImages.Clear();
		}

		private void SetDotContainerActive(bool active)
		{
			if (dotContainer != null)
			{
				dotContainer.gameObject.SetActive(active);
			}
		}

		private void SetFillBarActive(bool active)
		{
			if (fillBarContainer != null)
			{
				fillBarContainer.gameObject.SetActive(active);
			}
		}

		private void SetArrowsActive(bool active)
		{
			if (arrowsGroup != null)
			{
				arrowsGroup.SetActive(active);
			}
		}

		private void SetInputFieldActive(bool active)
		{
			if (inputField != null)
			{
				inputField.gameObject.SetActive(active);
			}
		}

		private void OnInputFieldValueChanged(string value)
		{
			_textValue = value ?? "";
			this.OnTextChanged?.Invoke(_textValue);
			this.OnDirtyStateChanged?.Invoke();
		}

		private void AnimateFillBar()
		{
			if (!(fillBarImage == null))
			{
				float num = _rangeMax - _rangeMin;
				float endValue = ((num > 0f) ? Mathf.Clamp01((_rangeValue - _rangeMin) / num) : 0f);
				_fillTween.Stop();
				_fillTween = Tween.UIFillAmount(fillBarImage, endValue, transitionDuration, Ease.OutQuad, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
		}

		private void CaptureDefaultColors()
		{
			if (!_defaultsCaptured)
			{
				_defaultsCaptured = true;
				_defaultBackgroundColor = ((backgroundImage != null) ? backgroundImage.color : Color.clear);
				_defaultValueTextColor = ((valueText != null) ? valueText.color : Color.white);
				_defaultLabelTextColor = ((labelText != null) ? labelText.color : Color.white);
				_defaultDotColor = dotColor;
				_defaultFillColor = ((fillBarImage != null) ? fillBarImage.color : Color.white);
				Image image = ((leftButton != null) ? leftButton.GetComponent<Image>() : null);
				Image image2 = ((rightButton != null) ? rightButton.GetComponent<Image>() : null);
				_defaultLeftArrowColor = ((image != null) ? image.color : Color.white);
				_defaultRightArrowColor = ((image2 != null) ? image2.color : Color.white);
			}
		}

		private void FadeHoverColors(bool hover)
		{
			float duration = hoverFadeDuration;
			Color endValue = (hover ? hoverBackgroundColor : _defaultBackgroundColor);
			Color endValue2 = (hover ? hoverTextColor : _defaultValueTextColor);
			Color endValue3 = (hover ? hoverTextColor : _defaultLabelTextColor);
			Color endValue4 = (hover ? hoverArrowColor : _defaultLeftArrowColor);
			Color endValue5 = (hover ? hoverArrowColor : _defaultRightArrowColor);
			Color endValue6 = (hover ? hoverDotColor : _defaultDotColor);
			Color endValue7 = (hover ? hoverFillColor : _defaultFillColor);
			if (backgroundImage != null)
			{
				_bgTween.Stop();
				_bgTween = Tween.Color(backgroundImage, endValue, duration, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
			if (valueText != null)
			{
				_valueTween.Stop();
				_valueTween = Tween.Color(valueText, endValue2, duration, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
			if (labelText != null)
			{
				_labelTween.Stop();
				_labelTween = Tween.Color(labelText, endValue3, duration, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
			Image image = ((leftButton != null) ? leftButton.GetComponent<Image>() : null);
			if (image != null)
			{
				_leftArrowTween.Stop();
				_leftArrowTween = Tween.Color(image, endValue4, duration, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
			Image image2 = ((rightButton != null) ? rightButton.GetComponent<Image>() : null);
			if (image2 != null)
			{
				_rightArrowTween.Stop();
				_rightArrowTween = Tween.Color(image2, endValue5, duration, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
			foreach (Image dotImage in _dotImages)
			{
				if (dotImage != null)
				{
					Tween.Color(dotImage, endValue6, duration, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
				}
			}
			if (fillBarImage != null)
			{
				Tween.Color(fillBarImage, endValue7, duration, default(Easing), 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
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
