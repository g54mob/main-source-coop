using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GamepadIconsChanger : MonoBehaviour
{
	[Serializable]
	public struct GamepadIcons
	{
		public Sprite buttonSouth;

		public Sprite buttonNorth;

		public Sprite buttonEast;

		public Sprite buttonWest;

		public Sprite startButton;

		public Sprite selectButton;

		public Sprite leftTrigger;

		public Sprite rightTrigger;

		public Sprite leftShoulder;

		public Sprite rightShoulder;

		public Sprite dpad;

		public Sprite dpadUp;

		public Sprite dpadDown;

		public Sprite dpadLeft;

		public Sprite dpadRight;

		public Sprite leftStick;

		public Sprite leftStickleft;

		public Sprite leftStickup;

		public Sprite leftStickright;

		public Sprite leftStickdown;

		public Sprite rightStick;

		public Sprite rightStickleft;

		public Sprite rightStickup;

		public Sprite rightStickright;

		public Sprite rightStickdown;

		public Sprite leftStickPress;

		public Sprite rightStickPress;

		public Sprite GetSprite(string controlPath)
		{
			return controlPath switch
			{
				"buttonSouth" => buttonSouth, 
				"buttonNorth" => buttonNorth, 
				"buttonEast" => buttonEast, 
				"buttonWest" => buttonWest, 
				"start" => startButton, 
				"select" => selectButton, 
				"leftTrigger" => leftTrigger, 
				"rightTrigger" => rightTrigger, 
				"leftShoulder" => leftShoulder, 
				"rightShoulder" => rightShoulder, 
				"dpad" => dpad, 
				"dpad/up" => dpadUp, 
				"dpad/down" => dpadDown, 
				"dpad/left" => dpadLeft, 
				"dpad/right" => dpadRight, 
				"leftStick" => leftStick, 
				"leftStick/left" => leftStickleft, 
				"leftStick/up" => leftStickup, 
				"leftStick/right" => leftStickright, 
				"leftStick/down" => leftStickdown, 
				"rightStick" => rightStick, 
				"rightStick/left" => rightStickleft, 
				"rightStick/up" => rightStickup, 
				"rightStick/right" => rightStickright, 
				"rightStick/down" => rightStickdown, 
				"leftStickPress" => leftStickPress, 
				"rightStickPress" => rightStickPress, 
				_ => null, 
			};
		}
	}

	[Serializable]
	public struct Keyboard
	{
		public Sprite esc;

		public Sprite f1;

		public Sprite f2;

		public Sprite f3;

		public Sprite f4;

		public Sprite f5;

		public Sprite f6;

		public Sprite f7;

		public Sprite f8;

		public Sprite f9;

		public Sprite f10;

		public Sprite f11;

		public Sprite f12;

		public Sprite printScreen;

		public Sprite scrollLock;

		public Sprite pause;

		public Sprite backquote;

		public Sprite num1;

		public Sprite num2;

		public Sprite num3;

		public Sprite num4;

		public Sprite num5;

		public Sprite num6;

		public Sprite num7;

		public Sprite num8;

		public Sprite num9;

		public Sprite num0;

		public Sprite minus;

		public Sprite equals;

		public Sprite backspace;

		public Sprite insert;

		public Sprite home;

		public Sprite pageUp;

		public Sprite numLock;

		public Sprite numpadDivide;

		public Sprite numpadMultiply;

		public Sprite numpadMinus;

		public Sprite tab;

		public Sprite q;

		public Sprite w;

		public Sprite e;

		public Sprite r;

		public Sprite t;

		public Sprite y;

		public Sprite u;

		public Sprite i;

		public Sprite o;

		public Sprite p;

		public Sprite leftBracket;

		public Sprite rightBracket;

		public Sprite enter;

		public Sprite delete;

		public Sprite end;

		public Sprite pageDown;

		public Sprite numpad7;

		public Sprite numpad8;

		public Sprite numpad9;

		public Sprite numpadPlus;

		public Sprite capsLock;

		public Sprite a;

		public Sprite s;

		public Sprite d;

		public Sprite f;

		public Sprite g;

		public Sprite h;

		public Sprite j;

		public Sprite k;

		public Sprite l;

		public Sprite semicolon;

		public Sprite quote;

		public Sprite backslash;

		public Sprite numpad4;

		public Sprite numpad5;

		public Sprite numpad6;

		public Sprite leftShift;

		public Sprite OEM1;

		public Sprite z;

		public Sprite x;

		public Sprite c;

		public Sprite v;

		public Sprite b;

		public Sprite n;

		public Sprite m;

		public Sprite comma;

		public Sprite period;

		public Sprite slash;

		public Sprite rightShift;

		public Sprite upArrow;

		public Sprite numpad1;

		public Sprite numpad2;

		public Sprite numpad3;

		public Sprite numpadEnter;

		public Sprite leftCtrl;

		public Sprite leftMeta;

		public Sprite leftAlt;

		public Sprite space;

		public Sprite rightAlt;

		public Sprite rightCtrl;

		public Sprite leftArrow;

		public Sprite downArrow;

		public Sprite rightArrow;

		public Sprite numpad0;

		public Sprite numpadPeriod;

		public Sprite GetSprite(string controlPath)
		{
			return controlPath switch
			{
				"esc" => esc, 
				"f1" => f1, 
				"f2" => f2, 
				"f3" => f3, 
				"f4" => f4, 
				"f5" => f5, 
				"f6" => f6, 
				"f7" => f7, 
				"f8" => f8, 
				"f9" => f9, 
				"f10" => f10, 
				"f11" => f11, 
				"f12" => f12, 
				"printScreen" => printScreen, 
				"scrollLock" => scrollLock, 
				"pause" => pause, 
				"backquote" => backquote, 
				"1" => num1, 
				"2" => num2, 
				"3" => num3, 
				"4" => num4, 
				"5" => num5, 
				"6" => num6, 
				"7" => num7, 
				"8" => num8, 
				"9" => num9, 
				"0" => num0, 
				"minus" => minus, 
				"equals" => equals, 
				"backspace" => backspace, 
				"insert" => insert, 
				"home" => home, 
				"pageUp" => pageUp, 
				"numLock" => numLock, 
				"numpadDivide" => numpadDivide, 
				"numpadMultiply" => numpadMultiply, 
				"numpadMinus" => numpadMinus, 
				"tab" => tab, 
				"q" => q, 
				"w" => w, 
				"e" => e, 
				"r" => r, 
				"t" => t, 
				"y" => y, 
				"u" => u, 
				"i" => i, 
				"o" => o, 
				"p" => p, 
				"leftBracket" => leftBracket, 
				"rightBracket" => rightBracket, 
				"enter" => enter, 
				"delete" => delete, 
				"end" => end, 
				"pageDown" => pageDown, 
				"numpad7" => numpad7, 
				"numpad8" => numpad8, 
				"numpad9" => numpad9, 
				"numpadPlus" => numpadPlus, 
				"capsLock" => capsLock, 
				"a" => a, 
				"s" => s, 
				"d" => d, 
				"f" => f, 
				"g" => g, 
				"h" => h, 
				"j" => j, 
				"k" => k, 
				"l" => l, 
				"semicolon" => semicolon, 
				"quote" => quote, 
				"backslash" => backslash, 
				"leftShift" => leftShift, 
				"OEM1" => OEM1, 
				"z" => z, 
				"x" => x, 
				"c" => c, 
				"v" => v, 
				"b" => b, 
				"n" => n, 
				"m" => m, 
				"comma" => comma, 
				"period" => period, 
				"slash" => slash, 
				"rightShift" => rightShift, 
				"upArrow" => upArrow, 
				"numpad1" => numpad1, 
				"numpad2" => numpad2, 
				"numpad3" => numpad3, 
				"numpad4" => numpad4, 
				"numpad5" => numpad5, 
				"numpad6" => numpad6, 
				"numpadEnter" => numpadEnter, 
				"leftCtrl" => leftCtrl, 
				"leftMeta" => leftMeta, 
				"leftAlt" => leftAlt, 
				"space" => space, 
				"rightAlt" => rightAlt, 
				"rightCtrl" => rightCtrl, 
				"leftArrow" => leftArrow, 
				"downArrow" => downArrow, 
				"rightArrow" => rightArrow, 
				"numpad0" => numpad0, 
				"numpadPeriod" => numpadPeriod, 
				_ => null, 
			};
		}
	}

	public GamepadIcons xbox;

	public GamepadIcons ps4;

	public Keyboard keyboard;

	private void OnEnable()
	{
		RebindAction[] componentsInChildren = base.transform.GetComponentsInChildren<RebindAction>();
		foreach (RebindAction obj in componentsInChildren)
		{
			obj.onBindingDisplayUpdate.AddListener(UpdateBindingDisplay);
			obj.UpdateBindingDisplay();
		}
	}

	protected void UpdateBindingDisplay(RebindAction action, string bindingDisplayString, string deviceLayoutName, string controlPath)
	{
		if (!string.IsNullOrEmpty(deviceLayoutName) && !string.IsNullOrEmpty(controlPath))
		{
			Sprite sprite = null;
			sprite = (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "DualShockGamepad") ? ps4.GetSprite(controlPath) : ((!InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Keyboard")) ? xbox.GetSprite(controlPath) : keyboard.GetSprite(controlPath)));
			TextMeshProUGUI bindingtext = action.Bindingtext;
			Image component = bindingtext.transform.parent.Find("ActionBindingIcon").GetComponent<Image>();
			if (sprite != null)
			{
				bindingtext.gameObject.SetActive(value: false);
				component.sprite = sprite;
				component.gameObject.SetActive(value: true);
			}
			else
			{
				bindingtext.gameObject.SetActive(value: true);
				component.gameObject.SetActive(value: false);
			}
		}
	}
}
