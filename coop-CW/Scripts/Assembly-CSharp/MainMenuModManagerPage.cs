using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zorro.ControllerSupport;
using Zorro.Core;
using Zorro.UI;

public class MainMenuModManagerPage : MainMenuPage, IHaveParentPage, INavigationPage
{
	public SelectedPluginUI selectedPluginUI;

	public Button backButton;

	public Transform pluginListParent;

	public GameObject pluginCellPrefab;

	private List<PluginCell> m_cells = new List<PluginCell>();

	private PluginCell m_currentSelectedCell;

	private void Awake()
	{
		backButton.onClick.AddListener(BackButtonClicked);
	}

	public override void OnPageEnter()
	{
		base.OnPageEnter();
		DrawMods();
	}

	public void DrawMods()
	{
		pluginListParent.ClearChildren();
		m_cells.Clear();
		List<Plugin> list = new List<Plugin>();
		list.AddRange(PluginHandler.LocalPlugins);
		list.AddRange(PluginHandler.SubscribedPlugins);
		list.AddRange(PluginHandler.ExternalPlugins);
		list.AddRange(PluginHandler.PublishedPlugins);
		List<ModManagerPlugin> uniquePlugins = new List<ModManagerPlugin>();
		foreach (Plugin item in list)
		{
			Plugin plugin = item;
			if (AlreadyExists(out var existingPlugin))
			{
				existingPlugin.Update(plugin);
			}
			else
			{
				uniquePlugins.Add(new ModManagerPlugin(plugin));
			}
			bool AlreadyExists(out ModManagerPlugin reference)
			{
				foreach (ModManagerPlugin item2 in uniquePlugins)
				{
					if (Plugin.HasSameIdentity(item2.OriginalPlugin, plugin))
					{
						reference = item2;
						return true;
					}
				}
				reference = null;
				return false;
			}
		}
		foreach (ModManagerPlugin item3 in uniquePlugins)
		{
			PluginCell component = Object.Instantiate(pluginCellPrefab, pluginListParent).GetComponent<PluginCell>();
			component.Setup(item3, this);
			m_cells.Add(component);
		}
		if (m_cells.Count > 0)
		{
			Select(m_cells[0]);
		}
	}

	public void Select(PluginCell cell)
	{
		if (!(m_currentSelectedCell == cell))
		{
			if (m_currentSelectedCell != null)
			{
				m_currentSelectedCell.Deselect();
			}
			m_currentSelectedCell = cell;
			cell.Select();
			selectedPluginUI.DrawPlugin(cell.GetPlugin(), this);
		}
	}

	private void BackButtonClicked()
	{
		pageHandler.TransistionToPage<MainMenuMainPage>();
	}

	public (UIPage, PageTransistion) GetParentPage()
	{
		return (pageHandler.GetPage<MainMenuMainPage>(), new SetActivePageTransistion());
	}

	public GameObject GetFirstSelectedGameObject()
	{
		return backButton.gameObject;
	}

	public void SelectFromIdentity(Plugin plugin)
	{
		foreach (PluginCell cell in m_cells)
		{
			if (Plugin.HasSameIdentity(plugin, cell.GetPlugin().OriginalPlugin))
			{
				Select(cell);
				break;
			}
		}
	}
}
