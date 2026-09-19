using System;
using Features.DeadPartsModule.Data;
using Features.NetworkedModelRuntime;
using Features.SkinConfiguration.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace PlayerCustomization.LobbyAvatar.Networked
{
	[NetworkBehaviourWeaved(41)]
	public class PlayerLobbyAvatarNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Nickname", 0, 33)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkString<_32> _Nickname;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("PrimaryColor", 33, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _PrimaryColor;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("VariableColor", 34, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _VariableColor;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("HatPartSkinId", 35, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private SkinType _HatPartSkinId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("TorsoPartSkinId", 36, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private SkinType _TorsoPartSkinId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("BottomPartSkinId", 37, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private SkinType _BottomPartSkinId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsFullSkin", 38, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsFullSkin;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ButtTexturePreset", 39, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ButtTexturePreset _ButtTexturePreset;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ButtMeshPreset", 40, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ButtMeshPreset _ButtMeshPreset;

		[Networked]
		[OnChangedRender("OnNicknameChangedRender")]
		[NetworkedWeaved(0, 33)]
		public unsafe NetworkString<_32> Nickname
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.Nickname. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkString<_32>*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.Nickname. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkString<_32>*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnPrimaryColorChangedRender")]
		[NetworkedWeaved(33, 1)]
		public unsafe int PrimaryColor
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.PrimaryColor. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[33];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.PrimaryColor. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[33] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnVariableColorChangedRender")]
		[NetworkedWeaved(34, 1)]
		public unsafe int VariableColor
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.VariableColor. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[34];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.VariableColor. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[34] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnHatPartSkinIdChangedRender")]
		[NetworkedWeaved(35, 1)]
		public unsafe SkinType HatPartSkinId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.HatPartSkinId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (SkinType)Ptr[35];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.HatPartSkinId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[35] = (int)value;
			}
		}

		[Networked]
		[OnChangedRender("OnTorsoPartSkinIdChangedRender")]
		[NetworkedWeaved(36, 1)]
		public unsafe SkinType TorsoPartSkinId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.TorsoPartSkinId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (SkinType)Ptr[36];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.TorsoPartSkinId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[36] = (int)value;
			}
		}

		[Networked]
		[OnChangedRender("OnBottomPartSkinIdChangedRender")]
		[NetworkedWeaved(37, 1)]
		public unsafe SkinType BottomPartSkinId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.BottomPartSkinId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (SkinType)Ptr[37];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.BottomPartSkinId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[37] = (int)value;
			}
		}

		[Networked]
		[OnChangedRender("OnIsFullSkinChangedRender")]
		[NetworkedWeaved(38, 1)]
		public unsafe bool IsFullSkin
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.IsFullSkin. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 38);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.IsFullSkin. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 38) = new NetworkBool(value);
			}
		}

		[Networked]
		[OnChangedRender("OnButtTexturePresetChangedRender")]
		[NetworkedWeaved(39, 1)]
		public unsafe ButtTexturePreset ButtTexturePreset
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.ButtTexturePreset. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (ButtTexturePreset)Ptr[39];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.ButtTexturePreset. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[39] = (int)value;
			}
		}

		[Networked]
		[OnChangedRender("OnButtMeshPresetChangedRender")]
		[NetworkedWeaved(40, 1)]
		public unsafe ButtMeshPreset ButtMeshPreset
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.ButtMeshPreset. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (ButtMeshPreset)Ptr[40];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLobbyAvatarNetworkObject.ButtMeshPreset. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[40] = (int)value;
			}
		}

		public event Action<NetworkString<_32>> OnNetworkedNicknameChanged;

		public event Action<int> OnNetworkedPrimaryColorChanged;

		public event Action<int> OnNetworkedVariableColorChanged;

		public event Action<SkinType> OnNetworkedHatPartSkinIdChanged;

		public event Action<SkinType> OnNetworkedTorsoPartSkinIdChanged;

		public event Action<SkinType> OnNetworkedBottomPartSkinIdChanged;

		public event Action<bool> OnNetworkedIsFullSkinChanged;

		public event Action<ButtTexturePreset> OnNetworkedButtTexturePresetChanged;

		public event Action<ButtMeshPreset> OnNetworkedButtMeshPresetChanged;

		public event Action OnAuthoritativeTick;

		public event Action OnDespawned;

		[Inject]
		public void InjectDependencies(INetworkedModelInstanceProvider networkedModelInstanceProvider)
		{
			_networkedModelInstanceProvider = networkedModelInstanceProvider;
		}

		public override void Spawned()
		{
			_networkedModelInstanceProvider?.Register(this);
			this.OnNetworkedNicknameChanged?.Invoke(Nickname);
			this.OnNetworkedPrimaryColorChanged?.Invoke(PrimaryColor);
			this.OnNetworkedVariableColorChanged?.Invoke(VariableColor);
			this.OnNetworkedHatPartSkinIdChanged?.Invoke(HatPartSkinId);
			this.OnNetworkedTorsoPartSkinIdChanged?.Invoke(TorsoPartSkinId);
			this.OnNetworkedBottomPartSkinIdChanged?.Invoke(BottomPartSkinId);
			this.OnNetworkedIsFullSkinChanged?.Invoke(IsFullSkin);
			this.OnNetworkedButtTexturePresetChanged?.Invoke(ButtTexturePreset);
			this.OnNetworkedButtMeshPresetChanged?.Invoke(ButtMeshPreset);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_networkedModelInstanceProvider?.Unregister(this);
			this.OnDespawned?.Invoke();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				this.OnAuthoritativeTick?.Invoke();
			}
		}

		public bool TryWriteNickname(NetworkString<_32> nickname)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Nickname = nickname;
			return true;
		}

		public bool TryWritePrimaryColor(int primaryColor)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			PrimaryColor = primaryColor;
			return true;
		}

		public bool TryWriteVariableColor(int variableColor)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			VariableColor = variableColor;
			return true;
		}

		public bool TryWriteHatPartSkinId(SkinType hatPartSkinId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			HatPartSkinId = hatPartSkinId;
			return true;
		}

		public bool TryWriteTorsoPartSkinId(SkinType torsoPartSkinId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			TorsoPartSkinId = torsoPartSkinId;
			return true;
		}

		public bool TryWriteBottomPartSkinId(SkinType bottomPartSkinId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			BottomPartSkinId = bottomPartSkinId;
			return true;
		}

		public bool TryWriteIsFullSkin(bool isFullSkin)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			IsFullSkin = isFullSkin;
			return true;
		}

		public bool TryWriteButtTexturePreset(ButtTexturePreset buttTexturePreset)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			ButtTexturePreset = buttTexturePreset;
			return true;
		}

		public bool TryWriteButtMeshPreset(ButtMeshPreset buttMeshPreset)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			ButtMeshPreset = buttMeshPreset;
			return true;
		}

		private void OnNicknameChangedRender()
		{
			this.OnNetworkedNicknameChanged?.Invoke(Nickname);
		}

		private void OnPrimaryColorChangedRender()
		{
			this.OnNetworkedPrimaryColorChanged?.Invoke(PrimaryColor);
		}

		private void OnVariableColorChangedRender()
		{
			this.OnNetworkedVariableColorChanged?.Invoke(VariableColor);
		}

		private void OnHatPartSkinIdChangedRender()
		{
			this.OnNetworkedHatPartSkinIdChanged?.Invoke(HatPartSkinId);
		}

		private void OnTorsoPartSkinIdChangedRender()
		{
			this.OnNetworkedTorsoPartSkinIdChanged?.Invoke(TorsoPartSkinId);
		}

		private void OnBottomPartSkinIdChangedRender()
		{
			this.OnNetworkedBottomPartSkinIdChanged?.Invoke(BottomPartSkinId);
		}

		private void OnIsFullSkinChangedRender()
		{
			this.OnNetworkedIsFullSkinChanged?.Invoke(IsFullSkin);
		}

		private void OnButtTexturePresetChangedRender()
		{
			this.OnNetworkedButtTexturePresetChanged?.Invoke(ButtTexturePreset);
		}

		private void OnButtMeshPresetChangedRender()
		{
			this.OnNetworkedButtMeshPresetChanged?.Invoke(ButtMeshPreset);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Nickname = _Nickname;
			PrimaryColor = _PrimaryColor;
			VariableColor = _VariableColor;
			HatPartSkinId = _HatPartSkinId;
			TorsoPartSkinId = _TorsoPartSkinId;
			BottomPartSkinId = _BottomPartSkinId;
			IsFullSkin = _IsFullSkin;
			ButtTexturePreset = _ButtTexturePreset;
			ButtMeshPreset = _ButtMeshPreset;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Nickname = Nickname;
			_PrimaryColor = PrimaryColor;
			_VariableColor = VariableColor;
			_HatPartSkinId = HatPartSkinId;
			_TorsoPartSkinId = TorsoPartSkinId;
			_BottomPartSkinId = BottomPartSkinId;
			_IsFullSkin = IsFullSkin;
			_ButtTexturePreset = ButtTexturePreset;
			_ButtMeshPreset = ButtMeshPreset;
		}
	}
}
