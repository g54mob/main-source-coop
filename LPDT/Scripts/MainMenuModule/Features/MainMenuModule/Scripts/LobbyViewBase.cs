using System;
using Global.Modules.LocalizationModule.Scripts.Generated;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts
{
	public abstract class LobbyViewBase : ViewBehaviour, IFocusableElement
	{
		public Slider RSlider;

		public Slider GSlider;

		public Slider BSlider;

		public Image FinalColorImage;

		[field: SerializeField]
		public Selectable FirstButtonToSelect { get; private set; }

		[field: SerializeField]
		public Button CopyCodeButton { get; private set; }

		[field: SerializeField]
		public TMP_Text CodeText { get; private set; }

		[field: SerializeField]
		public RectTransform CodeContainer { get; private set; }

		[field: SerializeField]
		public Button StartGameButton { get; private set; }

		[field: SerializeField]
		public Button ExitLobbyButton { get; private set; }

		[field: SerializeField]
		public float SaveColorDelay { get; private set; } = 0.5f;

		[field: SerializeField]
		public float BackButtonClickDelay { get; private set; } = 0.45f;

		[field: SerializeField]
		public TMP_Text WaitingText { get; private set; }

		[field: SerializeField]
		public GameObject WaitingTextContainer { get; private set; }

		[field: SerializeField]
		public LocalizationKey WaitingLocalizationKey { get; private set; }

		[field: SerializeField]
		public float WaitingDotsInterval { get; private set; } = 0.5f;

		[field: SerializeField]
		public Button InviteButton { get; private set; }

		[field: SerializeField]
		public Toggle PublicToggle { get; private set; }

		[field: SerializeField]
		public GameObject PublicToggleContainer { get; private set; }

		[field: SerializeField]
		public TMP_InputField NicknameInput { get; private set; }

		[field: SerializeField]
		public float SaveNicknameDelay { get; private set; } = 1f;

		public bool IsFocused { get; private set; }

		public bool IsFocusable { get; private set; }

		public event Action OnFocused;

		public event Action OnUnFocused;

		public void Focus(Action onInteract)
		{
			IsFocused = true;
		}

		public void UnFocus()
		{
			IsFocused = false;
		}

		public void MakeFocusable()
		{
			if (!IsFocusable)
			{
				IsFocusable = true;
				this.OnFocused?.Invoke();
			}
		}

		public void MakeUnFocusable()
		{
			if (IsFocusable)
			{
				IsFocusable = false;
				this.OnUnFocused?.Invoke();
			}
		}
	}
}
