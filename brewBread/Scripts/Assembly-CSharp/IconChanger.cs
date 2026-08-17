using System.Collections.Generic;
using ButtonsCodesConst;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

public class IconChanger : MonoBehaviour
{
	[Header("Info")]
	[SerializeField]
	private IconChangerInfo _info;

	[Header("Sprites")]
	[SerializeField]
	private SpriteRenderer _sprite;

	[SerializeField]
	private Image _image;

	private ControllerType _controllerType;

	private Player _player;

	private void Awake()
	{
		_player = ReInput.players.GetPlayer(_info.PlayerID);
		Controller lastActiveController = _player.controllers.GetLastActiveController();
		if (lastActiveController != null)
		{
			_controllerType = lastActiveController.type;
		}
		else
		{
			_controllerType = ControllerType.Keyboard;
		}
		if (_controllerType == ControllerType.Keyboard)
		{
			SetKeyboardIcon();
		}
		if (_controllerType == ControllerType.Joystick)
		{
			SetButtonIcon();
		}
	}

	private void SetKeyboardIcon()
	{
		ActionElementMap actionElementMap = null;
		if (_info.AxisRange != AxisRange.Full)
		{
			foreach (ActionElementMap item in _player.controllers.maps.ElementMapsWithAction(_controllerType, (int)_info.ActionID, skipDisabledMaps: false))
			{
				if (item.axisContribution.ToString() == _info.AxisRange.ToString())
				{
					actionElementMap = item;
					break;
				}
			}
		}
		else
		{
			actionElementMap = _player.controllers.maps.GetFirstElementMapWithAction(_controllerType, (int)_info.ActionID, skipDisabledMaps: false);
		}
		if (actionElementMap != null)
		{
			ChangeVisual(actionElementMap.keyboardKeyCode);
		}
	}

	private void SetButtonIcon()
	{
		ActionElementMap firstElementMapWithAction = _player.controllers.maps.GetFirstElementMapWithAction(_controllerType, (int)_info.ActionID, skipDisabledMaps: false);
		Controller currentController = GetCurrentController();
		if (currentController != null && firstElementMapWithAction != null && currentController.templateCount != 0)
		{
			IControllerTemplate controllerTemplate = currentController.Templates[0];
			List<ControllerTemplateElementTarget> list = new List<ControllerTemplateElementTarget>();
			if (controllerTemplate.GetElementTargets(firstElementMapWithAction, list) != 0)
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
			Sprite controllerSprite = StaticInstance<ConstInputVisuals>.Instance.GetControllerSprite(id);
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
		if (_sprite != null)
		{
			_sprite.sprite = newSprite;
		}
		else
		{
			_image.sprite = newSprite;
		}
	}
}
