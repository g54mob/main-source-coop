using NomadDrive.Features.Vehicle.Enums;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.UI
{
	public class VehicleAnalogGauge : MonoBehaviour
	{
		[SerializeField]
		private DashboardDisplayMode displayMode;

		[SerializeField]
		private float maxValue;

		[SerializeField]
		private float startAngle = 574f;

		[SerializeField]
		private float endAngle = 330f;

		[SerializeField]
		[Range(0f, 1f)]
		private float needleSmoothing;

		[SerializeField]
		private ControlRotationAxis rotationAxis = ControlRotationAxis.Y;

		[SerializeField]
		private Transform needle;

		[SerializeField]
		private bool lockAtStart;

		[SerializeField]
		private bool lockAtEnd;

		private float _currentValue;

		private float _angle;

		private float _prevAngle;

		private Vector3 _axisVector;

		private Vector3 _defaultEuler;

		private bool _initialized;

		public float MaxValue
		{
			get
			{
				return maxValue;
			}
			set
			{
				maxValue = value;
			}
		}

		public float Value
		{
			get
			{
				return _currentValue;
			}
			set
			{
				_currentValue = Mathf.Clamp(value, 0f, maxValue);
			}
		}

		private void Awake()
		{
			Initialize();
		}

		private void Start()
		{
			_angle = startAngle;
		}

		private void Initialize()
		{
			_axisVector = GetAxisVector(rotationAxis);
			if (needle != null)
			{
				_defaultEuler = needle.localEulerAngles;
			}
			_initialized = true;
		}

		private void Update()
		{
			if (!(maxValue <= 0f))
			{
				float num = Mathf.Clamp01(_currentValue / maxValue);
				_prevAngle = _angle;
				_angle = Mathf.Lerp(startAngle + (endAngle - startAngle) * num, _prevAngle, needleSmoothing);
				_angle = Mathf.Clamp(_angle, Mathf.Min(startAngle, endAngle), Mathf.Max(startAngle, endAngle));
				if (lockAtEnd)
				{
					_angle = endAngle;
				}
				if (lockAtStart)
				{
					_angle = startAngle;
				}
				ApplyNeedleRotation();
			}
		}

		private void ApplyNeedleRotation()
		{
			if (!(needle == null))
			{
				Vector3 defaultEuler = _defaultEuler;
				switch (rotationAxis)
				{
				case ControlRotationAxis.X:
					defaultEuler.x = _angle;
					break;
				case ControlRotationAxis.Y:
					defaultEuler.y = _angle;
					break;
				case ControlRotationAxis.Z:
					defaultEuler.z = _angle;
					break;
				}
				needle.localEulerAngles = defaultEuler;
			}
		}

		private static Vector3 GetAxisVector(ControlRotationAxis axis)
		{
			return axis switch
			{
				ControlRotationAxis.X => Vector3.right, 
				ControlRotationAxis.Y => Vector3.up, 
				ControlRotationAxis.Z => Vector3.forward, 
				_ => Vector3.forward, 
			};
		}
	}
}
