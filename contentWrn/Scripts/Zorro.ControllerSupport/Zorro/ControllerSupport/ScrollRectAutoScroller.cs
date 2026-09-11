using UnityEngine;
using UnityEngine.UI;
using Zorro.Core;

namespace Zorro.ControllerSupport
{
	public class ScrollRectAutoScroller : MonoBehaviour
	{
		private ScrollRect m_scrollRect;

		private Optionable<float> m_targetScrollPosition;

		private void Start()
		{
			m_scrollRect = GetComponent<ScrollRect>();
			m_targetScrollPosition = Optionable<float>.None;
		}

		public void SetScrollPosition(float scrollPosition)
		{
			if (InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad)
			{
				m_targetScrollPosition = Optionable<float>.Some(scrollPosition);
			}
		}

		private void Update()
		{
			if (InputHandler.GetCurrentUsedInputScheme() != InputScheme.Gamepad)
			{
				m_targetScrollPosition = Optionable<float>.None;
			}
			else if (m_targetScrollPosition.IsSome)
			{
				m_scrollRect.verticalScrollbar.value = Mathf.Lerp(m_scrollRect.verticalScrollbar.value, m_targetScrollPosition.Value, Time.unscaledDeltaTime * 15f);
			}
		}
	}
}
