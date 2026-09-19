using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.UINavigationModuleRealization.Scripts.TextInput
{
	public class TextInputFocusService : ITextInputFocusService
	{
		public bool IsTextInputEditing
		{
			get
			{
				GameObject gameObject = EventSystem.current?.currentSelectedGameObject;
				TMP_InputField tMP_InputField = ((gameObject != null) ? gameObject.GetComponentInParent<TMP_InputField>() : null);
				if (tMP_InputField != null)
				{
					return tMP_InputField.isFocused;
				}
				return false;
			}
		}
	}
}
