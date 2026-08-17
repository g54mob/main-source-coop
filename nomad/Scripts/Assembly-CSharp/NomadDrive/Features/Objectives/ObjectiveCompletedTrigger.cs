using System;
using UnityEngine;

namespace NomadDrive.Features.Objectives
{
	[Serializable]
	public class ObjectiveCompletedTrigger : ObjectiveTrigger
	{
		[Tooltip("ObjectiveId that must complete before this trigger fires.")]
		public string TargetObjectiveId;

		[Tooltip("If the target objective is already completed when this trigger activates, fire immediately.")]
		public bool FireImmediatelyIfAlreadyComplete = true;

		protected override void OnActivate()
		{
			IObjectivesService objectivesService = Context?.ObjectivesService;
			if (objectivesService != null && !string.IsNullOrEmpty(TargetObjectiveId))
			{
				if (FireImmediatelyIfAlreadyComplete && objectivesService.IsCompleted(TargetObjectiveId))
				{
					Fire?.Invoke();
				}
				else
				{
					objectivesService.ObjectiveCompleted += HandleObjectiveCompleted;
				}
			}
		}

		protected override void OnDeactivate()
		{
			IObjectivesService objectivesService = Context?.ObjectivesService;
			if (objectivesService != null)
			{
				objectivesService.ObjectiveCompleted -= HandleObjectiveCompleted;
			}
		}

		private void HandleObjectiveCompleted(ObjectiveDefinition def)
		{
			if (!(def == null) && !(def.ObjectiveId != TargetObjectiveId))
			{
				Fire?.Invoke();
			}
		}
	}
}
