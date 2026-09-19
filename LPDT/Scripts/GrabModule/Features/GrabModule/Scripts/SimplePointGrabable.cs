using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.GrabModule.Scripts.PhysGrab;
using Features.GrabModule.Scripts.Tips;
using Features.InteractModule.Scripts;
using Features.PhysicsUtilsModule.Scripts;
using Features.TipsModule.Scripts.Data;
using Fusion;
using QuickOutline.Scripts;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.GrabModule.Scripts
{
	[NetworkBehaviourWeaved(3)]
	public class SimplePointGrabable : NetworkBehaviour, IPointGrabable, IInterestEnter, IPublicFacingInterface
	{
		[SerializeField]
		private bool _isMainRagdollGrabable;

		[SerializeField]
		private bool _ignoredByCart;

		[SerializeField]
		private GrabbableStaticType _grabbableStaticType;

		[SerializeField]
		private bool _resetPhysicsOnCart;

		[SerializeField]
		private float _cartMass = 1f;

		[SerializeField]
		private bool _weightResetOnCart;

		[SerializeField]
		private InteractableBase _interactableBase;

		[SerializeField]
		private GrabObjectBase _grabObject;

		private readonly List<int> _grabbedByPlayers = new List<int>();

		private readonly List<int> _grabbedByExternals = new List<int>();

		[SerializeField]
		private Outline _outline;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private List<Transform> _handles = new List<Transform>();

		[SerializeField]
		private bool _freezeRotationOnGrab;

		[SerializeField]
		private int _availablePlayerCount = 999;

		[SerializeField]
		private int _availableExternalCount = 1;

		[SerializeField]
		private ArmConfiguration _armConfiguration;

		[SerializeField]
		private NetworkObjectComposite _networkObjectComposite;

		[SerializeField]
		private float _weight = 5f;

		[SerializeField]
		private bool _isWithChangeStateAuthority = true;

		[SerializeField]
		private bool _canGrabInStun;

		[SerializeField]
		private bool _canOverridePhysicMaterial = true;

		[SerializeField]
		private List<TipType> _allTips = new List<TipType>
		{
			TipType.ThrowTip,
			TipType.ZoomInOutTip
		};

		[SerializeField]
		private List<TipType> _allRaycastTips = new List<TipType> { TipType.PickUpTip };

		[SerializeField]
		private GrabbableTipsReactorBase _tipsReactor;

		[SerializeField]
		private SimplePointGrabable[] _connectedGrabables;

		[SerializeField]
		private bool _changeRigibodyKinematic = true;

		[SerializeField]
		private List<GameObject> _gameObjectsToDeactivateOnFirstGrab = new List<GameObject>();

		[SerializeField]
		private PhysicsResolutionController _physicsResolutionController;

		private bool _isOutlineLocked;

		private bool _lastSetOutline;

		[WeaverGenerated]
		[DefaultForProperty("IsReadyToQuotaInternal", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsReadyToQuotaInternal = true;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsAuthorityRequested", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsAuthorityRequested;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("GrabBlockedInternal", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _GrabBlockedInternal;

		private readonly List<int> _ignoreItemsCollisionRequests = new List<int>();

		private float _originalMass;

		private IGrabPassengerPolicy _passengerPolicy;

		private bool _passengerPolicyResolved;

		private static readonly List<int> _replicateScratch = new List<int>();

		[field: SerializeField]
		public GrabDistanceType GrabDistanceType { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe bool IsReadyToQuotaInternal
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SimplePointGrabable.IsReadyToQuotaInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SimplePointGrabable.IsReadyToQuotaInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		public bool IsReadyToQuota
		{
			get
			{
				if (!Initialized)
				{
					return false;
				}
				return IsReadyToQuotaInternal;
			}
			set
			{
				IsReadyToQuotaInternal = value;
			}
		}

		public bool IsMainRagdollGrabable => _isMainRagdollGrabable;

		public bool ResetPhysicsOnCart => _resetPhysicsOnCart;

		public bool WeightResetOnCart => _weightResetOnCart;

		public bool IgnoredByCart => _ignoredByCart;

		public List<SimplePointGrabable> ConnectedGrabables => _connectedGrabables.ToList();

		public float CartMass => _cartMass;

		public float OriginalMass => _originalMass;

		public bool Initialized { get; private set; }

		public bool CanGrabInStun => _canGrabInStun;

		public InteractableBase Interactable => _interactableBase;

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe bool IsAuthorityRequested
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SimplePointGrabable.IsAuthorityRequested. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SimplePointGrabable.IsAuthorityRequested. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe bool GrabBlockedInternal
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SimplePointGrabable.GrabBlockedInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SimplePointGrabable.GrabBlockedInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 2) = new NetworkBool(value);
			}
		}

		public bool GrabBlocked
		{
			get
			{
				if (!Initialized)
				{
					return false;
				}
				return GrabBlockedInternal;
			}
			set
			{
				if (Initialized)
				{
					GrabBlockedInternal = value;
				}
			}
		}

		public bool InCart { get; set; }

		public List<GameObject> Carts { get; set; } = new List<GameObject>();

		public bool LocalGrabBlocked { get; set; }

		public ArmConfiguration ArmConfiguration => _armConfiguration;

		public GrabObjectBase GrabObject => _grabObject;

		public bool FreezeRotationOnGrab => _freezeRotationOnGrab;

		[field: SerializeField]
		public bool IsReturnsAuthorityToHost { get; private set; }

		public bool IgnoreItemsCollision => _ignoreItemsCollisionRequests.Count > 0;

		public GrabbableStaticType GrabbableStaticType => _grabbableStaticType;

		public bool ChangeRigidbodyKinematic
		{
			get
			{
				return _changeRigibodyKinematic;
			}
			set
			{
				_changeRigibodyKinematic = value;
			}
		}

		[field: SerializeField]
		public bool IsHeavyItem { get; private set; }

		[field: SerializeField]
		public bool IsAutoGrabEnabled { get; private set; } = true;

		[field: SerializeField]
		public float HeavyItemMultiplier { get; set; } = 1f;

		[field: SerializeField]
		public bool DisableGrabWhileHoldingOther { get; set; }

		public bool IsGrabPriority { get; set; }

		public NetworkObject NetworkObject => base.Object;

		public Rigidbody Rigidbody => _rigidbody;

		public GameObject GameObject => base.gameObject;

		public List<Transform> Handles
		{
			get
			{
				return _handles;
			}
			set
			{
				_handles = value;
			}
		}

		public Outline Outline
		{
			get
			{
				return _outline;
			}
			set
			{
				_outline = value;
				this.OnOutlineUpdated?.Invoke();
			}
		}

		public float Weight => _weight;

		public PhysicsResolutionController PhysicsResolutionController => _physicsResolutionController;

		private IGrabPassengerPolicy PassengerPolicy
		{
			get
			{
				if (!_passengerPolicyResolved)
				{
					_passengerPolicy = GetComponent<IGrabPassengerPolicy>();
					_passengerPolicyResolved = true;
				}
				return _passengerPolicy;
			}
		}

		public List<int> GrabbedByPlayers => _grabbedByPlayers;

		public List<int> GrabbedByExternals => _grabbedByExternals;

		public int GrabbedByPlayersCount => _connectedGrabables.Sum((SimplePointGrabable grabbable) => grabbable.GrabbedByPlayers.Count) + GrabbedByPlayers.Count;

		public int GrabbedByExternalsCount => _grabbedByExternals.Count;

		public int GrabbedBySomethingCount => GrabObject.Grabbers.Count;

		public event Action<int> LocalOnGrab;

		public event Action LocalOnUnGrab;

		public event Action<int, int> LocalOnExternalGrab;

		public event Action LocalOnExternalUnGrab;

		public event Action OnOutlineUpdated;

		public event Action OnGrab;

		public event Action OnUnGrab;

		public event Action OnCleanup;

		public event Action OnGrabbedPlayersChanged;

		public event Action OnGrabbedExternalsChanged;

		public event Action<bool> OnTipsChanged;

		public void SetConnectedGrabables(SimplePointGrabable[] connectedGrabables)
		{
			_connectedGrabables = connectedGrabables;
		}

		public void SetHeavyItem(bool isHeavy)
		{
			IsHeavyItem = isHeavy;
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			if (_outline != null)
			{
				_outline.enabled = false;
			}
			if (_tipsReactor != null)
			{
				_tipsReactor.OnTipsChanged += InvokeTipsChanged;
			}
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			if (_tipsReactor != null)
			{
				_tipsReactor.OnTipsChanged -= InvokeTipsChanged;
			}
		}

		private void InvokeTipsChanged()
		{
			this.OnTipsChanged?.Invoke(_tipsReactor != null && _tipsReactor.RepaintAllTipsOnChange);
		}

		public override void Spawned()
		{
			_originalMass = Rigidbody.mass;
			Initialized = true;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			Initialized = false;
			Cleanup();
		}

		private void Start()
		{
			Physics.SyncTransforms();
		}

		public void EnableOutline(bool enable)
		{
			if (_outline != null && GrabbedByPlayers.Count < _availablePlayerCount && !_isOutlineLocked)
			{
				_outline.enabled = enable;
			}
			_lastSetOutline = enable;
		}

		public void LockOutline(bool locked)
		{
			_isOutlineLocked = locked;
			if (!locked)
			{
				EnableOutline(_lastSetOutline);
			}
		}

		public void ForceOutline(bool enable)
		{
			EnableOutline(enable);
			LockOutline(enable);
		}

		public void InvokeOnGrab()
		{
			this.OnGrab?.Invoke();
		}

		public void InvokeOnUnGrab()
		{
			this.OnUnGrab?.Invoke();
		}

		public void GrabbedByPlayer(int playerId)
		{
			if (!(base.Object == null) && GrabbedByPlayers.Count < _availablePlayerCount)
			{
				GrabbedByPlayerRPC(playerId);
			}
		}

		public void UnGrabbedByPlayer(int playerId)
		{
			if (!Initialized)
			{
				GrabbedByPlayers.Remove(playerId);
				this.OnGrabbedPlayersChanged?.Invoke();
			}
			else
			{
				UnGrabbedByPlayerRPC(playerId);
			}
		}

		public void GrabbedByExternal(int holderId, int authorityPlayerId)
		{
			if (!(base.Object == null) && _grabbedByExternals.Count < _availableExternalCount)
			{
				GrabbedByExternalRPC(holderId, authorityPlayerId);
			}
		}

		public void UnGrabbedByExternal(int holderId)
		{
			if (!Initialized)
			{
				this.OnGrabbedExternalsChanged?.Invoke();
			}
			else
			{
				UnGrabbedByExternalRPC(holderId);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2919647707u)]
		public void BlockGrabRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2919647707u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.SimplePointGrabable::BlockGrabRPC()", invokeInfo, PlayerRef.None);
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
			if (base.HasStateAuthority)
			{
				GrabBlocked = true;
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2125246275u)]
		public void EnableGrabRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2125246275u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.SimplePointGrabable::EnableGrabRPC()", invokeInfo, PlayerRef.None);
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
			if (base.HasStateAuthority)
			{
				GrabBlocked = false;
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2088633928u)]
		public void GrabbedByPlayerRPC([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2088633928u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.SimplePointGrabable::GrabbedByPlayerRPC(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			foreach (GameObject item in _gameObjectsToDeactivateOnFirstGrab)
			{
				item.SetActive(value: false);
			}
			if (base.Runner.LocalPlayer.PlayerId == playerId && _isWithChangeStateAuthority && _grabbableStaticType == GrabbableStaticType.NonStatic && ShouldTakeAuthorityOnGrab(playerId))
			{
				_networkObjectComposite.RequestStateAuthority();
			}
			GrabbedByPlayers.Add(playerId);
			this.OnGrabbedPlayersChanged?.Invoke();
			this.LocalOnGrab?.Invoke(playerId);
		}

		private bool ShouldTakeAuthorityOnGrab(int playerId)
		{
			IGrabPassengerPolicy passengerPolicy = PassengerPolicy;
			if (passengerPolicy == null)
			{
				return GrabbedByPlayersCount == 0;
			}
			if (passengerPolicy.IsPassengerGrabber(playerId))
			{
				return false;
			}
			int playerId2 = base.Object.StateAuthority.PlayerId;
			if (playerId2 != playerId)
			{
				return !_grabbedByPlayers.Contains(playerId2);
			}
			return true;
		}

		private void FixedUpdate()
		{
			if (!Initialized)
			{
				return;
			}
			RestoreOriginalMassInHand();
			if (!_changeRigibodyKinematic || _rigidbody == null)
			{
				return;
			}
			if (_grabbableStaticType != GrabbableStaticType.NonStatic)
			{
				if (!_rigidbody.isKinematic)
				{
					_rigidbody.linearVelocity = Vector3.zero;
					_rigidbody.angularVelocity = Vector3.zero;
					_rigidbody.isKinematic = true;
				}
			}
			else
			{
				bool flag = !base.HasStateAuthority;
				if (_rigidbody.isKinematic != flag)
				{
					_rigidbody.isKinematic = flag;
				}
			}
		}

		private void RestoreOriginalMassInHand()
		{
			if (_weightResetOnCart && !(_rigidbody == null) && !(_originalMass <= 0f) && (GrabbedByPlayers.Count > 0 || (_grabObject != null && _grabObject.Grabbers.Count > 0)) && !Mathf.Approximately(_rigidbody.mass, _originalMass))
			{
				_rigidbody.mass = _originalMass;
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1844558790u)]
		public void RequestStateAuthorityRPC([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1844558790u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.SimplePointGrabable::RequestStateAuthorityRPC(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_isWithChangeStateAuthority && _grabbableStaticType == GrabbableStaticType.NonStatic && base.Runner.LocalPlayer.PlayerId == playerId)
			{
				_networkObjectComposite.RequestStateAuthority();
			}
		}

		public void AddIgnoreItemsCollisionRequest(int requestOwnerID)
		{
			if (!_ignoreItemsCollisionRequests.Contains(requestOwnerID))
			{
				_ignoreItemsCollisionRequests.Add(requestOwnerID);
			}
		}

		public void RemoveIgnoreItemsCollisionRequest(int requestOwnerID)
		{
			if (_ignoreItemsCollisionRequests.Contains(requestOwnerID))
			{
				_ignoreItemsCollisionRequests.Remove(requestOwnerID);
			}
		}

		public void SuppressCollisionDamageFor(float seconds)
		{
			if (TryGetComponent<ICollisionDamageSuppressible>(out var component))
			{
				component.SuppressCollisionDamageFor(seconds);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3479374531u)]
		public void UnGrabbedByPlayerRPC([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3479374531u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.SimplePointGrabable::UnGrabbedByPlayerRPC(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			GrabbedByPlayers.Remove(playerId);
			this.OnGrabbedPlayersChanged?.Invoke();
			this.LocalOnUnGrab?.Invoke();
			if (GrabbedByPlayers.Count != 0 && base.Object.HasStateAuthority && _isWithChangeStateAuthority && _grabbableStaticType == GrabbableStaticType.NonStatic)
			{
				AssignStateAuthorityRpc(GrabbedByPlayers[0]);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2051468795u)]
		public void GrabbedByExternalRPC([RpcPayload(4)] int holderId, [RpcPayload(4)] int authorityPlayerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2051468795u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.SimplePointGrabable::GrabbedByExternalRPC(System.Int32,System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(holderId, 4);
						writer.Write(authorityPlayerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (!_grabbedByExternals.Contains(holderId))
			{
				_grabbedByExternals.Add(holderId);
				this.OnGrabbedExternalsChanged?.Invoke();
				if (base.Runner.LocalPlayer.PlayerId == authorityPlayerId && GrabbedByPlayersCount == 0 && _isWithChangeStateAuthority && _grabbableStaticType == GrabbableStaticType.NonStatic)
				{
					_networkObjectComposite.RequestStateAuthority();
				}
				this.LocalOnExternalGrab?.Invoke(holderId, authorityPlayerId);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 694078033u)]
		public void UnGrabbedByExternalRPC([RpcPayload(4)] int holderId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(694078033u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.SimplePointGrabable::UnGrabbedByExternalRPC(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(holderId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_grabbedByExternals.Remove(holderId);
			this.OnGrabbedExternalsChanged?.Invoke();
			this.LocalOnExternalUnGrab?.Invoke();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2650317478u)]
		public void AssignStateAuthorityRpc([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2650317478u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.SimplePointGrabable::AssignStateAuthorityRpc(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (base.Runner.LocalPlayer.PlayerId == playerId && (PassengerPolicy == null || !PassengerPolicy.IsPassengerGrabber(playerId)))
			{
				_networkObjectComposite.RequestStateAuthority();
			}
		}

		public void Cleanup()
		{
			EnableOutline(enable: false);
			Carts.Clear();
			this.OnCleanup?.Invoke();
		}

		public Transform GetNearestHandle(Vector3 position)
		{
			Transform result = null;
			float num = float.MaxValue;
			foreach (Transform handle in Handles)
			{
				if (!(handle == null))
				{
					float num2 = Vector3.Distance(position, handle.position);
					if (num2 < num)
					{
						num = num2;
						result = handle;
					}
				}
			}
			return result;
		}

		public List<TipType> GetTips()
		{
			if (!(_tipsReactor != null))
			{
				return _allTips;
			}
			return new List<TipType>(_tipsReactor.GetTips());
		}

		public List<TipType> GetRaycastTips()
		{
			if (!(_tipsReactor != null))
			{
				return _allRaycastTips;
			}
			return new List<TipType>(_tipsReactor.GetRaycastTips());
		}

		public void SetPhysicsMaterialToColliders(PhysicsMaterial physicMaterialOnGrab)
		{
			if (_canOverridePhysicMaterial)
			{
				Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].material = physicMaterialOnGrab;
				}
			}
		}

		public void InterestEnter(PlayerRef player)
		{
			ReplicateRemoteStateRpc(base.Runner.LocalPlayer);
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 3371082324u)]
		private void ReplicateRemoteStateRpc([RpcPayload(4)] PlayerRef requestPeer)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3371082324u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.SimplePointGrabable::ReplicateRemoteStateRpc(Fusion.PlayerRef)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(requestPeer, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (GrabbedByPlayers.Count == 0)
			{
				ReplicateGrabbedByPlayersEmptyRpc(requestPeer);
			}
			else
			{
				ReplicateGrabbedByPlayersRpc(requestPeer, GrabbedByPlayers.Select((int x) => (ushort)x).ToArray());
			}
			if (_grabbedByExternals.Count == 0)
			{
				ReplicateGrabbedByExternalsEmptyRpc(requestPeer);
			}
			else
			{
				ReplicateGrabbedByExternalsRpc(requestPeer, _grabbedByExternals.ToArray());
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3628732815u)]
		private void ReplicateGrabbedByPlayersRpc([RpcTarget] PlayerRef player, [RpcPayload(2)] ushort[] grabbedByPlayers)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(grabbedByPlayers.Length, 2);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3628732815u, bytePayloadSize, (NetworkBehaviour)this, player))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.SimplePointGrabable::ReplicateGrabbedByPlayersRpc(Fusion.PlayerRef,System.UInt16[])", invokeInfo, player);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(grabbedByPlayers, 2);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, player));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			ApplyReplicatedGrabbedByPlayers(grabbedByPlayers);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3348186660u)]
		private void ReplicateGrabbedByPlayersEmptyRpc([RpcTarget] PlayerRef player)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3348186660u, 0, (NetworkBehaviour)this, player))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.SimplePointGrabable::ReplicateGrabbedByPlayersEmptyRpc(Fusion.PlayerRef)", invokeInfo, player);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, player));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			ApplyReplicatedGrabbedByPlayers(Array.Empty<ushort>());
		}

		private void ApplyReplicatedGrabbedByPlayers(ushort[] grabbedByPlayers)
		{
			int playerId = base.Runner.LocalPlayer.PlayerId;
			bool flag = GrabbedByPlayers.Contains(playerId);
			_replicateScratch.Clear();
			foreach (ushort item in grabbedByPlayers)
			{
				_replicateScratch.Add(item);
			}
			if (flag && !_replicateScratch.Contains(playerId))
			{
				_replicateScratch.Add(playerId);
			}
			bool flag2 = _replicateScratch.Count != GrabbedByPlayers.Count;
			if (!flag2)
			{
				for (int j = 0; j < _replicateScratch.Count; j++)
				{
					if (_replicateScratch[j] != GrabbedByPlayers[j])
					{
						flag2 = true;
						break;
					}
				}
			}
			if (flag2)
			{
				GrabbedByPlayers.Clear();
				GrabbedByPlayers.AddRange(_replicateScratch);
				this.OnGrabbedPlayersChanged?.Invoke();
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1306461198u)]
		private void ReplicateGrabbedByExternalsRpc([RpcTarget] PlayerRef player, [RpcPayload(4)] int[] grabbedByExternals)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(grabbedByExternals.Length, 4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1306461198u, bytePayloadSize, (NetworkBehaviour)this, player))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.SimplePointGrabable::ReplicateGrabbedByExternalsRpc(Fusion.PlayerRef,System.Int32[])", invokeInfo, player);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(grabbedByExternals, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, player));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_grabbedByExternals.Clear();
			foreach (int item in grabbedByExternals)
			{
				_grabbedByExternals.Add(item);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2495452406u)]
		private void ReplicateGrabbedByExternalsEmptyRpc([RpcTarget] PlayerRef player)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2495452406u, 0, (NetworkBehaviour)this, player))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.SimplePointGrabable::ReplicateGrabbedByExternalsEmptyRpc(Fusion.PlayerRef)", invokeInfo, player);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, player));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_grabbedByExternals.Clear();
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsReadyToQuotaInternal = _IsReadyToQuotaInternal;
			IsAuthorityRequested = _IsAuthorityRequested;
			GrabBlockedInternal = _GrabBlockedInternal;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsReadyToQuotaInternal = IsReadyToQuotaInternal;
			_IsAuthorityRequested = IsAuthorityRequested;
			_GrabBlockedInternal = GrabBlockedInternal;
		}

		[NetworkRpcWeavedInvoker(2919647707u)]
		[Preserve]
		[WeaverGenerated]
		protected static void BlockGrabRPC_0040Invoker2919647707([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimplePointGrabable)context.TargetBehaviour).BlockGrabRPC();
		}

		[NetworkRpcWeavedInvoker(2125246275u)]
		[Preserve]
		[WeaverGenerated]
		protected static void EnableGrabRPC_0040Invoker2125246275([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimplePointGrabable)context.TargetBehaviour).EnableGrabRPC();
		}

		[NetworkRpcWeavedInvoker(2088633928u)]
		[Preserve]
		[WeaverGenerated]
		protected static void GrabbedByPlayerRPC_0040Invoker2088633928([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimplePointGrabable)context.TargetBehaviour).GrabbedByPlayerRPC(value);
		}

		[NetworkRpcWeavedInvoker(1844558790u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RequestStateAuthorityRPC_0040Invoker1844558790([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimplePointGrabable)context.TargetBehaviour).RequestStateAuthorityRPC(value);
		}

		[NetworkRpcWeavedInvoker(3479374531u)]
		[Preserve]
		[WeaverGenerated]
		protected static void UnGrabbedByPlayerRPC_0040Invoker3479374531([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimplePointGrabable)context.TargetBehaviour).UnGrabbedByPlayerRPC(value);
		}

		[NetworkRpcWeavedInvoker(2051468795u)]
		[Preserve]
		[WeaverGenerated]
		protected static void GrabbedByExternalRPC_0040Invoker2051468795([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out int value, 4);
			payloadReader.Read(out int value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimplePointGrabable)context.TargetBehaviour).GrabbedByExternalRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(694078033u)]
		[Preserve]
		[WeaverGenerated]
		protected static void UnGrabbedByExternalRPC_0040Invoker694078033([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimplePointGrabable)context.TargetBehaviour).UnGrabbedByExternalRPC(value);
		}

		[NetworkRpcWeavedInvoker(2650317478u)]
		[Preserve]
		[WeaverGenerated]
		protected static void AssignStateAuthorityRpc_0040Invoker2650317478([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimplePointGrabable)context.TargetBehaviour).AssignStateAuthorityRpc(value);
		}

		[NetworkRpcWeavedInvoker(3371082324u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ReplicateRemoteStateRpc_0040Invoker3371082324([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out PlayerRef value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimplePointGrabable)context.TargetBehaviour).ReplicateRemoteStateRpc(value);
		}

		[NetworkRpcWeavedInvoker(3628732815u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ReplicateGrabbedByPlayersRpc_0040Invoker3628732815([In] ref RpcInvokeContext context)
		{
			PlayerRef targetPlayer = context.TargetPlayer;
			context.PayloadReader.Read(out ushort[] value, 2);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimplePointGrabable)context.TargetBehaviour).ReplicateGrabbedByPlayersRpc(targetPlayer, value);
		}

		[NetworkRpcWeavedInvoker(3348186660u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ReplicateGrabbedByPlayersEmptyRpc_0040Invoker3348186660([In] ref RpcInvokeContext context)
		{
			PlayerRef targetPlayer = context.TargetPlayer;
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimplePointGrabable)context.TargetBehaviour).ReplicateGrabbedByPlayersEmptyRpc(targetPlayer);
		}

		[NetworkRpcWeavedInvoker(1306461198u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ReplicateGrabbedByExternalsRpc_0040Invoker1306461198([In] ref RpcInvokeContext context)
		{
			PlayerRef targetPlayer = context.TargetPlayer;
			context.PayloadReader.Read(out int[] value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimplePointGrabable)context.TargetBehaviour).ReplicateGrabbedByExternalsRpc(targetPlayer, value);
		}

		[NetworkRpcWeavedInvoker(2495452406u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ReplicateGrabbedByExternalsEmptyRpc_0040Invoker2495452406([In] ref RpcInvokeContext context)
		{
			PlayerRef targetPlayer = context.TargetPlayer;
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimplePointGrabable)context.TargetBehaviour).ReplicateGrabbedByExternalsEmptyRpc(targetPlayer);
		}
	}
}
