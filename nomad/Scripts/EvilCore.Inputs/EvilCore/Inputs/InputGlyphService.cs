using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

namespace EvilCore.Inputs
{
	public class InputGlyphService : MonoBehaviour, IInputGlyphService
	{
		[Header("Device Sprite Maps")]
		[SerializeField]
		private InputSpriteMap keyboardMouseSpriteMap;

		[SerializeField]
		private InputSpriteMap playstationSpriteMap;

		[SerializeField]
		private InputSpriteMap xboxSpriteMap;

		[SerializeField]
		private InputSpriteMap switchSpriteMap;

		[Header("Error")]
		[Tooltip("Shown when action name is empty, invalid, or sprite not found")]
		[SerializeField]
		private Sprite errorSprite;

		private InputDeviceFamily _activeDeviceFamily;

		private InputSpriteMap _activeSpriteMap;

		private ControllerType _lastControllerType;

		private int _lastControllerId = -1;

		private readonly Dictionary<int, Sprite> _cache = new Dictionary<int, Sprite>();

		private readonly List<ActionElementMap> _tempAems = new List<ActionElementMap>();

		public InputDeviceFamily ActiveDeviceFamily => _activeDeviceFamily;

		public event Action OnGlyphsChanged;

		private void Awake()
		{
			_activeSpriteMap = keyboardMouseSpriteMap;
		}

		private void Update()
		{
			if (ReInput.isReady)
			{
				DetectControllerChange();
			}
		}

		private void DetectControllerChange()
		{
			Player player = ReInput.players.GetPlayer(0);
			if (player == null)
			{
				return;
			}
			Controller lastActiveController = player.controllers.GetLastActiveController();
			if (lastActiveController != null && (lastActiveController.type != _lastControllerType || lastActiveController.id != _lastControllerId))
			{
				_lastControllerType = lastActiveController.type;
				_lastControllerId = lastActiveController.id;
				InputDeviceFamily inputDeviceFamily = ResolveDeviceFamily(lastActiveController);
				if (inputDeviceFamily != _activeDeviceFamily)
				{
					_activeDeviceFamily = inputDeviceFamily;
					_activeSpriteMap = GetSpriteMapForFamily(inputDeviceFamily);
					InvalidateCache();
				}
			}
		}

		private InputDeviceFamily ResolveDeviceFamily(Controller controller)
		{
			if (controller.type == ControllerType.Keyboard || controller.type == ControllerType.Mouse)
			{
				return InputDeviceFamily.KeyboardMouse;
			}
			if (controller.type != ControllerType.Joystick)
			{
				return InputDeviceFamily.KeyboardMouse;
			}
			string s = controller.hardwareName ?? "";
			string s2 = controller.name ?? "";
			if (ContainsAny(s, s2, "DualSense", "DualShock", "PS4", "PS5", "Sony"))
			{
				return InputDeviceFamily.PlayStation;
			}
			if (ContainsAny(s, s2, "Xbox", "XInput"))
			{
				return InputDeviceFamily.Xbox;
			}
			if (ContainsAny(s, s2, "Switch", "Nintendo", "Pro Controller", "Joy-Con"))
			{
				return InputDeviceFamily.NintendoSwitch;
			}
			return InputDeviceFamily.Xbox;
		}

		private static bool ContainsAny(string s1, string s2, params string[] keywords)
		{
			foreach (string value in keywords)
			{
				if (s1.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0 || s2.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		private InputSpriteMap GetSpriteMapForFamily(InputDeviceFamily family)
		{
			return family switch
			{
				InputDeviceFamily.KeyboardMouse => keyboardMouseSpriteMap, 
				InputDeviceFamily.PlayStation => playstationSpriteMap, 
				InputDeviceFamily.Xbox => xboxSpriteMap, 
				InputDeviceFamily.NintendoSwitch => switchSpriteMap, 
				_ => keyboardMouseSpriteMap, 
			};
		}

		public void InvalidateCache()
		{
			_cache.Clear();
			this.OnGlyphsChanged?.Invoke();
		}

		public Sprite GetSpriteForElement(string elementIdentifierName)
		{
			if (string.IsNullOrEmpty(elementIdentifierName))
			{
				return null;
			}
			if (!(_activeSpriteMap != null))
			{
				return null;
			}
			return _activeSpriteMap.GetSprite(elementIdentifierName);
		}

		public Sprite GetSpriteForAction(string rewiredActionName)
		{
			if (!ReInput.isReady || string.IsNullOrEmpty(rewiredActionName))
			{
				return errorSprite;
			}
			InputAction action = ReInput.mapping.GetAction(rewiredActionName);
			if (action == null)
			{
				return errorSprite;
			}
			if (_cache.TryGetValue(action.id, out var value))
			{
				return value;
			}
			Sprite sprite = ResolveSprite(action.id);
			_cache[action.id] = sprite;
			return sprite;
		}

		private Sprite ResolveSprite(int actionId)
		{
			ActionElementMap firstBindingForAction = GetFirstBindingForAction(actionId);
			if (firstBindingForAction == null)
			{
				return errorSprite;
			}
			Sprite sprite = _activeSpriteMap?.GetSprite(firstBindingForAction.elementIdentifierName);
			if (sprite == null)
			{
				return errorSprite;
			}
			return sprite;
		}

		private ActionElementMap GetFirstBindingForAction(int actionId)
		{
			Player player = ReInput.players.GetPlayer(0);
			if (player == null)
			{
				return null;
			}
			Controller lastActiveController = player.controllers.GetLastActiveController();
			if (lastActiveController != null)
			{
				if (lastActiveController.type == ControllerType.Keyboard || lastActiveController.type == ControllerType.Mouse)
				{
					ActionElementMap actionElementMap = TryGetFromController(player, ControllerType.Keyboard, actionId);
					if (actionElementMap != null)
					{
						return actionElementMap;
					}
					actionElementMap = TryGetFromController(player, ControllerType.Mouse, actionId);
					if (actionElementMap != null)
					{
						return actionElementMap;
					}
				}
				else
				{
					ActionElementMap actionElementMap2 = TryGetFromController(player, lastActiveController.type, lastActiveController.id, actionId);
					if (actionElementMap2 != null)
					{
						return actionElementMap2;
					}
					actionElementMap2 = TryGetFromController(player, lastActiveController.type, actionId);
					if (actionElementMap2 != null)
					{
						return actionElementMap2;
					}
				}
			}
			_tempAems.Clear();
			player.controllers.maps.GetElementMapsWithAction(actionId, skipDisabledMaps: true, _tempAems);
			return FindFirstPositiveBinding(_tempAems);
		}

		private ActionElementMap TryGetFromController(Player player, ControllerType type, int actionId)
		{
			_tempAems.Clear();
			player.controllers.maps.GetElementMapsWithAction(type, actionId, skipDisabledMaps: true, _tempAems);
			return FindFirstPositiveBinding(_tempAems);
		}

		private ActionElementMap TryGetFromController(Player player, ControllerType type, int controllerId, int actionId)
		{
			_tempAems.Clear();
			player.controllers.maps.GetElementMapsWithAction(type, controllerId, actionId, skipDisabledMaps: true, _tempAems);
			return FindFirstPositiveBinding(_tempAems);
		}

		private static ActionElementMap FindFirstPositiveBinding(List<ActionElementMap> aems)
		{
			foreach (ActionElementMap aem in aems)
			{
				if (aem.axisRange == AxisRange.Full)
				{
					return aem;
				}
			}
			foreach (ActionElementMap aem2 in aems)
			{
				if (aem2.axisContribution == Pole.Positive || aem2.axisType == AxisType.None)
				{
					return aem2;
				}
			}
			if (aems.Count <= 0)
			{
				return null;
			}
			return aems[0];
		}
	}
}
