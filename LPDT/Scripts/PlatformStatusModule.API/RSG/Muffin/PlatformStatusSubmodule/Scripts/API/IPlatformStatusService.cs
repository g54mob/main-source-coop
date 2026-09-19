namespace RSG.Muffin.PlatformStatusSubmodule.Scripts.API
{
	public interface IPlatformStatusService
	{
		void UpdateGenericStatus<TValue>(string key, TValue value);

		void UpdatePlayerGroup(string groupId, int groupSize);

		void ClearPlayerGroup();
	}
}
