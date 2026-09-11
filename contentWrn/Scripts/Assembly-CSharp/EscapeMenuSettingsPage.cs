using UnityEngine;
using UnityEngine.UI;
using Zorro.ControllerSupport;
using Zorro.UI;

public class EscapeMenuSettingsPage : EscapeMenuPage, IHaveParentPage, INavigationPage
{
	public Button backButton;

	private void Awake()
	{
		backButton.onClick.AddListener(OnBackButtonClicked);
	}

	private void OnBackButtonClicked()
	{
		SaveSettings();
		pageHandler.TransistionToPage<EscapeMenuMainPage>();
	}

	public (UIPage, PageTransistion) GetParentPage()
	{
		SaveSettings();
		return (pageHandler.GetPage<EscapeMenuMainPage>(), new SetActivePageTransistion());
	}

	private void SaveSettings()
	{
	}

	public GameObject GetFirstSelectedGameObject()
	{
		return backButton.gameObject;
	}
}
