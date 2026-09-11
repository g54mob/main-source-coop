using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zorro.UI
{
	public class UIPageHandler : MonoBehaviour
	{
		public UIPage currentPage;

		private Dictionary<Type, UIPage> _pages = new Dictionary<Type, UIPage>();

		public Action<UIPage> onPageTransition;

		protected virtual void Start()
		{
			UIPage[] componentsInChildren = GetComponentsInChildren<UIPage>(includeInactive: true);
			foreach (UIPage uIPage in componentsInChildren)
			{
				uIPage.gameObject.SetActive(value: false);
				_pages.Add(uIPage.GetType(), uIPage);
			}
			UIPageHandlerStartPageSelector component = GetComponent<UIPageHandlerStartPageSelector>();
			if (component != null)
			{
				currentPage = component.GetStartPage();
			}
			currentPage.gameObject.SetActive(value: true);
			currentPage.OnPageEnter();
			currentPage.OnPageEntered();
		}

		public T TransistionToPage<T>() where T : UIPage
		{
			if (_pages.TryGetValue(typeof(T), out var value))
			{
				return TransistionToPage(value, new SetActivePageTransistion()) as T;
			}
			return null;
		}

		public T TransistionToPage<T>(PageTransistion pageTransistion) where T : UIPage
		{
			if (_pages.TryGetValue(typeof(T), out var value))
			{
				return TransistionToPage(value, pageTransistion) as T;
			}
			return null;
		}

		public UIPage TransistionToPage(UIPage page, PageTransistion pageTransistion)
		{
			if (currentPage == page)
			{
				Debug.LogWarning("Trying to transition to current page: " + page.name);
				return currentPage;
			}
			Debug.Log("Transitioning to " + page.name);
			pageTransistion.Transistion(currentPage, page);
			currentPage = page;
			OnTransistionedToPage(page);
			return page;
		}

		protected virtual void OnTransistionedToPage(UIPage newPage)
		{
			onPageTransition?.Invoke(newPage);
		}

		public UIPage GetPage<T>() where T : UIPage
		{
			return _pages.GetValueOrDefault(typeof(T));
		}
	}
}
