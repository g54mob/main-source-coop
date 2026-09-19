using System.Collections;
using Mirror;
using UnityEngine;

public class CharacterSkinHandler : MonoBehaviour
{
	public static CharacterSkinHandler instance;

	[SerializeField]
	private GameObject characterSkinPrefab;

	[SerializeField]
	private Transform[] spawnPositions;

	public CharacterSkinElement[] clientsCharacters;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		clientsCharacters = new CharacterSkinElement[NetworkManager.singleton.maxConnections];
		StartCoroutine(WaitTillSteamInitialized());
	}

	private IEnumerator WaitTillSteamInitialized()
	{
		while (!SteamManager.Initialized)
		{
			yield return new WaitForEndOfFrame();
		}
		SpawnCharacterMesh(null);
	}

	public void SpawnCharacterMesh(MyClient client)
	{
		int nextPlatformIndex = GetNextPlatformIndex(client);
		if ((bool)client && client.isLocalPlayer)
		{
			client.characterInstance = clientsCharacters[0];
			client.characterInstance.Initialize(client, client.IsReady);
			return;
		}
		clientsCharacters[nextPlatformIndex] = Object.Instantiate(characterSkinPrefab, spawnPositions[nextPlatformIndex]).GetComponent<CharacterSkinElement>();
		clientsCharacters[nextPlatformIndex].Initialize(client, (bool)client && client.IsReady);
		if ((bool)client)
		{
			client.characterInstance = clientsCharacters[nextPlatformIndex];
		}
	}

	public int GetNextPlatformIndex(MyClient client)
	{
		if (client == null || client.isLocalPlayer)
		{
			return 0;
		}
		for (int i = 0; i < clientsCharacters.Length; i++)
		{
			if (clientsCharacters[i] == null)
			{
				return i;
			}
		}
		return 0;
	}

	public void DestroyCharacterMesh(MyClient client)
	{
		CharacterSkinElement[] array = clientsCharacters;
		foreach (CharacterSkinElement characterSkinElement in array)
		{
			if (!(characterSkinElement == null) && characterSkinElement.client == client)
			{
				if (characterSkinElement == clientsCharacters[0])
				{
					clientsCharacters[0].Initialize(client, _isReady: false);
					MainMenu.instance.UpdateReadyButton(value: false);
				}
				else
				{
					Object.Destroy(characterSkinElement.gameObject);
				}
				break;
			}
		}
	}
}
