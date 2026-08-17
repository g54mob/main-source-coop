using UnityEngine;
using UnityEngine.EventSystems;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwButtonRedoAll")]
	[AddComponentMenu("CW/Paint Core/CW Button Redo All")]
	public class CwButtonRedoAll : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		public void OnPointerClick(PointerEventData eventData)
		{
			RedoAll();
		}

		[ContextMenu("Redo All")]
		public void RedoAll()
		{
			CwStateManager.RedoAll();
		}

		protected virtual void Update()
		{
			CanvasGroup component = GetComponent<CanvasGroup>();
			if (component != null)
			{
				component.alpha = (CwStateManager.CanRedo ? 1f : 0.5f);
			}
		}
	}
}
