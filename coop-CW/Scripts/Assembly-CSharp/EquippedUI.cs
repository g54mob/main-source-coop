using System.Collections.Generic;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using Zorro.ControllerSupport;

public class EquippedUI : MonoBehaviour
{
	public TextMeshProUGUI m_textPrefab;

	public TextMeshProUGUI m_itemNameText;

	private ItemDescriptor m_lastItemDescriptor;

	private InputScheme m_lastInputScheme;

	private List<(IHaveUIData, TextMeshProUGUI)> m_entries = new List<(IHaveUIData, TextMeshProUGUI)>();

	public void SetData(ItemDescriptor itemDescriptor)
	{
		if (itemDescriptor.data != m_lastItemDescriptor.data || itemDescriptor.item == null)
		{
			UpdateUI(itemDescriptor);
		}
		m_lastItemDescriptor = itemDescriptor;
	}

	private void UpdateUI(ItemDescriptor itemDescriptor)
	{
		foreach (var entry in m_entries)
		{
			Object.Destroy(entry.Item2.gameObject);
		}
		m_entries.Clear();
		bool flag = itemDescriptor.item != null;
		if (flag)
		{
			m_itemNameText.text = itemDescriptor.item.GetLocalizedDisplayName();
			foreach (ItemDataEntry dataEntry in itemDescriptor.data.m_dataEntries)
			{
				if (dataEntry is IHaveUIData uiData)
				{
					SpawnText(uiData);
				}
			}
			itemDescriptor.item.GetTootipData().ForEach(SpawnText);
			string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.DropItem);
			string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.ThrowItemToolTip);
			DropKeybindSetting setting = GameHandler.Instance.SettingsHandler.GetSetting<DropKeybindSetting>();
			SpawnText(new ItemKeyTooltip("{key} " + localizedString, setting, new List<ControllerGlyphs.GlyphType> { ControllerGlyphs.GlyphType.Drop }));
			SpawnText(new ItemKeyTooltip(localizedString2, setting, new List<ControllerGlyphs.GlyphType> { ControllerGlyphs.GlyphType.Drop }));
			foreach (var entry2 in m_entries)
			{
				entry2.Item1.UpdateLocale();
				string text = entry2.Item1.GetString();
				entry2.Item2.text = text;
			}
		}
		base.gameObject.SetActive(value: false);
		base.gameObject.SetActive(flag);
		void SpawnText(IHaveUIData item)
		{
			TextMeshProUGUI component = Object.Instantiate(m_textPrefab, m_textPrefab.transform.parent).GetComponent<TextMeshProUGUI>();
			component.gameObject.SetActive(value: true);
			m_entries.Add((item, component));
		}
	}

	private void LateUpdate()
	{
		if (Time.frameCount % 10 != 0)
		{
			return;
		}
		foreach (var entry in m_entries)
		{
			InputScheme currentUsedInputScheme = InputHandler.GetCurrentUsedInputScheme();
			if (currentUsedInputScheme != m_lastInputScheme)
			{
				m_lastInputScheme = currentUsedInputScheme;
				UpdateUI(m_lastItemDescriptor);
				break;
			}
			string text = entry.Item1.GetString();
			entry.Item2.text = text;
		}
	}

	private void OnEnable()
	{
		LocalizationKeys.OnLanguageChanged += OnSelectedLocaleChanged;
	}

	private void OnDisable()
	{
		LocalizationKeys.OnLanguageChanged -= OnSelectedLocaleChanged;
	}

	private void Start()
	{
		foreach (var entry in m_entries)
		{
			entry.Item1.UpdateLocale();
		}
	}

	private void OnSelectedLocaleChanged()
	{
		UpdateUI(m_lastItemDescriptor);
	}
}
