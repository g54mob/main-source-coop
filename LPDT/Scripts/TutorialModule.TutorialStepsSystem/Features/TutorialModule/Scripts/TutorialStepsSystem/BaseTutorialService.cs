using System;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	public class BaseTutorialService : TutorialBehaviour
	{
		public event Action OnInitialize;

		public event Action OnQueryEnded;

		public BaseTutorialService(BaseTutorialStepsQuery tutorialStepsQuery, CurrentTutorialStepModel currentTutorialStepsModel)
			: base(tutorialStepsQuery, currentTutorialStepsModel)
		{
		}

		public override void Initialize()
		{
			base.Initialize();
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_AcquireContainer).SetDefaultTutorialStep<BaseTutorial_Default_AcquireContainer>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_ContainerToLocationDeliver).SetDefaultTutorialStep<BaseTutorial_Default_ContainerToLocationDeliver>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_LocationContainerInteraction).SetDefaultTutorialStep<BaseTutorial_Default_LocationContainerInteraction>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_QuotaCollect).SetDefaultTutorialStep<BaseTutorial_Default_QuotaCollect>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_Hide).SetDefaultTutorialStep<BaseTutorial_Default_Hide>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_EnemyEncounter).SetDefaultTutorialStep<BaseTutorial_Default_EnemyEncounter>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_AcquireGuideCarryItem).SetDefaultTutorialStep<BaseTutorial_Default_AcquireGuideCarryItem>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_EnemyFight).SetDefaultTutorialStep<BaseTutorial_Default_EnemyFight>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_AcquireDeadPart).SetDefaultTutorialStep<BaseTutorial_Default_AcquireDeadPart>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_PlayerResurrect).SetDefaultTutorialStep<BaseTutorial_Default_PlayerResurrect>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_AcquireContainer2).SetDefaultTutorialStep<BaseTutorial_Default_AcquireContainer2>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_ContainerToBoatDeliver).SetDefaultTutorialStep<BaseTutorial_Default_ContainerToBoatDeliver>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_QuotaSubmit).SetDefaultTutorialStep<BaseTutorial_Default_QuotaSubmit>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_AcquireBell).SetDefaultTutorialStep<BaseTutorial_Default_AcquireBell>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_LevelTransition).SetDefaultTutorialStep<BaseTutorial_Default_LevelTransition>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_ChooseStoreCard).SetDefaultTutorialStep<BaseTutorial_Default_ChooseStoreCard>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_ConfirmStorePurchase).SetDefaultTutorialStep<BaseTutorial_Default_ConfirmStorePurchase>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_AcquireReward).SetDefaultTutorialStep<BaseTutorial_Default_AcquireReward>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_RewardInteraction).SetDefaultTutorialStep<BaseTutorial_Default_RewardInteraction>());
			_tutorialStepsQuery.AddTutorialStep(TutorialStepMapperBuilder.CreateTutorialStepMapper(TutorialStep.BaseTutorial_End).SetDefaultTutorialStep<BaseTutorial_Default_End>());
			this.OnInitialize?.Invoke();
			TutorialStepsQuery tutorialStepsQuery = _tutorialStepsQuery;
			tutorialStepsQuery.OnQueryEnded = (Action)Delegate.Combine(tutorialStepsQuery.OnQueryEnded, new Action(InvokeOnQueryEnded));
		}

		public override void Dispose()
		{
			base.Dispose();
			TutorialStepsQuery tutorialStepsQuery = _tutorialStepsQuery;
			tutorialStepsQuery.OnQueryEnded = (Action)Delegate.Remove(tutorialStepsQuery.OnQueryEnded, new Action(InvokeOnQueryEnded));
		}

		private void InvokeOnQueryEnded()
		{
			this.OnQueryEnded?.Invoke();
		}
	}
}
