using UnityEngine;
using UnityEngine.UI;

namespace UIControllers
{
	public class MainMenuController : UIController
	{
		[Space(10f)]
		[SerializeField]
		private GameObject _continueButton;

		[SerializeField]
		private GameObject _newGameButton;

		[SerializeField]
		private GameObject _exitButton;

		private void Awake()
		{
		}

		public override void Start()
		{
			base.Start();
			if (!SaveSystem.FileExist())
			{
				HideContinueButton();
			}
			else
			{
				ShowContinueButton();
			}
		}

		private void ShowContinueButton()
		{
			_continueButton.SetActive(value: true);
			_eventSystem.firstSelectedGameObject = _continueButton;
		}

		private void HideContinueButton()
		{
			_continueButton.SetActive(value: false);
			_eventSystem.firstSelectedGameObject = _newGameButton;
			Navigation navigation = _newGameButton.GetComponent<Button>().navigation;
			navigation.selectOnUp = _exitButton.GetComponent<Button>();
			_newGameButton.GetComponent<Button>().navigation = navigation;
			Navigation navigation2 = _exitButton.GetComponent<Button>().navigation;
			navigation2.selectOnDown = _newGameButton.GetComponent<Button>();
			_exitButton.GetComponent<Button>().navigation = navigation2;
		}

		public void SetLoadGame(bool value)
		{
			StaticInstance<DataBetweenScenes>.Instance.loadGame = value;
		}
	}
}
