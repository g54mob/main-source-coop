using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Zorro.ControllerSupport;
using Zorro.Core;
using Zorro.Settings;

public class SettingsMenu : Singleton<SettingsMenu>
{
	public CW_TAB firstTab;

	public CW_TABS categoryTabs;

	public CW_TAB modsTab;

	public Transform m_settingsContainer;

	public GameObject m_settingsCell;

	public GameObject m_controllerLayout;

	private SettingCategory m_currentCategory;

	private List<SettingsCell> m_cells = new List<SettingsCell>();

	private void OnEnable()
	{
		categoryTabs.Select(firstTab);
		if (GameHandler.Instance.SettingsHandler.GetAllSettingsNonAlloc().Any((Setting s) => s is IExposedSetting exposedSetting && exposedSetting.GetSettingCategory() == SettingCategory.Mods))
		{
			modsTab.gameObject.SetActive(value: true);
		}
		LocalizationKeys.OnLanguageChanged += OnLanguageChanged;
	}

	private void OnDisable()
	{
		LocalizationKeys.OnLanguageChanged -= OnLanguageChanged;
	}

	private void OnLanguageChanged()
	{
		Show(m_currentCategory);
	}

	private void Show(SettingCategory category)
	{
		m_controllerLayout.SetActive(value: false);
		foreach (SettingsCell cell in m_cells)
		{
			Object.Destroy(cell.gameObject);
		}
		m_cells.Clear();
		SettingsHandler settingsHandler = GameHandler.Instance.SettingsHandler;
		List<Setting> settings = settingsHandler.GetSettings(category);
		m_currentCategory = category;
		for (int i = 0; i < settings.Count; i++)
		{
			Setting setting = settings[i];
			SettingsCell component = Object.Instantiate(m_settingsCell, m_settingsContainer).GetComponent<SettingsCell>();
			component.Setup(setting, settingsHandler);
			m_cells.Add(component);
		}
		if (InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad)
		{
			for (int j = 0; j < m_cells.Count; j++)
			{
				if (m_cells[j].gameObject.activeSelf)
				{
					GameObject selection = m_cells[j].GetSelection();
					if (selection != null)
					{
						EventSystem.current.SetSelectedGameObject(selection);
						break;
					}
				}
			}
		}
		if (category == SettingCategory.Controller)
		{
			m_controllerLayout.SetActive(value: true);
			m_controllerLayout.transform.SetAsLastSibling();
		}
	}

	public void SelectCategory(SettingCategory settingCategory)
	{
		Show(settingCategory);
	}
}
