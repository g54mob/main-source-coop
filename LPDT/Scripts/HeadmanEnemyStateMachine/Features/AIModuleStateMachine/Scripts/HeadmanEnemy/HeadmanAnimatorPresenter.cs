using FMOD.Studio;
using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using Features.AudioServiceModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy
{
	public class HeadmanAnimatorPresenter : MonoBehaviour
	{
		[SerializeField]
		private HeadmanEnemy _enemy;

		[SerializeField]
		private HeadmanEnemyContext _context;

		private readonly int _wanderingHashTrigger = Animator.StringToHash("Wandering");

		private readonly int _chasingHashTrigger = Animator.StringToHash("Chasing");

		private readonly int _startChasingHashTrigger = Animator.StringToHash("StartChasing");

		private readonly int _baseAttackHashTrigger = Animator.StringToHash("BaseAttack");

		private readonly int _lowAttackHashTrigger = Animator.StringToHash("LowAttack");

		private readonly int _rageToTargetHashTrigger = Animator.StringToHash("RageToTarget");

		private readonly int _roarHashTrigger = Animator.StringToHash("Roar");

		private readonly int _inAttackHashBool = Animator.StringToHash("InAttack");

		private readonly int _inRageHashBool = Animator.StringToHash("InRage");

		private readonly int _rageClosetHashBool = Animator.StringToHash("RageCloset");

		private readonly int _rageTableHashBool = Animator.StringToHash("RageTable");

		private readonly int _rageBaseHashBool = Animator.StringToHash("RageBase");

		private readonly int _randomRageIndexHash = Animator.StringToHash("RandomRageIndex");

		private HeadmanVisualState _lastState;

		private bool _lastChasingLoop;

		private int _lastRoarSequence;

		private SafeZoneType _lastRageSafeZoneType;

		private int _lastRageRandomIndex = int.MinValue;

		private IAudioService _audioService;

		[Inject]
		public void InjectDependencies(IAudioService audioService)
		{
			_audioService = audioService;
		}

		private void Update()
		{
			if (!(_enemy == null) && !(_context == null) && _enemy.Object.IsValid)
			{
				_context.UpdateLongEventOcclusion();
				ApplyPresentation(_enemy.VisualState);
			}
		}

		private void ApplyPresentation(HeadmanVisualState state)
		{
			SyncChasingLoopSound(state);
			if (state != _lastState)
			{
				_lastState = state;
				Animator animator = _context.Animator;
				if (!(animator == null))
				{
					ApplyAnimatorForNewVisualState(animator, state);
				}
			}
		}

		private static bool IsChasingLoopVisual(HeadmanVisualState state)
		{
			if (state != HeadmanVisualState.ChasingStart && state != HeadmanVisualState.Chasing)
			{
				return state == HeadmanVisualState.Slowed;
			}
			return true;
		}

		private void SyncChasingLoopSound(HeadmanVisualState state)
		{
			bool flag = IsChasingLoopVisual(state);
			if (flag != _lastChasingLoop)
			{
				_lastChasingLoop = flag;
				if (flag)
				{
					_audioService.StartInstanceWith3DAttributes(_context.ChasingSoundInstance, _context.SoundSourceBehaviour);
				}
				else
				{
					_audioService.StopInstance(_context.ChasingSoundInstance, STOP_MODE.ALLOWFADEOUT);
				}
			}
		}

		private void ApplyAnimatorForNewVisualState(Animator animator, HeadmanVisualState state)
		{
			SetAttackAndRageBools(animator, state);
			ResetPresentationTriggers(animator, state);
			ApplyRoarSequenceTrigger(animator);
			ApplyRageInteractedLayer(animator, state);
			SetTriggerForVisualState(animator, state);
		}

		private void SetAttackAndRageBools(Animator animator, HeadmanVisualState state)
		{
			animator.SetBool(_inAttackHashBool, state == HeadmanVisualState.AttackingBase || state == HeadmanVisualState.AttackingLow);
			bool value = state == HeadmanVisualState.Rage || state == HeadmanVisualState.RageInteracted || state == HeadmanVisualState.RageEnd;
			animator.SetBool(_inRageHashBool, value);
		}

		private void ResetPresentationTriggers(Animator animator, HeadmanVisualState state)
		{
			animator.ResetTrigger(_wanderingHashTrigger);
			animator.ResetTrigger(_startChasingHashTrigger);
			animator.ResetTrigger(_rageToTargetHashTrigger);
			animator.ResetTrigger(_roarHashTrigger);
			if (state != HeadmanVisualState.AttackingBase)
			{
				animator.ResetTrigger(_baseAttackHashTrigger);
			}
			if (state != HeadmanVisualState.AttackingLow)
			{
				animator.ResetTrigger(_lowAttackHashTrigger);
			}
			if (state != HeadmanVisualState.AttackingBase && state != HeadmanVisualState.AttackingLow && state != HeadmanVisualState.Chasing && state != HeadmanVisualState.ChasingStart && state != HeadmanVisualState.Slowed)
			{
				animator.ResetTrigger(_chasingHashTrigger);
			}
		}

		private void ApplyRoarSequenceTrigger(Animator animator)
		{
			int roarSequence = _enemy.RoarSequence;
			if (roarSequence != _lastRoarSequence)
			{
				_lastRoarSequence = roarSequence;
				animator.SetTrigger(_roarHashTrigger);
			}
		}

		private void ApplyRageInteractedLayer(Animator animator, HeadmanVisualState state)
		{
			if (state == HeadmanVisualState.RageInteracted)
			{
				SafeZoneType rageSafeZoneType = _enemy.RageSafeZoneType;
				int rageRandomIndex = _enemy.RageRandomIndex;
				if (rageSafeZoneType != _lastRageSafeZoneType)
				{
					_lastRageSafeZoneType = rageSafeZoneType;
					animator.SetBool(_rageClosetHashBool, rageSafeZoneType == SafeZoneType.Closet);
					animator.SetBool(_rageTableHashBool, rageSafeZoneType == SafeZoneType.Table);
					animator.SetBool(_rageBaseHashBool, rageSafeZoneType == SafeZoneType.Base || rageSafeZoneType == SafeZoneType.None);
				}
				if (rageRandomIndex != _lastRageRandomIndex)
				{
					_lastRageRandomIndex = rageRandomIndex;
					animator.SetInteger(_randomRageIndexHash, rageRandomIndex);
				}
			}
			else
			{
				ClearRageSafeZoneBoolsIfNeeded(animator);
			}
		}

		private void ClearRageSafeZoneBoolsIfNeeded(Animator animator)
		{
			if (_lastRageSafeZoneType != SafeZoneType.None)
			{
				_lastRageSafeZoneType = SafeZoneType.None;
				animator.SetBool(_rageClosetHashBool, value: false);
				animator.SetBool(_rageTableHashBool, value: false);
				animator.SetBool(_rageBaseHashBool, value: false);
			}
		}

		private void SetTriggerForVisualState(Animator animator, HeadmanVisualState state)
		{
			switch (state)
			{
			case HeadmanVisualState.Wandering:
				animator.SetTrigger(_wanderingHashTrigger);
				break;
			case HeadmanVisualState.ChasingStart:
				animator.SetTrigger(_startChasingHashTrigger);
				break;
			case HeadmanVisualState.Chasing:
				animator.SetTrigger(_chasingHashTrigger);
				break;
			case HeadmanVisualState.AttackingBase:
				animator.SetTrigger(_baseAttackHashTrigger);
				break;
			case HeadmanVisualState.AttackingLow:
				animator.SetTrigger(_lowAttackHashTrigger);
				break;
			case HeadmanVisualState.Rage:
				animator.SetTrigger(_chasingHashTrigger);
				break;
			case HeadmanVisualState.RageInteracted:
				animator.SetTrigger(_rageToTargetHashTrigger);
				break;
			case HeadmanVisualState.Fear:
				animator.SetTrigger(_chasingHashTrigger);
				break;
			case HeadmanVisualState.None:
			case HeadmanVisualState.RageEnd:
			case HeadmanVisualState.Slowed:
			case HeadmanVisualState.Dead:
				break;
			}
		}
	}
}
