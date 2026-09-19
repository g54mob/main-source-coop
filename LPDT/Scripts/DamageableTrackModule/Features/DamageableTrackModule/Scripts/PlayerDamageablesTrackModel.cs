using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;

namespace Features.DamageableTrackModule.Scripts
{
	public class PlayerDamageablesTrackModel : ISessionCleanup
	{
		private readonly Dictionary<int, IDamageable> _allPlayerDamageables = new Dictionary<int, IDamageable>();

		public IReadOnlyDictionary<int, IDamageable> AllPlayerDamageables => _allPlayerDamageables;

		public event Action OnPlayerDamageablesChanged;

		public event Action<int, IDamageable> OnPlayerDamageableAdded;

		public event Action<int> OnPlayerDamageableRemoved;

		public void AddTrackedDamageable(int playerId, IDamageable damageable)
		{
			if (damageable == null)
			{
				throw new NullReferenceException();
			}
			if (!_allPlayerDamageables.TryAdd(playerId, damageable))
			{
				_allPlayerDamageables[playerId] = damageable;
			}
			this.OnPlayerDamageableAdded?.Invoke(playerId, damageable);
			this.OnPlayerDamageablesChanged?.Invoke();
		}

		public void RemoveTrackedDamageable(int playerId)
		{
			_allPlayerDamageables.Remove(playerId);
			this.OnPlayerDamageableRemoved?.Invoke(playerId);
			this.OnPlayerDamageablesChanged?.Invoke();
		}

		public void Cleanup()
		{
			_allPlayerDamageables.Clear();
			this.OnPlayerDamageablesChanged = null;
			this.OnPlayerDamageableAdded = null;
			this.OnPlayerDamageableRemoved = null;
		}
	}
}
