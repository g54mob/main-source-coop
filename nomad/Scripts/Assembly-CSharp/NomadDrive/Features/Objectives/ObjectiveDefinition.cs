using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace NomadDrive.Features.Objectives
{
	[CreateAssetMenu(menuName = "NomadDrive/Objectives/Objective", fileName = "Objective")]
	public class ObjectiveDefinition : SerializedScriptableObject
	{
		[Tooltip("Stable id used for save data. Never change after release.")]
		public string ObjectiveId;

		[Tooltip("Localization key (@category.entry) or raw text for the objective title.")]
		[TextArea(1, 3)]
		public string DescriptionKey;

		public ObjectiveTextStyle DescriptionStyle = new ObjectiveTextStyle
		{
			Bold = true,
			UseCustomColor = false,
			Color = Color.white,
			FontSizeOverride = 0
		};

		[Tooltip("Optional. Shown in the discovery banner and (if set) on the panel row.")]
		public Sprite Icon;

		[Tooltip("If true, this objective's discovery and step progress are synchronized across all players via ObjectivesNetworkSync. Any player firing a trigger advances the shared state, and late joiners receive the current state on connect. Re-attaching the same source (same netId) does not double-count.")]
		public bool MustSync;

		[Tooltip("Any of these conditions activates the objective. Each entry may optionally auto-complete a step.")]
		public List<ObjectiveDiscoveryEntry> DiscoveryTriggers = new List<ObjectiveDiscoveryEntry>();

		[HideInInspector]
		[SerializeReference]
		[FormerlySerializedAs("DiscoveryTrigger")]
		private ObjectiveTrigger _legacyDiscoveryTrigger;

		public List<ObjectiveStepDefinition> Steps = new List<ObjectiveStepDefinition>();

		public int StepCount => Steps?.Count ?? 0;

		public ObjectiveStepDefinition GetStep(string stepId)
		{
			if (Steps == null)
			{
				return null;
			}
			for (int i = 0; i < Steps.Count; i++)
			{
				if (Steps[i] != null && Steps[i].StepId == stepId)
				{
					return Steps[i];
				}
			}
			return null;
		}

		protected override void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();
			MigrateLegacyDiscoveryTrigger();
		}

		private void MigrateLegacyDiscoveryTrigger()
		{
			if (_legacyDiscoveryTrigger != null)
			{
				if (DiscoveryTriggers == null)
				{
					DiscoveryTriggers = new List<ObjectiveDiscoveryEntry>();
				}
				if (DiscoveryTriggers.Count == 0)
				{
					DiscoveryTriggers.Add(new ObjectiveDiscoveryEntry
					{
						Trigger = _legacyDiscoveryTrigger,
						AutoCompletesStepId = string.Empty
					});
				}
				_legacyDiscoveryTrigger = null;
			}
		}
	}
}
