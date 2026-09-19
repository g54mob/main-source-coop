using System;
using Features.MultiplayerSessionServices.Scripts.NetworkMasterClientTracking;
using Fusion;

namespace Features.MultiplayerSessionServices.Scripts
{
	public class MultiplayerModel
	{
		private bool _isOperationInProgress;

		private bool _isSearchingInProgress;

		private bool _isReconnecting;

		private NetworkMasterClientTracker _networkMasterClientTracker;

		private NetworkRunner _networkRunner;

		public Action<int> ReconnectDeathAction;

		public NetworkRunner NetworkRunner
		{
			get
			{
				return _networkRunner;
			}
			set
			{
				_networkRunner = value;
				this.OnNetworkRunnerChanged?.Invoke();
			}
		}

		public NetworkMasterClientTracker NetworkMasterClientTracker
		{
			get
			{
				return _networkMasterClientTracker;
			}
			set
			{
				_networkMasterClientTracker = value;
				if (_networkMasterClientTracker != null)
				{
					this.OnNetworkMasterClientTrackerInitialized?.Invoke();
				}
			}
		}

		public bool IsOperationInProgress
		{
			get
			{
				return _isOperationInProgress;
			}
			set
			{
				_isOperationInProgress = value;
				this.OnStartGameInProgressChanged?.Invoke(value);
			}
		}

		public bool IsSearchingInProgress
		{
			get
			{
				return _isSearchingInProgress;
			}
			set
			{
				_isSearchingInProgress = value;
				this.OnSearchingInProgressChanged?.Invoke(value);
			}
		}

		public bool IsReconnecting
		{
			get
			{
				return _isReconnecting;
			}
			set
			{
				_isReconnecting = value;
			}
		}

		public JoinSource LocalJoinSource { get; set; }

		public event Action OnNetworkRunnerChanged;

		public event Action<bool> OnStartGameInProgressChanged;

		public event Action<bool> OnSearchingInProgressChanged;

		public event Action OnNetworkMasterClientTrackerInitialized;

		public event Action<bool> OnSessionOpenedChanged;

		public void InvokeSessionOpenedChanged(bool isOpened)
		{
			this.OnSessionOpenedChanged?.Invoke(isOpened);
		}
	}
}
