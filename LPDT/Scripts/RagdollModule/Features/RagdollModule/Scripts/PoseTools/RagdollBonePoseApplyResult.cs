namespace Features.RagdollModule.Scripts.PoseTools
{
	public struct RagdollBonePoseApplyResult
	{
		public bool Success;

		public int AppliedCount;

		public int MissingCount;

		public string Message;

		public static RagdollBonePoseApplyResult Applied(int appliedCount, int missingCount)
		{
			return new RagdollBonePoseApplyResult
			{
				Success = true,
				AppliedCount = appliedCount,
				MissingCount = missingCount,
				Message = $"Applied {appliedCount} bone poses. Missing: {missingCount}."
			};
		}

		public static RagdollBonePoseApplyResult Failed(string message)
		{
			return new RagdollBonePoseApplyResult
			{
				Success = false,
				AppliedCount = 0,
				MissingCount = 0,
				Message = message
			};
		}
	}
}
