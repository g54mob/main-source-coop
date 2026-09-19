using System;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Settings
{
	[CreateAssetMenu(fileName = "MimicFacialAnimationSettings", menuName = "RubberArms/Mimic/Mimic Facial Animation Settings")]
	public class MimicFacialAnimationSettings : ScriptableObject
	{
		[SerializeField]
		private MouthBlendShapeWeight[] _closedMouthBlendShapes = new MouthBlendShapeWeight[3]
		{
			new MouthBlendShapeWeight
			{
				Index = 0,
				ClosedWeight = 0f
			},
			new MouthBlendShapeWeight
			{
				Index = 1,
				ClosedWeight = 100f
			},
			new MouthBlendShapeWeight
			{
				Index = 2,
				ClosedWeight = 75f
			}
		};

		[field: SerializeField]
		public bool EnableEyeAnimation { get; private set; } = true;

		[field: SerializeField]
		public bool EnableLipSync { get; private set; } = true;

		[field: SerializeField]
		public float EyesHorizontalLimit { get; private set; } = 70f;

		[field: SerializeField]
		public float EyesVerticalLimitTop { get; private set; } = 55f;

		[field: SerializeField]
		public float EyesVerticalLimitBottom { get; private set; } = -50f;

		[field: SerializeField]
		public float MaxTargetDistance { get; private set; } = 50f;

		[field: SerializeField]
		public float EyesLerpSpeed { get; private set; } = 10f;

		public ReadOnlySpan<MouthBlendShapeWeight> ClosedMouthBlendShapes => _closedMouthBlendShapes;
	}
}
