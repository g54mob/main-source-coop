using Fusion;

namespace Features.AIModuleStateMachine.Scripts.Data
{
	public class TargetDetectData
	{
		public PlayerRef DetectedPlayer;

		public TargetDetectionReason DetectionReason;

		public TargetDetectData(PlayerRef detectedPlayer, TargetDetectionReason detectionReason)
		{
			DetectedPlayer = detectedPlayer;
			DetectionReason = detectionReason;
		}
	}
}
