using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModManagerUI : MainMenuPage
{
	public GameObject modlist;

	public ModlistEntryUI modlistEntryPrefab;

	public ModlistEntryHeaderUI modlistEntryHeaderPrefab;

	public ModManagerEditPanelUI modManagerEditPanel;

	public Button backButton;

	private bool hasInit;

	private void Awake()
	{
		modManagerEditPanel.gameObject.SetActive(value: false);
		backButton.onClick.AddListener(delegate
		{
			pageHandler.TransistionToPage<MainMenuMainPage>();
		});
	}

	private void Update()
	{
		if (!hasInit && SteamManager.Initialized)
		{
			hasInit = true;
			RefreshList();
		}
	}

	public void EditMod(PluginLocal plugin)
	{
		modManagerEditPanel.gameObject.SetActive(value: true);
		modManagerEditPanel.EditMod(plugin);
	}

	private void RefreshList()
	{
		foreach (Transform item in modlist.transform)
		{
			Object.Destroy(item.gameObject);
		}
		AddList("Local plugins:", PluginHandler.LocalPlugins);
		AddList("Subscribed plugins:", PluginHandler.SubscribedPlugins);
		AddList("External plugins:", PluginHandler.ExternalPlugins);
		AddList("Published plugins:", PluginHandler.PublishedPlugins);
	}

	private void AddList(string headerName, IReadOnlyList<Plugin> plugins)
	{
		if (plugins.Count == 0)
		{
			return;
		}
		Object.Instantiate(modlistEntryHeaderPrefab, modlist.transform).text.text = headerName;
		foreach (Plugin plugin in plugins)
		{
			Object.Instantiate(modlistEntryPrefab, modlist.transform).SetInfo(this, plugin);
		}
	}
}
