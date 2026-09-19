using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Synty.AnimationBaseLocomotion.Samples.InputSystem
{
	public class InputReader : MonoBehaviour, Controls.IPlayerActions
	{
		public Vector2 _mouseDelta;

		public Vector2 _moveComposite;

		public float _movementInputDuration;

		public bool _movementInputDetected;

		private Controls _controls;

		public Action onAimActivated;

		public Action onAimDeactivated;

		public Action onCrouchActivated;

		public Action onCrouchDeactivated;

		public Action onJumpPerformed;

		public Action onLockOnToggled;

		public Action onSprintActivated;

		public Action onSprintDeactivated;

		public Action onWalkToggled;

		private void OnEnable()
		{
			if (_controls == null)
			{
				_controls = new Controls();
				_controls.Player.SetCallbacks(this);
			}
			_controls.Player.Enable();
		}

		public void OnDisable()
		{
			_controls.Player.Disable();
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			_mouseDelta = context.ReadValue<Vector2>();
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			_moveComposite = context.ReadValue<Vector2>();
			_movementInputDetected = _moveComposite.magnitude > 0f;
		}

		public void OnJump(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				onJumpPerformed?.Invoke();
			}
		}

		public void OnToggleWalk(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				onWalkToggled?.Invoke();
			}
		}

		public void OnSprint(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				onSprintActivated?.Invoke();
			}
			else if (context.canceled)
			{
				onSprintDeactivated?.Invoke();
			}
		}

		public void OnCrouch(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				onCrouchActivated?.Invoke();
			}
			else if (context.canceled)
			{
				onCrouchDeactivated?.Invoke();
			}
		}

		public void OnAim(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				onAimActivated?.Invoke();
			}
			if (context.canceled)
			{
				onAimDeactivated?.Invoke();
			}
		}

		public void OnLockOn(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				onLockOnToggled?.Invoke();
				onSprintDeactivated?.Invoke();
			}
		}
	}
}
