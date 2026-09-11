using System;
using System.Collections;
using System.Threading.Tasks;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModManagerEditPanelUI : MonoBehaviour
{
	private PluginPublished _existingPlugin;

	private PluginLocal _plugin;

	public TMP_InputField title;

	public TMP_InputField description;

	public TMP_Text reasonCantPublish;

	public Button submitButton;

	private void Awake()
	{
		submitButton.onClick.AddListener(SubmitClicked);
		title.onValueChanged.AddListener(delegate
		{
			CheckPublishable();
		});
		description.onValueChanged.AddListener(delegate
		{
			CheckPublishable();
		});
		CheckPublishable();
	}

	private void CheckPublishable()
	{
		submitButton.interactable = CanPublish(out var reason);
		reasonCantPublish.text = reason ?? string.Empty;
	}

	public bool CanPublish(out string reason)
	{
		if (string.IsNullOrEmpty(title.text))
		{
			reason = "Missing title";
			return false;
		}
		if (string.IsNullOrEmpty(description.text))
		{
			reason = "Missing description";
			return false;
		}
		if (_plugin.GetPreviewImagePath() == null)
		{
			reason = "Missing preview.png in plugin directory";
			return false;
		}
		reason = null;
		return true;
	}

	public void EditMod(PluginLocal plugin)
	{
		_plugin = plugin;
		foreach (PluginPublished publishedPlugin in PluginHandler.PublishedPlugins)
		{
			if (Plugin.HasSameIdentity(publishedPlugin, plugin))
			{
				_existingPlugin = publishedPlugin;
				break;
			}
		}
		title.text = _existingPlugin?.steamUgcDetails.m_rgchTitle ?? _plugin.DisplayName;
		description.text = _existingPlugin?.steamUgcDetails.m_rgchDescription ?? "";
	}

	private async void SubmitClicked()
	{
		PublishedFileId_t publishedFileId;
		if (_existingPlugin != null)
		{
			publishedFileId = _existingPlugin.steamUgcDetails.m_nPublishedFileId;
		}
		else
		{
			PublishedFileId_t? publishedFileId_t = await CreateNewMod();
			if (!publishedFileId_t.HasValue)
			{
				return;
			}
			publishedFileId = publishedFileId_t.Value;
		}
		string previewImagePath = _plugin.GetPreviewImagePath();
		if (previewImagePath == null)
		{
			Modal.ShowError("Failed to publish", "Missing preview.png in plugin directory");
			return;
		}
		UGCUpdateHandle_t handle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), publishedFileId);
		if (!SteamUGC.SetItemTitle(handle, title.text))
		{
			Modal.ShowError("Failed to publish", "Failed to set item title");
			return;
		}
		if (!SteamUGC.SetItemDescription(handle, description.text))
		{
			Modal.ShowError("Failed to publish", "Failed to set item description");
			return;
		}
		if (!SteamUGC.SetItemMetadata(handle, _plugin.info.guid))
		{
			Modal.ShowError("Failed to publish", "Failed to set item metadata");
			return;
		}
		if (!SteamUGC.SetItemPreview(handle, previewImagePath))
		{
			Modal.ShowError("Failed to publish", "Failed to set item preview image");
			return;
		}
		if (!SteamUGC.SetItemContent(handle, _plugin.directory))
		{
			Modal.ShowError("Failed to publish", "Failed to set item content directory");
			return;
		}
		string pchChangeNote = "Upload from ingame, version " + _plugin.info.version;
		Task<SubmitItemUpdateResult_t> task = SteamUGC.SubmitItemUpdate(handle, pchChangeNote).ToAsync<SubmitItemUpdateResult_t>();
		StartCoroutine(UpdateProgressBar(handle, task));
		SubmitItemUpdateResult_t submitItemUpdateResult_t = await task;
		if (submitItemUpdateResult_t.m_eResult != EResult.k_EResultOK)
		{
			Modal.ShowError("Error", $"Could not update Steam Workshop item: {submitItemUpdateResult_t.m_eResult}");
		}
		if (submitItemUpdateResult_t.m_bUserNeedsToAcceptWorkshopLegalAgreement)
		{
			Modal.ShowError("You need to accept the Steam Workshop legal agreement", "You need to accept the Steam Workshop legal agreement");
		}
		SteamFriends.ActivateGameOverlayToWebPage($"steam://url/CommunityFilePage/{submitItemUpdateResult_t.m_nPublishedFileId}");
		Debug.Log($"Submit {publishedFileId} title {title.text} descr {description.text}");
	}

	private IEnumerator UpdateProgressBar(UGCUpdateHandle_t handle, Task completeTask)
	{
		while (!completeTask.IsCompleted)
		{
			ulong punBytesProcessed;
			ulong punBytesTotal;
			string text;
			string message;
			switch (SteamUGC.GetItemUpdateProgress(handle, out punBytesProcessed, out punBytesTotal))
			{
			case EItemUpdateStatus.k_EItemUpdateStatusPreparingConfig:
				text = "Preparing config";
				goto IL_008d;
			case EItemUpdateStatus.k_EItemUpdateStatusPreparingContent:
				text = "Preparing content";
				goto IL_008d;
			case EItemUpdateStatus.k_EItemUpdateStatusUploadingContent:
				text = "Uploading content";
				goto IL_008d;
			case EItemUpdateStatus.k_EItemUpdateStatusUploadingPreviewFile:
				text = "Uploading preview file";
				goto IL_008d;
			case EItemUpdateStatus.k_EItemUpdateStatusCommittingChanges:
				text = "Committing changes";
				goto IL_008d;
			default:
				throw new ArgumentOutOfRangeException();
			case EItemUpdateStatus.k_EItemUpdateStatusInvalid:
				break;
				IL_008d:
				message = text;
				UpdateProgressBarUI(message, punBytesProcessed, punBytesTotal);
				yield return null;
				continue;
			}
			break;
		}
		UpdateProgressBarUI(null, 0uL, 0uL);
	}

	private void UpdateProgressBarUI(string message, ulong bytesProcessed, ulong bytesTotal)
	{
		string text = ((message == null) ? "Done uploading" : $"Update progress bar: {message} -- {bytesProcessed}/{bytesTotal} ({(float)bytesProcessed / (float)bytesTotal * 100f:F2}%)");
		Debug.Log(text);
		reasonCantPublish.text = text;
	}

	private static async Task<PublishedFileId_t?> CreateNewMod()
	{
		CreateItemResult_t createItemResult_t = await SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeFirst).ToAsync<CreateItemResult_t>();
		if (createItemResult_t.m_eResult != EResult.k_EResultOK)
		{
			Modal.ShowError("Error", $"Could not create Steam Workshop item: {createItemResult_t.m_eResult}");
			return null;
		}
		return createItemResult_t.m_nPublishedFileId;
	}
}
