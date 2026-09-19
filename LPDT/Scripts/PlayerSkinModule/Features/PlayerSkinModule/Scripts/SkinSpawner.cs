using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.HeadwearModule.Scripts;
using Features.LevelLightModule.Scripts.Initialization;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerGrabModule.Scripts;
using Features.PlayerSkinModule.Scripts.Features.PlayerSkinModule.Scripts.VisibilityHandling;
using Features.PlayerStatesModule.Scripts;
using Features.SkinConfiguration.Scripts;
using Fusion;
using PlayerCustomization;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;
using Zenject;

namespace Features.PlayerSkinModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class SkinSpawner : NetworkBehaviour
	{
		[SerializeField]
		private Transform _skinParent;

		[SerializeField]
		private Transform _topSkinParent;

		[SerializeField]
		private Transform _midSkinParent;

		[SerializeField]
		private Transform _bootsSkinParent;

		[SerializeField]
		private Transform _characterRootBone;

		[SerializeField]
		private SkinnedMeshRenderer _characterSkinnedMesh;

		[SerializeField]
		private VisibilityHandlerBase _allVisibilityHandler;

		[SerializeField]
		private VisibilityHandlerBase _baseVisibilityHandler;

		[SerializeField]
		private VisibilityHandlerBase _hatVisibilityHandler;

		[FormerlySerializedAs("_topVisibilityHandler")]
		[SerializeField]
		private VisibilityHandlerBase _torsoVisibilityHandler;

		[SerializeField]
		private VisibilityHandlerBase _botVisibilityHandler;

		[SerializeField]
		private CustomizationAnimationReactor _customizationAnimationReactor;

		[SerializeField]
		private PlayerLightInitializer _playerLightInitializer;

		private PlayerCustomizationModel _playerCustomizationModel;

		private PLayerGrabOutlineModel _pLayerGrabOutlineModel;

		private SkinsConfiguration _skinsConfiguration;

		private SkinModel _skinModel;

		private SkinChangedEvent _skinChangedEvent;

		private PlayersStatesSynchronizer _playersStatesSynchronizer;

		private HeadwearModel _headwearModel;

		private readonly Dictionary<SkinType, MultipleVisibilityHandler> _hatPartCachedSkins = new Dictionary<SkinType, MultipleVisibilityHandler>();

		private readonly Dictionary<SkinType, MultipleVisibilityHandler> _torsoPartCachedSkins = new Dictionary<SkinType, MultipleVisibilityHandler>();

		private readonly Dictionary<SkinType, MultipleVisibilityHandler> _bottomPartCachedSkins = new Dictionary<SkinType, MultipleVisibilityHandler>();

		private PlayerCustomizationSlotData _currentSlotData;

		private bool _isSkinModelRegistered;

		private bool _isHeadwearWorn;

		private MultipleVisibilityHandler HatVisibilityHandler;

		private MultipleVisibilityHandler TorsoVisibilityHandler;

		private MultipleVisibilityHandler BottomVisibilityHandler;

		[Inject]
		public void InjectDependencies(SkinsConfiguration skinsConfiguration, PlayerCustomizationModel playerCustomizationModel, SkinModel skinModel, PLayerGrabOutlineModel pLayerGrabOutlineModel, SkinChangedEvent skinChangedEvent, PlayersStatesSynchronizer playersStatesSynchronizer, HeadwearModel headwearModel)
		{
			_playerCustomizationModel = playerCustomizationModel;
			_skinsConfiguration = skinsConfiguration;
			_skinModel = skinModel;
			_pLayerGrabOutlineModel = pLayerGrabOutlineModel;
			_skinChangedEvent = skinChangedEvent;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_headwearModel = headwearModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			TryHideBottomForDeadSpawn();
			int inputAuthorityPlayerId = base.Object.InputAuthority.PlayerId;
			_playerCustomizationModel.OnSlotsChanged += OnPlayerCustomizationChanged;
			_headwearModel.OnHeadwearWornChanged += OnHeadwearWornChanged;
			_isHeadwearWorn = _headwearModel.TryGetHeadwear(inputAuthorityPlayerId, out var _);
			PlayerCustomizationSlotData playerCustomizationSlotData = _playerCustomizationModel.Slots.FirstOrDefault((PlayerCustomizationSlotData s) => s.PlayerId == inputAuthorityPlayerId);
			if (playerCustomizationSlotData != null)
			{
				ActivateHatPartSkin(playerCustomizationSlotData.HatPartSkinId);
				ActivateTorsoPartSkin(playerCustomizationSlotData.TorsoPartSkinId);
				ActivateBottomPartSkin(playerCustomizationSlotData.BottomPartSkinId);
				_skinModel.AllCharacterVisibility.Add(base.Object.InputAuthority.PlayerId, _allVisibilityHandler);
				_skinModel.BaseCharacterVisibility.Add(base.Object.InputAuthority.PlayerId, _baseVisibilityHandler);
				_skinModel.HatCharacterVisibility.Add(base.Object.InputAuthority.PlayerId, (_hatVisibilityHandler != null) ? _hatVisibilityHandler : _torsoVisibilityHandler);
				_skinModel.TorsoCharacterVisibility.Add(base.Object.InputAuthority.PlayerId, _torsoVisibilityHandler);
				_skinModel.BottomCharacterVisibility.Add(base.Object.InputAuthority.PlayerId, _botVisibilityHandler);
				_skinModel.SkinVisibility.Add(base.Object.InputAuthority.PlayerId, this);
				_skinModel.NotifyPlayerSkinRegistered(base.Object.InputAuthority.PlayerId);
				_isSkinModelRegistered = true;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_playerCustomizationModel.OnSlotsChanged -= OnPlayerCustomizationChanged;
			_headwearModel.OnHeadwearWornChanged -= OnHeadwearWornChanged;
			if (_isSkinModelRegistered)
			{
				_skinModel.UnregisterPlayer(base.Object.InputAuthority.PlayerId);
				_isSkinModelRegistered = false;
			}
		}

		private void ActivateHatPartSkin(SkinType skinId)
		{
			HatVisibilityHandler = ActivatePartSkin(skinId, _hatPartCachedSkins, _topSkinParent);
			ApplyHeadwearHatState();
		}

		private void ActivateTorsoPartSkin(SkinType skinId)
		{
			TorsoVisibilityHandler = ActivatePartSkin(skinId, _torsoPartCachedSkins, _midSkinParent);
			_pLayerGrabOutlineModel.AddOutline(base.Object.InputAuthority.PlayerId, TorsoVisibilityHandler.GetComponent<PlayerOutlineRegistrar>().Outline);
		}

		private void ActivateBottomPartSkin(SkinType skinId)
		{
			BottomVisibilityHandler = ActivatePartSkin(skinId, _bottomPartCachedSkins, _bootsSkinParent);
		}

		private MultipleVisibilityHandler ActivatePartSkin(SkinType skinId, Dictionary<SkinType, MultipleVisibilityHandler> cachedSkins, Transform skinParent)
		{
			foreach (MultipleVisibilityHandler value2 in cachedSkins.Values)
			{
				value2.DisableRenderObject();
			}
			if (!cachedSkins.TryGetValue(skinId, out var value))
			{
				PlayerSkin playerSkin = UnityEngine.Object.Instantiate(_skinsConfiguration.PlayerSkins[skinId], (skinParent != null) ? skinParent : _skinParent, worldPositionStays: false);
				playerSkin.Initialize(_customizationAnimationReactor);
				SkinnedMeshRenderer[] componentsInChildren = playerSkin.GetComponentsInChildren<SkinnedMeshRenderer>();
				SkinnedMeshRenderer[] array = componentsInChildren;
				foreach (SkinnedMeshRenderer obj in array)
				{
					obj.rootBone = _characterRootBone;
					obj.bones = _characterSkinnedMesh.bones;
				}
				_playerLightInitializer.InitializeLight(((IEnumerable<SkinnedMeshRenderer>)componentsInChildren).Select((Func<SkinnedMeshRenderer, Renderer>)((SkinnedMeshRenderer x) => x)).ToList());
				value = (cachedSkins[skinId] = playerSkin.GetComponent<MultipleVisibilityHandler>());
			}
			value.EnableRenderObject();
			return value;
		}

		private void OnPlayerCustomizationChanged()
		{
			PlayerCustomizationSlotData playerCustomizationSlotData = _playerCustomizationModel.Slots.FirstOrDefault((PlayerCustomizationSlotData x) => x.PlayerId == base.Object.InputAuthority.PlayerId);
			if (playerCustomizationSlotData == null || (_currentSlotData != null && _currentSlotData == playerCustomizationSlotData))
			{
				return;
			}
			_currentSlotData = playerCustomizationSlotData;
			ActivateHatPartSkin(_currentSlotData.HatPartSkinId);
			ActivateTorsoPartSkin(_currentSlotData.TorsoPartSkinId);
			ActivateBottomPartSkin(_currentSlotData.BottomPartSkinId);
			if (!_isSkinModelRegistered)
			{
				_skinModel.AllCharacterVisibility.Add(base.Object.InputAuthority.PlayerId, _allVisibilityHandler);
				_skinModel.BaseCharacterVisibility.Add(base.Object.InputAuthority.PlayerId, _baseVisibilityHandler);
				_skinModel.HatCharacterVisibility.Add(base.Object.InputAuthority.PlayerId, (_hatVisibilityHandler != null) ? _hatVisibilityHandler : _torsoVisibilityHandler);
				_skinModel.TorsoCharacterVisibility.Add(base.Object.InputAuthority.PlayerId, _torsoVisibilityHandler);
				_skinModel.BottomCharacterVisibility.Add(base.Object.InputAuthority.PlayerId, _botVisibilityHandler);
				_skinModel.SkinVisibility.Add(base.Object.InputAuthority.PlayerId, this);
				_skinModel.NotifyPlayerSkinRegistered(base.Object.InputAuthority.PlayerId);
				_isSkinModelRegistered = true;
				return;
			}
			HatVisibilityHandler.EnableHatRenderObjectOnly();
			HatVisibilityHandler.DisableTorsoRenderObject();
			HatVisibilityHandler.DisableBottomRenderObject();
			TorsoVisibilityHandler.EnableTorsoRenderObjectOnly();
			TorsoVisibilityHandler.DisableHatRenderObject();
			TorsoVisibilityHandler.DisableBottomRenderObject();
			BottomVisibilityHandler.EnableBottomRenderObjectOnly();
			BottomVisibilityHandler.DisableHatRenderObject();
			BottomVisibilityHandler.DisableTorsoRenderObject();
			if (base.HasInputAuthority)
			{
				DisableVisibility();
			}
			else
			{
				EnableVisibility();
			}
			ApplyHeadwearHatState();
			_skinChangedEvent.Invoke(base.Object.InputAuthority.PlayerId);
		}

		public void EnableVisibility()
		{
			HatVisibilityHandler.EnableHatVisibility();
			HatVisibilityHandler.DisableTorsoVisibility();
			HatVisibilityHandler.DisableBottomVisibility();
			TorsoVisibilityHandler.EnableTorsoVisibility();
			TorsoVisibilityHandler.DisableHatVisibility();
			TorsoVisibilityHandler.DisableBottomVisibility();
			BottomVisibilityHandler.EnableBottomVisibility();
			BottomVisibilityHandler.DisableHatVisibility();
			BottomVisibilityHandler.DisableTorsoVisibility();
		}

		public void EnableDeadVisibility()
		{
			HatVisibilityHandler.EnableHatVisibility();
			HatVisibilityHandler.DisableTorsoVisibility();
			HatVisibilityHandler.DisableBottomVisibility();
			TorsoVisibilityHandler.EnableTorsoVisibility();
			TorsoVisibilityHandler.DisableHatVisibility();
			TorsoVisibilityHandler.DisableBottomVisibility();
			BottomVisibilityHandler.DisableBottomVisibility();
			BottomVisibilityHandler.DisableHatVisibility();
			BottomVisibilityHandler.DisableTorsoVisibility();
		}

		public void DisableVisibility()
		{
			HatVisibilityHandler.DisableHatVisibility();
			HatVisibilityHandler.DisableTorsoVisibility();
			HatVisibilityHandler.DisableBottomVisibility();
			TorsoVisibilityHandler.DisableHatVisibility();
			TorsoVisibilityHandler.DisableTorsoVisibility();
			TorsoVisibilityHandler.DisableBottomVisibility();
			BottomVisibilityHandler.DisableBottomVisibility();
			BottomVisibilityHandler.DisableHatVisibility();
			BottomVisibilityHandler.DisableTorsoVisibility();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3855687050u)]
		public void DeadSkinRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3855687050u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.PlayerSkinModule.Scripts.SkinSpawner::DeadSkinRPC()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			ApplyDeadSkinLocal();
		}

		public void ApplyDeadSkinLocal()
		{
			if (!(HatVisibilityHandler == null) && !(TorsoVisibilityHandler == null) && !(BottomVisibilityHandler == null))
			{
				HatVisibilityHandler.EnableHatRenderObjectOnly();
				TorsoVisibilityHandler.EnableTorsoRenderObjectOnly();
				BottomVisibilityHandler.DisableHatRenderObject();
				BottomVisibilityHandler.DisableTorsoRenderObject();
				BottomVisibilityHandler.DisableBottomRenderObject();
				ApplyHeadwearHatState();
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1191171727u)]
		public void AliveSkinRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1191171727u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.PlayerSkinModule.Scripts.SkinSpawner::AliveSkinRPC()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			ApplyAliveSkinLocal();
		}

		public void ApplyAliveSkinLocal()
		{
			if (!(HatVisibilityHandler == null) && !(TorsoVisibilityHandler == null) && !(BottomVisibilityHandler == null))
			{
				HatVisibilityHandler.EnableHatRenderObjectOnly();
				TorsoVisibilityHandler.EnableTorsoRenderObjectOnly();
				BottomVisibilityHandler.EnableBottomRenderObjectOnly();
				ApplyHeadwearHatState();
			}
		}

		public void HideAllSkinRenderObjects()
		{
			if (!(HatVisibilityHandler == null) && !(TorsoVisibilityHandler == null) && !(BottomVisibilityHandler == null))
			{
				HideSkinPart(HatVisibilityHandler);
				HideSkinPart(TorsoVisibilityHandler);
				HideSkinPart(BottomVisibilityHandler);
			}
		}

		private static void HideSkinPart(MultipleVisibilityHandler handler)
		{
			handler.DisableHatRenderObject();
			handler.DisableTorsoRenderObject();
			handler.DisableBottomRenderObject();
			handler.DisableRenderObject();
		}

		private void OnHeadwearWornChanged(int playerId, bool isWorn)
		{
			if (playerId == base.Object.InputAuthority.PlayerId)
			{
				_isHeadwearWorn = isWorn;
				ApplyHeadwearHatState();
			}
		}

		private void ApplyHeadwearHatState()
		{
			if (!(HatVisibilityHandler == null))
			{
				if (_isHeadwearWorn)
				{
					HatVisibilityHandler.DisableHatRenderObject();
				}
				else
				{
					HatVisibilityHandler.EnableHatRenderObject();
				}
			}
		}

		private void TryHideBottomForDeadSpawn()
		{
			if (ShouldHideBottomForDeadSpawn())
			{
				_botVisibilityHandler.DisableRenderObject();
			}
		}

		private bool ShouldHideBottomForDeadSpawn()
		{
			if (base.HasInputAuthority && PlayerSessionPrefs.IsDeadPartSpawned())
			{
				return true;
			}
			if (_playersStatesSynchronizer.TryGetState(base.Object.InputAuthority.PlayerId, out var state))
			{
				if (state != PlayerState.Dead)
				{
					return state == PlayerState.PreDeadCrouch;
				}
				return true;
			}
			return false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(3855687050u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DeadSkinRPC_0040Invoker3855687050([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SkinSpawner)context.TargetBehaviour).DeadSkinRPC();
		}

		[NetworkRpcWeavedInvoker(1191171727u)]
		[Preserve]
		[WeaverGenerated]
		protected static void AliveSkinRPC_0040Invoker1191171727([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SkinSpawner)context.TargetBehaviour).AliveSkinRPC();
		}
	}
}
