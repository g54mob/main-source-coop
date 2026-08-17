using System;
using UnityEngine;

namespace Ami.BroAudio.Data
{
	[Serializable]
	public struct AudioParameterBinding
	{
		public string ParameterId;

		public AudioBindingTarget Target;

		public float InputMin;

		public float InputMax;

		public float OutputMin;

		public float OutputMax;

		public AnimationCurve Curve;

		public float BoolFalseValue;

		public float BoolTrueValue;

		public float SmoothingTime;

		public static AudioParameterBinding CreateDefault()
		{
			return new AudioParameterBinding
			{
				ParameterId = string.Empty,
				Target = AudioBindingTarget.None,
				InputMin = 0f,
				InputMax = 1f,
				OutputMin = 0f,
				OutputMax = 1f,
				Curve = AnimationCurve.Linear(0f, 0f, 1f, 1f),
				BoolFalseValue = 1f,
				BoolTrueValue = 1f,
				SmoothingTime = 0f
			};
		}
	}
}
