using UnityEngine;
using UnityEngine.AI;

namespace ECM2
{
	[RequireComponent(typeof(Character))]
	[RequireComponent(typeof(NavMeshAgent))]
	public class NavMeshCharacter : MonoBehaviour
	{
		public delegate void DestinationReachedEventHandler();

		[Space(15f)]
		[Tooltip("Should the agent brake automatically to avoid overshooting the destination point? \nIf true, the agent will brake automatically as it nears the destination.")]
		[SerializeField]
		private bool _autoBraking;

		[Tooltip("Distance from target position to start braking.")]
		[SerializeField]
		private float _brakingDistance;

		[Tooltip("Stop within this distance from the target position.")]
		[SerializeField]
		private float _stoppingDistance;

		private NavMeshAgent _agent;

		private Character _character;

		public NavMeshAgent agent => _agent;

		public Character character => _character;

		public bool autoBraking
		{
			get
			{
				return _autoBraking;
			}
			set
			{
				_autoBraking = value;
				agent.autoBraking = _autoBraking;
			}
		}

		public float brakingDistance
		{
			get
			{
				return _brakingDistance;
			}
			set
			{
				_brakingDistance = Mathf.Max(0.0001f, value);
			}
		}

		public float brakingRatio
		{
			get
			{
				if (!autoBraking)
				{
					return 1f;
				}
				if (!agent.hasPath)
				{
					return 1f;
				}
				return Mathf.InverseLerp(0f, brakingDistance, agent.remainingDistance);
			}
		}

		public float stoppingDistance
		{
			get
			{
				return _stoppingDistance;
			}
			set
			{
				_stoppingDistance = Mathf.Max(0f, value);
				agent.stoppingDistance = _stoppingDistance;
			}
		}

		public event DestinationReachedEventHandler DestinationReached;

		public virtual void OnDestinationReached()
		{
			this.DestinationReached?.Invoke();
		}

		protected virtual void CacheComponents()
		{
			_agent = GetComponent<NavMeshAgent>();
			_character = GetComponent<Character>();
		}

		public virtual bool HasPath()
		{
			return agent.hasPath;
		}

		public virtual bool IsPathFollowing()
		{
			if (agent.hasPath)
			{
				return !agent.isStopped;
			}
			return false;
		}

		public virtual Vector3 GetDestination()
		{
			return agent.destination;
		}

		public virtual void MoveToDestination(Vector3 destination)
		{
			Vector3 planeNormal = -character.GetGravityDirection();
			if (Vector3.ProjectOnPlane(destination - character.position, planeNormal).sqrMagnitude >= MathLib.Square(stoppingDistance))
			{
				agent.SetDestination(destination);
			}
		}

		public virtual void PauseMovement(bool pause)
		{
			agent.isStopped = pause;
			character.SetMovementDirection(Vector3.zero);
		}

		public virtual void StopMovement()
		{
			agent.ResetPath();
			character.SetMovementDirection(Vector3.zero);
		}

		protected virtual float ComputeAnalogInputModifier(Vector3 desiredVelocity)
		{
			float maxSpeed = _character.GetMaxSpeed();
			if (desiredVelocity.sqrMagnitude > 0f && maxSpeed > 1E-08f)
			{
				return Mathf.Clamp01(desiredVelocity.magnitude / maxSpeed);
			}
			return 0f;
		}

		protected virtual Vector3 CalcMovementDirection(Vector3 desiredVelocity)
		{
			Vector3 planeNormal = -character.GetGravityDirection();
			Vector3 vector = Vector3.ProjectOnPlane(desiredVelocity, planeNormal) * brakingRatio;
			float minAnalogSpeed = _character.GetMinAnalogSpeed();
			if (vector.sqrMagnitude < MathLib.Square(minAnalogSpeed))
			{
				vector = vector.normalized * minAnalogSpeed;
			}
			return Vector3.ClampMagnitude(vector, ComputeAnalogInputModifier(vector));
		}

		protected virtual void DoPathFollowing()
		{
			if (IsPathFollowing())
			{
				if (agent.remainingDistance <= stoppingDistance)
				{
					StopMovement();
					OnDestinationReached();
				}
				else
				{
					Vector3 movementDirection = CalcMovementDirection(agent.desiredVelocity);
					character.SetMovementDirection(movementDirection);
				}
			}
		}

		protected virtual void SyncNavMeshAgent()
		{
			agent.angularSpeed = _character.rotationRate;
			agent.speed = _character.GetMaxSpeed();
			agent.acceleration = _character.GetMaxAcceleration();
			agent.velocity = _character.GetVelocity();
			agent.nextPosition = _character.GetPosition();
			agent.radius = _character.radius;
			agent.height = _character.height;
		}

		protected virtual void OnMovementModeChanged(Character.MovementMode prevMovementMode, int prevCustomMovementMode)
		{
			if (!character.IsWalking() || !character.IsFalling())
			{
				StopMovement();
			}
		}

		protected virtual void OnBeforeSimulationUpdated(float deltaTime)
		{
			DoPathFollowing();
		}

		private void Reset()
		{
			_autoBraking = true;
			_brakingDistance = 2f;
			_stoppingDistance = 1f;
		}

		private void OnValidate()
		{
			if (_agent == null)
			{
				_agent = GetComponent<NavMeshAgent>();
			}
			brakingDistance = _brakingDistance;
			stoppingDistance = _stoppingDistance;
		}

		protected virtual void Awake()
		{
			CacheComponents();
			agent.autoBraking = autoBraking;
			agent.stoppingDistance = stoppingDistance;
			agent.updatePosition = false;
			agent.updateRotation = false;
			agent.updateUpAxis = false;
		}

		protected virtual void OnEnable()
		{
			character.MovementModeChanged += OnMovementModeChanged;
			character.BeforeSimulationUpdated += OnBeforeSimulationUpdated;
		}

		protected virtual void OnDisable()
		{
			character.MovementModeChanged -= OnMovementModeChanged;
			character.BeforeSimulationUpdated -= OnBeforeSimulationUpdated;
		}

		protected virtual void LateUpdate()
		{
			SyncNavMeshAgent();
		}
	}
}
