using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Zorro.ControllerSupport
{
	public class ScrollRectAutoScrollerElement : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		private ScrollRectAutoScroller m_scrollRect;

		private float scrollPosition = 1f;

		private IEnumerator Start()
		{
			yield return 1;
			m_scrollRect = GetComponentInParent<ScrollRectAutoScroller>();
			int num = base.transform.parent.childCount - 1;
			int siblingIndex = base.transform.GetSiblingIndex();
			siblingIndex = (((float)siblingIndex < (float)num / 2f) ? (siblingIndex - 1) : siblingIndex);
			scrollPosition = Mathf.Clamp01(1f - (float)siblingIndex / (float)num);
		}

		public void OnSelect(BaseEventData eventData)
		{
			if (InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad && m_scrollRect != null)
			{
				m_scrollRect.SetScrollPosition(scrollPosition);
			}
		}
	}
}
