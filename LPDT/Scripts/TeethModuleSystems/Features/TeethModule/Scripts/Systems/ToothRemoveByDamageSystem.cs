using System;
using System.Collections.Generic;
using Features.DamageableTrackModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.TeethModule.Scripts.Tooth;
using UnityEngine;
using Zenject;

namespace Features.TeethModule.Scripts.Systems
{
	public class ToothRemoveByDamageSystem : IInitializable, IDisposable
	{
		private readonly PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ICharacterTeethService _characterTeethService;

		private readonly ToothRemoveByDamageConfiguration _toothRemoveByDamageConfiguration;

		private IDamageable _localDamageable;

		public ToothRemoveByDamageSystem(PlayerDamageablesTrackModel playerDamageablesTrackModel, MultiplayerModel multiplayerModel, ICharacterTeethService characterTeethService, ToothRemoveByDamageConfiguration toothRemoveByDamageConfiguration)
		{
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
			_multiplayerModel = multiplayerModel;
			_characterTeethService = characterTeethService;
			_toothRemoveByDamageConfiguration = toothRemoveByDamageConfiguration;
		}

		public void Initialize()
		{
			_playerDamageablesTrackModel.OnPlayerDamageableAdded += ProcessPlayerDamageableAdded;
			InitLocalPlayerDamageable();
		}

		public void Dispose()
		{
			_playerDamageablesTrackModel.OnPlayerDamageableAdded -= ProcessPlayerDamageableAdded;
			RemoveListerLocalDamageable();
		}

		private void ProcessPlayerDamageableAdded(int owner, IDamageable damageable)
		{
			if (_localDamageable == null && owner == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				_localDamageable = damageable;
				StartListerLocalDamageable();
			}
		}

		private void InitLocalPlayerDamageable()
		{
			foreach (KeyValuePair<int, IDamageable> allPlayerDamageable in _playerDamageablesTrackModel.AllPlayerDamageables)
			{
				if (allPlayerDamageable.Key == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
				{
					_localDamageable = allPlayerDamageable.Value;
					StartListerLocalDamageable();
				}
			}
		}

		private void StartListerLocalDamageable()
		{
			_localDamageable.OnDamaged += OnLocalPlayerTakeDamage;
		}

		private void RemoveListerLocalDamageable()
		{
			if (_localDamageable != null)
			{
				_localDamageable.OnDamaged -= OnLocalPlayerTakeDamage;
			}
		}

		private void OnLocalPlayerTakeDamage(DamageData damage)
		{
			if (!(damage.Damage <= _toothRemoveByDamageConfiguration.MinToothAffectDamage))
			{
				int value = Mathf.RoundToInt(damage.Damage / _toothRemoveByDamageConfiguration.TeethRemoveDamage);
				value = Mathf.Clamp(value, 1, _toothRemoveByDamageConfiguration.MaxToothAffectCount);
				RemoveTooth(value, spawnObject: true);
			}
		}

		private void RemoveTooth(int toothCount, bool spawnObject)
		{
			_characterTeethService.RemoveRandomTooth(toothCount, spawnObject);
		}
	}
}
