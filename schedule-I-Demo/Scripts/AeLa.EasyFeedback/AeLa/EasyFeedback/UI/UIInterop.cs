using System;
using AeLa.EasyFeedback.UI.Interfaces;
using AeLa.EasyFeedback.UI.TMP;
using AeLa.EasyFeedback.UI.UGUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AeLa.EasyFeedback.UI
{
	internal static class UIInterop
	{
		public static IText GetText(GameObject go)
		{
			if ((bool)UIInteropWrapper<TMP_Text>.GetTarget(go))
			{
				return new TMPTextWrapper(UIInteropWrapper<TMP_Text>.GetTarget(go));
			}
			if ((bool)UIInteropWrapper<Text>.GetTarget(go))
			{
				return new UGUITextWrapper(UIInteropWrapper<Text>.GetTarget(go));
			}
			throw GetNonCompatibleException("Text", go);
		}

		internal static IDropdown GetDropdown(GameObject gameObject)
		{
			if ((bool)UIInteropWrapper<TMP_Dropdown>.GetTarget(gameObject))
			{
				return new TMPDropdownWrapper(UIInteropWrapper<TMP_Dropdown>.GetTarget(gameObject));
			}
			if ((bool)UIInteropWrapper<Dropdown>.GetTarget(gameObject))
			{
				return new UGUIDropdownWrapper(UIInteropWrapper<Dropdown>.GetTarget(gameObject));
			}
			throw GetNonCompatibleException("Dropdown", gameObject);
		}

		internal static IInputField GetInputField(GameObject gameObject, bool soft = false)
		{
			if ((bool)UIInteropWrapper<TMP_InputField>.GetTarget(gameObject))
			{
				return new TMPInputFieldWrapper(UIInteropWrapper<TMP_InputField>.GetTarget(gameObject));
			}
			if ((bool)UIInteropWrapper<InputField>.GetTarget(gameObject))
			{
				return new UGUIInputFieldWrapper(UIInteropWrapper<InputField>.GetTarget(gameObject));
			}
			if (soft)
			{
				return null;
			}
			throw GetNonCompatibleException("InputField", gameObject);
		}

		internal static IText GetText(Text unityText, TMP_Text tmpText)
		{
			if ((bool)unityText)
			{
				return new UGUITextWrapper(unityText);
			}
			if ((bool)tmpText)
			{
				return new TMPTextWrapper(tmpText);
			}
			return null;
		}

		private static Exception GetNonCompatibleException(string elementType, GameObject go)
		{
			return new Exception("Could not find a " + elementType + " component attached to " + go);
		}
	}
}
