using Fusion;

namespace Features.MultiplayerSessionServices.Scripts
{
	public class JoinRoomResult
	{
		public bool IsSuccess { get; set; }

		public string ErrorMessage { get; set; }

		public ShutdownReason ShutdownReason { get; set; }

		public JoinFailureReason FailureReason { get; set; }

		public JoinRoomResult(bool isSuccess)
		{
			IsSuccess = isSuccess;
		}
	}
}
