using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zorro.ControllerSupport;
using Zorro.Core;
using Zorro.Core.CLI;

public class Modal : RetrievableResourceSingleton<Modal>
{
	public Transform ButtonContainer;

	public TextMeshProUGUI TitleText;

	public TextMeshProUGUI BodyText;

	public GameObject m_buttonPrefab;

	public CanvasGroup m_canvasGroup;

	private bool m_show;

	public bool Open => m_show;

	protected override void OnCreated()
	{
		base.OnCreated();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public static void Show(string title, string body, ModalOption[] options, Action onClosed = null)
	{
		RetrievableResourceSingleton<Modal>.Instance.ShowModal(title, body, options, onClosed);
	}

	private void ShowModal(string title, string body, ModalOption[] options, Action onClosed)
	{
		if (Open)
		{
			return;
		}
		ButtonContainer.ClearChildren();
		Debug.Log("Showing modal with title: " + title);
		TitleText.text = title;
		BodyText.text = body;
		m_show = true;
		foreach (ModalOption option in options)
		{
			Button component = UnityEngine.Object.Instantiate(m_buttonPrefab, ButtonContainer).GetComponent<Button>();
			component.GetComponentInChildren<TextMeshProUGUI>().text = option.Text;
			component.onClick.AddListener(delegate
			{
				Debug.Log("Clicked on option: " + option.Text);
				option.OnClick?.Invoke();
				m_show = false;
				InputHandler.AddInputBlock();
				onClosed?.Invoke();
				if (EventSystem.current.currentSelectedGameObject != null)
				{
					EventSystem.current.SetSelectedGameObject(null);
				}
				foreach (Transform item in ButtonContainer.transform)
				{
					item.HasComponent(delegate(Button button1)
					{
						UnityEngine.Object.Destroy(button1.gameObject);
					});
				}
			});
		}
		for (int num = 0; num < options.Length; num++)
		{
			Button component2 = ButtonContainer.GetChild(num).GetComponent<Button>();
			Navigation navigation = new Navigation
			{
				mode = Navigation.Mode.Explicit
			};
			if (num > 0)
			{
				navigation.selectOnLeft = ButtonContainer.GetChild(num - 1).GetComponent<Button>();
			}
			if (num < options.Length - 1)
			{
				navigation.selectOnRight = ButtonContainer.GetChild(num + 1).GetComponent<Button>();
			}
			component2.navigation = navigation;
		}
		if (InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad)
		{
			EventSystem.current.SetSelectedGameObject(ButtonContainer.GetChild(0).gameObject);
		}
	}

	private void Update()
	{
		float b = (m_show ? 1 : 0);
		m_canvasGroup.alpha = Mathf.Lerp(m_canvasGroup.alpha, b, Time.unscaledDeltaTime * 20f);
		m_canvasGroup.blocksRaycasts = m_show;
		m_canvasGroup.interactable = m_show;
		if (Open && InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad)
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject == null || !currentSelectedGameObject.transform.IsChildOf(ButtonContainer))
			{
				EventSystem.current.SetSelectedGameObject(ButtonContainer.GetChild(0).gameObject);
			}
		}
	}

	[ConsoleCommand]
	public static void ShowTest()
	{
		Show("Delete Save", "Are you sure you want to delete this save?", new ModalOption[2]
		{
			new ModalOption("Yes"),
			new ModalOption("No")
		});
	}

	public static void ShowError(string title, string body)
	{
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Ok);
		Show(title, body, new ModalOption[1]
		{
			new ModalOption(localizedString)
		});
	}
}
