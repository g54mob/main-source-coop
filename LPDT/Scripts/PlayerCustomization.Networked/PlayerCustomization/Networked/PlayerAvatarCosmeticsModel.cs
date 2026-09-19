using System;
using System.Collections.Generic;

namespace PlayerCustomization.Networked
{
	public class PlayerAvatarCosmeticsModel
	{
		private readonly Dictionary<int, PlayerAvatarCosmetics> _byPlayerId = new Dictionary<int, PlayerAvatarCosmetics>();

		public PlayerAvatarCosmetics LocalAvatarCosmetics { get; private set; }

		public event Action<int> OnAvatarRegistered;

		public void Register(int playerId, PlayerAvatarCosmetics cosmetics, bool isLocal)
		{
			_byPlayerId[playerId] = cosmetics;
			if (isLocal)
			{
				LocalAvatarCosmetics = cosmetics;
			}
			this.OnAvatarRegistered?.Invoke(playerId);
		}

		public void Unregister(int playerId)
		{
			if (_byPlayerId.TryGetValue(playerId, out var value) && value == LocalAvatarCosmetics)
			{
				LocalAvatarCosmetics = null;
			}
			_byPlayerId.Remove(playerId);
		}

		public bool TryGet(int playerId, out PlayerAvatarCosmetics cosmetics)
		{
			return _byPlayerId.TryGetValue(playerId, out cosmetics);
		}
	}
}
