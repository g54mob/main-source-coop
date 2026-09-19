using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.VoiceControlModule.Scripts
{
	public interface IVoiceService
	{
		UniTask SpawnNon3dVoiceSpeaker(int playerId, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion));

		void DespawnNon3dVoiceSpeaker(int playerId);

		UniTask SpawnVoiceSpeaker(int playerId, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion));

		void DespawnVoiceSpeaker(int playerId);

		void ProcessFadeEffectForSpeaker(int playerId, float targetValue, float duration, string effectName);

		void ProcessEffectForSpeaker(int playerId, float targetValue, string effectName);

		void ProcessEffectForSpeakerNon3d(int playerId, float targetValue, string effectName);

		bool TryGetEffectValueForSpeaker(int playerId, string effectName, out float value);

		void ProcessFadeVolumeEffectForSpeaker(int playerId, float targetValue, float duration);

		void ProcessFadeVolumeEffectForSpeakerNon3d(int playerId, float targetValue, float duration);

		void SetVolumeForSpeakerNon3d(int playerId, float targetValue);

		void SetVolumeForSpeaker(int playerId, float targetValue);

		float GetVolumeForSpeaker(int playerId);

		bool IsVoiceEffectPlaying(int playerId);
	}
}
