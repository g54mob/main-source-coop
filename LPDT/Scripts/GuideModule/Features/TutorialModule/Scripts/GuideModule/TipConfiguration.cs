using Global.SerializableDictionary;
using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	[CreateAssetMenu(fileName = "TipConfiguration_Default", menuName = "Configurations/GuideModule/TipConfiguration")]
	public class TipConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<TipType, TipEntity> Prefabs { get; private set; }

		[field: SerializeField]
		public int InitialPoolSize { get; private set; } = 4;

		[field: SerializeField]
		public int MaxCapacity { get; private set; } = 16;

		[field: SerializeField]
		public float DissolveAnimationDuration { get; private set; } = 0.4f;

		[Header("Projection (used when CreateTip project = true)")]
		[field: SerializeField]
		public LayerMask ProjectionLayerMask { get; private set; }

		[field: SerializeField]
		public float ProjectionDistance { get; private set; } = 10f;

		[field: SerializeField]
		public float ProjectionUpOffset { get; private set; } = 1f;
	}
}
