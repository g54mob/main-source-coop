using TMPro;
using UnityEngine;

namespace HSVPicker
{
	[RequireComponent(typeof(TMP_InputField))]
	[DefaultExecutionOrder(10)]
	public class HexColorField : MonoBehaviour
	{
		public ColorPicker hsvpicker;

		public bool displayAlpha;

		private TMP_InputField hexInputField;

		private void Start()
		{
			hexInputField = GetComponent<TMP_InputField>();
			hexInputField.onEndEdit.AddListener(UpdateColor);
			hsvpicker.onValueChanged.AddListener(UpdateHex);
			UpdateHex(hsvpicker.CurrentColor);
		}

		private void OnDestroy()
		{
			hexInputField.onValueChanged.RemoveListener(UpdateColor);
			hsvpicker.onValueChanged.RemoveListener(UpdateHex);
		}

		private void UpdateHex(Color newColor)
		{
			hexInputField.text = ColorToHex(newColor);
		}

		private void UpdateColor(string newHex)
		{
			if (!newHex.StartsWith("#"))
			{
				newHex = "#" + newHex;
			}
			if (ColorUtility.TryParseHtmlString(newHex, out var color))
			{
				hsvpicker.CurrentColor = color;
			}
			else
			{
				Debug.Log("hex value is in the wrong format, valid formats are: #RGB, #RGBA, #RRGGBB and #RRGGBBAA (# is optional)");
			}
		}

		private string ColorToHex(Color32 color)
		{
			if (!displayAlpha)
			{
				return $"#{color.r:X2}{color.g:X2}{color.b:X2}";
			}
			return $"#{color.r:X2}{color.g:X2}{color.b:X2}{color.a:X2}";
		}
	}
}
