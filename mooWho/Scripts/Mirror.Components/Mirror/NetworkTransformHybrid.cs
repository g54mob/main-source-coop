using System.Collections.Generic;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror
{
	[AddComponentMenu("Network/Network Transform Hybrid")]
	public class NetworkTransformHybrid : NetworkBehaviourHybrid
	{
		public bool useFixedUpdate;

		private TransformSnapshot? pendingSnapshot;

		[Header("Target")]
		[Tooltip("The Transform component to sync. May be on this GameObject, or on a child.")]
		public Transform target;

		[Tooltip("Buffer size limit to avoid ever growing list memory consumption attacks.")]
		public int bufferSizeLimit = 64;

		internal SortedList<double, TransformSnapshot> clientSnapshots = new SortedList<double, TransformSnapshot>();

		internal SortedList<double, TransformSnapshot> serverSnapshots = new SortedList<double, TransformSnapshot>();

		[Header("Synchronization")]
		[Tooltip("Send N snapshots per second. Multiples of frame rate make sense.")]
		public int sendRate = 30;

		private Vector3 lastSerializedBaselinePosition = Vector3.zero;

		private Quaternion lastSerializedBaselineRotation = Quaternion.identity;

		private Vector3 lastSerializedBaselineScale = Vector3.one;

		private Vector3 lastDeserializedBaselinePosition = Vector3.zero;

		private Quaternion lastDeserializedBaselineRotation = Quaternion.identity;

		private Vector3 lastDeserializedBaselineScale = Vector3.one;

		[Header("Sensitivity")]
		[Tooltip("Sensitivity of changes needed before an updated state is sent over the network")]
		public float positionSensitivity = 0.01f;

		public float rotationSensitivity = 0.01f;

		public float scaleSensitivity = 0.01f;

		[Header("Selective Sync & interpolation")]
		public bool syncPosition = true;

		public bool syncRotation = true;

		public bool syncScale;

		[Header("Debug")]
		public bool debugDraw;

		public bool showGizmos;

		public bool showOverlay;

		public Color overlayColor = new Color(0f, 0f, 0f, 0.5f);

		public float sendInterval => 1f / (float)sendRate;

		public Vector3 velocity { get; private set; }

		public Vector3 angularVelocity { get; private set; }

		protected virtual void Awake()
		{
		}

		protected override void OnValidate()
		{
			if (!Application.isPlaying)
			{
				base.OnValidate();
				Reset();
			}
		}

		private void Reset()
		{
			if (target == null)
			{
				target = base.transform;
			}
			syncInterval = sendInterval;
			syncDirection = SyncDirection.ClientToServer;
		}

		protected virtual void ApplySnapshot(TransformSnapshot interpolated)
		{
			if (!base.isOwned && Time.deltaTime > 0f)
			{
				velocity = (base.transform.localPosition - interpolated.position) / Time.deltaTime;
				angularVelocity = (base.transform.localRotation.eulerAngles - interpolated.rotation.eulerAngles) / Time.deltaTime;
			}
			if (syncPosition)
			{
				target.localPosition = interpolated.position;
			}
			if (syncRotation)
			{
				target.localRotation = interpolated.rotation;
			}
			if (syncScale)
			{
				target.localScale = interpolated.scale;
			}
		}

		protected override void StoreState()
		{
			target.GetLocalPositionAndRotation(out lastSerializedBaselinePosition, out lastSerializedBaselineRotation);
			lastSerializedBaselineScale = target.localScale;
		}

		protected override bool StateChanged()
		{
			target.GetLocalPositionAndRotation(out var localPosition, out var localRotation);
			Vector3 localScale = target.localScale;
			if (syncPosition && Vector3.Distance(localPosition, lastSerializedBaselinePosition) >= positionSensitivity)
			{
				return true;
			}
			if (syncRotation && Quaternion.Angle(lastSerializedBaselineRotation, localRotation) >= rotationSensitivity)
			{
				return true;
			}
			if (syncScale && Vector3.Distance(localScale, lastSerializedBaselineScale) >= scaleSensitivity)
			{
				return true;
			}
			return false;
		}

		protected override void OnSerializeBaseline(NetworkWriter writer)
		{
			target.GetLocalPositionAndRotation(out var localPosition, out var localRotation);
			Vector3 localScale = target.localScale;
			if (syncPosition)
			{
				writer.WriteVector3(localPosition);
			}
			if (syncRotation)
			{
				writer.WriteQuaternion(localRotation);
			}
			if (syncScale)
			{
				writer.WriteVector3(localScale);
			}
		}

		protected override void OnDeserializeBaseline(NetworkReader reader, byte baselineTick)
		{
			Vector3? position = null;
			Quaternion? rotation = null;
			Vector3? scale = null;
			if (syncPosition)
			{
				position = reader.ReadVector3();
				lastDeserializedBaselinePosition = position.Value;
			}
			if (syncRotation)
			{
				rotation = reader.ReadQuaternion();
				lastDeserializedBaselineRotation = rotation.Value;
			}
			if (syncScale)
			{
				scale = reader.ReadVector3();
				lastDeserializedBaselineScale = scale.Value;
			}
			if (debugDraw && position.HasValue)
			{
				Debug.DrawLine(position.Value, position.Value + Vector3.up, Color.yellow, 10f);
			}
			if (baselineIsDelta)
			{
				if (base.isServer)
				{
					OnClientToServerDeltaSync(position, rotation, scale);
				}
				else if (base.isClient)
				{
					OnServerToClientDeltaSync(position, rotation, scale);
				}
			}
		}

		protected override void OnSerializeDelta(NetworkWriter writer)
		{
			target.GetLocalPositionAndRotation(out var localPosition, out var localRotation);
			Vector3 localScale = target.localScale;
			if (syncPosition)
			{
				writer.WriteVector3(localPosition);
			}
			if (syncRotation)
			{
				writer.WriteQuaternion(localRotation);
			}
			if (syncScale)
			{
				writer.WriteVector3(localScale);
			}
		}

		protected override void OnDeserializeDelta(NetworkReader reader, byte baselineTick)
		{
			Vector3? position = null;
			Quaternion? rotation = null;
			Vector3? scale = null;
			if (syncPosition)
			{
				position = reader.ReadVector3();
			}
			if (syncRotation)
			{
				rotation = reader.ReadQuaternion();
			}
			if (syncScale)
			{
				scale = reader.ReadVector3();
			}
			if (debugDraw && position.HasValue)
			{
				Debug.DrawLine(position.Value, position.Value + Vector3.up, Color.white, 10f);
			}
			if (base.isServer)
			{
				OnClientToServerDeltaSync(position, rotation, scale);
			}
			else if (base.isClient)
			{
				OnServerToClientDeltaSync(position, rotation, scale);
			}
		}

		protected virtual void OnClientToServerDeltaSync(Vector3? position, Quaternion? rotation, Vector3? scale)
		{
			if (syncDirection == SyncDirection.ClientToServer && serverSnapshots.Count < base.connectionToClient.snapshotBufferSizeLimit)
			{
				double remoteTimeStamp = base.connectionToClient.remoteTimeStamp;
				SnapshotInterpolation.InsertIfNotExists(serverSnapshots, bufferSizeLimit, new TransformSnapshot(remoteTimeStamp, NetworkTime.localTime, position.HasValue ? position.Value : Vector3.zero, rotation.HasValue ? rotation.Value : Quaternion.identity, scale.HasValue ? scale.Value : Vector3.one));
			}
		}

		protected virtual void OnServerToClientDeltaSync(Vector3? position, Quaternion? rotation, Vector3? scale)
		{
			if (!base.isServer && !base.IsClientWithAuthority)
			{
				double remoteTimeStamp = NetworkClient.connection.remoteTimeStamp;
				SnapshotInterpolation.InsertIfNotExists(clientSnapshots, bufferSizeLimit, new TransformSnapshot(remoteTimeStamp, NetworkTime.localTime, position.HasValue ? position.Value : Vector3.zero, rotation.HasValue ? rotation.Value : Quaternion.identity, scale.HasValue ? scale.Value : Vector3.one));
			}
		}

		private void UpdateServerInterpolation()
		{
			if (syncDirection == SyncDirection.ClientToServer && !base.isOwned && serverSnapshots.Count > 0)
			{
				SnapshotInterpolation.StepInterpolation(serverSnapshots, base.connectionToClient.remoteTimeline - (double)sendInterval, out var fromSnapshot, out var toSnapshot, out var t);
				TransformSnapshot transformSnapshot = TransformSnapshot.Interpolate(fromSnapshot, toSnapshot, t);
				if (useFixedUpdate)
				{
					pendingSnapshot = transformSnapshot;
				}
				else
				{
					ApplySnapshot(transformSnapshot);
				}
			}
		}

		private void UpdateClientInterpolation()
		{
			if (clientSnapshots.Count > 0)
			{
				SnapshotInterpolation.StepInterpolation(clientSnapshots, NetworkTime.time - (double)sendInterval, out var fromSnapshot, out var toSnapshot, out var t);
				TransformSnapshot transformSnapshot = TransformSnapshot.Interpolate(fromSnapshot, toSnapshot, t);
				if (useFixedUpdate)
				{
					pendingSnapshot = transformSnapshot;
				}
				else
				{
					ApplySnapshot(transformSnapshot);
				}
			}
		}

		protected override void Update()
		{
			base.Update();
			if (base.isServer)
			{
				UpdateServerInterpolation();
			}
			else if (base.isClient && !base.IsClientWithAuthority)
			{
				UpdateClientInterpolation();
			}
		}

		private void FixedUpdate()
		{
			if (useFixedUpdate && pendingSnapshot.HasValue)
			{
				ApplySnapshot(pendingSnapshot.Value);
				pendingSnapshot = null;
			}
		}

		protected virtual void OnTeleport(Vector3 destination)
		{
			ResetState();
			target.position = destination;
		}

		protected virtual void OnTeleport(Vector3 destination, Quaternion rotation)
		{
			ResetState();
			target.position = destination;
			target.rotation = rotation;
		}

		[ClientRpc]
		public void RpcTeleport(Vector3 destination)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(destination);
			SendRPCInternal("System.Void Mirror.NetworkTransformHybrid::RpcTeleport(UnityEngine.Vector3)", 1743702279, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		public void RpcTeleport(Vector3 destination, Quaternion rotation)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(destination);
			writer.WriteQuaternion(rotation);
			SendRPCInternal("System.Void Mirror.NetworkTransformHybrid::RpcTeleport(UnityEngine.Vector3,UnityEngine.Quaternion)", 2143998938, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		public void CmdTeleport(Vector3 destination)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(destination);
			SendCommandInternal("System.Void Mirror.NetworkTransformHybrid::CmdTeleport(UnityEngine.Vector3)", 1347595190, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		public void CmdTeleport(Vector3 destination, Quaternion rotation)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(destination);
			writer.WriteQuaternion(rotation);
			SendCommandInternal("System.Void Mirror.NetworkTransformHybrid::CmdTeleport(UnityEngine.Vector3,UnityEngine.Quaternion)", 1728720081, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ServerTeleport(Vector3 destination, Quaternion rotation)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void Mirror.NetworkTransformHybrid::ServerTeleport(UnityEngine.Vector3,UnityEngine.Quaternion)' called when server was not active");
				return;
			}
			OnTeleport(destination, rotation);
			RpcTeleport(destination, rotation);
		}

		public override void ResetState()
		{
			base.ResetState();
			serverSnapshots.Clear();
			clientSnapshots.Clear();
			lastSerializedBaselinePosition = Vector3.zero;
			lastSerializedBaselineRotation = Quaternion.identity;
			lastSerializedBaselineScale = Vector3.one;
			lastDeserializedBaselinePosition = Vector3.zero;
			lastDeserializedBaselineRotation = Quaternion.identity;
			lastDeserializedBaselineScale = Vector3.one;
			Physics.SyncTransforms();
		}

		protected virtual void OnDisable()
		{
			ResetState();
		}

		protected virtual void OnEnable()
		{
			ResetState();
		}

		public override void OnSerialize(NetworkWriter writer, bool initialState)
		{
			base.OnSerialize(writer, initialState);
			if (initialState)
			{
				target.GetLocalPositionAndRotation(out var localPosition, out var localRotation);
				Vector3 localScale = target.localScale;
				if (syncPosition)
				{
					writer.WriteVector3(localPosition);
				}
				if (syncRotation)
				{
					writer.WriteQuaternion(localRotation);
				}
				if (syncScale)
				{
					writer.WriteVector3(localScale);
				}
			}
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			base.OnDeserialize(reader, initialState);
			if (initialState)
			{
				Vector3 value = Vector3.zero;
				Quaternion value2 = Quaternion.identity;
				Vector3 value3 = Vector3.one;
				if (syncPosition)
				{
					value = (lastDeserializedBaselinePosition = reader.ReadVector3());
				}
				if (syncRotation)
				{
					value2 = (lastDeserializedBaselineRotation = reader.ReadQuaternion());
				}
				if (syncScale)
				{
					value3 = (lastDeserializedBaselineScale = reader.ReadVector3());
				}
				if (baselineIsDelta)
				{
					OnServerToClientDeltaSync(value, value2, value3);
				}
			}
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_RpcTeleport__Vector3(Vector3 destination)
		{
			OnTeleport(destination);
		}

		protected static void InvokeUserCode_RpcTeleport__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcTeleport called on server.");
			}
			else
			{
				((NetworkTransformHybrid)obj).UserCode_RpcTeleport__Vector3(reader.ReadVector3());
			}
		}

		protected void UserCode_RpcTeleport__Vector3__Quaternion(Vector3 destination, Quaternion rotation)
		{
			OnTeleport(destination, rotation);
		}

		protected static void InvokeUserCode_RpcTeleport__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcTeleport called on server.");
			}
			else
			{
				((NetworkTransformHybrid)obj).UserCode_RpcTeleport__Vector3__Quaternion(reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		protected void UserCode_CmdTeleport__Vector3(Vector3 destination)
		{
			if (syncDirection == SyncDirection.ClientToServer)
			{
				OnTeleport(destination);
				RpcTeleport(destination);
			}
		}

		protected static void InvokeUserCode_CmdTeleport__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdTeleport called on client.");
			}
			else
			{
				((NetworkTransformHybrid)obj).UserCode_CmdTeleport__Vector3(reader.ReadVector3());
			}
		}

		protected void UserCode_CmdTeleport__Vector3__Quaternion(Vector3 destination, Quaternion rotation)
		{
			if (syncDirection == SyncDirection.ClientToServer)
			{
				OnTeleport(destination, rotation);
				RpcTeleport(destination, rotation);
			}
		}

		protected static void InvokeUserCode_CmdTeleport__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdTeleport called on client.");
			}
			else
			{
				((NetworkTransformHybrid)obj).UserCode_CmdTeleport__Vector3__Quaternion(reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		static NetworkTransformHybrid()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkTransformHybrid), "System.Void Mirror.NetworkTransformHybrid::CmdTeleport(UnityEngine.Vector3)", InvokeUserCode_CmdTeleport__Vector3, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkTransformHybrid), "System.Void Mirror.NetworkTransformHybrid::CmdTeleport(UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_CmdTeleport__Vector3__Quaternion, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkTransformHybrid), "System.Void Mirror.NetworkTransformHybrid::RpcTeleport(UnityEngine.Vector3)", InvokeUserCode_RpcTeleport__Vector3);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkTransformHybrid), "System.Void Mirror.NetworkTransformHybrid::RpcTeleport(UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_RpcTeleport__Vector3__Quaternion);
		}
	}
}
