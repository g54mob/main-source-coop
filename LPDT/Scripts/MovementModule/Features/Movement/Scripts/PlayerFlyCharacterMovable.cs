using System;
using Features.CameraModelModule;
using Features.InputModule.Scripts.Generated;
using Features.PlayerSpawner.Scripts;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using UnityEngine;
using Zenject;

namespace Features.Movement.Scripts
{
	public class PlayerFlyCharacterMovable : CharacterMovableMonoBase
	{
		[Header("Fly Configuration")]
		[SerializeField]
		private float _flySpeed = 10f;

		[SerializeField]
		private float _acceleration = 50f;

		[SerializeField]
		private float _deceleration = 40f;

		private bool _ignoreMovement;

		private bool _followPlayer = true;

		private Vector3 _currentVelocity;

		private Vector2 _movementInput;

		private PlayerMovableModel _playerMovableModel;

		private CameraModel _cameraModel;

		private IInputService _inputService;

		[Inject]
		public void InjectDependencies(CameraModel cameraModel, PlayerMovableModel playerMovableModel, IInputService inputService)
		{
			_cameraModel = cameraModel;
			_playerMovableModel = playerMovableModel;
			_inputService = inputService;
		}

		private void OnEnable()
		{
			_playerMovableModel.FreeFlyMovable = this;
			InputVector2Actions movement = _inputService.Movement;
			movement.VectorChangedPerformed = (Action<Vector2>)Delegate.Combine(movement.VectorChangedPerformed, new Action<Vector2>(OnMovementInput));
			InputVector2Actions movement2 = _inputService.Movement;
			movement2.VectorChangedCanceled = (Action<Vector2>)Delegate.Combine(movement2.VectorChangedCanceled, new Action<Vector2>(OnMovementCancelled));
		}

		private void OnDisable()
		{
			InputVector2Actions movement = _inputService.Movement;
			movement.VectorChangedPerformed = (Action<Vector2>)Delegate.Remove(movement.VectorChangedPerformed, new Action<Vector2>(OnMovementInput));
			InputVector2Actions movement2 = _inputService.Movement;
			movement2.VectorChangedCanceled = (Action<Vector2>)Delegate.Remove(movement2.VectorChangedCanceled, new Action<Vector2>(OnMovementCancelled));
			if ((object)_playerMovableModel.FreeFlyMovable == this)
			{
				_playerMovableModel.FreeFlyMovable = null;
			}
		}

		private void OnMovementInput(Vector2 input)
		{
			_movementInput = input;
		}

		private void OnMovementCancelled(Vector2 vector2)
		{
			_movementInput = Vector2.zero;
		}

		private void Update()
		{
			if (!(_playerMovableModel.LocalMovable == null))
			{
				if (_followPlayer && !PlayerSpawnLock.ShouldBlockPositionOverride())
				{
					base.transform.position = _playerMovableModel.LocalMovable.GetPosition();
				}
				if (!_ignoreMovement)
				{
					MoveTowardsInput(new Vector3(_movementInput.x, 0f, _movementInput.y));
				}
			}
		}

		public override void MoveTowardsInput(Vector3 input, bool overrideSpeed = false)
		{
			if (!_ignoreMovement)
			{
				Camera cameraObject = _cameraModel.CameraObject;
				Vector3 vector = cameraObject.transform.right * input.x + cameraObject.transform.forward * input.z;
				Vector3 target = ((vector.sqrMagnitude > 0.0001f) ? (vector.normalized * _flySpeed) : Vector3.zero);
				float num = ((vector.sqrMagnitude > 0.0001f) ? _acceleration : _deceleration);
				_currentVelocity = Vector3.MoveTowards(_currentVelocity, target, num * Time.deltaTime);
				base.transform.position += _currentVelocity * Time.deltaTime;
			}
		}

		public override void MoveTowardsPosition(Vector3 position)
		{
			if (!_ignoreMovement)
			{
				Vector3 vector = position - GetPosition();
				if (!(vector.sqrMagnitude < 0.0001f))
				{
					Vector3 target = vector.normalized * _flySpeed;
					float deltaTime = Time.deltaTime;
					_currentVelocity = Vector3.MoveTowards(_currentVelocity, target, _acceleration * deltaTime);
					base.transform.position += _currentVelocity * deltaTime;
				}
			}
		}

		public override void ChangePosition(Vector3 position)
		{
			if (!_ignoreMovement && !PlayerSpawnLock.ShouldBlockPositionOverride())
			{
				base.transform.position = position;
			}
		}

		public override void Warp(Vector3 position)
		{
			if (!PlayerSpawnLock.ShouldBlockPositionOverride())
			{
				_currentVelocity = Vector3.zero;
				base.transform.position = position;
			}
		}

		public override void DisableMovement()
		{
			_ignoreMovement = true;
			_followPlayer = true;
			_movementInput = Vector2.zero;
			_currentVelocity = Vector3.zero;
		}

		public override void EnableMovement()
		{
			_ignoreMovement = false;
			_followPlayer = false;
		}

		public override Vector3 GetVelocity()
		{
			return _currentVelocity;
		}

		public override Vector3 GetPosition()
		{
			return base.transform.position;
		}

		public override bool IsMoving()
		{
			return _currentVelocity.magnitude > 0.1f;
		}

		public override void Jump()
		{
		}

		public override void Crouch()
		{
		}

		public override void ForceCrouch(bool isCrouching)
		{
		}
	}
}
