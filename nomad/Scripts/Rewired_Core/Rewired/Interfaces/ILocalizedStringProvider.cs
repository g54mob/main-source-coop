namespace Rewired.Interfaces
{
	public interface ILocalizedStringProvider
	{
		bool TryGetLocalizedString(string key, out string result);
	}
}
