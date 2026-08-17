using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mirror.Examples.Common.Controllers.Flyer
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(NetworkIdentity))]
	[DisallowMultipleComponent]
	public class FlyerControllerBase : NetworkBehaviour
	{
		[Serializable]
		public struct OptionsKeys
		{
			public KeyCode MouseSteer;

			public KeyCode AutoRun;

			public KeyCode ToggleUI;
		}

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
		}

		[Serializable]
		public struct FlightKeys
		{
			public KeyCode PitchDown;

			public KeyCode PitchUp;

			public KeyCode RollLeft;

			public KeyCode RollRight;

			public KeyCode AutoLevel;
		}

		[Flags]
		public enum ControlOptions : byte
		{
			None = 0,
			MouseSteer = 1,
			AutoRun = 2,
			AutoLevel = 4,
			ShowUI = 8
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
			[Range(-180f, 180f)]
			private float _pitchAngle;

			[ReadOnly]
			[SerializeField]
			[Range(-180f, 180f)]
			private float _pitchSpeed;

			[ReadOnly]
			[SerializeField]
			[Range(-180f, 180f)]
			private float _rollAngle;

			[ReadOnly]
			[SerializeField]
			[Range(-180f, 180f)]
			private float _rollSpeed;

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

			public float pitchAngle
			{
				get
				{
					return _pitchAngle;
				}
				internal set
				{
					_pitchAngle = value;
				}
			}

			public float pitchSpeed
			{
				get
				{
					return _pitchSpeed;
				}
				internal set
				{
					_pitchSpeed = value;
				}
			}

			public float rollAngle
			{
				get
				{
					return _rollAngle;
				}
				internal set
				{
					_rollAngle = value;
				}
			}

			public float rollSpeed
			{
				get
				{
					return _rollSpeed;
				}
				internal set
				{
					_rollSpeed = value;
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
		public CapsuleCollider capsuleCollider;

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
			TurnRight = KeyCode.E
		};

		[SerializeField]
		public FlightKeys flightKeys = new FlightKeys
		{
			PitchDown = KeyCode.UpArrow,
			PitchUp = KeyCode.DownArrow,
			RollLeft = KeyCode.LeftArrow,
			RollRight = KeyCode.RightArrow,
			AutoLevel = KeyCode.L
		};

		[SerializeField]
		public OptionsKeys optionsKeys = new OptionsKeys
		{
			MouseSteer = KeyCode.M,
			AutoRun = KeyCode.R,
			ToggleUI = KeyCode.U
		};

		[Space(5f)]
		public ControlOptions controlOptions = ControlOptions.AutoLevel | ControlOptions.ShowUI;

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

		[Header("Pitch")]
		[Range(0f, 180f)]
		[Tooltip("Max Pitch in degrees per second")]
		public float maxPitchSpeed = 30f;

		[Range(0f, 180f)]
		[Tooltip("Max Pitch in degrees")]
		public float maxPitchUpAngle = 20f;

		[Range(0f, 180f)]
		[Tooltip("Max Pitch in degrees")]
		public float maxPitchDownAngle = 45f;

		[Range(0f, 10f)]
		[Tooltip("Pitch acceleration in degrees per second squared")]
		public float pitchAcceleration = 3f;

		[Header("Roll")]
		[Range(0f, 180f)]
		[Tooltip("Max Roll in degrees per second")]
		public float maxRollSpeed = 30f;

		[Range(0f, 180f)]
		[Tooltip("Max Roll in degrees")]
		public float maxRollAngle = 45f;

		[Range(0f, 10f)]
		[Tooltip("Roll acceleration in degrees per second squared")]
		public float rollAcceleration = 3f;

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
			if (capsuleCollider == null)
			{
				capsuleCollider = GetComponent<CapsuleCollider>();
			}
			capsuleCollider.enabled = true;
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

		public override void OnStartAuthority()
		{
			float num = ((Screen.dpi > 0f) ? (Screen.dpi / 96f) : 1f);
			runtimeData.mouseSensitivity = turnAcceleration * num;
			SetCursor(controlOptions.HasFlag(ControlOptions.MouseSteer));
			capsuleCollider.enabled = false;
			characterController.enabled = true;
			base.enabled = true;
		}

		public override void OnStopAuthority()
		{
			base.enabled = false;
			capsuleCollider.enabled = true;
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
				if (runtimeData.controllerUI.TryGetComponent<FlyerControllerUI>(out var component))
				{
					component.Refresh(moveKeys, flightKeys, optionsKeys);
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
			if (Application.isFocused && characterController.enabled)
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
				HandlePitch(deltaTime);
				HandleRoll(deltaTime);
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
			if (flightKeys.AutoLevel != KeyCode.None && Input.GetKeyUp(flightKeys.AutoLevel))
			{
				controlOptions ^= ControlOptions.AutoLevel;
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
			runtimeData.turnSpeed = Mathf.MoveTowards(runtimeData.turnSpeed, num, turnAcceleration * maxTurnSpeed * deltaTime);
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

		private void HandlePitch(float deltaTime)
		{
			float num = 0f;
			bool flag = false;
			if (flightKeys.PitchUp != KeyCode.None && Input.GetKey(flightKeys.PitchUp))
			{
				num -= maxPitchSpeed;
				flag = true;
			}
			if (flightKeys.PitchDown != KeyCode.None && Input.GetKey(flightKeys.PitchDown))
			{
				num += maxPitchSpeed;
				flag = true;
			}
			runtimeData.pitchSpeed = Mathf.MoveTowards(runtimeData.pitchSpeed, num, pitchAcceleration * maxPitchSpeed * deltaTime);
			runtimeData.pitchAngle += runtimeData.pitchSpeed * deltaTime;
			runtimeData.pitchAngle = Mathf.Clamp(runtimeData.pitchAngle, 0f - maxPitchUpAngle, maxPitchDownAngle);
			if (!flag && controlOptions.HasFlag(ControlOptions.AutoLevel))
			{
				runtimeData.pitchAngle = Mathf.MoveTowards(runtimeData.pitchAngle, 0f, maxPitchSpeed * deltaTime);
			}
			ApplyRotation();
		}

		private void HandleRoll(float deltaTime)
		{
			float num = 0f;
			bool flag = false;
			if (flightKeys.RollRight != KeyCode.None && Input.GetKey(flightKeys.RollRight))
			{
				num -= maxRollSpeed;
				flag = true;
			}
			if (flightKeys.RollLeft != KeyCode.None && Input.GetKey(flightKeys.RollLeft))
			{
				num += maxRollSpeed;
				flag = true;
			}
			runtimeData.rollSpeed = Mathf.MoveTowards(runtimeData.rollSpeed, num, rollAcceleration * maxRollSpeed * deltaTime);
			runtimeData.rollAngle += runtimeData.rollSpeed * deltaTime;
			runtimeData.rollAngle = Mathf.Clamp(runtimeData.rollAngle, 0f - maxRollAngle, maxRollAngle);
			if (!flag && controlOptions.HasFlag(ControlOptions.AutoLevel))
			{
				runtimeData.rollAngle = Mathf.MoveTowards(runtimeData.rollAngle, 0f, maxRollSpeed * deltaTime);
			}
			ApplyRotation();
		}

		private void ApplyRotation()
		{
			float y = base.transform.localRotation.eulerAngles.y;
			base.transform.localRotation = Quaternion.Euler(runtimeData.pitchAngle, y, runtimeData.rollAngle);
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
			characterController.Move(runtimeData.direction * deltaTime);
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
