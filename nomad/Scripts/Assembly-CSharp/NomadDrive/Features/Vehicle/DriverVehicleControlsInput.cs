using System.Collections.Generic;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Inputs;
using NomadDrive.Features.Vehicle.Interactables;
using NomadDrive.Features.Vehicle.Modules;
using NomadDrive.Features.Vehicle.UI;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Vehicle
{
	public class DriverVehicleControlsInput : MonoBehaviour
	{
		private const int Headlights = 0;

		private const int Wipers = 1;

		private const int Ignition = 2;

		private const int CabinLight = 3;

		private const int Handbrake = 4;

		private static readonly string[] InputIds = new string[7] { "Driving_Headlights", "Driving_Wipers", "Driving_Ignition", "Driving_CabinLight", "Driving_Handbrake", "Driving_Horn", "Driving_SwitchCamera" };

		[Tooltip("How often (seconds) the handbrake row's greyed/active state is re-checked while driving.")]
		[SerializeField]
		private float availabilityRefreshInterval = 0.25f;

		[Inject]
		private IGameUIManager _guiManager;

		[Inject]
		private UIFeedbackManager _uiFeedback;

		private DrivingActionsPanel _panel;

		private HeadlightButton _headlightButton;

		private WindshieldButton _windshieldButton;

		private readonly List<DriverCabinLight> _cabinLights = new List<DriverCabinLight>();

		private VehicleIgnitionModule _ignitionModule;

		private VehicleHandbrakeModule _handbrakeModule;

		private bool _driving;

		private float _availabilityTimer;

		private readonly bool[] _lastEnabled = new bool[InputIds.Length];

		private readonly List<DrivingActionEntry> _entryScratch = new List<DrivingActionEntry>();

		private UIFeedbackManager _feedbackFallback;

		public void BeginDriving(VehicleManager vehicle)
		{
			if (!(vehicle == null))
			{
				_headlightButton = vehicle.GetComponentInChildren<HeadlightButton>(includeInactive: true);
				_windshieldButton = vehicle.GetComponentInChildren<WindshieldButton>(includeInactive: true);
				_cabinLights.Clear();
				_cabinLights.AddRange(vehicle.GetComponentsInChildren<DriverCabinLight>(includeInactive: true));
				_ignitionModule = vehicle.GetModule<VehicleIgnitionModule>();
				_handbrakeModule = vehicle.GetModule<VehicleHandbrakeModule>();
				if (_panel == null)
				{
					_panel = Object.FindFirstObjectByType<DrivingActionsPanel>(FindObjectsInactive.Include);
				}
				_driving = true;
				_availabilityTimer = 0f;
				if (_guiManager != null)
				{
					_guiManager.OnMenuClosed -= HandleMenuClosed;
					_guiManager.OnMenuClosed += HandleMenuClosed;
				}
				_guiManager?.ShowCanvasGroup(GameCanvasGroupName.DrivingActions, interactable: false, blockRaycast: false);
				PushAvailability(force: true);
			}
		}

		public void EndDriving()
		{
			_driving = false;
			if (_guiManager != null)
			{
				_guiManager.OnMenuClosed -= HandleMenuClosed;
			}
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.DrivingActions);
			_headlightButton = null;
			_windshieldButton = null;
			_cabinLights.Clear();
			_ignitionModule = null;
			_handbrakeModule = null;
		}

		private void OnDisable()
		{
			if (_guiManager != null)
			{
				_guiManager.OnMenuClosed -= HandleMenuClosed;
			}
		}

		private void HandleMenuClosed()
		{
			if (_driving)
			{
				PushAvailability(force: true);
			}
		}

		private void Update()
		{
			if (_driving)
			{
				if (DrivingInputs.IsToggleHeadlightsButtonDown())
				{
					HandlePress(0);
				}
				if (DrivingInputs.IsToggleWipersButtonDown())
				{
					HandlePress(1);
				}
				if (DrivingInputs.IsToggleIgnitionButtonDown())
				{
					HandlePress(2);
				}
				if (DrivingInputs.IsToggleCabinLightButtonDown())
				{
					HandlePress(3);
				}
				if (DrivingInputs.IsToggleHandbrakeButtonDown())
				{
					HandlePress(4);
				}
				_availabilityTimer += Time.deltaTime;
				if (_availabilityTimer >= availabilityRefreshInterval)
				{
					_availabilityTimer = 0f;
					PushAvailability(force: false);
				}
			}
		}

		private void HandlePress(int index)
		{
			switch (index)
			{
			case 0:
				_headlightButton?.PerformPrimaryInteractionFromInput();
				break;
			case 1:
				_windshieldButton?.PerformPrimaryInteractionFromInput();
				break;
			case 2:
				_ignitionModule?.ToggleIgnitionFromInput();
				break;
			case 3:
			{
				foreach (DriverCabinLight cabinLight in _cabinLights)
				{
					if (cabinLight != null)
					{
						cabinLight.PerformPrimaryInteractionFromInput();
					}
				}
				break;
			}
			case 4:
				if (IsHandbrakeInstalled())
				{
					_handbrakeModule.InstalledHandbrake.ToggleFromInput();
				}
				else
				{
					Feedback()?.CreateFloatingMessage("@vehicle.handbrake_missing", FeedbackType.Warning);
				}
				break;
			}
		}

		private bool IsHandbrakeInstalled()
		{
			if (_handbrakeModule != null)
			{
				return _handbrakeModule.InstalledHandbrake != null;
			}
			return false;
		}

		private bool IsEnabled(int index)
		{
			if (index == 4)
			{
				return IsHandbrakeInstalled();
			}
			return true;
		}

		private void PushAvailability(bool force)
		{
			bool flag = force;
			for (int i = 0; i < _lastEnabled.Length; i++)
			{
				bool flag2 = IsEnabled(i);
				if (flag2 != _lastEnabled[i])
				{
					_lastEnabled[i] = flag2;
					flag = true;
				}
			}
			if (flag)
			{
				_entryScratch.Clear();
				for (int j = 0; j < InputIds.Length; j++)
				{
					_entryScratch.Add(new DrivingActionEntry(InputIds[j], _lastEnabled[j]));
				}
				_panel?.SetActions(_entryScratch);
			}
		}

		private UIFeedbackManager Feedback()
		{
			if (_uiFeedback != null)
			{
				return _uiFeedback;
			}
			if (_feedbackFallback == null)
			{
				_feedbackFallback = Object.FindFirstObjectByType<UIFeedbackManager>();
			}
			return _feedbackFallback;
		}
	}
}
