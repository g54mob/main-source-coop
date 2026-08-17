using System;
using UnityEngine;

namespace Ami.BroAudio.Data
{
	[Serializable]
	public struct AudioTransition
	{
		public AudioTransitionType Type;

		public string ParameterId;

		public string ParameterName;

		public ParameterCondition Condition;

		public bool CompareBool;

		public int CompareInt;

		public float CompareFloat;

		public AudioTransitionTiming Timing;

		public bool Evaluate(object currentValue, AudioParameterType parameterType)
		{
			if (currentValue == null)
			{
				return false;
			}
			switch (parameterType)
			{
			case AudioParameterType.Bool:
				if (!(currentValue is bool flag))
				{
					return false;
				}
				return Condition switch
				{
					ParameterCondition.Equals => flag == CompareBool, 
					ParameterCondition.NotEquals => flag != CompareBool, 
					_ => false, 
				};
			case AudioParameterType.Int:
				if (!(currentValue is int num2))
				{
					return false;
				}
				return Condition switch
				{
					ParameterCondition.Equals => num2 == CompareInt, 
					ParameterCondition.NotEquals => num2 != CompareInt, 
					ParameterCondition.GreaterThan => num2 > CompareInt, 
					ParameterCondition.LessThan => num2 < CompareInt, 
					ParameterCondition.GreaterThanOrEqual => num2 >= CompareInt, 
					ParameterCondition.LessThanOrEqual => num2 <= CompareInt, 
					_ => false, 
				};
			case AudioParameterType.Float:
				if (!(currentValue is float num))
				{
					return false;
				}
				return Condition switch
				{
					ParameterCondition.Equals => Mathf.Approximately(num, CompareFloat), 
					ParameterCondition.NotEquals => !Mathf.Approximately(num, CompareFloat), 
					ParameterCondition.GreaterThan => num > CompareFloat, 
					ParameterCondition.LessThan => num < CompareFloat, 
					ParameterCondition.GreaterThanOrEqual => num >= CompareFloat, 
					ParameterCondition.LessThanOrEqual => num <= CompareFloat, 
					_ => false, 
				};
			default:
				return false;
			}
		}
	}
}
