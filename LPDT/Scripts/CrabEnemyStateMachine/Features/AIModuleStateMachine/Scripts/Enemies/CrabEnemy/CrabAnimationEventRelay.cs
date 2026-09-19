using FMODUnity;
using Features.AudioServiceModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy
{
	public class CrabAnimationEventRelay : MonoBehaviour
	{
		private const float StepDirectionDeadZone = 0.05f;

		[SerializeField]
		private SoundSourceBehaviour _soundSource;

		[SerializeField]
		private EventReference _stepSound;

		private CrabEnemyContext _context;

		private IAudioService _audioService;

		[Inject]
		private void InjectDependencies(CrabEnemyContext context, IAudioService audioService)
		{
			_context = context;
			_audioService = audioService;
		}

		public void OnDetachCompleted()
		{
			if (_context != null)
			{
				_context.RequestDetachCompleted();
			}
		}

		public void OnStepLeft()
		{
			if (ShouldPlayStep(forLeft: true))
			{
				PlayStepSound();
			}
		}

		public void OnStepRight()
		{
			if (ShouldPlayStep(forLeft: false))
			{
				PlayStepSound();
			}
		}

		private bool ShouldPlayStep(bool forLeft)
		{
			if (_context == null)
			{
				return false;
			}
			float direction = _context.Direction;
			if (Mathf.Abs(direction) <= 0.05f)
			{
				return false;
			}
			if (!forLeft)
			{
				return direction > 0f;
			}
			return direction < 0f;
		}

		private void PlayStepSound()
		{
			if (_audioService != null && !_stepSound.IsNull && !(_soundSource == null))
			{
				_audioService.PlayOneShot(_stepSound, _soundSource);
			}
		}
	}
}
