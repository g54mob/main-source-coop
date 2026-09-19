using System.Collections.Generic;

namespace Features.DamageableTrackModule.Scripts
{
	public class PlayerDamageAbsorbersModel
	{
		private readonly Dictionary<int, IDamageAbsorber> _absorbers = new Dictionary<int, IDamageAbsorber>();

		public void Set(int playerId, IDamageAbsorber absorber)
		{
			_absorbers[playerId] = absorber;
		}

		public void Clear(int playerId, IDamageAbsorber absorber)
		{
			if (_absorbers.TryGetValue(playerId, out var value) && value == absorber)
			{
				_absorbers.Remove(playerId);
			}
		}

		public bool TryAbsorb(int playerId, DamageData damageData)
		{
			if (_absorbers.TryGetValue(playerId, out var value) && value != null)
			{
				return value.TryAbsorb(damageData);
			}
			return false;
		}
	}
}
