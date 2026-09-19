using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public struct BaseTutorialCustomData : ITutorialCustomData
	{
		public Transform StepPoi { get; set; }
	}
}
