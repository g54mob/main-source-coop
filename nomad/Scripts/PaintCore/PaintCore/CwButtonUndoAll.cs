using UnityEngine;
using UnityEngine.EventSystems;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwButtonUndoAll")]
	[AddComponentMenu("CW/Paint Core/CW Button Undo All")]
	public class CwButtonUndoAll : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		public void OnPointerClick(PointerEventData eventData)
		{
			UndoAll();
		}

		[ContextMenu("Undo All")]
		public void UndoAll()
		{
			CwStateManager.UndoAll();
		}

		protected virtual void Update()
		{
			CanvasGroup component = GetComponent<CanvasGroup>();
			if (component != null)
			{
				component.alpha = (CwStateManager.CanUndo ? 1f : 0.5f);
			}
		}
	}
}
