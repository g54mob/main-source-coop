using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;

namespace Features.PlayerPresenceModule.Networked
{
	public sealed class SessionPlayerDeadPartStore
	{
		private readonly MultiplayerModel _multiplayerModel;

		public SessionPlayerDeadPartStore(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public bool TryGetBottomPart(int playerId, out BottomPartSnapshot snapshot)
		{
			snapshot = default(BottomPartSnapshot);
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null)
			{
				return false;
			}
			if (!SessionPlayerObjectRegistry.TryGetByStateAuthority(networkRunner, playerId, out var result))
			{
				return false;
			}
			snapshot = new BottomPartSnapshot
			{
				HasValue = (result.DeadPartTypeId != 0),
				TypeId = result.DeadPartTypeId,
				Color = Unpack(result.BottomPartColor),
				SkinId = result.BottomPartSkinId,
				TexturePresetId = result.BottomPartTexturePreset,
				MeshPresetId = result.BottomPartMeshPreset,
				UsageCount = result.BottomPartUsageCount
			};
			return true;
		}

		public bool TryGetCosmeticSeed(int playerId, out int seed)
		{
			seed = 0;
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null)
			{
				return false;
			}
			if (!SessionPlayerObjectRegistry.TryGetByStateAuthority(networkRunner, playerId, out var result))
			{
				return false;
			}
			seed = result.CosmeticSeed;
			return seed != 0;
		}

		public bool TryWriteLocalBottomPart(int typeId, Color color, int skinId, int texturePreset, int meshPreset, int usageCount)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null)
			{
				return false;
			}
			int playerId = networkRunner.LocalPlayer.PlayerId;
			if (!SessionPlayerObjectRegistry.TryGetLocalAuthority(networkRunner, playerId, out var result))
			{
				return false;
			}
			return result.TryWriteBottomPart(typeId, Pack(color), skinId, texturePreset, meshPreset, usageCount);
		}

		private static int Pack(Color color)
		{
			Color32 color2 = color;
			return (color2.r << 24) | (color2.g << 16) | (color2.b << 8) | color2.a;
		}

		private static Color Unpack(int packed)
		{
			byte r = (byte)((packed >> 24) & 0xFF);
			byte g = (byte)((packed >> 16) & 0xFF);
			byte b = (byte)((packed >> 8) & 0xFF);
			byte a = (byte)(packed & 0xFF);
			return new Color32(r, g, b, a);
		}
	}
}
