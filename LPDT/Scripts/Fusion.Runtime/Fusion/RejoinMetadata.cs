using Photon.Realtime;

namespace Fusion
{
	internal struct RejoinMetadata
	{
		public bool TryingToReconnect;

		public bool TryingMoveSessionToNewRoom;

		public int RejoinAttempts;

		public AppSettings AppSettings;

		public StartGameArgs StartGameArgs;

		private const int RejoinAttemptsMax = 5;

		public RejoinMetadata()
		{
			StartGameArgs = default(StartGameArgs);
			TryingToReconnect = false;
			TryingMoveSessionToNewRoom = false;
			RejoinAttempts = 5;
			AppSettings = null;
		}

		public void Reset()
		{
			RejoinAttempts = 5;
			TryingMoveSessionToNewRoom = false;
			TryingToReconnect = false;
		}
	}
}
