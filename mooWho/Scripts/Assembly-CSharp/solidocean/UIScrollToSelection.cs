using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace solidocean
{
	[RequireComponent(typeof(ScrollRect))]
	public class UIScrollToSelection : MonoBehaviour
	{
		public float scrollSpeed = 10f;

		private ScrollRect m_ScrollRect;

		private RectTransform m_RectTransform;

		private RectTransform m_ContentRectTransform;

		private RectTransform m_SelectedRectTransform;

		private void Awake()
		{
			m_ScrollRect = GetComponent<ScrollRect>();
			m_RectTransform = GetComponent<RectTransform>();
			m_ContentRectTransform = m_ScrollRect.content;
		}

		private void Update()
		{
			UpdateScrollToSelected();
		}

		private void UpdateScrollToSelected()
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (!(currentSelectedGameObject == null) && !(currentSelectedGameObject.transform.parent != m_ContentRectTransform.transform))
			{
				m_SelectedRectTransform = currentSelectedGameObject.GetComponent<RectTransform>();
				Vector3 vector = m_RectTransform.localPosition - m_SelectedRectTransform.localPosition;
				float num = m_ContentRectTransform.rect.height - m_RectTransform.rect.height;
				float num2 = m_ContentRectTransform.rect.height - vector.y;
				float num3 = m_ScrollRect.normalizedPosition.y * num;
				float num4 = num3 - m_SelectedRectTransform.rect.height / 2f + m_RectTransform.rect.height;
				float num5 = num3 + m_SelectedRectTransform.rect.height / 2f;
				if (num2 > num4)
				{
					float num6 = num2 - num4;
					float y = (num3 + num6) / num;
					m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, new Vector2(0f, y), scrollSpeed * Time.deltaTime);
				}
				else if (num2 < num5)
				{
					float num7 = num2 - num5;
					float y2 = (num3 + num7) / num;
					m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, new Vector2(0f, y2), scrollSpeed * Time.deltaTime);
				}
			}
		}
	}
}
