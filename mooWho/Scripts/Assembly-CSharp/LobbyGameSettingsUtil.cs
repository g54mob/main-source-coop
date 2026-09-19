public static class LobbyGameSettingsUtil
{
	public static float GetAnimalSoundMax(AnimalSoundLevel level)
	{
		return level switch
		{
			AnimalSoundLevel.Low => 85f, 
			AnimalSoundLevel.High => 47f, 
			_ => 70f, 
		};
	}

	public static float GetAnimalSoundSkew(AnimalSoundLevel level)
	{
		return level switch
		{
			AnimalSoundLevel.Low => 4f, 
			AnimalSoundLevel.High => 2f, 
			_ => 2.5f, 
		};
	}

	public static int GetNpcPopulationCount(NpcPopulationLevel level)
	{
		return level switch
		{
			NpcPopulationLevel.Low => 4, 
			NpcPopulationLevel.High => 12, 
			_ => 8, 
		};
	}
}
