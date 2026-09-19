using FMOD.Studio;
using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using Features.AudioServiceModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy
{
	public class MonkeyAnimatorPresenter : MonoBehaviour
	{
		private readonly int _speedHash = Animator.StringToHash("Speed");

		private readonly int _hungerHash = Animator.StringToHash("Hunger");

		private MonkeyEnemy _enemy;

		private MonkeyEnemyContext _context;

		private MonkeyEnemySettings _enemySettings;

		private MonkeyMovementSettings _movementSettings;

		private MonkeyPlayerInteractionSettings _interactionSettings;

		private MonkeyCombatSettings _combatSettings;

		private MonkeyItemInteractionSettings _itemSettings;

		private MonkeyFearSettings _fearSettings;

		private IAudioService _audioService;

		private float _currentAnimatorSpeed;

		private bool _isMoveSoundPlaying;

		private bool _isPlayingAngrySound;

		[Inject]
		private void InjectDependencies(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyEnemySettings enemySettings, MonkeyMovementSettings movementSettings, MonkeyPlayerInteractionSettings interactionSettings, MonkeyCombatSettings combatSettings, MonkeyItemInteractionSettings itemSettings, MonkeyFearSettings fearSettings, IAudioService audioService)
		{
			_enemy = enemy;
			_context = context;
			_enemySettings = enemySettings;
			_movementSettings = movementSettings;
			_interactionSettings = interactionSettings;
			_combatSettings = combatSettings;
			_itemSettings = itemSettings;
			_fearSettings = fearSettings;
			_audioService = audioService;
		}

		private void Update()
		{
			if (!(_enemy == null) && !(_enemy.Object == null) && _enemy.Object.IsValid)
			{
				_context.UpdateFmod3DAttributes();
				ApplyAnimatorFloats();
				ApplyCoinVisibility();
				ApplyFakeCoinVisibility();
				UpdateMoveSound();
			}
		}

		private void ApplyAnimatorFloats()
		{
			float visualMaxSpeed = GetVisualMaxSpeed(_enemy.VisualState);
			float b = ((visualMaxSpeed > 0f) ? Mathf.Clamp01(_enemy.AgentVelocity / visualMaxSpeed) : 0f);
			_currentAnimatorSpeed = Mathf.Lerp(_currentAnimatorSpeed, b, _enemySettings.SpeedAnimationLerpSpeed * Time.deltaTime);
			_context.Animator.SetFloat(_speedHash, _currentAnimatorSpeed);
			_context.Animator.SetFloat(_hungerHash, _enemy.HungerPercent);
		}

		private void ApplyCoinVisibility()
		{
			if (_context.CoinObject.activeSelf != _enemy.IsCoinVisible)
			{
				_context.CoinObject.SetActive(_enemy.IsCoinVisible);
			}
		}

		private void ApplyFakeCoinVisibility()
		{
			bool flag = _enemy.VisualState != MonkeyVisualState.CoinHideout && _enemy.VisualState != MonkeyVisualState.GivingItem;
			if (_context.FakeCoinObject.activeSelf != flag)
			{
				_context.FakeCoinObject.SetActive(flag);
			}
		}

		private void UpdateMoveSound()
		{
			if (!(_enemy.AgentVelocity > 0.1f) || !IsMovementVisualState(_enemy.VisualState))
			{
				StopMoveSound();
			}
			else if (_enemy.IsStarving)
			{
				if (!_isMoveSoundPlaying || !_isPlayingAngrySound)
				{
					if (_isMoveSoundPlaying)
					{
						_audioService.StopInstance(_context.MoveFriendlyInstance, STOP_MODE.ALLOWFADEOUT);
					}
					_audioService.StartInstanceWith3DAttributes(_context.MoveAngryInstance, _context.SoundSourceBehaviour);
					_isMoveSoundPlaying = true;
					_isPlayingAngrySound = true;
				}
			}
			else if (!_isMoveSoundPlaying || _isPlayingAngrySound)
			{
				if (_isMoveSoundPlaying)
				{
					_audioService.StopInstance(_context.MoveAngryInstance, STOP_MODE.ALLOWFADEOUT);
				}
				_audioService.StartInstanceWith3DAttributes(_context.MoveFriendlyInstance, _context.SoundSourceBehaviour);
				_isMoveSoundPlaying = true;
				_isPlayingAngrySound = false;
			}
		}

		private void StopMoveSound()
		{
			if (_isMoveSoundPlaying)
			{
				if (_isPlayingAngrySound)
				{
					_audioService.StopInstance(_context.MoveAngryInstance, STOP_MODE.ALLOWFADEOUT);
				}
				else
				{
					_audioService.StopInstance(_context.MoveFriendlyInstance, STOP_MODE.ALLOWFADEOUT);
				}
				_isMoveSoundPlaying = false;
				_isPlayingAngrySound = false;
			}
		}

		private float GetVisualMaxSpeed(MonkeyVisualState state)
		{
			return state switch
			{
				MonkeyVisualState.Wandering => _movementSettings.WanderingSpeed, 
				MonkeyVisualState.InteractionChasing => _interactionSettings.InteractionChaseSpeed, 
				MonkeyVisualState.LastChance => _interactionSettings.InteractionChaseSpeed, 
				MonkeyVisualState.RunAround => _interactionSettings.RunAroundSpeed, 
				MonkeyVisualState.FakeAttacking => _interactionSettings.RunAroundSpeed, 
				MonkeyVisualState.CombatChasing => _combatSettings.ChasingSpeed, 
				MonkeyVisualState.MoveToItem => _itemSettings.MoveToItemSpeed, 
				MonkeyVisualState.CoinHideout => _itemSettings.HideoutFleeSpeed, 
				MonkeyVisualState.Fear => _fearSettings.FearSpeed, 
				_ => 0f, 
			};
		}

		private static bool IsMovementVisualState(MonkeyVisualState state)
		{
			if (state != MonkeyVisualState.InteractionChasing && state != MonkeyVisualState.LastChance && state != MonkeyVisualState.RunAround && state != MonkeyVisualState.FakeAttacking && state != MonkeyVisualState.CombatChasing && state != MonkeyVisualState.MoveToItem && state != MonkeyVisualState.CoinHideout)
			{
				return state == MonkeyVisualState.Fear;
			}
			return true;
		}
	}
}
