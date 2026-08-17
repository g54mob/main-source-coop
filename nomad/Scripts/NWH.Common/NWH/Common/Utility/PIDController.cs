using UnityEngine;

namespace NWH.Common.Utility
{
	public class PIDController
	{
		public float maxValue;

		public float minValue;

		private float _processVariable;

		public float GainDerivative { get; set; }

		public float GainIntegral { get; set; }

		public float GainProportional { get; set; }

		public float IntegralTerm { get; private set; }

		public float ProcessVariable
		{
			get
			{
				return _processVariable;
			}
			set
			{
				ProcessVariableLast = _processVariable;
				_processVariable = value;
			}
		}

		public float ProcessVariableLast { get; private set; }

		public float SetPoint { get; set; }

		public PIDController(float gainProportional, float gainIntegral, float gainDerivative, float outputMin, float outputMax)
		{
			GainDerivative = gainDerivative;
			GainIntegral = gainIntegral;
			GainProportional = gainProportional;
			maxValue = outputMax;
			minValue = outputMin;
		}

		public float ControlVariable(float timeSinceLastUpdate)
		{
			float num = SetPoint - ProcessVariable;
			IntegralTerm += GainIntegral * num * timeSinceLastUpdate;
			IntegralTerm = Mathf.Clamp(IntegralTerm, minValue, maxValue);
			float num2 = _processVariable - ProcessVariableLast;
			float num3 = GainDerivative * (num2 / timeSinceLastUpdate);
			return Mathf.Clamp(GainProportional * num + IntegralTerm - num3, minValue, maxValue);
		}
	}
}
