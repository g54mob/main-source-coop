using System;
using Features.DeviceModule.Scripts.DeviceData;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	[Serializable]
	public class TutorialStepMapper
	{
		public TutorialStep TutorialStep;

		public Type DefaultTutorialStep;

		public Type PCTutorialStep;

		public Type MobileTutorialStep;

		public Type ConsoleTutorialStep;

		public void SetTutorialStep(TutorialStep tutorialStep)
		{
			TutorialStep = tutorialStep;
		}

		public void SetDefaultTutorialStepType(Type defaultTutorialStep)
		{
			DefaultTutorialStep = defaultTutorialStep;
		}

		public void SetDefaultTutorialStepType<TTutorialStepType>() where TTutorialStepType : ITutorialStep
		{
			DefaultTutorialStep = typeof(TTutorialStepType);
		}

		public void OverridePCTutorialStepType(Type pcTutorialStep)
		{
			PCTutorialStep = pcTutorialStep;
		}

		public void OverridePCTutorialStepType<TTutorialStepType>() where TTutorialStepType : ITutorialStep
		{
			PCTutorialStep = typeof(TTutorialStepType);
		}

		public void OverrideMobileTutorialStepType(Type mobileTutorialStep)
		{
			MobileTutorialStep = mobileTutorialStep;
		}

		public void OverrideMobileTutorialStepType<TTutorialStepType>() where TTutorialStepType : ITutorialStep
		{
			MobileTutorialStep = typeof(TTutorialStepType);
		}

		public void OverrideConsoleTutorialStepType(Type consoleTutorialStep)
		{
			ConsoleTutorialStep = consoleTutorialStep;
		}

		public void OverrideConsoleTutorialStepType<TTutorialStepType>() where TTutorialStepType : ITutorialStep
		{
			ConsoleTutorialStep = typeof(TTutorialStepType);
		}

		public Type GetTutorialStepTypeByPlatform(DeviceType deviceType)
		{
			switch (deviceType)
			{
			case DeviceType.PersonalComputer:
				return PCTutorialStep ?? DefaultTutorialStep;
			case DeviceType.Android:
			case DeviceType.IOS:
				return MobileTutorialStep ?? DefaultTutorialStep;
			case DeviceType.SteamDeck:
			case DeviceType.XBox:
			case DeviceType.PlayStation5:
			case DeviceType.Nintendo:
			case DeviceType.PlayStation4:
				return ConsoleTutorialStep ?? DefaultTutorialStep;
			default:
				return DefaultTutorialStep;
			}
		}
	}
}
