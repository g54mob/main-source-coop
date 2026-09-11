using System.Threading.Tasks;
using JetBrains.Annotations;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zorro.Core;

public class SelectedPluginUI : MonoBehaviour
{
	public GameObject m_actionButtonPrefab;

	public Transform m_actionButtonParent;

	public PluginNameField m_pluginNameField;

	public PluginDescriptionField m_pluginDescriptionField;

	public PluginLoadingScreen m_loadingScreen;

	public PluginSteamworkshopInfo m_workshopInfo;

	public TextMeshProUGUI m_pluginName;

	public void DrawPlugin(ModManagerPlugin plugin, MainMenuModManagerPage page)
	{
		ClearScreen();
		m_pluginName.text = plugin.DisplayName;
		if (plugin.PluginPublished != null)
		{
			m_workshopInfo.Show(plugin.PluginPublished.steamUgcDetails.m_rgchDescription);
			SpawnActionButton("Open Workshop", delegate
			{
				OpenPublishedPlugin(plugin.PluginPublished.steamUgcDetails.m_nPublishedFileId);
			});
		}
		else if (plugin.PluginSubscribed != null)
		{
			string message = (plugin.PluginSubscribed.steamUgcDetails.HasValue ? plugin.PluginSubscribed.steamUgcDetails.Value.m_rgchDescription : "No description");
			m_workshopInfo.Show(message);
			SpawnActionButton("Open Workshop", delegate
			{
				OpenPublishedPlugin(plugin.PluginSubscribed.publishedFileId);
			});
		}
		if (plugin.PluginLocal == null)
		{
			return;
		}
		SpawnActionButton("Open Directory", delegate
		{
			Application.OpenURL("file://" + plugin.PluginLocal.directory);
		});
		if (plugin.PluginPublished == null)
		{
			SpawnActionButton("Publish to workshop", delegate
			{
				DrawPublishScreen(plugin, page);
			});
		}
		else
		{
			SpawnActionButton("Update Workshop", delegate
			{
				DrawPublishScreen(plugin, page);
			});
		}
	}

	public void DrawPublishScreen(ModManagerPlugin plugin, MainMenuModManagerPage page)
	{
		bool alreadyPublished = plugin.PluginPublished != null;
		ClearScreen();
		m_pluginName.text = (alreadyPublished ? "Update Plugin on" : "Publish to") + " " + plugin.DisplayName + " workshop";
		string defaultValue = "";
		if (alreadyPublished)
		{
			defaultValue = plugin.PluginPublished.steamUgcDetails.m_rgchDescription;
		}
		m_pluginNameField.Enable(plugin.DisplayName);
		m_pluginDescriptionField.Enable(defaultValue);
		SpawnActionButton("Cancel", delegate
		{
			DrawPlugin(plugin, page);
		});
		SpawnActionButton("Upload", TryPublish);
		static async Task<PublishedFileId_t?> CreateNewMod()
		{
			CreateItemResult_t createItemResult_t = await SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeFirst).ToAsync<CreateItemResult_t>();
			if (createItemResult_t.m_eResult != EResult.k_EResultOK)
			{
				Modal.ShowError("Error", $"Could not create Steam Workshop item: {createItemResult_t.m_eResult}");
				return null;
			}
			return createItemResult_t.m_nPublishedFileId;
		}
		[CanBeNull]
		async Task<bool> Publish(string pluginName, string pluginDescription)
		{
			PluginPublished existingPublishedPlugin = plugin.PluginPublished;
			PluginLocal localPlugin = plugin.PluginLocal;
			PublishedFileId_t nPublishedFileID;
			if (existingPublishedPlugin != null)
			{
				nPublishedFileID = existingPublishedPlugin.steamUgcDetails.m_nPublishedFileId;
			}
			else
			{
				PublishedFileId_t? publishedFileId_t = await CreateNewMod();
				if (!publishedFileId_t.HasValue)
				{
					return false;
				}
				nPublishedFileID = publishedFileId_t.Value;
			}
			string previewImagePath = localPlugin.GetPreviewImagePath();
			if (previewImagePath == null)
			{
				Modal.ShowError("Failed to publish", "Missing preview.png in plugin directory");
				return false;
			}
			UGCUpdateHandle_t handle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), nPublishedFileID);
			if (!SteamUGC.SetItemTitle(handle, pluginName))
			{
				Modal.ShowError("Failed to publish", "Failed to set item title");
				return false;
			}
			if (!SteamUGC.SetItemDescription(handle, pluginDescription))
			{
				Modal.ShowError("Failed to publish", "Failed to set item description");
				return false;
			}
			if (!SteamUGC.SetItemMetadata(handle, localPlugin.info.guid))
			{
				Modal.ShowError("Failed to publish", "Failed to set item metadata");
				return false;
			}
			if (!SteamUGC.SetItemPreview(handle, previewImagePath))
			{
				Modal.ShowError("Failed to publish", "Failed to set item preview image");
				return false;
			}
			if (!SteamUGC.SetItemContent(handle, localPlugin.directory))
			{
				Modal.ShowError("Failed to publish", "Failed to set item content directory");
				return false;
			}
			bool num = existingPublishedPlugin != null;
			string pchChangeNote = "Initial upload, version " + localPlugin.info.version;
			if (num)
			{
				pchChangeNote = "Update to version " + localPlugin.info.version;
			}
			Task<SubmitItemUpdateResult_t> task = SteamUGC.SubmitItemUpdate(handle, pchChangeNote).ToAsync<SubmitItemUpdateResult_t>();
			ShowLoadingScreen(handle, task);
			SubmitItemUpdateResult_t submitItemUpdateResult_t = await task;
			if (submitItemUpdateResult_t.m_eResult != EResult.k_EResultOK)
			{
				Modal.ShowError("Error", $"Could not update Steam Workshop item: {submitItemUpdateResult_t.m_eResult}");
			}
			if (submitItemUpdateResult_t.m_bUserNeedsToAcceptWorkshopLegalAgreement)
			{
				Modal.ShowError("Plugin not fully published", "You need to accept the Steam Workshop legal agreement to publish your plugin. Please check the the Steam Workshop page for your plugin that should have opened.");
			}
			string[] obj = new string[6] { "Published plugin with id ", null, null, null, null, null };
			PublishedFileId_t nPublishedFileId = submitItemUpdateResult_t.m_nPublishedFileId;
			obj[1] = nPublishedFileId.ToString();
			obj[2] = " with name: ";
			obj[3] = pluginName;
			obj[4] = " and description: ";
			obj[5] = pluginDescription;
			Debug.Log(string.Concat(obj));
			OpenPublishedPlugin(submitItemUpdateResult_t.m_nPublishedFileId);
			return true;
		}
		async void TryPublish()
		{
			string value = m_pluginNameField.GetValue();
			string value2 = m_pluginDescriptionField.GetValue();
			if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
			{
				Modal.Show("Plugin Needs Name", "Plugin needs a name to be published", new ModalOption[1]
				{
					new ModalOption("Ok")
				});
			}
			else if (string.IsNullOrEmpty(value2) || string.IsNullOrWhiteSpace(value2))
			{
				Modal.Show("Plugin Needs Description", "Plugin needs a description to be published", new ModalOption[1]
				{
					new ModalOption("Ok")
				});
			}
			else
			{
				if (alreadyPublished)
				{
					Debug.Log("Updating " + value + " with description " + value2);
				}
				else
				{
					Debug.Log("Publishing " + value + " with description " + value2);
				}
				if (await Publish(value, value2))
				{
					await PluginHandler.UpdatePublishedPlugins();
					page.DrawMods();
					page.SelectFromIdentity(plugin.PluginLocal);
				}
				else
				{
					DrawPublishScreen(plugin, page);
				}
			}
		}
	}

	private static void OpenPublishedPlugin(PublishedFileId_t fileID)
	{
		PublishedFileId_t publishedFileId_t = fileID;
		Application.OpenURL(" steam://url/CommunityFilePage/" + publishedFileId_t.ToString());
		publishedFileId_t = fileID;
		SteamFriends.ActivateGameOverlayToWebPage("https://steamcommunity.com/sharedfiles/filedetails/?id=" + publishedFileId_t.ToString());
	}

	private void ShowLoadingScreen(UGCUpdateHandle_t handle, Task completeTask)
	{
		ClearScreen();
		m_loadingScreen.Show(handle, completeTask);
	}

	private void ClearScreen()
	{
		m_actionButtonParent.ClearChildren();
		m_pluginNameField.gameObject.SetActive(value: false);
		m_pluginDescriptionField.gameObject.SetActive(value: false);
		m_loadingScreen.gameObject.SetActive(value: false);
		m_workshopInfo.gameObject.SetActive(value: false);
	}

	private void SpawnActionButton(string text, UnityAction action)
	{
		GameObject gameObject = Object.Instantiate(m_actionButtonPrefab, m_actionButtonParent);
		gameObject.gameObject.SetActive(value: true);
		gameObject.GetComponentInChildren<TextMeshProUGUI>().text = text;
		if (action != null)
		{
			gameObject.GetComponent<Button>().onClick.AddListener(action);
		}
	}
}
