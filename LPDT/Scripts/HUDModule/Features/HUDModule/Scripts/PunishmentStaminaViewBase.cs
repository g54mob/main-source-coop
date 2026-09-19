using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.HUDModule.Scripts
{
	public abstract class PunishmentStaminaViewBase : ViewBehaviour
	{
		public TMP_Text StaminaText;

		public TMP_Text StaminaMaxText;

		public Image StaminaInsideIcon;

		public Image StaminaOutsideIcon;

		public Color StaminaTextMinColor;

		public Color StaminaIconMinColor;

		public Color StaminaMaxTextMinColor;

		public RectTransform StaminaIconRect;

		public RectTransform TextRect;

		public float ShakeMaxMagnitude = 5f;

		public float ShakeFrequency = 30f;
	}
}
