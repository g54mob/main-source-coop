using System;
using Features.ExtendedLogger.Scripts;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public abstract class BaseTutorialStep : ITutorialStep, IDisposable
	{
		private BaseTutorialCustomData _customData;

		public ITutorialCustomData CustomData => _customData;

		public abstract event Action OnStepEnd;

		public abstract void Dispose();

		public virtual void ActivateStep()
		{
			DebugStepActivation();
		}

		public abstract void DeActivateStep();

		public abstract void SkipStep();

		public abstract void RevertStep();

		protected void SetStepPoi(Transform poi)
		{
			_customData.StepPoi = poi;
		}

		private void DebugStepActivation()
		{
			ExtendedDebug.LogFiltered(DebugFilterType.Tutorial, GetType().Name + " is activated");
		}
	}
}
