using System;
using System.Collections.Generic;
using Mimicraft.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class DialogView : MonoBehaviour
	{
		private static DialogView instance;

		private const string ResourcePath = "Dialog";

		private static bool warnedMissingPrefab;

		[SerializeField]
		private TextMeshProUGUI messageLabel;

		[SerializeField]
		private TextMeshProUGUI errorLabel;

		[SerializeField]
		private TMP_InputField inputField;

		[SerializeField]
		private Button confirmButton;

		[SerializeField]
		private Button alternateButton;

		[SerializeField]
		private Button cancelButton;

		[SerializeField]
		private RectTransform card;

		private TextMeshProUGUI confirmLabel;

		private TextMeshProUGUI alternateLabel;

		private TextMeshProUGUI cancelLabel;

		private Action<DialogAnswer, string> pending;

		private bool requiresInput;

		private static readonly Color Neutral = new Color(0.26f, 0.26f, 0.26f, 1f);

		private static readonly Color Destructive = new Color(0.45f, 0.19f, 0.17f, 1f);

		private readonly HashSet<string> warnedMissingButtons = new HashSet<string>();

		public static DialogView Instance
		{
			get
			{
				if (instance != null)
				{
					return instance;
				}
				instance = UnityEngine.Object.FindFirstObjectByType<DialogView>(FindObjectsInactive.Include);
				if (instance != null)
				{
					return instance;
				}
				GameObject gameObject = Resources.Load<GameObject>("Dialog");
				if (gameObject == null)
				{
					if (!warnedMissingPrefab)
					{
						warnedMissingPrefab = true;
						Debug.LogWarning("[Dialog] Resources/Dialog bulunamadi - onay ve giris pencereleri gosterilemeyecek." + Elsewhere());
					}
					return null;
				}
				GameObject obj = UnityEngine.Object.Instantiate(gameObject);
				obj.name = gameObject.name;
				UnityEngine.Object.DontDestroyOnLoad(obj);
				instance = obj.GetComponentInChildren<DialogView>(includeInactive: true);
				if (instance != null)
				{
					instance.gameObject.SetActive(value: false);
				}
				return instance;
			}
		}

		public bool IsOpen => base.gameObject.activeSelf;

		public static bool IsShowing
		{
			get
			{
				if (instance != null)
				{
					return instance.IsOpen;
				}
				return false;
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}

		private static string Elsewhere()
		{
			return " Uzerinde DialogView olan bir prefab'i bir Resources klasorune 'Dialog' adiyla koy.";
		}

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			CacheLabels();
			Listen(cancelButton, DialogAnswer.Cancel);
			Listen(alternateButton, DialogAnswer.Alternate);
			Listen(confirmButton, DialogAnswer.Confirm);
			if (inputField != null)
			{
				inputField.onSubmit.RemoveAllListeners();
				inputField.onSubmit.AddListener(delegate
				{
					Answer(DialogAnswer.Confirm);
				});
			}
		}

		private void OnDestroy()
		{
			if (instance == this)
			{
				instance = null;
			}
		}

		private void CacheLabels()
		{
			if (confirmLabel == null && confirmButton != null)
			{
				confirmLabel = confirmButton.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
			}
			if (alternateLabel == null && alternateButton != null)
			{
				alternateLabel = alternateButton.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
			}
			if (cancelLabel == null && cancelButton != null)
			{
				cancelLabel = cancelButton.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
			}
		}

		public static void Confirm(string message, Action onConfirm, string confirmLabel = null, string cancelLabel = null)
		{
			Show(new DialogRequest(message, confirmLabel ?? Loc.Get("Common.Yes"), cancelLabel ?? Loc.Get("Common.No")), delegate(DialogAnswer answer, string _)
			{
				if (answer == DialogAnswer.Confirm)
				{
					onConfirm?.Invoke();
				}
			});
		}

		public static void Prompt(string message, string prefill, Action<string> onConfirm, string confirmLabel = null, string cancelLabel = null, bool requiresInput = true)
		{
			Show(new DialogRequest(message, confirmLabel ?? Loc.Get("Common.Save"), cancelLabel ?? Loc.Get("Common.Cancel"), null, wantsInput: true, prefill, null, inputIsPassword: false, requiresInput), delegate(DialogAnswer answer, string text)
			{
				if (answer == DialogAnswer.Confirm)
				{
					onConfirm?.Invoke(text);
				}
			});
		}

		public static void Show(DialogRequest request, Action<DialogAnswer, string> answered)
		{
			DialogView dialogView = Instance;
			if (dialogView == null)
			{
				answered?.Invoke(DialogAnswer.Cancel, "");
			}
			else
			{
				dialogView.Present(request, answered);
			}
		}

		public void Present(DialogRequest request, Action<DialogAnswer, string> answered)
		{
			if (pending != null)
			{
				Action<DialogAnswer, string> action = pending;
				pending = null;
				action(DialogAnswer.Cancel, "");
			}
			base.gameObject.SetActive(value: true);
			CacheLabels();
			if (messageLabel != null)
			{
				messageLabel.gameObject.SetActive(!string.IsNullOrEmpty(request.Message));
				messageLabel.text = request.Message ?? "";
			}
			SetError("");
			if (inputField != null)
			{
				inputField.gameObject.SetActive(request.WantsInput);
				if (request.WantsInput)
				{
					inputField.contentType = (request.InputIsPassword ? TMP_InputField.ContentType.Password : TMP_InputField.ContentType.Standard);
					inputField.text = request.InputPrefill ?? "";
					if (inputField.placeholder is TextMeshProUGUI textMeshProUGUI)
					{
						textMeshProUGUI.text = request.InputPlaceholder ?? "";
					}
					inputField.Select();
					inputField.ActivateInputField();
				}
			}
			requiresInput = request.WantsInput && request.RequiresInput;
			WarnMissing(confirmButton, request.ConfirmLabel, "Confirm");
			WarnMissing(alternateButton, request.AlternateLabel, "Alternate");
			WarnMissing(cancelButton, request.CancelLabel, "Cancel");
			Label(confirmButton, confirmLabel, request.ConfirmLabel);
			Label(alternateButton, alternateLabel, request.AlternateLabel);
			Label(cancelButton, cancelLabel, request.CancelLabel);
			Tint(alternateButton, request.AlternateIsDestructive ? Destructive : Neutral);
			TintText(alternateButton, Color.white);
			pending = answered;
			RefreshConfirmInteractable();
			if (card != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(card);
			}
		}

		private static void Tint(Button button, Color color)
		{
			if (button != null && button.TryGetComponent<Image>(out var component))
			{
				component.color = color;
			}
		}

		private static void TintText(Button button, Color color)
		{
			if (button != null)
			{
				TextMeshProUGUI componentInChildren = button.GetComponentInChildren<TextMeshProUGUI>();
				if ((object)componentInChildren != null)
				{
					componentInChildren.color = color;
				}
			}
		}

		private void Listen(Button button, DialogAnswer answer)
		{
			if (!(button == null))
			{
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(delegate
				{
					Answer(answer);
				});
			}
		}

		private void WarnMissing(Button button, string label, string which)
		{
			if (!(button != null) && !string.IsNullOrEmpty(label) && !warnedMissingButtons.Contains(which))
			{
				warnedMissingButtons.Add(which);
				Debug.LogWarning("[Dialog] Prefab'da " + which + " butonu yok - '" + label + "' secenegi sunulamiyor.", this);
			}
		}

		private static void Label(Button button, TextMeshProUGUI label, string text)
		{
			bool flag = !string.IsNullOrEmpty(text);
			if (button != null)
			{
				button.gameObject.SetActive(flag);
			}
			if (flag && label != null)
			{
				label.text = text;
			}
		}

		public void SetError(string message)
		{
			if (!(errorLabel == null))
			{
				errorLabel.text = message ?? "";
				errorLabel.gameObject.SetActive(!string.IsNullOrEmpty(message));
			}
		}

		public static void ShowError(string message)
		{
			if (instance != null && instance.IsOpen)
			{
				instance.SetError(message);
			}
		}

		public void Hide()
		{
			pending = null;
			base.gameObject.SetActive(value: false);
		}

		private void Answer(DialogAnswer answer)
		{
			if (answer != DialogAnswer.Confirm || CanConfirm())
			{
				string arg = ((inputField != null) ? inputField.text : "");
				Action<DialogAnswer, string> action = pending;
				pending = null;
				base.gameObject.SetActive(value: false);
				action?.Invoke(answer, arg);
			}
		}

		private bool CanConfirm()
		{
			if (requiresInput)
			{
				return !string.IsNullOrWhiteSpace(inputField?.text);
			}
			return true;
		}

		private void RefreshConfirmInteractable()
		{
			if (confirmButton != null)
			{
				confirmButton.interactable = CanConfirm();
			}
		}

		private void Update()
		{
			if (!IsOpen)
			{
				return;
			}
			if (requiresInput)
			{
				RefreshConfirmInteractable();
			}
			Keyboard current = Keyboard.current;
			if (current != null && current.escapeKey.wasPressedThisFrame)
			{
				GameMenuState.RequestEscape(100, delegate
				{
					Answer(DialogAnswer.Cancel);
				});
			}
		}
	}
}
