using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zorro.ControllerSupport;
using Zorro.Core;
using Zorro.UI;

public class MainMenuCreditsPage : MainMenuPage, IHaveParentPage, INavigationPage
{
	public Button backButton;

	public Scrollbar scrollbar;

	public GameObject scrollDownThing;

	public Button scrollDownButton;

	public AnimationCurve scrollDownCurve;

	private void Awake()
	{
		backButton.onClick.AddListener(BackButtonClicked);
		scrollDownButton.onClick.AddListener(ScrollDownButtonClicked);
	}

	private void ScrollDownButtonClicked()
	{
		StartCoroutine(ScrollDownCoro());
		IEnumerator ScrollDownCoro()
		{
			float startValue = scrollbar.value;
			return scrollDownCurve.YieldForCurve(delegate(float value)
			{
				scrollbar.value = Mathf.Lerp(startValue, 0f, value);
			});
		}
	}

	private void Update()
	{
		scrollDownThing.SetActive(scrollbar.value > 0f);
	}

	private void BackButtonClicked()
	{
		pageHandler.TransistionToPage<MainMenuMainPage>();
	}

	public (UIPage, PageTransistion) GetParentPage()
	{
		return (pageHandler.GetPage<MainMenuMainPage>(), new SetActivePageTransistion());
	}

	public GameObject GetFirstSelectedGameObject()
	{
		return backButton.gameObject;
	}
}
