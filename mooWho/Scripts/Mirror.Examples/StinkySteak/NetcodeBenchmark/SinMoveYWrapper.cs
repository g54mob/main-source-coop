using System;
using UnityEngine;

namespace StinkySteak.NetcodeBenchmark
{
	[Serializable]
	public struct SinMoveYWrapper : IMoveWrapper
	{
		[SerializeField]
		private float _minSpeed;

		[SerializeField]
		private float _maxSpeed;

		[SerializeField]
		private float _minAmplitude;

		[SerializeField]
		private float _maxAmplitude;

		[SerializeField]
		private float _positionMaxRandom;

		private Vector3 _initialPosition;

		private float _speed;

		private float _amplitude;

		public static SinMoveYWrapper CreateDefault()
		{
			return new SinMoveYWrapper
			{
				_minSpeed = 0.5f,
				_maxSpeed = 1f,
				_minAmplitude = 0.5f,
				_maxAmplitude = 1f,
				_positionMaxRandom = 5f
			};
		}

		public void NetworkStart(Transform transform)
		{
			_speed = UnityEngine.Random.Range(_minSpeed, _maxSpeed);
			_amplitude = UnityEngine.Random.Range(_minAmplitude, _maxAmplitude);
			_initialPosition = RandomVector3.Get(_positionMaxRandom);
		}

		public void NetworkUpdate(Transform transform)
		{
			float num = Mathf.Sin(Time.time * _speed) * _amplitude;
			transform.position = _initialPosition + Vector3.up * num;
		}
	}
}
