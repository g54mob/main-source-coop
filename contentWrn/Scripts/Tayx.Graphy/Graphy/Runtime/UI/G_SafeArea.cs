using UnityEngine;

namespace Graphy.Runtime.UI
{
	[RequireComponent(typeof(RectTransform))]
	public sealed class G_SafeArea : MonoBehaviour
	{
		[SerializeField]
		private bool m_conformX = true;

		[SerializeField]
		private bool m_conformY = true;

		private RectTransform m_rectTransform;

		private Rect m_lastSafeArea = new Rect(0f, 0f, 0f, 0f);

		private void Awake()
		{
			m_rectTransform = GetComponent<RectTransform>();
			Refresh();
		}

		private void Update()
		{
			Refresh();
		}

		private void Refresh()
		{
			Rect safeArea = Screen.safeArea;
			if (safeArea != m_lastSafeArea)
			{
				ApplySafeArea(safeArea);
			}
		}

		private void ApplySafeArea(Rect r)
		{
			m_lastSafeArea = r;
			if (!m_conformX)
			{
				r.x = 0f;
				r.width = Screen.width;
			}
			if (!m_conformY)
			{
				r.y = 0f;
				r.height = Screen.height;
			}
			Vector2 position = r.position;
			Vector2 anchorMax = r.position + r.size;
			position.x /= Screen.width;
			position.y /= Screen.height;
			anchorMax.x /= Screen.width;
			anchorMax.y /= Screen.height;
			m_rectTransform.anchorMin = position;
			m_rectTransform.anchorMax = anchorMax;
		}
	}
}
