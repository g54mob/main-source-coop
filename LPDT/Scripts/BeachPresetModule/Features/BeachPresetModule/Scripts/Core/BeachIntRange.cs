using System;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core
{
	[Serializable]
	public struct BeachIntRange
	{
		[Tooltip("Lowest value that can be selected.")]
		[SerializeField]
		private int _min;

		[Tooltip("Highest value that can be selected.")]
		[SerializeField]
		private int _max;

		public int Min => _min;

		public int Max => _max;

		public BeachIntRange(int min, int max)
		{
			_min = min;
			_max = max;
		}

		public int Evaluate()
		{
			if (_min != _max)
			{
				return UnityEngine.Random.Range(_min, _max + 1);
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
