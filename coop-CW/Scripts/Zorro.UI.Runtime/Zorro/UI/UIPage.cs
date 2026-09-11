using UnityEngine;

namespace Zorro.UI
{
	public abstract class UIPage : PageBase
	{
		protected UIPageHandler pageHandler;

		private UISubPage currentSubPage;

		public virtual UISubPage[] GetSubPages()
		{
			return null;
		}

		public override void OnPageEnter()
		{
			base.OnPageEnter();
			if (!(pageHandler == null))
			{
				return;
			}
			pageHandler = GetComponentInParent<UIPageHandler>();
			UISubPage[] subPages = GetSubPages();
			if (subPages != null)
			{
				UISubPage[] array = subPages;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].gameObject.SetActive(value: false);
				}
				if (subPages.Length != 0)
				{
					subPages[0].gameObject.SetActive(value: true);
					currentSubPage = subPages[0];
				}
			}
		}

		public void TransistionToSubPage(UISubPage subPage, PageTransistion pageTransistion)
		{
			if (currentSubPage == subPage)
			{
				Debug.Log("Trying to transistion to current subpage");
				return;
			}
			pageTransistion.Transistion(currentSubPage, subPage);
			currentSubPage = subPage;
		}

		public T GetPageHandler<T>() where T : UIPageHandler
		{
			return pageHandler as T;
		}
	}
}
