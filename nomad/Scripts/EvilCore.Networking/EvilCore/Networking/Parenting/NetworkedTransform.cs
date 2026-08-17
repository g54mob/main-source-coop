using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;
using UnityEngine.Animations;
using VContainer;

namespace EvilCore.Networking.Parenting
{
	[RequireComponent(typeof(ParentConstraint))]
	public class NetworkedTransform : NetworkBehaviour
	{
		[SerializeField]
		private NetworkedTransform _root;

		[SerializeField]
		private NetworkedTransformType _networkedTransformType;

		[SerializeField]
		private byte _networkedTransformIndex;

		[SerializeField]
		private uint _parentNetworkedTransformNetId;

		[SerializeField]
		private Transform _networkedParentTransform;

		[Tooltip("If set, parenting will sync to this transform instead of the object's own transform")]
		public Transform alternativeSyncTransform;

		[Header("Parent Lookup Retry Settings")]
		[Tooltip("Maximum retry attempts when parent object not found")]
		[SerializeField]
		[Range(1f, 100f)]
		private int maxParentLookupRetries = 30;

		[Tooltip("Initial delay before first retry (seconds)")]
		[SerializeField]
		[Range(0.05f, 1f)]
		private float parentLookupInitialDelay = 0.1f;

		[Tooltip("Maximum delay cap between retries (seconds)")]
		[SerializeField]
		[Range(0.5f, 5f)]
		private float parentLookupMaxDelay = 2f;

		[Tooltip("Multiplier for exponential backoff")]
		[SerializeField]
		[Range(1f, 2f)]
		private float parentLookupBackoffMultiplier = 1.5f;

		[Tooltip("Register with SpawnWatcher for fallback recovery if primary lookup fails")]
		[SerializeField]
		private bool enableFallbackRecovery = true;

		private readonly HashSet<NetworkedTransform> _subNetworkedTransforms = new HashSet<NetworkedTransform>();

		private ParentConstraint _parentConstraint;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private INetworkObjectSpawnWatcher _spawnWatcher;

		private uint _pendingFallbackNetId;

		private Action<GameObject> _pendingFallbackCallback;

		[SyncVar(hook = "OnNetworkedParentTransformDataChanged")]
		private NetworkedParentTransformData _networkedParentTransformData;

		public Action<NetworkedParentTransformData, NetworkedParentTransformData> _Mirror_SyncVarHookDelegate__networkedParentTransformData;

		public NetworkedTransform Root => _root;

		public byte NetworkedTransformIndex => _networkedTransformIndex;

		public NetworkedTransformType TransformType => _networkedTransformType;

		public uint ParentNetId => _parentNetworkedTransformNetId;

		public Transform ParentTransform => _networkedParentTransform;

		public bool HasParent => _parentNetworkedTransformNetId != 0;

		public ParentConstraint ParentConstraint => _parentConstraint;

		public NetworkedParentTransformData SyncedParentData => _networkedParentTransformData;

		public IReadOnlyCollection<NetworkedTransform> SubNetworkedTransforms => _subNetworkedTransforms;

		public NetworkedParentTransformData Network_networkedParentTransformData
		{
			get
			{
				return _networkedParentTransformData;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _networkedParentTransformData, 1uL, _Mirror_SyncVarHookDelegate__networkedParentTransformData);
			}
		}

		private void Awake()
		{
			if (TryGetComponent<NetworkIdentity>(out var _))
			{
				_networkedTransformType = NetworkedTransformType.Root;
				_networkedTransformIndex = 0;
				NetworkedTransform[] componentsInChildren = GetComponentsInChildren<NetworkedTransform>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i]._networkedTransformIndex = (byte)i;
					componentsInChildren[i]._root = this;
					_subNetworkedTransforms.Add(componentsInChildren[i]);
				}
			}
			else
			{
				_networkedTransformType = NetworkedTransformType.Child;
			}
			_parentConstraint = (TryGetComponent<ParentConstraint>(out var component2) ? component2 : base.gameObject.AddComponent<ParentConstraint>());
		}

		private void OnDestroy()
		{
			UnregisterPendingFallback();
		}

		private void OnNetworkedParentTransformDataChanged(NetworkedParentTransformData _, NetworkedParentTransformData newData)
		{
			uint parentNetworkedTransformNetId = newData.ParentNetworkedTransformNetId;
			byte subTransformIndex = newData.SubTransformIndex;
			if (parentNetworkedTransformNetId == 0 && subTransformIndex == 255)
			{
				_parentNetworkedTransformNetId = 0u;
				_networkedParentTransform = null;
				DeactivateParentConstraint();
				RemoveAllSourcesFromParentConstraint();
			}
			else
			{
				RetryParentLookupAsync(newData).Forget();
			}
		}

		public void SetParent(NetworkedTransform parent, NetworkedTransformParentingConfig parentingConfig = default(NetworkedTransformParentingConfig), byte subTransformIndex = 0)
		{
			if (subTransformIndex == 255)
			{
				EvilLogger.LogError("Sub transform index cannot be 255, this means that transform has 255 children!", "SetParent", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\Parenting\\NetworkedTransform.cs", 145);
			}
			else
			{
				CmdSetParent(parent, parentingConfig, subTransformIndex);
			}
		}

		public void ClearParent()
		{
			SetParent(null, default(NetworkedTransformParentingConfig), 0);
		}

		[Server]
		public void ServerSetParent(NetworkedTransform parent, NetworkedTransformParentingConfig parentingConfig = default(NetworkedTransformParentingConfig), byte subTransformIndex = 0)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void EvilCore.Networking.Parenting.NetworkedTransform::ServerSetParent(EvilCore.Networking.Parenting.NetworkedTransform,EvilCore.Networking.Parenting.NetworkedTransformParentingConfig,System.Byte)' called when server was not active");
				return;
			}
			if (subTransformIndex == 255)
			{
				EvilLogger.LogError("Sub transform index cannot be 255, this means that transform has 255 children!", "ServerSetParent", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\Parenting\\NetworkedTransform.cs", 170);
				return;
			}
			NetworkedParentTransformData network_networkedParentTransformData = ((!(parent == null)) ? new NetworkedParentTransformData
			{
				ParentNetworkedTransformNetId = parent.netId,
				SubTransformIndex = subTransformIndex,
				ParentingConfig = parentingConfig
			} : new NetworkedParentTransformData
			{
				ParentNetworkedTransformNetId = 0u,
				SubTransformIndex = 255,
				ParentingConfig = new NetworkedTransformParentingConfig
				{
					PositionOffset = Vector3.zero,
					RotationOffset = Vector3.zero,
					KeepPositionAxes = false,
					KeepRotationAxes = false
				}
			});
			Network_networkedParentTransformData = network_networkedParentTransformData;
		}

		public void ClientForceClearParent()
		{
			_parentNetworkedTransformNetId = 0u;
			_networkedParentTransform = null;
			DeactivateParentConstraint();
			RemoveAllSourcesFromParentConstraint();
		}

		public void ClientApplyParentingImmediate(NetworkedTransform parent, NetworkedTransformParentingConfig config, byte subTransformIndex)
		{
			if (!(parent == null) && !(parent.netIdentity == null))
			{
				NetworkedParentTransformData data = new NetworkedParentTransformData
				{
					ParentNetworkedTransformNetId = parent.netId,
					SubTransformIndex = subTransformIndex,
					ParentingConfig = config
				};
				ApplyParenting(data, parent.netIdentity.gameObject);
			}
		}

		public NetworkedTransform GetChild(byte childIndex)
		{
			return _subNetworkedTransforms.FirstOrDefault((NetworkedTransform child) => child._networkedTransformIndex == childIndex);
		}

		public byte GetChildIndex(NetworkedTransform childTransform)
		{
			return _subNetworkedTransforms.FirstOrDefault((NetworkedTransform child) => child == childTransform)?._networkedTransformIndex ?? 0;
		}

		[Command(requiresAuthority = false)]
		private void CmdSetParent(NetworkedTransform parent, NetworkedTransformParentingConfig parentingConfig, byte subTransformIndex)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteNetworkBehaviour(parent);
			GeneratedNetworkCode._Write_EvilCore_002ENetworking_002EParenting_002ENetworkedTransformParentingConfig(writer, parentingConfig);
			NetworkWriterExtensions.WriteByte(writer, subTransformIndex);
			SendCommandInternal("System.Void EvilCore.Networking.Parenting.NetworkedTransform::CmdSetParent(EvilCore.Networking.Parenting.NetworkedTransform,EvilCore.Networking.Parenting.NetworkedTransformParentingConfig,System.Byte)", 985464272, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void SetParentConstraint(Vector3 positionOffset, Vector3 rotationOffset, bool keepPositionAxes, bool keepRotationAxes)
		{
			_parentConstraint.translationAxis = ((!keepPositionAxes) ? (Axis.X | Axis.Y | Axis.Z) : Axis.None);
			_parentConstraint.rotationAxis = ((!keepRotationAxes) ? (Axis.X | Axis.Y | Axis.Z) : Axis.None);
			_parentConstraint.SetTranslationOffset(0, positionOffset);
			_parentConstraint.SetRotationOffset(0, rotationOffset);
		}

		private void ApplyParenting(NetworkedParentTransformData data, GameObject rootNetworkedTransformObject)
		{
			NetworkedTransform component = rootNetworkedTransformObject.GetComponent<NetworkedTransform>();
			if (component == null)
			{
				EvilLogger.LogError("[NetworkedTransform] Root object " + rootNetworkedTransformObject.name + " does not have NetworkedTransform component", "ApplyParenting", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\Parenting\\NetworkedTransform.cs", 327);
				return;
			}
			NetworkedTransform child = component.GetChild(data.SubTransformIndex);
			if (child == null)
			{
				EvilLogger.LogError($"[NetworkedTransform] Child with index {data.SubTransformIndex} not found on {component.name}", "ApplyParenting", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\Parenting\\NetworkedTransform.cs", 335);
				return;
			}
			Transform transform = child.alternativeSyncTransform;
			if (transform != null)
			{
				_networkedParentTransform = transform;
			}
			else
			{
				_networkedParentTransform = child.transform;
			}
			_parentNetworkedTransformNetId = data.ParentNetworkedTransformNetId;
			RemoveAllSourcesFromParentConstraint();
			AddNewSourceToParentConstraint(_networkedParentTransform);
			SetParentConstraint(data.ParentingConfig.PositionOffset, data.ParentingConfig.RotationOffset, data.ParentingConfig.KeepPositionAxes, data.ParentingConfig.KeepRotationAxes);
			ActivateParentConstraint();
		}

		private async UniTaskVoid RetryParentLookupAsync(NetworkedParentTransformData data)
		{
			UnregisterPendingFallback();
			int initialDelayMs = Mathf.RoundToInt(parentLookupInitialDelay * 1000f);
			int maxDelayMs = Mathf.RoundToInt(parentLookupMaxDelay * 1000f);
			(bool, GameObject) tuple = await _networkManager.TryGetNetworkObjectByIdWithBackoffAsync(data.ParentNetworkedTransformNetId, maxParentLookupRetries, initialDelayMs, maxDelayMs, parentLookupBackoffMultiplier, this.GetCancellationTokenOnDestroy());
			if (tuple.Item1 && tuple.Item2 != null)
			{
				ApplyParenting(data, tuple.Item2);
			}
			else if (enableFallbackRecovery && _spawnWatcher != null)
			{
				RegisterPendingFallback(data);
			}
			else
			{
				EvilLogger.LogError($"<color=red>[NetworkedTransform]</color> Failed to find parent (netId: {data.ParentNetworkedTransformNetId}) for {base.gameObject.name} after {maxParentLookupRetries} retries. Fallback recovery disabled or unavailable.", "RetryParentLookupAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\Parenting\\NetworkedTransform.cs", 400);
			}
		}

		private void RegisterPendingFallback(NetworkedParentTransformData data)
		{
			NetworkedParentTransformData capturedData = data;
			_pendingFallbackNetId = data.ParentNetworkedTransformNetId;
			_pendingFallbackCallback = delegate(GameObject spawnedObject)
			{
				OnFallbackParentFound(capturedData, spawnedObject);
			};
			_spawnWatcher.RegisterPendingLookup(_pendingFallbackNetId, _pendingFallbackCallback, base.gameObject.name);
		}

		private void UnregisterPendingFallback()
		{
			if (_pendingFallbackNetId != 0 && _pendingFallbackCallback != null && _spawnWatcher != null)
			{
				_spawnWatcher.UnregisterPendingLookup(_pendingFallbackNetId, _pendingFallbackCallback);
			}
			_pendingFallbackNetId = 0u;
			_pendingFallbackCallback = null;
		}

		private void OnFallbackParentFound(NetworkedParentTransformData data, GameObject parentObject)
		{
			_pendingFallbackNetId = 0u;
			_pendingFallbackCallback = null;
			if (!(this == null) && !(base.gameObject == null))
			{
				ApplyParenting(data, parentObject);
			}
		}

		private void AddNewSourceToParentConstraint(Transform attacherTransform)
		{
			ConstraintSource source = new ConstraintSource
			{
				sourceTransform = attacherTransform,
				weight = 1f
			};
			_parentConstraint.AddSource(source);
		}

		private void RemoveAllSourcesFromParentConstraint()
		{
			for (int num = _parentConstraint.sourceCount - 1; num >= 0; num--)
			{
				_parentConstraint.RemoveSource(num);
			}
		}

		private void ActivateParentConstraint()
		{
			_parentConstraint.constraintActive = true;
			_parentConstraint.locked = true;
		}

		private void DeactivateParentConstraint()
		{
			_parentConstraint.constraintActive = false;
			_parentConstraint.locked = false;
		}

		public NetworkedTransform()
		{
			_Mirror_SyncVarHookDelegate__networkedParentTransformData = OnNetworkedParentTransformDataChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSetParent__NetworkedTransform__NetworkedTransformParentingConfig__Byte(NetworkedTransform parent, NetworkedTransformParentingConfig parentingConfig, byte subTransformIndex)
		{
			_ = parent != null;
			NetworkedParentTransformData network_networkedParentTransformData = ((!(parent == null)) ? new NetworkedParentTransformData
			{
				ParentNetworkedTransformNetId = parent.netId,
				SubTransformIndex = subTransformIndex,
				ParentingConfig = new NetworkedTransformParentingConfig
				{
					PositionOffset = parentingConfig.PositionOffset,
					RotationOffset = parentingConfig.RotationOffset,
					KeepPositionAxes = parentingConfig.KeepPositionAxes,
					KeepRotationAxes = parentingConfig.KeepRotationAxes
				}
			} : new NetworkedParentTransformData
			{
				ParentNetworkedTransformNetId = 0u,
				SubTransformIndex = 255,
				ParentingConfig = new NetworkedTransformParentingConfig
				{
					PositionOffset = Vector3.zero,
					RotationOffset = Vector3.zero,
					KeepPositionAxes = false,
					KeepRotationAxes = false
				}
			});
			Network_networkedParentTransformData = network_networkedParentTransformData;
		}

		protected static void InvokeUserCode_CmdSetParent__NetworkedTransform__NetworkedTransformParentingConfig__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetParent called on client.");
			}
			else
			{
				((NetworkedTransform)obj).UserCode_CmdSetParent__NetworkedTransform__NetworkedTransformParentingConfig__Byte(reader.ReadNetworkBehaviour<NetworkedTransform>(), GeneratedNetworkCode._Read_EvilCore_002ENetworking_002EParenting_002ENetworkedTransformParentingConfig(reader), NetworkReaderExtensions.ReadByte(reader));
			}
		}

		static NetworkedTransform()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedTransform), "System.Void EvilCore.Networking.Parenting.NetworkedTransform::CmdSetParent(EvilCore.Networking.Parenting.NetworkedTransform,EvilCore.Networking.Parenting.NetworkedTransformParentingConfig,System.Byte)", InvokeUserCode_CmdSetParent__NetworkedTransform__NetworkedTransformParentingConfig__Byte, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				GeneratedNetworkCode._Write_EvilCore_002ENetworking_002EParenting_002ENetworkedParentTransformData(writer, _networkedParentTransformData);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				GeneratedNetworkCode._Write_EvilCore_002ENetworking_002EParenting_002ENetworkedParentTransformData(writer, _networkedParentTransformData);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _networkedParentTransformData, _Mirror_SyncVarHookDelegate__networkedParentTransformData, GeneratedNetworkCode._Read_EvilCore_002ENetworking_002EParenting_002ENetworkedParentTransformData(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _networkedParentTransformData, _Mirror_SyncVarHookDelegate__networkedParentTransformData, GeneratedNetworkCode._Read_EvilCore_002ENetworking_002EParenting_002ENetworkedParentTransformData(reader));
			}
		}
	}
}
