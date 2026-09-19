using System;
using FMOD;
using FMOD.Studio;
using Features.AudioServiceModule.Scripts;
using Zenject;

namespace Features.EntitiesSoundOcclusionModule.Scripts
{
	public class AudioOcclusionRoutingSystem : IInitializable, IDisposable
	{
		private readonly AudioModel _audioModel;

		private readonly IEntitiesSoundOcclusionService _entitiesSoundOcclusionService;

		private readonly EntitiesSoundOcclusionConfiguration _entitiesSoundOcclusionConfiguration;

		public AudioOcclusionRoutingSystem(AudioModel audioModel, IEntitiesSoundOcclusionService entitiesSoundOcclusionService, EntitiesSoundOcclusionConfiguration entitiesSoundOcclusionConfiguration)
		{
			_audioModel = audioModel;
			_entitiesSoundOcclusionService = entitiesSoundOcclusionService;
			_entitiesSoundOcclusionConfiguration = entitiesSoundOcclusionConfiguration;
		}

		public void Initialize()
		{
			_audioModel.OnInstanceStarted += OnInstanceStarted;
			_audioModel.OnInstanceStopped += OnInstanceStopped;
		}

		public void Dispose()
		{
			_audioModel.OnInstanceStarted -= OnInstanceStarted;
			_audioModel.OnInstanceStopped -= OnInstanceStopped;
		}

		private void OnInstanceStarted(EventInstance soundInstance, ISoundSource soundSource)
		{
			if (!IsBlacklisted(soundInstance))
			{
				_entitiesSoundOcclusionService.StartTrackingSound(soundInstance, soundSource);
			}
		}

		private void OnInstanceStopped(EventInstance soundInstance)
		{
			_entitiesSoundOcclusionService.StopTrackingSound(soundInstance);
		}

		private bool IsBlacklisted(EventInstance soundInstance)
		{
			if (soundInstance.getDescription(out var description) != RESULT.OK)
			{
				return false;
			}
			if (description.getID(out var id) != RESULT.OK)
			{
				return false;
			}
			return _entitiesSoundOcclusionConfiguration.IsBlacklisted(id);
		}
	}
}
