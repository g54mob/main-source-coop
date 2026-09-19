using Features.MultiplayerSessionServices.Scripts;
using Features.SkinConfiguration.Scripts;
using PlayerCustomization.Networked;
using UnityEngine;

namespace Features.SkinChangeModule.Scripts
{
	public class SkinChangeService
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly SkinsConfiguration _skinsConfiguration;

		private readonly PlayerAvatarCosmeticsModel _playerAvatarCosmeticsModel;

		private readonly SessionRewardModel _sessionRewardModel;

		public SkinChangeService(MultiplayerModel multiplayerModel, SkinsConfiguration skinsConfiguration, PlayerAvatarCosmeticsModel playerAvatarCosmeticsModel, SessionRewardModel sessionRewardModel)
		{
			_multiplayerModel = multiplayerModel;
			_skinsConfiguration = skinsConfiguration;
			_playerAvatarCosmeticsModel = playerAvatarCosmeticsModel;
			_sessionRewardModel = sessionRewardModel;
		}

		public bool ChangeLocalSkinPart(SkinPartType skinPartType, SkinType skinId)
		{
			if (_multiplayerModel.NetworkRunner == null)
			{
				return false;
			}
			return ChangeSkinPart(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, skinPartType, skinId);
		}

		public bool ChangeSkinPart(int playerId, SkinPartType skinPartType, SkinType skinId)
		{
			if (!_skinsConfiguration.PlayerSkins.ContainsKey(skinId))
			{
				Debug.LogError($"[SkinChangeService] unknown skin id {skinId} for player {playerId} — grant skipped.");
				return false;
			}
			if (!_playerAvatarCosmeticsModel.TryGet(playerId, out var cosmetics))
			{
				Debug.LogError($"[SkinChangeService] no avatar cosmetics for player {playerId} — grant skipped.");
				return false;
			}
			CosmeticRewardPart cosmeticRewardPart = ToRewardPart(skinPartType);
			if (cosmeticRewardPart == CosmeticRewardPart.None)
			{
				return false;
			}
			string value = cosmetics.StablePlayerId.Value;
			if (string.IsNullOrEmpty(value))
			{
				Debug.LogError($"[SkinChangeService] avatar for player {playerId} has empty StablePlayerId — grant skipped.");
				return false;
			}
			if (!_sessionRewardModel.HasStore)
			{
				Debug.LogError($"[SkinChangeService] SessionRewardStore not registered yet — grant for player {playerId} skipped.");
				return false;
			}
			return _sessionRewardModel.TryGrant(value, cosmeticRewardPart, skinId);
		}

		private static CosmeticRewardPart ToRewardPart(SkinPartType skinPartType)
		{
			return skinPartType switch
			{
				SkinPartType.Hat => CosmeticRewardPart.Hat, 
				SkinPartType.Torso => CosmeticRewardPart.Torso, 
				SkinPartType.Bottom => CosmeticRewardPart.Bottom, 
				_ => CosmeticRewardPart.None, 
			};
		}
	}
}
