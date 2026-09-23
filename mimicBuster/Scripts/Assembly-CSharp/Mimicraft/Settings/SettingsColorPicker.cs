using System;
using Mimicraft.Localization;
using Mimicraft.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mimicraft.Settings
{
	public class SettingsColorPicker : MonoBehaviour
	{
		[Tooltip("Seçicinin tamamını taşıyan obje. Kapalı başlamalı.")]
		[SerializeField]
		private GameObject panelRoot;

		[Tooltip("HSV çarkı. Editördeki boya panelinde kullanılanın aynısı - PaintPanelView'a bak.")]
		[SerializeField]
		private ColorWheelWidget wheel;

		[Tooltip("Parlaklık (V). Çark yalnızca renk tonu ve doygunluğu taşır, üçüncü eksen buraya düşer.")]
		[SerializeField]
		private Slider valueSlider;

		[Tooltip("Saydamlık. Gizmo ve ızgara renklerinde gerçekten işe yarar: modelin üstüne çizilen bir ızgaranın ne kadar baskın olacağını belirleyen şey bu.")]
		[SerializeField]
		private Slider alphaSlider;

		[SerializeField]
		private Image preview;

		[SerializeField]
		private TextMeshProUGUI titleLabel;

		[SerializeField]
		private Button closeButton;

		[Tooltip("Rengi kendi varsayılanına döndürür. Hangi varsayılan olduğunu açan taraf söyler.")]
		[SerializeField]
		private Button resetButton;

		private Action<Color> write;

		private Color fallback = Color.white;

		private Color current = Color.white;

		private bool applyingToControls;

		public bool IsOpen
		{
			get
			{
				if (panelRoot != null)
				{
					return panelRoot.activeSelf;
				}
				return false;
			}
		}

		private void Awake()
		{
			if (wheel != null)
			{
				wheel.ColorChanged += OnWheelChanged;
			}
			if (valueSlider != null)
			{
				valueSlider.minValue = 0f;
				valueSlider.maxValue = 1f;
				valueSlider.onValueChanged.RemoveAllListeners();
				valueSlider.onValueChanged.AddListener(OnValueChanged);
			}
			if (alphaSlider != null)
			{
				alphaSlider.minValue = 0f;
				alphaSlider.maxValue = 1f;
				alphaSlider.onValueChanged.RemoveAllListeners();
				alphaSlider.onValueChanged.AddListener(OnAlphaChanged);
			}
			if (closeButton != null)
			{
				closeButton.onClick.RemoveAllListeners();
				closeButton.onClick.AddListener(Close);
			}
			if (resetButton != null)
			{
				resetButton.onClick.RemoveAllListeners();
				resetButton.onClick.AddListener(delegate
				{
					Commit(fallback, syncControls: true);
				});
			}
			if (panelRoot != null)
			{
				panelRoot.SetActive(value: false);
			}
		}

		private void OnDestroy()
		{
			if (wheel != null)
			{
				wheel.ColorChanged -= OnWheelChanged;
			}
		}

		private void Update()
		{
			if (IsOpen && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				GameMenuState.RequestEscape(100, Close);
			}
		}

		public void Open(string titleKey, Color colour, Color defaultColor, Action<Color> onChanged)
		{
			if (!(panelRoot == null))
			{
				write = onChanged;
				fallback = defaultColor;
				current = colour;
				if (titleLabel != null)
				{
					LocalizedText.Attach(titleLabel, titleKey);
				}
				SyncControls();
				panelRoot.SetActive(value: true);
			}
		}

		public void Close()
		{
			if (!(panelRoot == null))
			{
				panelRoot.SetActive(value: false);
				write = null;
			}
		}

		private void SyncControls()
		{
			applyingToControls = true;
			Color.RGBToHSV(current, out var _, out var _, out var V);
			if (wheel != null)
			{
				wheel.SetColor(current);
			}
			if (valueSlider != null)
			{
				valueSlider.SetValueWithoutNotify(V);
			}
			if (alphaSlider != null)
			{
				alphaSlider.SetValueWithoutNotify(current.a);
			}
			if (preview != null)
			{
				preview.color = current;
			}
			applyingToControls = false;
		}

		private void OnWheelChanged(Color colour)
		{
			if (!applyingToControls)
			{
				colour.a = ((alphaSlider != null) ? alphaSlider.value : current.a);
				Commit(colour, syncControls: false);
			}
		}

		private void OnValueChanged(float v)
		{
			if (!applyingToControls)
			{
				if (wheel != null)
				{
					wheel.SetValue(v);
					return;
				}
				Color.RGBToHSV(current, out var H, out var S, out var _);
				Color colour = Color.HSVToRGB(H, S, v);
				colour.a = current.a;
				Commit(colour, syncControls: false);
			}
		}

		private void OnAlphaChanged(float a)
		{
			if (!applyingToControls)
			{
				Color colour = current;
				colour.a = a;
				Commit(colour, syncControls: false);
			}
		}

		private void Commit(Color colour, bool syncControls)
		{
			current = colour;
			if (preview != null)
			{
				preview.color = colour;
			}
			write?.Invoke(colour);
			if (syncControls)
			{
				SyncControls();
			}
		}
	}
}
