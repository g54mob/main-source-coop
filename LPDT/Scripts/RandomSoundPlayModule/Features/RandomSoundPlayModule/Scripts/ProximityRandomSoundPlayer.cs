using System.Collections.Generic;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.RandomSoundPlayModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public sealed class ProximityRandomSoundPlayer : RandomSoundPlayerBase
	{
		[SerializeField]
		private Transform _soundPoint;

		[SerializeField]
		[Min(0.1f)]
		private float _proximityDistance = 2.5f;

		[SerializeField]
		[Min(0.05f)]
		private float _checkInterval = 0.25f;

		[SerializeField]
		[Min(0f)]
		private float _rotationThreshold = 5f;

		[SerializeField]
		[Min(0.01f)]
		private float _rotationExceedFadeOutDuration = 0.25f;

		private PlayerMovableModel _playerMovableModel;

		private Quaternion _spawnRotation;

		private float _nextCheckTime;

		private float _proximityDistanceSqr;

		[Inject]
		private void InjectDependencies(PlayerMovableModel playerMovableModel)
		{
			_playerMovableModel = playerMovableModel;
		}

		private void OnValidate()
		{
			UpdateProximityDistanceSqr();
		}

		public override void Spawned()
		{
			base.Spawned();
			_spawnRotation = base.transform.rotation;
			UpdateProximityDistanceSqr();
		}

		protected override void Update()
		{
			base.Update();
			TryFadeOutIfRotatedPastThreshold();
			if (CanTryPlay() && _playerMovableModel != null && !(Time.time < _nextCheckTime))
			{
				_nextCheckTime = Time.time + _checkInterval;
				if (IsRotationWithinThreshold() && TryGetNearestPlayerWithinProximity(out var playerRef, out var _))
				{
					TryPlayRandomSound(_soundPoint.position, playerRef.PlayerId);
				}
			}
		}

		private void TryFadeOutIfRotatedPastThreshold()
		{
			if ((bool)base.Object && base.Object.IsValid && base.HasStateAuthority && base.HasActiveSoundInstance && !base.IsFadingSound && !IsRotationWithinThreshold())
			{
				FadeOutPlayingSound(_rotationExceedFadeOutDuration);
			}
		}

		private bool IsRotationWithinThreshold()
		{
			return Quaternion.Angle(_spawnRotation, base.transform.rotation) <= _rotationThreshold;
		}

		private bool TryGetNearestPlayerWithinProximity(out PlayerRef playerRef, out PlayerCharacterMovableBase nearestMovable)
		{
			playerRef = PlayerRef.None;
			nearestMovable = null;
			float num = float.MaxValue;
			foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
			{
				PlayerCharacterMovableBase value = allCharacterMovable.Value;
				if (!(value == null))
				{
					float sqrMagnitude = (value.transform.position - base.transform.position).sqrMagnitude;
					if (!(sqrMagnitude > _proximityDistanceSqr) && !(sqrMagnitude >= num))
					{
						num = sqrMagnitude;
						playerRef = allCharacterMovable.Key;
						nearestMovable = value;
					}
				}
			}
			return nearestMovable != null;
		}

		private void UpdateProximityDistanceSqr()
		{
			_proximityDistanceSqr = _proximityDistance * _proximityDistance;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
