using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Data;
using UIControllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
	[Serializable]
	public enum UIStates
	{
		None = 0,
		MainMenu = 1,
		Exit = 2,
		PlayMenu = 3,
		OptionsMenu = 4,
		CharacterSelect = 5,
		Game = 6,
		Pause = 7,
		AssistOptions = 8,
		Controls = 9
	}

	[SerializeField]
	private PlayMakerFSM _stateMachine;

	[SerializeField]
	private UserDataStore_PlayerPrefs _userPrefs;

	private UIStates _currentState;

	private UIStates _prevState;

	private Dictionary<UIStates, UIController> _uiControllers;

	private Player _uiPlayer;

	private bool _inputEnabled = true;

	public UnityEvent<Controller> UIConfirm;

	public UnityEvent<Controller> UICancel;

	public bool InputEnabled
	{
		get
		{
			return _inputEnabled;
		}
		set
		{
			_inputEnabled = value;
		}
	}

	public UIStates CurrentState => _currentState;

	public UIStates PreviousState => _prevState;

	protected override void Awake()
	{
		base.Awake();
		_stateMachine = GetComponent<PlayMakerFSM>();
		_uiControllers = new Dictionary<UIStates, UIController>();
		_currentState = UIStates.MainMenu;
		_uiPlayer = ReInput.players.GetPlayer(3);
		foreach (Controller controller in ReInput.controllers.Controllers)
		{
			if (!_uiPlayer.controllers.ContainsController(controller))
			{
				_uiPlayer.controllers.AddController(controller, removeFromOtherPlayers: false);
			}
		}
		ReInput.ControllerConnectedEvent += ControllerConected;
		ReInput.ControllerDisconnectedEvent += ControllerDisconnected;
		UIConfirm = new UnityEvent<Controller>();
		UICancel = new UnityEvent<Controller>();
		_userPrefs = UnityEngine.Object.FindObjectOfType<UserDataStore_PlayerPrefs>();
		LoadInputData();
		ReInput.ControllerConnectedEvent += LoadControllerPrefs;
	}

	private void LoadControllerPrefs(ControllerStatusChangedEventArgs obj)
	{
		AssignControllers();
		_userPrefs.Load();
		CleanControllers();
	}

	private void LoadInputData()
	{
		AssignControllers();
		_userPrefs.Load();
		AssignUIControllers();
		CleanControllers();
	}

	private void AssignUIControllers()
	{
		Player player = ReInput.players.GetPlayer(3);
		foreach (Controller controller in ReInput.controllers.Controllers)
		{
			player.controllers.AddController(controller, removeFromOtherPlayers: false);
		}
	}

	private void AssignControllers()
	{
		foreach (Player player in ReInput.players.Players)
		{
			foreach (Controller controller in ReInput.controllers.Controllers)
			{
				player.controllers.AddController(controller, removeFromOtherPlayers: false);
			}
		}
	}

	private void CleanControllers()
	{
		foreach (Player player in ReInput.players.Players)
		{
			if (player.id == 3)
			{
				continue;
			}
			foreach (Controller controller in ReInput.controllers.Controllers)
			{
				if (controller.type != ControllerType.Keyboard)
				{
					player.controllers.RemoveController(controller);
				}
			}
		}
	}

	private void ControllerConected(ControllerStatusChangedEventArgs obj)
	{
		if (!_uiPlayer.controllers.ContainsController(obj.controller))
		{
			_uiPlayer.controllers.AddController(obj.controller, removeFromOtherPlayers: false);
		}
	}

	private void ControllerDisconnected(ControllerStatusChangedEventArgs obj)
	{
		_uiPlayer.controllers.RemoveController(obj.controller);
	}

	private void OnDestroy()
	{
	}

	private void Update()
	{
		if (_inputEnabled)
		{
			ManageInput();
		}
	}

	private void ManageInput()
	{
		if (_uiPlayer.GetButtonDown(18))
		{
			UIConfirm?.Invoke(_uiPlayer.controllers.GetLastActiveController());
		}
		if (_uiPlayer.GetButtonDown(19))
		{
			UICancel?.Invoke(_uiPlayer.controllers.GetLastActiveController());
			SendCancelEvent();
		}
	}

	public void SetInitialScene()
	{
		string activeScene = StaticInstance<TransitionSystem>.Instance.GetActiveScene();
		if (activeScene == Scenes.VictorGame.ToString())
		{
			_stateMachine.SetState("Game");
		}
		else if (activeScene == Scenes.SinglePlayer.ToString())
		{
			StaticInstance<DataBetweenScenes>.Instance.SinglePlayer = true;
			_stateMachine.SetState("Game");
		}
		_stateMachine.SetState(activeScene);
	}

	private void SendCancelEvent()
	{
		SendEvent(UIStateMachineEvents.Cancel);
	}

	public void AddToDictionary(UIStates state, UIController controller)
	{
		_uiControllers.Add(state, controller);
	}

	public void RemoveFromDictionray(UIStates state)
	{
		_uiControllers.Remove(state);
	}

	public void SendEvent(UIStateMachineEvents newEvent)
	{
		_stateMachine.SendEvent(newEvent.ToString());
	}

	public void SaveUserPrefs()
	{
		_userPrefs.Save();
	}

	public void ChangeState(UIStates state)
	{
		if (state == _currentState)
		{
			return;
		}
		_uiControllers.TryGetValue(_currentState, out var value);
		foreach (KeyValuePair<UIStates, UIController> uiController in _uiControllers)
		{
			if ((bool)uiController.Value.EventSystem)
			{
				uiController.Value.EventSystem.enabled = false;
			}
		}
		_prevState = _currentState;
		_currentState = state;
		if ((bool)value)
		{
			value.Exit();
		}
	}

	public void LoadScene(Scenes name, LoadSceneMode mode, TransitionType transition, bool loadingScreen = false)
	{
		if (StaticInstance<TransitionSystem>.Instance.IsSceneLoaded(name))
		{
			EnableCurrentEventManager();
		}
		else
		{
			StaticInstance<TransitionSystem>.Instance.LoadSceneAsync(name, mode, transition, loadingScreen).completed += EnableCurrentEventManager;
		}
	}

	private void EnableCurrentEventManager(AsyncOperation obj = null)
	{
		foreach (KeyValuePair<UIStates, UIController> uiController in _uiControllers)
		{
			if (uiController.Key == _currentState)
			{
				if ((bool)uiController.Value.EventSystem)
				{
					uiController.Value.EventSystem.enabled = true;
				}
				break;
			}
		}
	}

	public UIStateMachineEvents GetReturnOption(UIStates currentState)
	{
		_uiControllers.TryGetValue(currentState, out var value);
		if (value == null)
		{
			Debug.LogError("Current state not loaded, something really bad and unexpected happened");
		}
		switch (value.PrevState)
		{
		case UIStates.MainMenu:
			return UIStateMachineEvents.MainMenu;
		case UIStates.PlayMenu:
			return UIStateMachineEvents.PlayMenu;
		case UIStates.Pause:
			return UIStateMachineEvents.Pause;
		case UIStates.OptionsMenu:
			return UIStateMachineEvents.Options;
		default:
			Debug.LogError("The current scene doesn't need a return Option");
			return UIStateMachineEvents.None;
		}
	}
}
