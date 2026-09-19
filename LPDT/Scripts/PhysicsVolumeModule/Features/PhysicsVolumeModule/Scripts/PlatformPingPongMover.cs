using Fusion;
using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public class PlatformPingPongMover : MonoBehaviour
	{
		[SerializeField]
		private Vector3 _travelOffset = new Vector3(8f, 0f, 0f);

		[SerializeField]
		private float _speed = 2f;

		private Vector3 _startPosition;

		private NetworkRunner _runner;

		private void Awake()
		{
			_startPosition = base.transform.position;
		}

		private void Update()
		{
			float magnitude = _travelOffset.magnitude;
			if (!(magnitude <= Mathf.Epsilon) && !(_speed <= 0f))
			{
				float num = Mathf.PingPong((float)(SharedTime() * (double)_speed), magnitude);
				base.transform.position = _startPosition + _travelOffset * (num / magnitude);
			}
		}

		private double SharedTime()
		{
			if (_runner == null || !_runner.IsRunning)
			{
				_runner = null;
				foreach (NetworkRunner instance in NetworkRunner.Instances)
				{
					if (!(instance == null) && instance.IsRunning)
					{
						_runner = instance;
						break;
					}
				}
			}
			if (!(_runner != null))
			{
				return Time.timeAsDouble;
			}
			return _runner.LocalRenderTime;
		}
	}
}
