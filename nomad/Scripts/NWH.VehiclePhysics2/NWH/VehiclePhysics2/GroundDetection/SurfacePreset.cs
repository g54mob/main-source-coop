using System;
using NWH.Common.Vehicles;
using UnityEngine;
using UnityEngine.Serialization;

namespace NWH.VehiclePhysics2.GroundDetection
{
	[Serializable]
	[CreateAssetMenu(fileName = "NWH Vehicle Physics 2", menuName = "NWH/Vehicle Physics 2/Surface Preset", order = 1)]
	public class SurfacePreset : ScriptableObject
	{
		public enum ParticleType
		{
			Smoke = 0,
			Dust = 1
		}

		[Tooltip("Type of particles generated.\r\n- Smoke - depends on wheel slip. Use for hard surfaces. Will not emit when there is no wheel slip.\r\n- Dust - depends on speed only. Use for dusty surfaces, e.g. gravel or sand.")]
		public ParticleType particleType;

		[FormerlySerializedAs("dustColor")]
		[Tooltip("    Color of generated particles on this surface type.")]
		public Color particleColor = new Color(0.9f, 0.9f, 0.9f, 0.9f);

		[Range(0f, 2f)]
		[Tooltip("    Maximum amount of particles emitted over distance.")]
		public float maxParticleEmissionRateOverDistance = 0.36f;

		[Tooltip("    Initial size of the emitted particles.")]
		public float particleSize = 1f;

		[Tooltip("    Should the particles be emitted on this surface type?")]
		public bool emitParticles = true;

		[Tooltip("    Should dirt chunks / stones be thrown behind the wheel on this surface type?")]
		public bool emitChunks;

		[Tooltip("    Maximum amount of chunks emitted over distance.")]
		public float maxChunkEmissionRateOverDistance = 1f;

		[Tooltip("    Determines maximum distance from the wheel that the chunk can stay alive.")]
		public float chunkLifeDistance = 3f;

		[FormerlySerializedAs("maxChunkLifeTime")]
		[Tooltip("    Maximum life time of an emitted chunk.")]
		public float maxChunkLifetime = 0.5f;

		[Range(0f, 1f)]
		[Tooltip("    Maximum alpha value start color of an emitted particle can achieve.")]
		public float particleMaxAlpha = 0.8f;

		[Tooltip("    Maximum particle start lifetime.")]
		public float maxParticleLifetime = 3.5f;

		[Tooltip("    Maximum distance from the vehicle a particle can achieve.")]
		public float particleLifeDistance = 10f;

		[Tooltip("Friction preset of WC3D that will be used for this surface. More presets can be added in WheelController.FrictionPresets.")]
		public FrictionPreset frictionPreset;

		[Range(1f, 50f)]
		[Tooltip("Multiplies the rolling resistance of the WheelUAPI by this value based on the load on the tire.\r\nUseful for surfaces such as sand where the wheel digs into the material.")]
		public float rollingResistanceMaxMultiplier = 1f;

		[Tooltip("Name of the surface map.")]
		public new string name;

		[Tooltip("    AudioClip used for wheel skidding sound effect.")]
		public AudioClip skidSoundClip;

		[Tooltip("    Should tire skid sounds be played for this surface type?")]
		public bool playSkidSounds = true;

		[Tooltip("    Sound pitch of wheel skidding over the surface.")]
		public float skidSoundPitch = 1f;

		[Tooltip("    Sound volume of wheel skidding over the surface.")]
		public float skidSoundVolume = 0.3f;

		[Range(0f, 1f)]
		public float slipFactor = 0.5f;

		[FormerlySerializedAs("slipSensitiveSound")]
		[Tooltip("If set to true surface volume will be dependent on slip (asphalt, concrete, etc.). Set to false for dirt, grass and other soft surfaces.")]
		public bool slipSensitiveSurfaceSound;

		[Tooltip("    Should tire rolling over the surface sound be played for this surface type?")]
		public bool playSurfaceSounds = true;

		[Tooltip("    AudioClip used for wheel rolling sound effect.")]
		public AudioClip surfaceSoundClip;

		[Tooltip("    Sound pitch of wheel rolling over the surface.")]
		public float surfaceSoundPitch = 1f;

		[Tooltip("    Sound volume of wheel rolling over the surface.")]
		public float surfaceSoundVolume = 0.3f;

		[Tooltip("    Should skid/thread marks be drawn on this surface?")]
		public bool drawSkidmarks = true;

		[Tooltip("    Material used for skid/thread marks on this type of surface.")]
		public Material skidmarkMaterial;

		[FormerlySerializedAs("baseIntensity")]
		[Range(0f, 1f)]
		[Tooltip("Intensity of the skidmarks when there is no wheel slip.\r\nSet to 0 for hard surfaces and >0 for soft surfaces where the tire leaves the mark by rolling over it.")]
		public float skidmarkBaseIntensity = 0.5f;
	}
}
