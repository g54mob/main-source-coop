using System;
using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Extensions;
using EvilCore.Managers;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Player;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

namespace EvilCore.Recording
{
	public class DirectorManager : MonoBehaviour
	{
		[SerializeField]
		private KeyCode toggleKey = KeyCode.F12;

		[SerializeField]
		private bool requireControl = true;

		[SerializeField]
		private float defaultBlendDuration = 1f;

		private DirectorState _state;

		private CinemachineBrain _brain;

		private readonly List<DirectorCameraSetup> _cameras = new List<DirectorCameraSetup>();

		private DirectorCameraSetup _activeCamera;

		private int _activeCameraIndex = -1;

		private int _cameraCounter;

		private DirectorTimeController _timeController;

		private DirectorFreeCameraController _freeCamera;

		private DirectorUIController _uiController;

		private DirectorHiddenControls _hiddenControls;

		private Camera _mainCamera;

		private bool _wasPlayerSitting;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private IGameUIManager _gameUIManager;

		private DirectorSequencer _sequencer;

		public DirectorState State => _state;

		public bool IsActive => _state != DirectorState.Inactive;

		public IReadOnlyList<DirectorCameraSetup> Cameras => _cameras;

		public DirectorCameraSetup ActiveCamera => _activeCamera;

		public int ActiveCameraIndex => _activeCameraIndex;

		public DirectorTimeController TimeController => _timeController;

		public DirectorSequencer Sequencer => _sequencer;

		public DirectorHiddenControls HiddenControls => _hiddenControls;

		public event Action OnDirectorModeEntered;

		public event Action OnDirectorModeExited;

		public event Action<DirectorCameraSetup> OnCameraActivated;

		public event Action OnCameraListChanged;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
			_timeController = new DirectorTimeController();
			_sequencer = new DirectorSequencer();
			_hiddenControls = new DirectorHiddenControls(this);
		}

		private void Update()
		{
			if (ShouldToggle())
			{
				ToggleDirectorMode();
			}
			if (_state == DirectorState.Inactive)
			{
				return;
			}
			if (Input.GetKeyDown(KeyCode.H))
			{
				_hiddenControls.Reset();
				_uiController?.TogglePanelVisibility();
				if (_uiController != null && _uiController.IsPanelHidden)
				{
					HideGameHUD();
					GameCursor.Lock();
				}
				else
				{
					HideGameHUD();
					GameCursor.Unlock();
				}
			}
			if (_uiController != null && _uiController.IsPanelHidden)
			{
				_hiddenControls.HandleInput();
			}
			if (_activeCamera != null && _activeCamera.IsPositioning)
			{
				_activeCamera.UpdatePositioning();
				return;
			}
			bool flag = _uiController != null && _uiController.IsPanelHidden;
			if (_state == DirectorState.Sequencing)
			{
				_sequencer.Update(Time.unscaledDeltaTime);
			}
			if (_activeCamera != null && (!_activeCamera.ExecuteOnHide || flag))
			{
				_activeCamera.UpdateRuntime(Time.unscaledDeltaTime);
			}
		}

		private bool ShouldToggle()
		{
			if (requireControl && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
			{
				return false;
			}
			return Input.GetKeyDown(toggleKey);
		}

		public void ToggleDirectorMode()
		{
			if (_state == DirectorState.Inactive)
			{
				EnterDirectorMode();
			}
			else
			{
				ExitDirectorMode();
			}
		}

		public void EnterDirectorMode()
		{
			if (_state != DirectorState.Inactive || !_playerService.IsPlayerSpawned)
			{
				return;
			}
			_mainCamera = _playerService.CameraTransform.GetComponentInParent<Camera>();
			if (_mainCamera == null)
			{
				EvilLogger.LogError("[DirectorManager] Cannot find main camera", "EnterDirectorMode", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Recording\\Director\\DirectorManager.cs", 135);
				return;
			}
			FreezePlayer();
			HideGameHUD();
			_brain = _mainCamera.gameObject.GetComponent<CinemachineBrain>();
			if (_brain == null)
			{
				_brain = _mainCamera.gameObject.AddComponent<CinemachineBrain>();
			}
			_brain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.EaseInOut, defaultBlendDuration);
			EnsureUIController();
			_uiController.Show(this);
			GameCursor.Unlock();
			_state = DirectorState.Active;
			_timeController.Enter();
			this.OnDirectorModeEntered?.Invoke();
		}

		public void ExitDirectorMode()
		{
			if (_state == DirectorState.Inactive)
			{
				return;
			}
			foreach (DirectorCameraSetup camera in _cameras)
			{
				camera.Deactivate();
			}
			_activeCamera = null;
			_activeCameraIndex = -1;
			_uiController?.Hide();
			if (_brain != null)
			{
				UnityEngine.Object.Destroy(_brain);
				_brain = null;
			}
			_timeController.Exit();
			UnfreezePlayer();
			RestoreGameHUD();
			GameCursor.Lock();
			_state = DirectorState.Inactive;
			this.OnDirectorModeExited?.Invoke();
		}

		public DirectorCameraSetup AddCamera(string name = null)
		{
			_cameraCounter++;
			string text = name ?? $"Camera {_cameraCounter}";
			GameObject gameObject = new GameObject("DirectorCam_" + text);
			gameObject.transform.SetParent(base.transform);
			if (_playerService != null && _playerService.IsPlayerSpawned && _playerService.LocalPlayer != null)
			{
				Transform transform = _playerService.LocalPlayer.transform;
				gameObject.transform.position = transform.position;
				gameObject.transform.rotation = transform.rotation;
			}
			else if (_mainCamera != null)
			{
				gameObject.transform.position = _mainCamera.transform.position;
				gameObject.transform.rotation = _mainCamera.transform.rotation;
			}
			DirectorCameraSetup directorCameraSetup = gameObject.AddComponent<DirectorCameraSetup>();
			directorCameraSetup.Initialize(text);
			_cameras.Add(directorCameraSetup);
			this.OnCameraListChanged?.Invoke();
			return directorCameraSetup;
		}

		public void RemoveCamera(int index)
		{
			if (index >= 0 && index < _cameras.Count)
			{
				DirectorCameraSetup directorCameraSetup = _cameras[index];
				if (_activeCamera == directorCameraSetup)
				{
					_activeCamera = null;
					_activeCameraIndex = -1;
				}
				_cameras.RemoveAt(index);
				directorCameraSetup.Cleanup();
				UnityEngine.Object.Destroy(directorCameraSetup.gameObject);
				if (_activeCameraIndex >= _cameras.Count)
				{
					_activeCameraIndex = _cameras.Count - 1;
				}
				this.OnCameraListChanged?.Invoke();
			}
		}

		public void ActivateCamera(int index)
		{
			if (index >= 0 && index < _cameras.Count && _state != DirectorState.Inactive)
			{
				if (_activeCamera != null)
				{
					_activeCamera.Deactivate();
				}
				_activeCameraIndex = index;
				_activeCamera = _cameras[index];
				if (_activeCamera.TransitionType == TransitionType.Cut && _brain != null)
				{
					_brain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, 0f);
				}
				else if (_brain != null)
				{
					_brain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.EaseInOut, _activeCamera.BlendDuration);
				}
				_activeCamera.Activate();
				this.OnCameraActivated?.Invoke(_activeCamera);
			}
		}

		public void NextCamera()
		{
			if (_cameras.Count != 0)
			{
				int index = (_activeCameraIndex + 1) % _cameras.Count;
				ActivateCamera(index);
			}
		}

		public void PreviousCamera()
		{
			if (_cameras.Count != 0)
			{
				int index = ((_activeCameraIndex <= 0) ? (_cameras.Count - 1) : (_activeCameraIndex - 1));
				ActivateCamera(index);
			}
		}

		public void StartSequence()
		{
			if (_cameras.Count >= 2)
			{
				_sequencer.BuildFromCameraList(_cameras);
				_sequencer.Play(OnSequenceCameraSwitch, OnSequenceComplete);
				_state = DirectorState.Sequencing;
			}
		}

		public void StopSequence()
		{
			_sequencer.Stop();
			_state = DirectorState.Active;
		}

		public void PauseSequence()
		{
			_sequencer.Pause();
		}

		public void ResumeSequence()
		{
			_sequencer.Resume();
		}

		private void OnSequenceCameraSwitch(int cameraIndex, TransitionType transition, float blendDuration)
		{
			if (_activeCamera != null)
			{
				_activeCamera.Deactivate();
			}
			if (_brain != null)
			{
				_brain.DefaultBlend = ((transition == TransitionType.Cut) ? new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, 0f) : new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.EaseInOut, blendDuration));
			}
			_activeCameraIndex = cameraIndex;
			_activeCamera = _cameras[cameraIndex];
			_activeCamera.Activate();
			this.OnCameraActivated?.Invoke(_activeCamera);
		}

		private void OnSequenceComplete()
		{
			_state = DirectorState.Active;
		}

		public void SetTimeScale(float scale)
		{
			_timeController.SetTimeScale(scale);
		}

		public DirectorPresetData ExportPreset(string presetName)
		{
			DirectorPresetData directorPresetData = new DirectorPresetData
			{
				presetName = presetName
			};
			foreach (DirectorCameraSetup camera in _cameras)
			{
				directorPresetData.cameras.Add(camera.GetSettings());
			}
			return directorPresetData;
		}

		public void ImportPreset(DirectorPresetData preset)
		{
			ClearAllCameras();
			foreach (DirectorCameraData camera in preset.cameras)
			{
				AddCamera(camera.cameraName).ApplySettings(camera);
			}
			if (_cameras.Count > 0)
			{
				ActivateCamera(0);
			}
		}

		public void ClearAllCameras()
		{
			for (int num = _cameras.Count - 1; num >= 0; num--)
			{
				DirectorCameraSetup directorCameraSetup = _cameras[num];
				directorCameraSetup.Cleanup();
				UnityEngine.Object.Destroy(directorCameraSetup.gameObject);
			}
			_cameras.Clear();
			_activeCamera = null;
			_activeCameraIndex = -1;
			this.OnCameraListChanged?.Invoke();
		}

		private void FreezePlayer()
		{
			if (_playerService.IsPlayerSpawned)
			{
				if (_playerService.TryGetFirstPersonController(out var controller))
				{
					_wasPlayerSitting = controller.IsSitting;
					controller.DisableMovement();
					controller.DisableCameraRotate();
					controller.DisableCharacterRotate();
					controller.DisableJump();
					controller.DisableCrouch();
					controller.DisableSprint();
					controller.DisableZoom();
				}
				if (_playerService.TryGetInteractionManager(out var manager))
				{
					manager.DisableInteraction();
				}
			}
		}

		private void UnfreezePlayer()
		{
			if (!_playerService.IsPlayerSpawned)
			{
				return;
			}
			if (_playerService.TryGetFirstPersonController(out var controller))
			{
				controller.EnableCameraRotate();
				controller.EnableZoom();
				if (!_wasPlayerSitting)
				{
					controller.EnableMovement();
					controller.EnableCharacterRotate();
					controller.EnableJump();
					controller.EnableCrouch();
					controller.EnableSprint();
				}
			}
			if (_playerService.TryGetInteractionManager(out var manager))
			{
				manager.EnableInteraction();
			}
		}

		private void HideGameHUD()
		{
			_gameUIManager?.HideCanvasGroup(GameCanvasGroupName.Crosshair);
			_gameUIManager?.HideCanvasGroup(GameCanvasGroupName.Game);
			_gameUIManager?.HideCanvasGroup(GameCanvasGroupName.Interaction);
			_gameUIManager?.HideCanvasGroup(GameCanvasGroupName.InfoMessage);
			_gameUIManager?.HideCanvasGroup(GameCanvasGroupName.InputActionPrompts);
			_gameUIManager?.HideCanvasGroup(GameCanvasGroupName.LiquidContainerInfo);
			_gameUIManager?.HideCanvasGroup(GameCanvasGroupName.AttachableObjectInfo);
			_gameUIManager?.HideCanvasGroup(GameCanvasGroupName.StandupActionPrompt);
			_gameUIManager?.HideCanvasGroup(GameCanvasGroupName.EquippedItemBatteryInfo);
		}

		private void RestoreGameHUD()
		{
			_gameUIManager?.SetCanvasVisibilityForGameStart();
		}

		private void EnsureUIController()
		{
			if (!(_uiController != null))
			{
				_uiController = GetComponentInChildren<DirectorUIController>();
				if (!(_uiController != null))
				{
					GameObject gameObject = new GameObject("DirectorUI");
					gameObject.transform.SetParent(base.transform);
					_uiController = gameObject.AddComponent<DirectorUIController>();
				}
			}
		}

		private void OnDestroy()
		{
			if (IsActive)
			{
				ExitDirectorMode();
			}
		}
	}
}
