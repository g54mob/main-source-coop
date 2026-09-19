using System;
using System.Reflection;
using System.Runtime.InteropServices;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.ParticleSpawnModule.Scripts;
using Features.ScreenShakeModule.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.ItemsModule.Scripts
{
	[NetworkBehaviourWeaved(6)]
	public class MonoItem : NetworkBehaviour, IItem, IPoolableObject
	{
		[SerializeField]
		private ItemConfig _itemConfig;

		[SerializeField]
		private Transform _pricePointer;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private bool _isNeedToShowPrice = true;

		[SerializeField]
		private ParticleSpawner _particleSpawner;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private bool _isSpawned;

		private bool _isConsumed;

		private bool _isDespawned;

		private bool _isInitialized;

		private bool _isBrokenIntoParts;

		private bool _initialized;

		private IScreenShakeService _screenShakeService;

		private SpawnedItemsModel _spawnedItemsModel;

		private IAudioService _audioService;

		[field: SerializeField]
		public EventReference CollisionSound { get; private set; }

		[field: SerializeField]
		public CinemachineImpulseSource CinemachineImpulseSource { get; private set; }

		[field: SerializeField]
		public ScreenShakeData ScreenShakeData { get; private set; }

		[field: SerializeField]
		public ScreenShakeData ScreenShakeDataOnDestroy { get; private set; }

		[field: SerializeField]
		public ParticleSystem CollisionParticle { get; private set; }

		[field: SerializeField]
		public bool IsSoundOnAnyCollision { get; private set; }

		public bool IsNeedToShowPrice
		{
			get
			{
				return _isNeedToShowPrice;
			}
			set
			{
				_isNeedToShowPrice = value;
			}
		}

		[UnityNonSerialized]
		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe ItemType TypeInternal
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonoItem.TypeInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((ItemType*)Ptr)[0];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonoItem.TypeInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				((ItemType*)Ptr)[0] = value;
			}
		}

		[UnityNonSerialized]
		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe ushort CurrencyValueInternal
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonoItem.CurrencyValueInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((ushort*)Ptr)[2];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonoItem.CurrencyValueInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				((short*)Ptr)[2] = (short)value;
			}
		}

		[UnityNonSerialized]
		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe ushort MaxCurrencyValue
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonoItem.MaxCurrencyValue. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((ushort*)Ptr)[4];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonoItem.MaxCurrencyValue. Networked properties can only be accessed when Spawned() has been called.");
				}
				((short*)Ptr)[4] = (short)value;
			}
		}

		[UnityNonSerialized]
		[Networked]
		[NetworkedWeaved(3, 1)]
		public unsafe bool IsCollectable
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonoItem.IsCollectable. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 3);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonoItem.IsCollectable. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 3) = new NetworkBool(value);
			}
		}

		[UnityNonSerialized]
		[Networked(AllowPrediction = true)]
		[NetworkedWeaved(4, 1)]
		public unsafe int LastGrabTikRaw
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonoItem.LastGrabTikRaw. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[4];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonoItem.LastGrabTikRaw. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[4] = value;
			}
		}

		[UnityNonSerialized]
		[Networked]
		[NetworkedWeaved(5, 1)]
		public unsafe bool AreDespawnEffectsSuppressed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonoItem.AreDespawnEffectsSuppressed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 5);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MonoItem.AreDespawnEffectsSuppressed. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 5) = new NetworkBool(value);
			}
		}

		public ItemType Type
		{
			get
			{
				if (!IsSpawned)
				{
					return ItemType.None;
				}
				return TypeInternal;
			}
			set
			{
				TypeInternal = value;
			}
		}

		public ushort CurrencyValue
		{
			get
			{
				if (!_isSpawned)
				{
					return 0;
				}
				return CurrencyValueInternal;
			}
			private set
			{
				CurrencyValueInternal = value;
			}
		}

		public bool AvailableForEnemy { get; set; } = true;

		public NetworkObject NetworkObject => base.Object;

		public bool IsDespawned => _isDespawned;

		public bool IsConsumed => _isConsumed;

		public bool IsSpawned => _isSpawned;

		public ItemConfig DefaultConfig => _itemConfig;

		public bool IsReducible => _itemConfig.IsReducible;

		public float DamageOnCollide => _itemConfig.DamageOnCollide;

		public ITransformBasedSoundSource SoundSource => _soundSourceBehaviour;

		public event Action<IItem> OnSpawn;

		public event Action<IItem> OnDespawn;

		[Inject]
		public void InjectDependencies(IScreenShakeService screenShakeService, SpawnedItemsModel spawnedItemsModel, IAudioService audioService)
		{
			_screenShakeService = screenShakeService;
			_spawnedItemsModel = spawnedItemsModel;
			_audioService = audioService;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (Type == ItemType.None && _itemConfig != null && base.HasStateAuthority)
			{
				Initialize(new ItemData(_itemConfig));
			}
			if (base.HasStateAuthority)
			{
				SetDespawnEffectsSuppressed(suppressed: false);
			}
			_isSpawned = true;
			_spawnedItemsModel.Register(this);
			this.OnSpawn?.Invoke(this);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (!AreDespawnEffectsSuppressed && CurrencyValue <= 0 && (!(_itemConfig != null) || !_itemConfig.CanDestroyWithoutMoney))
			{
				_screenShakeService.TriggerScreenShake(CinemachineImpulseSource, ScreenShakeDataOnDestroy);
			}
			_isDespawned = true;
			_spawnedItemsModel.Unregister(this);
			this.OnDespawn?.Invoke(this);
		}

		public void Initialize(ItemData itemData)
		{
			if (itemData != null && base.HasStateAuthority && !_isInitialized)
			{
				ApplyItemData(itemData);
				_isInitialized = true;
			}
		}

		public void ForceInitialize(ItemData itemData)
		{
			if (itemData != null && base.HasStateAuthority)
			{
				ApplyItemData(itemData);
				_isInitialized = true;
			}
		}

		private void ApplyItemData(ItemData itemData)
		{
			Type = itemData.ItemType;
			CurrencyValue = (ushort)itemData.CurrencyValue;
			MaxCurrencyValue = (ushort)itemData.MaxCurrencyValue;
			IsCollectable = itemData.IsCollectable;
			LastGrabTikRaw = 0;
		}

		public void SetCurrencyValue(int newValue)
		{
			if (base.Object.HasStateAuthority)
			{
				CurrencyValue = (ushort)newValue;
			}
			else
			{
				SetCurrencyValueRPC((ushort)newValue);
			}
		}

		public void SetLastGrabTime(Tick grabTime)
		{
			LastGrabTikRaw = new Tick
			{
				Raw = grabTime
			};
		}

		public void SetIsCollectable(bool isTakeable)
		{
			IsCollectable = isTakeable;
		}

		public void Consume()
		{
			ConsumeRPC();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 843420332u)]
		private void ConsumeRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(843420332u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.ItemsModule.Scripts.MonoItem::ConsumeRPC()", invokeInfo, PlayerRef.None);
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
			if (base.HasStateAuthority && !_isConsumed)
			{
				_isConsumed = true;
				if (_isSpawned)
				{
					SetIsCollectable(isTakeable: false);
				}
				DespawnHierarchy();
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 4252953782u)]
		private void SetCurrencyValueRPC([RpcPayload(2)] ushort newValue)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(2);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4252953782u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.ItemsModule.Scripts.MonoItem::SetCurrencyValueRPC(System.UInt16)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(newValue, 2);
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
				CurrencyValue = newValue;
			}
		}

		public ItemCollisionConfig GetCollisionConfig()
		{
			return _itemConfig?.CollisionConfig;
		}

		public void OnSpawned()
		{
		}

		public void OnDespawned()
		{
		}

		public void OnActivated()
		{
		}

		public void OnDeactivated()
		{
			_isSpawned = false;
			_isConsumed = false;
			_isDespawned = false;
			_isBrokenIntoParts = false;
		}

		public Vector3 GetPricePosition()
		{
			if (!(_pricePointer != null))
			{
				return base.transform.position;
			}
			return _pricePointer.position;
		}

		public void DespawnItem()
		{
			DespawnRPC();
		}

		public void BreakItem()
		{
			BreakRPC();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 755116291u)]
		private void BreakRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(755116291u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.ItemsModule.Scripts.MonoItem::BreakRPC()", invokeInfo, PlayerRef.None);
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
			if (!_isBrokenIntoParts)
			{
				_isBrokenIntoParts = true;
				PlayBreakSound();
				if (base.HasStateAuthority)
				{
					SpawnBreakParts();
					DespawnHierarchy();
				}
			}
		}

		private void PlayBreakSound()
		{
			if (_audioService != null && TryGetComponent<ItemBreakParts>(out var component) && component.IsEnabled && !component.BreakSound.IsNull)
			{
				if (_soundSourceBehaviour != null)
				{
					_audioService.PlayOneShot(component.BreakSound, _soundSourceBehaviour);
				}
				else
				{
					_audioService.PlayOneShot(component.BreakSound, new GenericSoundSource(base.transform.position, base.transform.GetInstanceID()));
				}
			}
		}

		private void SpawnBreakParts()
		{
			if (!TryGetComponent<ItemBreakParts>(out var component) || !component.IsEnabled)
			{
				return;
			}
			foreach (ItemBreakPartData part in component.Parts)
			{
				if (!(part.PartPrefab == null))
				{
					Vector3 value = base.transform.TransformPoint(part.LocalPosition);
					Quaternion value2 = base.transform.rotation * Quaternion.Euler(part.LocalRotation);
					try
					{
						base.Runner.Spawn(part.PartPrefab, value, value2, base.Runner.LocalPlayer);
					}
					catch (NetworkObjectSpawnException)
					{
					}
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2751844216u)]
		private void DespawnRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2751844216u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.ItemsModule.Scripts.MonoItem::DespawnRPC()", invokeInfo, PlayerRef.None);
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
				DespawnHierarchy();
			}
		}

		public void SetDespawnEffectsSuppressed(bool suppressed)
		{
			if (base.HasStateAuthority)
			{
				AreDespawnEffectsSuppressed = suppressed;
				if (_particleSpawner != null)
				{
					_particleSpawner.SetDespawnEffectsSuppressed(suppressed);
				}
			}
		}

		private void DespawnHierarchy()
		{
			base.Object.DespawnHierarchy();
		}

		public void AddForce(float force, Vector3 direction, ForceMode forceMode)
		{
			Vector3 vector = direction * force;
			AddForceRPC(vector, (byte)forceMode);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 316829032u)]
		private void AddForceRPC([RpcPayload(12)] Vector3Compressed forceDirection, [RpcPayload(1)] byte forceModeEncoded)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(12);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(1);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(316829032u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.ItemsModule.Scripts.MonoItem::AddForceRPC(Fusion.Vector3Compressed,System.Byte)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(forceDirection, 12);
						writer.Write(forceModeEncoded, 1);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_rigidbody != null)
			{
				_rigidbody.AddForce(forceDirection, (ForceMode)forceModeEncoded);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(843420332u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ConsumeRPC_0040Invoker843420332([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonoItem)context.TargetBehaviour).ConsumeRPC();
		}

		[NetworkRpcWeavedInvoker(4252953782u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetCurrencyValueRPC_0040Invoker4252953782([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out ushort value, 2);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonoItem)context.TargetBehaviour).SetCurrencyValueRPC(value);
		}

		[NetworkRpcWeavedInvoker(755116291u)]
		[Preserve]
		[WeaverGenerated]
		protected static void BreakRPC_0040Invoker755116291([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonoItem)context.TargetBehaviour).BreakRPC();
		}

		[NetworkRpcWeavedInvoker(2751844216u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DespawnRPC_0040Invoker2751844216([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonoItem)context.TargetBehaviour).DespawnRPC();
		}

		[NetworkRpcWeavedInvoker(316829032u)]
		[Preserve]
		[WeaverGenerated]
		protected static void AddForceRPC_0040Invoker316829032([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out Vector3Compressed value, 12);
			payloadReader.Read(out byte value2, 1);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MonoItem)context.TargetBehaviour).AddForceRPC(value, value2);
		}
	}
}
