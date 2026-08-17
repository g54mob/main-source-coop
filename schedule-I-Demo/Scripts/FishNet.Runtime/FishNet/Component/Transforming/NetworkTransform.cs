using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Connection;
using FishNet.Documenting;
using FishNet.Managing;
using FishNet.Managing.Logging;
using FishNet.Managing.Observing;
using FishNet.Managing.Server;
using FishNet.Object;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using GameKit.Utilities;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;

namespace FishNet.Component.Transforming
{
	[DisallowMultipleComponent]
	[AddComponentMenu("FishNet/Component/NetworkTransform")]
	public sealed class NetworkTransform : NetworkBehaviour
	{
		[Serializable]
		public enum ComponentConfigurationType
		{
			Disabled = 0,
			CharacterController = 1,
			Rigidbody = 2,
			Rigidbody2D = 3
		}

		private struct ReceivedClientData
		{
			public List<bool> HasData;

			public PooledWriter Writer;

			public Channel Channel;

			public void Update(ArraySegment<byte> data, Channel channel, bool updateHasData)
			{
				if (Writer == null)
				{
					Writer = WriterPool.Retrieve();
				}
				Writer.Reset();
				Writer.WriteArraySegment(data);
				Channel = channel;
				if (updateHasData)
				{
					SetHasData(value: true);
				}
			}

			public void SetHasData(bool value)
			{
				for (int i = 0; i < HasData.Count; i++)
				{
					HasData[i] = value;
				}
			}

			public void SetHasData(bool value, byte index)
			{
				if (index < HasData.Count)
				{
					HasData[index] = value;
				}
			}
		}

		[Serializable]
		public struct SnappedAxes
		{
			public bool X;

			public bool Y;

			public bool Z;
		}

		private enum ChangedDelta : uint
		{
			Unset = 0u,
			PositionX = 1u,
			PositionY = 2u,
			PositionZ = 4u,
			Rotation = 8u,
			Extended = 16u,
			ScaleX = 32u,
			ScaleY = 64u,
			ScaleZ = 128u,
			Nested = 256u,
			All = uint.MaxValue
		}

		private enum ChangedFull
		{
			Unset = 0,
			Position = 1,
			Rotation = 2,
			Scale = 4,
			Nested = 8
		}

		private enum UpdateFlagA : byte
		{
			Unset = 0,
			X2 = 1,
			X4 = 2,
			Y2 = 4,
			Y4 = 8,
			Z2 = 0x10,
			Z4 = 0x20,
			Rotation = 0x40,
			Extended = 0x80
		}

		private enum UpdateFlagB : byte
		{
			Unset = 0,
			X2 = 1,
			X4 = 2,
			Y2 = 4,
			Y4 = 8,
			Z2 = 0x10,
			Z4 = 0x20,
			Nested = 0x40
		}

		public class GoalData : IResettable
		{
			public uint ReceivedTick;

			public RateData Rates = new RateData();

			public TransformData Transforms = new TransformData();

			public void ResetState()
			{
				ReceivedTick = 0u;
				Transforms.ResetState();
				Rates.ResetState();
			}

			public void InitializeState()
			{
			}
		}

		public class RateData : IResettable
		{
			public float Position;

			public float Rotation;

			public float Scale;

			public float LastUnalteredPositionRate;

			public uint TickSpan;

			internal bool AbnormalRateDetected;

			internal float TimeRemaining;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Update(RateData rd)
			{
				Update(rd.Position, rd.Rotation, rd.Scale, rd.LastUnalteredPositionRate, rd.TickSpan, rd.AbnormalRateDetected, rd.TimeRemaining);
			}

			public void Update(float position, float rotation, float scale, float unalteredPositionRate, uint tickSpan, bool abnormalRateDetected, float timeRemaining)
			{
				Position = position;
				Rotation = rotation;
				Scale = scale;
				LastUnalteredPositionRate = unalteredPositionRate;
				TickSpan = tickSpan;
				AbnormalRateDetected = abnormalRateDetected;
				TimeRemaining = timeRemaining;
			}

			public void ResetState()
			{
				Position = 0f;
				Rotation = 0f;
				Scale = 0f;
				LastUnalteredPositionRate = 0f;
				TickSpan = 0u;
				AbnormalRateDetected = false;
				TimeRemaining = 0f;
			}

			public void InitializeState()
			{
			}
		}

		public class TransformData : IResettable
		{
			public enum ExtrapolateState : byte
			{
				Disabled = 0,
				Available = 1,
				Active = 2
			}

			public uint Tick;

			public bool SnappingChecked;

			public Vector3 Position;

			public Quaternion Rotation;

			public Vector3 Scale;

			public Vector3 ExtrapolatedPosition;

			public ExtrapolateState ExtrapolationState;

			public NetworkBehaviour ParentBehaviour;

			public bool IsDefault { get; private set; } = true;

			[Preserve]
			public TransformData()
			{
			}

			internal void Update(TransformData copy)
			{
				Update(copy.Tick, copy.Position, copy.Rotation, copy.Scale, copy.ExtrapolatedPosition, copy.ParentBehaviour);
			}

			internal void Update(uint tick, Vector3 position, Quaternion rotation, Vector3 scale, Vector3 extrapolatedPosition, NetworkBehaviour parentBehaviour)
			{
				IsDefault = false;
				Tick = tick;
				Position = position;
				Rotation = rotation;
				Scale = scale;
				ExtrapolatedPosition = extrapolatedPosition;
				ParentBehaviour = parentBehaviour;
			}

			public void ResetState()
			{
				IsDefault = true;
				Tick = 0u;
				SnappingChecked = false;
				Position = Vector3.zero;
				Rotation = Quaternion.identity;
				Scale = Vector3.zero;
				ExtrapolatedPosition = Vector3.zero;
				ExtrapolationState = ExtrapolateState.Disabled;
				ParentBehaviour = null;
			}

			public void InitializeState()
			{
			}
		}

		[APIExclude]
		public delegate void DataReceivedChanged(TransformData prev, TransformData next);

		[Tooltip("Attached movement component to automatically configure.")]
		[SerializeField]
		private ComponentConfigurationType _componentConfiguration;

		[Tooltip("True to synchronize when this transform changes parent.")]
		[SerializeField]
		private bool _synchronizeParent;

		[Tooltip("How much to compress each transform property.")]
		[SerializeField]
		private TransformPackingData _packing = new TransformPackingData
		{
			Position = AutoPackType.Packed,
			Rotation = AutoPackType.Packed,
			Scale = AutoPackType.Unpacked
		};

		[Tooltip("How many ticks to interpolate.")]
		[Range(1f, 250f)]
		[SerializeField]
		private ushort _interpolation = 2;

		[Tooltip("How many ticks to extrapolate.")]
		[Range(0f, 1024f)]
		[SerializeField]
		private ushort _extrapolation = 2;

		[Tooltip("True to enable teleport threshhold.")]
		[SerializeField]
		private bool _enableTeleport;

		[Tooltip("How far the transform must travel in a single update to cause a teleport rather than smoothing. Using 0f will teleport every update.")]
		[Range(0f, float.MaxValue)]
		[SerializeField]
		private float _teleportThreshold = 1f;

		[Tooltip("True to increase the teleport threshhold based on LOD of the object.")]
		[SerializeField]
		private bool _scaleThreshold = true;

		[Tooltip("True if owner controls how the object is synchronized.")]
		[SerializeField]
		private bool _clientAuthoritative = true;

		[Tooltip("True to synchronize movements on server to owner when not using client authoritative movement.")]
		[SerializeField]
		private bool _sendToOwner = true;

		[Tooltip("True to use Network Level of Detail when the feature is enabled.")]
		[FormerlySerializedAs("_useNetworkLod")]
		[SerializeField]
		private bool _enableNetworkLod = true;

		[Tooltip("How often in ticks to synchronize. This is default to 1 but can be set longer to send less often. This value may also be changed at runtime. Enabling Network level of detail for this NetworkTransform disables manual control of this feature as it will be handled internally.")]
		[Range(1f, 255f)]
		[SerializeField]
		private byte _interval = 1;

		[Tooltip("True to synchronize position. Even while checked only changed values are sent.")]
		[SerializeField]
		private bool _synchronizePosition = true;

		[Tooltip("Axes to snap on position.")]
		[SerializeField]
		private SnappedAxes _positionSnapping;

		[Tooltip("True to synchronize rotation. Even while checked only changed values are sent.")]
		[SerializeField]
		private bool _synchronizeRotation = true;

		[Tooltip("Axes to snap on rotation.")]
		[SerializeField]
		private SnappedAxes _rotationSnapping;

		[Tooltip("True to synchronize scale. Even while checked only changed values are sent.")]
		[SerializeField]
		private bool _synchronizeScale = true;

		[Tooltip("Axes to snap on scale.")]
		[SerializeField]
		private SnappedAxes _scaleSnapping;

		private TransformPackingData _unpacked = new TransformPackingData
		{
			Position = AutoPackType.Unpacked,
			Rotation = AutoPackType.Unpacked,
			Scale = AutoPackType.Unpacked
		};

		private bool _lastReceiveReliable = true;

		private NetworkBehaviour _parentBehaviour;

		private Transform _parentTransform;

		private List<ChangedDelta> _serverChangedSinceReliable;

		private ChangedDelta _clientChangedSinceReliable;

		private uint _lastObserversRpcTick;

		private uint _lastServerRpcTick;

		private ReceivedClientData _authoritativeClientData;

		private bool _subscribedToTicks;

		private TransformData _lastReceivedServerTransformData;

		private TransformData _lastReceivedClientTransformData;

		private RateData _lastCalculatedRateData = new RateData();

		private Queue<GoalData> _goalDataQueue = new Queue<GoalData>();

		private GoalData _currentGoalData;

		private bool _changedSinceStart;

		private short _intervalsRemaining;

		private List<TransformData> _lastSentTransformDatas;

		private List<PooledWriter> _toClientChangedWriters;

		private uint _forceSendTick;

		public const ushort MAX_INTERPOLATION = 250;

		private bool NetworkInitialize___EarlyFishNet_002EComponent_002ETransforming_002ENetworkTransformFishNet_002ERuntime_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EComponent_002ETransforming_002ENetworkTransformFishNet_002ERuntime_002Edll_Excuted;

		public bool TakenOwnership { get; private set; }

		private ChangedDelta _fullChanged => ChangedDelta.All;

		public event DataReceivedChanged OnDataReceived;

		public event Action<GoalData> OnNextGoal;

		public event Action OnInterpolationComplete;

		public bool GetSendToOwner()
		{
			return _sendToOwner;
		}

		public void SetSendToOwner(bool value)
		{
			_sendToOwner = value;
			if (base.IsServerInitialized)
			{
				ObserversSetSendToOwner(value);
			}
		}

		public void SetSynchronizePosition(bool value)
		{
			_synchronizePosition = value;
		}

		public void SetPositionSnapping(SnappedAxes axes)
		{
			_positionSnapping = axes;
		}

		public void SetSynchronizeRotation(bool value)
		{
			_synchronizeRotation = value;
		}

		public void SetRotationSnapping(SnappedAxes axes)
		{
			_rotationSnapping = axes;
		}

		public void SetSynchronizeScale(bool value)
		{
			_synchronizeScale = value;
		}

		public void SetScaleSnapping(SnappedAxes axes)
		{
			_scaleSnapping = axes;
		}

		public void Awake()
		{
			NetworkInitialize___Early();
			Awake_UserLogic_FishNet_002EComponent_002ETransforming_002ENetworkTransform_FishNet_002ERuntime_002Edll();
			NetworkInitialize__Late();
		}

		private void OnDestroy()
		{
			ResetState(destroyed: true);
		}

		public override void OnStartNetwork()
		{
			if (_enableNetworkLod && !base.ObserverManager.GetEnableNetworkLod())
			{
				_enableNetworkLod = false;
			}
		}

		public override void OnStartServer()
		{
			_lastReceivedClientTransformData = ObjectCaches<TransformData>.Retrieve();
			ConfigureComponents();
			AddCollections(asServer: true);
			SetDefaultGoalData();
			ChangeTickSubscription(subscribe: true);
		}

		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (base.NetworkObject.gameObject != base.gameObject && _changedSinceStart)
			{
				PooledWriter pooledWriter = WriterPool.Retrieve();
				SerializeChanged(_fullChanged, pooledWriter, 0);
				TargetUpdateTransform(connection, pooledWriter.GetArraySegment(), Channel.Reliable);
				pooledWriter.Store();
			}
		}

		public override void OnStartClient()
		{
			_lastReceivedServerTransformData = ObjectCaches<TransformData>.Retrieve();
			ConfigureComponents();
			AddCollections(asServer: false);
			SetDefaultGoalData();
		}

		public override void OnOwnershipServer(NetworkConnection prevOwner)
		{
			_intervalsRemaining = 0;
			_lastServerRpcTick = 0u;
		}

		public override void OnOwnershipClient(NetworkConnection prevOwner)
		{
			_intervalsRemaining = 0;
			if (base.IsOwner)
			{
				ChangeTickSubscription(subscribe: true);
				return;
			}
			if (_clientAuthoritative)
			{
				SetDefaultGoalData();
			}
			if (!base.IsServerInitialized)
			{
				ChangeTickSubscription(subscribe: false);
			}
		}

		public override void OnStopNetwork()
		{
			ResetState(destroyed: false);
		}

		private void ResetState(bool destroyed)
		{
			ChangeTickSubscription(subscribe: false);
			ObjectCaches<PooledWriter>.StoreAndDefault(ref _authoritativeClientData.Writer);
			if (_toClientChangedWriters != null)
			{
				foreach (PooledWriter toClientChangedWriter in _toClientChangedWriters)
				{
					WriterPool.Store(toClientChangedWriter);
				}
			}
			CollectionCaches<PooledWriter>.StoreAndDefault(ref _toClientChangedWriters);
			CollectionCaches<bool>.StoreAndDefault(ref _authoritativeClientData.HasData);
			CollectionCaches<ChangedDelta>.StoreAndDefault(ref _serverChangedSinceReliable);
			ResettableObjectCaches<TransformData>.StoreAndDefault(ref _lastReceivedClientTransformData);
			ResettableObjectCaches<TransformData>.StoreAndDefault(ref _lastReceivedServerTransformData);
			while (_goalDataQueue.Count > 0)
			{
				ResettableObjectCaches<GoalData>.Store(_goalDataQueue.Dequeue());
			}
			ResettableCollectionCaches<TransformData>.StoreAndDefault(ref _lastSentTransformDatas);
			ResettableObjectCaches<GoalData>.StoreAndDefault(ref _currentGoalData);
		}

		private void Update()
		{
			MoveToTarget();
		}

		private void AddCollections(bool asServer)
		{
			bool flag = !asServer && !base.IsServer;
			if (asServer || flag)
			{
				if (_toClientChangedWriters == null)
				{
					_toClientChangedWriters = CollectionCaches<PooledWriter>.RetrieveList();
				}
				else if (_toClientChangedWriters.Count > 0)
				{
					base.NetworkManager.LogWarning("_toClientChangedWriters contains values when it should not.");
				}
				if (_lastSentTransformDatas == null)
				{
					_lastSentTransformDatas = ResettableCollectionCaches<TransformData>.RetrieveList();
				}
				else if (_lastSentTransformDatas.Count > 0)
				{
					base.NetworkManager.LogWarning(string.Format("{0} contains values when it should not. Hash {1}", "_lastSentTransformDatas", _lastSentTransformDatas.GetHashCode()));
				}
			}
			if (asServer)
			{
				int count = base.ObserverManager.GetLevelOfDetailDistances().Count;
				if (_authoritativeClientData.HasData == null)
				{
					_authoritativeClientData.HasData = CollectionCaches<bool>.RetrieveList();
				}
				else if (_authoritativeClientData.HasData.Count > 0)
				{
					base.NetworkManager.LogWarning("HasData contains values when it should not.");
				}
				if (_serverChangedSinceReliable == null)
				{
					_serverChangedSinceReliable = CollectionCaches<ChangedDelta>.RetrieveList();
				}
				else if (_serverChangedSinceReliable.Count > 0)
				{
					base.NetworkManager.LogWarning("_serverChangedSinceReliable contains values when it should not.");
				}
				for (int i = 0; i < count; i++)
				{
					_toClientChangedWriters.Add(WriterPool.Retrieve());
					TransformData item = ResettableObjectCaches<TransformData>.Retrieve();
					_lastSentTransformDatas.Add(item);
					if (asServer)
					{
						_authoritativeClientData.HasData.Add(item: false);
						_serverChangedSinceReliable.Add(ChangedDelta.Unset);
					}
				}
			}
			if (flag)
			{
				TransformData item2 = ResettableObjectCaches<TransformData>.Retrieve();
				_lastSentTransformDatas.Add(item2);
			}
		}

		private void ConfigureComponents()
		{
			if (_componentConfiguration == ComponentConfigurationType.Disabled)
			{
				return;
			}
			CharacterController component3;
			if (_componentConfiguration == ComponentConfigurationType.Rigidbody)
			{
				if (TryGetComponent<Rigidbody>(out var component))
				{
					bool isKinematic = CanMakeKinematic();
					component.isKinematic = isKinematic;
					component.interpolation = RigidbodyInterpolation.None;
				}
			}
			else if (_componentConfiguration == ComponentConfigurationType.Rigidbody2D)
			{
				if (_clientAuthoritative && TryGetComponent<Rigidbody2D>(out var component2))
				{
					bool flag = (component2.isKinematic = CanMakeKinematic());
					component2.simulated = !flag;
					component2.interpolation = RigidbodyInterpolation2D.None;
				}
			}
			else if (_componentConfiguration == ComponentConfigurationType.CharacterController && TryGetComponent<CharacterController>(out component3))
			{
				if (_clientAuthoritative)
				{
					component3.enabled = base.IsOwner;
				}
				else if (_sendToOwner)
				{
					component3.enabled = base.IsServerInitialized;
				}
				else
				{
					component3.enabled = base.IsServerInitialized || base.IsOwner;
				}
			}
			bool CanMakeKinematic()
			{
				if (_clientAuthoritative)
				{
					if (base.IsOwner)
					{
						return base.IsServerOnly;
					}
					return true;
				}
				return !base.IsServerInitialized;
			}
		}

		private void TimeManager_OnPostTick()
		{
			if (_forceSendTick != 0 && base.TimeManager.LocalTick > _forceSendTick)
			{
				_forceSendTick = 0u;
				ForceSend();
			}
			UpdateParentBehaviour();
			if (!_enableNetworkLod && _interval > 1)
			{
				if (_intervalsRemaining == -1)
				{
					if (!base.transform.hasChanged)
					{
						return;
					}
					_intervalsRemaining = _interval;
				}
				_intervalsRemaining--;
				if (_intervalsRemaining > 0)
				{
					return;
				}
				_intervalsRemaining = -1;
			}
			if (base.IsServerInitialized)
			{
				byte lodIndex = (byte)(_enableNetworkLod ? base.ObserverManager.LevelOfDetailIndex : 0);
				SendToClients(lodIndex);
			}
			if (base.IsClientInitialized && _lastSentTransformDatas != null)
			{
				SendToServer(_lastSentTransformDatas[0]);
			}
		}

		private void ChangeTickSubscription(bool subscribe)
		{
			if (subscribe != _subscribedToTicks && !(base.NetworkManager == null))
			{
				_subscribedToTicks = subscribe;
				if (subscribe)
				{
					base.NetworkManager.TimeManager.OnPostTick += TimeManager_OnPostTick;
				}
				else
				{
					base.NetworkManager.TimeManager.OnPostTick -= TimeManager_OnPostTick;
				}
			}
		}

		private bool CanControl()
		{
			bool isServerInitialized = base.IsServerInitialized;
			if (_clientAuthoritative)
			{
				if (base.IsOwner)
				{
					return true;
				}
				if (!base.Owner.IsValid && isServerInitialized)
				{
					return true;
				}
			}
			else if (isServerInitialized)
			{
				return true;
			}
			return false;
		}

		[ObserversRpc(BufferLast = true, ExcludeServer = true)]
		private void ObserversSetSendToOwner(bool value)
		{
			RpcWriter___Observers_ObserversSetSendToOwner_1140765316(value);
		}

		public void ForceSend(uint ticks)
		{
			if (_forceSendTick != 0)
			{
				ForceSend();
			}
			_forceSendTick = base.TimeManager.LocalTick + ticks;
		}

		public void ForceSend()
		{
			for (int i = 0; i < _lastSentTransformDatas.Count; i++)
			{
				_lastSentTransformDatas[i].ResetState();
			}
			if (_authoritativeClientData.Writer != null)
			{
				_authoritativeClientData.SetHasData(value: true);
			}
		}

		public void SetInterval(byte value)
		{
			if ((base.IsServerInitialized && !_clientAuthoritative) || (base.IsServerInitialized && _clientAuthoritative && !base.Owner.IsValid) || (_clientAuthoritative && base.IsOwner))
			{
				if (base.IsServerInitialized)
				{
					ObserversSetInterval(value);
				}
				else
				{
					ServerSetInterval(value);
				}
			}
		}

		private void SetIntervalInternal(byte value)
		{
			value = (byte)Mathf.Max(value, 1);
			_interval = value;
		}

		[ServerRpc(RunLocally = true)]
		private void ServerSetInterval(byte value)
		{
			RpcWriter___Server_ServerSetInterval_1246646286(value);
			RpcLogic___ServerSetInterval_1246646286(value);
		}

		[ObserversRpc(BufferLast = true, RunLocally = true)]
		private void ObserversSetInterval(byte value)
		{
			RpcWriter___Observers_ObserversSetInterval_1246646286(value);
			RpcLogic___ObserversSetInterval_1246646286(value);
		}

		private void SetDefaultGoalData()
		{
			Transform t = base.transform;
			NetworkBehaviour parentBehaviour = null;
			if (_synchronizeParent && base.NetworkObject.CurrentParentNetworkObject != null)
			{
				base.transform.parent.TryGetComponent<NetworkBehaviour>(out parentBehaviour);
				if (parentBehaviour == null)
				{
					LogInvalidParent();
				}
				else
				{
					_parentTransform = base.transform.parent;
					_parentBehaviour = parentBehaviour;
				}
			}
			SetLastReceived(_lastReceivedServerTransformData);
			SetLastReceived(_lastReceivedClientTransformData);
			void SetLastReceived(TransformData td)
			{
				td?.Update(0u, t.localPosition, t.localRotation, t.localScale, t.localPosition, parentBehaviour);
			}
		}

		private void LogInvalidParent()
		{
			Debug.LogWarning($"{base.gameObject.name} [Id {base.ObjectId}] is nested but the parent {base.transform.parent.name} does not contain a NetworkBehaviour component. To synchronize parents the parent object must have a NetworkBehaviour component, even if empty.");
		}

		private void SerializeChanged(ChangedDelta changed, PooledWriter writer, byte lodIndex)
		{
			UpdateFlagA updateFlagA = UpdateFlagA.Unset;
			UpdateFlagB updateFlagB = UpdateFlagB.Unset;
			TransformPackingData transformPackingData = (ChangedContains(changed, ChangedDelta.Nested) ? _unpacked : _packing);
			if (_enableNetworkLod)
			{
				writer.WriteByte(lodIndex);
			}
			int position = writer.Position;
			writer.Reserve(1);
			float num = 100f;
			float num2 = 32766f;
			Transform transform = base.transform;
			if (_synchronizePosition)
			{
				AutoPackType position2 = transformPackingData.Position;
				if (ChangedContains(changed, ChangedDelta.PositionX))
				{
					float x = transform.localPosition.x;
					float num3 = x * num;
					if (position2 != AutoPackType.Unpacked && Math.Abs(num3) <= num2)
					{
						updateFlagA |= UpdateFlagA.X2;
						writer.WriteInt16((short)num3);
					}
					else
					{
						updateFlagA |= UpdateFlagA.X4;
						writer.WriteSingle(x);
					}
				}
				if (ChangedContains(changed, ChangedDelta.PositionY))
				{
					float x = transform.localPosition.y;
					float num3 = x * num;
					if (position2 != AutoPackType.Unpacked && Math.Abs(num3) <= num2)
					{
						updateFlagA |= UpdateFlagA.Y2;
						writer.WriteInt16((short)num3);
					}
					else
					{
						updateFlagA |= UpdateFlagA.Y4;
						writer.WriteSingle(x);
					}
				}
				if (ChangedContains(changed, ChangedDelta.PositionZ))
				{
					float x = transform.localPosition.z;
					float num3 = x * num;
					if (position2 != AutoPackType.Unpacked && Math.Abs(num3) <= num2)
					{
						updateFlagA |= UpdateFlagA.Z2;
						writer.WriteInt16((short)num3);
					}
					else
					{
						updateFlagA |= UpdateFlagA.Z4;
						writer.WriteSingle(x);
					}
				}
			}
			if (_synchronizeRotation && ChangedContains(changed, ChangedDelta.Rotation))
			{
				updateFlagA |= UpdateFlagA.Rotation;
				writer.WriteQuaternion(transform.localRotation, _packing.Rotation);
			}
			if (ChangedContains(changed, ChangedDelta.Extended))
			{
				AutoPackType scale = transformPackingData.Scale;
				updateFlagA |= UpdateFlagA.Extended;
				int position3 = writer.Position;
				writer.Reserve(1);
				if (_synchronizeScale)
				{
					if (ChangedContains(changed, ChangedDelta.ScaleX))
					{
						float x = transform.localScale.x;
						float num3 = x * num;
						if (scale != AutoPackType.Unpacked && Math.Abs(num3) <= num2)
						{
							updateFlagB |= UpdateFlagB.X2;
							writer.WriteInt16((short)num3);
						}
						else
						{
							updateFlagB |= UpdateFlagB.X4;
							writer.WriteSingle(x);
						}
					}
					if (ChangedContains(changed, ChangedDelta.ScaleY))
					{
						float x = transform.localScale.y;
						float num3 = x * num;
						if (scale != AutoPackType.Unpacked && Math.Abs(num3) <= num2)
						{
							updateFlagB |= UpdateFlagB.Y2;
							writer.WriteInt16((short)num3);
						}
						else
						{
							updateFlagB |= UpdateFlagB.Y4;
							writer.WriteSingle(x);
						}
					}
					if (ChangedContains(changed, ChangedDelta.ScaleZ))
					{
						float x = transform.localScale.z;
						float num3 = x * num;
						if (scale != AutoPackType.Unpacked && Math.Abs(num3) <= num2)
						{
							updateFlagB |= UpdateFlagB.Z2;
							writer.WriteInt16((short)num3);
						}
						else
						{
							updateFlagB |= UpdateFlagB.Z4;
							writer.WriteSingle(x);
						}
					}
				}
				if (ChangedContains(changed, ChangedDelta.Nested) && _parentBehaviour != null)
				{
					updateFlagB |= UpdateFlagB.Nested;
					writer.WriteNetworkBehaviour(_parentBehaviour);
				}
				writer.FastInsertByte((byte)updateFlagB, position3);
			}
			writer.FastInsertByte((byte)updateFlagA, position);
			static bool ChangedContains(ChangedDelta whole, ChangedDelta part)
			{
				return (whole & part) == part;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DeserializePacket(ArraySegment<byte> data, TransformData prevTransformData, TransformData nextTransformData, ref ChangedFull changedFull, out byte lodIndex)
		{
			PooledReader pooledReader = ReaderPool.Retrieve(data, base.NetworkManager);
			if (_enableNetworkLod)
			{
				lodIndex = pooledReader.ReadByte();
			}
			else
			{
				lodIndex = 0;
			}
			UpdateFlagA whole = (UpdateFlagA)pooledReader.ReadByte();
			int remaining = pooledReader.Remaining;
			if (UpdateFlagAContains(whole, UpdateFlagA.X2))
			{
				nextTransformData.Position.x = (float)pooledReader.ReadInt16() / 100f;
			}
			else if (UpdateFlagAContains(whole, UpdateFlagA.X4))
			{
				nextTransformData.Position.x = pooledReader.ReadSingle();
			}
			else
			{
				nextTransformData.Position.x = prevTransformData.Position.x;
			}
			if (UpdateFlagAContains(whole, UpdateFlagA.Y2))
			{
				nextTransformData.Position.y = (float)pooledReader.ReadInt16() / 100f;
			}
			else if (UpdateFlagAContains(whole, UpdateFlagA.Y4))
			{
				nextTransformData.Position.y = pooledReader.ReadSingle();
			}
			else
			{
				nextTransformData.Position.y = prevTransformData.Position.y;
			}
			if (UpdateFlagAContains(whole, UpdateFlagA.Z2))
			{
				nextTransformData.Position.z = (float)pooledReader.ReadInt16() / 100f;
			}
			else if (UpdateFlagAContains(whole, UpdateFlagA.Z4))
			{
				nextTransformData.Position.z = pooledReader.ReadSingle();
			}
			else
			{
				nextTransformData.Position.z = prevTransformData.Position.z;
			}
			if (remaining != pooledReader.Remaining)
			{
				changedFull |= ChangedFull.Position;
			}
			if (UpdateFlagAContains(whole, UpdateFlagA.Rotation))
			{
				nextTransformData.Rotation = pooledReader.ReadQuaternion(_packing.Rotation);
				changedFull |= ChangedFull.Rotation;
			}
			else
			{
				nextTransformData.Rotation = prevTransformData.Rotation;
			}
			if (UpdateFlagAContains(whole, UpdateFlagA.Extended))
			{
				UpdateFlagB whole2 = (UpdateFlagB)pooledReader.ReadByte();
				remaining = pooledReader.Remaining;
				if (UpdateFlagBContains(whole2, UpdateFlagB.X2))
				{
					nextTransformData.Scale.x = (float)pooledReader.ReadInt16() / 100f;
				}
				else if (UpdateFlagBContains(whole2, UpdateFlagB.X4))
				{
					nextTransformData.Scale.x = pooledReader.ReadSingle();
				}
				else
				{
					nextTransformData.Scale.x = prevTransformData.Scale.x;
				}
				if (UpdateFlagBContains(whole2, UpdateFlagB.Y2))
				{
					nextTransformData.Scale.y = (float)pooledReader.ReadInt16() / 100f;
				}
				else if (UpdateFlagBContains(whole2, UpdateFlagB.Y4))
				{
					nextTransformData.Scale.y = pooledReader.ReadSingle();
				}
				else
				{
					nextTransformData.Scale.y = prevTransformData.Scale.y;
				}
				if (UpdateFlagBContains(whole2, UpdateFlagB.Z2))
				{
					nextTransformData.Scale.z = (float)pooledReader.ReadInt16() / 100f;
				}
				else if (UpdateFlagBContains(whole2, UpdateFlagB.Z4))
				{
					nextTransformData.Scale.z = pooledReader.ReadSingle();
				}
				else
				{
					nextTransformData.Scale.z = prevTransformData.Scale.z;
				}
				if (pooledReader.Remaining != remaining)
				{
					changedFull |= ChangedFull.Scale;
				}
				else
				{
					nextTransformData.Scale = prevTransformData.Scale;
				}
				if (UpdateFlagBContains(whole2, UpdateFlagB.Nested))
				{
					nextTransformData.ParentBehaviour = pooledReader.ReadNetworkBehaviour();
					changedFull |= ChangedFull.Nested;
				}
				else
				{
					Unnest();
				}
			}
			else
			{
				nextTransformData.Scale = prevTransformData.Scale;
				Unnest();
			}
			pooledReader.Store();
			void Unnest()
			{
				nextTransformData.ParentBehaviour = null;
			}
			static bool UpdateFlagAContains(UpdateFlagA updateFlagA, UpdateFlagA part)
			{
				return (updateFlagA & part) == part;
			}
			static bool UpdateFlagBContains(UpdateFlagB updateFlagB, UpdateFlagB part)
			{
				return (updateFlagB & part) == part;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void UpdateParentBehaviour()
		{
			if (!_synchronizeParent || !CanControl())
			{
				return;
			}
			Transform parent = base.transform.parent;
			if (parent == null)
			{
				if (_parentTransform != null && base.NetworkObject.RuntimeParentTransform != null)
				{
					Debug.LogWarning(base.gameObject.name + " parent object was removed without calling UnsetParent. Use networkObject.UnsetParent() to remove a NetworkObject from it's parent. This is being made a requirement in Fish-Networking v4.");
				}
				_parentBehaviour = null;
				_parentTransform = null;
			}
			else if (!(_parentTransform == parent))
			{
				_parentTransform = parent;
				parent.TryGetComponent<NetworkBehaviour>(out _parentBehaviour);
				if (_parentBehaviour == null)
				{
					LogInvalidParent();
				}
				else if (base.NetworkObject.RuntimeParentTransform != parent)
				{
					Debug.LogWarning(base.gameObject.name + " parent was set without calling SetParent. Use networkObject.SetParent(obj) to assign a NetworkObject a new parent. This is being made a requirement in Fish-Networking v4.");
				}
			}
		}

		private void SetParent(NetworkBehaviour parent, RateData rd)
		{
			Transform transform = ((parent == null) ? null : parent.transform);
			if (!(transform == base.transform.parent))
			{
				Vector3 localScale = base.transform.localScale;
				if (transform != null)
				{
					base.NetworkObject.SetParent(parent);
				}
				else
				{
					base.NetworkObject.UnsetParent();
				}
				base.transform.localScale = localScale;
				rd?.Update(-1f, -1f, -1f, rd.LastUnalteredPositionRate, rd.TickSpan, rd.AbnormalRateDetected, rd.TimeRemaining);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void MoveToTarget(float deltaOverride = -1f)
		{
			if (_currentGoalData == null || (!base.IsServerInitialized && !base.IsClientInitialized))
			{
				return;
			}
			if (_clientAuthoritative)
			{
				if (base.IsOwner || TakenOwnership)
				{
					return;
				}
			}
			else if (base.IsOwner && !_sendToOwner)
			{
				return;
			}
			if ((!_clientAuthoritative || !base.Owner.IsActive) && base.IsServerInitialized)
			{
				return;
			}
			float num = ((deltaOverride != -1f) ? deltaOverride : Time.deltaTime);
			TransformData transforms = _currentGoalData.Transforms;
			RateData rates = _currentGoalData.Rates;
			if (_synchronizeParent)
			{
				SetParent(transforms.ParentBehaviour, rates);
			}
			float num2 = 1f;
			int count = _goalDataQueue.Count;
			if (count > _interpolation + 1)
			{
				num2 += 0.05f * (float)count;
			}
			Transform transform = base.transform;
			SnapProperties(transforms);
			if (_synchronizePosition)
			{
				float position = rates.Position;
				Vector3 target = ((transforms.ExtrapolationState == TransformData.ExtrapolateState.Active && !_lastReceiveReliable) ? transforms.ExtrapolatedPosition : transforms.Position);
				if (position == -1f)
				{
					transform.localPosition = transforms.Position;
				}
				else
				{
					transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, position * num * num2);
				}
			}
			if (_synchronizeRotation)
			{
				float position = rates.Rotation;
				if (position == -1f)
				{
					transform.localRotation = transforms.Rotation;
				}
				else
				{
					transform.localRotation = Quaternion.RotateTowards(transform.localRotation, transforms.Rotation, position * num);
				}
			}
			if (_synchronizeScale)
			{
				float position = rates.Scale;
				if (position == -1f)
				{
					transform.localScale = transforms.Scale;
				}
				else
				{
					transform.localScale = Vector3.MoveTowards(transform.localScale, transforms.Scale, position * num);
				}
			}
			float num3 = rates.TimeRemaining - num * num2;
			if (num3 < 0f - num)
			{
				num3 = 0f - num;
			}
			rates.TimeRemaining = num3;
			if (!(rates.TimeRemaining <= 0f))
			{
				return;
			}
			float num4 = Mathf.Abs(rates.TimeRemaining);
			if (count > 0)
			{
				SetCurrentGoalData(_goalDataQueue.Dequeue());
				if (num4 > 0f)
				{
					MoveToTarget(num4);
				}
			}
			else
			{
				if (!HasChanged(transforms))
				{
					_currentGoalData = null;
				}
				this.OnInterpolationComplete?.Invoke();
			}
		}

		private void SendToClients(byte lodIndex)
		{
			bool num = _clientAuthoritative && base.Owner.IsValid;
			Channel channel = Channel.Unreliable;
			if (num && !base.Owner.IsLocalClient)
			{
				if (_authoritativeClientData.HasData[lodIndex])
				{
					_changedSinceStart = true;
					ObserversUpdateClientAuthoritativeTransform(_authoritativeClientData.Writer.GetArraySegment(), _authoritativeClientData.Channel);
					_authoritativeClientData.SetHasData(value: false, lodIndex);
				}
				return;
			}
			bool flag = false;
			for (int num2 = lodIndex; num2 >= 0; num2--)
			{
				PooledWriter pooledWriter = _toClientChangedWriters[num2];
				pooledWriter.Reset();
				TransformData transformData = _lastSentTransformDatas[num2];
				ChangedDelta changedDelta = GetChanged(transformData);
				if (changedDelta == ChangedDelta.Unset)
				{
					if (_serverChangedSinceReliable[num2] == ChangedDelta.Unset)
					{
						continue;
					}
					changedDelta = _serverChangedSinceReliable[lodIndex];
					_serverChangedSinceReliable[num2] = ChangedDelta.Unset;
					channel = Channel.Reliable;
				}
				else
				{
					_serverChangedSinceReliable[num2] |= changedDelta;
				}
				flag = true;
				_changedSinceStart = true;
				Transform transform = base.transform;
				transformData.Update(0u, transform.localPosition, transform.localRotation, transform.localScale, transform.localPosition, _parentBehaviour);
				SerializeChanged(changedDelta, pooledWriter, lodIndex);
			}
			if (!flag)
			{
				return;
			}
			ArraySegment<byte> arraySegment = _toClientChangedWriters[lodIndex].GetArraySegment();
			if (arraySegment.Count <= 0)
			{
				return;
			}
			_ = _enableNetworkLod;
			foreach (NetworkConnection observer in base.Observers)
			{
				if ((_sendToOwner || !(observer == base.Owner)) && !observer.IsLocalClient)
				{
					TargetUpdateTransform(observer, arraySegment, channel);
				}
			}
		}

		private void SendToServer(TransformData lastSentTransformData)
		{
			if (base.IsServerInitialized || !_clientAuthoritative || !base.IsOwner)
			{
				return;
			}
			Channel channel = Channel.Unreliable;
			ChangedDelta changedDelta = GetChanged(lastSentTransformData);
			if (changedDelta == ChangedDelta.Unset)
			{
				if (_clientChangedSinceReliable == ChangedDelta.Unset)
				{
					return;
				}
				changedDelta = _clientChangedSinceReliable;
				_clientChangedSinceReliable = ChangedDelta.Unset;
				channel = Channel.Reliable;
			}
			else
			{
				_clientChangedSinceReliable |= changedDelta;
			}
			Transform transform = base.transform;
			lastSentTransformData.Update(0u, transform.localPosition, transform.localRotation, transform.localScale, transform.localPosition, _parentBehaviour);
			PooledWriter pooledWriter = WriterPool.Retrieve();
			SerializeChanged(changedDelta, pooledWriter, 0);
			ServerUpdateTransform(pooledWriter.GetArraySegment(), channel);
			pooledWriter.Store();
		}

		private bool HasChanged(TransformData td)
		{
			if (!(td.Position != base.transform.localPosition) && !(td.Rotation != base.transform.localRotation))
			{
				return td.Scale != base.transform.localScale;
			}
			return true;
		}

		private bool HasChanged(TransformData a, TransformData b)
		{
			if (!(a.Position != b.Position) && !(a.Rotation != b.Rotation) && !(a.Scale != b.Scale))
			{
				return a.ParentBehaviour != b.ParentBehaviour;
			}
			return true;
		}

		private bool HasChanged(TransformData a, TransformData b, ref ChangedFull changedFull)
		{
			bool result = false;
			if (a.Position != b.Position)
			{
				result = true;
				changedFull |= ChangedFull.Position;
			}
			if (a.Rotation != b.Rotation)
			{
				result = true;
				changedFull |= ChangedFull.Rotation;
			}
			if (a.Scale != b.Scale)
			{
				result = true;
				changedFull |= ChangedFull.Scale;
			}
			if (a.ParentBehaviour != b.ParentBehaviour)
			{
				result = true;
				changedFull |= ChangedFull.Nested;
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ChangedDelta GetChanged(TransformData transformData)
		{
			if (transformData.IsDefault)
			{
				return _fullChanged;
			}
			return GetChanged(ref transformData.Position, ref transformData.Rotation, ref transformData.Scale, transformData.ParentBehaviour);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ChangedDelta GetChanged(ref Vector3 lastPosition, ref Quaternion lastRotation, ref Vector3 lastScale, NetworkBehaviour lastParentBehaviour)
		{
			ChangedDelta changedDelta = ChangedDelta.Unset;
			Transform obj = base.transform;
			Vector3 localPosition = obj.localPosition;
			if (localPosition.x != lastPosition.x)
			{
				changedDelta |= ChangedDelta.PositionX;
			}
			if (localPosition.y != lastPosition.y)
			{
				changedDelta |= ChangedDelta.PositionY;
			}
			if (localPosition.z != lastPosition.z)
			{
				changedDelta |= ChangedDelta.PositionZ;
			}
			if (!obj.localRotation.Matches(lastRotation, precise: true))
			{
				changedDelta |= ChangedDelta.Rotation;
			}
			ChangedDelta changedDelta2 = changedDelta;
			Vector3 localScale = obj.localScale;
			if (localScale.x != lastScale.x)
			{
				changedDelta |= ChangedDelta.ScaleX;
			}
			if (localScale.y != lastScale.y)
			{
				changedDelta |= ChangedDelta.ScaleY;
			}
			if (localScale.z != lastScale.z)
			{
				changedDelta |= ChangedDelta.ScaleZ;
			}
			if (_parentBehaviour != null)
			{
				changedDelta |= ChangedDelta.Nested;
			}
			if (changedDelta2 != changedDelta)
			{
				changedDelta |= ChangedDelta.Extended;
			}
			return changedDelta;
		}

		private void SnapProperties(TransformData transformData, bool force = false)
		{
			if (!transformData.SnappingChecked)
			{
				transformData.SnappingChecked = true;
				Transform transform = base.transform;
				if (_synchronizePosition)
				{
					Vector3 localPosition = default(Vector3);
					localPosition.x = ((force || _positionSnapping.X) ? transformData.Position.x : transform.localPosition.x);
					localPosition.y = ((force || _positionSnapping.Y) ? transformData.Position.y : transform.localPosition.y);
					localPosition.z = ((force || _positionSnapping.Z) ? transformData.Position.z : transform.localPosition.z);
					transform.localPosition = localPosition;
				}
				if (_synchronizeRotation)
				{
					Vector3 eulerAngles = transformData.Rotation.eulerAngles;
					Vector3 localEulerAngles = default(Vector3);
					localEulerAngles.x = ((force || _rotationSnapping.X) ? eulerAngles.x : transform.localEulerAngles.x);
					localEulerAngles.y = ((force || _rotationSnapping.Y) ? eulerAngles.y : transform.localEulerAngles.y);
					localEulerAngles.z = ((force || _rotationSnapping.Z) ? eulerAngles.z : transform.localEulerAngles.z);
					transform.localEulerAngles = localEulerAngles;
				}
				if (_synchronizeScale)
				{
					Vector3 localScale = default(Vector3);
					localScale.x = ((force || _scaleSnapping.X) ? transformData.Scale.x : transform.localScale.x);
					localScale.y = ((force || _scaleSnapping.Y) ? transformData.Scale.y : transform.localScale.y);
					localScale.z = ((force || _scaleSnapping.Z) ? transformData.Scale.z : transform.localScale.z);
					transform.localScale = localScale;
				}
			}
		}

		private void SetInstantRates(RateData rd, uint tickDifference, float timeRemaining)
		{
			rd.Update(-1f, -1f, -1f, -1f, tickDifference, abnormalRateDetected: false, timeRemaining);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetCalculatedRates(byte lodIndex, TransformData prevTd, RateData prevRd, GoalData nextGd, ChangedFull changedFull, bool hasChanged, Channel channel, bool asServer)
		{
			TransformData transforms = nextGd.Transforms;
			if (channel == Channel.Reliable && !hasChanged)
			{
				nextGd.Rates.Update(prevRd);
				return;
			}
			float timePassed;
			uint tickDifference = GetTickDifference(prevTd, nextGd, 1u, asServer, out timePassed);
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			RateData rates = nextGd.Rates;
			float num4 = 1f;
			bool abnormalRateDetected = false;
			float num5 = rates.LastUnalteredPositionRate;
			if (ChangedFullContains(changedFull, ChangedFull.Position))
			{
				float num6 = Vector3.Distance(prevTd.Position, transforms.Position);
				if (_enableTeleport)
				{
					float num7 = _teleportThreshold;
					if (_scaleThreshold)
					{
						num7 *= (float)(int)ObserverManager.GetLevelOfDetailInterval(lodIndex);
					}
					if (num6 >= num7)
					{
						SetInstantRates(rates, tickDifference, timePassed);
						return;
					}
				}
				if (LowDistance(num6, rotation: false))
				{
					num5 = -1f;
					abnormalRateDetected = false;
					num = -1f;
				}
				else
				{
					num5 = num6 / timePassed;
					if (num5 > 0f && rates.LastUnalteredPositionRate > 0f && Mathf.Abs(1f - num5 / rates.LastUnalteredPositionRate) > 0.25f)
					{
						float num8 = rates.LastUnalteredPositionRate / num5;
						float num9 = 0.1f;
						if ((num8 < 1f && Mathf.Abs(0.5f - num8) < num9) || (num8 > 1f && Mathf.Abs(2f - num8) < num9))
						{
							num4 = num8;
							abnormalRateDetected = true;
						}
					}
					num = num5 * num4;
					if (num <= 0f)
					{
						num = -1f;
					}
				}
			}
			if (ChangedFullContains(changedFull, ChangedFull.Rotation))
			{
				float num6 = prevTd.Rotation.Angle(transforms.Rotation, precise: true);
				if (LowDistance(num6, rotation: true))
				{
					num2 = -1f;
				}
				else
				{
					num2 = num6 / timePassed * num4;
					if (num2 <= 0f)
					{
						num2 = -1f;
					}
				}
			}
			if (ChangedFullContains(changedFull, ChangedFull.Scale))
			{
				float num6 = Vector3.Distance(prevTd.Scale, transforms.Scale);
				if (LowDistance(num6, rotation: false))
				{
					num3 = -1f;
				}
				else
				{
					num3 = num6 / timePassed * num4;
					if (num3 <= 0f)
					{
						num3 = -1f;
					}
				}
			}
			rates.Update(num, num2, num3, num5, tickDifference, abnormalRateDetected, timePassed);
			static bool ChangedFullContains(ChangedFull whole, ChangedFull part)
			{
				return (whole & part) == part;
			}
			static bool LowDistance(float dist, bool rotation)
			{
				if (rotation)
				{
					return dist < 1f;
				}
				return dist < 0.0001f;
			}
		}

		private uint GetTickDifference(TransformData prevTd, GoalData nextGd, uint minimum, bool asServer, out float timePassed)
		{
			long num;
			if (asServer)
			{
				num = 1L;
			}
			else
			{
				TransformData transforms = nextGd.Transforms;
				uint num2 = prevTd.Tick;
				if (num2 == 0)
				{
					num2 = transforms.Tick - _interval;
				}
				num = transforms.Tick - num2;
				if (num < minimum)
				{
					num = minimum;
				}
			}
			timePassed = (float)base.NetworkManager.TimeManager.TicksToTime((uint)num);
			return (uint)num;
		}

		private void SetExtrapolation(TransformData prev, TransformData next, Channel channel)
		{
			next.ExtrapolationState = TransformData.ExtrapolateState.Disabled;
		}

		[TargetRpc(ValidateTarget = false)]
		private void TargetUpdateTransform(NetworkConnection conn, ArraySegment<byte> data, Channel channel)
		{
			RpcWriter___Target_TargetUpdateTransform_748863190(conn, data, channel);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ObserversRpc]
		private void ObserversUpdateClientAuthoritativeTransform(ArraySegment<byte> data, Channel channel)
		{
			RpcWriter___Observers_ObserversUpdateClientAuthoritativeTransform_2713644489(data, channel);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ServerRpc]
		private void ServerUpdateTransform(ArraySegment<byte> data, Channel channel)
		{
			RpcWriter___Server_ServerUpdateTransform_2713644489(data, channel);
		}

		private void DataReceived(ArraySegment<byte> data, Channel channel, bool asServer)
		{
			if (base.IsDeinitializing)
			{
				return;
			}
			TransformData transformData = (asServer ? _lastReceivedClientTransformData : _lastReceivedServerTransformData);
			RateData lastCalculatedRateData = _lastCalculatedRateData;
			ChangedFull changedFull = ChangedFull.Unset;
			GoalData goalData = ResettableObjectCaches<GoalData>.Retrieve();
			TransformData transforms = goalData.Transforms;
			UpdateTransformData(data, transformData, transforms, ref changedFull, out var lodIndex);
			this.OnDataReceived?.Invoke(transformData, transforms);
			SetExtrapolation(transformData, transforms, channel);
			if (_enableNetworkLod)
			{
				_interval = lodIndex;
			}
			bool flag = HasChanged(transformData, transforms);
			if (asServer && !base.IsClientInitialized)
			{
				float timePassed;
				uint tickDifference = GetTickDifference(transformData, goalData, 1u, asServer, out timePassed);
				SetInstantRates(goalData.Rates, tickDifference, timePassed);
			}
			else
			{
				SetCalculatedRates(lodIndex, transformData, lastCalculatedRateData, goalData, changedFull, flag, channel, asServer);
			}
			transformData.Update(transforms);
			_lastReceiveReliable = channel == Channel.Reliable;
			if (channel == Channel.Reliable)
			{
				transforms.Tick = 0u;
			}
			transformData.Update(transforms);
			lastCalculatedRateData.Update(goalData.Rates);
			goalData.ReceivedTick = base.TimeManager.LocalTick;
			bool flag2 = _currentGoalData == null;
			if (!flag2 && _currentGoalData.Transforms.ExtrapolationState == TransformData.ExtrapolateState.Active)
			{
				SetCurrentGoalData(goalData);
			}
			else if ((flag2 && _goalDataQueue.Count >= _interpolation) || channel == Channel.Reliable)
			{
				if (_goalDataQueue.Count > 0)
				{
					SetCurrentGoalData(_goalDataQueue.Dequeue());
					if (flag)
					{
						_goalDataQueue.Enqueue(goalData);
					}
				}
				else
				{
					SetCurrentGoalData(goalData);
				}
			}
			else
			{
				_goalDataQueue.Enqueue(goalData);
			}
			if (_goalDataQueue.Count > _interpolation + 3)
			{
				while (_goalDataQueue.Count > _interpolation)
				{
					ResettableObjectCaches<GoalData>.Store(_goalDataQueue.Dequeue());
				}
				SetCurrentGoalData(_goalDataQueue.Dequeue());
				SetInstantRates(_currentGoalData.Rates, 1u, -1f);
				SnapProperties(_currentGoalData.Transforms, force: true);
			}
		}

		private void SetCurrentGoalData(GoalData data)
		{
			if (_currentGoalData != null)
			{
				ResettableObjectCaches<GoalData>.Store(_currentGoalData);
			}
			_currentGoalData = data;
			this.OnNextGoal?.Invoke(data);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void UpdateTransformData(ArraySegment<byte> packetData, TransformData prevTransformData, TransformData nextTransformData, ref ChangedFull changedFull, out byte lodIndex)
		{
			DeserializePacket(packetData, prevTransformData, nextTransformData, ref changedFull, out lodIndex);
			nextTransformData.Tick = base.TimeManager.LastPacketTick;
		}

		internal void ConfigureForCSP()
		{
			_clientAuthoritative = false;
			if (base.IsServerInitialized)
			{
				_sendToOwner = false;
			}
			_componentConfiguration = ComponentConfigurationType.CharacterController;
			ConfigureComponents();
		}

		public void SetSynchronizedProperties(SynchronizedProperty value)
		{
			if (base.IsServerInitialized || (_clientAuthoritative && base.IsOwner))
			{
				if (base.IsServerInitialized)
				{
					ObserversSetSynchronizedProperties(value);
				}
				else
				{
					ServerSetSynchronizedProperties(value);
				}
			}
		}

		[ServerRpc]
		private void ServerSetSynchronizedProperties(SynchronizedProperty value)
		{
			RpcWriter___Server_ServerSetSynchronizedProperties_535967898(value);
		}

		[ObserversRpc(BufferLast = true)]
		private void ObserversSetSynchronizedProperties(SynchronizedProperty value)
		{
			RpcWriter___Observers_ObserversSetSynchronizedProperties_535967898(value);
		}

		private void SetSynchronizedPropertiesInternal(SynchronizedProperty value)
		{
			_synchronizeParent = SynchronizedPropertyContains(value, SynchronizedProperty.Parent);
			_synchronizePosition = SynchronizedPropertyContains(value, SynchronizedProperty.Position);
			_synchronizeRotation = SynchronizedPropertyContains(value, SynchronizedProperty.Rotation);
			_synchronizeScale = SynchronizedPropertyContains(value, SynchronizedProperty.Scale);
			static bool SynchronizedPropertyContains(SynchronizedProperty whole, SynchronizedProperty part)
			{
				return (whole & part) == part;
			}
		}

		public void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EComponent_002ETransforming_002ENetworkTransformFishNet_002ERuntime_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EComponent_002ETransforming_002ENetworkTransformFishNet_002ERuntime_002Edll_Excuted = true;
				RegisterObserversRpc(0u, RpcReader___Observers_ObserversSetSendToOwner_1140765316);
				RegisterServerRpc(1u, RpcReader___Server_ServerSetInterval_1246646286);
				RegisterObserversRpc(2u, RpcReader___Observers_ObserversSetInterval_1246646286);
				RegisterTargetRpc(3u, RpcReader___Target_TargetUpdateTransform_748863190);
				RegisterObserversRpc(4u, RpcReader___Observers_ObserversUpdateClientAuthoritativeTransform_2713644489);
				RegisterServerRpc(5u, RpcReader___Server_ServerUpdateTransform_2713644489);
				RegisterServerRpc(6u, RpcReader___Server_ServerSetSynchronizedProperties_535967898);
				RegisterObserversRpc(7u, RpcReader___Observers_ObserversSetSynchronizedProperties_535967898);
			}
		}

		public void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EComponent_002ETransforming_002ENetworkTransformFishNet_002ERuntime_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EComponent_002ETransforming_002ENetworkTransformFishNet_002ERuntime_002Edll_Excuted = true;
			}
		}

		public override void NetworkInitializeIfDisabled()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}

		private void RpcWriter___Observers_ObserversSetSendToOwner_1140765316(bool value)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else
			{
				Channel channel = Channel.Reliable;
				PooledWriter writer = WriterPool.GetWriter();
				GeneratedWriters___Internal.InstancedExtension___WriteBoolean(writer, value);
				SendObserversRpc(0u, writer, channel, DataOrderType.Default, bufferLast: true, excludeServer: true, excludeOwner: false);
				writer.Store();
			}
		}

		private void RpcLogic___ObserversSetSendToOwner_1140765316(bool value)
		{
			_sendToOwner = value;
		}

		private void RpcReader___Observers_ObserversSetSendToOwner_1140765316(PooledReader PooledReader0, Channel channel)
		{
			bool value = GeneratedReaders___Internal.InstancedExtension___ReadBoolean(PooledReader0);
			if (base.IsClientInitialized)
			{
				RpcLogic___ObserversSetSendToOwner_1140765316(value);
			}
		}

		private void RpcWriter___Server_ServerSetInterval_1246646286(byte value)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if ((object)networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
			}
			else
			{
				Channel channel = Channel.Reliable;
				PooledWriter writer = WriterPool.GetWriter();
				GeneratedWriters___Internal.InstancedExtension___WriteByte(writer, value);
				SendServerRpc(1u, writer, channel, DataOrderType.Default);
				writer.Store();
			}
		}

		private void RpcLogic___ServerSetInterval_1246646286(byte value)
		{
			if (!_clientAuthoritative)
			{
				base.Owner.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"Connection Id {base.Owner.ClientId} has been kicked for trying to update this object without client authority.");
			}
			else
			{
				SetIntervalInternal(value);
			}
		}

		private void RpcReader___Server_ServerSetInterval_1246646286(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			byte value = GeneratedReaders___Internal.InstancedExtension___ReadByte(PooledReader0);
			if (base.IsServerInitialized && OwnerMatches(conn) && !conn.IsLocalClient)
			{
				RpcLogic___ServerSetInterval_1246646286(value);
			}
		}

		private void RpcWriter___Observers_ObserversSetInterval_1246646286(byte value)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else
			{
				Channel channel = Channel.Reliable;
				PooledWriter writer = WriterPool.GetWriter();
				GeneratedWriters___Internal.InstancedExtension___WriteByte(writer, value);
				SendObserversRpc(2u, writer, channel, DataOrderType.Default, bufferLast: true, excludeServer: false, excludeOwner: false);
				writer.Store();
			}
		}

		private void RpcLogic___ObserversSetInterval_1246646286(byte value)
		{
			SetIntervalInternal(value);
		}

		private void RpcReader___Observers_ObserversSetInterval_1246646286(PooledReader PooledReader0, Channel channel)
		{
			byte value = GeneratedReaders___Internal.InstancedExtension___ReadByte(PooledReader0);
			if (base.IsClientInitialized && !base.IsHost)
			{
				RpcLogic___ObserversSetInterval_1246646286(value);
			}
		}

		private void RpcWriter___Target_TargetUpdateTransform_748863190(NetworkConnection conn, ArraySegment<byte> data, Channel channel)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else
			{
				Channel channel2 = channel;
				PooledWriter writer = WriterPool.GetWriter();
				GeneratedWriters___Internal.InstancedExtension___WriteArraySegmentAndSize(writer, data);
				SendTargetRpc(3u, writer, channel2, DataOrderType.Default, conn, excludeServer: false, validateTarget: false);
				writer.Store();
			}
		}

		private void RpcLogic___TargetUpdateTransform_748863190(NetworkConnection conn, ArraySegment<byte> data, Channel channel)
		{
			if (data.Count != 0)
			{
				DataReceived(data, channel, asServer: false);
			}
		}

		private void RpcReader___Target_TargetUpdateTransform_748863190(PooledReader PooledReader0, Channel channel)
		{
			ArraySegment<byte> data = GeneratedReaders___Internal.InstancedExtension___ReadArraySegmentAndSize(PooledReader0);
			if (base.IsClientInitialized)
			{
				RpcLogic___TargetUpdateTransform_748863190(base.LocalConnection, data, channel);
			}
		}

		private void RpcWriter___Observers_ObserversUpdateClientAuthoritativeTransform_2713644489(ArraySegment<byte> data, Channel channel)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else
			{
				Channel channel2 = channel;
				PooledWriter writer = WriterPool.GetWriter();
				GeneratedWriters___Internal.InstancedExtension___WriteArraySegmentAndSize(writer, data);
				SendObserversRpc(4u, writer, channel2, DataOrderType.Default, bufferLast: false, excludeServer: false, excludeOwner: false);
				writer.Store();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void RpcLogic___ObserversUpdateClientAuthoritativeTransform_2713644489(ArraySegment<byte> data, Channel channel)
		{
			if ((_clientAuthoritative || !base.IsOwner || _sendToOwner) && (!_clientAuthoritative || !base.IsOwner) && !base.IsServerInitialized)
			{
				uint lastPacketTick = base.TimeManager.LastPacketTick;
				if (lastPacketTick > _lastObserversRpcTick)
				{
					_lastObserversRpcTick = lastPacketTick;
					DataReceived(data, channel, asServer: false);
				}
			}
		}

		private void RpcReader___Observers_ObserversUpdateClientAuthoritativeTransform_2713644489(PooledReader PooledReader0, Channel channel)
		{
			ArraySegment<byte> data = GeneratedReaders___Internal.InstancedExtension___ReadArraySegmentAndSize(PooledReader0);
			if (base.IsClientInitialized)
			{
				RpcLogic___ObserversUpdateClientAuthoritativeTransform_2713644489(data, channel);
			}
		}

		private void RpcWriter___Server_ServerUpdateTransform_2713644489(ArraySegment<byte> data, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if ((object)networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
			}
			else
			{
				Channel channel2 = channel;
				PooledWriter writer = WriterPool.GetWriter();
				GeneratedWriters___Internal.InstancedExtension___WriteArraySegmentAndSize(writer, data);
				SendServerRpc(5u, writer, channel2, DataOrderType.Default);
				writer.Store();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void RpcLogic___ServerUpdateTransform_2713644489(ArraySegment<byte> data, Channel channel)
		{
			if (!_clientAuthoritative)
			{
				base.Owner.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"Connection Id {base.Owner.ClientId} has been kicked for trying to update this object without client authority.");
				return;
			}
			uint lastPacketTick = base.TimeManager.LastPacketTick;
			if (lastPacketTick > _lastServerRpcTick)
			{
				_lastServerRpcTick = lastPacketTick;
				_authoritativeClientData.Update(data, channel, updateHasData: true);
				DataReceived(data, channel, asServer: true);
			}
		}

		private void RpcReader___Server_ServerUpdateTransform_2713644489(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ArraySegment<byte> data = GeneratedReaders___Internal.InstancedExtension___ReadArraySegmentAndSize(PooledReader0);
			if (base.IsServerInitialized && OwnerMatches(conn))
			{
				RpcLogic___ServerUpdateTransform_2713644489(data, channel);
			}
		}

		private void RpcWriter___Server_ServerSetSynchronizedProperties_535967898(SynchronizedProperty value)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if ((object)networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
			}
			else
			{
				Channel channel = Channel.Reliable;
				PooledWriter writer = WriterPool.GetWriter();
				GeneratedWriters___Internal.Write___FishNet_002EComponent_002ETransforming_002ESynchronizedPropertyFishNet_002ESerializing_002EGenerated(writer, value);
				SendServerRpc(6u, writer, channel, DataOrderType.Default);
				writer.Store();
			}
		}

		private void RpcLogic___ServerSetSynchronizedProperties_535967898(SynchronizedProperty value)
		{
			if (!_clientAuthoritative)
			{
				base.Owner.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"Connection Id {base.Owner.ClientId} has been kicked for trying to update this object without client authority.");
				return;
			}
			SetSynchronizedPropertiesInternal(value);
			ObserversSetSynchronizedProperties(value);
		}

		private void RpcReader___Server_ServerSetSynchronizedProperties_535967898(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			SynchronizedProperty value = GeneratedReaders___Internal.Read___FishNet_002EComponent_002ETransforming_002ESynchronizedPropertyFishNet_002ESerializing_002EGenerateds(PooledReader0);
			if (base.IsServerInitialized && OwnerMatches(conn))
			{
				RpcLogic___ServerSetSynchronizedProperties_535967898(value);
			}
		}

		private void RpcWriter___Observers_ObserversSetSynchronizedProperties_535967898(SynchronizedProperty value)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else
			{
				Channel channel = Channel.Reliable;
				PooledWriter writer = WriterPool.GetWriter();
				GeneratedWriters___Internal.Write___FishNet_002EComponent_002ETransforming_002ESynchronizedPropertyFishNet_002ESerializing_002EGenerated(writer, value);
				SendObserversRpc(7u, writer, channel, DataOrderType.Default, bufferLast: true, excludeServer: false, excludeOwner: false);
				writer.Store();
			}
		}

		private void RpcLogic___ObserversSetSynchronizedProperties_535967898(SynchronizedProperty value)
		{
			if (!base.IsServerInitialized)
			{
				SetSynchronizedPropertiesInternal(value);
			}
		}

		private void RpcReader___Observers_ObserversSetSynchronizedProperties_535967898(PooledReader PooledReader0, Channel channel)
		{
			SynchronizedProperty value = GeneratedReaders___Internal.Read___FishNet_002EComponent_002ETransforming_002ESynchronizedPropertyFishNet_002ESerializing_002EGenerateds(PooledReader0);
			if (base.IsClientInitialized)
			{
				RpcLogic___ObserversSetSynchronizedProperties_535967898(value);
			}
		}

		private void Awake_UserLogic_FishNet_002EComponent_002ETransforming_002ENetworkTransform_FishNet_002ERuntime_002Edll()
		{
			_interval = Math.Max(_interval, (byte)1);
		}
	}
}
