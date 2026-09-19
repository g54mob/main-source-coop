using System;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core
{
	[Serializable]
	public struct BeachFloatRange
	{
		[Tooltip("Lowest value that can be selected.")]
		[SerializeField]
		private float _min;

		[Tooltip("Highest value that can be selected.")]
		[SerializeField]
		private float _max;

		public float Min => _min;

		public float Max => _max;

		public BeachFloatRange(float min, float max)
		{
			_min = min;
			_max = max;
		}

		public float Evaluate()
		{
			if (!Mathf.Approximately(_min, _max))
			{
				return UnityEngine.Random.Range(_min, _max);
			}
			return _min;
		}

		public void Validate(BeachValidationResult result, string fieldName)
		{
			if (_min > _max)
			{
				result.AddError(fieldName + ": minimum value must be less than or equal to maximum value.");
			}
		}
	}
}
