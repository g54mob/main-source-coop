using System;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired.Data.Mapping
{
	[Serializable]
	public class HardwareAxisInfo : IDeepCloneable
	{
		[SerializeField]
		internal AxisCoordinateMode _dataFormat;

		[SerializeField]
		internal bool _excludeFromPolling;

		[SerializeField]
		internal SpecialAxisType _specialAxisType;

		[SerializeField]
		internal float _pollingDeadZone = -1f;

		public AxisCoordinateMode dataFormat => _dataFormat;

		public bool excludeFromPolling => _excludeFromPolling;

		public SpecialAxisType specialAxisType => _specialAxisType;

		public float pollingDeadZone => _pollingDeadZone;

		[CustomObfuscation(rename = false)]
		internal static HardwareAxisInfo Default => new HardwareAxisInfo(AxisCoordinateMode.Absolute, false, -1f, SpecialAxisType.None);

		public HardwareAxisInfo()
		{
			_dataFormat = AxisCoordinateMode.Absolute;
			_excludeFromPolling = false;
			_specialAxisType = SpecialAxisType.None;
		}

		[CustomObfuscation(rename = false)]
		internal HardwareAxisInfo(AxisCoordinateMode P_0, bool P_1, float P_2, SpecialAxisType P_3)
		{
			_dataFormat = P_0;
			_excludeFromPolling = P_1;
			_pollingDeadZone = P_2;
			_specialAxisType = P_3;
		}

		public object DeepClone()
		{
			return new HardwareAxisInfo(_dataFormat, _excludeFromPolling, _pollingDeadZone, _specialAxisType);
		}
	}
}
