namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data
{
	public class MenuOpenedAnalyticsSourceModel
	{
		private bool _hasPending;

		private MainMenuOpenedAnalyticsSource _pending;

		public void SetPending(MainMenuOpenedAnalyticsSource source)
		{
			_pending = source;
			_hasPending = true;
		}

		public MainMenuOpenedAnalyticsSource Consume()
		{
			MainMenuOpenedAnalyticsSource result = (_hasPending ? _pending : MainMenuOpenedAnalyticsSource.GameStart);
			_hasPending = false;
			return result;
		}
	}
}
