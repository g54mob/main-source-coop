using UnityEngine;

namespace EvilCore.Particles
{
	public interface IParticlesManager
	{
		void PlayOneShot(string key, Vector3 position, Quaternion rotation = default(Quaternion));

		void PlayOneShot(string key, Vector3 position, Quaternion rotation, ParticleOverrides overrides);

		void PlayOneShotAttached(string key, Transform parent, Vector3 localOffset = default(Vector3));

		ParticleHandle Play(string key, Vector3 position, Quaternion rotation = default(Quaternion));

		ParticleHandle PlayAttached(string key, Transform parent, Vector3 localOffset = default(Vector3));

		void Stop(ParticleHandle handle, bool clear = false);

		void Warmup(string key, int count);

		void ClearAllPools();
	}
}
