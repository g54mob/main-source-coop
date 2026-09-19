using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace solidocean
{
	public class FixScrollRect : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler, IScrollHandler
	{
		public ScrollRect MainScroll;

		public void OnBeginDrag(PointerEventData eventData)
		{
			MainScroll.OnBeginDrag(eventData);
		}

		public void OnDrag(PointerEventData eventData)
		{
			MainScroll.OnDrag(eventData);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			MainScroll.OnEndDrag(eventData);
		}

		public void OnScroll(PointerEventData data)
		{
			MainScroll.OnScroll(data);
		}
	}
}
