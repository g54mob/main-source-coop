using System;
using System.Threading.Tasks;
using Steamworks;
using TMPro;
using UnityEngine;

public class PluginLoadingScreen : MonoBehaviour
{
	public TextMeshProUGUI m_text;

	private UGCUpdateHandle_t _handle;

	private Task _completeTask;

	public void Show(UGCUpdateHandle_t handle, Task completeTask)
	{
		base.gameObject.SetActive(value: true);
		_handle = handle;
		_completeTask = completeTask;
	}

	private void Update()
	{
		if (!_completeTask.IsCompleted)
		{
			ulong punBytesProcessed;
			ulong punBytesTotal;
			string text;
			string text2;
			switch (SteamUGC.GetItemUpdateProgress(_handle, out punBytesProcessed, out punBytesTotal))
			{
			case EItemUpdateStatus.k_EItemUpdateStatusPreparingConfig:
				text = "Preparing config";
				goto IL_0074;
			case EItemUpdateStatus.k_EItemUpdateStatusPreparingContent:
				text = "Preparing content";
				goto IL_0074;
			case EItemUpdateStatus.k_EItemUpdateStatusUploadingContent:
				text = "Uploading content";
				goto IL_0074;
			case EItemUpdateStatus.k_EItemUpdateStatusUploadingPreviewFile:
				text = "Uploading preview file";
				goto IL_0074;
			case EItemUpdateStatus.k_EItemUpdateStatusCommittingChanges:
				text = "Committing changes";
				goto IL_0074;
			default:
				throw new ArgumentOutOfRangeException();
			case EItemUpdateStatus.k_EItemUpdateStatusInvalid:
				{
					m_text.text = "Invalid status";
					break;
				}
				IL_0074:
				text2 = text;
				m_text.text = text2;
				break;
			}
		}
		else
		{
			m_text.text = "Done uploading";
		}
	}
}
