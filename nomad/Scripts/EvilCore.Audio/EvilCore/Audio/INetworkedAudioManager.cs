using Ami.BroAudio;
using Mirror;
using UnityEngine;

namespace EvilCore.Audio
{
	public interface INetworkedAudioManager
	{
		void PlayOneShot(SoundID id, Vector3 position);

		void PlayOneShotExcludeSelf(SoundID id, Vector3 position);

		void PlayOneShotAttached(SoundID id, NetworkIdentity attachTo);

		void PlayOneShotAttachedExcludeSelf(SoundID id, NetworkIdentity attachTo);

		uint PlayLooping(SoundID id, Vector3 position);

		uint PlayLoopingAttached(SoundID id, NetworkIdentity attachTo);

		void StopLooping(uint soundId, bool allowFadeout = true);

		bool IsLoopingPlaying(uint soundId);

		void SetLoopingParameter(uint soundId, string parameterName, float value);
	}
}
