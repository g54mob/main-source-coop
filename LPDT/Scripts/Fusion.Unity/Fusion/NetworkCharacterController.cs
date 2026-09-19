using UnityEngine;

namespace Fusion
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(CharacterController))]
	[NetworkBehaviourWeaved(18)]
	public sealed class NetworkCharacterController : NetworkTRSP, INetworkTRSPTeleport, IBeforeAllTicks, IPublicFacingInterface, IAfterAllTicks, IBeforeCopyPreviousState
	{
		[Header("Character Controller Settings")]
		public float gravity = -20f;

		public float jumpImpulse = 8f;

		public float acceleration = 10f;

		public float braking = 10f;

		public float maxSpeed = 2f;

		public float rotationSpeed = 15f;

		private Tick _initial;

		private CharacterController _controller;

		private ref NetworkCCData CCData => ref ReinterpretState<NetworkCCData>(14);

		public Vector3 Velocity
		{
			get
			{
				return CCData.Velocity;
			}
			set
			{
				CCData.Velocity = value;
			}
		}

		public bool Grounded
		{
			get
			{
				return CCData.Grounded;
			}
			set
			{
				CCData.Grounded = value;
			}
		}

		public void Teleport(Vector3? position = null, Quaternion? rotation = null)
		{
			_controller.enabled = false;
			NetworkTRSP.Teleport(this, base.transform, position, rotation);
			_controller.enabled = true;
		}

		public void Jump(bool ignoreGrounded = false, float? overrideImpulse = null)
		{
			if (CCData.Grounded || ignoreGrounded)
			{
				Vector3 velocity = CCData.Velocity;
				velocity.y += overrideImpulse ?? jumpImpulse;
				CCData.Velocity = velocity;
			}
		}

		public void Move(Vector3 direction)
		{
			float deltaTime = base.Runner.DeltaTime;
			Vector3 position = base.transform.position;
			Vector3 velocity = CCData.Velocity;
			direction = direction.normalized;
			if (CCData.Grounded && velocity.y < 0f)
			{
				velocity.y = 0f;
			}
			velocity.y += gravity * base.Runner.DeltaTime;
			Vector3 vector = new Vector3
			{
				x = velocity.x,
				z = velocity.z
			};
			if (direction == default(Vector3))
			{
				vector = Vector3.Lerp(vector, default(Vector3), braking * deltaTime);
			}
			else
			{
				vector = Vector3.ClampMagnitude(vector + direction * acceleration * deltaTime, maxSpeed);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * base.Runner.DeltaTime);
			}
			velocity.x = vector.x;
			velocity.z = vector.z;
			_controller.Move(velocity * deltaTime);
			CCData.Velocity = (base.transform.position - position) * base.Runner.TickRate;
			CCData.Grounded = _controller.isGrounded;
		}

		public override void Spawned()
		{
			_initial = default(Tick);
			TryGetComponent<CharacterController>(out _controller);
			_controller.enabled = false;
			if (base.Object.HasStateAuthority && base.Object.LastReceiveTick == default(Tick))
			{
				CopyToBuffer();
			}
			else
			{
				CopyToEngine();
			}
			_controller.enabled = true;
		}

		public override void Render()
		{
			NetworkTRSP.Render(this, base.transform, syncScale: false, syncParent: false, local: false, ref _initial);
		}

		void IBeforeAllTicks.BeforeAllTicks(bool resimulation, int tickCount)
		{
			CopyToEngine();
		}

		void IAfterAllTicks.AfterAllTicks(bool resimulation, int tickCount)
		{
			CopyToBuffer();
		}

		void IBeforeCopyPreviousState.BeforeCopyPreviousState()
		{
			CopyToBuffer();
		}

		private void Awake()
		{
			TryGetComponent<CharacterController>(out _controller);
		}

		private void CopyToBuffer()
		{
			ref NetworkTRSPData state = ref base.State;
			state.Position = base.transform.position;
			state.Rotation = base.transform.rotation;
		}

		private void CopyToEngine()
		{
			_controller.enabled = false;
			ref NetworkTRSPData state = ref base.State;
			base.transform.SetPositionAndRotation(state.Position, state.Rotation);
			_controller.enabled = true;
		}
	}
}
