using System;
using System.Collections;
using System.Collections.Generic;
using Features.CoroutineUtils.Scripts;
using Features.DeviceModule.Scripts;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialStepsFactorySystems.API;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	public class TutorialStepsQuery : IDisposable
	{
		private readonly CurrentTutorialStepModel _currentTutorialStepModel;

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly TutorialStepFactoryHolder _tutorialStepFactoryHolder;

		private readonly List<TutorialStepMapper> _tutorialStepMappers = new List<TutorialStepMapper>();

		private int _currentId;

		private ITutorialStep _currentTutorialStep;

		private bool _isActivating;

		private bool _endRequested;

		private bool _endIsQueryEnd;

		public Action OnQueryEnded;

		public Action OnStepEnded;

		private IDeviceService _deviceService;

		public TutorialStepsQuery(TutorialStepFactoryHolder tutorialStepFactoryHolder, CurrentTutorialStepModel currentTutorialStepModel, ICoroutineRunner coroutineRunner, IDeviceService deviceService)
		{
			_deviceService = deviceService;
			_tutorialStepFactoryHolder = tutorialStepFactoryHolder;
			_currentTutorialStepModel = currentTutorialStepModel;
			_coroutineRunner = coroutineRunner;
		}

		public void Dispose()
		{
			if (_currentTutorialStep != null)
			{
				CleanCurrentStep();
			}
		}

		public void AddTutorialStep(TutorialStepMapper tutorialStepMapper)
		{
			foreach (TutorialStepMapper tutorialStepMapper2 in _tutorialStepMappers)
			{
				if (tutorialStepMapper.TutorialStep == tutorialStepMapper2.TutorialStep)
				{
					return;
				}
			}
			_tutorialStepMappers.Add(tutorialStepMapper);
		}

		public void PopStep()
		{
			TutorialStepMapper tutorialStepMapper = _tutorialStepMappers[_currentId];
			ActivateStep(tutorialStepMapper);
		}

		public void RestartQuery()
		{
			_currentTutorialStep?.DeActivateStep();
			_currentId = 0;
		}

		public void GoToStep(TutorialStep step)
		{
			_currentId = _tutorialStepMappers.FindIndex((TutorialStepMapper stepMapper) => stepMapper.TutorialStep == step);
			TutorialStepMapper tutorialStepMapper = _tutorialStepMappers[_currentId];
			ActivateStep(tutorialStepMapper);
		}

		public void SkipQueryToEnd()
		{
			_coroutineRunner.StartCoroutine(StartSkipQueryCoroutine(0));
		}

		public void SkipCurrentStep()
		{
			_currentTutorialStep.SkipStep();
		}

		public void SkipQueryToStep(TutorialStep step)
		{
			int stepId = _tutorialStepMappers.FindIndex((TutorialStepMapper stepMapper) => stepMapper.TutorialStep == step);
			_coroutineRunner.StartCoroutine(StartSkipQueryCoroutine(stepId));
		}

		private void ActivateStep(TutorialStepMapper tutorialStepMapper)
		{
			Type tutorialStepTypeByPlatform = tutorialStepMapper.GetTutorialStepTypeByPlatform(_deviceService.GetCurrentDevice());
			ITutorialStep tutorialStep = (_currentTutorialStep = _tutorialStepFactoryHolder.CurrentLocalTutorialStepFactory.Create(tutorialStepTypeByPlatform));
			if (tutorialStep is ITutorialStoppableStep tutorialStoppableStep)
			{
				tutorialStoppableStep.OnStopTutorial += InvokeQueryEnd;
			}
			bool num = _currentId == _tutorialStepMappers.Count - 1;
			if (num)
			{
				tutorialStep.OnStepEnd += InvokeQueryEnd;
			}
			else
			{
				tutorialStep.OnStepEnd += InvokeStepEnd;
			}
			_currentTutorialStepModel.SetStepStarted(tutorialStep, tutorialStepMapper.TutorialStep);
			if (!num)
			{
				_currentId++;
				_currentTutorialStepModel.SetStepInitialized();
			}
			_isActivating = true;
			tutorialStep.ActivateStep();
			_isActivating = false;
			ProcessPendingEnd();
		}

		private void InvokeStepEnd()
		{
			if (_isActivating)
			{
				_endRequested = true;
				_endIsQueryEnd = false;
			}
			else
			{
				DoStepEnd();
			}
		}

		private void InvokeQueryEnd()
		{
			if (_isActivating)
			{
				_endRequested = true;
				_endIsQueryEnd = true;
			}
			else
			{
				DoQueryEnd();
			}
		}

		private void ProcessPendingEnd()
		{
			if (_endRequested)
			{
				_endRequested = false;
				if (_endIsQueryEnd)
				{
					DoQueryEnd();
				}
				else
				{
					DoStepEnd();
				}
			}
		}

		private void DoStepEnd()
		{
			_currentTutorialStepModel.SetStepEnded();
			_currentTutorialStep.DeActivateStep();
			CleanCurrentStep();
			OnStepEnded?.Invoke();
		}

		private void DoQueryEnd()
		{
			_currentTutorialStepModel.SetStepEnded();
			_currentTutorialStep.DeActivateStep();
			CleanCurrentStep();
			OnQueryEnded?.Invoke();
		}

		private void CleanCurrentStep()
		{
			_currentTutorialStep.OnStepEnd -= InvokeStepEnd;
			_currentTutorialStep.OnStepEnd -= InvokeQueryEnd;
			if (_currentTutorialStep is ITutorialStoppableStep tutorialStoppableStep)
			{
				tutorialStoppableStep.OnStopTutorial -= InvokeQueryEnd;
			}
			_currentTutorialStep.Dispose();
		}

		private IEnumerator StartSkipQueryCoroutine(int stepId)
		{
			while (_currentId != stepId)
			{
				yield return null;
				yield return null;
				_currentTutorialStep.SkipStep();
			}
		}
	}
}
