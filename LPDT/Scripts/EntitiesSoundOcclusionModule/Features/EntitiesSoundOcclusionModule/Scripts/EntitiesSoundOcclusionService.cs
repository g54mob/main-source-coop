using FMOD.Studio;
using Features.AudioServiceModule.Scripts;
using UnityEngine;

namespace Features.EntitiesSoundOcclusionModule.Scripts
{
	public class EntitiesSoundOcclusionService : IEntitiesSoundOcclusionService
	{
		private readonly EntitiesSoundOcclusionModel _entitiesSoundOcclusionModel;

		public EntitiesSoundOcclusionService(EntitiesSoundOcclusionModel entitiesSoundOcclusionModel)
		{
			_entitiesSoundOcclusionModel = entitiesSoundOcclusionModel;
		}

		public void TriggerOcclusionForEntities(Vector3 soundPosition, float soundDistance, ISoundSource soundSource, string soundPath = null)
		{
			_entitiesSoundOcclusionModel.TriggerEntitiesTriggeredBySound(soundPosition, soundDistance, soundSource, soundPath);
		}

		public void StartTrackingSound(EventInstance soundInstance, ISoundSource soundSource)
		{
			_entitiesSoundOcclusionModel.TrackedSounds[soundInstance] = soundSource;
		}

		public void StopTrackingSound(EventInstance soundInstance)
		{
			_entitiesSoundOcclusionModel.TrackedSounds.Remove(soundInstance);
		}
	}
}
