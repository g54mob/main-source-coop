using System.Collections;
using Mirror;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
	public static LobbyController instance;

	private void Awake()
	{
		instance = this;
	}

	public void StartGameWithParty()
	{
		if (AllPlayersReady())
		{
			StartGame();
		}
	}

	public void StartGameSolo()
	{
		StartCoroutine(StartSinglePlayer());
	}

	private IEnumerator StartSinglePlayer()
	{
		NetworkManager.singleton.StartHost();
		while (NetworkClient.localPlayer == null)
		{
			yield return new WaitForEndOfFrame();
		}
		((MyNetworkManager)NetworkManager.singleton).SetMultiplayer(value: false);
		StartGame();
	}

	private void StartGame()
	{
		NetworkManager.singleton.ServerChangeScene("Stage_1");
	}

	private bool AllPlayersReady()
	{
		foreach (MyClient allClient in ((MyNetworkManager)NetworkManager.singleton).allClients)
		{
			if (!allClient.IsReady)
			{
				return false;
			}
		}
		return true;
	}
}
