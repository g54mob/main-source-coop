using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using UnityEngine;

namespace UIControllers
{
	public class RemaperController : UIController
	{
		[SerializeField]
		private GameObject _listening;

		[SerializeField]
		private GameObject _controllerNotConnected;

		[SerializeField]
		private AudioClip _audioClip;

		private AudioSource _audioSource;

		private InputMapper _inputMapper = new InputMapper();

		private ControllerType _controllerType;

		private Player _player;

		private int _category;

		private ControllerMap _controllerMap;

		private RebindKey _currentBindKey;

		private bool _canBind = true;

		private float _lastBindTime = 0.1f;

		private List<Controller> _player1Controllers;

		private List<Controller> _player2Controllers;

		private List<Controller> _player3Controllers;

		private void Awake()
		{
			_inputMapper.options.timeout = 5f;
			_inputMapper.options.ignoreMouseXAxis = true;
			_inputMapper.options.ignoreMouseYAxis = true;
			_inputMapper.options.checkForConflicts = false;
			_inputMapper.InputMappedEvent += OnInputMapped;
			_inputMapper.StoppedEvent += OnStopped;
			AssignControllers();
			ReInput.ControllerConnectedEvent += OnControllerConnected;
			ChangePlayer(0);
			_audioSource = GetComponent<AudioSource>();
		}

		private void OnControllerConnected(ControllerStatusChangedEventArgs obj)
		{
			_controllerNotConnected.SetActive(value: false);
			foreach (Player player in ReInput.players.Players)
			{
				player.controllers.AddController(obj.controller, removeFromOtherPlayers: false);
			}
		}

		public override void Exit()
		{
			base.Exit();
			StaticInstance<UIManager>.Instance.SaveUserPrefs();
			CleanControllers();
			StaticInstance<TransitionSystem>.Instance.UnloadScene(Scenes.Controls);
			ReInput.ControllerConnectedEvent -= OnControllerConnected;
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
		}

		private void AssignControllers()
		{
			_player1Controllers = ReInput.players.GetPlayer(0).controllers.Controllers.ToList();
			_player2Controllers = ReInput.players.GetPlayer(1).controllers.Controllers.ToList();
			_player3Controllers = ReInput.players.GetPlayer(2).controllers.Controllers.ToList();
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
					player.controllers.RemoveController(controller);
				}
			}
			foreach (Controller player1Controller in _player1Controllers)
			{
				ReInput.players.GetPlayer(0).controllers.AddController(player1Controller, removeFromOtherPlayers: false);
			}
			foreach (Controller player2Controller in _player2Controllers)
			{
				ReInput.players.GetPlayer(1).controllers.AddController(player2Controller, removeFromOtherPlayers: false);
			}
			foreach (Controller player3Controller in _player3Controllers)
			{
				ReInput.players.GetPlayer(2).controllers.AddController(player3Controller, removeFromOtherPlayers: false);
			}
		}

		public void ChangePlayer(int newPlayer)
		{
			_player = ReInput.players.GetPlayer(newPlayer);
			switch (newPlayer)
			{
			case 0:
				_category = 1;
				break;
			case 1:
				_category = 2;
				break;
			case 2:
				_category = 3;
				break;
			case 3:
				_category = 4;
				break;
			}
			SetControllMap();
		}

		public void ChangeController(ControllerTypeInspector type)
		{
			if (type.controllerType == ControllerType.Joystick)
			{
				if (ReInput.controllers.joystickCount <= 0)
				{
					_controllerNotConnected.SetActive(value: true);
				}
				else
				{
					_controllerNotConnected.SetActive(value: false);
				}
			}
			else
			{
				_controllerNotConnected.SetActive(value: false);
			}
			_controllerType = type.controllerType;
			SetControllMap();
		}

		private void SetControllMap()
		{
			_controllerMap = _player.controllers.maps.GetMap(_controllerType, 0, _category, 0);
		}

		public void StartListening(RebindKey rebindKey)
		{
			if (_canBind)
			{
				_canBind = false;
				_currentBindKey = rebindKey;
				StaticInstance<UIManager>.Instance.InputEnabled = false;
				_listening.SetActive(value: true);
				StartCoroutine(StartListeningDelayed(rebindKey));
			}
		}

		private void OnStopped(InputMapper.StoppedEventData obj)
		{
			StartCoroutine(EnableUIInputDelayed());
		}

		private IEnumerator EnableUIInputDelayed()
		{
			yield return new WaitForSecondsRealtime(0.1f);
			_lastBindTime = 0f;
			_listening.SetActive(value: false);
			StaticInstance<UIManager>.Instance.InputEnabled = true;
			StartCoroutine(LastBindTimeTimer());
		}

		private IEnumerator LastBindTimeTimer()
		{
			yield return new WaitForSecondsRealtime(_lastBindTime);
			_canBind = true;
		}

		private IEnumerator StartListeningDelayed(RebindKey rebindKey)
		{
			int actionElementMapId = -1;
			foreach (ActionElementMap item in _controllerMap.ElementMapsWithAction(rebindKey.ActionID))
			{
				if (item.ShowInField(rebindKey.AxisRange))
				{
					base.name = item.elementIdentifierName;
					actionElementMapId = item.id;
					break;
				}
			}
			yield return new WaitForSecondsRealtime(0.1f);
			_inputMapper.Start(new InputMapper.Context
			{
				actionId = rebindKey.ActionID,
				controllerMap = _controllerMap,
				actionRange = rebindKey.AxisRange,
				actionElementMapToReplace = _controllerMap.GetElementMap(actionElementMapId)
			});
		}

		private void OnInputMapped(InputMapper.InputMappedEventData obj)
		{
			if ((bool)_currentBindKey)
			{
				if (obj.actionElementMap.controllerMap.controllerType == ControllerType.Keyboard)
				{
					_currentBindKey.ChangeVisual(obj.actionElementMap.keyboardKeyCode);
				}
				else if (obj.actionElementMap.controllerMap.controllerType == ControllerType.Joystick)
				{
					SetControllerVisuals(obj);
				}
			}
			if (_audioSource != null)
			{
				_audioSource.PlayOneShot(_audioClip);
			}
		}

		private void SetControllerVisuals(InputMapper.InputMappedEventData obj)
		{
			Controller currentController = GetCurrentController();
			if (currentController == null)
			{
				return;
			}
			ActionElementMap firstElementMapWithAction = _player.controllers.maps.GetFirstElementMapWithAction(currentController, obj.actionElementMap.actionId, skipDisabledMaps: true);
			if (firstElementMapWithAction != null && currentController.templateCount != 0)
			{
				IControllerTemplate controllerTemplate = currentController.Templates[0];
				List<ControllerTemplateElementTarget> list = new List<ControllerTemplateElementTarget>();
				if (controllerTemplate.GetElementTargets(firstElementMapWithAction, list) != 0)
				{
					ControllerTemplateElementTarget controllerTemplateElementTarget = list[0];
					_currentBindKey.ChangeVisual(controllerTemplateElementTarget.element.id);
				}
			}
		}

		private Controller GetCurrentController()
		{
			Controller lastActiveController = _player.controllers.GetLastActiveController();
			if (lastActiveController != null)
			{
				return lastActiveController;
			}
			if (_player.controllers.joystickCount > 0)
			{
				return _player.controllers.Joysticks[0];
			}
			return null;
		}

		private void CheckForConflicts()
		{
		}
	}
}
