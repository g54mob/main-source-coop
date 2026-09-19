namespace Features.SteamImplementationModule.Scripts
{
	public interface ISteamRegionProvider
	{
		bool IsAvailable { get; }

		string GetIpCountry();
	}
}
