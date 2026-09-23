using System;
using System.Collections.Generic;
using Mimicraft.Customization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mimicraft.UI
{
	public class CharacterPartListView : MonoBehaviour
	{
		[Serializable]
		public class PremadeRow
		{
			[Tooltip("Bu satirin temsil ettigi parcanin Part Id'si - CharacterPartDefinition'daki degerin AYNISI. Yanlis yazilirsa satir gizlenir ve konsola dogru kimlikler yazilir.")]
			public string PartId;

			[Tooltip("Satirin kendisi - uzerinde CharacterPartRowView olan obje.")]
			public CharacterPartRowView RowView;
		}

		[Tooltip("Satırların altına ekleneceği obje - genelde Vertical Layout Group taşıyan bir içerik objesi. Boş bırakılırsa bu objenin kendisi kullanılır.")]
		[SerializeField]
		private Transform rowContainer;

		[Tooltip("Bir satırın prefab'ı - üzerinde CharacterPartRowView olmalı. Görünüşü tamamen senin; burası sadece kopyalayıp dolduruyor.")]
		[SerializeField]
		private CharacterPartRowView rowTemplate;

		[Tooltip("Hiç parça yokken açılacak obje - 'karakter kurulmadı' yazısı gibi. İsteğe bağlı.")]
		[SerializeField]
		private GameObject emptyState;

		[Tooltip("Elle yerlestirdigin satirlar. Her biri temsil ettigi parcanin Part Id'sini soyler - listedeki SIRASI degil, o kimlik baglar. Kimligi hicbir parcaya uymayan satir gizlenir ve konsola rig'in gercek kimlikleri yazilir.")]
		[FormerlySerializedAs("PremadeRows")]
		[SerializeField]
		private List<PremadeRow> premadeRows = new List<PremadeRow>();

		private readonly List<CharacterPartRowView> spawnedRows = new List<CharacterPartRowView>();

		private CharacterAssembler character;

		private Action<string> onFocus;

		private int unclaimedPartIndex = -1;

		private bool warned;

		public void Bind(CharacterAssembler character, Action<string> onFocus)
		{
			this.character = character;
			this.onFocus = onFocus;
			Refresh();
		}

		private void Awake()
		{
			if (rowTemplate != null)
			{
				rowTemplate.gameObject.SetActive(value: false);
			}
		}

		private void OnEnable()
		{
			Refresh();
		}

		public void Refresh()
		{
			IReadOnlyList<CharacterAssembler.BuiltPart> readOnlyList = ((character != null) ? character.BuiltParts : null);
			int num = readOnlyList?.Count ?? 0;
			if (emptyState != null)
			{
				emptyState.SetActive(num == 0);
			}
			foreach (PremadeRow premadeRow in premadeRows)
			{
				if (premadeRow != null && premadeRow.RowView != null)
				{
					premadeRow.RowView.gameObject.SetActive(value: false);
				}
			}
			unclaimedPartIndex = -1;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				CharacterPartDefinition definition = readOnlyList[i].Definition;
				if (definition == null)
				{
					continue;
				}
				CharacterPartRowView characterPartRowView = FindPremade(definition.PartId);
				if (characterPartRowView == null)
				{
					characterPartRowView = TakeClone(num2++, i);
				}
				if (!(characterPartRowView == null))
				{
					if (!characterPartRowView.gameObject.activeSelf)
					{
						characterPartRowView.gameObject.SetActive(value: true);
					}
					characterPartRowView.Bind(definition.PartId, definition.DisplayName, character.IsPartVisible(definition.PartId), onFocus, ToggleVisibility);
				}
			}
			for (int j = num2; j < spawnedRows.Count; j++)
			{
				if (spawnedRows[j] != null && spawnedRows[j].gameObject.activeSelf)
				{
					spawnedRows[j].gameObject.SetActive(value: false);
				}
			}
			ReportUnclaimed(readOnlyList, num);
		}

		private CharacterPartRowView FindPremade(string partId)
		{
			if (string.IsNullOrWhiteSpace(partId))
			{
				return null;
			}
			foreach (PremadeRow premadeRow in premadeRows)
			{
				if (premadeRow != null && premadeRow.RowView != null && string.Equals(premadeRow.PartId?.Trim(), partId, StringComparison.Ordinal))
				{
					return premadeRow.RowView;
				}
			}
			return null;
		}

		private CharacterPartRowView TakeClone(int index, int partIndex)
		{
			if (index < spawnedRows.Count)
			{
				return spawnedRows[index];
			}
			if (rowTemplate == null)
			{
				unclaimedPartIndex = partIndex;
				return null;
			}
			Transform parent = ((rowContainer != null) ? rowContainer : base.transform);
			CharacterPartRowView characterPartRowView = UnityEngine.Object.Instantiate(rowTemplate, parent);
			spawnedRows.Add(characterPartRowView);
			return characterPartRowView;
		}

		private void ReportUnclaimed(IReadOnlyList<CharacterAssembler.BuiltPart> parts, int count)
		{
			if (warned || count == 0)
			{
				return;
			}
			List<string> list = new List<string>();
			foreach (PremadeRow premadeRow in premadeRows)
			{
				if (premadeRow != null && !(premadeRow.RowView == null) && (string.IsNullOrWhiteSpace(premadeRow.PartId) || FindPart(parts, count, premadeRow.PartId) == null))
				{
					list.Add(string.IsNullOrWhiteSpace(premadeRow.PartId) ? "(bos)" : premadeRow.PartId);
				}
			}
			if (list.Count == 0 && unclaimedPartIndex < 0)
			{
				return;
			}
			warned = true;
			List<string> list2 = new List<string>(count);
			for (int i = 0; i < count; i++)
			{
				if (parts[i].Definition != null)
				{
					list2.Add(parts[i].Definition.PartId);
				}
			}
			if (list.Count > 0)
			{
				Debug.LogWarning("[CharacterPartListView] Su satirlarin Part Id'si hicbir parcaya uymuyor: " + string.Join(", ", list) + ". Rig'deki kimlikler: " + string.Join(", ", list2), this);
			}
			if (unclaimedPartIndex >= 0 && unclaimedPartIndex < count && parts[unclaimedPartIndex].Definition != null)
			{
				Debug.LogWarning("[CharacterPartListView] '" + parts[unclaimedPartIndex].Definition.PartId + "' parcasi icin ne elle konmus bir satir var ne de Row Template - o parca listede hic gorunmeyecek.", this);
			}
		}

		private static CharacterPartDefinition FindPart(IReadOnlyList<CharacterAssembler.BuiltPart> parts, int count, string partId)
		{
			for (int i = 0; i < count; i++)
			{
				if (parts[i].Definition != null && string.Equals(parts[i].Definition.PartId, partId.Trim(), StringComparison.Ordinal))
				{
					return parts[i].Definition;
				}
			}
			return null;
		}

		private void ToggleVisibility(string partId)
		{
			if (!(character == null))
			{
				character.SetPartVisible(partId, !character.IsPartVisible(partId));
				Refresh();
			}
		}
	}
}
