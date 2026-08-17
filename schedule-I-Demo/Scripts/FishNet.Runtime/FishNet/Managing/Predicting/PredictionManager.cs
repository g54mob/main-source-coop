using System;
using System.Collections.Generic;
using FishNet.Documenting;
using FishNet.Object;
using FishNet.Serializing.Helping;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishNet.Managing.Predicting
{
	[DisallowMultipleComponent]
	[AddComponentMenu("FishNet/Manager/PredictionManager")]
	public sealed class PredictionManager : MonoBehaviour
	{
		private bool _isReplaying;

		[Tooltip("Number of inputs to keep in queue should the server miss receiving an input update from the client. Higher values will increase the likeliness of the server always having input from the client while lower values will allow the client input to run on the server faster. This value cannot be higher than MaximumServerReplicates.")]
		[Range(1f, 15f)]
		[SerializeField]
		private ushort _queuedInputs = 1;

		[Tooltip("True to drop replicates from clients which are being received excessively. This can help with attacks but may cause client to temporarily desynchronize during connectivity issues. When false the server will hold at most up to 3 seconds worth of replicates, consuming multiple per tick to clear out the buffer quicker. This is good to ensure all inputs are executed but potentially could allow speed hacking.")]
		[SerializeField]
		private bool _dropExcessiveReplicates = true;

		[Tooltip("Maximum number of replicates a server can queue per object. Higher values will put more load on the server and add replicate latency for the client.")]
		[SerializeField]
		private ushort _maximumServerReplicates = 15;

		[Tooltip("Maximum number of excessive replicates which can be consumed per tick. Consumption count will scale up to this value automatically.")]
		[SerializeField]
		private byte _maximumConsumeCount = 4;

		[Tooltip("Maximum number of past inputs which may send.")]
		[Range(2f, 15f)]
		[SerializeField]
		private byte _redundancyCount = 2;

		[Tooltip("True to allow clients to use predicted spawning and despawning. While true, each NetworkObject prefab you wish to predicted spawn must be marked as to allow this feature.")]
		[SerializeField]
		private bool _allowPredictedSpawning;

		[Tooltip("Maximum number of Ids to reserve on clients for predicted spawning. Higher values will allow clients to send more predicted spawns per second but may reduce availability of ObjectIds with high player counts.")]
		[Range(1f, 100f)]
		[SerializeField]
		private byte _reservedObjectIds = 15;

		[NonSerialized]
		private HashSet<UnityEngine.Component> _rigidbodies = new HashSet<UnityEngine.Component>();

		[NonSerialized]
		private HashSet<UnityEngine.Component> _componentCache = new HashSet<UnityEngine.Component>();

		private HashSet<Scene> _replayingScenes = new HashSet<Scene>(new SceneHandleEqualityComparer());

		private NetworkManager _networkManager;

		private const byte MINIMUM_PAST_INPUTS = 2;

		internal const byte MAXIMUM_PAST_INPUTS = 15;

		private const ushort MINIMUM_REPLICATE_QUEUE_SIZE = 10;

		private const ushort MAXIMUM_REPLICATE_QUEUE_SIZE = 500;

		public uint LastReconcileTick { get; internal set; }

		public uint LastReplicateTick { get; internal set; }

		internal bool UsingRigidbodies => _rigidbodies.Count > 0;

		public ushort QueuedInputs => (ushort)(_queuedInputs + 1);

		internal bool DropExcessiveReplicates => _dropExcessiveReplicates;

		internal byte MaximumReplicateConsumeCount => _maximumConsumeCount;

		internal ushort MaximumClientReplicates => (ushort)(_networkManager.TimeManager.TickRate * 5);

		internal byte RedundancyCount => _redundancyCount;

		public event Action<NetworkBehaviour> OnPreReconcile;

		public event Action<NetworkBehaviour> OnPostReconcile;

		public event Action<uint, PhysicsScene, PhysicsScene2D> OnPreReplicateReplay;

		public event Action<uint, PhysicsScene, PhysicsScene2D> OnPostReplicateReplay;

		public event Action<NetworkBehaviour> OnPreServerReconcile;

		public event Action<NetworkBehaviour> OnPostServerReconcile;

		public bool IsReplaying()
		{
			return _isReplaying;
		}

		public bool IsReplaying(Scene scene)
		{
			return _replayingScenes.Contains(scene);
		}

		public ushort GetMaximumServerReplicates()
		{
			return _maximumServerReplicates;
		}

		public void SetMaximumServerReplicates(ushort value)
		{
			_maximumServerReplicates = (ushort)Mathf.Clamp(value, 10, 500);
		}

		internal bool GetAllowPredictedSpawning()
		{
			return _allowPredictedSpawning;
		}

		internal byte GetReservedObjectIds()
		{
			return _reservedObjectIds;
		}

		private void OnEnable()
		{
			SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
		}

		private void OnDisable()
		{
			SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
		}

		internal void InitializeOnce(NetworkManager manager)
		{
			_networkManager = manager;
			_networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
		}

		private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
		{
			if (obj.ConnectionState != LocalConnectionState.Started)
			{
				_replayingScenes.Clear();
			}
			_isReplaying = false;
		}

		internal void InvokeServerReconcile(NetworkBehaviour caller, bool before)
		{
			if (before)
			{
				this.OnPreServerReconcile?.Invoke(caller);
			}
			else
			{
				this.OnPostServerReconcile?.Invoke(caller);
			}
		}

		[APIExclude]
		public void AddRigidbodyCount(UnityEngine.Component c)
		{
			_rigidbodies.Add(c);
		}

		[APIExclude]
		public void RemoveRigidbodyCount(UnityEngine.Component c)
		{
			if (_rigidbodies.Remove(c))
			{
				return;
			}
			_componentCache.Clear();
			foreach (UnityEngine.Component rigidbody in _rigidbodies)
			{
				if (rigidbody != null)
				{
					_componentCache.Add(rigidbody);
				}
			}
			_rigidbodies.Clear();
			foreach (UnityEngine.Component item in _componentCache)
			{
				_rigidbodies.Add(item);
			}
		}

		[APIExclude]
		[CodegenMakePublic]
		public void InvokeOnReconcile(NetworkBehaviour nb, bool before)
		{
			nb.IsReconciling = before;
			if (before)
			{
				this.OnPreReconcile?.Invoke(nb);
			}
			else
			{
				this.OnPostReconcile?.Invoke(nb);
			}
		}

		[APIExclude]
		internal void InvokeOnReplicateReplay(Scene scene, uint tick, PhysicsScene ps, PhysicsScene2D ps2d, bool before)
		{
			_isReplaying = before;
			if (before)
			{
				_replayingScenes.Add(scene);
				this.OnPreReplicateReplay?.Invoke(tick, ps, ps2d);
			}
			else
			{
				_replayingScenes.Remove(scene);
				this.OnPostReplicateReplay?.Invoke(tick, ps, ps2d);
			}
		}

		private void SceneManager_sceneUnloaded(Scene s)
		{
			_replayingScenes.Remove(s);
		}
	}
}
