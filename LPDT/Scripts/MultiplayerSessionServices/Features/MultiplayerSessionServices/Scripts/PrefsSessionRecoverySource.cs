namespace Features.MultiplayerSessionServices.Scripts
{
	public sealed class PrefsSessionRecoverySource : ISessionRecoverySource
	{
		public bool HasError => PlayerSessionPrefs.IsError();

		public bool TryGetReconnectSession(out string sessionName)
		{
			return PlayerSessionPrefs.TryGetSavedSessionName(out sessionName);
		}
	}
}
