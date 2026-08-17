using System.Collections.Generic;
using ButtonsCodesConst;
using Rewired;
using RewiredEnums;
using UnityEngine;
using UnityEngine.UI;

public class RebindKey : MonoBehaviour
{
	[Space(5f)]
	[Header("Input options")]
	[SerializeField]
	private PenguinRewiredActions _actionID;

	[SerializeField]
	private AxisRange _axisRange;

	[SerializeField]
	private int _playerID;

	[SerializeField]
	private ControllerType _controllerType;

	[SerializeField]
	private int _categoryID;

	[Space(5f)]
	[Header("Visual Options")]
	[SerializeField]
	private SpriteRenderer _sprite;

	[SerializeField]
	private Image _image;

	private Player _player;

	public int ActionID => (int)_actionID;

	public AxisRange AxisRange => _axisRange;

	private void Start()
	{
		_player = ReInput.players.GetPlayer(_playerID);
		ActionElementMap actionElementMap = null;
		if (_controllerType == ControllerType.Keyboard && _axisRange != AxisRange.Full)
		{
			foreach (ActionElementMap item in _player.controllers.maps.ElementMapsWithAction(_controllerType, (int)_actionID, skipDisabledMaps: false))
			{
				if (item.axisContribution.ToString() == _axisRange.ToString())
				{
					actionElementMap = item;
					break;
				}
			}
		}
		else
		{
			actionElementMap = _player.controllers.maps.GetFirstElementMapWithAction(_controllerType, (int)_actionID, skipDisabledMaps: false);
		}
		ReInput.ControllerConnectedEvent += OnControllerAdded;
		if (_controllerType == ControllerType.Keyboard)
		{
			ChangeVisual(actionElementMap.keyboardKeyCode);
		}
		else if (_controllerType == ControllerType.Joystick)
		{
			SetControllerVisuals(actionElementMap);
		}
		else if (_controllerType != ControllerType.Keyboard)
		{
			Debug.LogError("No Controller Type supported: " + _controllerType);
		}
	}

	private void OnControllerAdded(ControllerStatusChangedEventArgs obj)
	{
		ActionElementMap firstElementMapWithAction = _player.controllers.maps.GetFirstElementMapWithAction(_controllerType, (int)_actionID, skipDisabledMaps: false);
		SetControllerVisuals(firstElementMapWithAction);
	}

	private void OnDestroy()
	{
		ReInput.ControllerConnectedEvent -= OnControllerAdded;
	}

	public void ChangeVisual(KeyboardKeyCode keyCode)
	{
		if (_controllerType == ControllerType.Keyboard)
		{
			Sprite keyboardSprite = StaticInstance<ConstInputVisuals>.Instance.GetKeyboardSprite(keyCode);
			if (keyboardSprite == null)
			{
				Debug.LogError("No sprite found with Keycode: " + keyCode);
			}
			else
			{
				ChangeSprite(keyboardSprite);
			}
		}
		else
		{
			Debug.LogError("ControllerType not supported");
		}
	}

	public void ChangeVisual(int id)
	{
		if (_controllerType == ControllerType.Joystick)
		{
			Sprite controllerSprite = StaticInstance<ConstInputVisuals>.Instance.GetControllerSprite(id, _axisRange);
			if (controllerSprite == null)
			{
				Debug.LogError("No sprite found with ID: " + id);
			}
			else
			{
				ChangeSprite(controllerSprite);
			}
		}
		else
		{
			Debug.LogError("ControllerType not supported");
		}
	}

	private void ChangeSprite(Sprite newSprite)
	{
		if ((bool)_sprite)
		{
			_sprite.sprite = newSprite;
		}
		else
		{
			_image.sprite = newSprite;
		}
	}

	private void SetControllerVisuals(ActionElementMap aem)
	{
		Controller currentController = GetCurrentController();
		if (currentController != null && aem != null && currentController.templateCount != 0)
		{
			IControllerTemplate controllerTemplate = currentController.Templates[0];
			List<ControllerTemplateElementTarget> list = new List<ControllerTemplateElementTarget>();
			if (controllerTemplate.GetElementTargets(aem, list) != 0)
			{
				ChangeVisual(list[0].element.id);
			}
		}
	}

	private Controller GetCurrentController()
	{
		Controller lastActiveController = _player.controllers.GetLastActiveController();
		if (lastActiveController != null && lastActiveController.type == ControllerType.Joystick)
		{
			return lastActiveController;
		}
		if (_player.controllers.joystickCount > 0)
		{
			return _player.controllers.Joysticks[0];
		}
		return null;
	}
}
