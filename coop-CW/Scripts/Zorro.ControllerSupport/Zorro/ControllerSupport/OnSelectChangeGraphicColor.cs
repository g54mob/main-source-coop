using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Zorro.ControllerSupport
{
	public class OnSelectChangeGraphicColor : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
	{
		public Graphic graphic;

		public Color selectedColor;

		private Color m_defaultColor;

		private void Awake()
		{
			m_defaultColor = graphic.color;
		}

		public void OnSelect(BaseEventData eventData)
		{
			graphic.color = selectedColor;
		}

		public void OnDeselect(BaseEventData eventData)
		{
			graphic.color = m_defaultColor;
		}
	}
}
