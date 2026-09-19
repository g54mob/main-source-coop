using System;
using System.Collections.Generic;
using Features.SkinConfiguration.Scripts;

namespace PlayerCustomization.Networked
{
	public class SessionRewardModel
	{
		private SessionRewardStore _store;

		public bool HasStore => _store != null;

		public SessionRewardStore Store => _store;

		public event Action OnRewardsChanged;

		public void RegisterStore(SessionRewardStore store)
		{
			_store = store;
			this.OnRewardsChanged?.Invoke();
		}

		public void UnregisterStore(SessionRewardStore store)
		{
			if (_store == store)
			{
				_store = null;
			}
		}

		public void NotifyRewardsChanged()
		{
			this.OnRewardsChanged?.Invoke();
		}

		public IEnumerable<RewardSlot> GetRewards(string persistentId)
		{
			if (_store == null)
			{
				yield break;
			}
			foreach (RewardSlot item in _store.EnumerateRewards())
			{
				if ((bool)item.Occupied && item.PersistentId.Value == persistentId)
				{
					yield return item;
				}
			}
		}

		public bool TryGrant(string persistentId, CosmeticRewardPart part, SkinType skinId)
		{
			if (_store != null)
			{
				return _store.Grant(persistentId, part, skinId);
			}
			return false;
		}
	}
}
