namespace Mimicraft.Customization
{
	public readonly struct CharacterListEntry
	{
		public readonly string FilePath;

		public readonly string CharacterName;

		public CharacterListEntry(string filePath, string characterName)
		{
			FilePath = filePath;
			CharacterName = characterName;
		}
	}
}
