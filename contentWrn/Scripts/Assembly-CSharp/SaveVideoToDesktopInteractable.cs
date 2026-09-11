using Portningsbolaget.Platforms;
using UnityEngine;

public class SaveVideoToDesktopInteractable : Interactable
{
	private CameraRecording m_recording;

	private void Start()
	{
		hoverText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.SaveVideo);
	}

	public override bool IsValid(Player player)
	{
		return m_recording != null;
	}

	public override void Interact(Player player)
	{
		Debug.Log("Saving video to desktop...");
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.VideoSaved);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.VideoSavedAs);
		string localizedString3 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.VideoFailedSave);
		string localizedString4 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Ok);
		if (m_recording.SaveToDesktop(out var videoFileName))
		{
			Modal.Show(localizedString, localizedString2 + "  " + videoFileName, new ModalOption[1]
			{
				new ModalOption(localizedString4)
			});
			PlatformManager.UnlockAchievement(Achievements.ACH_SAVE_VIDEO);
		}
		else
		{
			Modal.Show(localizedString3, "", new ModalOption[1]
			{
				new ModalOption(localizedString4)
			});
		}
	}

	public void SetRecording(CameraRecording recording)
	{
		m_recording = recording;
	}
}
