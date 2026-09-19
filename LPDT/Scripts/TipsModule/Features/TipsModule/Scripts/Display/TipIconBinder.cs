using Features.InputDeviceModuleRealization.Scripts;
using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Features.TipsModule.Scripts.Display
{
	internal static class TipIconBinder
	{
		public static void Apply(Image tipIcon, TMP_Text tipIconText, InputActionReference inputAction, string tipIconTextValue, IInputDeviceService inputDeviceService, InputDeviceIconCustomizationType iconCustomizationType)
		{
			if (inputAction?.action == null)
			{
				tipIcon.gameObject.SetActive(value: false);
				tipIconText.SetText(tipIconTextValue);
				return;
			}
			InputKeyVisualizationHolder bindingKey = inputDeviceService.GetBindingKey(inputAction, (int)iconCustomizationType);
			if (bindingKey != null && bindingKey.IconCustomizationData != null)
			{
				tipIcon.gameObject.SetActive(value: true);
				tipIcon.sprite = bindingKey.IconCustomizationData.Icon;
				tipIcon.rectTransform.localScale = Vector3.one * bindingKey.IconCustomizationData.Scale;
				tipIconText.SetText(string.Empty);
			}
		}
	}
}
