using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using Features.AudioServiceModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy
{
	public class MonkeyPorterAnimatorPresenter : MonoBehaviour
	{
		private readonly int _speedHash = Animator.StringToHash("Speed");

		private readonly int _complainHash = Animator.StringToHash("Complain");

		private readonly int _angryFaceHash = Animator.StringToHash("Hunger");

		private readonly int _runTriggerHash = Animator.StringToHash("Run");

		private readonly int _walkTriggerHash = Animator.StringToHash("MoveSlow");

		private static readonly int _runStateHash = Animator.StringToHash("Run_Tree");

		private MonkeyPorterEnemy _enemy;

		private MonkeyPorterContext _context;

		private MonkeyPorterSettings _settings;

		private IAudioService _audioService;

		private float _currentAnimatorSpeed;

		private float _currentAngryFace;

		private float _complainCooldownLeft;

		private float _panicCooldownLeft;

		private MonkeyPorterVisualState _lastVisualState;

		private bool _isRunClipRequested;

		private readonly Dictionary<int, bool> _resolvedParameters = new Dictionary<int, bool>();

		private bool IsPanicking => _enemy.VisualState == MonkeyPorterVisualState.Panic;

		[Inject]
		public void InjectDependencies(MonkeyPorterEnemy enemy, MonkeyPorterContext context, MonkeyPorterSettings settings, IAudioService audioService)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
			_audioService = audioService;
		}

		private void Update()
		{
			if (!(_enemy == null) && !(_enemy.Object == null) && _enemy.Object.IsValid)
			{
				UpdateSpeed();
				UpdateComplain();
				UpdatePanic();
				UpdateLocomotionClip();
			}
		}

		private void UpdateLocomotionClip()
		{
			if (HasParameter(_runTriggerHash) && HasParameter(_walkTriggerHash))
			{
				bool isPanicking = IsPanicking;
				if (isPanicking == _isRunClipRequested)
				{
					ReassertRunClip();
					return;
				}
				_isRunClipRequested = isPanicking;
				_context.Animator.ResetTrigger(isPanicking ? _walkTriggerHash : _runTriggerHash);
				_context.Animator.SetTrigger(isPanicking ? _runTriggerHash : _walkTriggerHash);
			}
		}

		private void ReassertRunClip()
		{
			if (_isRunClipRequested && !_context.Animator.IsInTransition(0) && _context.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash != _runStateHash)
			{
				_context.Animator.SetTrigger(_runTriggerHash);
			}
		}

		private void UpdateSpeed()
		{
			float num = (IsPanicking ? _settings.FleeSpeed : Mathf.Max(_settings.MoveSpeed, _settings.CarrySpeed));
			float b = ((num > 0f) ? Mathf.Clamp01(_enemy.AgentVelocity / num) : 0f);
			_currentAnimatorSpeed = Mathf.Lerp(_currentAnimatorSpeed, b, _settings.SpeedAnimationLerpSpeed * Time.deltaTime);
			_context.Animator.SetFloat(_speedHash, _currentAnimatorSpeed);
		}

		private void UpdatePanic()
		{
			_panicCooldownLeft -= Time.deltaTime;
			MonkeyPorterVisualState visualState = _enemy.VisualState;
			MonkeyPorterVisualState lastVisualState = _lastVisualState;
			_lastVisualState = visualState;
			if (visualState == MonkeyPorterVisualState.Panic && lastVisualState != MonkeyPorterVisualState.Panic && !(_panicCooldownLeft > 0f))
			{
				_panicCooldownLeft = _settings.PanicSoundCooldown;
				PlayPanic();
			}
		}

		private void PlayPanic()
		{
			if (HasComplainParameter())
			{
				_context.Animator.SetTrigger(_complainHash);
			}
			if (!(_context.SoundSourceBehaviour == null) && !_context.PanicSound.IsNull)
			{
				_audioService.PlayOneShotAttached(_context.PanicSound, _context.SoundSourceBehaviour);
			}
		}

		private void UpdateComplain()
		{
			_complainCooldownLeft -= Time.deltaTime;
			bool flag = _enemy.VisualState == MonkeyPorterVisualState.Blocked;
			bool flag2 = IsPanicking || _enemy.VisualState == MonkeyPorterVisualState.Hiding;
			_currentAngryFace = Mathf.MoveTowards(_currentAngryFace, (flag || flag2) ? 1f : 0f, _settings.AngryFaceLerpSpeed * Time.deltaTime);
			_context.Animator.SetFloat(_angryFaceHash, _currentAngryFace);
			if (flag && !(_complainCooldownLeft > 0f))
			{
				_complainCooldownLeft = _settings.ComplainCooldown;
				PlayComplain();
			}
		}

		private void PlayComplain()
		{
			if (HasComplainParameter())
			{
				_context.Animator.SetTrigger(_complainHash);
			}
			if (!(_context.SoundSourceBehaviour == null) && !_context.ComplainSound.IsNull)
			{
				_audioService.PlayOneShotAttached(_context.ComplainSound, _context.SoundSourceBehaviour);
			}
		}

		private bool HasComplainParameter()
		{
			return HasParameter(_complainHash);
		}

		private bool HasParameter(int nameHash)
		{
			if (_resolvedParameters.TryGetValue(nameHash, out var value))
			{
				return value;
			}
			value = false;
			AnimatorControllerParameter[] parameters = _context.Animator.parameters;
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].nameHash == nameHash)
				{
					value = true;
					break;
				}
			}
			_resolvedParameters[nameHash] = value;
			return value;
		}
	}
}
