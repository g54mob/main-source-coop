using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Features.CameraModelModule;
using Features.GamePauseModule.Scripts;
using Features.InputModule.Scripts.Generated;
using Features.MouseVisibilityModule.Scripts;
using Features.SettingsMenuModule.Scripts;
using Features.UserReport.Adapter;
using Features.UserReport.CustomUserReporting.Scripts.Client;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.UserReport.CustomUserReporting.Scripts.Default
{
	public class UserReportService : IUserReportService
	{
		private readonly SettingsWindow _settingsWindow;

		private readonly IInputService _inputService;

		private readonly MouseVisibilityModel _mouseVisibilityModel;

		private readonly GameGlobalNetworkingPause _gameGlobalNetworkingPause;

		private readonly CameraModel _cameraModel;

		private UserReportingScript _userReportingScript;

		private bool _wasPauseMenuShown;

		public UserReportService(SettingsWindow settingsWindow, IInputService inputService, MouseVisibilityModel mouseVisibilityModel, GameGlobalNetworkingPause gameGlobalNetworkingPause, CameraModel cameraModel)
		{
			_settingsWindow = settingsWindow;
			_inputService = inputService;
			_mouseVisibilityModel = mouseVisibilityModel;
			_gameGlobalNetworkingPause = gameGlobalNetworkingPause;
			_cameraModel = cameraModel;
		}

		public void OpenUserReport()
		{
			_mouseVisibilityModel.IsMouseVisibleOnBlocked = true;
			_mouseVisibilityModel.IsMouseVisibleBlocked = true;
			_cameraModel.AddCameraInputLockReason(LockCameraInputReasonEnum.UserReport);
			_inputService.DisableUI();
			DisablePauseMenu();
			ICollection<UserReportNamedValue> inGameData;
			try
			{
				inGameData = GenerateInGameData();
			}
			catch (Exception arg)
			{
				Debug.LogError($"Error while generating user report data: {arg}. Report will be created but without additional game data");
				CreateReport();
				return;
			}
			_gameGlobalNetworkingPause.SetLocalPaused(isPaused: true);
			CreateReportWithInGameData(inGameData);
		}

		private ICollection<UserReportNamedValue> GenerateInGameData()
		{
			return new Collection<UserReportNamedValue>();
		}

		private void DisablePauseMenu()
		{
			try
			{
				SettingsWindow settingsWindow = _settingsWindow;
				if (settingsWindow != null && settingsWindow.WindowStatus == WindowStatus.Showed)
				{
					_wasPauseMenuShown = true;
					_settingsWindow.Hide();
				}
			}
			catch (Exception arg)
			{
				Debug.LogError($"Caught exception while trying to hide pause menu during report generation: {arg}");
			}
		}

		private void CreateReport()
		{
			_userReportingScript = UnityEngine.Object.FindObjectOfType<UserReportingScript>();
			if (!(_userReportingScript == null))
			{
				_userReportingScript.CreateUserReport();
				_userReportingScript.OnReportClosed += ReturnToNormalState;
			}
		}

		private void CreateReportWithInGameData(ICollection<UserReportNamedValue> inGameData)
		{
			_userReportingScript = UnityEngine.Object.FindObjectOfType<UserReportingScript>();
			if (!(_userReportingScript == null))
			{
				_userReportingScript.CreateUserReportWithAdditionalText(inGameData);
				_userReportingScript.OnReportClosed += ReturnToNormalState;
			}
		}

		private void ReturnToNormalState()
		{
			_gameGlobalNetworkingPause.SetLocalPaused(isPaused: false);
			_mouseVisibilityModel.IsMouseVisibleOnBlocked = false;
			_mouseVisibilityModel.IsMouseVisibleBlocked = false;
			_cameraModel.RemoveCameraInputLockReason(LockCameraInputReasonEnum.UserReport);
			_inputService.EnableUI();
			_userReportingScript.OnReportClosed -= ReturnToNormalState;
			if (_wasPauseMenuShown)
			{
				_settingsWindow.Show();
			}
			_wasPauseMenuShown = false;
		}
	}
}
