using System;
using UnityEngine;

namespace Zorro.Core
{
	[Serializable]
	public struct OneDPhysicsSpring : IPhysicsSpring, IOneDimension
	{
		[SerializeField]
		private float target;

		[SerializeField]
		private float spring;

		[SerializeField]
		private float drag;

		private float velocity;

		private float current;

		public float Spring
		{
			get
			{
				return spring;
			}
			set
			{
				spring = value;
			}
		}

		public float Drag
		{
			get
			{
				return drag;
			}
			set
			{
				drag = value;
			}
		}

		public float Target
		{
			get
			{
				return target;
			}
			set
			{
				target = value;
			}
		}

		public float Velocity => velocity;

		public float Current => current;

		public void FixedUpdate()
		{
			velocity *= 1f - drag;
		}

		public void Update()
		{
			float cappedDeltaTime = Timez.CappedDeltaTime;
			float num = target - current;
			velocity += num * cappedDeltaTime * spring;
			current += velocity * cappedDeltaTime;
		}

		public void SetCurrent(float current)
		{
			this.current = current;
		}

		public void SetVelocity(float velocity)
		{
			this.velocity = velocity;
		}

		public void AddForce(float force)
		{
			velocity += force;
		}
	}
}
