using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.CharacterSelection
{
	public class SceneReferencer : MonoBehaviour
	{
		public Button buttonCharacterSelection;

		private CharacterData characterData;

		public GameObject characterSelectionObject;

		public GameObject sceneObjects;

		public GameObject cameraObject;

		private void Start()
		{
			characterData = CharacterData.characterDataSingleton;
			if (characterData == null)
			{
				Debug.Log("Add CharacterData prefab singleton into the scene.");
			}
			else
			{
				buttonCharacterSelection.onClick.AddListener(ButtonCharacterSelection);
			}
		}

		public void ButtonCharacterSelection()
		{
			cameraObject.SetActive(value: false);
			sceneObjects.SetActive(value: false);
			characterSelectionObject.SetActive(value: true);
			GetComponent<Canvas>().enabled = false;
		}

		public void CloseCharacterSelection()
		{
			cameraObject.SetActive(value: true);
			characterSelectionObject.SetActive(value: false);
			sceneObjects.SetActive(value: true);
			GetComponent<Canvas>().enabled = true;
		}
	}
}
