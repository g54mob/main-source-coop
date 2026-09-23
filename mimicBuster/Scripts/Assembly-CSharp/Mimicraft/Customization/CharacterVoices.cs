namespace Mimicraft.Customization
{
	public static class CharacterVoices
	{
		public static CharacterVoice FromByte(byte value)
		{
			if (value != 1)
			{
				return CharacterVoice.Male;
			}
			return CharacterVoice.Female;
		}
	}
}
