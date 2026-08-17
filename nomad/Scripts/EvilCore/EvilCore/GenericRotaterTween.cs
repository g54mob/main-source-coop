using PrimeTween;
using UnityEngine;

namespace EvilCore
{
	public class GenericRotaterTween : MonoBehaviour
	{
		[Header("Target")]
		[Tooltip("Object to rotate. Leave empty to use this GameObject.")]
		[SerializeField]
		private Transform target;

		[Header("Rotation")]
		[Tooltip("Rotation axis. Interpreted in local or world space depending on useLocalSpace.")]
		[SerializeField]
		private Vector3 rotationAxis = Vector3.up;

		[Tooltip("Rotation speed in degrees per second. Negative values reverse the rotation.")]
		[SerializeField]
		[Range(-3600f, 3600f)]
		private float speedDegreesPerSecond = 180f;

		[Tooltip("If true, rotates around the target's local axis. If false, around world axis.")]
		[SerializeField]
		private bool useLocalSpace = true;

		[Header("Behavior")]
		[Tooltip("Start rotating automatically when this component is enabled.")]
		[SerializeField]
		private bool playOnEnable = true;

		[Tooltip("Time to ramp up to full speed when starting. 0 = instant.")]
		[SerializeField]
		[Min(0f)]
		private float accelerationDuration;

		[Tooltip("Time to ramp down to a stop when stopping. 0 = instant.")]
		[SerializeField]
		[Min(0f)]
		private float decelerationDuration;

		[Tooltip("Easing curve used for acceleration and deceleration.")]
		[SerializeField]
		private Ease rampEase = Ease.OutQuad;

		private Tween _speedTween;

		private float _currentSpeed;

		private bool _isRotating;

		public bool IsRotating => _isRotating;

		public float CurrentSpeed => _currentSpeed;

		public float TargetSpeed => speedDegreesPerSecond;

		private void Awake()
		{
			if (target == null)
			{
				target = base.transform;
			}
		}

		private void OnEnable()
		{
			if (playOnEnable)
			{
				StartRotation();
			}
		}

		private void OnDisable()
		{
			StopImmediate();
		}

		private void Update()
		{
			if (!(target == null) && !Mathf.Approximately(_currentSpeed, 0f))
			{
				float angle = _currentSpeed * Time.deltaTime;
				target.Rotate(rotationAxis, angle, useLocalSpace ? Space.Self : Space.World);
			}
		}

		public void StartRotation()
		{
			_isRotating = true;
			RampSpeedTo(speedDegreesPerSecond, accelerationDuration);
		}

		public void StopRotation()
		{
			_isRotating = false;
			RampSpeedTo(0f, decelerationDuration);
		}

		public void ToggleRotation()
		{
			if (_isRotating)
			{
				StopRotation();
			}
			else
			{
				StartRotation();
			}
		}

		public void StopImmediate()
		{
			_speedTween.Stop();
			_currentSpeed = 0f;
			_isRotating = false;
		}

		public void SetSpeed(float newSpeedDegreesPerSecond, float rampDuration = 0f)
		{
			speedDegreesPerSecond = newSpeedDegreesPerSecond;
			if (_isRotating)
			{
				RampSpeedTo(newSpeedDegreesPerSecond, rampDuration);
			}
		}

		public void SetRotationAxis(Vector3 newAxis)
		{
			rotationAxis = newAxis;
		}

		public void SetTarget(Transform newTarget)
		{
			target = ((newTarget != null) ? newTarget : base.transform);
		}

		private void RampSpeedTo(float targetSpeed, float duration)
		{
			_speedTween.Stop();
			if (duration <= 0f)
			{
				_currentSpeed = targetSpeed;
				return;
			}
			_speedTween = Tween.Custom(_currentSpeed, targetSpeed, duration, delegate(float v)
			{
				_currentSpeed = v;
			}, rampEase);
		}

		private void OnDestroy()
		{
			_speedTween.Stop();
		}
	}
}
