using System;
using UnityEngine;

namespace NomadDrive.Features.Objectives
{
	[Serializable]
	public class ObjectiveDiscoveryEntry
	{
		[SerializeReference]
		[Tooltip("One of the conditions that can discover this objective. Any entry firing activates the objective.")]
		public ObjectiveTrigger Trigger;

		[Tooltip("Optional. StepId in this objective that this same condition also represents. If set, that step is auto-marked completed when this trigger fires the discovery.")]
		public string AutoCompletesStepId;
	}
}
