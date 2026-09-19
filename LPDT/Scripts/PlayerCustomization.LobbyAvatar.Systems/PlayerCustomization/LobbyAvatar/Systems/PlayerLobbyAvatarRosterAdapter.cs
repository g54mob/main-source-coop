using System;
using System.Collections.Generic;
using Features.DeadPartsModule.Data;
using Features.NetworkedModelRuntime;
using Features.SkinConfiguration.Scripts;
using Fusion;
using PlayerCustomization.LobbyAvatar.Networked;
using UnityEngine;
using Zenject;

namespace PlayerCustomization.LobbyAvatar.Systems
{
	public class PlayerLobbyAvatarRosterAdapter : IInitializable, IDisposable
	{
		private class Binding
		{
			private readonly PlayerLobbyAvatarNetworkObject _transport;

			private readonly PlayerCustomizationModel _playerCustomizationModel;

			private readonly PlayerCustomizationSlotData _slot;

			public Binding(PlayerLobbyAvatarNetworkObject transport, int playerId, PlayerCustomizationModel playerCustomizationModel)
			{
				_transport = transport;
				_playerCustomizationModel = playerCustomizationModel;
				_slot = new PlayerCustomizationSlotData
				{
					PlayerId = playerId,
					Nickname = transport.Nickname.Value,
					PrimaryColor = LobbyAvatarColor.Unpack(transport.PrimaryColor),
					VariableColor = LobbyAvatarColor.Unpack(transport.VariableColor),
					HatPartSkinId = transport.HatPartSkinId,
					TorsoPartSkinId = transport.TorsoPartSkinId,
					BottomPartSkinId = transport.BottomPartSkinId,
					IsFullSkin = transport.IsFullSkin,
					ButtTexturePreset = transport.ButtTexturePreset,
					ButtMeshPreset = transport.ButtMeshPreset,
					PlayerCustomizationSlotDataStatus = PlayerCustomizationSlotDataStatus.Initialized
				};
			}

			public void Subscribe()
			{
				_transport.OnNetworkedNicknameChanged += OnNicknameChanged;
				_transport.OnNetworkedPrimaryColorChanged += OnPrimaryColorChanged;
				_transport.OnNetworkedVariableColorChanged += OnVariableColorChanged;
				_transport.OnNetworkedHatPartSkinIdChanged += OnHatPartSkinIdChanged;
				_transport.OnNetworkedTorsoPartSkinIdChanged += OnTorsoPartSkinIdChanged;
				_transport.OnNetworkedBottomPartSkinIdChanged += OnBottomPartSkinIdChanged;
				_transport.OnNetworkedIsFullSkinChanged += OnIsFullSkinChanged;
				_transport.OnNetworkedButtTexturePresetChanged += OnButtTexturePresetChanged;
				_transport.OnNetworkedButtMeshPresetChanged += OnButtMeshPresetChanged;
			}

			public void Unsubscribe()
			{
				_transport.OnNetworkedNicknameChanged -= OnNicknameChanged;
				_transport.OnNetworkedPrimaryColorChanged -= OnPrimaryColorChanged;
				_transport.OnNetworkedVariableColorChanged -= OnVariableColorChanged;
				_transport.OnNetworkedHatPartSkinIdChanged -= OnHatPartSkinIdChanged;
				_transport.OnNetworkedTorsoPartSkinIdChanged -= OnTorsoPartSkinIdChanged;
				_transport.OnNetworkedBottomPartSkinIdChanged -= OnBottomPartSkinIdChanged;
				_transport.OnNetworkedIsFullSkinChanged -= OnIsFullSkinChanged;
				_transport.OnNetworkedButtTexturePresetChanged -= OnButtTexturePresetChanged;
				_transport.OnNetworkedButtMeshPresetChanged -= OnButtMeshPresetChanged;
			}

			public void Apply()
			{
				_playerCustomizationModel.UpsertSlot(_slot.PlayerId, _slot);
			}

			public void RemoveOwnedSlot()
			{
				foreach (PlayerCustomizationSlotData slot in _playerCustomizationModel.Slots)
				{
					if (slot == _slot)
					{
						_playerCustomizationModel.RemoveSlot(_slot.PlayerId);
						break;
					}
				}
			}

			private void OnNicknameChanged(NetworkString<_32> nickname)
			{
				string value = nickname.Value;
				if (!(_slot.Nickname == value))
				{
					_slot.Nickname = value;
					Apply();
				}
			}

			private void OnPrimaryColorChanged(int packedColor)
			{
				Color color = LobbyAvatarColor.Unpack(packedColor);
				if (!(_slot.PrimaryColor == color))
				{
					_slot.PrimaryColor = color;
					Apply();
				}
			}

			private void OnVariableColorChanged(int packedColor)
			{
				Color color = LobbyAvatarColor.Unpack(packedColor);
				if (!(_slot.VariableColor == color))
				{
					_slot.VariableColor = color;
					Apply();
				}
			}

			private void OnHatPartSkinIdChanged(SkinType skinType)
			{
				if (_slot.HatPartSkinId != skinType)
				{
					_slot.HatPartSkinId = skinType;
					Apply();
				}
			}

			private void OnTorsoPartSkinIdChanged(SkinType skinType)
			{
				if (_slot.TorsoPartSkinId != skinType)
				{
					_slot.TorsoPartSkinId = skinType;
					Apply();
				}
			}

			private void OnBottomPartSkinIdChanged(SkinType skinType)
			{
				if (_slot.BottomPartSkinId != skinType)
				{
					_slot.BottomPartSkinId = skinType;
					Apply();
				}
			}

			private void OnIsFullSkinChanged(bool isFullSkin)
			{
				if (_slot.IsFullSkin != isFullSkin)
				{
					_slot.IsFullSkin = isFullSkin;
					Apply();
				}
			}

			private void OnButtTexturePresetChanged(ButtTexturePreset buttTexturePreset)
			{
				if (_slot.ButtTexturePreset != buttTexturePreset)
				{
					_slot.ButtTexturePreset = buttTexturePreset;
					Apply();
				}
			}

			private void OnButtMeshPresetChanged(ButtMeshPreset buttMeshPreset)
			{
				if (_slot.ButtMeshPreset != buttMeshPreset)
				{
					_slot.ButtMeshPreset = buttMeshPreset;
					Apply();
				}
			}
		}

		private readonly INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		private readonly PlayerCustomizationModel _playerCustomizationModel;

		private readonly Dictionary<PlayerLobbyAvatarNetworkObject, Binding> _bindingsByTransport = new Dictionary<PlayerLobbyAvatarNetworkObject, Binding>();

		public PlayerLobbyAvatarRosterAdapter(INetworkedModelInstanceProvider networkedModelInstanceProvider, PlayerCustomizationModel playerCustomizationModel)
		{
			_networkedModelInstanceProvider = networkedModelInstanceProvider;
			_playerCustomizationModel = playerCustomizationModel;
		}

		public void Initialize()
		{
			_networkedModelInstanceProvider.Added += OnInstanceAdded;
			_networkedModelInstanceProvider.Removed += OnInstanceRemoved;
			_playerCustomizationModel.OnClear += OnModelCleared;
			foreach (PlayerLobbyAvatarNetworkObject item in _networkedModelInstanceProvider.GetAll<PlayerLobbyAvatarNetworkObject>())
			{
				OnInstanceAdded(item);
			}
		}

		public void Dispose()
		{
			_networkedModelInstanceProvider.Added -= OnInstanceAdded;
			_networkedModelInstanceProvider.Removed -= OnInstanceRemoved;
			_playerCustomizationModel.OnClear -= OnModelCleared;
			foreach (Binding value in _bindingsByTransport.Values)
			{
				value.Unsubscribe();
			}
			_bindingsByTransport.Clear();
		}

		private void OnInstanceAdded(NetworkBehaviour instance)
		{
			if (instance is PlayerLobbyAvatarNetworkObject playerLobbyAvatarNetworkObject && !_bindingsByTransport.ContainsKey(playerLobbyAvatarNetworkObject) && !(playerLobbyAvatarNetworkObject.Object == null) && playerLobbyAvatarNetworkObject.Object.IsValid)
			{
				int playerId = playerLobbyAvatarNetworkObject.Object.StateAuthority.PlayerId;
				if (playerId >= 0)
				{
					Binding binding = new Binding(playerLobbyAvatarNetworkObject, playerId, _playerCustomizationModel);
					_bindingsByTransport[playerLobbyAvatarNetworkObject] = binding;
					binding.Subscribe();
					binding.Apply();
				}
			}
		}

		private void OnInstanceRemoved(NetworkBehaviour instance)
		{
			if (instance is PlayerLobbyAvatarNetworkObject key && _bindingsByTransport.TryGetValue(key, out var value))
			{
				value.Unsubscribe();
				_bindingsByTransport.Remove(key);
				value.RemoveOwnedSlot();
			}
		}

		private void OnModelCleared()
		{
			foreach (Binding value in _bindingsByTransport.Values)
			{
				value.Apply();
			}
		}
	}
}
