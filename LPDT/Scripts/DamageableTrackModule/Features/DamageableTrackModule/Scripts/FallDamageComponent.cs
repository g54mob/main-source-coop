using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.LevelModule.Scripts;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.DamageableTrackModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class FallDamageComponent : NetworkBehaviour
	{
		[SerializeField]
		private CharacterMovableBase _characterMovable;

		[SerializeField]
		private PlayerDamageable _playerDamageable;

		[SerializeField]
		private float _minDamageVelocity = 10f;

		[SerializeField]
		private float _maxDamageVelocity = 15f;

		[SerializeField]
		private float _maxDamage = 100f;

		[SerializeField]
		private float _minDamage = 5f;

		[SerializeField]
		private float _maxRagdollDuration = 3f;

		[SerializeField]
		private float _minRagdollDuration = 1f;

		[SerializeField]
		private float _groundedDelay = 5f;

		[SerializeField]
		private LayerMask _groundLayerMask;

		[SerializeField]
		private float _timerBorder = 0.5f;

		[SerializeField]
		private EventReference _onPlayerLandedEvent;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private EventReference _onPlayerOnLevelEvent;

		[SerializeField]
		private EventReference _onEndLevelEvent;

		private IPlayerStateService _playerStateService;

		private PlayerTransitedToLevelEvent _playerTransitedToLevelEvent;

		private MultiplayerModel _multiplayerModel;

		private PlayersRagdollModel _playersRagdollModel;

		private FallDamageGateModel _fallDamageGateModel;

		private IAudioService _audioService;

		private float _nonGroundedTimer;

		private float _groundedTimer;

		private float _fallSpeedThisStep;

		private float _fallSpeedPrevStep;

		private bool _isPlayerOnLevelSoundPlayed;

		private bool _isDriver;

		private FallDamageGateIntent _lastAppliedIntent;

		private Coroutine _armingCoroutine;

		private CancellationTokenSource _fallDamageRagdollCts;

		[Inject]
		private void InjectDependencies(IPlayerStateService playerStateService, PlayerTransitedToLevelEvent playerTransitedToLevelEvent, MultiplayerModel multiplayerModel, PlayersRagdollModel playersRagdollModel, FallDamageGateModel fallDamageGateModel, IAudioService audioService)
		{
			_playerStateService = playerStateService;
			_playerTransitedToLevelEvent = playerTransitedToLevelEvent;
			_multiplayerModel = multiplayerModel;
			_playersRagdollModel = playersRagdollModel;
			_fallDamageGateModel = fallDamageGateModel;
			_audioService = audioService;
		}

		public override void Spawned()
		{
			_isDriver = base.HasStateAuthority;
			if (_isDriver)
			{
				_fallDamageGateModel.OnIntentChanged += ApplyGateIntent;
				_lastAppliedIntent = _fallDamageGateModel.Intent;
				if (_lastAppliedIntent == FallDamageGateIntent.Arming)
				{
					BeginArming();
				}
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_isDriver)
			{
				_fallDamageGateModel.OnIntentChanged -= ApplyGateIntent;
			}
			StopArming();
			CancelFallDamageTimer();
		}

		private void FixedUpdate()
		{
			_fallSpeedPrevStep = _fallSpeedThisStep;
			_fallSpeedThisStep = 0f - _characterMovable.GetVelocity().y;
			if (_characterMovable.IsGrounded)
			{
				_groundedTimer += Time.fixedDeltaTime;
				if (_groundedTimer > 1f)
				{
					_nonGroundedTimer = 0f;
				}
			}
			else
			{
				_groundedTimer = 0f;
				_nonGroundedTimer += Time.fixedDeltaTime;
			}
		}

		private void ApplyGateIntent()
		{
			FallDamageGateIntent intent = _fallDamageGateModel.Intent;
			if (intent != _lastAppliedIntent)
			{
				_lastAppliedIntent = intent;
				if (intent == FallDamageGateIntent.Arming)
				{
					BeginArming();
				}
				else
				{
					HandleDisarmed();
				}
			}
		}

		private void BeginArming()
		{
			StopArming();
			_armingCoroutine = StartCoroutine(WaitForLandingThenArm());
		}

		private void StopArming()
		{
			if (_armingCoroutine != null)
			{
				StopCoroutine(_armingCoroutine);
				_armingCoroutine = null;
			}
		}

		private void HandleDisarmed()
		{
			StopArming();
			_isPlayerOnLevelSoundPlayed = false;
			_audioService.PlayOneShot(_onEndLevelEvent);
			Debug.Log($"[HealthTrace] FallDamage disarmed (Level transition) p{base.Object.InputAuthority.PlayerId} t={Time.time:0.##}");
		}

		private IEnumerator WaitForLandingThenArm()
		{
			while (!_characterMovable.IsGrounded)
			{
				yield return new WaitForFixedUpdate();
			}
			_playerTransitedToLevelEvent.Invoke(base.Object.InputAuthority.PlayerId);
			if (!_isPlayerOnLevelSoundPlayed)
			{
				_isPlayerOnLevelSoundPlayed = true;
				_audioService.PlayOneShot(_onPlayerOnLevelEvent);
			}
			yield return new WaitForSeconds(_groundedDelay);
			_fallDamageGateModel.SetArmed();
			_armingCoroutine = null;
			Debug.Log($"[HealthTrace] FallDamage armed (landed + {_groundedDelay}s) p{base.Object.InputAuthority.PlayerId}");
		}

		private void OnCollisionEnter(Collision other)
		{
			if (!base.HasStateAuthority || !_fallDamageGateModel.IsArmed || _nonGroundedTimer < _timerBorder || base.Object.InputAuthority.PlayerId != base.Runner.LocalPlayer.PlayerId || !IsGroundSurface(other.gameObject) || _playerStateService.IsPlayerStunned(base.Object.InputAuthority.PlayerId))
			{
				return;
			}
			float num = Mathf.Max(other.relativeVelocity.y, _fallSpeedPrevStep);
			float num2 = 0f;
			float duration = 1f;
			if (num > _minDamageVelocity)
			{
				float t = Mathf.Clamp01((num - _minDamageVelocity) / (_maxDamageVelocity - _minDamageVelocity));
				num2 = Mathf.Lerp(_minDamage, _maxDamage, t);
				duration = Mathf.Lerp(_minRagdollDuration, _maxRagdollDuration, t);
			}
			if (!(num2 <= 0f))
			{
				_nonGroundedTimer = 0f;
				Debug.Log($"[HealthTrace] FallDamage hit p{base.Object.InputAuthority.PlayerId} vel={num:0.#} dmg={num2:0.#} hitLayer={LayerMask.LayerToName(other.gameObject.layer)}");
				DamageData damageData = new DamageData
				{
					Damage = num2,
					Force = 0f,
					Direction = Vector3.zero,
					ForceMode = ForceMode.Impulse,
					IsStunning = false,
					Source = DamageDataSourceExtensions.ForEnvironment(DamageType.Fall)
				};
				_playerDamageable.Damage(damageData);
				_audioService.PlayOneShotAttached(_onPlayerLandedEvent, _soundSourceBehaviour);
				if (num2 > 0f && _playerDamageable.Health > 0f)
				{
					AddFallDamageRagdollTemporary(duration);
				}
			}
		}

		private bool IsGroundSurface(GameObject surface)
		{
			if ((_groundLayerMask.value & (1 << surface.layer)) != 0)
			{
				return true;
			}
			return surface.GetComponentInParent<IFallDamageSurface>() != null;
		}

		private void AddFallDamageRagdollTemporary(float duration)
		{
			if (_playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				CancelFallDamageTimer();
				ragdoll.AddSimulationReason(RagdollSimulationReasonEnum.FallDamage);
				_fallDamageRagdollCts = new CancellationTokenSource();
				WaitAndRemoveFallDamageReason(duration, ragdoll, _fallDamageRagdollCts.Token).Forget();
			}
		}

		private async UniTaskVoid WaitAndRemoveFallDamageReason(float duration, PlayerRagdollEntity ragdoll, CancellationToken ct)
		{
			try
			{
				await UniTask.WaitForSeconds(duration, ignoreTimeScale: false, PlayerLoopTiming.Update, ct);
				ragdoll.RemoveSimulationReason(RagdollSimulationReasonEnum.FallDamage);
			}
			catch (OperationCanceledException)
			{
			}
		}

		private void CancelFallDamageTimer()
		{
			_fallDamageRagdollCts?.Cancel();
			_fallDamageRagdollCts?.Dispose();
			_fallDamageRagdollCts = null;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
