using System;
using UnityEngine;

namespace EvilCore.Particles
{
	public readonly struct ParticleHandle : IEquatable<ParticleHandle>
	{
		public static readonly ParticleHandle Invalid = new ParticleHandle(0, null);

		public readonly int Id;

		public readonly PooledParticle Pooled;

		public bool IsValid
		{
			get
			{
				if (Id != 0 && Pooled != null)
				{
					return Pooled.ActiveId == Id;
				}
				return false;
			}
		}

		public ParticleSystem ParticleSystem
		{
			get
			{
				if (!IsValid)
				{
					return null;
				}
				return Pooled.ParticleSystem;
			}
		}

		public Transform Transform
		{
			get
			{
				if (!IsValid)
				{
					return null;
				}
				return Pooled.transform;
			}
		}

		public ParticleHandle(int id, PooledParticle pooled)
		{
			Id = id;
			Pooled = pooled;
		}

		public bool Equals(ParticleHandle other)
		{
			return Id == other.Id;
		}

		public override bool Equals(object obj)
		{
			if (obj is ParticleHandle other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Id;
		}

		public static bool operator ==(ParticleHandle left, ParticleHandle right)
		{
			return left.Id == right.Id;
		}

		public static bool operator !=(ParticleHandle left, ParticleHandle right)
		{
			return left.Id != right.Id;
		}
	}
}
