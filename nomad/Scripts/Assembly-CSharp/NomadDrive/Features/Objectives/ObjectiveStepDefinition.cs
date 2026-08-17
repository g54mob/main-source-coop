using System;
using UnityEngine;

namespace NomadDrive.Features.Objectives
{
	[Serializable]
	public class ObjectiveStepDefinition
	{
		[Tooltip("Stable identifier unique within the parent objective. Used in save data.")]
		public string StepId;

		[Tooltip("Localization key (@category.entry) or raw text shown next to the checkmark.")]
		[TextArea(1, 3)]
		public string DescriptionKey;

		[Tooltip("Optional. If left empty, the row renders text only.")]
		public Sprite Icon;

		[SerializeReference]
		[Tooltip("Trigger that, when fired, completes this step (Boolean), increments its counter (DiscreteCount), or pushes ratio updates (FloatRatio).")]
		public ObjectiveTrigger CompletionTrigger;

		[Tooltip("Boolean: single fire completes. DiscreteCount: each fire increments by 1, shown as X/Y text. FloatRatio: trigger pushes continuous 0..1 fill, shown as a progress bar.")]
		public StepProgressMode ProgressMode;

		[Tooltip("Number of trigger fires required to complete this step. Each fire of CompletionTrigger increments by 1.")]
		public int DiscreteTarget = 1;
	}
}
