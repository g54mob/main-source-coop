using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace Features.BarBeachInteractableModule.Scripts
{
	[CreateAssetMenu(fileName = "BarBeachInteractableConfiguration_Default", menuName = "Configurations/BarBeachInteractableModule/BarBeachInteractableConfiguration")]
	public class BarBeachInteractableConfiguration : ScriptableObject
	{
		[Header("Serve")]
		[field: SerializeField]
		[field: Min(0f)]
		public float ServeCooldownSeconds { get; private set; } = 0.4f;

		[field: SerializeField]
		[field: Range(1f, 4f)]
		public int MaxBottlesOnCounter { get; private set; } = 4;

		[field: SerializeField]
		[field: Min(0f)]
		public int InitialServeStock { get; private set; } = 4;

		[field: SerializeField]
		[field: Min(0f)]
		public int RestockAmount { get; private set; } = 4;

		[field: SerializeField]
		[field: Min(0f)]
		public float SlotLeaveRadius { get; private set; } = 0.2f;

		[field: SerializeField]
		[field: Min(0f)]
		public float SlotLeaveDropY { get; private set; } = 0.12f;

		[Header("Slide (CardsJumper-style)")]
		[field: SerializeField]
		[field: Min(0.05f)]
		public float SlideDuration { get; private set; } = 0.75f;

		[field: SerializeField]
		[field: Min(0f)]
		public float SlideHeight { get; private set; } = 0.08f;

		[field: SerializeField]
		public AnimationCurve SlideHeightCurve { get; private set; } = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);

		[field: SerializeField]
		public AnimationCurve SlideProgressCurve { get; private set; } = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[field: SerializeField]
		public float SlideYawSpinDegrees { get; private set; } = 360f;

		[Header("Bottles (effect is on each prefab)")]
		[field: SerializeField]
		public List<BarBottlePrefabWeight> BottlePrefabWeights { get; private set; } = new List<BarBottlePrefabWeight>();

		[Header("Music")]
		[field: SerializeField]
		public List<EventReference> MusicTracks { get; private set; } = new List<EventReference>();

		[field: SerializeField]
		public EventReference MusicBellRingEvent { get; private set; }

		[Header("WaterBell")]
		[field: SerializeField]
		public EventReference WaterBellSplashEvent { get; private set; }
	}
}
