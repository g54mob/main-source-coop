using System;
using Cysharp.Threading.Tasks;
using Features.GrabModule.Scripts.PhysGrab;
using Features.InputModule.Scripts.Generated;
using Features.LineArmModule.Scripts.Data;
using Features.SceneTransitionsModule.Scripts.LoadingScreen;
using Features.SettingsMenuModule.Scripts.Data;
using Fusion;
using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Features.LineArmModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class LineArmControllerByMouse : LineArmControllerBase
	{
		[SerializeField]
		private NetworkBehaviour _physGrabberPrefab;

		[SerializeField]
		private float _physGrabberByMouseYThreshold;

		[SerializeField]
		private LayerMask _mouseRaycastLayerMask;

		[SerializeField]
		private float _physGrabberByMouseForwardThreshold;

		[SerializeField]
		private float _centerOffsetByMouseForwardThreshold;

		[SerializeField]
		private float _physGrabberByMouseRightThreshold;

		[SerializeField]
		private float _physGrabberMinPlanarRange;

		[SerializeField]
		private float _physGrabberMaxPlanarRange;

		private CurrentSettingsModel _currentSettingsModel;

		private LineArmConfiguration _lineArmConfiguration;

		private VirtualCursorModel _virtualCursorModel;

		private IInputDeviceService _deviceService;

		private IInputService _inputService;

		private PhysGrabber _physGrabberInstance;

		private bool _isMovementEnabled = true;

		private bool _isAwaitingFadeOutForMovement;

		private Vector2 _virtualCursorPosition;

		private bool _isVirtualCursorInitialized;

		private Vector2 _rotationInput;

		private TableCenterModel _tableCenterModel;

		private bool _isSpawned;

		private LoadingScreenModel _loadingScreenModel;

		[WeaverGenerated]
		[DefaultForProperty("PhysGrabberNetworkId", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _PhysGrabberNetworkId;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe NetworkId PhysGrabberNetworkId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LineArmControllerByMouse.PhysGrabberNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LineArmControllerByMouse.PhysGrabberNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)((byte*)Ptr + 0) = value;
			}
		}

		public override PhysGrabber PhysGrabber
		{
			get
			{
				EnsureGrabberResolved();
				return _physGrabberInstance;
			}
		}

		public override LineArmType ArmType => LineArmType.RightArmByMouse;

		[Inject]
		private void InjectDeviceService(IInputDeviceService deviceService, IInputService inputService, CurrentSettingsModel currentSettingsModel, LineArmConfiguration lineArmConfiguration, VirtualCursorModel virtualCursorModel, TableCenterModel tableCenterModel, LoadingScreenModel loadingScreenModel)
		{
			_deviceService = deviceService;
			_inputService = inputService;
			_currentSettingsModel = currentSettingsModel;
			_lineArmConfiguration = lineArmConfiguration;
			_virtualCursorModel = virtualCursorModel;
			_tableCenterModel = tableCenterModel;
			_loadingScreenModel = loadingScreenModel;
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			SubscribeToRotationInput();
			if (base.HasStateAuthority)
			{
				SyncMovementWithFadeState();
			}
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			UnsubscribeFromRotationInput();
			_loadingScreenModel.OnEndedFadeOut -= EnableMovement;
			_isAwaitingFadeOutForMovement = false;
			_virtualCursorModel?.SetState(isVirtualCursorActive: false, GetDefaultVirtualCursorPosition());
		}

		private void SyncMovementWithFadeState()
		{
			if (_loadingScreenModel.IsVisible)
			{
				_isMovementEnabled = false;
				WaitForFadeOutToEnableMovement();
			}
			else
			{
				_isMovementEnabled = _physGrabberInstance != null;
			}
		}

		private void WaitForFadeOutToEnableMovement()
		{
			if (!_isAwaitingFadeOutForMovement)
			{
				_isAwaitingFadeOutForMovement = true;
				_loadingScreenModel.OnEndedFadeOut += EnableMovement;
			}
		}

		private void EnableMovement()
		{
			_loadingScreenModel.OnEndedFadeOut -= EnableMovement;
			_isAwaitingFadeOutForMovement = false;
			_isMovementEnabled = true;
		}

		public override async void Spawned()
		{
			base.Spawned();
			_isSpawned = true;
			if (base.HasStateAuthority)
			{
				await EnsurePhysGrabberSpawnedAsync();
			}
		}

		private void EnsureGrabberResolved()
		{
			if (!(_physGrabberInstance != null) && _isSpawned && !(base.Runner == null) && base.Runner.IsRunning && !(PhysGrabberNetworkId == default(NetworkId)) && base.Runner.TryFindObject(PhysGrabberNetworkId, out var networkObject))
			{
				_physGrabberInstance = networkObject.GetComponent<PhysGrabber>();
				ProcessStatsInitialization();
				SyncMovementWithFadeState();
			}
		}

		public override void OnPlayerRebound(PlayerRef playerRef, NetworkObject avatar)
		{
			base.OnPlayerRebound(playerRef, avatar);
			if (playerRef.PlayerId == base.Object.InputAuthority.PlayerId)
			{
				if (avatar == null)
				{
					_physGrabberInstance = null;
					_isMovementEnabled = false;
				}
				else
				{
					EnsurePhysGrabberSpawnedAsync().Forget();
				}
			}
		}

		private async UniTask EnsurePhysGrabberSpawnedAsync()
		{
			if (!base.HasStateAuthority || _physGrabberInstance != null || base.Runner == null || !base.Runner.IsRunning)
			{
				return;
			}
			NetworkRunner runner = base.Runner;
			NetworkObject networkObject = await runner.SpawnAsync(_physGrabberPrefab, base.transform.position, Quaternion.identity);
			if (base.Object == null || !base.Object.IsValid)
			{
				if (networkObject != null && networkObject.IsValid && runner.IsRunning)
				{
					runner.Despawn(networkObject);
				}
			}
			else
			{
				_physGrabberInstance = networkObject.GetComponent<PhysGrabber>();
				PhysGrabberNetworkId = networkObject.Id;
				ProcessStatsInitialization();
				SyncMovementWithFadeState();
			}
		}

		public override void FixedUpdateNetwork()
		{
			EnsureGrabberResolved();
			if (!_isGrabEnabled || !_isMovementEnabled)
			{
				DisableLastOutline();
				if (_physGrabberInstance != null)
				{
					_physGrabberInstance.transform.position = base.transform.position;
				}
				return;
			}
			if (_physGrabberInstance == null)
			{
				DisableLastOutline();
				return;
			}
			base.FixedUpdateNetwork();
			if (Physics.Raycast(_cameraModel.CameraObject.ScreenPointToRay(GetCursorScreenPosition()), out var hitInfo, 20f, _mouseRaycastLayerMask))
			{
				Vector3 vector = hitInfo.point + Vector3.up * _physGrabberByMouseYThreshold + base.transform.forward * _physGrabberByMouseForwardThreshold + base.transform.right * _physGrabberByMouseRightThreshold;
				_physGrabberInstance.transform.position = ((_currentGrabbables.Count > 0) ? ClampPhysGrabberToPlanarRange(vector) : vector);
			}
		}

		private Vector3 ClampPhysGrabberToPlanarRange(Vector3 worldPosition)
		{
			Transform transform = base.transform;
			if (_tableCenterModel != null && _tableCenterModel.TryGetCenter(out var center))
			{
				transform = center;
			}
			Vector3 vector = transform.position - base.transform.forward * _centerOffsetByMouseForwardThreshold;
			float num = Mathf.Min(_physGrabberMinPlanarRange, _physGrabberMaxPlanarRange);
			float num2 = Mathf.Max(_physGrabberMinPlanarRange, _physGrabberMaxPlanarRange);
			if (num2 <= 0f)
			{
				return worldPosition;
			}
			Vector3 vector2 = new Vector3(worldPosition.x - vector.x, 0f, worldPosition.z - vector.z);
			float magnitude = vector2.magnitude;
			if (magnitude < Mathf.Epsilon)
			{
				Vector3 vector3 = Vector3.zero;
				if (_physGrabberInstance != null)
				{
					vector3 = new Vector3(_physGrabberInstance.transform.position.x - vector.x, 0f, _physGrabberInstance.transform.position.z - vector.z);
				}
				if (vector3.sqrMagnitude < Mathf.Epsilon)
				{
					vector3 = new Vector3(transform.forward.x, 0f, transform.forward.z);
				}
				vector2 = vector3.normalized * num;
			}
			else if (magnitude < num || magnitude > num2)
			{
				vector2 = vector2 / magnitude * Mathf.Clamp(magnitude, num, num2);
			}
			return new Vector3(vector.x + vector2.x, worldPosition.y, vector.z + vector2.z);
		}

		protected override Ray GetRayByRaycastType()
		{
			return _cameraModel.CameraObject.ScreenPointToRay(GetCursorScreenPosition());
		}

		private Vector2 GetCursorScreenPosition()
		{
			if (!base.Object.HasInputAuthority)
			{
				return Input.mousePosition;
			}
			if (!IsGamepadCursorMode())
			{
				Vector2 vector = Input.mousePosition;
				_virtualCursorModel?.SetState(isVirtualCursorActive: false, vector);
				return vector;
			}
			InitializeVirtualCursorIfNeeded();
			UpdateVirtualCursorByGamepadInput();
			_virtualCursorModel?.SetState(isVirtualCursorActive: true, _virtualCursorPosition);
			return _virtualCursorPosition;
		}

		private bool IsGamepadCursorMode()
		{
			if (_deviceService != null)
			{
				if (!_deviceService.IsCurrentActiveDeviceGamepad())
				{
					return _deviceService.IsCurrentActiveDeviceJoystick();
				}
				return true;
			}
			return false;
		}

		private void InitializeVirtualCursorIfNeeded()
		{
			if (!_isVirtualCursorInitialized)
			{
				_virtualCursorPosition = GetDefaultVirtualCursorPosition();
				_virtualCursorPosition = ClampVirtualCursorToScreen(_virtualCursorPosition);
				_isVirtualCursorInitialized = true;
			}
		}

		private void UpdateVirtualCursorByGamepadInput()
		{
			if (!(_rotationInput.sqrMagnitude < 0.0001f))
			{
				float num = Mathf.Max(0.01f, _currentSettingsModel.MouseSensitivity / 100f);
				float num2 = _lineArmConfiguration.VirtualCursorBaseSpeed * _lineArmConfiguration.VirtualCursorSensitivityMultiplier * num;
				Vector2 vector = _rotationInput * (num2 * Time.deltaTime);
				vector = Vector2.ClampMagnitude(vector, _lineArmConfiguration.VirtualCursorMaxStepPerFrame);
				_virtualCursorPosition += vector;
				_virtualCursorPosition = ClampVirtualCursorToScreen(_virtualCursorPosition);
			}
		}

		private Vector2 ClampVirtualCursorToScreen(Vector2 cursorPosition)
		{
			Vector2 virtualCursorScreenPadding = _lineArmConfiguration.VirtualCursorScreenPadding;
			float num = Mathf.Max(0f, virtualCursorScreenPadding.x);
			float max = Mathf.Max(num, (float)Screen.width - virtualCursorScreenPadding.x);
			float num2 = Mathf.Max(0f, virtualCursorScreenPadding.y);
			float max2 = Mathf.Max(num2, (float)Screen.height - virtualCursorScreenPadding.y);
			return new Vector2(Mathf.Clamp(cursorPosition.x, num, max), Mathf.Clamp(cursorPosition.y, num2, max2));
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_isSpawned = false;
			UnsubscribeFromRotationInput();
			if (base.HasStateAuthority && _physGrabberInstance != null && _physGrabberInstance.Object != null && _physGrabberInstance.Object.IsValid && runner != null && runner.IsRunning)
			{
				runner.Despawn(_physGrabberInstance.Object);
			}
			_physGrabberInstance = null;
		}

		private void SubscribeToRotationInput()
		{
			InputVector2Actions rotation = _inputService.Rotation;
			rotation.VectorChangedWithDeviceCallbackPerformed = (Action<InputDevice, Vector2>)Delegate.Combine(rotation.VectorChangedWithDeviceCallbackPerformed, new Action<InputDevice, Vector2>(OnRotationChanged));
			InputVector2Actions rotation2 = _inputService.Rotation;
			rotation2.VectorChangedWithDeviceCallbackCanceled = (Action<InputDevice, Vector2>)Delegate.Combine(rotation2.VectorChangedWithDeviceCallbackCanceled, new Action<InputDevice, Vector2>(OnRotationCanceled));
		}

		private void UnsubscribeFromRotationInput()
		{
			InputVector2Actions rotation = _inputService.Rotation;
			rotation.VectorChangedWithDeviceCallbackPerformed = (Action<InputDevice, Vector2>)Delegate.Remove(rotation.VectorChangedWithDeviceCallbackPerformed, new Action<InputDevice, Vector2>(OnRotationChanged));
			InputVector2Actions rotation2 = _inputService.Rotation;
			rotation2.VectorChangedWithDeviceCallbackCanceled = (Action<InputDevice, Vector2>)Delegate.Remove(rotation2.VectorChangedWithDeviceCallbackCanceled, new Action<InputDevice, Vector2>(OnRotationCanceled));
		}

		private void OnRotationChanged(InputDevice inputDevice, Vector2 rotation)
		{
			_rotationInput = (_deviceService.IsDeviceController(inputDevice) ? rotation : Vector2.zero);
		}

		private void OnRotationCanceled(InputDevice inputDevice, Vector2 rotation)
		{
			if (_deviceService.IsDeviceController(inputDevice))
			{
				_rotationInput = Vector2.zero;
			}
		}

		private static Vector2 GetDefaultVirtualCursorPosition()
		{
			return new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			PhysGrabberNetworkId = _PhysGrabberNetworkId;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_PhysGrabberNetworkId = PhysGrabberNetworkId;
		}
	}
}
