using UnityEngine;
using UnityEngine.UI;

namespace Zorro.UI
{
	public class OverlayUIHandler : UIPageHandler
	{
		[SerializeField]
		private UIPage m_mainPage;

		[SerializeField]
		private Button m_closeUIButton;

		private bool shouldBeOpen;

		public bool IsOpen => base.gameObject.activeSelf;

		protected override void Start()
		{
			base.Start();
			if (!shouldBeOpen && IsOpen)
			{
				Close();
			}
			m_closeUIButton?.onClick.AddListener(Close);
		}

		public virtual void Close()
		{
			shouldBeOpen = false;
			base.gameObject.SetActive(value: false);
		}

		public virtual void Open()
		{
			shouldBeOpen = true;
			base.gameObject.SetActive(value: true);
		}

		protected override void OnTransistionedToPage(UIPage newPage)
		{
			base.OnTransistionedToPage(newPage);
			bool active = m_mainPage == newPage;
			m_closeUIButton.gameObject.SetActive(active);
		}
	}
}
