namespace Features.SteamImplementationModule.Scripts
{
	public interface ISteamLanguageProvider
	{
		bool IsAvailable { get; }

		string GetSteamUILanguage();
	}
}
