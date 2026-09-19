using System;
using Features.PlayerIdentityModule;
using Fusion;
using UnityEngine;

namespace Features.PlayerPresenceModule.Networked
{
	[NetworkBehaviourWeaved(120)]
	public class SessionPlayerNetworkObject : NetworkBehaviour
	{
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("OwnerId", 0, 65)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkString<_64> _OwnerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Nickname", 65, 33)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkString<_32> _Nickname;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ColorId", 98, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _ColorId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CosmeticsMask", 99, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _CosmeticsMask;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("StatusMask", 100, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _StatusMask;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("LifeState", 101, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _LifeState;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SavedLevelId", 102, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _SavedLevelId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SavedPosX", 103, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _SavedPosX;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SavedPosY", 104, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _SavedPosY;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SavedPosZ", 105, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _SavedPosZ;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SavedYaw", 106, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _SavedYaw;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SavedIsCrouching", 107, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _SavedIsCrouching;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SavedHealth", 108, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _SavedHealth;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("HasSavedReconnectState", 109, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _HasSavedReconnectState;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ShopVoteEpoch", 110, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _ShopVoteEpoch;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("HasVotedToLeaveShop", 111, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _HasVotedToLeaveShop;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("DeadPartTypeId", 112, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _DeadPartTypeId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("BottomPartColor", 113, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _BottomPartColor;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("BottomPartSkinId", 114, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _BottomPartSkinId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("BottomPartTexturePreset", 115, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _BottomPartTexturePreset;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("BottomPartMeshPreset", 116, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _BottomPartMeshPreset;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("BottomPartUsageCount", 117, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _BottomPartUsageCount;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CosmeticSeed", 118, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _CosmeticSeed;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsHeldByOwner", 119, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsHeldByOwner;

		private bool _isRegisteredInObjectRegistry;

		private bool _claimedByThisPeer;

		[Networked]
		[OnChangedRender("OnOwnerChangedRender")]
		[NetworkedWeaved(0, 65)]
		public unsafe NetworkString<_64> OwnerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.OwnerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkString<_64>*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.OwnerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkString<_64>*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnProfileChangedRender")]
		[NetworkedWeaved(65, 33)]
		public unsafe NetworkString<_32> Nickname
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.Nickname. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkString<_32>*)(Ptr + 65);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.Nickname. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkString<_32>*)(Ptr + 65) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnProfileChangedRender")]
		[NetworkedWeaved(98, 1)]
		public unsafe int ColorId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.ColorId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[98];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.ColorId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[98] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnProfileChangedRender")]
		[NetworkedWeaved(99, 1)]
		public unsafe int CosmeticsMask
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.CosmeticsMask. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[99];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.CosmeticsMask. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[99] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnProfileChangedRender")]
		[NetworkedWeaved(100, 1)]
		public unsafe int StatusMask
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.StatusMask. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[100];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.StatusMask. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[100] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnProfileChangedRender")]
		[NetworkedWeaved(101, 1)]
		public unsafe int LifeState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.LifeState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[101];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.LifeState. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[101] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(102, 1)]
		public unsafe int SavedLevelId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.SavedLevelId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[102];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.SavedLevelId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[102] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(103, 1)]
		public unsafe float SavedPosX
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.SavedPosX. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 103);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.SavedPosX. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 103) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(104, 1)]
		public unsafe float SavedPosY
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.SavedPosY. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 104);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.SavedPosY. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 104) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(105, 1)]
		public unsafe float SavedPosZ
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.SavedPosZ. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 105);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.SavedPosZ. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 105) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(106, 1)]
		public unsafe float SavedYaw
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.SavedYaw. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 106);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.SavedYaw. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 106) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(107, 1)]
		public unsafe NetworkBool SavedIsCrouching
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.SavedIsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 107);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.SavedIsCrouching. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 107) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(108, 1)]
		public unsafe float SavedHealth
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.SavedHealth. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 108);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.SavedHealth. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 108) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(109, 1)]
		public unsafe NetworkBool HasSavedReconnectState
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.HasSavedReconnectState. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 109);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.HasSavedReconnectState. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 109) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(110, 1)]
		public unsafe int ShopVoteEpoch
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.ShopVoteEpoch. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[110];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.ShopVoteEpoch. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[110] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(111, 1)]
		public unsafe NetworkBool HasVotedToLeaveShop
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.HasVotedToLeaveShop. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 111);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.HasVotedToLeaveShop. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 111) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(112, 1)]
		public unsafe int DeadPartTypeId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.DeadPartTypeId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[112];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.DeadPartTypeId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[112] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(113, 1)]
		public unsafe int BottomPartColor
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.BottomPartColor. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[113];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.BottomPartColor. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[113] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(114, 1)]
		public unsafe int BottomPartSkinId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.BottomPartSkinId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[114];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.BottomPartSkinId. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[114] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(115, 1)]
		public unsafe int BottomPartTexturePreset
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.BottomPartTexturePreset. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[115];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.BottomPartTexturePreset. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[115] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(116, 1)]
		public unsafe int BottomPartMeshPreset
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.BottomPartMeshPreset. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[116];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.BottomPartMeshPreset. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[116] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(117, 1)]
		public unsafe int BottomPartUsageCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.BottomPartUsageCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[117];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.BottomPartUsageCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[117] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(118, 1)]
		public unsafe int CosmeticSeed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.CosmeticSeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[118];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.CosmeticSeed. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[118] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(119, 1)]
		public unsafe NetworkBool IsHeldByOwner
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.IsHeldByOwner. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 119);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionPlayerNetworkObject.IsHeldByOwner. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 119) = value;
			}
		}

		public PersistentPlayerId OwnerIdValue => new PersistentPlayerId(OwnerId.ToString());

		public SessionPlayerProfileData CurrentProfile => new SessionPlayerProfileData(Nickname.ToString(), ColorId, CosmeticsMask, StatusMask, LifeState);

		public event Action<PersistentPlayerId> OnNetworkedOwnerChanged;

		public event Action<SessionPlayerProfileData> OnNetworkedProfileChanged;

		public event Action OnAuthoritativeTick;

		public event Action OnDespawned;

		public override void Spawned()
		{
			_claimedByThisPeer = false;
			if (!_isRegisteredInObjectRegistry)
			{
				SessionPlayerObjectRegistry.Register(this);
				_isRegisteredInObjectRegistry = true;
			}
			this.OnNetworkedOwnerChanged?.Invoke(OwnerIdValue);
			this.OnNetworkedProfileChanged?.Invoke(CurrentProfile);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_isRegisteredInObjectRegistry)
			{
				SessionPlayerObjectRegistry.Unregister(runner, this);
				_isRegisteredInObjectRegistry = false;
			}
			this.OnDespawned?.Invoke();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				IsHeldByOwner = _claimedByThisPeer;
				this.OnAuthoritativeTick?.Invoke();
			}
		}

		public bool TryClaimOwnership(PersistentPlayerId ownerId)
		{
			if (!base.HasStateAuthority || ownerId.IsNone)
			{
				return false;
			}
			PersistentPlayerId ownerIdValue = OwnerIdValue;
			if (!ownerIdValue.IsNone && !ownerIdValue.Equals(ownerId))
			{
				return false;
			}
			OwnerId = ownerId.Value;
			_claimedByThisPeer = true;
			return true;
		}

		public bool TryWriteProfile(SessionPlayerProfileData profile)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Nickname = profile.Nickname;
			ColorId = profile.ColorId;
			CosmeticsMask = profile.CosmeticsMask;
			StatusMask = profile.StatusMask;
			LifeState = profile.LifeState;
			return true;
		}

		public bool TryWriteReconnectState(int levelId, float posX, float posY, float posZ, float yaw, bool isCrouching, float health)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			SavedLevelId = levelId;
			SavedPosX = posX;
			SavedPosY = posY;
			SavedPosZ = posZ;
			SavedYaw = yaw;
			SavedIsCrouching = isCrouching;
			SavedHealth = health;
			HasSavedReconnectState = true;
			return true;
		}

		public bool TryWriteShopVote(int voteEpoch, bool hasVoted)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			ShopVoteEpoch = voteEpoch;
			HasVotedToLeaveShop = hasVoted;
			return true;
		}

		public bool TryWriteCosmeticSeed(int seed)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			if (CosmeticSeed != 0)
			{
				return true;
			}
			CosmeticSeed = seed;
			return true;
		}

		public bool TryWriteBottomPart(int deadPartTypeId, int packedColor, int skinId, int texturePreset, int meshPreset, int usageCount)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			DeadPartTypeId = deadPartTypeId;
			BottomPartColor = packedColor;
			BottomPartSkinId = skinId;
			BottomPartTexturePreset = texturePreset;
			BottomPartMeshPreset = meshPreset;
			BottomPartUsageCount = usageCount;
			return true;
		}

		public bool TryReclaim(PersistentPlayerId claimantId)
		{
			if (claimantId.IsNone)
			{
				return false;
			}
			PersistentPlayerId ownerIdValue = OwnerIdValue;
			if (!ownerIdValue.IsNone && !ownerIdValue.Equals(claimantId))
			{
				return false;
			}
			_claimedByThisPeer = true;
			if (base.HasStateAuthority)
			{
				return true;
			}
			base.Object.RequestStateAuthority();
			return true;
		}

		private void OnOwnerChangedRender()
		{
			this.OnNetworkedOwnerChanged?.Invoke(OwnerIdValue);
		}

		private void OnProfileChangedRender()
		{
			this.OnNetworkedProfileChanged?.Invoke(CurrentProfile);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			OwnerId = _OwnerId;
			Nickname = _Nickname;
			ColorId = _ColorId;
			CosmeticsMask = _CosmeticsMask;
			StatusMask = _StatusMask;
			LifeState = _LifeState;
			SavedLevelId = _SavedLevelId;
			SavedPosX = _SavedPosX;
			SavedPosY = _SavedPosY;
			SavedPosZ = _SavedPosZ;
			SavedYaw = _SavedYaw;
			SavedIsCrouching = _SavedIsCrouching;
			SavedHealth = _SavedHealth;
			HasSavedReconnectState = _HasSavedReconnectState;
			ShopVoteEpoch = _ShopVoteEpoch;
			HasVotedToLeaveShop = _HasVotedToLeaveShop;
			DeadPartTypeId = _DeadPartTypeId;
			BottomPartColor = _BottomPartColor;
			BottomPartSkinId = _BottomPartSkinId;
			BottomPartTexturePreset = _BottomPartTexturePreset;
			BottomPartMeshPreset = _BottomPartMeshPreset;
			BottomPartUsageCount = _BottomPartUsageCount;
			CosmeticSeed = _CosmeticSeed;
			IsHeldByOwner = _IsHeldByOwner;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_OwnerId = OwnerId;
			_Nickname = Nickname;
			_ColorId = ColorId;
			_CosmeticsMask = CosmeticsMask;
			_StatusMask = StatusMask;
			_LifeState = LifeState;
			_SavedLevelId = SavedLevelId;
			_SavedPosX = SavedPosX;
			_SavedPosY = SavedPosY;
			_SavedPosZ = SavedPosZ;
			_SavedYaw = SavedYaw;
			_SavedIsCrouching = SavedIsCrouching;
			_SavedHealth = SavedHealth;
			_HasSavedReconnectState = HasSavedReconnectState;
			_ShopVoteEpoch = ShopVoteEpoch;
			_HasVotedToLeaveShop = HasVotedToLeaveShop;
			_DeadPartTypeId = DeadPartTypeId;
			_BottomPartColor = BottomPartColor;
			_BottomPartSkinId = BottomPartSkinId;
			_BottomPartTexturePreset = BottomPartTexturePreset;
			_BottomPartMeshPreset = BottomPartMeshPreset;
			_BottomPartUsageCount = BottomPartUsageCount;
			_CosmeticSeed = CosmeticSeed;
			_IsHeldByOwner = IsHeldByOwner;
		}
	}
}
