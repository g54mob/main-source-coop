using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModlistEntryUI : MonoBehaviour
{
	private Plugin plugin;

	private PublishedFileId_t? publishedFileId;

	private ModManagerUI modManagerUI;

	public TMP_Text title;

	public Button webUIButton;

	public Button editButton;

	private void Awake()
	{
		webUIButton.onClick.AddListener(OnWebUIClick);
		editButton.onClick.AddListener(OnEditClick);
	}

	private void OnWebUIClick()
	{
		if (publishedFileId.HasValue)
		{
			Application.OpenURL("https://steamcommunity.com/sharedfiles/filedetails/?id=" + publishedFileId.Value.ToString());
		}
		else
		{
			Debug.LogWarning("webui button clicked when the button should be disabled: " + plugin.DisplayName);
		}
	}

	private void OnEditClick()
	{
		if (plugin is PluginLocal pluginLocal)
		{
			modManagerUI.EditMod(pluginLocal);
		}
		else
		{
			Debug.LogWarning("Edit plugin button clicked when the button should be disabled: " + plugin.DisplayName);
		}
	}

	public void SetInfo(ModManagerUI modManagerUI, Plugin plugin)
	{
		this.modManagerUI = modManagerUI;
		this.plugin = plugin;
		publishedFileId = plugin.DiscoverPublishedFileId(PluginHandler.AllPlugins);
		webUIButton.gameObject.SetActive(publishedFileId.HasValue);
		editButton.gameObject.SetActive(plugin.Publishable);
		title.text = plugin.DisplayName;
	}
}
