using UnityEngine;

public class GameSettingsButtonGate : MonoBehaviour
{
	[Tooltip("Boş bırakılırsa Awake'te bu objenin altında isimle (\"GameSettingsButton\") aranır.")]
	public GameObject GameSettingsButton;

	private void Awake()
	{
		if (GameSettingsButton == null)
		{
			Transform transform = base.transform.Find("GameSettingsButton");
			GameSettingsButton = ((transform != null) ? transform.gameObject : null);
		}
	}

	private void Update()
	{
		if (!(GameSettingsButton == null))
		{
			bool num = LobbyManager.Instance != null;
			bool flag = MyNetworkManager.Singleton != null && MyNetworkManager.Singleton.IsInGameScene;
			bool flag2 = num || flag;
			if (GameSettingsButton.activeSelf != flag2)
			{
				GameSettingsButton.SetActive(flag2);
			}
		}
	}
}
