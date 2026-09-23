using System;
using System.Collections.Generic;
using Mimicraft.Localization;
using Mimicraft.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class KeybindingListView : MonoBehaviour
	{
		[Serializable]
		public struct PremadeRow
		{
			[Tooltip("GameInput.Command.Id - örneğin 'Movement/Jump' ya da 'Movement/Move#Up'. Konsolda 'binds' yazarak tam listeyi görebilirsin.")]
			public string CommandId;

			public KeybindingRowView Row;
		}

		[Tooltip("Hangi grup gösterilecek: 'Common' (her an geçerli - skor tablosu, sohbet, Tab), 'Movement' (yürüme tuşları - insan da model de aynısını kullanır), 'Human' (insanken yapılanlar), 'Modeler' (voxel modelin içindeyken) ya da 'Editor'. GameInput'taki harita adlarıyla birebir aynı yazılmalı. Boş bırakılırsa hepsi listelenir.")]
		[SerializeField]
		private string group = "Movement";

		[Tooltip("Grubun başlığı - 'Hareket', 'İnsan'. Metni tablodan gelir ve dil değişince kendini günceller; elle yazdığın şey silinir. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI groupNameLabel;

		[Tooltip("Klonlanan satırların konacağı yer. Boş bırakılırsa bu obje kullanılır.")]
		[SerializeField]
		private Transform rowContainer;

		[Tooltip("Kopyalanacak satır. Kapalı olmalı - şablonun kendisi listede görünmez.")]
		[SerializeField]
		private KeybindingRowView rowTemplate;

		[Tooltip("Elle yerleştirilmiş satırlar. Burada olan bir komut için şablon klonlanmaz.")]
		[SerializeField]
		private List<PremadeRow> premadeRows = new List<PremadeRow>();

		[Tooltip("Bu gruptaki her tuşu varsayılana döndürür.")]
		[SerializeField]
		private Button resetGroupButton;

		[Tooltip("Bir tuş birden fazla komuta denk geldiğinde uyarıyı yazar. Çakışma yoksa gizlenir.")]
		[SerializeField]
		private TextMeshProUGUI conflictLabel;

		[Tooltip("Yeni tuş beklenirken açılan uyarı ('Bir tuşa bas, iptal için Esc'). İsteğe bağlı.")]
		[SerializeField]
		private GameObject rebindPrompt;

		private readonly List<KeybindingRowView> clones = new List<KeybindingRowView>();

		private readonly List<KeybindingRowView> live = new List<KeybindingRowView>();

		private bool warnedAboutLayout;

		private void OnEnable()
		{
			Loc.Changed += RefreshAll;
			Rebuild();
		}

		private void OnDisable()
		{
			Loc.Changed -= RefreshAll;
			GameInput.CancelRebind();
			if (rebindPrompt != null)
			{
				rebindPrompt.SetActive(value: false);
			}
		}

		private void Update()
		{
			bool flag = IsRebinding();
			if (rebindPrompt != null && rebindPrompt.activeSelf != flag)
			{
				rebindPrompt.SetActive(flag);
			}
			if (flag && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				GameMenuState.RequestEscape(100, delegate
				{
				});
			}
		}

		private bool IsRebinding()
		{
			return GameInput.IsRebinding;
		}

		private void ApplyGroupName()
		{
			if (!(groupNameLabel == null) && !string.IsNullOrEmpty(group))
			{
				string text = GameInput.GroupLabelKey(group);
				if (string.IsNullOrEmpty(text))
				{
					groupNameLabel.text = group;
				}
				else
				{
					LocalizedText.Attach(groupNameLabel, text);
				}
			}
		}

		public void Rebuild()
		{
			live.Clear();
			ApplyGroupName();
			if (rowTemplate != null)
			{
				rowTemplate.gameObject.SetActive(value: false);
			}
			int num = 0;
			foreach (GameInput.Command command in GameInput.Commands)
			{
				if (string.IsNullOrEmpty(group) || !(command.Group != group))
				{
					KeybindingRowView keybindingRowView = FindPremade(command.Id) ?? TakeClone(num++);
					if (!(keybindingRowView == null))
					{
						keybindingRowView.gameObject.SetActive(value: true);
						keybindingRowView.Bind(command, RefreshAll);
						live.Add(keybindingRowView);
					}
				}
			}
			for (int i = num; i < clones.Count; i++)
			{
				if (clones[i] != null)
				{
					clones[i].gameObject.SetActive(value: false);
				}
			}
			if (resetGroupButton != null)
			{
				resetGroupButton.onClick.RemoveAllListeners();
				resetGroupButton.onClick.AddListener(delegate
				{
					GameInput.ResetGroup(group);
					RefreshAll();
				});
			}
			ReportUnclaimed();
			RefreshConflicts();
			UILayout.RebuildFrom((rowContainer != null) ? rowContainer : base.transform);
			WarnIfNothingLaysOut(num);
		}

		private void WarnIfNothingLaysOut(int clonesUsed)
		{
			if (!warnedAboutLayout && clonesUsed != 0)
			{
				Transform transform = ((rowContainer != null) ? rowContainer : base.transform);
				if (!(transform.GetComponent<LayoutGroup>() != null))
				{
					warnedAboutLayout = true;
					Debug.LogWarning("[Keybindings] '" + transform.name + "' üzerinde LayoutGroup yok - klonlanan satırlar üst üste binecek. Bir Vertical Layout Group ekle.", transform);
				}
			}
		}

		private void RefreshAll()
		{
			foreach (KeybindingRowView item in live)
			{
				if (item != null)
				{
					item.Refresh();
				}
			}
			RefreshConflicts();
			UILayout.RebuildFrom((rowContainer != null) ? rowContainer : base.transform);
		}

		private void RefreshConflicts()
		{
			if (conflictLabel == null)
			{
				return;
			}
			List<string> list = new List<string>();
			foreach (GameInput.Command item in GameInput.Conflicts())
			{
				if (string.IsNullOrEmpty(group) || item.Group == group)
				{
					list.Add(item.Label);
				}
			}
			conflictLabel.gameObject.SetActive(list.Count > 0);
			if (list.Count > 0)
			{
				conflictLabel.text = Loc.Format("Input.KeyConflict", string.Join(", ", list));
			}
		}

		private KeybindingRowView FindPremade(string commandId)
		{
			foreach (PremadeRow premadeRow in premadeRows)
			{
				if (premadeRow.Row != null && premadeRow.CommandId == commandId)
				{
					return premadeRow.Row;
				}
			}
			return null;
		}

		private KeybindingRowView TakeClone(int index)
		{
			if (rowTemplate == null)
			{
				return null;
			}
			while (clones.Count <= index)
			{
				Transform parent = ((rowContainer != null) ? rowContainer : base.transform);
				KeybindingRowView keybindingRowView = UnityEngine.Object.Instantiate(rowTemplate, parent);
				keybindingRowView.name = rowTemplate.name + " (" + clones.Count + ")";
				clones.Add(keybindingRowView);
			}
			return clones[index];
		}

		private void ReportUnclaimed()
		{
			foreach (PremadeRow premadeRow in premadeRows)
			{
				if (!(premadeRow.Row == null) && !GameInput.TryFind(premadeRow.CommandId, out var _))
				{
					Debug.LogWarning("[Keybindings] '" + premadeRow.CommandId + "' diye bir komut yok - elle yerleştirilmiş satır boş kalacak. Konsolda 'binds' ile gecerli listeyi gorebilirsin.", premadeRow.Row);
				}
			}
		}
	}
}
