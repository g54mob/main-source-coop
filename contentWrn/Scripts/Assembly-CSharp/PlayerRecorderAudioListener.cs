using System.Collections;
using Zorro.Recorder;

public class PlayerRecorderAudioListener : RecorderAudioListener
{
	private VoiceChatModeSetting voiceChatModeSettings;

	private IEnumerator Start()
	{
		yield return 2;
		voiceChatModeSettings = GameHandler.Instance.SettingsHandler.GetSetting<VoiceChatModeSetting>();
	}

	public override bool CanRecordMic()
	{
		Player localPlayer = Player.localPlayer;
		bool result = false;
		if (voiceChatModeSettings != null)
		{
			result = voiceChatModeSettings.CanTalk();
		}
		if (localPlayer != null && !localPlayer.data.dead)
		{
			return result;
		}
		return false;
	}
}
