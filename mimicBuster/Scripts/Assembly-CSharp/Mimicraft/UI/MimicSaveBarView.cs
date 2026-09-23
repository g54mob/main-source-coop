using System.Collections.Generic;
using Mimicraft.Analytics;
using Mimicraft.Customization;
using Mimicraft.Localization;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class MimicSaveBarView : MonoBehaviour
	{
		[Header("Üst çubuk")]
		[Tooltip("Mimic'in adı.")]
		[SerializeField]
		private TMP_InputField nameField;

		[Tooltip("Açık dosyanın üstüne yazan buton.")]
		[SerializeField]
		private Button saveButton;

		[Tooltip("Sıfırdan yeni bir mimic başlatan buton. Model boş küpe döner ve açık dosya bırakılır; diske hiçbir şey yazılmaz, Kaydet'e basana kadar.")]
		[SerializeField]
		private Button createNewButton;

		[Tooltip("Ekrandan çıkan buton.")]
		[SerializeField]
		private Button closeButton;

		[Tooltip("Kaydedildi / yüklendi / silindi bilgisinin yazılacağı yer. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI statusLabel;

		[Tooltip("Durum yazısının ekranda kalma süresi, saniye.")]
		[SerializeField]
		[Min(0.5f)]
		private float statusSeconds = 4f;

		[Header("Kayıtlı mimic'ler")]
		[Tooltip("Listeyi açıp kapayan buton. İsteğe bağlı - liste sürekli açık da olabilir.")]
		[SerializeField]
		private Button browseButton;

		[Tooltip("Browse butonuyla açılıp kapanacak obje. Boş bırakılırsa liste hep açık kalır.")]
		[SerializeField]
		private GameObject listPanel;

		[Tooltip("Satırların altına ekleneceği obje. Boş bırakılırsa List Panel kullanılır.")]
		[SerializeField]
		private Transform rowContainer;

		[Tooltip("Bir satırın şablonu - üzerinde MimicSaveRowView olmalı. Kapalı bırak: kopyalanır.")]
		[SerializeField]
		private MimicSaveRowView rowTemplate;

		[Tooltip("Hiç kayıtlı mimic yokken açılacak obje. İsteğe bağlı.")]
		[SerializeField]
		private GameObject emptyState;

		private readonly List<MimicSaveRowView> rows = new List<MimicSaveRowView>();

		private MimicCustomizationView screen;

		private float statusUntil;

		public string CurrentName
		{
			get
			{
				string text = ((nameField != null) ? nameField.text : "");
				if (!string.IsNullOrWhiteSpace(text))
				{
					return text.Trim();
				}
				string currentTemplateName = VoxelEditorSettings.CurrentTemplateName;
				if (!string.IsNullOrWhiteSpace(currentTemplateName))
				{
					return currentTemplateName;
				}
				return Loc.Get("Mimic.Untitled");
			}
		}

		private void Awake()
		{
			if (rowTemplate != null)
			{
				rowTemplate.gameObject.SetActive(value: false);
			}
			if (saveButton != null)
			{
				saveButton.onClick.AddListener(Save);
			}
			if (createNewButton != null)
			{
				createNewButton.onClick.AddListener(CreateNew);
			}
			if (closeButton != null)
			{
				closeButton.onClick.AddListener(CloseScreen);
			}
			if (browseButton != null)
			{
				browseButton.onClick.AddListener(ToggleList);
			}
			UITooltipTrigger.AttachKey(saveButton, "Tooltip.Mimic.Save");
			UITooltipTrigger.AttachKey(createNewButton, "Tooltip.Mimic.New");
			UITooltipTrigger.AttachKey(browseButton, "Tooltip.Mimic.Browse");
			UITooltipTrigger.AttachKey(closeButton, "Tooltip.Close");
		}

		public void Bind(MimicCustomizationView owner)
		{
			screen = owner;
			RefreshName();
			RefreshList();
		}

		private void OnEnable()
		{
			TemplatePortraitService.PortraitWritten += OnPortraitWritten;
		}

		private void OnDisable()
		{
			TemplatePortraitService.PortraitWritten -= OnPortraitWritten;
		}

		private void OnPortraitWritten(string filePath)
		{
			if (screen == null || string.IsNullOrEmpty(filePath))
			{
				return;
			}
			foreach (TemplateListEntry item in StartingMimic.List())
			{
				if (!(item.FilePath != filePath))
				{
					RefreshList();
					break;
				}
			}
		}

		private void Update()
		{
			if (!(statusLabel == null) && !(statusUntil <= 0f) && !(Time.unscaledTime < statusUntil))
			{
				statusUntil = 0f;
				statusLabel.text = "";
			}
		}

		public void Save()
		{
			if (screen == null)
			{
				return;
			}
			if (!IsBodyLegal())
			{
				SetStatus(Loc.Get("Mimic.Invalid"));
				return;
			}
			string currentName = CurrentName;
			if (!TemplateSession.SaveCurrent(currentName, ""))
			{
				SetStatus(Loc.Get("Mimic.NothingToSave"));
				return;
			}
			StartingMimic.SelectedPath = VoxelEditorSettings.CurrentTemplateFilePath;
			ModelEditSession.MarkSaved();
			Telemetry.Send("mimic_saved", ("voxels", (screen.Model != null && screen.Model.Grid != null) ? screen.Model.Grid.Count : 0), ("mimics_this_session", Telemetry.Bump("mimics")));
			RefreshName();
			RefreshList();
			SetStatus(Loc.Format("Mimic.Saved", currentName));
		}

		private static bool IsBodyLegal()
		{
			List<VoxelBodyPiece> list = VoxelFocusManager.GatherBody();
			if (list.Count > 0)
			{
				return VoxelBodyRules.Evaluate(list).IsValid;
			}
			return false;
		}

		private void CreateNew()
		{
			if (!(screen == null))
			{
				screen.NewMimic();
				if (nameField != null)
				{
					nameField.SetTextWithoutNotify("");
				}
				RefreshList();
			}
		}

		private void CloseScreen()
		{
			if (screen != null)
			{
				screen.Close();
			}
		}

		private void ToggleList()
		{
			if (!(listPanel == null))
			{
				bool flag = !listPanel.activeSelf;
				listPanel.SetActive(flag);
				if (flag)
				{
					RefreshList();
				}
			}
		}

		public void RefreshList()
		{
			foreach (MimicSaveRowView row in rows)
			{
				if (row != null)
				{
					Object.Destroy(row.gameObject);
				}
			}
			rows.Clear();
			List<TemplateListEntry> list = StartingMimic.List();
			if (emptyState != null && emptyState.activeSelf != (list.Count == 0))
			{
				emptyState.SetActive(list.Count == 0);
			}
			if (rowTemplate == null)
			{
				return;
			}
			Transform parent = ((rowContainer != null) ? rowContainer : ((listPanel != null) ? listPanel.transform : base.transform));
			string currentTemplateFilePath = VoxelEditorSettings.CurrentTemplateFilePath;
			foreach (TemplateListEntry entry in list)
			{
				string path = entry.FilePath;
				string entryName = entry.ModelName;
				MimicSaveRowView mimicSaveRowView = Object.Instantiate(rowTemplate, parent);
				mimicSaveRowView.gameObject.SetActive(value: true);
				mimicSaveRowView.Bind(entryName, SavedThumbnails.Load(path), path == currentTemplateFilePath, delegate
				{
					Load(entry);
				}, delegate
				{
					AskThenDelete(path, entryName);
				});
				rows.Add(mimicSaveRowView);
			}
		}

		private void Load(TemplateListEntry entry)
		{
			if (!(screen == null))
			{
				if (!screen.LoadTemplate(entry))
				{
					SetStatus(Loc.Format("Mimic.LoadFailed", entry.ModelName));
					return;
				}
				StartingMimic.SelectedPath = entry.FilePath;
				ModelEditSession.MarkSaved();
				RefreshName();
				RefreshList();
				SetStatus(Loc.Format("Mimic.Loaded", entry.ModelName));
			}
		}

		private void AskThenDelete(string path, string entryName)
		{
			DialogView.Confirm(Loc.Format("Mimic.DeleteConfirm", entryName), delegate
			{
				Delete(path, entryName);
			});
		}

		private void Delete(string path, string entryName)
		{
			bool num = path == VoxelEditorSettings.CurrentTemplateFilePath;
			TemplateSession.DeleteEntry(path);
			if (StartingMimic.IsSelected(path))
			{
				StartingMimic.Clear();
			}
			if (num)
			{
				RefreshName();
			}
			RefreshList();
			SetStatus(Loc.Format("Mimic.Deleted", entryName));
		}

		private void RefreshName()
		{
			if (nameField != null)
			{
				nameField.SetTextWithoutNotify(VoxelEditorSettings.CurrentTemplateName ?? "");
			}
		}

		private void SetStatus(string text)
		{
			if (!(statusLabel == null))
			{
				statusLabel.text = text;
				statusUntil = Time.unscaledTime + statusSeconds;
			}
		}
	}
}
