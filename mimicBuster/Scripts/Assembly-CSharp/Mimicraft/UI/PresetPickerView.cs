using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class PresetPickerView : MonoBehaviour
	{
		[Tooltip("Açılıp kapanacak panel. Boş bırakılırsa bu objenin kendisi kullanılır.")]
		[SerializeField]
		private GameObject panel;

		[Tooltip("Listenin başlığı - hangi silahın/neyin preset'leri olduğunu yazar. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI titleLabel;

		[Tooltip("Satırların altına ekleneceği obje. Boş bırakılırsa panel kullanılır.")]
		[SerializeField]
		private Transform rowContainer;

		[Tooltip("Bir satırın prefab'ı - üzerinde PresetRowView olmalı.")]
		[SerializeField]
		private PresetRowView rowTemplate;

		[Tooltip("Hiç preset yokken açılacak obje. İsteğe bağlı.")]
		[SerializeField]
		private GameObject emptyState;

		[Tooltip("Hiçbir şey seçmeden kapatan buton. İsteğe bağlı.")]
		[SerializeField]
		private Button closeButton;

		[Tooltip("Açıksa bir preset seçilince panel kapanır. Kapalıyken (varsayılan) panel açık kalır ve preset'ler arasında hızlıca geçilebilir; açan butona tekrar basınca kapanır.")]
		[SerializeField]
		private bool closeOnPick;

		private readonly List<PresetRowView> rows = new List<PresetRowView>();

		private Action<int> chosen;

		private object openedFor;

		private bool warnedAboutTemplate;

		private bool warnedAboutHidden;

		public bool IsOpen
		{
			get
			{
				if (Panel != null)
				{
					return Panel.activeSelf;
				}
				return false;
			}
		}

		private GameObject Panel
		{
			get
			{
				if (!(panel != null))
				{
					return base.gameObject;
				}
				return panel;
			}
		}

		public bool IsOpenFor(object source)
		{
			if (IsOpen && openedFor != null)
			{
				return openedFor.Equals(source);
			}
			return false;
		}

		public void Toggle(object source, string title, IReadOnlyList<PresetPickerData> datas, Action<int> chosen)
		{
			if (IsOpenFor(source))
			{
				Hide();
				return;
			}
			Show(title, datas, chosen);
			openedFor = source;
		}

		private void Update()
		{
			if (IsOpen && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				GameMenuState.RequestEscape(100, Hide);
			}
		}

		private void Awake()
		{
			if (closeButton != null)
			{
				closeButton.onClick.AddListener(Hide);
			}
			if (rowTemplate != null)
			{
				rowTemplate.gameObject.SetActive(value: false);
			}
			Panel.SetActive(value: false);
		}

		public void Show(string title, IReadOnlyList<PresetPickerData> datas, Action<int> chosen)
		{
			this.chosen = chosen;
			openedFor = null;
			if (titleLabel != null)
			{
				titleLabel.text = title ?? "";
			}
			Build(datas);
			Panel.SetActive(value: true);
			if (!Panel.activeInHierarchy && !warnedAboutHidden)
			{
				warnedAboutHidden = true;
				Debug.LogWarning("[PresetPickerView] '" + base.gameObject.name + "' acildi ama ekranda degil - ust objelerinden biri kapali.", this);
			}
		}

		public void Hide()
		{
			chosen = null;
			openedFor = null;
			Panel.SetActive(value: false);
		}

		private void Build(IReadOnlyList<PresetPickerData> datas)
		{
			int num = datas?.Count ?? 0;
			if (rowTemplate == null)
			{
				if (!warnedAboutTemplate)
				{
					warnedAboutTemplate = true;
					Debug.LogWarning("[PresetPickerView] '" + base.gameObject.name + "' uzerinde Row Template yok - liste hicbir zaman dolmaz.", this);
				}
				return;
			}
			Transform parent = ((rowContainer != null) ? rowContainer : Panel.transform);
			while (rows.Count < num)
			{
				PresetRowView item = UnityEngine.Object.Instantiate(rowTemplate, parent);
				rows.Add(item);
			}
			for (int i = 0; i < rows.Count; i++)
			{
				bool flag = i < num;
				rows[i].gameObject.SetActive(flag);
				if (flag)
				{
					rows[i].Bind(i, datas[i], OnPicked);
				}
			}
			if (emptyState != null)
			{
				emptyState.SetActive(num == 0);
			}
		}

		private void OnPicked(int index)
		{
			Action<int> action = chosen;
			if (closeOnPick)
			{
				Hide();
			}
			action?.Invoke(index);
		}
	}
}
