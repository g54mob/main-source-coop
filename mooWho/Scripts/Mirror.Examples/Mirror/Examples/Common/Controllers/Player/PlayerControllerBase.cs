using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mirror.Examples.Common.Controllers.Player
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(NetworkIdentity))]
	[DisallowMultipleComponent]
	public class PlayerControllerBase : NetworkBehaviour
	{
		public enum GroundState : byte
		{
			Grounded = 0,
			Jumping = 1,
			Falling = 2
		}

		[Serializable]
		public struct MoveKeys
		{
			public KeyCode Forward;

			public KeyCode Back;

			public KeyCode StrafeLeft;

			public KeyCode StrafeRight;

			public KeyCode TurnLeft;

			public KeyCode TurnRight;

			public KeyCode Jump;
		}

		[Serializable]
		public struct OptionsKeys
		{
			public KeyCode MouseSteer;

			public KeyCode AutoRun;

			public KeyCode ToggleUI;
		}

		[Flags]
		public enum ControlOptions : byte
		{
			None = 0,
			MouseSteer = 1,
			AutoRun = 2,
			ShowUI = 4
		}

		[Serializable]
		public struct RuntimeData
		{
			[ReadOnly]
			[SerializeField]
			[Range(-1f, 1f)]
			private float _horizontal;

			[ReadOnly]
			[SerializeField]
			[Range(-1f, 1f)]
			private float _vertical;

			[ReadOnly]
			[SerializeField]
			[Range(-300f, 300f)]
			private float _turnSpeed;

			[ReadOnly]
			[SerializeField]
			[Range(-10f, 10f)]
			private float _jumpSpeed;

			[ReadOnly]
			[SerializeField]
			[Range(-1.5f, 1.5f)]
			private float _animVelocity;

			[ReadOnly]
			[SerializeField]
			[Range(-1.5f, 1.5f)]
			private float _animRotation;

			[ReadOnly]
			[SerializeField]
			[Range(-1f, 1f)]
			private float _mouseInputX;

			[ReadOnly]
			[SerializeField]
			[Range(0f, 30f)]
			private float _mouseSensitivity;

			[ReadOnly]
			[SerializeField]
			private GroundState _groundState;

			[ReadOnly]
			[SerializeField]
			private Vector3 _direction;

			[ReadOnly]
			[SerializeField]
			private Vector3Int _velocity;

			[ReadOnly]
			[SerializeField]
			private GameObject _controllerUI;

			public float horizontal
			{
				get
				{
					return _horizontal;
				}
				internal set
				{
					_horizontal = value;
				}
			}

			public float vertical
			{
				get
				{
					return _vertical;
				}
				internal set
				{
					_vertical = value;
				}
			}

			public float turnSpeed
			{
				get
				{
					return _turnSpeed;
				}
				internal set
				{
					_turnSpeed = value;
				}
			}

			public float jumpSpeed
			{
				get
				{
					return _jumpSpeed;
				}
				internal set
				{
					_jumpSpeed = value;
				}
			}

			public float animVelocity
			{
				get
				{
					return _animVelocity;
				}
				internal set
				{
					_animVelocity = value;
				}
			}

			public float animRotation
			{
				get
				{
					return _animRotation;
				}
				internal set
				{
					_animRotation = value;
				}
			}

			public float mouseInputX
			{
				get
				{
					return _mouseInputX;
				}
				internal set
				{
					_mouseInputX = value;
				}
			}

			public float mouseSensitivity
			{
				get
				{
					return _mouseSensitivity;
				}
				internal set
				{
					_mouseSensitivity = value;
				}
			}

			public GroundState groundState
			{
				get
				{
					return _groundState;
				}
				internal set
				{
					_groundState = value;
				}
			}

			public Vector3 direction
			{
				get
				{
					return _direction;
				}
				internal set
				{
					_direction = value;
				}
			}

			public Vector3Int velocity
			{
				get
				{
					return _velocity;
				}
				internal set
				{
					_velocity = value;
				}
			}

			public GameObject controllerUI
			{
				get
				{
					return _controllerUI;
				}
				internal set
				{
					_controllerUI = value;
				}
			}
		}

		private const float BASE_DPI = 96f;

		[Header("Avatar Components")]
		public CharacterController characterController;

		[Header("User Interface")]
		public GameObject ControllerUIPrefab;

		[Header("Configuration")]
		[SerializeField]
		public MoveKeys moveKeys = new MoveKeys
		{
			Forward = KeyCode.W,
			Back = KeyCode.S,
			StrafeLeft = KeyCode.A,
			StrafeRight = KeyCode.D,
			TurnLeft = KeyCode.Q,
			TurnRight = KeyCode.E,
			Jump = KeyCode.Space
		};

		[SerializeField]
		public OptionsKeys optionsKeys = new OptionsKeys
		{
			MouseSteer = KeyCode.M,
			AutoRun = KeyCode.R,
			ToggleUI = KeyCode.U
		};

		[Space(5f)]
		public ControlOptions controlOptions = ControlOptions.ShowUI;

		[Header("Movement")]
		[Range(0f, 20f)]
		[FormerlySerializedAs("moveSpeedMultiplier")]
		[Tooltip("Speed in meters per second")]
		public float maxMoveSpeed = 8f;

		[Range(0f, 10f)]
		[Tooltip("Sensitivity factors into accelleration")]
		public float inputSensitivity = 2f;

		[Range(0f, 10f)]
		[Tooltip("Gravity factors into decelleration")]
		public float inputGravity = 2f;

		[Header("Turning")]
		[Range(0f, 300f)]
		[Tooltip("Max Rotation in degrees per second")]
		public float maxTurnSpeed = 100f;

		[Range(0f, 10f)]
		[FormerlySerializedAs("turnDelta")]
		[Tooltip("Rotation acceleration in degrees per second squared")]
		public float turnAcceleration = 3f;

		[Header("Jumping")]
		[Range(0f, 10f)]
		[Tooltip("Initial jump speed in meters per second")]
		public float initialJumpSpeed = 2.5f;

		[Range(0f, 10f)]
		[Tooltip("Maximum jump speed in meters per second")]
		public float maxJumpSpeed = 3.5f;

		[Range(0f, 10f)]
		[FormerlySerializedAs("jumpDelta")]
		[Tooltip("Jump acceleration in meters per second squared")]
		public float jumpAcceleration = 4f;

		[Header("Diagnostics")]
		public RuntimeData runtimeData;

		protected override void OnValidate()
		{
			if (!Application.isPlaying)
			{
				base.OnValidate();
				Reset();
			}
		}

		private void Reset()
		{
			if (characterController == null)
			{
				characterController = GetComponent<CharacterController>();
			}
			characterController.enabled = false;
			characterController.skinWidth = 0.02f;
			characterController.minMoveDistance = 0f;
			GetComponent<Rigidbody>().isKinematic = true;
			base.enabled = false;
		}

		private void OnDisable()
		{
			runtimeData.horizontal = 0f;
			runtimeData.vertical = 0f;
			runtimeData.turnSpeed = 0f;
		}

		public override void OnStartAuthority()
		{
			float num = ((Screen.dpi > 0f) ? (Screen.dpi / 96f) : 1f);
			runtimeData.mouseSensitivity = turnAcceleration * num;
			SetCursor(controlOptions.HasFlag(ControlOptions.MouseSteer));
			characterController.enabled = true;
			base.enabled = true;
		}

		public override void OnStopAuthority()
		{
			base.enabled = false;
			characterController.enabled = false;
			SetCursor(locked: false);
		}

		public override void OnStartLocalPlayer()
		{
			if (ControllerUIPrefab != null)
			{
				runtimeData.controllerUI = UnityEngine.Object.Instantiate(ControllerUIPrefab);
			}
			if (runtimeData.controllerUI != null)
			{
				if (runtimeData.controllerUI.TryGetComponent<PlayerControllerUI>(out var component))
				{
					component.Refresh(moveKeys, optionsKeys);
				}
				runtimeData.controllerUI.SetActive(controlOptions.HasFlag(ControlOptions.ShowUI));
			}
		}

		public override void OnStopLocalPlayer()
		{
			if (runtimeData.controllerUI != null)
			{
				UnityEngine.Object.Destroy(runtimeData.controllerUI);
			}
			runtimeData.controllerUI = null;
		}

		private void Update()
		{
			if (characterController.enabled)
			{
				float deltaTime = Time.deltaTime;
				HandleOptions();
				if (controlOptions.HasFlag(ControlOptions.MouseSteer))
				{
					HandleMouseSteer(deltaTime);
				}
				else
				{
					HandleTurning(deltaTime);
				}
				HandleJumping(deltaTime);
				HandleMove(deltaTime);
				ApplyMove(deltaTime);
				if (characterController.isGrounded)
				{
					runtimeData.groundState = GroundState.Grounded;
				}
				else if (runtimeData.groundState != GroundState.Jumping)
				{
					runtimeData.groundState = GroundState.Falling;
				}
				runtimeData.velocity = Vector3Int.FloorToInt(characterController.velocity);
			}
		}

		private void SetCursor(bool locked)
		{
			Cursor.lockState = (locked ? CursorLockMode.Locked : CursorLockMode.None);
			Cursor.visible = !locked;
		}

		private void HandleOptions()
		{
			if (optionsKeys.MouseSteer != KeyCode.None && Input.GetKeyUp(optionsKeys.MouseSteer))
			{
				controlOptions ^= ControlOptions.MouseSteer;
				SetCursor(controlOptions.HasFlag(ControlOptions.MouseSteer));
			}
			if (optionsKeys.AutoRun != KeyCode.None && Input.GetKeyUp(optionsKeys.AutoRun))
			{
				controlOptions ^= ControlOptions.AutoRun;
			}
			if (optionsKeys.ToggleUI != KeyCode.None && Input.GetKeyUp(optionsKeys.ToggleUI))
			{
				controlOptions ^= ControlOptions.ShowUI;
				if (runtimeData.controllerUI != null)
				{
					runtimeData.controllerUI.SetActive(controlOptions.HasFlag(ControlOptions.ShowUI));
				}
			}
		}

		private void HandleTurning(float deltaTime)
		{
			float num = 0f;
			if (moveKeys.TurnLeft != KeyCode.None && Input.GetKey(moveKeys.TurnLeft))
			{
				num -= maxTurnSpeed;
			}
			if (moveKeys.TurnRight != KeyCode.None && Input.GetKey(moveKeys.TurnRight))
			{
				num += maxTurnSpeed;
			}
			if (num != 0f || !controlOptions.HasFlag(ControlOptions.AutoRun))
			{
				runtimeData.turnSpeed = Mathf.MoveTowards(runtimeData.turnSpeed, num, turnAcceleration * maxTurnSpeed * deltaTime);
			}
			base.transform.Rotate(0f, runtimeData.turnSpeed * deltaTime, 0f);
		}

		private void HandleMouseSteer(float deltaTime)
		{
			runtimeData.mouseInputX += Input.GetAxisRaw("Mouse X") * runtimeData.mouseSensitivity;
			runtimeData.mouseInputX = Mathf.Clamp(runtimeData.mouseInputX, -1f, 1f);
			float target = runtimeData.mouseInputX * maxTurnSpeed;
			runtimeData.turnSpeed = Mathf.MoveTowards(runtimeData.turnSpeed, target, runtimeData.mouseSensitivity * maxTurnSpeed * deltaTime);
			base.transform.Rotate(0f, runtimeData.turnSpeed * deltaTime, 0f);
			runtimeData.mouseInputX = Mathf.MoveTowards(runtimeData.mouseInputX, 0f, runtimeData.mouseSensitivity * deltaTime);
		}

		private void HandleJumping(float deltaTime)
		{
			if (runtimeData.groundState != GroundState.Falling && moveKeys.Jump != KeyCode.None && Input.GetKey(moveKeys.Jump))
			{
				if (runtimeData.groundState != GroundState.Jumping)
				{
					runtimeData.groundState = GroundState.Jumping;
					runtimeData.jumpSpeed = initialJumpSpeed;
				}
				else if (runtimeData.jumpSpeed < maxJumpSpeed)
				{
					float num = (runtimeData.jumpSpeed - initialJumpSpeed) / (maxJumpSpeed - initialJumpSpeed);
					runtimeData.jumpSpeed += jumpAcceleration * Mathf.Sqrt(1f - num) * deltaTime;
				}
				if (runtimeData.jumpSpeed >= maxJumpSpeed)
				{
					runtimeData.jumpSpeed = maxJumpSpeed;
					runtimeData.groundState = GroundState.Falling;
				}
			}
			else if (runtimeData.groundState != GroundState.Grounded)
			{
				runtimeData.groundState = GroundState.Falling;
				runtimeData.jumpSpeed = Mathf.Min(runtimeData.jumpSpeed, maxJumpSpeed);
				runtimeData.jumpSpeed += Physics.gravity.y * deltaTime;
			}
			else
			{
				runtimeData.jumpSpeed = Physics.gravity.y * deltaTime;
			}
		}

		private void HandleMove(float deltaTime)
		{
			float num = 0f;
			float num2 = 0f;
			if (moveKeys.Forward != KeyCode.None && Input.GetKey(moveKeys.Forward))
			{
				num2 = 1f;
			}
			if (moveKeys.Back != KeyCode.None && Input.GetKey(moveKeys.Back))
			{
				num2 = -1f;
			}
			if (moveKeys.StrafeLeft != KeyCode.None && Input.GetKey(moveKeys.StrafeLeft))
			{
				num = -1f;
			}
			if (moveKeys.StrafeRight != KeyCode.None && Input.GetKey(moveKeys.StrafeRight))
			{
				num = 1f;
			}
			if (num == 0f)
			{
				if (!controlOptions.HasFlag(ControlOptions.AutoRun))
				{
					runtimeData.horizontal = Mathf.MoveTowards(runtimeData.horizontal, num, inputGravity * deltaTime);
				}
			}
			else
			{
				runtimeData.horizontal = Mathf.MoveTowards(runtimeData.horizontal, num, inputSensitivity * deltaTime);
			}
			if (num2 == 0f)
			{
				if (!controlOptions.HasFlag(ControlOptions.AutoRun))
				{
					runtimeData.vertical = Mathf.MoveTowards(runtimeData.vertical, num2, inputGravity * deltaTime);
				}
			}
			else
			{
				runtimeData.vertical = Mathf.MoveTowards(runtimeData.vertical, num2, inputSensitivity * deltaTime);
			}
		}

		private void ApplyMove(float deltaTime)
		{
			runtimeData.direction = new Vector3(runtimeData.horizontal, 0f, runtimeData.vertical);
			runtimeData.direction = Vector3.ClampMagnitude(runtimeData.direction, 1f);
			runtimeData.direction = base.transform.TransformDirection(runtimeData.direction);
			runtimeData.direction *= maxMoveSpeed;
			runtimeData.direction = new Vector3(runtimeData.direction.x, runtimeData.jumpSpeed, runtimeData.direction.z);
			characterController.Move(runtimeData.direction * deltaTime);
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
