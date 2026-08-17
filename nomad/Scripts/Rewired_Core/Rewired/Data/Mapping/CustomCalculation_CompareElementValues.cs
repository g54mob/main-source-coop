using System;
using Rewired.Utils;
using Rewired.Utils.Classes;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired.Data.Mapping
{
	[Serializable]
	public sealed class CustomCalculation_CompareElementValues : CustomCalculation
	{
		public enum ComparisonType
		{
			Min = 0,
			Max = 1,
			MinAbs = 2,
			MaxAbs = 3
		}

		private const TypeWrapper.DataType resultType = TypeWrapper.DataType.Single;

		[SerializeField]
		private ComparisonType _comparisonType;

		TypeWrapper.DataType SerializedMethod.ResultType => TypeWrapper.DataType.Single;

		internal bool LBRgXBBBaztzvahhldIEGygMLAmFb()
		{
			bool flag = false;
			ClearResult();
			if (base.DataCount >= 1)
			{
				_result = lodFAFGfXDaXYoXtpQJXMvUJpJiXA();
				flag = true;
			}
			ClearData();
			_resultIsValid = flag;
			return flag;
		}

		private float lodFAFGfXDaXYoXtpQJXMvUJpJiXA()
		{
			int dataCount = base.DataCount;
			if (dataCount == 0)
			{
				return 0f;
			}
			float num = _data[0];
			for (int i = 1; i < dataCount; i++)
			{
				if (_data[i].type != TypeWrapper.DataType.Single)
				{
					throw new Exception("Data type must be the same on all data fields!");
				}
				float num2 = _data[i];
				switch (_comparisonType)
				{
				case ComparisonType.Min:
					num = Math.Min(num, num2);
					break;
				case ComparisonType.Max:
					num = Math.Max(num, num2);
					break;
				case ComparisonType.MinAbs:
					num = MathTools.MinMagnitude(num, num2);
					break;
				case ComparisonType.MaxAbs:
					num = MathTools.MaxMagnitude(num, num2);
					break;
				}
			}
			return num;
		}
	}
}
