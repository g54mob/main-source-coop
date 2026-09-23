using System;
using Mimicraft.Customization;
using Mimicraft.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class TauntVoiceRowView : MonoBehaviour
	{
		[Tooltip("Kaydın adı - 'Taunt 1' gibi.")]
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		[Tooltip("Kaydın dalga formu. Satırda göstermek istemiyorsan boş bırak.")]
		[SerializeField]
		private TauntWaveformView waveform;

		[Tooltip("Kaydı dinleten buton.")]
		[SerializeField]
		private Button previewButton;

		[Tooltip("Bu kaydı oyunda kullanılacak ses yapan buton.")]
		[SerializeField]
		private Button useButton;

		[Tooltip("Kullanılan kaydın üzerinde görünecek işaret - onay ikonu, çerçeve, ne istersen. Seçili olmayan satırlarda kapatılır.")]
		[SerializeField]
		private GameObject selectedMark;

		[Tooltip("Bu kaydı her 30 saniyede bir çıkan ZORUNLU taunt sesi yapan buton. İsteğe bağlı.")]
		[SerializeField]
		private Button useForcedButton;

		[Tooltip("Bu satır zorunlu taunt olarak kullanılıyorsa açılacak işaret. İsteğe bağlı.")]
		[SerializeField]
		private GameObject forcedMark;

		[Tooltip("Kaydı silen buton.")]
		[SerializeField]
		private Button deleteButton;

		private Action preview;

		private Action use;

		private Action useForced;

		private Action remove;

		private void Awake()
		{
			if (previewButton != null)
			{
				previewButton.onClick.AddListener(delegate
				{
					preview?.Invoke();
				});
			}
			if (useButton != null)
			{
				useButton.onClick.AddListener(delegate
				{
					use?.Invoke();
				});
			}
			if (useForcedButton != null)
			{
				useForcedButton.onClick.AddListener(delegate
				{
					useForced?.Invoke();
				});
			}
			if (deleteButton != null)
			{
				deleteButton.onClick.AddListener(delegate
				{
					remove?.Invoke();
				});
			}
		}

		public void SetProgress(float value)
		{
			if (waveform != null)
			{
				waveform.Progress = value;
			}
		}

		public void Bind(TauntVoiceLibrary.Entry entry, float[] peaks, bool selected, Action onPreview, Action onUse, Action onDelete)
		{
			Bind(entry, peaks, selected, selectedForced: false, onPreview, onUse, null, onDelete);
		}

		public void Bind(TauntVoiceLibrary.Entry entry, float[] peaks, bool selected, bool selectedForced, Action onPreview, Action onUse, Action onUseForced, Action onDelete)
		{
			preview = onPreview;
			use = onUse;
			useForced = onUseForced;
			remove = onDelete;
			if (forcedMark != null && forcedMark.activeSelf != selectedForced)
			{
				forcedMark.SetActive(selectedForced);
			}
			UITooltipTrigger.AttachKey(previewButton, "Tooltip.Taunt.RowPreview");
			UITooltipTrigger.AttachKey(useButton, "Tooltip.Taunt.Use");
			UITooltipTrigger.AttachKey(useForcedButton, "Tooltip.Taunt.UseForced");
			UITooltipTrigger.AttachKey(deleteButton, "Tooltip.Taunt.Delete");
			if (useForcedButton != null)
			{
				useForcedButton.interactable = !selectedForced;
				TextMeshProUGUI componentInChildren = useForcedButton.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
				if (componentInChildren != null)
				{
					componentInChildren.text = Loc.Get(selectedForced ? "Taunt.InUseForced" : "Taunt.UseForced");
				}
			}
			if (nameLabel != null)
			{
				nameLabel.text = entry.Id;
			}
			if (waveform != null)
			{
				waveform.Show(peaks);
			}
			if (selectedMark != null && selectedMark.activeSelf != selected)
			{
				selectedMark.SetActive(selected);
			}
			if (useButton != null)
			{
				useButton.interactable = !selected;
				TextMeshProUGUI componentInChildren2 = useButton.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
				if (componentInChildren2 != null)
				{
					componentInChildren2.text = Loc.Get(selected ? "Taunt.InUse" : "Taunt.Use");
				}
			}
		}
	}
}
