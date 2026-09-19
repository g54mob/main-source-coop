using System;
using System.Collections.Generic;
using Features.DamageableTrackModule.Scripts;
using Zenject;

namespace Features.HeadwearModule.Scripts
{
	public class HeadwearDamageDropSystem : IInitializable, IDisposable
	{
		private readonly HeadwearModel _headwearModel;

		private readonly PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private readonly Dictionary<int, Action<DamageData>> _handlers = new Dictionary<int, Action<DamageData>>();

		public HeadwearDamageDropSystem(HeadwearModel headwearModel, PlayerDamageablesTrackModel playerDamageablesTrackModel)
		{
			_headwearModel = headwearModel;
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
		}

		public void Initialize()
		{
			_headwearModel.OnHeadwearWornChanged += HandleHeadwearWornChanged;
		}

		public void Dispose()
		{
			_headwearModel.OnHeadwearWornChanged -= HandleHeadwearWornChanged;
			foreach (KeyValuePair<int, Action<DamageData>> handler in _handlers)
			{
				DetachHandler(handler.Key, handler.Value);
			}
			_handlers.Clear();
		}

		private void HandleHeadwearWornChanged(int playerId, bool isWorn)
		{
			if (isWorn)
			{
				Attach(playerId);
			}
			else
			{
				Detach(playerId);
			}
		}

		private void Attach(int playerId)
		{
			if (!_handlers.ContainsKey(playerId) && _playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(playerId, out var value))
			{
				Action<DamageData> value2 = delegate(DamageData damageData)
				{
					HandleDamaged(playerId, damageData);
				};
				_handlers[playerId] = value2;
				value.OnDamaged += value2;
			}
		}

		private void Detach(int playerId)
		{
			if (_handlers.TryGetValue(playerId, out var value))
			{
				DetachHandler(playerId, value);
				_handlers.Remove(playerId);
			}
		}

		private void DetachHandler(int playerId, Action<DamageData> handler)
		{
			if (_playerDamageablesTrackModel.AllPlayerDamageables.TryGetValue(playerId, out var value))
			{
				value.OnDamaged -= handler;
			}
		}

		private void HandleDamaged(int playerId, DamageData damageData)
		{
			if (damageData != null && damageData.Source.HasValue)
			{
				DamageSource value = damageData.Source.Value;
				if ((value.Category == DamageCauseCategory.Enemy || value.Type == DamageType.Projectile) && _headwearModel.TryGetHeadwear(playerId, out var headwear) && !(headwear == null) && headwear.IsWorn)
				{
					headwear.DropFromDamage(damageData.Direction);
				}
			}
		}
	}
}
