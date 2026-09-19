using System;
using System.Collections.Generic;
using Synty.AnimationBaseLocomotion.Samples.InputSystem;
using UnityEngine;

namespace Synty.AnimationBaseLocomotion.Samples
{
	public class SamplePlayerAnimationController : MonoBehaviour
	{
		private enum AnimationState
		{
			Base = 0,
			Locomotion = 1,
			Jump = 2,
			Fall = 3,
			Crouch = 4
		}

		private enum GaitState
		{
			Idle = 0,
			Walk = 1,
			Run = 2,
			Sprint = 3
		}

		private readonly int _movementInputTappedHash = Animator.StringToHash("MovementInputTapped");

		private readonly int _movementInputPressedHash = Animator.StringToHash("MovementInputPressed");

		private readonly int _movementInputHeldHash = Animator.StringToHash("MovementInputHeld");

		private readonly int _shuffleDirectionXHash = Animator.StringToHash("ShuffleDirectionX");

		private readonly int _shuffleDirectionZHash = Animator.StringToHash("ShuffleDirectionZ");

		private readonly int _moveSpeedHash = Animator.StringToHash("MoveSpeed");

		private readonly int _currentGaitHash = Animator.StringToHash("CurrentGait");

		private readonly int _isJumpingAnimHash = Animator.StringToHash("IsJumping");

		private readonly int _fallingDurationHash = Animator.StringToHash("FallingDuration");

		private readonly int _inclineAngleHash = Animator.StringToHash("InclineAngle");

		private readonly int _strafeDirectionXHash = Animator.StringToHash("StrafeDirectionX");

		private readonly int _strafeDirectionZHash = Animator.StringToHash("StrafeDirectionZ");

		private readonly int _forwardStrafeHash = Animator.StringToHash("ForwardStrafe");

		private readonly int _cameraRotationOffsetHash = Animator.StringToHash("CameraRotationOffset");

		private readonly int _isStrafingHash = Animator.StringToHash("IsStrafing");

		private readonly int _isTurningInPlaceHash = Animator.StringToHash("IsTurningInPlace");

		private readonly int _isCrouchingHash = Animator.StringToHash("IsCrouching");

		private readonly int _isWalkingHash = Animator.StringToHash("IsWalking");

		private readonly int _isStoppedHash = Animator.StringToHash("IsStopped");

		private readonly int _isStartingHash = Animator.StringToHash("IsStarting");

		private readonly int _isGroundedHash = Animator.StringToHash("IsGrounded");

		private readonly int _leanValueHash = Animator.StringToHash("LeanValue");

		private readonly int _headLookXHash = Animator.StringToHash("HeadLookX");

		private readonly int _headLookYHash = Animator.StringToHash("HeadLookY");

		private readonly int _bodyLookXHash = Animator.StringToHash("BodyLookX");

		private readonly int _bodyLookYHash = Animator.StringToHash("BodyLookY");

		private readonly int _locomotionStartDirectionHash = Animator.StringToHash("LocomotionStartDirection");

		[Header("External Components")]
		[Tooltip("Script controlling camera behavior")]
		[SerializeField]
		private SampleCameraController _cameraController;

		[Tooltip("InputReader handles player input")]
		[SerializeField]
		private InputReader _inputReader;

		[Tooltip("Animator component for controlling player animations")]
		[SerializeField]
		private Animator _animator;

		[Tooltip("Character Controller component for controlling player movement")]
		[SerializeField]
		private CharacterController _controller;

		[Header("Player Locomotion")]
		[Header("Main Settings")]
		[Tooltip("Whether the character always faces the camera facing direction")]
		[SerializeField]
		private bool _alwaysStrafe = true;

		[Tooltip("Slowest movement speed of the player when set to a walk state or half press tick")]
		[SerializeField]
		private float _walkSpeed = 1.4f;

		[Tooltip("Default movement speed of the player")]
		[SerializeField]
		private float _runSpeed = 2.5f;

		[Tooltip("Top movement speed of the player")]
		[SerializeField]
		private float _sprintSpeed = 7f;

		[Tooltip("Damping factor for changing speed")]
		[SerializeField]
		private float _speedChangeDamping = 10f;

		[Tooltip("Rotation smoothing factor.")]
		[SerializeField]
		private float _rotationSmoothing = 10f;

		[Tooltip("Offset for camera rotation.")]
		[SerializeField]
		private float _cameraRotationOffset;

		[Header("Shuffles")]
		[Tooltip("Threshold for button hold duration.")]
		[SerializeField]
		private float _buttonHoldThreshold = 0.15f;

		[Tooltip("Direction of shuffling on the X-axis.")]
		[SerializeField]
		private float _shuffleDirectionX;

		[Tooltip("Direction of shuffling on the Z-axis.")]
		[SerializeField]
		private float _shuffleDirectionZ;

		[Header("Capsule Values")]
		[Tooltip("Standing height of the player capsule.")]
		[SerializeField]
		private float _capsuleStandingHeight = 1.8f;

		[Tooltip("Standing center of the player capsule.")]
		[SerializeField]
		private float _capsuleStandingCentre = 0.93f;

		[Tooltip("Crouching height of the player capsule.")]
		[SerializeField]
		private float _capsuleCrouchingHeight = 1.2f;

		[Tooltip("Crouching center of the player capsule.")]
		[SerializeField]
		private float _capsuleCrouchingCentre = 0.6f;

		[Header("Player Strafing")]
		[Tooltip("Minimum threshold for forward strafing angle.")]
		[SerializeField]
		private float _forwardStrafeMinThreshold = -55f;

		[Tooltip("Maximum threshold for forward strafing angle.")]
		[SerializeField]
		private float _forwardStrafeMaxThreshold = 125f;

		[Tooltip("Current forward strafing value.")]
		[SerializeField]
		private float _forwardStrafe = 1f;

		[Header("Grounded Angle")]
		[Tooltip("Position of the rear ray for grounded angle check.")]
		[SerializeField]
		private Transform _rearRayPos;

		[Tooltip("Position of the front ray for grounded angle check.")]
		[SerializeField]
		private Transform _frontRayPos;

		[Tooltip("Layer mask for checking ground.")]
		[SerializeField]
		private LayerMask _groundLayerMask;

		[Tooltip("Current incline angle.")]
		[SerializeField]
		private float _inclineAngle;

		[Tooltip("Useful for rough ground")]
		[SerializeField]
		private float _groundedOffset = -0.14f;

		[Header("Player In-Air")]
		[Tooltip("Force applied when the player jumps.")]
		[SerializeField]
		private float _jumpForce = 10f;

		[Tooltip("Multiplier for gravity when in the air.")]
		[SerializeField]
		private float _gravityMultiplier = 2f;

		[Tooltip("Duration of falling.")]
		[SerializeField]
		private float _fallingDuration;

		[Header("Player Head Look")]
		[Tooltip("Flag indicating if head turning is enabled.")]
		[SerializeField]
		private bool _enableHeadTurn = true;

		[Tooltip("Delay for head turning.")]
		[SerializeField]
		private float _headLookDelay;

		[Tooltip("X-axis value for head turning.")]
		[SerializeField]
		private float _headLookX;

		[Tooltip("Y-axis value for head turning.")]
		[SerializeField]
		private float _headLookY;

		[Tooltip("Curve for X-axis head turning.")]
		[SerializeField]
		private AnimationCurve _headLookXCurve;

		[Header("Player Body Look")]
		[Tooltip("Flag indicating if body turning is enabled.")]
		[SerializeField]
		private bool _enableBodyTurn = true;

		[Tooltip("Delay for body turning.")]
		[SerializeField]
		private float _bodyLookDelay;

		[Tooltip("X-axis value for body turning.")]
		[SerializeField]
		private float _bodyLookX;

		[Tooltip("Y-axis value for body turning.")]
		[SerializeField]
		private float _bodyLookY;

		[Tooltip("Curve for X-axis body turning.")]
		[SerializeField]
		private AnimationCurve _bodyLookXCurve;

		[Header("Player Lean")]
		[Tooltip("Flag indicating if leaning is enabled.")]
		[SerializeField]
		private bool _enableLean = true;

		[Tooltip("Delay for leaning.")]
		[SerializeField]
		private float _leanDelay;

		[Tooltip("Current value for leaning.")]
		[SerializeField]
		private float _leanValue;

		[Tooltip("Curve for leaning.")]
		[SerializeField]
		private AnimationCurve _leanCurve;

		[Tooltip("Delay for head leaning looks.")]
		[SerializeField]
		private float _leansHeadLooksDelay;

		[Tooltip("Flag indicating if an animation clip has ended.")]
		[SerializeField]
		private bool _animationClipEnd;

		private readonly List<GameObject> _currentTargetCandidates = new List<GameObject>();

		private AnimationState _currentState;

		private bool _cannotStandUp;

		private bool _crouchKeyPressed;

		private bool _isAiming;

		private bool _isCrouching;

		private bool _isGrounded = true;

		private bool _isLockedOn;

		private bool _isSliding;

		private bool _isSprinting;

		private bool _isStarting;

		private bool _isStopped = true;

		private bool _isStrafing;

		private bool _isTurningInPlace;

		private bool _isWalking;

		private bool _movementInputHeld;

		private bool _movementInputPressed;

		private bool _movementInputTapped;

		private float _currentMaxSpeed;

		private float _locomotionStartDirection;

		private float _locomotionStartTimer;

		private float _lookingAngle;

		private float _newDirectionDifferenceAngle;

		private float _speed2D;

		private float _strafeAngle;

		private float _strafeDirectionX;

		private float _strafeDirectionZ;

		private GameObject _currentLockOnTarget;

		private GaitState _currentGait;

		private Transform _targetLockOnPos;

		private Vector3 _currentRotation = new Vector3(0f, 0f, 0f);

		private Vector3 _moveDirection;

		private Vector3 _previousRotation;

		private Vector3 _velocity;

		private const float _ANIMATION_DAMP_TIME = 5f;

		private const float _STRAFE_DIRECTION_DAMP_TIME = 20f;

		private float _targetMaxSpeed;

		private float _fallStartTime;

		private float _rotationRate;

		private float _initialLeanValue;

		private float _initialTurnValue;

		private Vector3 _cameraForward;

		private Vector3 _targetVelocity;

		private void Start()
		{
			_targetLockOnPos = base.transform.Find("TargetLockOnPos");
			InputReader inputReader = _inputReader;
			inputReader.onLockOnToggled = (Action)Delegate.Combine(inputReader.onLockOnToggled, new Action(ToggleLockOn));
			InputReader inputReader2 = _inputReader;
			inputReader2.onWalkToggled = (Action)Delegate.Combine(inputReader2.onWalkToggled, new Action(ToggleWalk));
			InputReader inputReader3 = _inputReader;
			inputReader3.onSprintActivated = (Action)Delegate.Combine(inputReader3.onSprintActivated, new Action(ActivateSprint));
			InputReader inputReader4 = _inputReader;
			inputReader4.onSprintDeactivated = (Action)Delegate.Combine(inputReader4.onSprintDeactivated, new Action(DeactivateSprint));
			InputReader inputReader5 = _inputReader;
			inputReader5.onCrouchActivated = (Action)Delegate.Combine(inputReader5.onCrouchActivated, new Action(ActivateCrouch));
			InputReader inputReader6 = _inputReader;
			inputReader6.onCrouchDeactivated = (Action)Delegate.Combine(inputReader6.onCrouchDeactivated, new Action(DeactivateCrouch));
			InputReader inputReader7 = _inputReader;
			inputReader7.onAimActivated = (Action)Delegate.Combine(inputReader7.onAimActivated, new Action(ActivateAim));
			InputReader inputReader8 = _inputReader;
			inputReader8.onAimDeactivated = (Action)Delegate.Combine(inputReader8.onAimDeactivated, new Action(DeactivateAim));
			_isStrafing = _alwaysStrafe;
			SwitchState(AnimationState.Locomotion);
		}

		private void ActivateAim()
		{
			_isAiming = true;
			_isStrafing = !_isSprinting;
		}

		private void DeactivateAim()
		{
			_isAiming = false;
			_isStrafing = !_isSprinting && (_alwaysStrafe || _isLockedOn);
		}

		public void AddTargetCandidate(GameObject newTarget)
		{
			if (newTarget != null)
			{
				_currentTargetCandidates.Add(newTarget);
			}
		}

		public void RemoveTarget(GameObject targetToRemove)
		{
			if (_currentTargetCandidates.Contains(targetToRemove))
			{
				_currentTargetCandidates.Remove(targetToRemove);
			}
		}

		private void ToggleLockOn()
		{
			EnableLockOn(!_isLockedOn);
		}

		private void EnableLockOn(bool enable)
		{
			_isLockedOn = enable;
			_isStrafing = false;
			_isStrafing = (enable ? (!_isSprinting) : (_alwaysStrafe || _isAiming));
			_cameraController.LockOn(enable, _targetLockOnPos);
			if (enable && _currentLockOnTarget != null)
			{
				_currentLockOnTarget.GetComponent<SampleObjectLockOn>().Highlight(enable: true, targetLock: true);
			}
		}

		private void ToggleWalk()
		{
			EnableWalk(!_isWalking);
		}

		private void EnableWalk(bool enable)
		{
			_isWalking = enable && _isGrounded && !_isSprinting;
		}

		private void ActivateSprint()
		{
			if (!_isCrouching)
			{
				EnableWalk(enable: false);
				_isSprinting = true;
				_isStrafing = false;
			}
		}

		private void DeactivateSprint()
		{
			_isSprinting = false;
			if (_alwaysStrafe || _isAiming || _isLockedOn)
			{
				_isStrafing = true;
			}
		}

		private void ActivateCrouch()
		{
			_crouchKeyPressed = true;
			if (_isGrounded)
			{
				CapsuleCrouchingSize(crouching: true);
				DeactivateSprint();
				_isCrouching = true;
			}
		}

		private void DeactivateCrouch()
		{
			_crouchKeyPressed = false;
			if (!_cannotStandUp && !_isSliding)
			{
				CapsuleCrouchingSize(crouching: false);
				_isCrouching = false;
			}
		}

		public void ActivateSliding()
		{
			_isSliding = true;
		}

		public void DeactivateSliding()
		{
			_isSliding = false;
		}

		private void CapsuleCrouchingSize(bool crouching)
		{
			if (crouching)
			{
				_controller.center = new Vector3(0f, _capsuleCrouchingCentre, 0f);
				_controller.height = _capsuleCrouchingHeight;
			}
			else
			{
				_controller.center = new Vector3(0f, _capsuleStandingCentre, 0f);
				_controller.height = _capsuleStandingHeight;
			}
		}

		private void SwitchState(AnimationState newState)
		{
			ExitCurrentState();
			EnterState(newState);
		}

		private void EnterState(AnimationState stateToEnter)
		{
			_currentState = stateToEnter;
			switch (_currentState)
			{
			case AnimationState.Base:
				EnterBaseState();
				break;
			case AnimationState.Locomotion:
				EnterLocomotionState();
				break;
			case AnimationState.Jump:
				EnterJumpState();
				break;
			case AnimationState.Fall:
				EnterFallState();
				break;
			case AnimationState.Crouch:
				EnterCrouchState();
				break;
			}
		}

		private void ExitCurrentState()
		{
			switch (_currentState)
			{
			case AnimationState.Locomotion:
				ExitLocomotionState();
				break;
			case AnimationState.Jump:
				ExitJumpState();
				break;
			case AnimationState.Crouch:
				ExitCrouchState();
				break;
			case AnimationState.Fall:
				break;
			}
		}

		private void Update()
		{
			switch (_currentState)
			{
			case AnimationState.Locomotion:
				UpdateLocomotionState();
				break;
			case AnimationState.Jump:
				UpdateJumpState();
				break;
			case AnimationState.Fall:
				UpdateFallState();
				break;
			case AnimationState.Crouch:
				UpdateCrouchState();
				break;
			}
		}

		private void UpdateAnimatorController()
		{
			_animator.SetFloat(_leanValueHash, _leanValue);
			_animator.SetFloat(_headLookXHash, _headLookX);
			_animator.SetFloat(_headLookYHash, _headLookY);
			_animator.SetFloat(_bodyLookXHash, _bodyLookX);
			_animator.SetFloat(_bodyLookYHash, _bodyLookY);
			_animator.SetFloat(_isStrafingHash, _isStrafing ? 1f : 0f);
			_animator.SetFloat(_inclineAngleHash, _inclineAngle);
			_animator.SetFloat(_moveSpeedHash, _speed2D);
			_animator.SetInteger(_currentGaitHash, (int)_currentGait);
			_animator.SetFloat(_strafeDirectionXHash, _strafeDirectionX);
			_animator.SetFloat(_strafeDirectionZHash, _strafeDirectionZ);
			_animator.SetFloat(_forwardStrafeHash, _forwardStrafe);
			_animator.SetFloat(_cameraRotationOffsetHash, _cameraRotationOffset);
			_animator.SetBool(_movementInputHeldHash, _movementInputHeld);
			_animator.SetBool(_movementInputPressedHash, _movementInputPressed);
			_animator.SetBool(_movementInputTappedHash, _movementInputTapped);
			_animator.SetFloat(_shuffleDirectionXHash, _shuffleDirectionX);
			_animator.SetFloat(_shuffleDirectionZHash, _shuffleDirectionZ);
			_animator.SetBool(_isTurningInPlaceHash, _isTurningInPlace);
			_animator.SetBool(_isCrouchingHash, _isCrouching);
			_animator.SetFloat(_fallingDurationHash, _fallingDuration);
			_animator.SetBool(_isGroundedHash, _isGrounded);
			_animator.SetBool(_isWalkingHash, _isWalking);
			_animator.SetBool(_isStoppedHash, _isStopped);
			_animator.SetFloat(_locomotionStartDirectionHash, _locomotionStartDirection);
		}

		private void EnterBaseState()
		{
			_previousRotation = base.transform.forward;
		}

		private void CalculateInput()
		{
			if (_inputReader._movementInputDetected)
			{
				if (_inputReader._movementInputDuration == 0f)
				{
					_movementInputTapped = true;
				}
				else if (_inputReader._movementInputDuration > 0f && _inputReader._movementInputDuration < _buttonHoldThreshold)
				{
					_movementInputTapped = false;
					_movementInputPressed = true;
					_movementInputHeld = false;
				}
				else
				{
					_movementInputTapped = false;
					_movementInputPressed = false;
					_movementInputHeld = true;
				}
				_inputReader._movementInputDuration += Time.deltaTime;
			}
			else
			{
				_inputReader._movementInputDuration = 0f;
				_movementInputTapped = false;
				_movementInputPressed = false;
				_movementInputHeld = false;
			}
			_moveDirection = _cameraController.GetCameraForwardZeroedYNormalised() * _inputReader._moveComposite.y + _cameraController.GetCameraRightZeroedYNormalised() * _inputReader._moveComposite.x;
		}

		private void Move()
		{
			_controller.Move(_velocity * Time.deltaTime);
			if (_isLockedOn && _currentLockOnTarget != null)
			{
				_targetLockOnPos.position = _currentLockOnTarget.transform.position;
			}
		}

		private void ApplyGravity()
		{
			if (_velocity.y > Physics.gravity.y)
			{
				_velocity.y += Physics.gravity.y * _gravityMultiplier * Time.deltaTime;
			}
		}

		private void CalculateMoveDirection()
		{
			CalculateInput();
			if (!_isGrounded)
			{
				_targetMaxSpeed = _currentMaxSpeed;
			}
			else if (_isCrouching)
			{
				_targetMaxSpeed = _walkSpeed;
			}
			else if (_isSprinting)
			{
				_targetMaxSpeed = _sprintSpeed;
			}
			else if (_isWalking)
			{
				_targetMaxSpeed = _walkSpeed;
			}
			else
			{
				_targetMaxSpeed = _runSpeed;
			}
			_currentMaxSpeed = Mathf.Lerp(_currentMaxSpeed, _targetMaxSpeed, 5f * Time.deltaTime);
			_targetVelocity.x = _moveDirection.x * _currentMaxSpeed;
			_targetVelocity.z = _moveDirection.z * _currentMaxSpeed;
			_velocity.z = Mathf.Lerp(_velocity.z, _targetVelocity.z, _speedChangeDamping * Time.deltaTime);
			_velocity.x = Mathf.Lerp(_velocity.x, _targetVelocity.x, _speedChangeDamping * Time.deltaTime);
			_speed2D = new Vector3(_velocity.x, 0f, _velocity.z).magnitude;
			_speed2D = Mathf.Round(_speed2D * 1000f) / 1000f;
			Vector3 forward = base.transform.forward;
			_newDirectionDifferenceAngle = ((forward != _moveDirection) ? Vector3.SignedAngle(forward, _moveDirection, Vector3.up) : 0f);
			CalculateGait();
		}

		private void CalculateGait()
		{
			float num = (_walkSpeed + _runSpeed) / 2f;
			float num2 = (_runSpeed + _sprintSpeed) / 2f;
			if ((double)_speed2D < 0.01)
			{
				_currentGait = GaitState.Idle;
			}
			else if (_speed2D < num)
			{
				_currentGait = GaitState.Walk;
			}
			else if (_speed2D < num2)
			{
				_currentGait = GaitState.Run;
			}
			else
			{
				_currentGait = GaitState.Sprint;
			}
		}

		private void FaceMoveDirection()
		{
			Vector3 normalized = new Vector3(base.transform.forward.x, 0f, base.transform.forward.z).normalized;
			Vector3 normalized2 = new Vector3(base.transform.right.x, 0f, base.transform.right.z).normalized;
			Vector3 normalized3 = new Vector3(_moveDirection.x, 0f, _moveDirection.z).normalized;
			_cameraForward = _cameraController.GetCameraForwardZeroedYNormalised();
			Quaternion b = Quaternion.LookRotation(_cameraForward);
			_strafeAngle = ((normalized != normalized3) ? Vector3.SignedAngle(normalized, normalized3, Vector3.up) : 0f);
			_isTurningInPlace = false;
			if (_isStrafing)
			{
				if ((double)_moveDirection.magnitude > 0.01)
				{
					if (_cameraForward != Vector3.zero)
					{
						_shuffleDirectionZ = Vector3.Dot(normalized, normalized3);
						_shuffleDirectionX = Vector3.Dot(normalized2, normalized3);
						UpdateStrafeDirection(Vector3.Dot(normalized, normalized3), Vector3.Dot(normalized2, normalized3));
						_cameraRotationOffset = Mathf.Lerp(_cameraRotationOffset, 0f, _rotationSmoothing * Time.deltaTime);
						float num = ((_strafeAngle > _forwardStrafeMinThreshold && _strafeAngle < _forwardStrafeMaxThreshold) ? 1f : 0f);
						if (Mathf.Abs(_forwardStrafe - num) <= 0.001f)
						{
							_forwardStrafe = num;
						}
						else
						{
							float t = Mathf.Clamp01(20f * Time.deltaTime);
							_forwardStrafe = Mathf.SmoothStep(_forwardStrafe, num, t);
						}
					}
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, _rotationSmoothing * Time.deltaTime);
				}
				else
				{
					UpdateStrafeDirection(1f, 0f);
					float t2 = 20f * Time.deltaTime;
					float b2 = 0f;
					if (normalized != _cameraForward)
					{
						b2 = Vector3.SignedAngle(normalized, _cameraForward, Vector3.up);
					}
					_cameraRotationOffset = Mathf.Lerp(_cameraRotationOffset, b2, t2);
					if (Mathf.Abs(_cameraRotationOffset) > 10f)
					{
						_isTurningInPlace = true;
					}
				}
			}
			else
			{
				UpdateStrafeDirection(1f, 0f);
				_cameraRotationOffset = Mathf.Lerp(_cameraRotationOffset, 0f, _rotationSmoothing * Time.deltaTime);
				_shuffleDirectionZ = 1f;
				_shuffleDirectionX = 0f;
				Vector3 vector = new Vector3(_velocity.x, 0f, _velocity.z);
				if (!(vector == Vector3.zero))
				{
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(vector), _rotationSmoothing * Time.deltaTime);
				}
			}
		}

		private void CheckIfStopped()
		{
			_isStopped = _moveDirection.magnitude == 0f && (double)_speed2D < 0.5;
		}

		private void CheckIfStarting()
		{
			_locomotionStartTimer = VariableOverrideDelayTimer(_locomotionStartTimer);
			bool flag = false;
			if (_locomotionStartTimer <= 0f)
			{
				if ((double)_moveDirection.magnitude > 0.01 && _speed2D < 1f && !_isStrafing)
				{
					flag = true;
				}
				if (flag)
				{
					if (!_isStarting)
					{
						_locomotionStartDirection = _newDirectionDifferenceAngle;
						_animator.SetFloat(_locomotionStartDirectionHash, _locomotionStartDirection);
					}
					_locomotionStartTimer = (_bodyLookDelay = (_headLookDelay = (_leanDelay = 0.2f)));
				}
			}
			else
			{
				flag = true;
			}
			_isStarting = flag;
			_animator.SetBool(_isStartingHash, _isStarting);
		}

		private void UpdateStrafeDirection(float TargetZ, float TargetX)
		{
			_strafeDirectionZ = Mathf.Lerp(_strafeDirectionZ, TargetZ, 5f * Time.deltaTime);
			_strafeDirectionX = Mathf.Lerp(_strafeDirectionX, TargetX, 5f * Time.deltaTime);
			_strafeDirectionZ = Mathf.Round(_strafeDirectionZ * 1000f) / 1000f;
			_strafeDirectionX = Mathf.Round(_strafeDirectionX * 1000f) / 1000f;
		}

		private void GroundedCheck()
		{
			Vector3 position = new Vector3(_controller.transform.position.x, _controller.transform.position.y - _groundedOffset, _controller.transform.position.z);
			_isGrounded = Physics.CheckSphere(position, _controller.radius, _groundLayerMask, QueryTriggerInteraction.Ignore);
			if (_isGrounded)
			{
				GroundInclineCheck();
			}
		}

		private void GroundInclineCheck()
		{
			float maxDistance = float.PositiveInfinity;
			_rearRayPos.rotation = Quaternion.Euler(base.transform.rotation.x, 0f, 0f);
			_frontRayPos.rotation = Quaternion.Euler(base.transform.rotation.x, 0f, 0f);
			Physics.Raycast(_rearRayPos.position, _rearRayPos.TransformDirection(-Vector3.up), out var hitInfo, maxDistance, _groundLayerMask);
			Physics.Raycast(_frontRayPos.position, _frontRayPos.TransformDirection(-Vector3.up), out var hitInfo2, maxDistance, _groundLayerMask);
			Vector3 vector = hitInfo2.point - hitInfo.point;
			float magnitude = new Vector2(vector.x, vector.z).magnitude;
			_inclineAngle = Mathf.Lerp(_inclineAngle, Mathf.Atan2(vector.y, magnitude) * 57.29578f, 20f * Time.deltaTime);
		}

		private void CeilingHeightCheck()
		{
			float maxDistance = float.PositiveInfinity;
			float num = _capsuleStandingHeight - _frontRayPos.localPosition.y;
			if (Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y + _frontRayPos.localPosition.y, base.transform.position.z), base.transform.TransformDirection(Vector3.up), out var hitInfo, maxDistance, _groundLayerMask))
			{
				_cannotStandUp = hitInfo.distance < num;
			}
			else
			{
				_cannotStandUp = false;
			}
		}

		private void ResetFallingDuration()
		{
			_fallStartTime = Time.time;
			_fallingDuration = 0f;
		}

		private void UpdateFallingDuration()
		{
			_fallingDuration = Time.time - _fallStartTime;
		}

		private void CheckEnableTurns()
		{
			_headLookDelay = VariableOverrideDelayTimer(_headLookDelay);
			_enableHeadTurn = _headLookDelay == 0f && !_isStarting;
			_bodyLookDelay = VariableOverrideDelayTimer(_bodyLookDelay);
			_enableBodyTurn = _bodyLookDelay == 0f && !_isStarting && !_isTurningInPlace;
		}

		private void CheckEnableLean()
		{
			_leanDelay = VariableOverrideDelayTimer(_leanDelay);
			_enableLean = _leanDelay == 0f && !_isStarting && !_isTurningInPlace;
		}

		private void CalculateRotationalAdditives(bool leansActivated, bool headLookActivated, bool bodyLookActivated)
		{
			if (headLookActivated || leansActivated || bodyLookActivated)
			{
				_currentRotation = base.transform.forward;
				_rotationRate = ((_currentRotation != _previousRotation) ? (Vector3.SignedAngle(_currentRotation, _previousRotation, Vector3.up) / Time.deltaTime * -1f) : 0f);
			}
			_initialLeanValue = (leansActivated ? _rotationRate : 0f);
			float smoothness = 5f;
			float maxRateChange = 275f;
			float referenceValue = _speed2D / _sprintSpeed;
			_leanValue = CalculateSmoothedValue(_leanValue, _initialLeanValue, maxRateChange, smoothness, _leanCurve, referenceValue, isMultiplier: true);
			float smoothness2 = 5f;
			if (headLookActivated && _isTurningInPlace)
			{
				_initialTurnValue = _cameraRotationOffset;
				_headLookX = Mathf.Lerp(_headLookX, _initialTurnValue / 200f, 5f * Time.deltaTime);
			}
			else
			{
				_initialTurnValue = (headLookActivated ? _rotationRate : 0f);
				_headLookX = CalculateSmoothedValue(_headLookX, _initialTurnValue, maxRateChange, smoothness2, _headLookXCurve, _headLookX, isMultiplier: false);
			}
			float smoothness3 = 5f;
			_initialTurnValue = (bodyLookActivated ? _rotationRate : 0f);
			_bodyLookX = CalculateSmoothedValue(_bodyLookX, _initialTurnValue, maxRateChange, smoothness3, _bodyLookXCurve, _bodyLookX, isMultiplier: false);
			float cameraTiltX = _cameraController.GetCameraTiltX();
			cameraTiltX = ((cameraTiltX > 180f) ? (cameraTiltX - 360f) : cameraTiltX) / -180f;
			_bodyLookY = (_headLookY = Mathf.Clamp(cameraTiltX, -0.1f, 1f));
			_previousRotation = _currentRotation;
		}

		private float CalculateSmoothedValue(float mainVariable, float newValue, float maxRateChange, float smoothness, AnimationCurve referenceCurve, float referenceValue, bool isMultiplier)
		{
			float value = newValue / maxRateChange;
			value = Mathf.Clamp(value, -1f, 1f);
			if (isMultiplier)
			{
				float num = referenceCurve.Evaluate(referenceValue);
				value *= num;
			}
			else
			{
				value = referenceCurve.Evaluate(value);
			}
			if (!value.Equals(mainVariable))
			{
				value = Mathf.Lerp(mainVariable, value, smoothness * Time.deltaTime);
			}
			return value;
		}

		private float VariableOverrideDelayTimer(float timeVariable)
		{
			if (timeVariable > 0f)
			{
				timeVariable -= Time.deltaTime;
				timeVariable = Mathf.Clamp(timeVariable, 0f, 1f);
			}
			else
			{
				timeVariable = 0f;
			}
			return timeVariable;
		}

		private void UpdateBestTarget()
		{
			GameObject currentLockOnTarget;
			if (_currentTargetCandidates.Count == 0)
			{
				currentLockOnTarget = null;
			}
			else if (_currentTargetCandidates.Count == 1)
			{
				currentLockOnTarget = _currentTargetCandidates[0];
			}
			else
			{
				currentLockOnTarget = null;
				float num = 0f;
				foreach (GameObject currentTargetCandidate in _currentTargetCandidates)
				{
					currentTargetCandidate.GetComponent<SampleObjectLockOn>().Highlight(enable: false, targetLock: false);
					float num2 = Vector3.Distance(base.transform.position, currentTargetCandidate.transform.position);
					float num3 = 1f / num2 * 100f;
					float num4 = Vector3.Dot((currentTargetCandidate.transform.position - _cameraController.GetCameraPosition()).normalized, _cameraController.GetCameraForward()) * 40f;
					float num5 = num3 + num4;
					if (num5 > num)
					{
						num = num5;
						currentLockOnTarget = currentTargetCandidate;
					}
				}
			}
			if (!_isLockedOn)
			{
				_currentLockOnTarget = currentLockOnTarget;
				if (_currentLockOnTarget != null)
				{
					_currentLockOnTarget.GetComponent<SampleObjectLockOn>().Highlight(enable: true, targetLock: false);
				}
			}
			else if (_currentTargetCandidates.Contains(_currentLockOnTarget))
			{
				_currentLockOnTarget.GetComponent<SampleObjectLockOn>().Highlight(enable: true, targetLock: true);
			}
			else
			{
				_currentLockOnTarget = currentLockOnTarget;
				EnableLockOn(enable: false);
			}
		}

		private void EnterLocomotionState()
		{
			InputReader inputReader = _inputReader;
			inputReader.onJumpPerformed = (Action)Delegate.Combine(inputReader.onJumpPerformed, new Action(LocomotionToJumpState));
		}

		private void UpdateLocomotionState()
		{
			UpdateBestTarget();
			GroundedCheck();
			if (!_isGrounded)
			{
				SwitchState(AnimationState.Fall);
			}
			if (_isCrouching)
			{
				SwitchState(AnimationState.Crouch);
			}
			CheckEnableTurns();
			CheckEnableLean();
			CalculateRotationalAdditives(_enableLean, _enableHeadTurn, _enableBodyTurn);
			CalculateMoveDirection();
			CheckIfStarting();
			CheckIfStopped();
			FaceMoveDirection();
			Move();
			UpdateAnimatorController();
		}

		private void ExitLocomotionState()
		{
			InputReader inputReader = _inputReader;
			inputReader.onJumpPerformed = (Action)Delegate.Remove(inputReader.onJumpPerformed, new Action(LocomotionToJumpState));
		}

		private void LocomotionToJumpState()
		{
			SwitchState(AnimationState.Jump);
		}

		private void EnterJumpState()
		{
			_animator.SetBool(_isJumpingAnimHash, value: true);
			_isSliding = false;
			_velocity = new Vector3(_velocity.x, _jumpForce, _velocity.z);
		}

		private void UpdateJumpState()
		{
			UpdateBestTarget();
			ApplyGravity();
			if (_velocity.y <= 0f)
			{
				_animator.SetBool(_isJumpingAnimHash, value: false);
				SwitchState(AnimationState.Fall);
			}
			GroundedCheck();
			CalculateRotationalAdditives(leansActivated: false, _enableHeadTurn, _enableBodyTurn);
			CalculateMoveDirection();
			FaceMoveDirection();
			Move();
			UpdateAnimatorController();
		}

		private void ExitJumpState()
		{
			_animator.SetBool(_isJumpingAnimHash, value: false);
		}

		private void EnterFallState()
		{
			ResetFallingDuration();
			_velocity.y = 0f;
			DeactivateCrouch();
			_isSliding = false;
		}

		private void UpdateFallState()
		{
			UpdateBestTarget();
			GroundedCheck();
			CalculateRotationalAdditives(leansActivated: false, _enableHeadTurn, _enableBodyTurn);
			CalculateMoveDirection();
			FaceMoveDirection();
			ApplyGravity();
			Move();
			UpdateAnimatorController();
			if (_controller.isGrounded)
			{
				SwitchState(AnimationState.Locomotion);
			}
			UpdateFallingDuration();
		}

		private void EnterCrouchState()
		{
			InputReader inputReader = _inputReader;
			inputReader.onJumpPerformed = (Action)Delegate.Combine(inputReader.onJumpPerformed, new Action(CrouchToJumpState));
		}

		private void UpdateCrouchState()
		{
			UpdateBestTarget();
			GroundedCheck();
			if (!_isGrounded)
			{
				DeactivateCrouch();
				CapsuleCrouchingSize(crouching: false);
				SwitchState(AnimationState.Fall);
			}
			CeilingHeightCheck();
			if (!_crouchKeyPressed && !_cannotStandUp)
			{
				DeactivateCrouch();
				SwitchToLocomotionState();
			}
			if (!_isCrouching)
			{
				CapsuleCrouchingSize(crouching: false);
				SwitchToLocomotionState();
			}
			CheckEnableTurns();
			CheckEnableLean();
			CalculateRotationalAdditives(leansActivated: false, _enableHeadTurn, bodyLookActivated: false);
			CalculateMoveDirection();
			CheckIfStarting();
			CheckIfStopped();
			FaceMoveDirection();
			Move();
			UpdateAnimatorController();
		}

		private void ExitCrouchState()
		{
			InputReader inputReader = _inputReader;
			inputReader.onJumpPerformed = (Action)Delegate.Remove(inputReader.onJumpPerformed, new Action(CrouchToJumpState));
		}

		private void CrouchToJumpState()
		{
			if (!_cannotStandUp)
			{
				DeactivateCrouch();
				SwitchState(AnimationState.Jump);
			}
		}

		private void SwitchToLocomotionState()
		{
			DeactivateCrouch();
			SwitchState(AnimationState.Locomotion);
		}
	}
}
