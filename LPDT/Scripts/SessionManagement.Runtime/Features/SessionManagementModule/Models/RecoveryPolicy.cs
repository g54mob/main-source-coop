namespace Features.SessionManagementModule.Models
{
	public sealed class RecoveryPolicy
	{
		private readonly int _maxReconnectAttempts;

		private int _attempts;

		private int _progressMarker;

		private bool _hasProgressMarker;

		public int Attempts => _attempts;

		public RecoveryPolicy(int maxReconnectAttempts)
		{
			_maxReconnectAttempts = maxReconnectAttempts;
		}

		public RecoveryDecision OnEntryFailed(int progressMarker)
		{
			if (!_hasProgressMarker || progressMarker != _progressMarker)
			{
				_attempts = 0;
				_progressMarker = progressMarker;
				_hasProgressMarker = true;
			}
			_attempts++;
			if (_attempts <= _maxReconnectAttempts)
			{
				return RecoveryDecision.Reconnect;
			}
			return RecoveryDecision.GiveUp;
		}

		public void OnEntrySucceeded(int progressMarker)
		{
			_attempts = 0;
			_progressMarker = progressMarker;
			_hasProgressMarker = true;
		}
	}
}
