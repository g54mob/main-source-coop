namespace Features.RagdollModule.Scripts.PoseTools
{
	public struct RagdollBonePoseCaptureResult
	{
		public bool Success;

		public int CapturedCount;

		public string Message;

		public static RagdollBonePoseCaptureResult Captured(int capturedCount)
		{
			return new RagdollBonePoseCaptureResult
			{
				Success = true,
				CapturedCount = capturedCount,
				Message = $"Captured {capturedCount} bone poses."
			};
		}

		public static RagdollBonePoseCaptureResult Failed(string message)
		{
			return new RagdollBonePoseCaptureResult
			{
				Success = false,
				CapturedCount = 0,
				Message = message
			};
		}
	}
}
