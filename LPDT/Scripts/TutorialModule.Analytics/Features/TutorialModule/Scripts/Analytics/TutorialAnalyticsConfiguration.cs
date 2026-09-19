using System.Collections.Generic;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.TutorialModule.Scripts.Analytics
{
	[CreateAssetMenu(fileName = "TutorialAnalyticsConfiguration_Default", menuName = "Configurations/TutorialModule/Analytics/TutorialAnalyticsConfiguration")]
	public class TutorialAnalyticsConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public List<TutorialStep> ExcludedSteps { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<TutorialStep, string> AnalyticsStepNameMap { get; private set; }
	}
}
