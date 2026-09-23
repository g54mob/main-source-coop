using UnityEngine;

namespace Mimicraft.Customization
{
	public class CharacterMenuPreview : MonoBehaviour
	{
		[Tooltip("Menüdeki karakter modeli. Boş bırakılırsa bu objede ve altında aranır.")]
		[SerializeField]
		private CharacterAssembler character;

		private void Awake()
		{
			if (character == null)
			{
				character = GetComponentInChildren<CharacterAssembler>(includeInactive: true);
			}
		}

		private void OnEnable()
		{
			Refresh();
		}

		public void Refresh()
		{
			if (character == null)
			{
				return;
			}
			string text = CharacterSelection.Resolve();
			if (string.IsNullOrEmpty(text))
			{
				CharacterData defaultCharacterData = character.DefaultCharacterData;
				if (defaultCharacterData != null)
				{
					character.Apply(defaultCharacterData);
				}
				else
				{
					character.Clear();
				}
			}
			else
			{
				character.Apply(CharacterStorage.Load(text, character.Rig));
			}
		}

		public static void RefreshAll()
		{
			CharacterMenuPreview[] array = Object.FindObjectsByType<CharacterMenuPreview>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Refresh();
			}
		}
	}
}
