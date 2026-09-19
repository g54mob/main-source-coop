using System;
using Features.DeadPartsModule.Data;
using Features.PlayerIdentityModule;
using Features.PlayerPresenceModule.Networked;
using Features.SkinConfiguration.Scripts;
using Fusion;
using PlayerCustomization.Data;
using UnityEngine;
using Zenject;

namespace PlayerCustomization.Networked
{
	[NetworkBehaviourWeaved(107)]
	public class PlayerAvatarCosmetics : NetworkBehaviour
	{
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Cosmetics", 0, 8)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private PlayerCosmeticsData _Cosmetics;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Nickname", 8, 33)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkString<_32> _Nickname;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("StablePlayerId", 41, 65)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkString<_64> _StablePlayerId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsAuthored", 106, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsAuthored;

		private PlayerCustomizationModel _playerCustomizationModel;

		private PlayerAvatarCosmeticsModel _playerAvatarCosmeticsModel;

		private PlayerProfileModel _playerProfileModel;

		private SessionRewardModel _sessionRewardModel;

		private SkinsConfiguration _skinsConfiguration;

		private IPersistentPlayerIdProvider _persistentPlayerIdProvider;

		private SessionPlayerDeadPartStore _sessionPlayerDeadPartStore;

		private bool _rewardsDirty;

		private bool _hasAppliedSeed;

		private int _lastAppliedSeed;

		private bool _hasResolved;

		private PlayerCosmeticsData _lastResolvedCosmetics;

		private NetworkString<_32> _lastResolvedNickname;

		private BottomPartSnapshot _lastResolvedBottomPart;

		[Networked]
		[NetworkedWeaved(0, 8)]
		public unsafe PlayerCosmeticsData Cosmetics
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerAvatarCosmetics.Cosmetics. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(PlayerCosmeticsData*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerAvatarCosmetics.Cosmetics. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(PlayerCosmeticsData*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(8, 33)]
		public unsafe NetworkString<_32> Nickname
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerAvatarCosmetics.Nickname. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkString<_32>*)(Ptr + 8);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerAvatarCosmetics.Nickname. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkString<_32>*)(Ptr + 8) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(41, 65)]
		public unsafe NetworkString<_64> StablePlayerId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerAvatarCosmetics.StablePlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkString<_64>*)(Ptr + 41);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerAvatarCosmetics.StablePlayerId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkString<_64>*)(Ptr + 41) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(106, 1)]
		public unsafe NetworkBool IsAuthored
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerAvatarCosmetics.IsAuthored. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 106);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerAvatarCosmetics.IsAuthored. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 106) = value;
			}
		}

		public int PlayerId => base.Object.InputAuthority.PlayerId;

		[Inject]
		public void InjectDependencies(PlayerCustomizationModel playerCustomizationModel, PlayerAvatarCosmeticsModel playerAvatarCosmeticsModel, PlayerProfileModel playerProfileModel, SessionRewardModel sessionRewardModel, SkinsConfiguration skinsConfiguration, IPersistentPlayerIdProvider persistentPlayerIdProvider, SessionPlayerDeadPartStore sessionPlayerDeadPartStore)
		{
			_playerCustomizationModel = playerCustomizationModel;
			_playerAvatarCosmeticsModel = playerAvatarCosmeticsModel;
			_playerProfileModel = playerProfileModel;
			_sessionRewardModel = sessionRewardModel;
			_skinsConfiguration = skinsConfiguration;
			_persistentPlayerIdProvider = persistentPlayerIdProvider;
			_sessionPlayerDeadPartStore = sessionPlayerDeadPartStore;
		}

		public override void Spawned()
		{
			base.Spawned();
			_playerAvatarCosmeticsModel.Register(PlayerId, this, base.Object.HasInputAuthority);
			_sessionRewardModel.OnRewardsChanged += OnRewardsChanged;
			if (base.HasStateAuthority && !IsAuthored)
			{
				Author();
			}
			_rewardsDirty = true;
			ResolveIfChanged();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && (bool)IsAuthored)
			{
				int num = ResolveCosmeticSeed(StablePlayerId.Value);
				if (_rewardsDirty || !_hasAppliedSeed || num != _lastAppliedSeed)
				{
					_rewardsDirty = false;
					_hasAppliedSeed = true;
					_lastAppliedSeed = num;
					ApplyOwnedRewards();
				}
			}
		}

		public override void Render()
		{
			ResolveIfChanged();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_sessionRewardModel.OnRewardsChanged -= OnRewardsChanged;
			_playerAvatarCosmeticsModel.Unregister(PlayerId);
			_playerCustomizationModel.RemoveSlot(PlayerId);
		}

		public void SetPrimaryColor(Color color)
		{
			if (base.HasStateAuthority)
			{
				_playerProfileModel.SetPlayerColor(color);
				PlayerCosmeticsData cosmetics = Cosmetics;
				cosmetics.PrimaryColor = color;
				Cosmetics = cosmetics;
			}
		}

		public void SetNickname(string nickname)
		{
			if (base.HasStateAuthority)
			{
				_playerProfileModel.SetPlayerName(nickname);
				Nickname = nickname;
			}
		}

		private void Author()
		{
			SkinType skinType = StarterSkin(ResolveCosmeticSeed(_persistentPlayerIdProvider.LocalId.Value));
			Cosmetics = new PlayerCosmeticsData
			{
				PrimaryColor = _playerProfileModel.PlayerColor,
				HatPartSkinId = skinType,
				TorsoPartSkinId = skinType,
				BottomPartSkinId = skinType,
				IsFullSkin = true
			};
			Nickname = _playerProfileModel.PlayerName;
			StablePlayerId = _persistentPlayerIdProvider.LocalId.Value;
			IsAuthored = true;
		}

		private void ApplyOwnedRewards()
		{
			if (!base.HasStateAuthority || !IsAuthored)
			{
				return;
			}
			SkinType skinType = StarterSkin(ResolveCosmeticSeed(StablePlayerId.Value));
			PlayerCosmeticsData data = Cosmetics;
			data.HatPartSkinId = skinType;
			data.TorsoPartSkinId = skinType;
			data.BottomPartSkinId = skinType;
			foreach (RewardSlot reward in _sessionRewardModel.GetRewards(StablePlayerId.Value))
			{
				ApplyPart(ref data, reward.Part, reward.SkinId);
			}
			data.IsFullSkin = data.HatPartSkinId == data.TorsoPartSkinId && data.TorsoPartSkinId == data.BottomPartSkinId;
			Cosmetics = data;
		}

		private static void ApplyPart(ref PlayerCosmeticsData data, CosmeticRewardPart part, SkinType skinId)
		{
			switch (part)
			{
			case CosmeticRewardPart.Hat:
				data.HatPartSkinId = skinId;
				break;
			case CosmeticRewardPart.Torso:
				data.TorsoPartSkinId = skinId;
				break;
			case CosmeticRewardPart.Bottom:
				data.BottomPartSkinId = skinId;
				break;
			}
		}

		private void OnRewardsChanged()
		{
			_rewardsDirty = true;
		}

		private void ResolveIfChanged()
		{
			if ((bool)IsAuthored)
			{
				_sessionPlayerDeadPartStore.TryGetBottomPart(PlayerId, out var snapshot);
				if (!_hasResolved || !CosmeticsEqual(Cosmetics, _lastResolvedCosmetics) || !Nickname.Equals(_lastResolvedNickname) || !BottomPartEqual(snapshot, _lastResolvedBottomPart))
				{
					_hasResolved = true;
					_lastResolvedCosmetics = Cosmetics;
					_lastResolvedNickname = Nickname;
					_lastResolvedBottomPart = snapshot;
					_playerCustomizationModel.UpsertSlot(PlayerId, ToSlot(snapshot));
				}
			}
		}

		private static bool CosmeticsEqual(PlayerCosmeticsData a, PlayerCosmeticsData b)
		{
			if (a.HatPartSkinId == b.HatPartSkinId && a.TorsoPartSkinId == b.TorsoPartSkinId && a.BottomPartSkinId == b.BottomPartSkinId && (bool)a.IsFullSkin == (bool)b.IsFullSkin)
			{
				return a.PrimaryColor == b.PrimaryColor;
			}
			return false;
		}

		private static bool BottomPartEqual(BottomPartSnapshot a, BottomPartSnapshot b)
		{
			if (a.HasValue == b.HasValue && a.Color == b.Color && a.SkinId == b.SkinId && a.TexturePresetId == b.TexturePresetId)
			{
				return a.MeshPresetId == b.MeshPresetId;
			}
			return false;
		}

		private PlayerCustomizationSlotData ToSlot(BottomPartSnapshot bottomPart)
		{
			PlayerCosmeticsData cosmetics = Cosmetics;
			return new PlayerCustomizationSlotData
			{
				PlayerId = PlayerId,
				Nickname = Nickname.Value,
				PrimaryColor = cosmetics.PrimaryColor,
				VariableColor = (bottomPart.HasValue ? bottomPart.Color : cosmetics.PrimaryColor),
				HatPartSkinId = cosmetics.HatPartSkinId,
				TorsoPartSkinId = cosmetics.TorsoPartSkinId,
				BottomPartSkinId = (bottomPart.HasValue ? ((SkinType)bottomPart.SkinId) : cosmetics.BottomPartSkinId),
				IsFullSkin = (!bottomPart.HasValue && (bool)cosmetics.IsFullSkin),
				ButtTexturePreset = ((!bottomPart.HasValue) ? ButtTexturePreset.Default : ((ButtTexturePreset)bottomPart.TexturePresetId)),
				ButtMeshPreset = ((!bottomPart.HasValue) ? ButtMeshPreset.Default : ((ButtMeshPreset)bottomPart.MeshPresetId)),
				PlayerCustomizationSlotDataStatus = PlayerCustomizationSlotDataStatus.Initialized
			};
		}

		private int ResolveCosmeticSeed(string ownIdentity)
		{
			if (_sessionPlayerDeadPartStore.TryGetCosmeticSeed(PlayerId, out var seed))
			{
				return seed;
			}
			return StableHash(ownIdentity);
		}

		private SkinType StarterSkin(int seed)
		{
			int count = _skinsConfiguration.StartedPossibleSkinTypes.Count;
			int index = (seed & 0x7FFFFFFF) % count;
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

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Cosmetics = _Cosmetics;
			Nickname = _Nickname;
			StablePlayerId = _StablePlayerId;
			IsAuthored = _IsAuthored;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Cosmetics = Cosmetics;
			_Nickname = Nickname;
			_StablePlayerId = StablePlayerId;
			_IsAuthored = IsAuthored;
		}
	}
}
