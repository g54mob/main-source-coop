using UnityEngine;

namespace NomadDrive.Features.Player.PlayerStateMachine
{
	public class PlayerStateManager
	{
		private PlayerState _currentState;

		private readonly FirstPersonControllerSettings _controllerSettings;

		private float _speedMultiplier = 1f;

		private bool _massDebuffEnabled = true;

		private bool _hasJumped;

		private float _airborneTimer;

		private float _landingTimer;

		private const float FallingThreshold = 0.3f;

		private const float JumpMinDuration = 0.5f;

		private const float LandingDuration = 0.5f;

		private const float LandingPrepTime = 0.2f;

		private const float MaxLandingPredictionHeight = 3f;

		private const float MinAirborneForLandingPredict = 0.12f;

		public bool CanSprintByMass { get; private set; } = true;

		public float SpeedMultiplier => _speedMultiplier;

		public bool MassDebuffEnabled => _massDebuffEnabled;

		public PlayerState CurrentState => _currentState;

		public PlayerState PreviousState { get; private set; }

		public PlayerStateManager(FirstPersonControllerSettings controllerSettings)
		{
			_controllerSettings = controllerSettings;
			_currentState = PlayerState.Idle;
			PreviousState = PlayerState.Idle;
		}

		public void SetState(PlayerState newState)
		{
			if (_currentState != newState)
			{
				PreviousState = _currentState;
				_currentState = newState;
			}
		}

		public void OnJumped()
		{
			_hasJumped = true;
			_airborneTimer = 0f;
		}

		public void OnLanded()
		{
			_hasJumped = false;
			_airborneTimer = 0f;
			if (_currentState == PlayerState.Jump || _currentState == PlayerState.Falling)
			{
				PreviousState = _currentState;
				_currentState = PlayerState.Landing;
				_landingTimer = 0.5f;
			}
		}

		public void UpdateState(float currentSpeed, bool hasMovementInput, bool isSprinting, bool isCrouching, bool isGrounded, float verticalVelocity, float distanceToGround)
		{
			if (_currentState != PlayerState.Sit && _currentState != PlayerState.Laying && _currentState != PlayerState.DownedLying && _currentState != PlayerState.DownedSitting)
			{
				PlayerState playerState = DetermineState(currentSpeed, hasMovementInput, isSprinting, isCrouching, isGrounded, verticalVelocity, distanceToGround);
				if (playerState != _currentState)
				{
					PreviousState = _currentState;
					_currentState = playerState;
				}
			}
		}

		private PlayerState DetermineState(float currentSpeed, bool hasMovementInput, bool isSprinting, bool isCrouching, bool isGrounded, float verticalVelocity, float distanceToGround)
		{
			if (_currentState == PlayerState.Landing)
			{
				_landingTimer -= Time.deltaTime;
				bool flag = _hasJumped || (isGrounded && hasMovementInput);
				if (_landingTimer > 0f && !flag)
				{
					return PlayerState.Landing;
				}
			}
			if (_hasJumped)
			{
				_airborneTimer += Time.deltaTime;
				if (_airborneTimer > 0.12f && verticalVelocity < -0.1f && IsPredictedLanding(verticalVelocity, distanceToGround))
				{
					_hasJumped = false;
					_airborneTimer = 0f;
					_landingTimer = 0.5f;
					return PlayerState.Landing;
				}
				if (_airborneTimer > 0.5f && !isGrounded && verticalVelocity < -0.1f)
				{
					_hasJumped = false;
					return PlayerState.Falling;
				}
				return PlayerState.Jump;
			}
			if (!isGrounded)
			{
				_airborneTimer += Time.deltaTime;
				if (_currentState == PlayerState.Falling && IsPredictedLanding(verticalVelocity, distanceToGround))
				{
					_airborneTimer = 0f;
					_landingTimer = 0.5f;
					return PlayerState.Landing;
				}
				if (_airborneTimer > 0.3f)
				{
					return PlayerState.Falling;
				}
				return _currentState;
			}
			_airborneTimer = 0f;
			if (!hasMovementInput)
			{
				if (!isCrouching)
				{
					return PlayerState.Idle;
				}
				return PlayerState.CrouchedIdle;
			}
			float num = (isCrouching ? _controllerSettings.crouchedWalkSpeed : _controllerSettings.walkSpeed) * _speedMultiplier;
			float num2 = (isCrouching ? _controllerSettings.crouchedSprintSpeed : _controllerSettings.sprintSpeed) * _speedMultiplier;
			float num3 = num * _controllerSettings.idleToWalkThreshold;
			float num4 = Mathf.Min(num2 * _controllerSettings.walkToSprintThreshold, num * 0.95f);
			if (isCrouching)
			{
				if (currentSpeed < num3)
				{
					return PlayerState.CrouchedIdle;
				}
				if (isSprinting && currentSpeed >= num4)
				{
					return PlayerState.CrouchedSprint;
				}
				return PlayerState.CrouchedWalk;
			}
			if (currentSpeed < num3)
			{
				return PlayerState.Idle;
			}
			if (isSprinting && currentSpeed >= num4)
			{
				return PlayerState.Sprint;
			}
			return PlayerState.Walk;
		}

		public float GetCurrentSpeed()
		{
			return _currentState switch
			{
				PlayerState.Idle => 0f, 
				PlayerState.Walk => _controllerSettings.walkSpeed, 
				PlayerState.Sprint => _controllerSettings.sprintSpeed, 
				PlayerState.CrouchedIdle => 0f, 
				PlayerState.CrouchedWalk => _controllerSettings.crouchedWalkSpeed, 
				PlayerState.CrouchedSprint => _controllerSettings.crouchedSprintSpeed, 
				PlayerState.Sit => 0f, 
				PlayerState.Laying => 0f, 
				PlayerState.DownedLying => 0f, 
				PlayerState.DownedSitting => 0f, 
				PlayerState.Jump => _controllerSettings.walkSpeed, 
				PlayerState.Falling => _controllerSettings.walkSpeed, 
				PlayerState.Landing => _controllerSettings.walkSpeed, 
				_ => _controllerSettings.walkSpeed, 
			};
		}

		public float GetMaxWalkSpeed(bool isCrouching)
		{
			float num = ((!isCrouching) ? (IsSprintState() ? _controllerSettings.sprintSpeed : _controllerSettings.walkSpeed) : (IsSprintState() ? _controllerSettings.crouchedSprintSpeed : _controllerSettings.crouchedWalkSpeed));
			return num * _speedMultiplier;
		}

		public float GetMaxCrouchedWalkSpeed()
		{
			return (IsSprintState() ? _controllerSettings.crouchedSprintSpeed : _controllerSettings.crouchedWalkSpeed) * _speedMultiplier;
		}

		public void SetMassDebuff(float mass, MassDebuffSetting setting)
		{
			if (!_massDebuffEnabled)
			{
				ClearMassDebuff();
				return;
			}
			if (mass <= setting.NoDebuffThreshold)
			{
				_speedMultiplier = 1f;
			}
			else
			{
				float num = setting.MaxDebuffMass - setting.NoDebuffThreshold;
				float num2 = Mathf.Clamp01((mass - setting.NoDebuffThreshold) / num);
				_speedMultiplier = 1f - num2 * setting.MaxSpeedReduction;
			}
			CanSprintByMass = mass < setting.SprintDisableMass;
		}

		public void ClearMassDebuff()
		{
			_speedMultiplier = 1f;
			CanSprintByMass = true;
		}

		public void SetMassDebuffEnabled(bool enabled)
		{
			_massDebuffEnabled = enabled;
			if (!enabled)
			{
				ClearMassDebuff();
			}
		}

		public bool IsSprintState()
		{
			if (_currentState != PlayerState.Sprint)
			{
				return _currentState == PlayerState.CrouchedSprint;
			}
			return true;
		}

		public bool IsAirborne()
		{
			if (_currentState != PlayerState.Jump && _currentState != PlayerState.Falling)
			{
				return _currentState == PlayerState.Landing;
			}
			return true;
		}

		public bool IsMovingState()
		{
			if (_currentState != PlayerState.Walk && _currentState != PlayerState.Sprint && _currentState != PlayerState.CrouchedWalk)
			{
				return _currentState == PlayerState.CrouchedSprint;
			}
			return true;
		}

		public bool IsCrouchedState()
		{
			if (_currentState != PlayerState.CrouchedIdle && _currentState != PlayerState.CrouchedWalk)
			{
				return _currentState == PlayerState.CrouchedSprint;
			}
			return true;
		}

		private bool IsPredictedLanding(float verticalVelocity, float distanceToGround)
		{
			if (distanceToGround <= 0f || distanceToGround > 3f)
			{
				return false;
			}
			float num = Mathf.Abs(verticalVelocity);
			if (num < 0.5f)
			{
				return false;
			}
			return distanceToGround / num < 0.2f;
		}
	}
}
