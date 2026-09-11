using UnityEngine;
using UnityEngine.UI;

public class EmoteUI : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	public Image progressImage;

	public Image emoteIconImage;

	private void Update()
	{
		Player localPlayer = Player.localPlayer;
		if ((bool)localPlayer)
		{
			if (localPlayer.refs.emotes.IsPlayingEmote)
			{
				Item latestEmotePlayed = localPlayer.refs.emotes.latestEmotePlayed;
				float emoteTime = localPlayer.data.emoteTime;
				float emoteLength = latestEmotePlayed.emoteInfo.emoteLength;
				progressImage.fillAmount = emoteTime / emoteLength;
				emoteIconImage.sprite = latestEmotePlayed.icon;
				canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, Time.deltaTime * 10f);
			}
			else
			{
				canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, Time.deltaTime * 10f);
			}
		}
	}
}
