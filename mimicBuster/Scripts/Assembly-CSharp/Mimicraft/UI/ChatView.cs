using System;
using System.Collections.Generic;
using Mimicraft.Gameplay;
using Mimicraft.Settings;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class ChatView : MonoBehaviour
	{
		private const int Capacity = 10;

		private const float PanelWidth = 420f;

		private const float RowHeight = 18f;

		private const int FontSize = 13;

		public const int MaxMessageLength = 80;

		[SerializeField]
		private List<TextMeshProUGUI> rows = new List<TextMeshProUGUI>();

		[SerializeField]
		private TMP_InputField inputField;

		[SerializeField]
		private RectTransform panelRect;

		private bool appliedShown = true;

		private readonly List<string> lines = new List<string>();

		private int submitFrame = -1;

		private int typingStartedFrame = -1;

		public static ChatView Instance { get; private set; }

		public static bool Shown { get; set; } = true;

		public bool IsTyping
		{
			get
			{
				if (inputField != null)
				{
					return inputField.gameObject.activeSelf;
				}
				return false;
			}
		}

		public event Action<string> MessageSubmitted;

		public static ChatView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "Chat");
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.zero;
			rectTransform.pivot = Vector2.zero;
			rectTransform.anchoredPosition = new Vector2(16f, 16f);
			rectTransform.sizeDelta = new Vector2(420f, 214f);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 1f;
			verticalLayoutGroup.padding = new RectOffset(6, 6, 6, 6);
			verticalLayoutGroup.childAlignment = TextAnchor.LowerLeft;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			ChatView chatView = rectTransform.gameObject.AddComponent<ChatView>();
			chatView.panelRect = rectTransform;
			for (int i = 0; i < 10; i++)
			{
				TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, $"ChatRow{i}", "", 13);
				textMeshProUGUI.alignment = TextAlignmentOptions.Left;
				textMeshProUGUI.gameObject.AddComponent<LayoutElement>().minHeight = 18f;
				chatView.rows.Add(textMeshProUGUI);
				textMeshProUGUI.gameObject.SetActive(value: false);
			}
			TMP_InputField tMP_InputField = UIFactory.CreateInputField(rectTransform, "ChatInput", "Mesaj yaz...");
			LayoutElement layoutElement = tMP_InputField.gameObject.AddComponent<LayoutElement>();
			layoutElement.minHeight = 24f;
			layoutElement.preferredHeight = 24f;
			tMP_InputField.characterLimit = 80;
			chatView.inputField = tMP_InputField;
			tMP_InputField.gameObject.SetActive(value: false);
			return chatView;
		}

		private void Awake()
		{
			Instance = this;
			if (inputField != null)
			{
				inputField.gameObject.SetActive(value: false);
			}
			if (panelRect == null)
			{
				panelRect = base.transform as RectTransform;
			}
			if (inputField != null)
			{
				inputField.onSubmit.RemoveAllListeners();
				inputField.onSubmit.AddListener(Submit);
			}
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void AddSystemMessage(string message)
		{
			Append("<color=#B4B4B4>" + message + "</color>");
		}

		public void AddPlayerMessage(string composed)
		{
			Append(composed);
		}

		private void Append(string line)
		{
			if (string.IsNullOrEmpty(line))
			{
				return;
			}
			lines.Add(line);
			if (lines.Count > 10)
			{
				lines.RemoveAt(0);
			}
			for (int i = 0; i < rows.Count; i++)
			{
				bool flag = i < lines.Count;
				if (rows[i].gameObject.activeSelf != flag)
				{
					rows[i].gameObject.SetActive(flag);
				}
				if (flag)
				{
					rows[i].text = ChatFilter.Apply(lines[i]);
				}
			}
			if (panelRect != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);
			}
		}

		private void Update()
		{
			ApplyShown();
			if (!Shown || Keyboard.current == null || inputField == null)
			{
				return;
			}
			if (!IsTyping)
			{
				if (Time.frameCount != submitFrame && !GameMenuState.IsMenuOpen && GameInput.Chat.WasPressedThisFrame())
				{
					BeginTyping();
				}
			}
			else if (Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				GameMenuState.RequestEscape(40, EndTyping);
			}
			else if (Time.frameCount > typingStartedFrame + 2 && !inputField.isFocused)
			{
				EndTyping();
			}
		}

		private void ApplyShown()
		{
			if (appliedShown != Shown)
			{
				appliedShown = Shown;
				HudVisibility.Apply(this, Shown);
				if (!Shown && IsTyping)
				{
					EndTyping();
				}
			}
		}

		private void BeginTyping()
		{
			inputField.gameObject.SetActive(value: true);
			inputField.text = "";
			GameMenuState.SetMenuOpen(this, open: true);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			inputField.ActivateInputField();
			typingStartedFrame = Time.frameCount;
		}

		private void EndTyping()
		{
			inputField.DeactivateInputField();
			inputField.text = "";
			inputField.gameObject.SetActive(value: false);
			GameMenuState.SetMenuOpen(this, open: false);
			PlayerEditSession playerEditSession = LocalEditSession();
			if (playerEditSession != null)
			{
				playerEditSession.RefreshCursorState();
				return;
			}
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		private static PlayerEditSession LocalEditSession()
		{
			NetworkObject networkObject = ((NetworkManager.Singleton != null && NetworkManager.Singleton.LocalClient != null) ? NetworkManager.Singleton.LocalClient.PlayerObject : null);
			if (!(networkObject != null))
			{
				return null;
			}
			return networkObject.GetComponent<PlayerEditSession>();
		}

		private void Submit(string text)
		{
			submitFrame = Time.frameCount;
			EndTyping();
			if (!string.IsNullOrWhiteSpace(text))
			{
				this.MessageSubmitted?.Invoke(text);
			}
		}
	}
}
