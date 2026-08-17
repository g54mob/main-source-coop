using System.Collections.Generic;
using Rewired.Data.Mapping;
using UnityEngine;

namespace Rewired
{
	public struct AxisCalibrationData
	{
		public bool enabled;

		public float deadZone;

		public float zero;

		public float min;

		public float max;

		public bool invert;

		public AxisSensitivityType sensitivityType;

		public float sensitivity;

		public AnimationCurve sensitivityCurve;

		public bool applyRangeCalibration;

		[CustomObfuscation(rename = false)]
		internal Dictionary<int, AxisCalibrationInfo> calibrations;

		public static AxisCalibrationData Default
		{
			get
			{
				AxisSensitivityType axisSensitivityType = (ReInput.isReady ? ReInput.configVars.defaultAxisSensitivityType : AxisSensitivityType.Multiplier);
				return new AxisCalibrationData(true, 0f, 0f, -1f, 1f, false, true, axisSensitivityType, 1f, (axisSensitivityType == AxisSensitivityType.Curve) ? AnimationCurve.Linear(-1f, 1f, 1f, 1f) : null);
			}
		}

		[CustomObfuscation(rename = false)]
		internal static AxisCalibrationData Raw
		{
			get
			{
				AxisSensitivityType axisSensitivityType = (ReInput.isReady ? ReInput.configVars.defaultAxisSensitivityType : AxisSensitivityType.Multiplier);
				return new AxisCalibrationData(true, 0f, 0f, -1f, 1f, false, false, axisSensitivityType, 1f, (axisSensitivityType == AxisSensitivityType.Curve) ? AnimationCurve.Linear(-1f, 1f, 1f, 1f) : null);
			}
		}

		public AxisCalibrationData(bool P_0, float P_1, float P_2, float P_3, float P_4, bool P_5, bool P_6)
		{
			enabled = P_0;
			deadZone = P_1;
			zero = P_2;
			min = P_3;
			max = P_4;
			invert = P_5;
			applyRangeCalibration = P_6;
			sensitivity = 1f;
			sensitivityType = (ReInput.isReady ? ReInput.configVars.defaultAxisSensitivityType : AxisSensitivityType.Multiplier);
			sensitivityCurve = ((sensitivityType == AxisSensitivityType.Curve) ? AnimationCurve.Linear(-1f, 1f, 1f, 1f) : null);
			calibrations = null;
		}

		public AxisCalibrationData(bool P_0, float P_1, float P_2, float P_3, float P_4, bool P_5, bool P_6, float P_7)
		{
			enabled = P_0;
			deadZone = P_1;
			zero = P_2;
			min = P_3;
			max = P_4;
			invert = P_5;
			applyRangeCalibration = P_6;
			sensitivity = P_7;
			sensitivityType = (ReInput.isReady ? ReInput.configVars.defaultAxisSensitivityType : AxisSensitivityType.Multiplier);
			sensitivityCurve = ((sensitivityType == AxisSensitivityType.Curve) ? AnimationCurve.Linear(-1f, 1f, 1f, 1f) : null);
			calibrations = null;
		}

		public AxisCalibrationData(bool P_0, float P_1, float P_2, float P_3, float P_4, bool P_5, bool P_6, AxisSensitivityType P_7, float P_8, AnimationCurve P_9)
		{
			enabled = P_0;
			deadZone = P_1;
			zero = P_2;
			min = P_3;
			max = P_4;
			invert = P_5;
			applyRangeCalibration = P_6;
			sensitivityType = P_7;
			sensitivity = P_8;
			sensitivityCurve = P_9;
			calibrations = null;
		}
	}
}
