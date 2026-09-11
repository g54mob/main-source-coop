using UnityEngine;
using UnityEngine.UI;
using Zorro.ControllerSupport;
using Zorro.UI;

public class MainMenuHostPage : MainMenuPage, INavigationPage, IHaveParentPage
{
	public Button backButton;

	public SaveUICellTABS saveUICellTABS;

	public Button hostButton;

	private void Awake()
	{
		backButton.onClick.AddListener(OnBackButtonClicked);
		hostButton.onClick.AddListener(OnHostButtonClicked);
	}

	private void OnHostButtonClicked()
	{
		MainMenuHandler.Instance.Host(saveUICellTABS.selectedButton.SaveIndex);
	}

	private void OnBackButtonClicked()
	{
		pageHandler.TransistionToPage<MainMenuMainPage>();
	}

	public GameObject GetFirstSelectedGameObject()
	{
		return saveUICellTABS.GetFirstSelectedGameObject();
	}

	public (UIPage, PageTransistion) GetParentPage()
	{
		return (pageHandler.GetPage<MainMenuMainPage>(), new SetActivePageTransistion());
	}
}
