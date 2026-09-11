using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PluginCell : MonoBehaviour
{
	public TextMeshProUGUI pluginName;

	public TextMeshProUGUI directory;

	public GameObject selectedIndicator;

	private bool m_selected;

	private MainMenuModManagerPage m_page;

	private ModManagerPlugin m_plugin;

	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(Clicked);
	}

	private void Clicked()
	{
		m_page.Select(this);
	}

	public void Setup(ModManagerPlugin plugin, MainMenuModManagerPage page)
	{
		m_plugin = plugin;
		pluginName.text = plugin.DisplayName;
		m_page = page;
		directory.text = plugin.Version;
		List<string> list = new List<string>();
		if (plugin.PluginLocal != null)
		{
			list.Add("Local");
		}
		if (plugin.PluginPublished != null)
		{
			list.Add("Published");
		}
		if (plugin.PluginSubscribed != null)
		{
			list.Add("Workshop");
		}
		if (plugin.PluginExternal != null)
		{
			list.Add("External");
		}
		TextMeshProUGUI textMeshProUGUI = directory;
		textMeshProUGUI.text = textMeshProUGUI.text + "    " + string.Join(" / ", list);
		if (plugin.PluginLocal != null)
		{
			TextMeshProUGUI textMeshProUGUI2 = directory;
			textMeshProUGUI2.text = textMeshProUGUI2.text + "    " + plugin.PluginLocal.directory;
		}
	}

	public void Select()
	{
		m_selected = true;
		selectedIndicator.SetActive(value: true);
	}

	public void Deselect()
	{
		m_selected = false;
		selectedIndicator.SetActive(value: false);
	}

	public ModManagerPlugin GetPlugin()
	{
		return m_plugin;
	}
}
