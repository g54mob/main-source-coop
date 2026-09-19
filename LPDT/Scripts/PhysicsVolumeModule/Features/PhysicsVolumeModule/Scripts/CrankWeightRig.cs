using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public class CrankWeightRig : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private CrankRotator _crankRotator;

		[Tooltip("The hanging weight root — its local Y is driven from the crank's rendered angle.")]
		[SerializeField]
		private Transform _weight;

		[Header("Winch")]
		[Tooltip("Metres the weight rises per crank revolution. Negative flips which winding direction raises it.")]
		[SerializeField]
		private float _metersPerRevolution = 1f;

		[Tooltip("Crank angle (deg) at which the weight rests at the bottom.")]
		[SerializeField]
		private float _bottomAngleDeg;

		[Header("Back-drive (falling weight spins the free crank)")]
		[Tooltip("How fast the hanging load accelerates the free crank backward (deg/s^2).")]
		[SerializeField]
		private float _backDriveAccelerationDegPerSec2 = 240f;

		[Tooltip("Cap on the back-drive spin (deg/s).")]
		[SerializeField]
		private float _backDriveMaxDegPerSec = 540f;

		private Vector3 _weightRestLocalPosition;

		private float _backDriveDegPerSec;

		private void Awake()
		{
			if (_weight != null)
			{
				_weightRestLocalPosition = _weight.localPosition;
			}
		}

		private void FixedUpdate()
		{
			if (!(_crankRotator == null) && !(_crankRotator.Object == null) && _crankRotator.Object.IsValid)
			{
				bool flag = !_crankRotator.IsGrabbed && !_crankRotator.IsLocked && _crankRotator.InertiaDegreesPerSecond == 0f && WoundHeight(_crankRotator.Angle) > 0.001f;
				_backDriveDegPerSec = (flag ? Mathf.Min(_backDriveMaxDegPerSec, _backDriveDegPerSec + _backDriveAccelerationDegPerSec2 * Time.fixedDeltaTime) : 0f);
				_crankRotator.ExternalDriveDegreesPerSecond = (flag ? ((0f - Mathf.Sign(_metersPerRevolution)) * _backDriveDegPerSec) : 0f);
			}
		}

		private void LateUpdate()
		{
			if (!(_crankRotator == null) && !(_weight == null))
			{
				Vector3 weightRestLocalPosition = _weightRestLocalPosition;
				weightRestLocalPosition.y += Mathf.Max(0f, WoundHeight(_crankRotator.RenderAngle));
				_weight.localPosition = weightRestLocalPosition;
			}
		}

		private float WoundHeight(float angleDeg)
		{
			return (angleDeg - _bottomAngleDeg) / 360f * _metersPerRevolution;
		}
	}
}
