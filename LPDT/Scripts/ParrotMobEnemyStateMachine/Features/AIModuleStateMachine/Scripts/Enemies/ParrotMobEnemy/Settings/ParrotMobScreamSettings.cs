using FMODUnity;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Settings
{
	[CreateAssetMenu(fileName = "ParrotMobScreamSettings_Default", menuName = "Configurations/AIModuleStateMachine/ParrotMob/ParrotMobScreamSettings")]
	public class ParrotMobScreamSettings : ScriptableObject
	{
		[field: SerializeField]
		public float ScreamDuration { get; private set; } = 2.5f;

		[field: SerializeField]
		public float AttractRadius { get; private set; } = 25f;

		[field: SerializeField]
		public float ScreamCooldown { get; private set; } = 8f;

		[field: SerializeField]
		public float LookRotationSpeed { get; private set; } = 4f;

		[Tooltip("Radius around the parrot where attracted enemies pick their approach point.")]
		[field: SerializeField]
		public float ApproachRadius { get; private set; } = 5f;

		[Tooltip("SciFi barrier VFX prefab (e.g. vfx_SciFi_Barrier_Dots_v1). Spawned on all clients while screaming.")]
		[field: Header("Scream VFX")]
		[field: SerializeField]
		public GameObject ScreamVfxPrefab { get; private set; }

		[Tooltip("Seconds to grow BarrierSize from 0 to AttractRadius * ScreamVfxSizeMultiplier.")]
		[field: SerializeField]
		public float ScreamVfxGrowDuration { get; private set; } = 1.5f;

		[Tooltip("Multiplier applied to AttractRadius when writing the VFX BarrierSize property.")]
		[field: SerializeField]
		public float ScreamVfxSizeMultiplier { get; private set; } = 1f;

		[Tooltip("Looping FMOD event played while the parrot is screaming. Assign on ParrotMobScreamSettings asset in installer.")]
		[field: Header("Audio")]
		[field: SerializeField]
		public EventReference ScreamSound { get; private set; }

		[Tooltip("Scales the growing VFX barrier radius into FMOD MAXIMUM_DISTANCE (1 = matches visual barrier size).")]
		[field: SerializeField]
		public float ScreamAudibleDistanceMultiplier { get; private set; } = 1f;

		[Tooltip("FMOD max hear distance while the barrier is still at size 0 (player always hears the parrot nearby when screaming).")]
		[field: SerializeField]
		public float ScreamAudibleMinDistance { get; private set; } = 5f;
	}
}
