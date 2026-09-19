using System;
using System.Collections.Generic;
using Features.PlayerSkinModule.Scripts.Features.PlayerSkinModule.Scripts.VisibilityHandling;

namespace Features.PlayerSkinModule.Scripts
{
	public class SkinModel
	{
		public Dictionary<int, VisibilityHandlerBase> AllCharacterVisibility = new Dictionary<int, VisibilityHandlerBase>();

		public Dictionary<int, VisibilityHandlerBase> BaseCharacterVisibility = new Dictionary<int, VisibilityHandlerBase>();

		public Dictionary<int, VisibilityHandlerBase> HatCharacterVisibility = new Dictionary<int, VisibilityHandlerBase>();

		public Dictionary<int, VisibilityHandlerBase> TorsoCharacterVisibility = new Dictionary<int, VisibilityHandlerBase>();

		public Dictionary<int, VisibilityHandlerBase> BottomCharacterVisibility = new Dictionary<int, VisibilityHandlerBase>();

		public Dictionary<int, SkinSpawner> SkinVisibility = new Dictionary<int, SkinSpawner>();

		public event Action<int> OnPlayerSkinRegistered;

		public event Action<int> OnPlayerSkinUnregistered;

		public void NotifyPlayerSkinRegistered(int playerId)
		{
			this.OnPlayerSkinRegistered?.Invoke(playerId);
		}

		public void UnregisterPlayer(int playerId)
		{
			AllCharacterVisibility.Remove(playerId);
			BaseCharacterVisibility.Remove(playerId);
			HatCharacterVisibility.Remove(playerId);
			TorsoCharacterVisibility.Remove(playerId);
			BottomCharacterVisibility.Remove(playerId);
			SkinVisibility.Remove(playerId);
			this.OnPlayerSkinUnregistered?.Invoke(playerId);
		}
	}
}
