using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.Particles;
using NomadDrive.Features.Vehicle.Collision;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleCollisionEffectsModule : VehicleModule
	{
		[Header("Audio Events")]
		[SerializeField]
		private SoundID lightCollisionAudio;

		[SerializeField]
		private SoundID mediumCollisionAudio;

		[SerializeField]
		private SoundID heavyCollisionAudio;

		[Header("Particle Effects")]
		[SerializeField]
		private ParticleKey lightCollisionParticle;

		[SerializeField]
		private ParticleKey mediumCollisionParticle;

		[SerializeField]
		private ParticleKey heavyCollisionParticle;

		[Inject]
		private IAudioManager _audioManager;

		[Inject]
		private IParticlesManager _particlesManager;

		protected override void SubscribeEvents()
		{
			base.EventBus.OnCollisionAllClients += OnCollisionAllClients;
		}

		protected override void UnsubscribeEvents()
		{
			base.EventBus.OnCollisionAllClients -= OnCollisionAllClients;
		}

		private void OnCollisionAllClients(VehicleCollisionData data)
		{
			SoundID id;
			ParticleKey particleKey;
			switch (data.Severity)
			{
			case CollisionSeverity.Heavy:
				id = heavyCollisionAudio;
				particleKey = heavyCollisionParticle;
				break;
			case CollisionSeverity.Medium:
				id = mediumCollisionAudio;
				particleKey = mediumCollisionParticle;
				break;
			default:
				id = lightCollisionAudio;
				particleKey = lightCollisionParticle;
				break;
			}
			if (id.IsValid())
			{
				_audioManager?.PlayOneShot(id, data.ImpactPoint);
			}
			if (particleKey.IsValid)
			{
				_particlesManager?.PlayOneShot(particleKey, data.ImpactPoint, Quaternion.LookRotation(data.ImpactNormal));
			}
		}
	}
}
