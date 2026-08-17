using System.Collections.Generic;

namespace NomadDrive.Features.Objectives
{
	public class ObjectiveTrackerState
	{
		public string ObjectiveId;

		public readonly Dictionary<string, ObjectiveStepProgress> StepProgresses = new Dictionary<string, ObjectiveStepProgress>();

		public IEnumerable<string> CompletedStepIds
		{
			get
			{
				foreach (KeyValuePair<string, ObjectiveStepProgress> stepProgress in StepProgresses)
				{
					if (stepProgress.Value.IsCompleted)
					{
						yield return stepProgress.Key;
					}
				}
			}
		}

		public ObjectiveTrackerState(string objectiveId)
		{
			ObjectiveId = objectiveId;
		}

		public bool IsStepCompleted(string stepId)
		{
			if (StepProgresses.TryGetValue(stepId, out var value))
			{
				return value.IsCompleted;
			}
			return false;
		}

		public ObjectiveStepProgress GetOrCreate(string stepId)
		{
			if (!StepProgresses.TryGetValue(stepId, out var value))
			{
				value = new ObjectiveStepProgress();
				StepProgresses[stepId] = value;
			}
			return value;
		}

		public bool MarkStepCompleted(string stepId)
		{
			ObjectiveStepProgress orCreate = GetOrCreate(stepId);
			if (orCreate.IsCompleted)
			{
				return false;
			}
			orCreate.IsCompleted = true;
			return true;
		}
	}
}
