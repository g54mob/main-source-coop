using Fusion;

namespace Features.MultiplayerSessionServices.Scripts
{
	public class CreateRoomResult
	{
		public bool IsSuccess { get; set; }

		public string ErrorMessage { get; set; }

		public ShutdownReason ShutdownReason { get; set; }

		public CreateRoomResult(bool isSuccess)
		{
			IsSuccess = isSuccess;
		}
	}
}
