using System;
using Mimicraft.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class KeybindingRowView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI labelText;

		[Tooltip("Tuşun kendisi - 'W', 'Left Ctrl'. Yeniden atama sırasında yerini '...' alır.")]
		[SerializeField]
		private TextMeshProUGUI bindingText;

		[Tooltip("Tıklanınca yeni tuşu bekler. Boş bırakılırsa bu objenin kendi Button'ı aranır.")]
		[SerializeField]
		private Button rebindButton;

		[Tooltip("Yalnızca bu satırı varsayılana döndürür. İsteğe bağlı - grup sıfırlama zaten listede var.")]
		[SerializeField]
		private Button resetButton;

		[Tooltip("Aynı haritada başka bir komutla aynı tuşa denk gelince tuş yazısı bu renge boyanır.")]
		[SerializeField]
		private Color conflictColor = new Color(0.95f, 0.35f, 0.3f, 1f);

		private Color restingColor;

		private bool restingColorCaptured;

		private string commandId;

		private Action changed;

		public string CommandId => commandId;

		public void Bind(GameInput.Command command, Action onChanged)
		{
			commandId = command.Id;
			changed = onChanged;
			if (rebindButton == null)
			{
				rebindButton = GetComponent<Button>();
			}
			if (rebindButton != null)
			{
				rebindButton.onClick.RemoveAllListeners();
				rebindButton.onClick.AddListener(BeginRebind);
			}
			if (resetButton != null)
			{
				resetButton.onClick.RemoveAllListeners();
				resetButton.onClick.AddListener(ResetBinding);
			}
			Refresh();
		}

		public void Refresh()
		{
			if (GameInput.TryFind(commandId, out var command) && !(bindingText == null))
			{
				if (!restingColorCaptured)
				{
					restingColorCaptured = true;
					restingColor = bindingText.color;
				}
				if (labelText != null)
				{
					labelText.text = command.Label;
				}
				bindingText.text = command.DisplayString;
				bindingText.color = (GameInput.IsConflicting(command) ? conflictColor : restingColor);
			}
		}

		private void BeginRebind()
		{
			if (GameInput.TryFind(commandId, out var command))
			{
				GameInput.Rebind(command, delegate
				{
					Refresh();
					changed?.Invoke();
				});
				if (bindingText != null)
				{
					bindingText.text = "...";
				}
			}
		}

		private void ResetBinding()
		{
			if (GameInput.TryFind(commandId, out var command))
			{
				GameInput.ResetBinding(command);
				Refresh();
				changed?.Invoke();
			}
		}
	}
}
