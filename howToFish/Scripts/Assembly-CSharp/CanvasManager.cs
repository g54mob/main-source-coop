using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
	private static CanvasManager _instance;

	[SerializeField]
	private CanvasGroup _mainMenuCanvas;

	[SerializeField]
	private GameObject _howToFishTitle;

	[SerializeField]
	private List<GameObject> _allMainMenuButtons;

	[SerializeField]
	private GameObject _devStuff;

	[SerializeField]
	private GameObject _discordButton;

	[Header("Permanent Canvas")]
	[SerializeField]
	private CanvasGroup _permaCanvas;

	[SerializeField]
	private TextMeshProUGUI _versionText;

	[Header("Underwater Canvas")]
	[SerializeField]
	private Image _underwaterImage;

	[Header("Black Screen")]
	[SerializeField]
	private Image _blackScreen;

	[SerializeField]
	private GameObject _unseenCharacterNotification;

	private void Awake()
	{
		Setter.SetSingleInstance(ref _instance, this);
		_versionText.text = Application.version ?? "";
	}

	private void Start()
	{
		_mainMenuCanvas.gameObject.SetActive(value: true);
		_devStuff.SetActive(SteamManager.IsDev);
		_discordButton.SetActive(!SteamManager.IsDev);
	}

	public static void ToggleBlackscreen(bool to, bool instant = false, float duration = 3f)
	{
		if ((bool)_instance)
		{
			LeanTween.cancel(_instance._blackScreen.gameObject);
			if (instant)
			{
				_instance._blackScreen.color = (to ? new Color(0f, 0f, 0f, 1f) : new Color(0f, 0f, 0f, 0f));
				return;
			}
			float num = ((!to) ? 1 : 0);
			float to2 = (to ? 1 : 0);
			LeanTween.value(_instance._blackScreen.gameObject, num, to2, duration).setEase(LeanTweenType.easeInOutSine).setOnUpdate(_instance.SetBlackscreenAlpha);
		}
	}

	private void SetBlackscreenAlpha(float alpha)
	{
		_blackScreen.color = new Color(0f, 0f, 0f, alpha);
	}

	public static void ToggleMenuUI(bool enabled, bool instant = false)
	{
		if (!_instance)
		{
			return;
		}
		_instance._mainMenuCanvas.gameObject.SetActive(value: true);
		LeanTween.cancel(_instance._mainMenuCanvas.gameObject);
		if (enabled)
		{
			_instance._howToFishTitle.SetActive(value: true);
		}
		if (instant)
		{
			_instance._mainMenuCanvas.gameObject.SetActive(enabled);
			_instance._mainMenuCanvas.alpha = (enabled ? 1 : 0);
		}
		else
		{
			LeanTween.value(_instance._mainMenuCanvas.gameObject, _instance._mainMenuCanvas.alpha, enabled ? 1 : 0, 1f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(_instance.UpdateMenuAlpha)
				.setOnComplete(_instance.HideMenuCanvas);
		}
		foreach (GameObject allMainMenuButton in _instance._allMainMenuButtons)
		{
			allMainMenuButton.SetActive(value: false);
		}
		_instance._allMainMenuButtons[0].SetActive(enabled);
		LocalSkin.DisablePreview();
		if (enabled)
		{
			UpdateCharacterNotification();
		}
	}

	public static void UpdateCharacterNotification()
	{
		_instance._unseenCharacterNotification.SetActive(SkinManager.HasUnseenClothing());
	}

	private void UpdateMenuAlpha(float alpha)
	{
		_mainMenuCanvas.alpha = alpha;
	}

	private void HideMenuCanvas()
	{
		if (!MainMenuManager.IsInMenu)
		{
			_mainMenuCanvas.gameObject.SetActive(value: false);
		}
	}

	public static void ToggleUnderwaterImage(bool to)
	{
		if ((bool)_instance)
		{
			_instance._underwaterImage.gameObject.SetActive(to);
		}
	}

	public static void TogglePermaCanvas(bool to)
	{
		if ((bool)_instance)
		{
			_instance._permaCanvas.alpha = (to ? 1 : 0);
		}
	}
}
