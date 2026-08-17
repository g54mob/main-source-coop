using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HSVPicker
{
	[RequireComponent(typeof(BoxSlider), typeof(RawImage))]
	[ExecuteInEditMode]
	[DefaultExecutionOrder(10)]
	public class SVBoxSlider : MonoBehaviour, IEndDragHandler, IEventSystemHandler
	{
		public ColorPicker picker;

		private BoxSlider slider;

		private RawImage image;

		private int textureWidth = 128;

		private int textureHeight = 128;

		private float lastH = -1f;

		private bool listen = true;

		[Header("Event")]
		public SliderOnChangeEndEvent onSliderChangeEndEvent = new SliderOnChangeEndEvent();

		public RectTransform rectTransform => base.transform as RectTransform;

		private void Awake()
		{
			slider = GetComponent<BoxSlider>();
			image = GetComponent<RawImage>();
		}

		private void OnEnable()
		{
			if (Application.isPlaying && picker != null)
			{
				slider.onValueChanged.AddListener(SliderChanged);
				picker.onHSVChanged.AddListener(HSVChanged);
				HSVChanged(picker.H, picker.S, picker.V);
			}
			if (Application.isPlaying)
			{
				RegenerateSVTexture();
			}
		}

		private void OnDisable()
		{
			if (picker != null)
			{
				slider.onValueChanged.RemoveListener(SliderChanged);
				picker.onHSVChanged.RemoveListener(HSVChanged);
			}
		}

		private void OnDestroy()
		{
			if (image.texture != null)
			{
				Object.DestroyImmediate(image.texture);
			}
		}

		private void SliderChanged(float saturation, float value)
		{
			if (listen)
			{
				picker.AssignColor(ColorValues.Saturation, saturation);
				picker.AssignColor(ColorValues.Value, value);
			}
			listen = true;
		}

		private void HSVChanged(float h, float s, float v)
		{
			if (!lastH.Equals(h))
			{
				lastH = h;
				RegenerateSVTexture();
			}
			if (!s.Equals(slider.normalizedValue))
			{
				listen = false;
				slider.normalizedValue = s;
			}
			if (!v.Equals(slider.normalizedValueY))
			{
				listen = false;
				slider.normalizedValueY = v;
			}
		}

		private void RegenerateSVTexture()
		{
			double h = ((picker != null) ? (picker.H * 360f) : 0f);
			if (image.texture != null)
			{
				Object.DestroyImmediate(image.texture);
			}
			Texture2D texture2D = new Texture2D(textureWidth, textureHeight);
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.hideFlags = HideFlags.DontSave;
			for (int i = 0; i < textureWidth; i++)
			{
				Color[] array = new Color[textureHeight];
				for (int j = 0; j < textureHeight; j++)
				{
					array[j] = HSVUtil.ConvertHsvToRgb(h, (float)i / (float)textureWidth, (float)j / (float)textureHeight, 1f);
				}
				texture2D.SetPixels(i, 0, 1, textureHeight, array);
			}
			texture2D.Apply();
			image.texture = texture2D;
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			onSliderChangeEndEvent.Invoke(slider.normalizedValue);
		}
	}
}
