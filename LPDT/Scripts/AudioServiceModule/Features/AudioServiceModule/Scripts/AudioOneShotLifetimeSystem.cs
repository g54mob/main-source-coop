using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Features.GameUpdaterModule;
using Zenject;

namespace Features.AudioServiceModule.Scripts
{
	public class AudioOneShotLifetimeSystem : IInitializable, IDisposable
	{
		private readonly AudioModel _audioModel;

		private readonly IGameUpdater _gameUpdater;

		private readonly List<EventInstance> _oneShotInstances = new List<EventInstance>();

		public AudioOneShotLifetimeSystem(AudioModel audioModel, IGameUpdater gameUpdater)
		{
			_audioModel = audioModel;
			_gameUpdater = gameUpdater;
		}

		public void Initialize()
		{
			_audioModel.OnOneShotStarted += Track;
			_gameUpdater.OnUpdate += ReleaseFinished;
		}

		public void Dispose()
		{
			_audioModel.OnOneShotStarted -= Track;
			_gameUpdater.OnUpdate -= ReleaseFinished;
			_oneShotInstances.Clear();
		}

		private void Track(EventInstance soundInstance, ISoundSource iSoundSource)
		{
			_oneShotInstances.Add(soundInstance);
		}

		private void ReleaseFinished()
		{
			for (int num = _oneShotInstances.Count - 1; num >= 0; num--)
			{
				EventInstance instance = _oneShotInstances[num];
				if (instance.isValid())
				{
					instance.getPlaybackState(out var state);
					if (state != PLAYBACK_STATE.STOPPED)
					{
						continue;
					}
				}
				_oneShotInstances.RemoveAt(num);
				RuntimeManager.DetachInstanceFromGameObject(instance);
				instance.release();
			}
		}
	}
}
