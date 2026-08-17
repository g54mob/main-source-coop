using AeLa.EasyFeedback.UI.Interfaces;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AeLa.EasyFeedback.UI.UGUI
{
	internal class UGUIInputFieldWrapper : UIInteropWrapper<InputField>, IInputField
	{
		public UnityEvent<string> OnValueChanged => InternalTarget.onValueChanged;

		public string Text
		{
			get
			{
				return InternalTarget.text;
			}
			set
			{
				InternalTarget.text = value;
			}
		}

		public bool IsFocused => InternalTarget.isFocused;

		public UGUIInputFieldWrapper(InputField internalTarget)
			: base(internalTarget)
		{
		}

		public void ActivateInputField()
		{
			InternalTarget.ActivateInputField();
		}

		public void DeactivateInputField()
		{
			InternalTarget.DeactivateInputField();
		}
	}
}
