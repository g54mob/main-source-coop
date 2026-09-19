using System;
using System.Collections.Generic;

namespace Features.HeadwearModule.Scripts
{
	public class HeadwearModel
	{
		private readonly Dictionary<int, Headwear> _worn = new Dictionary<int, Headwear>();

		public IReadOnlyDictionary<int, Headwear> Worn => _worn;

		public event Action<int, bool> OnHeadwearWornChanged;

		public bool TryGetHeadwear(int playerId, out Headwear headwear)
		{
			return _worn.TryGetValue(playerId, out headwear);
		}

		public void SetHeadwear(int playerId, bool isEquipped, Headwear headwear = null)
		{
			if (isEquipped)
			{
				_worn[playerId] = headwear;
			}
			else
			{
				if (headwear != null && _worn.TryGetValue(playerId, out var value) && value != null && value != headwear)
				{
					return;
				}
				_worn.Remove(playerId);
			}
			this.OnHeadwearWornChanged?.Invoke(playerId, isEquipped);
		}
	}
}
