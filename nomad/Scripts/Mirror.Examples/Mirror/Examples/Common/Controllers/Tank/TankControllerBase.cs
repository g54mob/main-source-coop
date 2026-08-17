using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mirror.Examples.Common.Controllers.Tank
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(NetworkIdentity))]
	[RequireComponent(typeof(TankHealth))]
	[DisallowMultipleComponent]
	public class TankControllerBase : NetworkBehaviour
	{
		public enum GroundState : byte
		{
			Grounded = 0,
			Falling = 1
		}

		[Serializable]
		public struct MoveKeys
		{
			public KeyCode Forward;

			public KeyCode Back;

			public KeyCode TurnLeft;

			public KeyCode TurnRight;
		}

		[Serializable]
		public struct OptionsKeys
		{
			public KeyCode AutoRun;

			public KeyCode ToggleUI;
		}

		[Flags]
		public enum ControlOptions : byte
		{
			None = 0,
			AutoRun = 1,
			ShowUI = 2
		}

		[Serializable]
		public struct RuntimeData
		{
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
			[Range(-1.5f, 1.5f)]
			private float _animVelocity;

			[ReadOnly]
			[SerializeField]
			[Range(-1.5f, 1.5f)]
			private float _animRotation;

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
		}

		[Header("Components")]
		public BoxCollider boxCollider;

		public CharacterController characterController;

		[Header("User Interface")]
		public GameObject ControllerUIPrefab;

		[Header("Configuration")]
		[SerializeField]
		public MoveKeys moveKeys = new MoveKeys
		{
			Forward = KeyCode.W,
			Back = KeyCode.S,
			TurnLeft = KeyCode.A,
			TurnRight = KeyCode.D
		};

		[SerializeField]
		public OptionsKeys optionsKeys = new OptionsKeys
		{
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

		protected virtual void Reset()
		{
			if (boxCollider == null)
			{
				boxCollider = GetComponentInChildren<BoxCollider>();
			}
			boxCollider.enabled = true;
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
			runtimeData.vertical = 0f;
			runtimeData.turnSpeed = 0f;
		}

		public override void OnStartAuthority()
		{
			characterController.enabled = true;
			base.enabled = true;
		}

		public override void OnStopAuthority()
		{
			base.enabled = false;
			characterController.enabled = false;
		}

		public override void OnStartLocalPlayer()
		{
			if (ControllerUIPrefab != null)
			{
				runtimeData.controllerUI = UnityEngine.Object.Instantiate(ControllerUIPrefab);
			}
			if (runtimeData.controllerUI != null)
			{
				if (runtimeData.controllerUI.TryGetComponent<TankControllerUI>(out var component))
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
				HandleTurning(deltaTime);
				HandleMove(deltaTime);
				ApplyMove(deltaTime);
				runtimeData.groundState = ((!characterController.isGrounded) ? GroundState.Falling : GroundState.Grounded);
				runtimeData.velocity = Vector3Int.FloorToInt(characterController.velocity);
			}
		}

		private void HandleOptions()
		{
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

		private void HandleMove(float deltaTime)
		{
			float num = 0f;
			if (moveKeys.Forward != KeyCode.None && Input.GetKey(moveKeys.Forward))
			{
				num = 1f;
			}
			if (moveKeys.Back != KeyCode.None && Input.GetKey(moveKeys.Back))
			{
				num = -1f;
			}
			if (num == 0f)
			{
				if (!controlOptions.HasFlag(ControlOptions.AutoRun))
				{
					runtimeData.vertical = Mathf.MoveTowards(runtimeData.vertical, num, inputGravity * deltaTime);
				}
			}
			else
			{
				runtimeData.vertical = Mathf.MoveTowards(runtimeData.vertical, num, inputSensitivity * deltaTime);
			}
		}

		private void ApplyMove(float deltaTime)
		{
			runtimeData.direction = new Vector3(0f, 0f, runtimeData.vertical);
			runtimeData.direction = base.transform.TransformDirection(runtimeData.direction);
			runtimeData.direction *= maxMoveSpeed;
			runtimeData.direction += Physics.gravity;
			characterController.Move(runtimeData.direction * deltaTime);
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
