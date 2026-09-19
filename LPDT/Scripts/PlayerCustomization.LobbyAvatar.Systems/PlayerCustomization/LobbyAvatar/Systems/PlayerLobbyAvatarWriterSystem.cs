using System;
using Features.DeadPartsModule.Data;
using Features.PlayerIdentityModule;
using Features.SkinConfiguration.Scripts;
using PlayerCustomization.Data;
using PlayerCustomization.LobbyAvatar.Data;
using UnityEngine;
using Zenject;

namespace PlayerCustomization.LobbyAvatar.Systems
{
	public class PlayerLobbyAvatarWriterSystem : IInitializable, IDisposable
	{
		private readonly PlayerLobbyAvatarModel _playerLobbyAvatarModel;

		private readonly PlayerProfileModel _playerProfileModel;

		private readonly IPersistentPlayerIdProvider _persistentPlayerIdProvider;

		private readonly SkinsConfiguration _skinsConfiguration;

		public PlayerLobbyAvatarWriterSystem(PlayerLobbyAvatarModel playerLobbyAvatarModel, PlayerProfileModel playerProfileModel, IPersistentPlayerIdProvider persistentPlayerIdProvider, SkinsConfiguration skinsConfiguration)
		{
			_playerLobbyAvatarModel = playerLobbyAvatarModel;
			_playerProfileModel = playerProfileModel;
			_persistentPlayerIdProvider = persistentPlayerIdProvider;
			_skinsConfiguration = skinsConfiguration;
		}

		public void Initialize()
		{
			_playerLobbyAvatarModel.AttachmentChanged += OnAttachmentChanged;
			_playerProfileModel.OnPlayerNameChanged += OnProfileNameChanged;
			_playerProfileModel.OnPlayerColorChanged += OnProfileColorChanged;
		}

		public void Dispose()
		{
			_playerLobbyAvatarModel.AttachmentChanged -= OnAttachmentChanged;
			_playerProfileModel.OnPlayerNameChanged -= OnProfileNameChanged;
			_playerProfileModel.OnPlayerColorChanged -= OnProfileColorChanged;
		}

		private void OnAttachmentChanged(bool isAttached)
		{
			if (isAttached)
			{
				WriteIfOwned();
			}
		}

		private void OnProfileNameChanged(string name)
		{
			WriteIfOwned();
		}

		private void OnProfileColorChanged(Color color)
		{
			WriteIfOwned();
		}

		private void WriteIfOwned()
		{
			if (_playerLobbyAvatarModel.IsAttached && _playerLobbyAvatarModel.IsAuthority)
			{
				SkinType value = StarterSkin();
				int value2 = LobbyAvatarColor.Pack(_playerProfileModel.PlayerColor);
				_playerLobbyAvatarModel.Nickname.Value = _playerProfileModel.PlayerName;
				_playerLobbyAvatarModel.PrimaryColor.Value = value2;
				_playerLobbyAvatarModel.VariableColor.Value = value2;
				_playerLobbyAvatarModel.HatPartSkinId.Value = value;
				_playerLobbyAvatarModel.TorsoPartSkinId.Value = value;
				_playerLobbyAvatarModel.BottomPartSkinId.Value = value;
				_playerLobbyAvatarModel.IsFullSkin.Value = true;
				_playerLobbyAvatarModel.ButtTexturePreset.Value = ButtTexturePreset.Default;
				_playerLobbyAvatarModel.ButtMeshPreset.Value = ButtMeshPreset.Default;
			}
		}

		private SkinType StarterSkin()
		{
			int count = _skinsConfiguration.StartedPossibleSkinTypes.Count;
			int index = (StableHash(_persistentPlayerIdProvider.LocalId.Value) & 0x7FFFFFFF) % count;
			return _skinsConfiguration.StartedPossibleSkinTypes[index];
		}

		private static int StableHash(string value)
		{
			int num = 17;
			foreach (char c in value)
			{
				num = num * 31 + c;
			}
			return num;
		}
	}
}
