using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zorro.UI;

namespace Zorro.ControllerSupport
{
	public class NavigationHandler : MonoBehaviour
	{
		public UIPageHandler PageHandler;

		public Canvas Canvas;

		private void Start()
		{
			UIPageHandler pageHandler = PageHandler;
			pageHandler.onPageTransition = (Action<UIPage>)Delegate.Combine(pageHandler.onPageTransition, new Action<UIPage>(OnPageTransition));
		}

		private void OnPageTransition(UIPage newPage)
		{
			if (InputHandler.GetCurrentUsedInputScheme() != InputScheme.KeyboardMouse && newPage is INavigationPage navigationPage)
			{
				GameObject firstSelectedGameObject = navigationPage.GetFirstSelectedGameObject();
				if (firstSelectedGameObject != null)
				{
					EventSystem.current.SetSelectedGameObject(firstSelectedGameObject);
				}
			}
		}

		private void OnDisable()
		{
			if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.transform.IsChildOf(base.transform))
			{
				EventSystem.current.SetSelectedGameObject(null);
				Debug.Log("Deselecting because NavigationHandler was disabled.");
			}
		}

		private void Update()
		{
			if (!Canvas.enabled)
			{
				return;
			}
			bool flag = InputHandler.GetCurrentUsedInputScheme() != InputScheme.KeyboardMouse;
			bool flag2 = EventSystem.current.currentSelectedGameObject == null && !EventSystem.current.alreadySelecting;
			if (EventSystem.current.currentSelectedGameObject != null && !EventSystem.current.currentSelectedGameObject.activeInHierarchy)
			{
				flag2 = true;
			}
			if (flag2)
			{
				if (flag && PageHandler.currentPage != null && PageHandler.currentPage is INavigationPage navigationPage)
				{
					GameObject firstSelectedGameObject = navigationPage.GetFirstSelectedGameObject();
					if (firstSelectedGameObject != null)
					{
						Debug.Log("Reselecting " + firstSelectedGameObject.name);
						EventSystem.current.SetSelectedGameObject(firstSelectedGameObject);
					}
				}
			}
			else if (!flag)
			{
				GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
				if (currentSelectedGameObject != null && currentSelectedGameObject.GetComponent<TMP_InputField>() == null)
				{
					EventSystem.current.SetSelectedGameObject(null);
				}
			}
		}
	}
}
