using System;
using Ami.BroAudio;
using EvilCore.Particles;
using UnityEngine;
using UnityEngine.Serialization;

namespace NomadDrive.Features.Vehicle.Collision
{
	[Serializable]
	public class TerrainTreeDestructionEntry
	{
		public GameObject treePrefab;

		public bool isDestructible = true;

		public float minimumForceToDestroy = 2000f;

		public GameObject destructionPrefab;

		public ParticleKey destructionParticle;

		[FormerlySerializedAs("destructionAudioEvent")]
		public SoundID destructionSound;

		public float debrisLifetime = 10f;

		[Range(0f, 1f)]
		public float debrisPushFraction = 0.15f;
	}
}
