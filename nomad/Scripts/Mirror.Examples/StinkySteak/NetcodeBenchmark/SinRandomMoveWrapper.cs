using System;
using UnityEngine;

namespace StinkySteak.NetcodeBenchmark
{
	[Serializable]
	public struct SinRandomMoveWrapper : IMoveWrapper
	{
		[SerializeField]
		private float _minSpeed;

		[SerializeField]
		private float _maxSpeed;

		[SerializeField]
		private float _amplitude;

		private Vector3 _targetPosition;

		private Vector3 _initialPosition;

		private float _speed;

		public static SinRandomMoveWrapper CreateDefault()
		{
			return new SinRandomMoveWrapper
			{
				_minSpeed = 1f,
				_maxSpeed = 1f,
				_amplitude = 1f
			};
		}

		public void NetworkStart(Transform transform)
		{
			_speed = UnityEngine.Random.Range(_minSpeed, _maxSpeed);
			_targetPosition = RandomVector3.Get(1f);
		}

		public void NetworkUpdate(Transform transform)
		{
			float num = Mathf.Sin(Time.time * _speed) * _amplitude;
			transform.position = _initialPosition + _targetPosition * num;
		}
	}
}
