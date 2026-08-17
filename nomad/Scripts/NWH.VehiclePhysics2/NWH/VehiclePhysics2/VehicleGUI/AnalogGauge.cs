using UnityEngine;

namespace NWH.VehiclePhysics2.VehicleGUI
{
	public class AnalogGauge : MonoBehaviour
	{
		[Tooltip("angle of the needle at the highest value. You can use lock at end option to adjust this value while in play mode.")]
		public float endAngle = 330f;

		[Tooltip("    Locks the needle position at the end angle (play mode only).")]
		public bool lockAtEnd;

		[Tooltip("    Locks the needle position at the start angle (play mode only).")]
		public bool lockAtStart;

		[Tooltip("    Value at the end of needle travel, at the end angle.")]
		public float maxValue;

		[Range(0f, 1f)]
		[Tooltip("    Smooths the travel of the needle making it more inert, as if actually had some mass and resistance.")]
		public float needleSmoothing;

		[Tooltip("angle of the needle at the lowest value. You can use lock at start option to adjust this value while in play mode.")]
		public float startAngle = 574f;

		private float _angle;

		private float _currentValue;

		private GameObject _needle;

		private float _percent;

		private float _prevAngle;

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
			_needle = base.transform.Find("Needle").gameObject;
		}

		private void Start()
		{
			_angle = startAngle;
		}

		private void Update()
		{
			_percent = Mathf.Clamp01(_currentValue / maxValue);
			_prevAngle = _angle;
			_angle = Mathf.Lerp(startAngle + (endAngle - startAngle) * _percent, _prevAngle, needleSmoothing);
			if (lockAtEnd)
			{
				_angle = endAngle;
			}
			if (lockAtStart)
			{
				_angle = startAngle;
			}
			Transform obj = _needle.transform;
			Vector3 localEulerAngles = obj.localEulerAngles;
			obj.localEulerAngles = new Vector3(localEulerAngles.x, localEulerAngles.y, _angle);
		}
	}
}
