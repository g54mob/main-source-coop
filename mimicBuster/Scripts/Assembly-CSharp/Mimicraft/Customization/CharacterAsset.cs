using UnityEngine;

namespace Mimicraft.Customization
{
	public class CharacterAsset : ScriptableObject
	{
		[Tooltip("Dosyanın içeriği - CharacterFile formatında. İçe aktarma sırasında dolduruluyor.")]
		[SerializeField]
		[HideInInspector]
		private byte[] data;

		[Tooltip("Dosyanın içindeki görünen ad. Sadece bilgi için; hangi asset olduğunu ayırt etmeye yarar.")]
		[SerializeField]
		private string characterName;

		public string CharacterName => characterName;

		public void SetContents(byte[] bytes, string name)
		{
			data = bytes;
			characterName = name;
		}

		public CharacterData ToCharacterData(CharacterRigDefinition rig)
		{
			if (data == null || data.Length == 0)
			{
				return null;
			}
			if (!CharacterFile.TryDecode(data, out var _, out var characterData, out var savedBoxes))
			{
				Debug.LogWarning("[CharacterAsset] '" + base.name + "' okunamadi - bu build'in anlamadigi bir surumde olabilir.", this);
				return null;
			}
			if (rig == null)
			{
				return characterData;
			}
			CharacterBoxMigration.Apply(characterData, savedBoxes, rig);
			CharacterData characterData2 = new CharacterData(characterData.RigId);
			foreach (CharacterPartData part in characterData.Parts)
			{
				if (part != null && !string.IsNullOrWhiteSpace(part.PartId) && rig.Find(part.PartId) != null)
				{
					characterData2.Set(part.PartId, part.Grid);
				}
			}
			return characterData2;
		}
	}
}
