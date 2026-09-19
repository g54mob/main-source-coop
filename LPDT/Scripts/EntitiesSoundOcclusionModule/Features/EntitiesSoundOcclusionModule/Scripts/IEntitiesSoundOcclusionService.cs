using FMOD.Studio;
using Features.AudioServiceModule.Scripts;
using UnityEngine;

namespace Features.EntitiesSoundOcclusionModule.Scripts
{
	public interface IEntitiesSoundOcclusionService
	{
		void TriggerOcclusionForEntities(Vector3 soundPosition, float soundDistance, ISoundSource soundSource, string soundPath = null);

		void StartTrackingSound(EventInstance soundInstance, ISoundSource soundSource);

		void StopTrackingSound(EventInstance soundInstance);
	}
}
