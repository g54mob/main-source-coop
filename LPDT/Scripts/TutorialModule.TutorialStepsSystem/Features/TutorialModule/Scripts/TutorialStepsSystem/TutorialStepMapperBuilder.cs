using System;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	public static class TutorialStepMapperBuilder
	{
		public static TutorialStepMapper CreateTutorialStepMapper(TutorialStep tutorialStep)
		{
			return new TutorialStepMapper
			{
				TutorialStep = tutorialStep
			};
		}

		public static TutorialStepMapper SetDefaultTutorialStepType(this TutorialStepMapper tutorialStepMapper, Type defaultTutorialStep)
		{
			tutorialStepMapper.DefaultTutorialStep = defaultTutorialStep;
			return tutorialStepMapper;
		}

		public static TutorialStepMapper SetDefaultTutorialStep<TTutorialStepType>(this TutorialStepMapper tutorialStepMapper) where TTutorialStepType : ITutorialStep
		{
			tutorialStepMapper.DefaultTutorialStep = typeof(TTutorialStepType);
			return tutorialStepMapper;
		}

		public static TutorialStepMapper OverridePCTutorialStep(this TutorialStepMapper tutorialStepMapper, Type pcTutorialStep)
		{
			tutorialStepMapper.PCTutorialStep = pcTutorialStep;
			return tutorialStepMapper;
		}

		public static TutorialStepMapper OverridePCTutorialStep<TTutorialStepType>(this TutorialStepMapper tutorialStepMapper) where TTutorialStepType : ITutorialStep
		{
			tutorialStepMapper.PCTutorialStep = typeof(TTutorialStepType);
			return tutorialStepMapper;
		}

		public static TutorialStepMapper OverrideMobileTutorialStep(this TutorialStepMapper tutorialStepMapper, Type mobileTutorialStep)
		{
			tutorialStepMapper.MobileTutorialStep = mobileTutorialStep;
			return tutorialStepMapper;
		}

		public static TutorialStepMapper OverrideMobileTutorialStep<TTutorialStepType>(this TutorialStepMapper tutorialStepMapper) where TTutorialStepType : ITutorialStep
		{
			tutorialStepMapper.MobileTutorialStep = typeof(TTutorialStepType);
			return tutorialStepMapper;
		}

		public static TutorialStepMapper OverrideConsoleTutorialStep(this TutorialStepMapper tutorialStepMapper, Type consoleTutorialStep)
		{
			tutorialStepMapper.ConsoleTutorialStep = consoleTutorialStep;
			return tutorialStepMapper;
		}

		public static TutorialStepMapper OverrideConsoleTutorialStep<TTutorialStepType>(this TutorialStepMapper tutorialStepMapper) where TTutorialStepType : ITutorialStep
		{
			tutorialStepMapper.ConsoleTutorialStep = typeof(TTutorialStepType);
			return tutorialStepMapper;
		}
	}
}
