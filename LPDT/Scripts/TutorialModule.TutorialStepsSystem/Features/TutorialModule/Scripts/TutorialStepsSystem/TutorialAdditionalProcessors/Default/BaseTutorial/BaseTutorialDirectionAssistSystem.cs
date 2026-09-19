using System;
using Features.CameraModelModule;
using Features.GameUpdaterModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial;
using Features.TutorialModule.Scripts.UITipsModule;
using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialDirectionAssistSystem : IInitializable, IDisposable
	{
		private readonly CurrentTutorialStepModel _currentTutorialStepModel;

		private readonly IGameUpdater _gameUpdater;

		private readonly TutorialStepsConfiguration _tutorialStepsConfiguration;

		private readonly IUITipService _uiTipService;

		private readonly BaseTutorialUIDataHolder _baseTutorialUIDataHolder;

		private readonly CameraModel _cameraModel;

		private readonly TutorialModel _tutorialModel;

		private ITutorialStep _currentStep;

		private UITipHandle _tipHandle = UITipHandle.Invalid;

		private bool _assistResolved;

		public BaseTutorialDirectionAssistSystem(CurrentTutorialStepModel currentTutorialStepModel, IGameUpdater gameUpdater, TutorialStepsConfiguration tutorialStepsConfiguration, IUITipService uiTipService, BaseTutorialUIDataHolder baseTutorialUIDataHolder, CameraModel cameraModel, TutorialModel tutorialModel)
		{
			_currentTutorialStepModel = currentTutorialStepModel;
			_gameUpdater = gameUpdater;
			_tutorialStepsConfiguration = tutorialStepsConfiguration;
			_uiTipService = uiTipService;
			_baseTutorialUIDataHolder = baseTutorialUIDataHolder;
			_cameraModel = cameraModel;
			_tutorialModel = tutorialModel;
		}

		public void Initialize()
		{
			_currentTutorialStepModel.OnStepStart += OnTutorialStepStarted;
			_currentTutorialStepModel.OnStepEnd += OnTutorialStepEnded;
			_gameUpdater.OnUpdate += Tick;
		}

		public void Dispose()
		{
			_currentTutorialStepModel.OnStepStart -= OnTutorialStepStarted;
			_currentTutorialStepModel.OnStepEnd -= OnTutorialStepEnded;
			_gameUpdater.OnUpdate -= Tick;
			KillTip();
		}

		private void OnTutorialStepStarted(TutorialStep tutorialStep, ITutorialStep step)
		{
			_currentStep = step;
			_assistResolved = false;
			KillTip();
		}

		private void OnTutorialStepEnded(TutorialStep tutorialStep, ITutorialStep step)
		{
			_currentStep = null;
			_assistResolved = false;
			KillTip();
		}

		private void Tick()
		{
			if (_currentStep == null || _assistResolved || !_tutorialModel.IsBaseTutorialSequenceInvoked)
			{
				return;
			}
			Transform stepPoi = ((BaseTutorialCustomData)(object)_currentStep.CustomData).StepPoi;
			if (!(stepPoi == null))
			{
				Transform transform = _cameraModel.CameraObject.transform;
				if (Vector3.Angle(transform.forward, stepPoi.position - transform.position) > _tutorialStepsConfiguration.DirectionAssistSpotAngle)
				{
					TryShowTip();
					return;
				}
				KillTip();
				_assistResolved = true;
			}
		}

		private void TryShowTip()
		{
			if (!_tipHandle.IsValid && _uiTipService.IsReady)
			{
				RectTransform turnAroundTipHolder = _baseTutorialUIDataHolder.TurnAroundTipHolder;
				if (!(turnAroundTipHolder == null))
				{
					_tipHandle = _uiTipService.CreateTip(UITipType.TurnAround, turnAroundTipHolder.anchoredPosition);
				}
			}
		}

		private void KillTip()
		{
			if (_tipHandle.IsValid)
			{
				_uiTipService.KillTip(_tipHandle);
				_tipHandle = UITipHandle.Invalid;
			}
		}
	}
}
