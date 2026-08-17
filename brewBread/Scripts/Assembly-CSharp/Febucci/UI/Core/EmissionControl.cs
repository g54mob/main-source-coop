using System;
using Febucci.Attributes;
using UnityEngine;

namespace Febucci.UI.Core
{
	[Serializable]
	internal struct EmissionControl
	{
		[SerializeField]
		[MinValue(0f)]
		private int cycles;

		[SerializeField]
		private AnimationCurve attackCurve;

		[SerializeField]
		[MinValue(0f)]
		private float overrideDuration;

		[SerializeField]
		private bool continueForever;

		[SerializeField]
		private AnimationCurve decayCurve;

		[NonSerialized]
		private float maxDuration;

		[NonSerialized]
		private AnimationCurve intensityOverDuration;

		[NonSerialized]
		private float passedTime;

		[NonSerialized]
		private float cycleDuration;

		[NonSerialized]
		public float effectWeigth;

		public void Initialize(float effectsMaxDuration)
		{
			passedTime = 0f;
			Keyframe[] array = new Keyframe[attackCurve.length + ((!continueForever) ? decayCurve.length : 0)];
			for (int i = 0; i < attackCurve.length; i++)
			{
				array[i] = attackCurve[i];
			}
			if (!continueForever)
			{
				if (overrideDuration > 0f)
				{
					effectsMaxDuration = overrideDuration;
				}
				float num = attackCurve.CalculateCurveDuration();
				for (int j = attackCurve.length; j < array.Length; j++)
				{
					array[j] = decayCurve[j - attackCurve.length];
					array[j].time += effectsMaxDuration + num;
				}
			}
			intensityOverDuration = new AnimationCurve(array);
			intensityOverDuration.preWrapMode = WrapMode.Loop;
			intensityOverDuration.postWrapMode = WrapMode.Loop;
			cycleDuration = intensityOverDuration.CalculateCurveDuration();
			effectWeigth = intensityOverDuration.Evaluate(passedTime);
			maxDuration = cycleDuration * (float)cycles;
		}

		public float IncreaseEffectTime(float deltaTime)
		{
			if (deltaTime == 0f)
			{
				return passedTime;
			}
			passedTime += deltaTime;
			if (passedTime < 0f)
			{
				passedTime = 0f;
			}
			if (passedTime > cycleDuration && continueForever)
			{
				effectWeigth = 1f;
				return passedTime;
			}
			if (cycles > 0 && passedTime >= maxDuration)
			{
				effectWeigth = 0f;
				return 0f;
			}
			effectWeigth = intensityOverDuration.Evaluate(passedTime);
			return passedTime;
		}
	}
}
