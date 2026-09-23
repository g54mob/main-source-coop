using System.Collections.Generic;
using Mimicraft.Customization;
using Mimicraft.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class WeaponSaveBarView : MonoBehaviour
	{
		[Tooltip("Bütün silah modellerini kaydeden buton.")]
		[SerializeField]
		private Button saveButton;

		[Tooltip("SEÇİLİ silahı varsayılan modeline döndüren buton. İsteğe bağlı.")]
		[SerializeField]
		private Button resetButton;

		[Tooltip("Bu silah icin hazir modelleri listeleyen buton. Secilen model ekrana gelir; diske Kaydet'e basana kadar hicbir sey yazilmaz.")]
		[SerializeField]
		private Button presetButton;

		[Tooltip("Sıradaki / önceki silaha geçen butonlar. İsteğe bağlı.")]
		[SerializeField]
		private Button nextButton;

		[SerializeField]
		private Button previousButton;

		[Tooltip("Seçili silahın adının yazılacağı yer. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI selectedLabel;

		[Tooltip("Ekrandan çıkan buton.")]
		[SerializeField]
		private Button closeButton;

		[Tooltip("İkinci kez basılmasını bekleyen onay göstergesi - 'Emin misin?' yazısı gibi. Reset butonu varsa önerilir.")]
		[SerializeField]
		private GameObject resetConfirmIndicator;

		[Tooltip("Kaydedildi / sıfırlandı bilgisinin yazılacağı yer. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI statusLabel;

		[Tooltip("Durum yazısının ve sıfırlama onayının ekranda kalma süresi, saniye.")]
		[SerializeField]
		[Min(0.5f)]
		private float statusSeconds = 4f;

		[Header("Skin kütüphanesi")]
		[Tooltip("Açık skinin adı. Yazdıkça isim değişir; Kaydet'e basınca diske o adla yazılır.")]
		[SerializeField]
		private TMP_InputField nameField;

		[Tooltip("Ekrandakini YENİ bir skin olarak kaydeder - açık olanın üstüne yazmaz.")]
		[SerializeField]
		private Button saveAsNewButton;

		[Tooltip("Kayıtlı skin listesini açıp kapatan buton.")]
		[SerializeField]
		private Button browseButton;

		[Tooltip("Listeyi taşıyan panel. Kapalı başlamalı.")]
		[SerializeField]
		private GameObject listPanel;

		[Tooltip("Satırların konacağı yer. Bir Vertical Layout Group olmalı.")]
		[SerializeField]
		private Transform rowContainer;

		[Tooltip("Kopyalanacak satır. Kapalı olmalı - şablonun kendisi listede görünmez.")]
		[SerializeField]
		private WeaponSkinRowView rowTemplate;

		[Tooltip("Bu silah için hiç kayıt yoksa açılacak obje. İsteğe bağlı.")]
		[SerializeField]
		private GameObject emptyState;

		private WeaponCustomizationView screen;

		private float statusClearTime;

		private float resetArmedUntil;

		private readonly List<WeaponSkinRowView> rows = new List<WeaponSkinRowView>();

		private string pendingDeletePath;

		private float deleteArmedUntil;

		private void Awake()
		{
			if (saveButton != null)
			{
				saveButton.onClick.AddListener(OnSave);
			}
			if (resetButton != null)
			{
				resetButton.onClick.AddListener(OnReset);
			}
			if (presetButton != null)
			{
				presetButton.onClick.AddListener(delegate
				{
					SetResetArmed(armed: false);
					screen?.ShowPresets();
				});
			}
			if (closeButton != null)
			{
				closeButton.onClick.AddListener(OnClose);
			}
			if (nextButton != null)
			{
				nextButton.onClick.AddListener(delegate
				{
					screen?.SelectNext();
					RefreshForWeapon();
				});
			}
			if (browseButton != null)
			{
				browseButton.onClick.RemoveAllListeners();
				browseButton.onClick.AddListener(ToggleList);
			}
			if (saveAsNewButton != null)
			{
				saveAsNewButton.onClick.RemoveAllListeners();
				saveAsNewButton.onClick.AddListener(OnSaveAsNew);
			}
			if (nameField != null)
			{
				nameField.onValueChanged.RemoveAllListeners();
				nameField.onValueChanged.AddListener(OnNameChanged);
			}
			if (previousButton != null)
			{
				previousButton.onClick.AddListener(delegate
				{
					screen?.SelectPrevious();
					RefreshForWeapon();
				});
			}
			SetResetArmed(armed: false);
		}

		public void Bind(WeaponCustomizationView screen)
		{
			this.screen = ((screen != null) ? screen : GetComponentInParent<WeaponCustomizationView>(includeInactive: true));
		}

		private void OnEnable()
		{
			if (screen == null)
			{
				Bind(null);
			}
			SetResetArmed(armed: false);
			RefreshName();
			WeaponSkinPortraitService.PortraitWritten += OnPortraitWritten;
			if (listPanel != null)
			{
				listPanel.SetActive(value: false);
			}
		}

		private void OnDisable()
		{
			WeaponSkinPortraitService.PortraitWritten -= OnPortraitWritten;
		}

		private void OnPortraitWritten(string skinFilePath)
		{
			if (base.isActiveAndEnabled && listPanel != null && listPanel.activeSelf)
			{
				RefreshList();
			}
		}

		private void OnSave()
		{
			if (!(screen == null))
			{
				screen.Save();
				RefreshList();
				SetStatus(Loc.Get("WeaponSkin.Saved"));
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
			if (screen == null || rowContainer == null || rowTemplate == null)
			{
				return;
			}
			rowTemplate.gameObject.SetActive(value: false);
			List<WeaponSkinListEntry> list = screen.ListSkins();
			string currentFilePath = screen.CurrentFilePath;
			for (int i = 0; i < list.Count; i++)
			{
				WeaponSkinRowView weaponSkinRowView = TakeRow(i);
				weaponSkinRowView.gameObject.SetActive(value: true);
				weaponSkinRowView.Bind(list[i].FilePath, list[i].SkinName, list[i].FilePath == currentFilePath, list[i].IsLegacy, OnLoadSkin, OnDuplicateSkin, OnDeleteSkin);
			}
			for (int j = list.Count; j < rows.Count; j++)
			{
				if (rows[j] != null)
				{
					rows[j].gameObject.SetActive(value: false);
				}
			}
			if (emptyState != null)
			{
				emptyState.SetActive(list.Count == 0);
			}
			WeaponSkinPortraitService.RequestMissing(screen.SelectedWeaponId);
			UILayout.RebuildFrom(rowContainer);
		}

		private WeaponSkinRowView TakeRow(int index)
		{
			while (rows.Count <= index)
			{
				WeaponSkinRowView weaponSkinRowView = Object.Instantiate(rowTemplate, rowContainer);
				weaponSkinRowView.name = rowTemplate.name + " (" + rows.Count + ")";
				rows.Add(weaponSkinRowView);
			}
			return rows[index];
		}

		private void OnLoadSkin(string filePath)
		{
			screen?.LoadSkin(filePath);
			RefreshName();
			RefreshList();
			SetStatus(Loc.Get("WeaponSkin.Loaded"));
		}

		private void OnDuplicateSkin(string filePath)
		{
			if (!(screen == null))
			{
				WeaponSkinStorage.LoadFile(filePath, out var _, out var skinName);
				screen.DuplicateSkin(filePath, Loc.Format("WeaponSkin.CopyName", skinName));
				RefreshName();
				RefreshList();
			}
		}

		private void OnDeleteSkin(string filePath)
		{
			if (!(screen == null))
			{
				if (pendingDeletePath != filePath || Time.unscaledTime > deleteArmedUntil)
				{
					pendingDeletePath = filePath;
					deleteArmedUntil = Time.unscaledTime + statusSeconds;
					WeaponSkinStorage.LoadFile(filePath, out var _, out var skinName);
					SetStatus(Loc.Format("WeaponSkin.ConfirmDelete", skinName));
				}
				else
				{
					pendingDeletePath = null;
					screen.DeleteSkin(filePath);
					RefreshName();
					RefreshList();
					SetStatus(Loc.Get("WeaponSkin.Deleted"));
				}
			}
		}

		private void OnSaveAsNew()
		{
			if (!(screen == null))
			{
				screen.SaveAsNewSkin(Loc.Format("WeaponSkin.CopyName", screen.CurrentSkinName));
				RefreshName();
				RefreshList();
				SetStatus(Loc.Get("WeaponSkin.Saved"));
			}
		}

		private void OnNameChanged(string value)
		{
			screen?.SetSkinName(value);
		}

		public void RefreshForWeapon(string status = null)
		{
			SetResetArmed(armed: false);
			pendingDeletePath = null;
			RefreshName();
			if (listPanel != null && listPanel.activeSelf)
			{
				RefreshList();
			}
			if (!string.IsNullOrEmpty(status))
			{
				SetStatus(status);
			}
		}

		private void RefreshName()
		{
			if (nameField != null && screen != null)
			{
				nameField.SetTextWithoutNotify(screen.CurrentSkinName);
			}
		}

		private void OnReset()
		{
			if (!(screen == null))
			{
				if (Time.unscaledTime > resetArmedUntil)
				{
					resetArmedUntil = Time.unscaledTime + statusSeconds;
					SetResetArmed(armed: true);
					SetStatus("'" + screen.SelectedName + "' varsayılanına dönecek - onaylamak için tekrar bas.");
				}
				else
				{
					SetResetArmed(armed: false);
					screen.ResetSelected();
					SetStatus("'" + screen.SelectedName + "' varsayılanına döndürüldü.");
				}
			}
		}

		private void OnClose()
		{
			screen?.Close();
		}

		private void SetResetArmed(bool armed)
		{
			if (!armed)
			{
				resetArmedUntil = 0f;
			}
			if (resetConfirmIndicator != null)
			{
				resetConfirmIndicator.SetActive(armed);
			}
		}

		private void SetStatus(string text)
		{
			if (!(statusLabel == null))
			{
				statusLabel.text = text;
				statusClearTime = Time.unscaledTime + statusSeconds;
			}
		}

		private void Update()
		{
			if (resetArmedUntil > 0f && Time.unscaledTime > resetArmedUntil)
			{
				SetResetArmed(armed: false);
			}
			if (statusLabel != null && statusLabel.text.Length > 0 && Time.unscaledTime >= statusClearTime)
			{
				statusLabel.text = "";
			}
			if (selectedLabel != null && screen != null && selectedLabel.text != screen.SelectedName)
			{
				selectedLabel.text = screen.SelectedName;
			}
		}
	}
}
