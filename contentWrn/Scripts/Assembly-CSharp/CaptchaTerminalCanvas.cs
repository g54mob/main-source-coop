using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using Zorro.ControllerSupport;

public class CaptchaTerminalCanvas : MonoBehaviour
{
	[Serializable]
	public struct CaptchaButton
	{
		[Serializable]
		public struct ButtonIcon
		{
			public GamepadType Gamepad;

			public Sprite Icon;
		}

		public string Character;

		public GamepadButton Button;

		public ButtonIcon[] Icons;
	}

	public TextMeshProUGUI triesText;

	public TextMeshProUGUI captchaText;

	public RectTransform captchaButtonsRoot;

	public TextMeshProUGUI inputText;

	public RectTransform inputButtonsRoot;

	public TextMeshProUGUI timerText;

	public float showFailureTimeForTime = 0.5f;

	public GameObject root;

	public GameObject terminalGo;

	public GameObject failureGo;

	public CaptchaButton[] captchaButtons;

	private void Awake()
	{
		root.SetActive(value: false);
	}

	public void Show()
	{
		root.SetActive(value: true);
	}

	public void LateUpdate()
	{
		if (root.activeSelf)
		{
			bool flag = InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad;
			captchaText.gameObject.SetActive(!flag);
			inputText.gameObject.SetActive(!flag);
			captchaButtonsRoot.gameObject.SetActive(flag);
			inputButtonsRoot.gameObject.SetActive(flag);
		}
	}

	public void StartGame(int maxTries, string captcha, float tryTime)
	{
		root.SetActive(value: true);
		triesText.text = $"{maxTries}/{maxTries}";
		timerText.text = tryTime.ToString(CultureInfo.InvariantCulture);
	}

	public void SetCaptcha(string textCaptcha, string buttonCaptcha)
	{
		captchaText.text = textCaptcha;
		if (Gamepad.current != null)
		{
			GamepadType gamepadType = InputHandler.GetGamepadType();
			for (int num = captchaButtonsRoot.childCount - 1; num >= 0; num--)
			{
				UnityEngine.Object.Destroy(captchaButtonsRoot.GetChild(num).gameObject);
			}
			foreach (char character in buttonCaptcha)
			{
				Sprite icon = GetIcon(character, gamepadType);
				AddButton(character, icon, captchaButtonsRoot);
			}
		}
	}

	private Sprite GetIcon(char character, GamepadType gamepad)
	{
		CaptchaButton[] array = captchaButtons;
		for (int i = 0; i < array.Length; i++)
		{
			CaptchaButton captchaButton = array[i];
			if (captchaButton.Character[0] != character)
			{
				continue;
			}
			CaptchaButton.ButtonIcon[] icons = captchaButton.Icons;
			for (int j = 0; j < icons.Length; j++)
			{
				CaptchaButton.ButtonIcon buttonIcon = icons[j];
				if (buttonIcon.Gamepad == gamepad)
				{
					return buttonIcon.Icon;
				}
			}
			break;
		}
		Debug.LogError($"No Captcha Icon for [ {character} ] on {gamepad}");
		return null;
	}

	public void SetInput(string input)
	{
		inputText.text = input;
	}

	public void AddChar(char input)
	{
		if (input != 0)
		{
			inputText.text += input;
		}
	}

	public string GetButtonString()
	{
		Gamepad current = Gamepad.current;
		if (current == null)
		{
			return string.Empty;
		}
		CaptchaButton[] array = captchaButtons;
		for (int i = 0; i < array.Length; i++)
		{
			CaptchaButton captchaButton = array[i];
			if (current[captchaButton.Button].wasPressedThisFrame)
			{
				return captchaButton.Character;
			}
		}
		return string.Empty;
	}

	public void SetButtons(string input)
	{
		if (input.Length > 0)
		{
			for (int i = 0; i < input.Length; i++)
			{
				AddButton(input[i]);
			}
			return;
		}
		for (int num = inputButtonsRoot.childCount - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(inputButtonsRoot.GetChild(num).gameObject);
		}
	}

	public void AddButton(char input)
	{
		if (Gamepad.current != null && input != 0)
		{
			GamepadType gamepadType = InputHandler.GetGamepadType();
			Sprite icon = GetIcon(input, gamepadType);
			AddButton(input, icon, inputButtonsRoot);
		}
	}

	private void AddButton(char character, Sprite icon, RectTransform parent)
	{
		GameObject obj = new GameObject(character.ToString());
		obj.transform.SetParent(parent);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		obj.transform.localScale = Vector3.one;
		obj.AddComponent<CanvasRenderer>();
		Image image = obj.AddComponent<Image>();
		image.color = Color.black;
		image.sprite = icon;
	}

	public void SetTries(int triesLeft, int maxTries)
	{
		triesText.text = $"{maxTries - triesLeft}";
	}

	private IEnumerator FailScreen(bool shake)
	{
		float elapsed = showFailureTimeForTime;
		failureGo.SetActive(value: true);
		terminalGo.SetActive(value: false);
		if (shake)
		{
			GamefeelHandler.instance.perlin.AddShake(MainCamera.instance.transform.position, 1f, 0.3f);
		}
		while (elapsed > 0f)
		{
			elapsed -= Time.deltaTime;
			yield return null;
		}
		failureGo.SetActive(value: false);
		terminalGo.SetActive(value: true);
	}

	public void DoFailStuff(bool shake)
	{
		StartCoroutine(FailScreen(shake));
	}

	public void SetTimer(float timeLeft, float totalTime)
	{
		timerText.text = ((int)timeLeft).ToString(CultureInfo.InvariantCulture);
	}
}
