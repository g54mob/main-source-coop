using Global.SerializableDictionary;
using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	[CreateAssetMenu(fileName = "GuideLineConfiguration_Default", menuName = "Configurations/GuideModule/GuideLineConfiguration")]
	public class GuideLineConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<GuideLineBuildType, PlaneGuideLineEntity> Prefabs { get; private set; }

		[field: SerializeField]
		public int InitialPoolSize { get; private set; } = 4;

		[field: SerializeField]
		public int MaxCapacity { get; private set; } = 16;

		[field: SerializeField]
		public float DissolveAnimationDuration { get; private set; } = 0.4f;

		[field: SerializeField]
		public float GuideLineUpdateFrequency { get; private set; } = 0.1f;

		[field: SerializeField]
		public GuideLineUpdateFrequencyType UpdateFrequencyTypeType { get; private set; } = GuideLineUpdateFrequencyType.EveryFrame;

		[field: SerializeField]
		public float GuideLineSampleRadius { get; private set; }

		[Header("Path Densification")]
		[Tooltip("One extra point is inserted per this many metres of segment length, so short segments get none and long ones follow the surface.")]
		[field: SerializeField]
		public float GuideLinePointSpacing { get; private set; } = 0.5f;

		[Tooltip("Upper bound on extra points for a single segment, so a very long open run cannot flood the line.")]
		[field: SerializeField]
		public int GuideLineMaxPointsPerSegment { get; private set; } = 32;

		[Tooltip("How far each inserted point probes up and down for the floor. Only needs to cover the local rise, not a whole storey — keep it under the floor-to-ceiling height.")]
		[field: SerializeField]
		public float GuideLineGroundProbeDistance { get; private set; } = 1f;

		[field: SerializeField]
		public float PathPointsGroundOffset { get; private set; } = 0.5f;

		[field: SerializeField]
		public int ArcSampleCount { get; private set; } = 24;

		[field: SerializeField]
		public float ArcMaxHeight { get; private set; } = 3f;

		[field: SerializeField]
		public float ArcMinHeight { get; private set; } = 0.5f;

		[field: SerializeField]
		public float ArcMaxHeightDistance { get; private set; } = 2f;

		[field: SerializeField]
		public float ArcMinHeightDistance { get; private set; } = 15f;
	}
}
