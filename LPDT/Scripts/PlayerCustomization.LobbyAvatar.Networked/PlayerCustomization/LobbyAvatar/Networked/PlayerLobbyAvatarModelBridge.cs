using System;
using Features.DeadPartsModule.Data;
using Features.NetworkedModelRuntime;
using Features.SkinConfiguration.Scripts;
using Fusion;
using PlayerCustomization.LobbyAvatar.Data;
using UnityEngine;

namespace PlayerCustomization.LobbyAvatar.Networked
{
	public class PlayerLobbyAvatarModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly PlayerLobbyAvatarModel _playerLobbyAvatarModel;

		private PlayerLobbyAvatarNetworkObject _playerLobbyAvatarNetworkObject;

		private bool _hasPendingNickname;

		private NetworkString<_32> _pendingNickname;

		private bool _hasPendingPrimaryColor;

		private int _pendingPrimaryColor;

		private bool _hasPendingVariableColor;

		private int _pendingVariableColor;

		private bool _hasPendingHatPartSkinId;

		private SkinType _pendingHatPartSkinId;

		private bool _hasPendingTorsoPartSkinId;

		private SkinType _pendingTorsoPartSkinId;

		private bool _hasPendingBottomPartSkinId;

		private SkinType _pendingBottomPartSkinId;

		private bool _hasPendingIsFullSkin;

		private bool _pendingIsFullSkin;

		private bool _hasPendingButtTexturePreset;

		private ButtTexturePreset _pendingButtTexturePreset;

		private bool _hasPendingButtMeshPreset;

		private ButtMeshPreset _pendingButtMeshPreset;

		public PlayerLobbyAvatarModelBridge(PlayerLobbyAvatarModel playerLobbyAvatarModel)
		{
			_playerLobbyAvatarModel = playerLobbyAvatarModel;
		}

		public void Bind(PlayerLobbyAvatarNetworkObject playerLobbyAvatarNetworkObject)
		{
			Unbind();
			_playerLobbyAvatarNetworkObject = playerLobbyAvatarNetworkObject;
			_playerLobbyAvatarNetworkObject.OnNetworkedNicknameChanged += HandleNetworkedNicknameChanged;
			_playerLobbyAvatarNetworkObject.OnNetworkedPrimaryColorChanged += HandleNetworkedPrimaryColorChanged;
			_playerLobbyAvatarNetworkObject.OnNetworkedVariableColorChanged += HandleNetworkedVariableColorChanged;
			_playerLobbyAvatarNetworkObject.OnNetworkedHatPartSkinIdChanged += HandleNetworkedHatPartSkinIdChanged;
			_playerLobbyAvatarNetworkObject.OnNetworkedTorsoPartSkinIdChanged += HandleNetworkedTorsoPartSkinIdChanged;
			_playerLobbyAvatarNetworkObject.OnNetworkedBottomPartSkinIdChanged += HandleNetworkedBottomPartSkinIdChanged;
			_playerLobbyAvatarNetworkObject.OnNetworkedIsFullSkinChanged += HandleNetworkedIsFullSkinChanged;
			_playerLobbyAvatarNetworkObject.OnNetworkedButtTexturePresetChanged += HandleNetworkedButtTexturePresetChanged;
			_playerLobbyAvatarNetworkObject.OnNetworkedButtMeshPresetChanged += HandleNetworkedButtMeshPresetChanged;
			_playerLobbyAvatarNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_playerLobbyAvatarNetworkObject.OnDespawned += HandleDespawned;
			_playerLobbyAvatarModel.Nickname.BindWriter(WriteNickname);
			_playerLobbyAvatarModel.PrimaryColor.BindWriter(WritePrimaryColor);
			_playerLobbyAvatarModel.VariableColor.BindWriter(WriteVariableColor);
			_playerLobbyAvatarModel.HatPartSkinId.BindWriter(WriteHatPartSkinId);
			_playerLobbyAvatarModel.TorsoPartSkinId.BindWriter(WriteTorsoPartSkinId);
			_playerLobbyAvatarModel.BottomPartSkinId.BindWriter(WriteBottomPartSkinId);
			_playerLobbyAvatarModel.IsFullSkin.BindWriter(WriteIsFullSkin);
			_playerLobbyAvatarModel.ButtTexturePreset.BindWriter(WriteButtTexturePreset);
			_playerLobbyAvatarModel.ButtMeshPreset.BindWriter(WriteButtMeshPreset);
			ApplyNicknameFromNetwork(_playerLobbyAvatarNetworkObject.Nickname);
			ApplyPrimaryColorFromNetwork(_playerLobbyAvatarNetworkObject.PrimaryColor);
			ApplyVariableColorFromNetwork(_playerLobbyAvatarNetworkObject.VariableColor);
			ApplyHatPartSkinIdFromNetwork(_playerLobbyAvatarNetworkObject.HatPartSkinId);
			ApplyTorsoPartSkinIdFromNetwork(_playerLobbyAvatarNetworkObject.TorsoPartSkinId);
			ApplyBottomPartSkinIdFromNetwork(_playerLobbyAvatarNetworkObject.BottomPartSkinId);
			ApplyIsFullSkinFromNetwork(_playerLobbyAvatarNetworkObject.IsFullSkin);
			ApplyButtTexturePresetFromNetwork(_playerLobbyAvatarNetworkObject.ButtTexturePreset);
			ApplyButtMeshPresetFromNetwork(_playerLobbyAvatarNetworkObject.ButtMeshPreset);
			_playerLobbyAvatarModel.SetAuthorityProvider(() => _playerLobbyAvatarNetworkObject.HasStateAuthority);
			_playerLobbyAvatarModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_playerLobbyAvatarNetworkObject == null))
			{
				_playerLobbyAvatarNetworkObject.OnNetworkedNicknameChanged -= HandleNetworkedNicknameChanged;
				_playerLobbyAvatarNetworkObject.OnNetworkedPrimaryColorChanged -= HandleNetworkedPrimaryColorChanged;
				_playerLobbyAvatarNetworkObject.OnNetworkedVariableColorChanged -= HandleNetworkedVariableColorChanged;
				_playerLobbyAvatarNetworkObject.OnNetworkedHatPartSkinIdChanged -= HandleNetworkedHatPartSkinIdChanged;
				_playerLobbyAvatarNetworkObject.OnNetworkedTorsoPartSkinIdChanged -= HandleNetworkedTorsoPartSkinIdChanged;
				_playerLobbyAvatarNetworkObject.OnNetworkedBottomPartSkinIdChanged -= HandleNetworkedBottomPartSkinIdChanged;
				_playerLobbyAvatarNetworkObject.OnNetworkedIsFullSkinChanged -= HandleNetworkedIsFullSkinChanged;
				_playerLobbyAvatarNetworkObject.OnNetworkedButtTexturePresetChanged -= HandleNetworkedButtTexturePresetChanged;
				_playerLobbyAvatarNetworkObject.OnNetworkedButtMeshPresetChanged -= HandleNetworkedButtMeshPresetChanged;
				_playerLobbyAvatarNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_playerLobbyAvatarNetworkObject.OnDespawned -= HandleDespawned;
				_playerLobbyAvatarModel.Nickname.BindWriter(null);
				_playerLobbyAvatarModel.PrimaryColor.BindWriter(null);
				_playerLobbyAvatarModel.VariableColor.BindWriter(null);
				_playerLobbyAvatarModel.HatPartSkinId.BindWriter(null);
				_playerLobbyAvatarModel.TorsoPartSkinId.BindWriter(null);
				_playerLobbyAvatarModel.BottomPartSkinId.BindWriter(null);
				_playerLobbyAvatarModel.IsFullSkin.BindWriter(null);
				_playerLobbyAvatarModel.ButtTexturePreset.BindWriter(null);
				_playerLobbyAvatarModel.ButtMeshPreset.BindWriter(null);
				_playerLobbyAvatarNetworkObject = null;
				_hasPendingNickname = false;
				_hasPendingPrimaryColor = false;
				_hasPendingVariableColor = false;
				_hasPendingHatPartSkinId = false;
				_hasPendingTorsoPartSkinId = false;
				_hasPendingBottomPartSkinId = false;
				_hasPendingIsFullSkin = false;
				_hasPendingButtTexturePreset = false;
				_hasPendingButtMeshPreset = false;
				_playerLobbyAvatarModel.SetAuthorityProvider(null);
				_playerLobbyAvatarModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<PlayerLobbyAvatarNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedNicknameChanged(NetworkString<_32> nickname)
		{
			ApplyNicknameFromNetwork(nickname);
		}

		private void HandleNetworkedPrimaryColorChanged(int primaryColor)
		{
			ApplyPrimaryColorFromNetwork(primaryColor);
		}

		private void HandleNetworkedVariableColorChanged(int variableColor)
		{
			ApplyVariableColorFromNetwork(variableColor);
		}

		private void HandleNetworkedHatPartSkinIdChanged(SkinType hatPartSkinId)
		{
			ApplyHatPartSkinIdFromNetwork(hatPartSkinId);
		}

		private void HandleNetworkedTorsoPartSkinIdChanged(SkinType torsoPartSkinId)
		{
			ApplyTorsoPartSkinIdFromNetwork(torsoPartSkinId);
		}

		private void HandleNetworkedBottomPartSkinIdChanged(SkinType bottomPartSkinId)
		{
			ApplyBottomPartSkinIdFromNetwork(bottomPartSkinId);
		}

		private void HandleNetworkedIsFullSkinChanged(bool isFullSkin)
		{
			ApplyIsFullSkinFromNetwork(isFullSkin);
		}

		private void HandleNetworkedButtTexturePresetChanged(ButtTexturePreset buttTexturePreset)
		{
			ApplyButtTexturePresetFromNetwork(buttTexturePreset);
		}

		private void HandleNetworkedButtMeshPresetChanged(ButtMeshPreset buttMeshPreset)
		{
			ApplyButtMeshPresetFromNetwork(buttMeshPreset);
		}

		private void ApplyNicknameFromNetwork(NetworkString<_32> nickname)
		{
			_playerLobbyAvatarModel.Nickname.ApplyFromNetwork(nickname);
		}

		private void ApplyPrimaryColorFromNetwork(int primaryColor)
		{
			_playerLobbyAvatarModel.PrimaryColor.ApplyFromNetwork(primaryColor);
		}

		private void ApplyVariableColorFromNetwork(int variableColor)
		{
			_playerLobbyAvatarModel.VariableColor.ApplyFromNetwork(variableColor);
		}

		private void ApplyHatPartSkinIdFromNetwork(SkinType hatPartSkinId)
		{
			_playerLobbyAvatarModel.HatPartSkinId.ApplyFromNetwork(hatPartSkinId);
		}

		private void ApplyTorsoPartSkinIdFromNetwork(SkinType torsoPartSkinId)
		{
			_playerLobbyAvatarModel.TorsoPartSkinId.ApplyFromNetwork(torsoPartSkinId);
		}

		private void ApplyBottomPartSkinIdFromNetwork(SkinType bottomPartSkinId)
		{
			_playerLobbyAvatarModel.BottomPartSkinId.ApplyFromNetwork(bottomPartSkinId);
		}

		private void ApplyIsFullSkinFromNetwork(bool isFullSkin)
		{
			_playerLobbyAvatarModel.IsFullSkin.ApplyFromNetwork(isFullSkin);
		}

		private void ApplyButtTexturePresetFromNetwork(ButtTexturePreset buttTexturePreset)
		{
			_playerLobbyAvatarModel.ButtTexturePreset.ApplyFromNetwork(buttTexturePreset);
		}

		private void ApplyButtMeshPresetFromNetwork(ButtMeshPreset buttMeshPreset)
		{
			_playerLobbyAvatarModel.ButtMeshPreset.ApplyFromNetwork(buttMeshPreset);
		}

		private bool WriteNickname(NetworkString<_32> value)
		{
			if (!_playerLobbyAvatarModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PlayerLobbyAvatarModel.Nickname was written without state authority; the write was ignored.");
				return false;
			}
			_pendingNickname = value;
			_hasPendingNickname = true;
			return true;
		}

		private bool WritePrimaryColor(int value)
		{
			if (!_playerLobbyAvatarModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PlayerLobbyAvatarModel.PrimaryColor was written without state authority; the write was ignored.");
				return false;
			}
			_pendingPrimaryColor = value;
			_hasPendingPrimaryColor = true;
			return true;
		}

		private bool WriteVariableColor(int value)
		{
			if (!_playerLobbyAvatarModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PlayerLobbyAvatarModel.VariableColor was written without state authority; the write was ignored.");
				return false;
			}
			_pendingVariableColor = value;
			_hasPendingVariableColor = true;
			return true;
		}

		private bool WriteHatPartSkinId(SkinType value)
		{
			if (!_playerLobbyAvatarModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PlayerLobbyAvatarModel.HatPartSkinId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingHatPartSkinId = value;
			_hasPendingHatPartSkinId = true;
			return true;
		}

		private bool WriteTorsoPartSkinId(SkinType value)
		{
			if (!_playerLobbyAvatarModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PlayerLobbyAvatarModel.TorsoPartSkinId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingTorsoPartSkinId = value;
			_hasPendingTorsoPartSkinId = true;
			return true;
		}

		private bool WriteBottomPartSkinId(SkinType value)
		{
			if (!_playerLobbyAvatarModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PlayerLobbyAvatarModel.BottomPartSkinId was written without state authority; the write was ignored.");
				return false;
			}
			_pendingBottomPartSkinId = value;
			_hasPendingBottomPartSkinId = true;
			return true;
		}

		private bool WriteIsFullSkin(bool value)
		{
			if (!_playerLobbyAvatarModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PlayerLobbyAvatarModel.IsFullSkin was written without state authority; the write was ignored.");
				return false;
			}
			_pendingIsFullSkin = value;
			_hasPendingIsFullSkin = true;
			return true;
		}

		private bool WriteButtTexturePreset(ButtTexturePreset value)
		{
			if (!_playerLobbyAvatarModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PlayerLobbyAvatarModel.ButtTexturePreset was written without state authority; the write was ignored.");
				return false;
			}
			_pendingButtTexturePreset = value;
			_hasPendingButtTexturePreset = true;
			return true;
		}

		private bool WriteButtMeshPreset(ButtMeshPreset value)
		{
			if (!_playerLobbyAvatarModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PlayerLobbyAvatarModel.ButtMeshPreset was written without state authority; the write was ignored.");
				return false;
			}
			_pendingButtMeshPreset = value;
			_hasPendingButtMeshPreset = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_playerLobbyAvatarNetworkObject == null || _playerLobbyAvatarNetworkObject.Object == null || _playerLobbyAvatarNetworkObject.Runner == null)
			{
				return false;
			}
			if (_playerLobbyAvatarNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_playerLobbyAvatarNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingNickname)
			{
				_hasPendingNickname = false;
				_playerLobbyAvatarNetworkObject.TryWriteNickname(_pendingNickname);
			}
			if (_hasPendingPrimaryColor)
			{
				_hasPendingPrimaryColor = false;
				_playerLobbyAvatarNetworkObject.TryWritePrimaryColor(_pendingPrimaryColor);
			}
			if (_hasPendingVariableColor)
			{
				_hasPendingVariableColor = false;
				_playerLobbyAvatarNetworkObject.TryWriteVariableColor(_pendingVariableColor);
			}
			if (_hasPendingHatPartSkinId)
			{
				_hasPendingHatPartSkinId = false;
				_playerLobbyAvatarNetworkObject.TryWriteHatPartSkinId(_pendingHatPartSkinId);
			}
			if (_hasPendingTorsoPartSkinId)
			{
				_hasPendingTorsoPartSkinId = false;
				_playerLobbyAvatarNetworkObject.TryWriteTorsoPartSkinId(_pendingTorsoPartSkinId);
			}
			if (_hasPendingBottomPartSkinId)
			{
				_hasPendingBottomPartSkinId = false;
				_playerLobbyAvatarNetworkObject.TryWriteBottomPartSkinId(_pendingBottomPartSkinId);
			}
			if (_hasPendingIsFullSkin)
			{
				_hasPendingIsFullSkin = false;
				_playerLobbyAvatarNetworkObject.TryWriteIsFullSkin(_pendingIsFullSkin);
			}
			if (_hasPendingButtTexturePreset)
			{
				_hasPendingButtTexturePreset = false;
				_playerLobbyAvatarNetworkObject.TryWriteButtTexturePreset(_pendingButtTexturePreset);
			}
			if (_hasPendingButtMeshPreset)
			{
				_hasPendingButtMeshPreset = false;
				_playerLobbyAvatarNetworkObject.TryWriteButtMeshPreset(_pendingButtMeshPreset);
			}
		}
	}
}
