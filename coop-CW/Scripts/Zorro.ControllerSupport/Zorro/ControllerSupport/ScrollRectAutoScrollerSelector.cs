using UnityEngine;
using UnityEngine.EventSystems;

namespace Zorro.ControllerSupport
{
	public class ScrollRectAutoScrollerSelector : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		private ScrollRectAutoScrollerElement m_scrollerElement;

		private void Start()
		{
			m_scrollerElement = GetComponentInParent<ScrollRectAutoScrollerElement>();
		}

		public void OnSelect(BaseEventData eventData)
		{
			if (m_scrollerElement != null)
			{
				m_scrollerElement.OnSelect(eventData);
			}
			else
			{
				Debug.LogError(base.gameObject.name + ": No ScrollRectAutoScrollerElement found in parent hierarchy.", base.gameObject);
			}
		}
	}
}
