using System;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Settings
{
	[Serializable]
	public struct ParrotMobLookYawRange
	{
		[SerializeField]
		private float _min;

		[SerializeField]
		private float _max;

		public float Min => _min;

		public float Max => _max;

		public ParrotMobLookYawRange(float min, float max)
		{
			_min = min;
			_max = max;
		}
	}
}
