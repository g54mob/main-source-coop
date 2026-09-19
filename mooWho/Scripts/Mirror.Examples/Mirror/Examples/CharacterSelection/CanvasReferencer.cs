using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mirror.Examples.CharacterSelection
{
	public class CanvasReferencer : MonoBehaviour
	{
		public Button buttonExit;

		public Button buttonNextCharacter;

		public Button buttonGo;

		public Button buttonColour;

		public Button buttonColourReset;

		public Text textTitle;

		public Text textHealth;

		public Text textSpeed;

		public Text textAttack;

		public Text textAbilities;

		public InputField inputFieldPlayerName;

		public Transform podiumPosition;

		private int currentlySelectedCharacter = 1;

		private CharacterData characterData;

		private GameObject currentInstantiatedCharacter;

		private CharacterSelection characterSelection;

		public SceneReferencer sceneReferencer;

		public Camera cameraObj;

		private void Start()
		{
			characterData = CharacterData.characterDataSingleton;
			if (characterData == null)
			{
				Debug.Log("Add CharacterData prefab singleton into the scene.");
				return;
			}
			buttonExit.onClick.AddListener(ButtonExit);
			buttonNextCharacter.onClick.AddListener(ButtonNextCharacter);
			buttonGo.onClick.AddListener(ButtonGo);
			buttonColour.onClick.AddListener(ButtonColour);
			buttonColourReset.onClick.AddListener(ButtonColourReset);
			inputFieldPlayerName.onValueChanged.AddListener(delegate
			{
				InputFieldChangedPlayerName();
			});
			LoadData();
			SetupCharacters();
		}

		public void ButtonExit()
		{
			if ((bool)sceneReferencer)
			{
				sceneReferencer.CloseCharacterSelection();
			}
		}

		public void ButtonGo()
		{
			if ((bool)sceneReferencer && NetworkClient.active)
			{
				NetworkManagerCharacterSelection.CreateCharacterMessage createCharacterMessage = new NetworkManagerCharacterSelection.CreateCharacterMessage
				{
					playerName = StaticVariables.playerName,
					characterNumber = StaticVariables.characterNumber,
					characterColour = StaticVariables.characterColour
				};
				NetworkManagerCharacterSelection.ReplaceCharacterMessage message = new NetworkManagerCharacterSelection.ReplaceCharacterMessage
				{
					createCharacterMessage = createCharacterMessage
				};
				NetworkManagerCharacterSelection.singleton.ReplaceCharacter(message);
				sceneReferencer.CloseCharacterSelection();
			}
			else
			{
				SceneManager.LoadScene("MirrorCharacterSelection");
			}
		}

		public void ButtonNextCharacter()
		{
			currentlySelectedCharacter++;
			if (currentlySelectedCharacter >= characterData.characterPrefabs.Length)
			{
				currentlySelectedCharacter = 1;
			}
			SetupCharacters();
			StaticVariables.characterNumber = currentlySelectedCharacter;
		}

		public void ButtonColour()
		{
			StaticVariables.characterColour = Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 1f);
			SetupCharacterColours();
		}

		public void ButtonColourReset()
		{
			StaticVariables.characterColour = new Color(0f, 0f, 0f, 0f);
			SetupCharacters();
		}

		private void SetupCharacters()
		{
			textTitle.text = characterData.characterTitles[currentlySelectedCharacter] ?? "";
			textHealth.text = "Health: " + characterData.characterHealths[currentlySelectedCharacter];
			textSpeed.text = "Speed: " + characterData.characterSpeeds[currentlySelectedCharacter];
			textAttack.text = "Attack: " + characterData.characterAttack[currentlySelectedCharacter];
			textAbilities.text = "Abilities:\n" + characterData.characterAbilities[currentlySelectedCharacter];
			if ((bool)currentInstantiatedCharacter)
			{
				Object.Destroy(currentInstantiatedCharacter);
			}
			currentInstantiatedCharacter = Object.Instantiate(characterData.characterPrefabs[currentlySelectedCharacter]);
			currentInstantiatedCharacter.transform.position = podiumPosition.position;
			currentInstantiatedCharacter.transform.rotation = podiumPosition.rotation;
			characterSelection = currentInstantiatedCharacter.GetComponent<CharacterSelection>();
			currentInstantiatedCharacter.transform.SetParent(base.transform.root);
			SetupCharacterColours();
			SetupPlayerName();
			if ((bool)cameraObj)
			{
				characterSelection.floatingInfo.forward = cameraObj.transform.forward;
			}
		}

		public void SetupCharacterColours()
		{
			if (StaticVariables.characterColour != new Color(0f, 0f, 0f, 0f))
			{
				characterSelection.NetworkcharacterColour = StaticVariables.characterColour;
				characterSelection.AssignColours();
			}
		}

		public void InputFieldChangedPlayerName()
		{
			StaticVariables.playerName = inputFieldPlayerName.text;
			SetupPlayerName();
		}

		public void SetupPlayerName()
		{
			if ((bool)characterSelection)
			{
				characterSelection.NetworkplayerName = StaticVariables.playerName;
				characterSelection.AssignName();
			}
		}

		public void LoadData()
		{
			if (StaticVariables.playerName != "")
			{
				if ((bool)inputFieldPlayerName)
				{
					inputFieldPlayerName.text = StaticVariables.playerName;
				}
			}
			else
			{
				StaticVariables.playerName = "Player Name";
			}
			if (StaticVariables.characterNumber > 0 && StaticVariables.characterNumber < characterData.characterPrefabs.Length)
			{
				currentlySelectedCharacter = StaticVariables.characterNumber;
			}
			else
			{
				StaticVariables.characterNumber = currentlySelectedCharacter;
			}
		}
	}
}
