using System;
using System.Collections.Generic;

namespace NomadDrive.Features.Objectives
{
	public interface IObjectivesService
	{
		IReadOnlyCollection<string> DiscoveredObjectiveIds { get; }

		IReadOnlyCollection<string> CompletedObjectiveIds { get; }

		IReadOnlyCollection<ObjectiveTrackerState> ActiveTrackers { get; }

		event Action<ObjectiveDefinition, ObjectiveTrackerState> ObjectiveDiscovered;

		event Action<ObjectiveDefinition, ObjectiveTrackerState, ObjectiveStepDefinition> StepCompleted;

		event Action<ObjectiveDefinition, ObjectiveTrackerState, ObjectiveStepDefinition, ObjectiveStepProgress> StepProgressChanged;

		event Action<ObjectiveDefinition> ObjectiveCompleted;

		bool IsDiscovered(string objectiveId);

		bool IsCompleted(string objectiveId);

		bool IsStepCompleted(string objectiveId, string stepId);

		void TryDiscoverObjective(string objectiveId);

		void TryDiscoverObjective(string objectiveId, string autoCompleteStepId);
	}
}
