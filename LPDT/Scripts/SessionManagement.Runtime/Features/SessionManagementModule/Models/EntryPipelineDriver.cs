using System.Collections.Generic;

namespace Features.SessionManagementModule.Models
{
	public static class EntryPipelineDriver
	{
		public static EntryPipelineResult Run(IReadOnlyList<EntryStepOutcome> stepOutcomes)
		{
			for (int i = 0; i < stepOutcomes.Count; i++)
			{
				switch (stepOutcomes[i])
				{
				case EntryStepOutcome.Threw:
					return EntryPipelineResult.Failed(i);
				case EntryStepOutcome.VerificationFailed:
					return EntryPipelineResult.VerificationFailed(i);
				}
			}
			return EntryPipelineResult.Completed();
		}
	}
}
