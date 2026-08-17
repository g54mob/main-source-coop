using System;
using ECM2;
using EvilCore;
using EvilCore.Audio;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking;
using EvilCore.Settings;
using NomadDrive.Features.Inputs;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.Player.Core;
using NomadDrive.Features.Player.PlayerStateMachine;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace NomadDrive.Features.Player
{
	[DefaultExecutionOrder(100)]
	public sealed class FirstPersonController : Character, IInitialize, IUniqueNetworkComponent, IPlayerComponent
	{
		public enum PlayerLookingCameraState
		{
			Standing = 0,
			Sitting = 1,
			Crouching = 2
		}

		[SerializeField]
		private PlayerStateManager _stateManager;

		[Header("Zoom Settings")]
		[SerializeField]
		private float _defaultFov;

		[SerializeField]
		private float _zoomedFov;

		[SerializeField]
		private float _zoomDuration;

		private Tween _fovTween;

		private bool _isZooming;

		private Action<float> _onFovChanged;

		private PlayerEyeZoomAnimator _eyeZoomAnimator;

		[SerializeField]
		private new Transform cameraTransform;

		[SerializeField]
		private Transform headFollowTransform;

		[Header("Eye Tracking")]
		[SerializeField]
		private Transform leftEye;

		[SerializeField]
		private Transform rightEye;

		private float _targetEyeHeight;

		private float _currentEyeHeight;

		private Vector3 _animRestLocalPos;

		private bool _animRestCaptured;

		private Vector3 _smoothAnimOffset;

		private Vector3 _animOffsetVelocity;

		private float _crouchBlend01;

		[Header("Mass Debuff")]
		[SerializeField]
		private MassDebuffSetting massDebuffSetting;

		[SerializeField]
		private SurfaceType currentSurfaceType;

		private Camera _playerCamera;

		private float _cameraPitch;

		private float _cameraYaw;

		private float _targetCameraPitch;

		private float _targetCameraYaw;

		[Inject]
		private IAudioManager _audioManager;

		[Inject]
		private ISettingsManager _settingsManager;

		private EquipmentManager _equipmentManagerRef;

		private ObjectPlacementManager _placementManagerRef;

		private bool _cameraDrivingSuspended;

		private int _groundLayerMask;

		[SerializeField]
		private PlayerLookingCameraState _playerLookingCameraState;

		public int SetupPriority => -10;

		[field: FormerlySerializedAs("<FirstPersonSettings>k__BackingField")]
		[field: FormerlySerializedAs("<FPSControllerSetting>k__BackingField")]
		[field: SerializeField]
		public FirstPersonControllerSettings FirstPersonControllerSettings { get; private set; }

		[field: SerializeField]
		private CameraRotateLimit cameraRotateLimit { get; set; } = CameraRotateLimit.Pitch;

		[field: SerializeField]
		private bool CanMove { get; set; } = true;

		[field: SerializeField]
		private bool CanCharacterRotate { get; set; } = true;

		[field: SerializeField]
		private bool CanZoom { get; set; } = true;

		[field: SerializeField]
		private new bool CanJump { get; set; } = true;

		[field: SerializeField]
		private bool CanCrouch { get; set; } = true;

		[field: SerializeField]
		private bool CanSprint { get; set; } = true;

		public PlayerState PlayerState => _stateManager?.CurrentState ?? PlayerState.Idle;

		public float FallGravityMagnitude
		{
			get
			{
				if (!(FirstPersonControllerSettings != null))
				{
					return Mathf.Abs(Physics.gravity.y);
				}
				return Mathf.Abs(FirstPersonControllerSettings.gravity);
			}
		}

		public bool IsSitting { get; set; }

		public float CurrentEyeHeight => _currentEyeHeight;

		public SurfaceType CurrentSurfaceType => currentSurfaceType;

		public bool IsActive { get; set; }

		public bool IsInitialized { get; set; }

		public Transform DrivenCameraTransform => cameraTransform;

		public bool InvertMouseY { get; set; }

		public event Action<Vector3> OnLandedImpact;

		public void SetupForPlayer(bool isLocalPlayer)
		{
			if (isLocalPlayer)
			{
				Init();
				IsActive = true;
				base.enabled = true;
				SetGravityScale(0f);
				DisableMovement();
				DisableCameraRotate();
			}
			else
			{
				IsActive = false;
				base.enabled = false;
			}
		}

		public void Init()
		{
			if (FirstPersonControllerSettings != null)
			{
				FirstPersonControllerSettings = UnityEngine.Object.Instantiate(FirstPersonControllerSettings);
			}
			_playerCamera = cameraTransform.GetComponent<Camera>();
			_playerCamera.nearClipPlane = FirstPersonControllerSettings.nearClipPlane;
			Cursor.lockState = CursorLockMode.Locked;
			_stateManager = new PlayerStateManager(FirstPersonControllerSettings);
			InitPlayerLookingCamera();
			base.maxWalkSpeed = FirstPersonControllerSettings.walkSpeed;
			base.crouchedHeight = FirstPersonControllerSettings.crouchedCapsuleHeight;
			base.unCrouchedHeight = FirstPersonControllerSettings.standingCapsuleHeight;
			base.characterMovement.SetHeight(FirstPersonControllerSettings.standingCapsuleHeight);
			base.maxWalkSpeedCrouched = FirstPersonControllerSettings.crouchedWalkSpeed;
			base.canJumpWhileCrouching = false;
			base.jumpMaxCount = 1;
			base.jumpImpulse = FirstPersonControllerSettings.jumpForce;
			base.gravity = new Vector3(0f, FirstPersonControllerSettings.gravity, 0f);
			base.mass = FirstPersonControllerSettings.playerMass;
			_onFovChanged = SetCameraFov;
			_defaultFov = FirstPersonControllerSettings.defaultFov;
			_zoomedFov = FirstPersonControllerSettings.zoomedFov;
			_zoomDuration = FirstPersonControllerSettings.zoomDuration;
			SetCameraFov(_defaultFov);
			_cameraPitch = (_targetCameraPitch = 0f);
			_cameraYaw = (_targetCameraYaw = 0f);
			_targetEyeHeight = FirstPersonControllerSettings.standingEyeHeight;
			_currentEyeHeight = _targetEyeHeight;
			_smoothAnimOffset = Vector3.zero;
			_animOffsetVelocity = Vector3.zero;
			_animRestCaptured = false;
			_eyeZoomAnimator = GetComponent<PlayerEyeZoomAnimator>();
			_equipmentManagerRef = GetComponent<EquipmentManager>();
			_placementManagerRef = GetComponent<ObjectPlacementManager>();
			if (_equipmentManagerRef != null)
			{
				_equipmentManagerRef.OnItemEquipped.AddListener(OnMassItemEquipped);
				_equipmentManagerRef.OnItemUnequipped.AddListener(OnMassItemUnequipped);
			}
			if (_placementManagerRef != null)
			{
				_placementManagerRef.OnPlacementEnter.AddListener(OnMassPlacementEnter);
				_placementManagerRef.OnPlacementExit.AddListener(OnMassPlacementExit);
			}
			if (_settingsManager != null)
			{
				_settingsManager.OnSettingsChanged -= ApplyUserSettings;
				_settingsManager.OnSettingsChanged += ApplyUserSettings;
			}
			ApplyUserSettings();
		}

		public void SuspendCameraDriving(bool suspended)
		{
			_cameraDrivingSuspended = suspended;
		}

		protected override void Start()
		{
			base.Start();
		}

		public void Activate()
		{
			IsActive = true;
		}

		public void Deactivate()
		{
			IsActive = false;
			base.enabled = false;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			base.Crouched += OnCrouchKeyPressed;
			base.UnCrouched += OnCrouchKeyPressed;
			base.Jumped += OnPlayerJumped;
			base.Landed += OnPlayerLanded;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.Crouched -= OnCrouchKeyPressed;
			base.UnCrouched -= OnCrouchKeyPressed;
			base.Jumped -= OnPlayerJumped;
			base.Landed -= OnPlayerLanded;
			if (_equipmentManagerRef != null)
			{
				_equipmentManagerRef.OnItemEquipped.RemoveListener(OnMassItemEquipped);
				_equipmentManagerRef.OnItemUnequipped.RemoveListener(OnMassItemUnequipped);
			}
			if (_placementManagerRef != null)
			{
				_placementManagerRef.OnPlacementEnter.RemoveListener(OnMassPlacementEnter);
				_placementManagerRef.OnPlacementExit.RemoveListener(OnMassPlacementExit);
			}
			if (_settingsManager != null)
			{
				_settingsManager.OnSettingsChanged -= ApplyUserSettings;
			}
			_fovTween.Stop();
		}

		private void Update()
		{
			if (IsActive)
			{
				HandleInputs();
				UpdatePlayerState();
				SetCurrentSurfaceType();
				UpdateFootprintParameters();
			}
		}

		private void UpdatePlayerState()
		{
			Vector3 vector = GetVelocity();
			float magnitude = new Vector3(vector.x, 0f, vector.z).magnitude;
			bool hasMovementInput = Mathf.Abs(OnFootInputs.GetMoveHorizontal()) > 0.01f || Mathf.Abs(OnFootInputs.GetMoveVertical()) > 0.01f;
			bool isSprinting = OnFootInputs.IsSprintButton() && CanSprint && _stateManager.CanSprintByMass;
			bool isCrouching = IsCrouched();
			bool flag = IsOnGround();
			float y = vector.y;
			float distanceToGround = (flag ? 0f : GetDistanceToGround());
			_stateManager.UpdateState(magnitude, hasMovementInput, isSprinting, isCrouching, flag, y, distanceToGround);
			base.maxWalkSpeed = _stateManager.GetMaxWalkSpeed(isCrouching);
			base.maxWalkSpeedCrouched = _stateManager.GetMaxCrouchedWalkSpeed();
		}

		private float GetDistanceToGround()
		{
			if (_groundLayerMask == 0)
			{
				_groundLayerMask = ~(1 << base.gameObject.layer);
			}
			if (Physics.Raycast(base.transform.position + Vector3.up * 0.5f, Vector3.down, out var hitInfo, 100f, _groundLayerMask, QueryTriggerInteraction.Ignore))
			{
				return Mathf.Max(0f, hitInfo.distance - 0.5f);
			}
			return 3.4028235E+38f;
		}

		private void OnPlayerJumped()
		{
			_stateManager?.OnJumped();
			GetComponent<NetworkedFootstepPlayer>()?.PlayJump();
		}

		private void OnPlayerLanded(Vector3 landingVelocity)
		{
			_stateManager?.OnLanded();
			GetComponent<NetworkedFootstepPlayer>()?.PlayLand();
			this.OnLandedImpact?.Invoke(landingVelocity);
		}

		private void LateUpdate()
		{
			if (IsActive && !_cameraDrivingSuspended)
			{
				UpdateCameraRootBased();
				UpdateCameraLookTransformRotation();
				SetHeadFollowTransform();
				_playerCamera.nearClipPlane = FirstPersonControllerSettings.nearClipPlane;
			}
		}

		private void UpdateCameraSitting()
		{
			if (!(leftEye == null) && !(rightEye == null))
			{
				Vector3 vector = (leftEye.position + rightEye.position) * 0.5f;
				Vector3 vector2 = base.transform.InverseTransformPoint(vector);
				Vector3 vector3 = new Vector3(FirstPersonControllerSettings.sittingOffsetX, FirstPersonControllerSettings.sittingOffsetY, FirstPersonControllerSettings.sittingOffsetZ);
				float f = _cameraPitch * ((float)Math.PI / 180f);
				float y = (0f - Mathf.Sin(f)) * FirstPersonControllerSettings.eyeRotationRadius;
				float num = (1f - Mathf.Cos(f)) * FirstPersonControllerSettings.eyeRotationRadius;
				float f2 = _cameraYaw * ((float)Math.PI / 180f);
				float x = Mathf.Sin(f2) * FirstPersonControllerSettings.eyeRotationRadius;
				num += (1f - Mathf.Cos(f2)) * FirstPersonControllerSettings.eyeRotationRadius;
				Vector3 localPosition = vector2 + vector3 + new Vector3(x, y, num);
				cameraTransform.localPosition = localPosition;
				_currentEyeHeight = localPosition.y;
			}
		}

		private void UpdateCameraRootBased()
		{
			if (_playerLookingCameraState == PlayerLookingCameraState.Sitting)
			{
				UpdateCameraSitting();
				return;
			}
			int num;
			if (leftEye != null)
			{
				num = ((rightEye != null) ? 1 : 0);
				if (num != 0 && IsOnGround() && PlayerState == PlayerState.Idle)
				{
					Vector3 vector = (leftEye.position + rightEye.position) * 0.5f;
					_animRestLocalPos = base.transform.InverseTransformPoint(vector);
					_animRestCaptured = true;
				}
			}
			else
			{
				num = 0;
			}
			_targetEyeHeight = ((_playerLookingCameraState == PlayerLookingCameraState.Crouching) ? FirstPersonControllerSettings.crouchedEyeHeight : FirstPersonControllerSettings.standingEyeHeight);
			_currentEyeHeight = Mathf.Lerp(_currentEyeHeight, _targetEyeHeight, FirstPersonControllerSettings.heightTransitionSpeed * Time.deltaTime);
			_crouchBlend01 = Mathf.Lerp(b: (_playerLookingCameraState == PlayerLookingCameraState.Crouching) ? 1f : 0f, a: _crouchBlend01, t: FirstPersonControllerSettings.heightTransitionSpeed * Time.deltaTime);
			float x = Mathf.Lerp(FirstPersonControllerSettings.cameraOffsetX, FirstPersonControllerSettings.crouchedOffsetX, _crouchBlend01);
			float num2 = Mathf.Lerp(0f, FirstPersonControllerSettings.crouchedOffsetY, _crouchBlend01);
			Vector3 localPosition = new Vector3(z: Mathf.Lerp(FirstPersonControllerSettings.cameraOffsetZ, FirstPersonControllerSettings.crouchedOffsetZ, _crouchBlend01), x: x, y: _currentEyeHeight + num2);
			float animationInfluence = FirstPersonControllerSettings.animationInfluence;
			bool flag = PlayerState == PlayerState.Walk || PlayerState == PlayerState.Sprint || PlayerState == PlayerState.CrouchedWalk || PlayerState == PlayerState.CrouchedSprint;
			bool flag2 = PlayerState == PlayerState.Jump || PlayerState == PlayerState.Falling || PlayerState == PlayerState.Landing;
			if (num != 0 && _animRestCaptured && (animationInfluence > 0f || flag2))
			{
				Vector3 target = Vector3.zero;
				float smoothTime = FirstPersonControllerSettings.animationDamping;
				Vector3 vector2 = base.transform.InverseTransformPoint((leftEye.position + rightEye.position) * 0.5f) - _animRestLocalPos;
				if (flag2)
				{
					float airborneHorizontalInfluence = FirstPersonControllerSettings.airborneHorizontalInfluence;
					vector2.x *= airborneHorizontalInfluence;
					vector2.z *= airborneHorizontalInfluence;
					vector2 = Vector3.ClampMagnitude(vector2, FirstPersonControllerSettings.airborneMaxAnimationOffset);
					target = vector2 * FirstPersonControllerSettings.airborneAnimationInfluence;
					smoothTime = FirstPersonControllerSettings.airborneAnimationDamping;
				}
				else if (flag && animationInfluence > 0f)
				{
					vector2 = Vector3.ClampMagnitude(vector2, FirstPersonControllerSettings.maxAnimationOffset);
					target = vector2 * animationInfluence;
				}
				_smoothAnimOffset = Vector3.SmoothDamp(_smoothAnimOffset, target, ref _animOffsetVelocity, smoothTime);
				localPosition += _smoothAnimOffset;
			}
			cameraTransform.localPosition = localPosition;
			if (FirstPersonControllerSettings.enableAntiClip)
			{
				ApplyAntiClip();
			}
		}

		private void ApplyAntiClip()
		{
			float antiClipRadius = FirstPersonControllerSettings.antiClipRadius;
			int num = 1 << base.gameObject.layer;
			int layerMask = (int)FirstPersonControllerSettings.antiClipLayerMask & ~num;
			if (Physics.OverlapSphere(cameraTransform.position, antiClipRadius, layerMask, QueryTriggerInteraction.Ignore).Length != 0)
			{
				Vector3 vector = base.transform.position + base.transform.up * _currentEyeHeight + base.transform.forward * (FirstPersonControllerSettings.cameraOffsetZ + antiClipRadius);
				cameraTransform.localPosition = base.transform.InverseTransformPoint(vector);
			}
		}

		private void SetHeadFollowTransform()
		{
			Vector3 vector = ((_playerLookingCameraState != PlayerLookingCameraState.Sitting) ? base.transform.forward : (Quaternion.Euler(0f, _cameraYaw, 0f) * base.transform.forward));
			float num = (0f - Mathf.Sin(_cameraPitch * ((float)Math.PI / 180f))) * FirstPersonControllerSettings.headFollowPitchInfluence;
			Vector3 normalized = (vector + Vector3.up * num).normalized;
			Vector3 vector2 = base.transform.position + Vector3.up * FirstPersonControllerSettings.standingCapsuleHeight;
			headFollowTransform.position = vector2 + normalized * FirstPersonControllerSettings.headFollowDistance;
		}

		public void AddControlYawInput(float value)
		{
			if (value != 0f)
			{
				AddYawInput(value);
			}
		}

		public void AddControlPitchInput(float value, float minPitch = -80f, float maxPitch = 80f)
		{
			if (value != 0f)
			{
				_targetCameraPitch = Mathf.Clamp(_targetCameraPitch + value, minPitch, maxPitch);
			}
		}

		private void AddCameraYawInput(float value)
		{
			if (value != 0f)
			{
				_targetCameraYaw = Mathf.Clamp(_targetCameraYaw + value, FirstPersonControllerSettings.minCameraYawOnSitting, FirstPersonControllerSettings.maxCameraYawOnSitting);
			}
		}

		private void AddCameraPitchInput(float value)
		{
			if (value != 0f)
			{
				_targetCameraPitch = Mathf.Clamp(_targetCameraPitch + value, FirstPersonControllerSettings.minCameraPitchOnSitting, FirstPersonControllerSettings.maxCameraPitchOnSitting);
			}
		}

		public void UpdateCameraLookTransformRotation()
		{
			if (cameraRotateLimit != CameraRotateLimit.Locked)
			{
				_targetCameraPitch = Mathf.Clamp(_targetCameraPitch, FirstPersonControllerSettings.minCameraPitch, FirstPersonControllerSettings.maxCameraPitch);
				_cameraPitch = Mathf.Clamp(_cameraPitch, FirstPersonControllerSettings.minCameraPitch, FirstPersonControllerSettings.maxCameraPitch);
				float cameraRotationSmoothSpeed = FirstPersonControllerSettings.cameraRotationSmoothSpeed;
				if (cameraRotationSmoothSpeed > 0f)
				{
					float t = cameraRotationSmoothSpeed * Time.deltaTime;
					_cameraPitch = Mathf.LerpAngle(_cameraPitch, _targetCameraPitch, t);
					_cameraYaw = Mathf.LerpAngle(_cameraYaw, _targetCameraYaw, t);
				}
				else
				{
					_cameraPitch = _targetCameraPitch;
					_cameraYaw = _targetCameraYaw;
				}
				if (cameraRotateLimit == CameraRotateLimit.Pitch)
				{
					cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);
				}
				else if (cameraRotateLimit == CameraRotateLimit.Yaw)
				{
					cameraTransform.localRotation = Quaternion.Euler(0f, _cameraYaw, 0f);
				}
				else if (cameraRotateLimit == CameraRotateLimit.None)
				{
					cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, _cameraYaw, 0f);
				}
				else
				{
					EvilLogger.LogError("CameraRotateLimit is not set", "UpdateCameraLookTransformRotation", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\FirstPersonController.cs", 610);
				}
			}
		}

		public CapsuleCollider GetPlayerCapsuleCollider()
		{
			return GetComponent<CapsuleCollider>();
		}

		public void OnCrouchKeyPressed()
		{
			if (IsCrouched())
			{
				base.maxWalkSpeed = FirstPersonControllerSettings.crouchedWalkSpeed;
				SetPlayerLookingCameraState(PlayerLookingCameraState.Crouching);
			}
			else
			{
				base.maxWalkSpeed = FirstPersonControllerSettings.walkSpeed;
				SetPlayerLookingCameraState(PlayerLookingCameraState.Standing);
			}
		}

		private void UpdateFootprintParameters()
		{
		}

		private void SetCurrentSurfaceType()
		{
			if (!(base.characterMovement.groundTransform == null) && base.characterMovement.groundTransform.TryGetComponent<SurfaceTypeComponent>(out var component))
			{
				currentSurfaceType = component.surfaceType;
			}
		}

		private void InitPlayerLookingCamera()
		{
			SetPlayerLookingCameraState(PlayerLookingCameraState.Standing);
		}

		public void SetPlayerLookingCameraState(PlayerLookingCameraState state, float duration = 0f)
		{
			switch (state)
			{
			case PlayerLookingCameraState.Standing:
				cameraRotateLimit = CameraRotateLimit.Pitch;
				_currentEyeHeight = cameraTransform.localPosition.y;
				_targetEyeHeight = FirstPersonControllerSettings.standingEyeHeight;
				_playerLookingCameraState = PlayerLookingCameraState.Standing;
				break;
			case PlayerLookingCameraState.Sitting:
				cameraRotateLimit = CameraRotateLimit.None;
				_cameraPitch = (_targetCameraPitch = 0f);
				_cameraYaw = (_targetCameraYaw = 0f);
				SetYaw(0f);
				_playerLookingCameraState = PlayerLookingCameraState.Sitting;
				break;
			case PlayerLookingCameraState.Crouching:
				cameraRotateLimit = CameraRotateLimit.Pitch;
				_currentEyeHeight = cameraTransform.localPosition.y;
				_targetEyeHeight = FirstPersonControllerSettings.crouchedEyeHeight;
				_playerLookingCameraState = PlayerLookingCameraState.Crouching;
				break;
			}
		}

		public void SetCameraFov(float fov)
		{
			_playerCamera.fieldOfView = fov;
		}

		private float GetCurrentFov()
		{
			return _playerCamera.fieldOfView;
		}

		private void SetCameraFovSmooth(float targetFov, float fullDuration, float startFov)
		{
			_fovTween.Stop();
			float currentFov = GetCurrentFov();
			if (!Mathf.Approximately(currentFov, targetFov))
			{
				if (fullDuration <= 0f)
				{
					SetCameraFov(targetFov);
					return;
				}
				float num = Mathf.Abs(targetFov - startFov);
				float num2 = Mathf.Abs(targetFov - currentFov);
				float duration = ((num > 0f) ? (num2 / num * fullDuration) : fullDuration);
				_fovTween = Tween.Custom(currentFov, targetFov, duration, _onFovChanged, Ease.OutQuad);
			}
		}

		public void ZoomIn()
		{
			_isZooming = true;
			SetCameraFovSmooth(_zoomedFov, _zoomDuration, _defaultFov);
			_eyeZoomAnimator?.SetEyesZoomed(zoomed: true);
		}

		public void ZoomOut()
		{
			_isZooming = false;
			SetCameraFovSmooth(_defaultFov, _zoomDuration, _zoomedFov);
			_eyeZoomAnimator?.SetEyesZoomed(zoomed: false);
		}

		public void ApplySettingsOverrides(Vector2 sensitivityScale, float fov, bool invertMouseY)
		{
			FirstPersonControllerSettings.cameraRotateSensitivity = sensitivityScale * FirstPersonControllerSettings.sensitivityScaleMultiplier;
			InvertMouseY = invertMouseY;
			_defaultFov = fov;
			SetCameraFov(_isZooming ? _zoomedFov : _defaultFov);
		}

		private void ApplyUserSettings()
		{
			if (_settingsManager != null)
			{
				ApplySettingsOverrides(new Vector2(_settingsManager.SensitivityX, _settingsManager.SensitivityY), _settingsManager.Fov, _settingsManager.InvertMouseY);
			}
		}

		public void DisableMovement()
		{
			CanMove = false;
		}

		public void EnableMovement()
		{
			CanMove = true;
		}

		public void DisableCharacterRotate()
		{
			CanCharacterRotate = false;
		}

		public void EnableCharacterRotate()
		{
			CanCharacterRotate = true;
		}

		public void EnableCameraRotate()
		{
			cameraRotateLimit = ((_playerLookingCameraState != PlayerLookingCameraState.Sitting) ? CameraRotateLimit.Pitch : CameraRotateLimit.None);
		}

		public void DisableCameraRotate()
		{
			cameraRotateLimit = CameraRotateLimit.Locked;
		}

		public void SetGravityScale(float value)
		{
			base.gravityScale = value;
		}

		public void DisableZoom()
		{
			CanZoom = false;
			if (_isZooming)
			{
				_isZooming = false;
				_eyeZoomAnimator?.SetEyesZoomed(zoomed: false);
			}
		}

		public void EnableZoom()
		{
			CanZoom = true;
		}

		public void DisableCrouch()
		{
			CanCrouch = false;
		}

		public void EnableCrouch()
		{
			CanCrouch = true;
		}

		public void DisableJump()
		{
			CanJump = false;
		}

		public void EnableJump()
		{
			CanJump = true;
		}

		public void EnableSprint()
		{
			CanSprint = true;
		}

		public void DisableSprint()
		{
			CanSprint = false;
		}

		public void ActivateCamera()
		{
			_playerCamera.enabled = true;
		}

		public void DeactivateCamera()
		{
			_playerCamera.enabled = false;
		}

		public void ResetHeadRotation()
		{
			_cameraPitch = (_targetCameraPitch = 0f);
			_cameraYaw = (_targetCameraYaw = 0f);
		}

		public void SetPlayerBehaviour(PlayerState state)
		{
			_stateManager?.SetState(state);
		}

		private void OnMassItemEquipped()
		{
			if (!(massDebuffSetting == null) && !(_equipmentManagerRef == null))
			{
				HeldItem equippedEntity = _equipmentManagerRef.EquippedEntity;
				if (!(equippedEntity == null) && equippedEntity.TryGetComponent<Rigidbody>(out var component))
				{
					_stateManager.SetMassDebuff(component.mass, massDebuffSetting);
				}
			}
		}

		private void OnMassItemUnequipped()
		{
			_stateManager?.ClearMassDebuff();
		}

		private void OnMassPlacementEnter(GameObject placementObject)
		{
			if (!(massDebuffSetting == null) && !(placementObject == null) && placementObject.TryGetComponent<Rigidbody>(out var component))
			{
				_stateManager.SetMassDebuff(component.mass, massDebuffSetting);
				if (_placementManagerRef != null)
				{
					_placementManagerRef.ScrollSpeedMultiplier = _stateManager.SpeedMultiplier;
				}
			}
		}

		private void OnMassPlacementExit()
		{
			_stateManager?.ClearMassDebuff();
			if (_placementManagerRef != null)
			{
				_placementManagerRef.ScrollSpeedMultiplier = 1f;
			}
		}

		public void SetMassDebuffEnabled(bool enabled)
		{
			if (_stateManager == null)
			{
				return;
			}
			_stateManager.SetMassDebuffEnabled(enabled);
			if (!enabled)
			{
				if (_placementManagerRef != null)
				{
					_placementManagerRef.ScrollSpeedMultiplier = 1f;
				}
			}
			else if (_placementManagerRef != null && _placementManagerRef.IsPlacementModeActive)
			{
				OnMassPlacementEnter(_placementManagerRef.CurrentPlacementObject);
			}
			else if (_equipmentManagerRef != null && _equipmentManagerRef.IsItemEquipped)
			{
				OnMassItemEquipped();
			}
		}

		private void HandleInputs()
		{
			HandleMovementInputs();
			HandleCharacterRotateInputs();
			HandleCameraRotateInputs();
			HandleZoomInputs();
			HandleCrouchInputs();
			HandleJumpInputs();
		}

		private void HandleMovementInputs()
		{
			if (CanMove)
			{
				Vector2 vector = new Vector2
				{
					x = OnFootInputs.GetMoveHorizontal(),
					y = OnFootInputs.GetMoveVertical()
				};
				Vector3 zero = Vector3.zero;
				zero += GetRightVector() * vector.x;
				zero += GetForwardVector() * vector.y;
				SetMovementDirection(zero);
			}
		}

		private void HandleCharacterRotateInputs()
		{
			if (CanCharacterRotate)
			{
				Vector2 vector = new Vector2
				{
					x = BaseInputs.GetLookHorizontal(),
					y = BaseInputs.GetLookVertical()
				};
				float num = vector.x * FirstPersonControllerSettings.cameraRotateSensitivity.x;
				if (num != 0f)
				{
					base.characterMovement.rotation *= Quaternion.Euler(0f, num, 0f);
				}
				float num2 = (InvertMouseY ? 1f : (-1f));
				AddControlPitchInput(vector.y * num2 * FirstPersonControllerSettings.cameraRotateSensitivity.y, FirstPersonControllerSettings.minCameraPitch, FirstPersonControllerSettings.maxCameraPitch);
			}
		}

		protected override void ConsumeRotationInput()
		{
		}

		private void HandleCameraRotateInputs()
		{
			if (_playerLookingCameraState == PlayerLookingCameraState.Sitting && cameraRotateLimit != CameraRotateLimit.Locked)
			{
				Vector2 vector = new Vector2
				{
					x = BaseInputs.GetLookHorizontal(),
					y = BaseInputs.GetLookVertical()
				};
				float num = (InvertMouseY ? 1f : (-1f));
				AddCameraPitchInput(vector.y * num * FirstPersonControllerSettings.cameraRotateSensitivity.y);
				AddCameraYawInput(vector.x * FirstPersonControllerSettings.cameraRotateSensitivity.x);
			}
		}

		private void HandleZoomInputs()
		{
			if (CanZoom)
			{
				if (BaseInputs.IsCameraZoomButtonDown())
				{
					ZoomIn();
				}
				if (BaseInputs.IsCameraZoomButtonUp())
				{
					ZoomOut();
				}
			}
		}

		private void HandleCrouchInputs()
		{
			if (CanCrouch && OnFootInputs.IsCrouchButtonDown())
			{
				if (IsCrouched())
				{
					UnCrouch();
				}
				else
				{
					Crouch();
				}
			}
		}

		private void HandleJumpInputs()
		{
			if (CanJump && !IsCrouched() && OnFootInputs.IsJumpButton())
			{
				Jump();
			}
			else
			{
				StopJumping();
			}
		}
	}
}
