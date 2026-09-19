using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace solidocean
{
	public class SwitchHandler : MonoBehaviour
	{
		[Header("Settings")]
		[SerializeField]
		private float AnimationSpeed;

		[Header("References")]
		[SerializeField]
		private RectTransform HandleImage;

		[SerializeField]
		private TextMeshProUGUI indicator;

		private Image backgroundImage;

		private Image handleImage;

		private Toggle toggle;

		private Vector2 handlePosition;

		private void Awake()
		{
			toggle = GetComponent<Toggle>();
			handlePosition = HandleImage.anchoredPosition;
			backgroundImage = HandleImage.parent.GetComponent<Image>();
			handleImage = HandleImage.GetComponent<Image>();
			toggle.onValueChanged.AddListener(OnSwitch);
			if (toggle.isOn)
			{
				OnSwitch(on: true);
			}
		}

		public void OnSwitch(bool on)
		{
			TextMeshProUGUI textMeshProUGUI = indicator;
			string text3;
			if (!on)
			{
				string text = (indicator.text = "OFF");
				text3 = text;
			}
			else
			{
				string text = (indicator.text = "ON");
				text3 = text;
			}
			textMeshProUGUI.text = text3;
			HandleImage.DOAnchorPos(on ? (handlePosition * -1f) : handlePosition, AnimationSpeed).SetEase(Ease.InSine);
		}

		private void OnDestroy()
		{
			toggle.onValueChanged.RemoveListener(OnSwitch);
		}
	}
}
