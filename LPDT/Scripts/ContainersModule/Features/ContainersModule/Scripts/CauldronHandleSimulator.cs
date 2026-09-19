using UnityEngine;

namespace Features.ContainersModule.Scripts
{
	public class CauldronHandleSimulator : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody _rootRigidbody;

		[SerializeField]
		private SimulatedHandle[] _handles = new SimulatedHandle[0];

		[SerializeField]
		private bool _useRigidbodyVelocity = true;

		private Vector3 _previousPosition;

		private Vector3 _previousVelocity;

		private bool _initialized;

		private void Awake()
		{
			Initialize();
		}

		private void Initialize()
		{
			if (_rootRigidbody == null)
			{
				_rootRigidbody = GetComponentInParent<Rigidbody>();
			}
			_previousPosition = base.transform.position;
			_previousVelocity = GetCurrentVelocity(Time.deltaTime);
			for (int i = 0; i < _handles.Length; i++)
			{
				_handles[i]?.Initialize();
			}
			_initialized = true;
		}

		private void LateUpdate()
		{
			float deltaTime = Time.deltaTime;
			if (_initialized && !(deltaTime <= 0f))
			{
				Vector3 currentVelocity = GetCurrentVelocity(deltaTime);
				Vector3 inertialAcceleration = (currentVelocity - _previousVelocity) / deltaTime;
				for (int i = 0; i < _handles.Length; i++)
				{
					_handles[i]?.Simulate(base.transform, inertialAcceleration, deltaTime);
				}
				_previousVelocity = currentVelocity;
				_previousPosition = base.transform.position;
			}
		}

		private Vector3 GetCurrentVelocity(float deltaTime)
		{
			if (_useRigidbodyVelocity && _rootRigidbody != null && !_rootRigidbody.isKinematic)
			{
				return _rootRigidbody.linearVelocity;
			}
			if (!(deltaTime > 0f))
			{
				return Vector3.zero;
			}
			return (base.transform.position - _previousPosition) / deltaTime;
		}
	}
}
