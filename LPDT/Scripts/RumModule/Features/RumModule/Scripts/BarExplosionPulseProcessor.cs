using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.RagdollModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Fusion;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using Zenject;

namespace Features.RumModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class BarExplosionPulseProcessor : NetworkBehaviour
	{
		private static readonly Collider[] OverlapBuffer = new Collider[32];

		[SerializeField]
		private float _radius = 5f;

		[SerializeField]
		private float _force = 150f;

		[SerializeField]
		private float _upBias = 0.6f;

		[SerializeField]
		private LayerMask _hitMask = -1;

		[SerializeField]
		private NetworkObject _particleSystem;

		[SerializeField]
		private EventReference _explosionSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private IAudioService _audioService;

		private ILocalPlayerThrowService _localPlayerThrowService;

		private IStat _pulseStat;

		private float _previousValue;

		private bool _firedThisPulse;

		[Inject]
		private void InjectDependencies(SpawnedEntityStatsModel spawnedEntityStatsModel, IAudioService audioService, ILocalPlayerThrowService localPlayerThrowService)
		{
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_audioService = audioService;
			_localPlayerThrowService = localPlayerThrowService;
		}

		public override void Spawned()
		{
			if (_spawnedEntityStatsModel != null)
			{
				_spawnedEntityStatsModel.OnPlayerStatRegistered += OnPlayerStatRegistered;
			}
			TryBindStat();
			_previousValue = ((_pulseStat != null) ? _pulseStat.FullValue : 0f);
			_firedThisPulse = _previousValue > 0f;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_spawnedEntityStatsModel != null)
			{
				_spawnedEntityStatsModel.OnPlayerStatRegistered -= OnPlayerStatRegistered;
			}
			UnbindStat();
			base.Despawned(runner, hasState);
		}

		private void OnPlayerStatRegistered(int playerId)
		{
			if (!(base.Object == null) && base.Object.IsValid && playerId == base.Object.InputAuthority.PlayerId)
			{
				TryBindStat();
			}
		}

		private void TryBindStat()
		{
			if (!(base.Object == null) && base.Object.IsValid && _spawnedEntityStatsModel != null && _spawnedEntityStatsModel.PlayerStats.TryGetValue(base.Object.InputAuthority.PlayerId, out var value))
			{
				UnbindStat();
				_pulseStat = value.GetStat(EntityStatType.BarExplosionPulse);
				if (_pulseStat != null)
				{
					_pulseStat.OnFullValueChanged += OnPulseStatChanged;
				}
			}
		}

		private void UnbindStat()
		{
			if (_pulseStat != null)
			{
				_pulseStat.OnFullValueChanged -= OnPulseStatChanged;
			}
			_pulseStat = null;
		}

		private void OnPulseStatChanged(float _)
		{
			if (_pulseStat != null)
			{
				float fullValue = _pulseStat.FullValue;
				bool num = _previousValue <= 0f && fullValue > 0f;
				if (fullValue <= 0f)
				{
					_firedThisPulse = false;
				}
				_previousValue = fullValue;
				if (num && !_firedThisPulse)
				{
					_firedThisPulse = true;
					PlayExplosionPresentation();
					TryKnockLocalPlayer();
				}
			}
		}

		private void PlayExplosionPresentation()
		{
			if (_audioService != null && !_explosionSound.IsNull && _soundSourceBehaviour != null)
			{
				_audioService.PlayOneShotAttached(_explosionSound, _soundSourceBehaviour);
			}
			if (base.Object.HasStateAuthority && !(_particleSystem == null) && !(base.Runner == null))
			{
				base.Runner.Spawn(_particleSystem, base.transform.position, base.transform.rotation);
			}
		}

		private void TryKnockLocalPlayer()
		{
			if (base.Runner == null || !base.Runner.IsRunning || _localPlayerThrowService == null)
			{
				return;
			}
			PlayerRef localPlayer = base.Runner.LocalPlayer;
			if (localPlayer == PlayerRef.None)
			{
				return;
			}
			Vector3 position = base.transform.position;
			int num = Physics.OverlapSphereNonAlloc(position, _radius, OverlapBuffer, _hitMask, QueryTriggerInteraction.Ignore);
			PlayerDamageable playerDamageable = null;
			Vector3 vector = position;
			for (int i = 0; i < num; i++)
			{
				Collider collider = OverlapBuffer[i];
				if (!(collider == null))
				{
					PlayerDamageable componentInParent = collider.GetComponentInParent<PlayerDamageable>();
					if (!(componentInParent == null) && !(componentInParent.Object == null) && componentInParent.Object.IsValid && !(componentInParent.Object.InputAuthority != localPlayer))
					{
						playerDamageable = componentInParent;
						vector = collider.ClosestPoint(position);
						break;
					}
				}
			}
			if (playerDamageable == null)
			{
				return;
			}
			float num2 = Mathf.Clamp01(Vector3.Distance(position, vector) / Mathf.Max(0.0001f, _radius));
			float num3 = 1f - num2;
			float num4 = _force * num3;
			if (!(num4 <= 0.01f))
			{
				Vector3 vector2 = vector - position;
				vector2.y = 0f;
				if (vector2.sqrMagnitude < 0.0001f)
				{
					vector2 = base.transform.forward;
				}
				Vector3 normalized = (vector2.normalized + Vector3.up * _upBias).normalized;
				_localPlayerThrowService.EnterThrow();
				playerDamageable.AddRPCForce(num4, normalized, ForceMode.Impulse);
			}
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
