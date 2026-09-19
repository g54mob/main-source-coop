using System;
using System.Collections.Generic;
using System.Linq;
using Features.GameUpdaterModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	public abstract class TutorialUISystemBehaviour : IInitializable, IDisposable
	{
		private readonly BaseTutorialService _baseTutorialService;

		private readonly CurrentTutorialStepModel _currentTutorialStepModel;

		private readonly TutorialModel _tutorialModel;

		private readonly IGameUpdater _gameUpdater;

		private readonly TutorialStepsConfiguration _tutorialStepsConfiguration;

		private readonly IWindowsService _windowsService;

		protected TutorialUISystemBehaviour(IWindowsService windowsService, BaseTutorialService baseTutorialService, CurrentTutorialStepModel currentTutorialStepModel, TutorialModel tutorialModel, IGameUpdater gameUpdater, TutorialStepsConfiguration tutorialStepsConfiguration)
		{
			_windowsService = windowsService;
			_baseTutorialService = baseTutorialService;
			_currentTutorialStepModel = currentTutorialStepModel;
			_tutorialModel = tutorialModel;
			_gameUpdater = gameUpdater;
			_tutorialStepsConfiguration = tutorialStepsConfiguration;
		}

		public void Initialize()
		{
			_currentTutorialStepModel.OnStepStart += HandleStepStarted;
			_currentTutorialStepModel.OnStepEnd += HandleStepEnded;
			_baseTutorialService.OnInitialize += HandleBaseTutorialStarted;
			_baseTutorialService.OnTutorialEnded += HandleBaseTutorialEnded;
			_gameUpdater.OnUpdate += Tick;
		}

		public void Dispose()
		{
			_currentTutorialStepModel.OnStepStart -= HandleStepStarted;
			_currentTutorialStepModel.OnStepEnd -= HandleStepEnded;
			_baseTutorialService.OnInitialize -= HandleBaseTutorialStarted;
			_baseTutorialService.OnTutorialEnded -= HandleBaseTutorialEnded;
			_gameUpdater.OnUpdate -= Tick;
		}

		private void Tick()
		{
			if (_tutorialModel.IsBaseTutorialCompleted)
			{
				_gameUpdater.OnUpdate -= Tick;
			}
		}

		protected virtual void HandleBaseTutorialStarted()
		{
		}

		protected virtual void HandleBaseTutorialEnded()
		{
		}

		protected virtual void HandleStepStarted(TutorialStep stepType, ITutorialStep step)
		{
		}

		protected virtual void HandleStepEnded(TutorialStep stepType, ITutorialStep tutorialStep)
		{
		}

		protected void PreOpenWindow<TWindow>() where TWindow : FocusableWindowBehaviour
		{
			if (!(_windowsService.GetAllWindows().FirstOrDefault((IWindow window) => window.GetType() == typeof(TWindow)) is TWindow val))
			{
				Debug.LogError($"Failed to find window for {typeof(TWindow)}");
			}
			else if (val.WindowStatus == WindowStatus.Closed)
			{
				val.Open();
				val.Hide();
			}
		}

		private List<TPresenter> GetPresenters<TPresenter>() where TPresenter : PresenterBehaviour
		{
			IEnumerable<IWindow> allWindows = _windowsService.GetAllWindows();
			List<TPresenter> list = new List<TPresenter>();
			foreach (IWindow item in allWindows)
			{
				if (item.TryGetPresenter(typeof(TPresenter), out var presenter))
				{
					list.Add(presenter as TPresenter);
				}
			}
			return list;
		}

		private void SetActionIfEqual(TutorialStep stepType, TutorialStep stepToCompare, Action action)
		{
			if (stepType == stepToCompare)
			{
				action();
			}
		}
	}
}
