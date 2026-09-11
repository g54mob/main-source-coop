using Photon.Pun;
using UnityEngine;

public class UniversalBotSounds : MonoBehaviour
{
	public SFX_Instance alertSound;

	private Bot bot;

	private PhotonView view;

	private void Awake()
	{
		view = GetComponent<PhotonView>();
		bot = base.transform.root.GetComponentInChildren<Bot>();
	}

	public void PlayAlertSound(float spookAmount)
	{
		SpookyMusicRelay.instance.AddDanger(spookAmount, bot.jumpScareLevel);
	}
}
