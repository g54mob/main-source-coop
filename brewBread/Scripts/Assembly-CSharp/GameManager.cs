using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	[SerializeField]
	private AudioSource _music;

	private GameStates _currentState;

	public GameStates CurrentState => _currentState;

	private void Start()
	{
		Application.targetFrameRate = 120;
		if (SceneManager.GetActiveScene().name == "Game" || SceneManager.GetActiveScene().name == "GameGuillem" || SceneManager.GetActiveScene().name == "VictorGame" || SceneManager.GetActiveScene().name == "SinglePlayer")
		{
			ChangeGameState(GameStates.STARTGAME);
		}
		StaticInstance<TransitionSystem>.Instance.OnSceneLoaded += SceneChanged;
	}

	private void OnDestroy()
	{
		if (StaticInstance<TransitionSystem>.Instance != null)
		{
			StaticInstance<TransitionSystem>.Instance.OnSceneLoaded -= SceneChanged;
		}
	}

	private void SceneChanged(string sceneName)
	{
		if (sceneName == Scenes.VictorGame.ToString() || sceneName == Scenes.SinglePlayer.ToString())
		{
			ChangeGameState(GameStates.STARTGAME);
		}
		else if (sceneName == Scenes.MainMenu.ToString())
		{
			ChangeGameState(GameStates.MAINMENU);
		}
	}

	public void ChangeGameState(GameStates newState)
	{
		if (_currentState != newState)
		{
			_currentState = newState;
			switch (newState)
			{
			case GameStates.MAINMENU:
				HandleMainMenu();
				break;
			case GameStates.CHARACTERSELECION:
				HandleCharacterSelection();
				break;
			case GameStates.OPTIONSMENU:
				HandleOptionsMenu();
				break;
			case GameStates.CONTROLSMENU:
				HandleControlsMenu();
				break;
			case GameStates.STARTGAME:
				HandleStartGame();
				break;
			case GameStates.INGAME:
			case GameStates.WIN:
				break;
			}
		}
	}

	private void HandleStartGame()
	{
		_music.gameObject.SetActive(value: false);
		if (StaticInstance<DataBetweenScenes>.Instance.loadGame)
		{
			SaveSystem.LoadData(StaticInstance<TransitionSystem>.Instance.GetActiveScene() == Scenes.SinglePlayer.ToString());
		}
		UnityEngine.Object.FindObjectOfType<CameraFollow>().Initialize();
		SetRope();
		ChangeGameState(GameStates.INGAME);
	}

	private void SetRope()
	{
		Rope rope = null;
		Rope[] array = UnityEngine.Object.FindObjectsOfType<Rope>();
		foreach (Rope rope2 in array)
		{
			if (rope2.gameObject.tag == "Player")
			{
				rope = rope2;
				break;
			}
		}
		if (!(rope == null))
		{
			rope.StartPoint = StaticInstance<Bread>.Instance.RopePoint;
			rope.EndPoint = StaticInstance<Fred>.Instance.RopePoint;
			rope.Initialize();
		}
	}

	private void HandleControlsMenu()
	{
		throw new NotImplementedException();
	}

	private void HandleOptionsMenu()
	{
		throw new NotImplementedException();
	}

	private void HandleCharacterSelection()
	{
		throw new NotImplementedException();
	}

	private void HandleMainMenu()
	{
		StaticInstance<DataBetweenScenes>.Instance.loadGame = false;
		_music.gameObject.SetActive(value: true);
		_music.Play();
	}

	internal void SetBlur(bool value)
	{
	}
}
