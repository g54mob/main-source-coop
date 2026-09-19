using System;
using System.Collections.Generic;
using Features.CameraModelModule;
using Features.InputModule.Scripts.Generated;
using Features.MouseVisibilityModule.Scripts;
using Features.SettingsMenuModule.Scripts;
using Features.ViewSystemModule.Scripts.Windows;
using Global.StateMachinesModule.Scripts;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

public class MouseVisibilityInGameStartSystem : IInitializable, IDisposable, ITickable
{
	private readonly MouseVisibilityModel _mouseVisibilityModel;

	private readonly GameFlowStateMachine _gameFlowStateMachine;

	private readonly IInputService _inputService;

	private readonly IWindowsService _windowsService;

	private readonly CameraModel _cameraModel;

	private readonly List<Type> _mouseVisibleWindow = new List<Type>();

	private readonly MouseVisibilityRequest _performedMouseVisibilityRequest = new MouseVisibilityRequest(2, isMouseVisible: true);

	private readonly MouseVisibilityRequest _settingsMouseVisibilityRequest = new MouseVisibilityRequest(1, isMouseVisible: true);

	private bool _targetCursorVisible;

	private CursorLockMode _targetLockMode;

	[Inject]
	public MouseVisibilityInGameStartSystem(MouseVisibilityModel mouseVisibilityModel, GameFlowStateMachine gameFlowStateMachine, IInputService inputService, IWindowsService windowsService, CameraModel cameraModel)
	{
		_windowsService = windowsService;
		_cameraModel = cameraModel;
		_inputService = inputService;
		_gameFlowStateMachine = gameFlowStateMachine;
		_mouseVisibilityModel = mouseVisibilityModel;
	}

	public void Initialize()
	{
		InitializeMouseVisibleWindows();
		if (_gameFlowStateMachine.CurrentState == GameFlowState.SessionGameState)
		{
			StartTrackGameFocusing();
		}
		else
		{
			_gameFlowStateMachine.OnStateEnter += HandleGameStateChanged;
		}
		MouseVisibilityModel mouseVisibilityModel = _mouseVisibilityModel;
		mouseVisibilityModel.OnMouseVisibilityChanged = (Action)Delegate.Combine(mouseVisibilityModel.OnMouseVisibilityChanged, new Action(HandleMouseVisibilityChanged));
		HandleMouseVisibilityChanged();
	}

	public void Dispose()
	{
		_gameFlowStateMachine.OnStateEnter -= HandleGameStateChanged;
		MouseVisibilityModel mouseVisibilityModel = _mouseVisibilityModel;
		mouseVisibilityModel.OnMouseVisibilityChanged = (Action)Delegate.Remove(mouseVisibilityModel.OnMouseVisibilityChanged, new Action(HandleMouseVisibilityChanged));
		InputDefaultActions changeMouseVisibility = _inputService.ChangeMouseVisibility;
		changeMouseVisibility.Performed = (Action)Delegate.Remove(changeMouseVisibility.Performed, new Action(PerformedMouseVisibilityChange));
	}

	public void Tick()
	{
		if (Cursor.visible != _targetCursorVisible || Cursor.lockState != _targetLockMode)
		{
			ApplyCursorState();
		}
		if (_mouseVisibilityModel.IsMouseVisibleBlocked && Cursor.visible != _mouseVisibilityModel.IsMouseVisibleOnBlocked)
		{
			ApplyBlockedCursorState();
		}
	}

	private void HandleGameStateChanged(GameFlowState state)
	{
		if (state == GameFlowState.SessionGameState)
		{
			StartTrackGameFocusing();
			return;
		}
		_mouseVisibilityModel.AddMouseVisibilityRequest(new MouseVisibilityRequest(0, isMouseVisible: true));
		StopTrackGameFocusing();
	}

	private void StartTrackGameFocusing()
	{
		StopTrackGameFocusing();
		_windowsService.OnWindowOpened += OnWindowOpened;
		_windowsService.OnWindowClosed += OnWindowClosed;
		InputDefaultActions changeMouseVisibility = _inputService.ChangeMouseVisibility;
		changeMouseVisibility.Performed = (Action)Delegate.Combine(changeMouseVisibility.Performed, new Action(PerformedMouseVisibilityChange));
	}

	private void StopTrackGameFocusing()
	{
		_windowsService.OnWindowOpened -= OnWindowOpened;
		_windowsService.OnWindowClosed -= OnWindowClosed;
		InputDefaultActions changeMouseVisibility = _inputService.ChangeMouseVisibility;
		changeMouseVisibility.Performed = (Action)Delegate.Remove(changeMouseVisibility.Performed, new Action(PerformedMouseVisibilityChange));
	}

	private void PerformedMouseVisibilityChange()
	{
		if (Debug.isDebugBuild || Application.isEditor)
		{
			if (_mouseVisibilityModel.IsMouseVisibilityRequestsContains(_performedMouseVisibilityRequest))
			{
				_mouseVisibilityModel.RemoveMouseVisibilityRequest(_performedMouseVisibilityRequest);
				_cameraModel.RemoveCameraInputLockReason(LockCameraInputReasonEnum.DebugMouseVisibility);
			}
			else
			{
				_mouseVisibilityModel.AddMouseVisibilityRequest(_performedMouseVisibilityRequest);
				_cameraModel.AddCameraInputLockReason(LockCameraInputReasonEnum.DebugMouseVisibility);
			}
		}
	}

	private void OnWindowOpened(Type windowType)
	{
		if (IsWindowMouseVisible(windowType))
		{
			_mouseVisibilityModel.AddMouseVisibilityRequest(_settingsMouseVisibilityRequest);
		}
	}

	private void OnWindowClosed(Type windowType)
	{
		if (IsWindowMouseVisible(windowType))
		{
			_mouseVisibilityModel.RemoveMouseVisibilityRequest(_settingsMouseVisibilityRequest);
		}
	}

	private void HandleMouseVisibilityChanged()
	{
		MouseVisibilityRequest priorityMouseVisibility = _mouseVisibilityModel.PriorityMouseVisibility;
		_targetCursorVisible = priorityMouseVisibility.IsMouseVisible;
		_targetLockMode = ((!priorityMouseVisibility.IsMouseVisible) ? CursorLockMode.Locked : CursorLockMode.None);
		ApplyCursorState();
	}

	private void ApplyCursorState()
	{
		if (!_mouseVisibilityModel.IsMouseVisibleBlocked)
		{
			Cursor.lockState = _targetLockMode;
			Cursor.visible = _targetCursorVisible;
		}
	}

	private void ApplyBlockedCursorState()
	{
		Cursor.lockState = ((!_mouseVisibilityModel.IsMouseVisibleOnBlocked) ? CursorLockMode.Locked : CursorLockMode.None);
		Cursor.visible = _mouseVisibilityModel.IsMouseVisibleOnBlocked;
	}

	private bool IsWindowMouseVisible(Type windowType)
	{
		return _mouseVisibleWindow.Contains(windowType);
	}

	private void InitializeMouseVisibleWindows()
	{
		_mouseVisibleWindow.Add(typeof(SettingsWindow));
		_mouseVisibleWindow.Add(typeof(GameOverWindow));
		_mouseVisibleWindow.Add(typeof(QuotaCompletedWindow));
	}
}
