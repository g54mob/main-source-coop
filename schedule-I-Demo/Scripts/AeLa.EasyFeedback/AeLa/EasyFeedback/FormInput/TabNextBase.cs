using AeLa.EasyFeedback.UI;
using AeLa.EasyFeedback.UI.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace AeLa.EasyFeedback.FormInput
{
	public abstract class TabNextBase : MonoBehaviour
	{
		public Selectable Next;

		public Selectable Previous;

		protected IInputField input;

		protected IInputField nextInput;

		protected IInputField previousInput;

		protected virtual void Start()
		{
			input = UIInterop.GetInputField(base.gameObject);
			if ((bool)Next)
			{
				nextInput = UIInterop.GetInputField(Next.gameObject, soft: true);
			}
			if ((bool)Previous)
			{
				previousInput = UIInterop.GetInputField(Previous.gameObject, soft: true);
			}
		}

		protected virtual void Select(Selectable selectable)
		{
			if (!selectable)
			{
				Debug.LogWarning("Selectable is null");
				return;
			}
			input.DeactivateInputField();
			selectable.Select();
			if (TryGetInputField(selectable, out var field))
			{
				field.ActivateInputField();
			}
		}

		protected bool TryGetInputField(Selectable selectable, out IInputField field)
		{
			return (field = GetInputField(selectable)) != null;
		}

		protected IInputField GetInputField(Selectable selectable)
		{
			if (selectable == Next)
			{
				return nextInput;
			}
			if (selectable == Previous)
			{
				return previousInput;
			}
			return null;
		}

		public virtual void Copy(TabNextBase other)
		{
			Next = other.Next;
			Previous = other.Previous;
		}
	}
}
