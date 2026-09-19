namespace Features.SessionManagementModule.Models
{
	public readonly struct EntryPipelineResult
	{
		public EntryPipelineStatus Status { get; }

		public int FailedStepIndex { get; }

		public bool IsFailure
		{
			get
			{
				if (Status != EntryPipelineStatus.Failed)
				{
					return Status == EntryPipelineStatus.VerificationFailed;
				}
				return true;
			}
		}

		private EntryPipelineResult(EntryPipelineStatus status, int failedStepIndex)
		{
			Status = status;
			FailedStepIndex = failedStepIndex;
		}

		public static EntryPipelineResult Completed()
		{
			return new EntryPipelineResult(EntryPipelineStatus.Completed, -1);
		}

		public static EntryPipelineResult Failed(int stepIndex)
		{
			return new EntryPipelineResult(EntryPipelineStatus.Failed, stepIndex);
		}

		public static EntryPipelineResult VerificationFailed(int stepIndex)
		{
			return new EntryPipelineResult(EntryPipelineStatus.VerificationFailed, stepIndex);
		}
	}
}
