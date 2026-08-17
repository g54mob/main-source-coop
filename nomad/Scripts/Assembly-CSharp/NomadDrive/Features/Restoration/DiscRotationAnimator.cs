using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public class DiscRotationAnimator : MonoBehaviour
	{
		[Header("Disc Animation")]
		[Tooltip("Transform of the rotating disc/pad. Will spin around its local axis.")]
		[SerializeField]
		private Transform discTransform;

		[Tooltip("Rotation axis in local space")]
		[SerializeField]
		private Vector3 rotationAxis = Vector3.forward;

		[Tooltip("Maximum rotation speed in degrees per second")]
		[SerializeField]
		[Range(100f, 3000f)]
		private float maxRotationSpeed = 1500f;

		[Tooltip("Time to reach max speed when tool is enabled")]
		[SerializeField]
		[Range(0.1f, 3f)]
		private float accelerationDuration = 0.8f;

		[Tooltip("Time to stop when tool is disabled")]
		[SerializeField]
		[Range(0.1f, 3f)]
		private float decelerationDuration = 1.5f;

		private float _currentSpeed;

		private Tween _speedTween;

		public float NormalizedSpeed
		{
			get
			{
				if (!(maxRotationSpeed > 0f))
				{
					return 0f;
				}
				return _currentSpeed / maxRotationSpeed;
			}
		}

		public void StartAcceleration()
		{
			if (!(discTransform == null))
			{
				_speedTween.Stop();
				_speedTween = Tween.Custom(_currentSpeed, maxRotationSpeed, accelerationDuration, delegate(float speed)
				{
					_currentSpeed = speed;
				}, Ease.OutQuad);
			}
		}

		public void StartDeceleration()
		{
			if (!(discTransform == null))
			{
				_speedTween.Stop();
				_speedTween = Tween.Custom(_currentSpeed, 0f, decelerationDuration, delegate(float speed)
				{
					_currentSpeed = speed;
				}, Ease.OutQuad);
			}
		}

		private void Update()
		{
			if (!(discTransform == null) && !(_currentSpeed <= 0f))
			{
				float angle = _currentSpeed * Time.deltaTime;
				discTransform.Rotate(rotationAxis, angle, Space.Self);
			}
		}

		private void OnDestroy()
		{
			_speedTween.Stop();
		}
	}
}
