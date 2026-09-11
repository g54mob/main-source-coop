using System;
using UnityEngine;
using pworld.Scripts.Extensions;

namespace pworld.Scripts.PPhysStandalone
{
	[Serializable]
	public abstract class PSAPhysSpringBase
	{
		[SerializeField]
		protected Vector3 _target;

		[SerializeField]
		protected Vector3 velocity;

		public float damp = 15f;

		public float spring = 15f;

		public float ropeLength;

		public float maxStepSize = 0.016f;

		public float sleepTime = 3f;

		public bool SetTargetOnAwake = true;

		private float dt;

		private bool lastIdle;

		private Vector3 lastTarget;

		private Vector3 lastVelocity;

		public Action OnExitIdle;

		public Action OnIdle;

		private bool sleeping;

		private float timeSinceVelZero;

		public Vector3 Velocity
		{
			get
			{
				return velocity;
			}
			set
			{
				timeSinceVelZero = 0f;
				Sleeping = false;
				velocity = value;
			}
		}

		public virtual Vector3 Target
		{
			get
			{
				return _target;
			}
			set
			{
				timeSinceVelZero = 0f;
				Sleeping = false;
				_target = value;
			}
		}

		public abstract Vector3 Current { get; set; }

		public Vector3 Acceleration { get; private set; }

		public bool Idle => Acceleration.magnitude < 1f;

		public bool Sleeping
		{
			get
			{
				return sleeping;
			}
			set
			{
				if (sleeping != value)
				{
					if (!value)
					{
						timeSinceVelZero = 0f;
					}
					sleeping = value;
				}
			}
		}

		public virtual void Awake()
		{
			if (SetTargetOnAwake)
			{
				Target = Current;
			}
		}

		public void Reset()
		{
			Velocity = 0.ToVec();
			GoToTarget();
		}

		public void Update()
		{
			if (Sleeping)
			{
				if (lastTarget != Target)
				{
					Sleeping = false;
				}
				lastTarget = Target;
				return;
			}
			lastTarget = Target;
			if (Velocity.magnitude < 0.005f && !Sleeping)
			{
				timeSinceVelZero += Time.deltaTime;
				if (timeSinceVelZero > sleepTime)
				{
					Sleeping = true;
					return;
				}
			}
			else
			{
				timeSinceVelZero = 0f;
			}
			dt = Time.deltaTime;
			if (Velocity.IsNaN())
			{
				Reset();
				return;
			}
			for (float num = dt / Mathf.Max(maxStepSize, 0.005f); num > 0f; num -= 1f)
			{
				if (num > 1f)
				{
					PhysicsStep(maxStepSize);
				}
				else
				{
					PhysicsStep(dt);
				}
			}
		}

		public void FixedUpdate()
		{
		}

		private void LateUpdate()
		{
		}

		public virtual void GoTo()
		{
			Current = Target;
		}

		protected virtual void PhysicsStep(float dt)
		{
			velocity = FRILerp.PLerp(Velocity, (Vector3.ClampMagnitude(Target - Current, ropeLength) + Target - Current) * spring, damp, dt);
			Current += Velocity * dt;
			CalculateDeltaVel();
		}

		public void Push(Vector3 force)
		{
			Velocity += force;
		}

		public virtual void GoToTarget()
		{
			Current = Target;
		}

		public void CalculateDeltaVel()
		{
			Acceleration = FRILerp.PLerp(Acceleration, (Velocity - lastVelocity) / dt, 15f, dt);
			lastVelocity = Velocity;
			if (lastIdle != Idle)
			{
				if (Idle)
				{
					OnIdle?.Invoke();
				}
				if (!Idle)
				{
					OnExitIdle?.Invoke();
				}
			}
			lastIdle = Idle;
		}
	}
}
