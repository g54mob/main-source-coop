using UnityEngine;

namespace Features.LevelLightModule.Scripts
{
	[CreateAssetMenu(fileName = "FlashlightAdjustConfiguration_Default", menuName = "Configurations/LevelLightModule/FlashlightAdjustConfiguration")]
	public class FlashlightAdjustConfiguration : ScriptableObject
	{
		[SerializeField]
		[Min(0f)]
		private float _minOuterCutOff = 35f;

		[SerializeField]
		[Min(0f)]
		private float _maxOuterCutOff = 50f;

		[SerializeField]
		[Min(0f)]
		private float _minIntensity = 20f;

		[SerializeField]
		[Min(0f)]
		private float _maxIntensity = 50f;

		[SerializeField]
		[Min(0f)]
		private float _distanceForMinValues = 3f;

		[SerializeField]
		[Min(0.01f)]
		private float _distanceForMaxValues = 15f;

		[SerializeField]
		[Min(0f)]
		private float _adjustmentLerpSpeed = 8f;

		public float MinOuterCutOff => _minOuterCutOff;

		public float MaxOuterCutOff => _maxOuterCutOff;

		public float MinIntensity => _minIntensity;

		public float MaxIntensity => _maxIntensity;

		public float DistanceForMinValues => _distanceForMinValues;

		public float DistanceForMaxValues => _distanceForMaxValues;

		public float AdjustmentLerpSpeed => _adjustmentLerpSpeed;
	}
}
