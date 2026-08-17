using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HSVPicker
{
	[RequireComponent(typeof(Slider))]
	[DefaultExecutionOrder(10)]
	public class ColorSlider : MonoBehaviour, IEndDragHandler, IEventSystemHandler
	{
		public ColorPicker hsvpicker;

		public ColorValues type;

		private Slider slider;

		private bool listen = true;

		[Header("Event")]
		public SliderOnChangeEndEvent onSliderChangeEndEvent = new SliderOnChangeEndEvent();

		private void Awake()
		{
			slider = GetComponent<Slider>();
			hsvpicker.onValueChanged.AddListener(ColorChanged);
			hsvpicker.onHSVChanged.AddListener(HSVChanged);
			slider.onValueChanged.AddListener(SliderChanged);
			ColorChanged(hsvpicker.CurrentColor);
			HSVChanged(hsvpicker.H, hsvpicker.S, hsvpicker.V);
		}

		private void OnDestroy()
		{
			hsvpicker.onValueChanged.RemoveListener(ColorChanged);
			hsvpicker.onHSVChanged.RemoveListener(HSVChanged);
			slider.onValueChanged.RemoveListener(SliderChanged);
		}

		private void ColorChanged(Color newColor)
		{
			listen = false;
			switch (type)
			{
			case ColorValues.R:
				slider.normalizedValue = newColor.r;
				break;
			case ColorValues.G:
				slider.normalizedValue = newColor.g;
				break;
			case ColorValues.B:
				slider.normalizedValue = newColor.b;
				break;
			case ColorValues.A:
				slider.normalizedValue = newColor.a;
				break;
			}
		}

		private void HSVChanged(float hue, float saturation, float value)
		{
			listen = false;
			switch (type)
			{
			case ColorValues.Hue:
				slider.normalizedValue = hue;
				break;
			case ColorValues.Saturation:
				slider.normalizedValue = saturation;
				break;
			case ColorValues.Value:
				slider.normalizedValue = value;
				break;
			}
		}

		private void SliderChanged(float newValue)
		{
			if (listen)
			{
				newValue = slider.normalizedValue;
				hsvpicker.AssignColor(type, newValue);
			}
			listen = true;
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			onSliderChangeEndEvent.Invoke(slider.normalizedValue);
		}

		public void SliderClicked(BaseEventData data)
		{
			SliderChanged(slider.value);
		}
	}
}
