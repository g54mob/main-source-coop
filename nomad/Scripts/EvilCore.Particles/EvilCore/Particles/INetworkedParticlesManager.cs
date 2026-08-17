using Mirror;
using UnityEngine;

namespace EvilCore.Particles
{
	public interface INetworkedParticlesManager
	{
		void PlayNetworkedOneShot(string key, Vector3 position, Quaternion rotation = default(Quaternion));

		void PlayNetworkedOneShot(string key, Vector3 position, Quaternion rotation, NetworkParticleOverrides overrides);

		void PlayNetworkedOneShotExcludeSelf(string key, Vector3 position, Quaternion rotation = default(Quaternion));

		void PlayNetworkedOneShotAttached(string key, NetworkIdentity attachTo, Vector3 localOffset = default(Vector3));

		uint PlayNetworkedPersistent(string key, Vector3 position, Quaternion rotation = default(Quaternion));

		uint PlayNetworkedPersistentAttached(string key, NetworkIdentity attachTo, Vector3 localOffset = default(Vector3));

		void StopNetworkedPersistent(uint particleId, bool clear = false);

		bool IsNetworkedPersistentPlaying(uint particleId);
	}
}
