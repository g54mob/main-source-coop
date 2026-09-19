using System;
using Features.CameraModelModule;
using Features.InputModule.Scripts.Generated;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using Zenject;

namespace Features.CameraModuleRotation.Scripts
{
	public class CameraRotatorBasedOnMovable : MonoBehaviour
	{
		[SerializeField]
		private CameraControllerBase _cameraControllerBase;

		[SerializeField]
		private AnimationCurve _screenShakeFrequencyBasedOnSpeed;

		[SerializeField]
		private AnimationCurve _screenShakeAmplitudeBasedOnSpeed;

		[SerializeField]
		private CinemachineRollExtender _rollExtender;

		[SerializeField]
		private float _noiseTransitionTime;

		[SerializeField]
		private NoiseType _initialNoiseType;

		[SerializeField]
		private float _noiseFrequencyGainStrength;

		[SerializeField]
		private float _noiseAmplitudeGainStrength;

		[Header("Roll Settings")]
		[SerializeField]
		private float _maxRollAngle = 5f;

		[SerializeField]
		private float _rollSpeed = 5f;

		[SerializeField]
		private AnimationCurve _rollCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private float _mouseMaxRollAngle = 2f;

		[SerializeField]
		private float _mouseRollSensitivity = 1.5f;

		[SerializeField]
		private float _mouseRollSmoothSpeed = 6f;

		[SerializeField]
		private float _mouseRollReturnSpeed = 3f;

		[SerializeField]
		private AnimationCurve _mouseRollBySpeedCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		private PlayerMovableModel _playerMovableModel;

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private MultiplayerModel _multiplayerModel;

		private IInputService _inputService;

		private HeadBobbingModel _headBobbingModel;

		private Quaternion _targetRotation;

		private Vector3 _targetPosition;

		private float _currentRoll;

		private bool _isMovingPrev;

		private Coroutine _noiseTransitionCoroutine;

		private float _movementRoll;

		private float _mouseRoll;

		private float _targetMouseRoll;

		private float _lastMouseInputTime;

		private float _initialFrequencyGain;

		private float _initialAmplitudeGain;

		private IStat _speedStat;

		private IStat _crouchSpeedStat;

		private IStat _sprintSpeedStat;

		[Inject]
		public void InjectDependencies(PlayerMovableModel playerMovableModel, IInputService inputService, SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel, HeadBobbingModel headBobbingModel)
		{
			_playerMovableModel = playerMovableModel;
			_inputService = inputService;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
			_headBobbingModel = headBobbingModel;
		}

		private void OnEnable()
		{
			InputVector2Actions rotation = _inputService.Rotation;
			rotation.VectorChangedPerformed = (Action<Vector2>)Delegate.Combine(rotation.VectorChangedPerformed, new Action<Vector2>(OnMouseRotation));
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				InitializePlayerStats(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			}
			else
			{
				_spawnedEntityStatsModel.OnPlayerStatRegistered += InitializePlayerStats;
			}
		}

		private void OnDisable()
		{
			InputVector2Actions rotation = _inputService.Rotation;
			rotation.VectorChangedPerformed = (Action<Vector2>)Delegate.Remove(rotation.VectorChangedPerformed, new Action<Vector2>(OnMouseRotation));
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= InitializePlayerStats;
		}

		private void Start()
		{
			_cameraControllerBase.SetNoiseType(_initialNoiseType);
			_initialFrequencyGain = _cameraControllerBase.GetFrequencyGain();
			_initialAmplitudeGain = _cameraControllerBase.GetAmplitudeGain();
		}

		private void Update()
		{
			if (_playerMovableModel != null && !(_playerMovableModel.LocalMovable == null))
			{
				bool flag = _playerMovableModel.RotationMode == PlayerRotationMode.HeadAndBodyRotation;
				HandleNoiseTransition(flag && IsMovableMoving());
				UpdateMovementRoll(flag);
				UpdateMouseRoll();
				ApplyFinalRoll();
			}
		}

		private bool IsMovableMoving()
		{
			if (!Mathf.Approximately(_playerMovableModel.LocalMovable.GetVelocity().magnitude, 0f))
			{
				return !Mathf.Approximately(_playerMovableModel.CurrentMovementInput.magnitude, 0f);
			}
			return false;
		}

		private void HandleNoiseTransition(bool isMoving)
		{
			if (isMoving != _isMovingPrev)
			{
				if (_noiseTransitionCoroutine != null)
				{
					StopCoroutine(_noiseTransitionCoroutine);
				}
				_noiseTransitionCoroutine = StartCoroutine(_cameraControllerBase.TransitionToNoise(GetNoiseTypeByMovement(isMoving), _noiseTransitionTime));
			}
			if (isMoving && _speedStat != null && !Mathf.Approximately(_crouchSpeedStat.FullValue, _sprintSpeedStat.FullValue))
			{
				float time = Mathf.Clamp01(1f - (_sprintSpeedStat.FullValue - _speedStat.FullValue) / (_sprintSpeedStat.FullValue - _crouchSpeedStat.FullValue));
				_cameraControllerBase.SetFrequencyGain((_initialFrequencyGain + _screenShakeFrequencyBasedOnSpeed.Evaluate(time) * _noiseFrequencyGainStrength) * _headBobbingModel.HeadBobbingIntensityNormalized);
				_cameraControllerBase.SetAmplitudeGain((_initialAmplitudeGain + _screenShakeAmplitudeBasedOnSpeed.Evaluate(time) * _noiseAmplitudeGainStrength) * _headBobbingModel.HeadBobbingIntensityNormalized);
			}
			else if (!isMoving)
			{
				_cameraControllerBase.SetFrequencyGain(_initialFrequencyGain * _headBobbingModel.HeadBobbingIntensityNormalized);
				_cameraControllerBase.SetAmplitudeGain(_initialAmplitudeGain * _headBobbingModel.HeadBobbingIntensityNormalized);
			}
			_isMovingPrev = isMoving;
		}

		private void UpdateMovementRoll(bool isMovementDriven)
		{
			float num = (isMovementDriven ? _playerMovableModel.CurrentMovementInput.x : 0f);
			float b = 0f;
			if (!Mathf.Approximately(num, 0f))
			{
				float time = Mathf.Clamp01(Mathf.Abs(num));
				float num2 = _rollCurve.Evaluate(time);
				b = Mathf.Sign(num) * num2 * _maxRollAngle;
			}
			_movementRoll = Mathf.Lerp(_movementRoll, b, Time.deltaTime * _rollSpeed);
		}

		private void UpdateMouseRoll()
		{
			bool num = Time.time - _lastMouseInputTime > 0.05f;
			float b = (num ? 0f : _targetMouseRoll);
			float num2 = (num ? _mouseRollReturnSpeed : _mouseRollSmoothSpeed);
			_mouseRoll = Mathf.Lerp(_mouseRoll, b, Time.deltaTime * num2);
		}

		private void ApplyFinalRoll()
		{
			if (!(_rollExtender == null))
			{
				_rollExtender.RollValue = (_movementRoll + _mouseRoll) * _headBobbingModel.HeadBobbingIntensityNormalized;
			}
		}

		private NoiseType GetNoiseTypeByMovement(bool isMoving)
		{
			if (!isMoving)
			{
				return NoiseType.Idle;
			}
			return NoiseType.Walking;
		}

		private void OnMouseRotation(Vector2 rotationDelta)
		{
			float f = 0f - rotationDelta.x;
			float time = Mathf.Clamp01(Mathf.Abs(f) / 5f);
			float num = _mouseRollBySpeedCurve.Evaluate(time);
			_targetMouseRoll = Mathf.Sign(f) * num * _mouseMaxRollAngle;
			_lastMouseInputTime = Time.time;
		}

		private void InitializePlayerStats(int playerId)
		{
			if (playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				_speedStat = _spawnedEntityStatsModel.PlayerStats[playerId].GetStat(EntityStatType.Speed);
				_crouchSpeedStat = _spawnedEntityStatsModel.PlayerStats[playerId].GetStat(EntityStatType.CrouchSpeed);
				_sprintSpeedStat = _spawnedEntityStatsModel.PlayerStats[playerId].GetStat(EntityStatType.SprintSpeed);
			}
		}
	}
}
