using System;
using UnityEngine;

namespace StinkySteak.NetcodeBenchmark
{
	[Serializable]
	public struct WanderMoveWrapper : IMoveWrapper
	{
		[SerializeField]
		private float _circleRadius;

		[SerializeField]
		private float _turnChance;

		[SerializeField]
		private float _maxRadius;

		[SerializeField]
		private float _mass;

		[SerializeField]
		private float _maxSpeed;

		[SerializeField]
		private float _maxForce;

		[SerializeField]
		private float _maxSpawnPositionRadius;

		private Vector3 _velocity;

		private Vector3 _wanderForce;

		private Vector3 _target;

		public static WanderMoveWrapper CreateDefault()
		{
			return new WanderMoveWrapper
			{
				_circleRadius = 1f,
				_turnChance = 0.05f,
				_maxRadius = 5f,
				_mass = 15f,
				_maxSpeed = 3f,
				_maxForce = 15f
			};
		}

		public void NetworkStart(Transform transform)
		{
			_velocity = UnityEngine.Random.onUnitSphere;
			_wanderForce = GetRandomWanderForce();
			transform.position = RandomVector3.Get(_maxSpawnPositionRadius);
		}

		public void NetworkUpdate(Transform transform)
		{
			Vector3 vector = GetWanderForce(transform).normalized * _maxSpeed;
			Vector3 vector2 = vector - _velocity;
			vector2 = Vector3.ClampMagnitude(vector2, _maxForce);
			vector2 /= _mass;
			_velocity = Vector3.ClampMagnitude(_velocity + vector2, _maxSpeed);
			transform.position += _velocity * Time.deltaTime;
			transform.forward = _velocity.normalized;
			Debug.DrawRay(transform.position, _velocity.normalized * 2f, Color.green);
			Debug.DrawRay(transform.position, vector.normalized * 2f, Color.magenta);
		}

		private Vector3 GetWanderForce(Transform transform)
		{
			if (transform.position.magnitude > _maxRadius)
			{
				Vector3 normalized = (_target - transform.position).normalized;
				_wanderForce = _velocity.normalized + normalized;
			}
			else if (UnityEngine.Random.value < _turnChance)
			{
				_wanderForce = GetRandomWanderForce();
			}
			return _wanderForce;
		}

		private Vector3 GetRandomWanderForce()
		{
			Vector3 normalized = _velocity.normalized;
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			Vector3 vector = new Vector3(insideUnitCircle.x, insideUnitCircle.y) * _circleRadius;
			vector = Quaternion.LookRotation(_velocity) * vector;
			return normalized + vector;
		}
	}
}
