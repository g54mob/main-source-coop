using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy
{
	public class SleeperAudioController : MonoBehaviour
	{
		[SerializeField]
		private EventReference _sleeperQuestionRoarReference;

		[SerializeField]
		private EventReference _sleeperDamageRoarReference;

		[SerializeField]
		private EventReference _sleepLoopReference;

		private SleeperEnemyContext _context;

		private IAudioService _audioService;

		private EventInstance _sleepLoopInstance;

		private bool _sleepLoopPlaying;

		private SleeperVisualState _lastVisualState;

		[Inject]
		public void InjectDependencies(SleeperEnemyContext context, IAudioService audioService)
		{
			_context = context;
			_audioService = audioService;
		}

		private void Update()
		{
			SleeperVisualState visualState = _context.VisualState;
			if (visualState != _lastVisualState)
			{
				if (_lastVisualState == SleeperVisualState.Sleep)
				{
					StopSleepLoop();
				}
				switch (visualState)
				{
				case SleeperVisualState.Sleep:
					StartSleepLoop();
					break;
				case SleeperVisualState.WakeUp:
					_context.PlayOccludedOneShot(_sleeperQuestionRoarReference);
					break;
				case SleeperVisualState.DamageAggro:
					_context.PlayOccludedOneShot(_sleeperDamageRoarReference);
					break;
				}
				_lastVisualState = visualState;
			}
		}

		private void LateUpdate()
		{
			if (_sleepLoopPlaying)
			{
				_sleepLoopInstance.set3DAttributes(_context.SoundSourceBehaviour.SoundSourceTransform.position.To3DAttributes());
				_context.ApplyOcclusion(_sleepLoopInstance, smooth: true);
			}
		}

		private void StartSleepLoop()
		{
			if (!_sleepLoopPlaying && !_sleepLoopReference.IsNull && _audioService != null && !(_context?.SoundSourceBehaviour == null))
			{
				_sleepLoopInstance = _audioService.CreateInstance(_sleepLoopReference);
				_audioService.StartInstanceWith3DAttributes(_sleepLoopInstance, _context.SoundSourceBehaviour);
				_context.ApplyOcclusion(_sleepLoopInstance, smooth: false);
				_sleepLoopPlaying = true;
			}
		}

		private void StopSleepLoop()
		{
			if (_sleepLoopPlaying)
			{
				_audioService.StopInstance(_sleepLoopInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				_audioService.ReleaseInstance(_sleepLoopInstance);
				_sleepLoopPlaying = false;
			}
		}

		private void OnDestroy()
		{
			if (_sleepLoopPlaying)
			{
				_audioService.StopInstance(_sleepLoopInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
				_audioService.ReleaseInstance(_sleepLoopInstance);
				_sleepLoopPlaying = false;
			}
		}
	}
}
