using UnityEngine;

namespace Mirror.Examples.CharacterSelection
{
	[AddComponentMenu("")]
	public class NetworkManagerCharacterSelection : NetworkManager
	{
		public struct CreateCharacterMessage : NetworkMessage
		{
			public string playerName;

			public int characterNumber;

			public Color characterColour;
		}

		public struct ReplaceCharacterMessage : NetworkMessage
		{
			public CreateCharacterMessage createCharacterMessage;
		}

		public bool SpawnAsCharacter = true;

		private CharacterData characterData;

		public new static NetworkManagerCharacterSelection singleton => (NetworkManagerCharacterSelection)NetworkManager.singleton;

		public override void Awake()
		{
			characterData = CharacterData.characterDataSingleton;
			if (characterData == null)
			{
				Debug.Log("Add CharacterData prefab singleton into the scene.");
			}
			else
			{
				base.Awake();
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			NetworkServer.RegisterHandler<CreateCharacterMessage>(OnCreateCharacter);
			NetworkServer.RegisterHandler<ReplaceCharacterMessage>(OnReplaceCharacterMessage);
		}

		public override void OnClientConnect()
		{
			base.OnClientConnect();
			if (SpawnAsCharacter)
			{
				NetworkClient.Send(new CreateCharacterMessage
				{
					playerName = StaticVariables.playerName,
					characterNumber = StaticVariables.characterNumber,
					characterColour = StaticVariables.characterColour
				});
			}
		}

		private void OnCreateCharacter(NetworkConnectionToClient conn, CreateCharacterMessage message)
		{
			Transform startPosition = GetStartPosition();
			if (message.playerName == "")
			{
				Debug.Log("OnCreateCharacter name invalid or not set, use random.");
				message.playerName = "Player: " + Random.Range(100, 1000);
			}
			if (message.characterNumber <= 0 || message.characterNumber >= characterData.characterPrefabs.Length)
			{
				Debug.Log("OnCreateCharacter prefab Invalid or not set, use random.");
				message.characterNumber = Random.Range(1, characterData.characterPrefabs.Length);
			}
			if (message.characterColour == new Color(0f, 0f, 0f, 0f))
			{
				Debug.Log("OnCreateCharacter colour invalid or not set, use random.");
				message.characterColour = Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 1f);
			}
			GameObject gameObject = ((startPosition != null) ? Object.Instantiate(characterData.characterPrefabs[message.characterNumber], startPosition.position, startPosition.rotation) : Object.Instantiate(characterData.characterPrefabs[message.characterNumber]));
			CharacterSelection component = gameObject.GetComponent<CharacterSelection>();
			component.NetworkplayerName = message.playerName;
			component.NetworkcharacterNumber = message.characterNumber;
			component.NetworkcharacterColour = message.characterColour;
			NetworkServer.AddPlayerForConnection(conn, gameObject);
		}

		private void OnReplaceCharacterMessage(NetworkConnectionToClient conn, ReplaceCharacterMessage message)
		{
			GameObject gameObject = conn.identity.gameObject;
			GameObject gameObject2 = Object.Instantiate(characterData.characterPrefabs[message.createCharacterMessage.characterNumber], gameObject.transform.position, gameObject.transform.rotation);
			NetworkServer.ReplacePlayerForConnection(conn, gameObject2, ReplacePlayerOptions.KeepActive);
			CharacterSelection component = gameObject2.GetComponent<CharacterSelection>();
			component.NetworkplayerName = message.createCharacterMessage.playerName;
			component.NetworkcharacterNumber = message.createCharacterMessage.characterNumber;
			component.NetworkcharacterColour = message.createCharacterMessage.characterColour;
			Object.Destroy(gameObject, 0.1f);
		}

		public void ReplaceCharacter(ReplaceCharacterMessage message)
		{
			NetworkClient.Send(message);
		}
	}
}
