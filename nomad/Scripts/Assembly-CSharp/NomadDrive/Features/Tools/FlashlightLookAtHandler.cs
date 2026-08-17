using System;
using System.Runtime.InteropServices;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Extensions;
using EvilCore.Networking.Parenting;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Player;
using UnityEngine;
using UnityEngine.Animations;
using VContainer;

namespace NomadDrive.Features.Tools
{
	public class FlashlightLookAtHandler : NetworkBehaviour
	{
		[SerializeField]
		private FlashlightLookConfig config;

		[SerializeField]
		private Transform beamPivot;

		[SerializeField]
		private bool debugMode;

		[Inject]
		private IPlayerService playerService;

		[SyncVar(hook = "OnAimOffsetHookChanged")]
		private Vector3 _syncedAimOffset = Vector3.zero;

		private NetworkedTransform _networkedTransform;

		private HeldItem _heldItem;

		private Quaternion _restBeamLocal = Quaternion.identity;

		private Quaternion _currentItemWorld = Quaternion.identity;

		private bool _worldInitialized;

		private Quaternion _currentRemoteOffset = Quaternion.identity;

		private Vector3 _currentOffset = Vector3.zero;

		private Vector3 _remoteTargetOffset = Vector3.zero;

		private Vector3 _lastSentOffset = Vector3.zero;

		private bool _hasRemoteTarget;

		private bool _lateJoinDone;

		private float _sendTimer;

		private float _heartbeatTimer;

		private float _debugTimer;

		private const float SendInterval = 1f / 12f;

		private const float SendHeartbeat = 0.5f;

		private const float OffsetEpsilonDeg = 0.5f;

		private const float DebugInterval = 1f;

		public Action<Vector3, Vector3> _Mirror_SyncVarHookDelegate__syncedAimOffset;

		private bool IsEquippedNow
		{
			get
			{
				if (_heldItem != null)
				{
					return _heldItem.IsEquipped;
				}
				return false;
			}
		}

		public Vector3 Network_syncedAimOffset
		{
			get
			{
				return _syncedAimOffset;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedAimOffset, 1uL, _Mirror_SyncVarHookDelegate__syncedAimOffset);
			}
		}

		private void Awake()
		{
			base.gameObject.InjectGameObject();
			_heldItem = GetComponent<HeldItem>();
			_networkedTransform = GetComponent<NetworkedTransform>();
			if (config == null)
			{
				EvilLogger.LogError("FlashlightLookAtHandler requires a FlashlightLookConfig", "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Flashlight\\Scripts\\FlashlightLookAtHandler.cs", 67);
				base.enabled = false;
			}
			else if (_networkedTransform == null)
			{
				EvilLogger.LogError("FlashlightLookAtHandler requires a NetworkedTransform on the same object", "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Flashlight\\Scripts\\FlashlightLookAtHandler.cs", 74);
				base.enabled = false;
			}
			else
			{
				_restBeamLocal = ((beamPivot != null) ? beamPivot.localRotation : Quaternion.identity);
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			_lateJoinDone = true;
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!base.isOwned && IsEquippedNow)
			{
				ParentConstraint parentConstraint = _networkedTransform.ParentConstraint;
				if (parentConstraint != null && parentConstraint.sourceCount > 0)
				{
					_currentRemoteOffset = Quaternion.Euler(_syncedAimOffset);
					_remoteTargetOffset = _syncedAimOffset;
					_hasRemoteTarget = true;
					parentConstraint.SetRotationOffset(0, _syncedAimOffset);
				}
			}
			_lateJoinDone = true;
		}

		private void LateUpdate()
		{
			if (!(config == null) && !(_networkedTransform == null))
			{
				ParentConstraint parentConstraint = _networkedTransform.ParentConstraint;
				bool flag = parentConstraint != null && parentConstraint.sourceCount > 0;
				if (debugMode)
				{
					DebugTick(parentConstraint, flag);
				}
				if (!flag || !IsEquippedNow)
				{
					_worldInitialized = false;
					_currentOffset = Vector3.zero;
					_currentRemoteOffset = Quaternion.identity;
					_hasRemoteTarget = false;
				}
				else if (base.isOwned)
				{
					OwnerLateUpdate(parentConstraint);
				}
				else
				{
					RemoteLateUpdate(parentConstraint);
				}
			}
		}

		private void OwnerLateUpdate(ParentConstraint pc)
		{
			Transform sourceTransform = pc.GetSource(0).sourceTransform;
			if (sourceTransform == null)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			Quaternion rotation = sourceTransform.rotation;
			if (!_worldInitialized)
			{
				_currentItemWorld = rotation;
				_worldInitialized = true;
			}
			if (IsEquippedNow)
			{
				if (TryGetCameraForward(out var forward))
				{
					Quaternion b = ComputeClampedItemWorld(sourceTransform, forward);
					float t = 1f - Mathf.Exp((0f - config.followSmoothness) * deltaTime);
					_currentItemWorld = Quaternion.Slerp(_currentItemWorld, b, t).normalized;
				}
			}
			else
			{
				float t2 = 1f - Mathf.Exp((0f - config.returnSmoothness) * deltaTime);
				_currentItemWorld = Quaternion.Slerp(_currentItemWorld, rotation, t2).normalized;
			}
			Vector3 vector = (_currentOffset = (Quaternion.Inverse(sourceTransform.rotation) * _currentItemWorld).eulerAngles);
			pc.SetRotationOffset(0, vector);
			SendAimIfDue(deltaTime, vector);
		}

		private void RemoteLateUpdate(ParentConstraint pc)
		{
			float deltaTime = Time.deltaTime;
			Quaternion b;
			float num;
			if (IsEquippedNow)
			{
				b = (_hasRemoteTarget ? Quaternion.Euler(_remoteTargetOffset) : Quaternion.identity);
				num = config.remoteFollowSmoothness;
			}
			else
			{
				b = Quaternion.identity;
				num = config.returnSmoothness;
			}
			float t = 1f - Mathf.Exp((0f - num) * deltaTime);
			_currentRemoteOffset = Quaternion.Slerp(_currentRemoteOffset, b, t).normalized;
			_currentOffset = _currentRemoteOffset.eulerAngles;
			pc.SetRotationOffset(0, _currentOffset);
		}

		private bool TryGetCameraForward(out Vector3 forward)
		{
			forward = Vector3.forward;
			if (playerService == null || !playerService.TryGetCameraTransform(out var cameraTransform) || cameraTransform == null)
			{
				return false;
			}
			forward = cameraTransform.forward;
			return true;
		}

		private Quaternion ComputeClampedItemWorld(Transform source, Vector3 camForward)
		{
			Quaternion quaternion = source.rotation * _restBeamLocal;
			Vector3 vector = quaternion * Vector3.forward;
			Vector3 rhs = new Vector3(vector.x, 0f, vector.z);
			if (rhs.sqrMagnitude < 1E-06f)
			{
				return source.rotation;
			}
			rhs.Normalize();
			Vector3 axis = Vector3.Cross(Vector3.up, rhs);
			float num = Mathf.Asin(Mathf.Clamp(vector.y, -1f, 1f)) * 57.29578f;
			float num2 = Mathf.Clamp(Mathf.Asin(Mathf.Clamp(camForward.y, -1f, 1f)) * 57.29578f - num, 0f - config.maxAngleDown, config.maxAngleUp);
			float current = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
			return Quaternion.AngleAxis(Mathf.Clamp((new Vector3(camForward.x, 0f, camForward.z).sqrMagnitude > 1E-06f) ? Mathf.DeltaAngle(current, Mathf.Atan2(camForward.x, camForward.z) * 57.29578f) : 0f, 0f - config.maxAngleHorizontal, config.maxAngleHorizontal), Vector3.up) * Quaternion.AngleAxis(0f - num2, axis) * quaternion * Quaternion.Inverse(_restBeamLocal);
		}

		private void SendAimIfDue(float dt, Vector3 offset)
		{
			_sendTimer += dt;
			if (_sendTimer < 1f / 12f)
			{
				return;
			}
			_sendTimer = 0f;
			if (IsEquippedNow)
			{
				_heartbeatTimer += 1f / 12f;
				bool flag = _heartbeatTimer >= 0.5f;
				if (Quaternion.Angle(Quaternion.Euler(offset), Quaternion.Euler(_lastSentOffset)) >= 0.5f || flag)
				{
					_heartbeatTimer = 0f;
					_lastSentOffset = offset;
					CmdSyncAimOffset(offset);
				}
			}
		}

		[Command(channel = 1)]
		private void CmdSyncAimOffset(Vector3 offset)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(offset);
			SendCommandInternal("System.Void NomadDrive.Features.Tools.FlashlightLookAtHandler::CmdSyncAimOffset(UnityEngine.Vector3)", 1144689362, writer, 1);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(channel = 1, includeOwner = false)]
		private void RpcReceiveAimOffset(Vector3 offset)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(offset);
			SendRPCInternal("System.Void NomadDrive.Features.Tools.FlashlightLookAtHandler::RpcReceiveAimOffset(UnityEngine.Vector3)", -1484787561, writer, 1, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnAimOffsetHookChanged(Vector3 oldValue, Vector3 newValue)
		{
			if (_lateJoinDone)
			{
				ApplyRemoteAim(newValue);
			}
		}

		private void ApplyRemoteAim(Vector3 offset)
		{
			if (!base.isOwned)
			{
				_remoteTargetOffset = offset;
				_hasRemoteTarget = true;
			}
		}

		private void DebugTick(ParentConstraint pc, bool hasSource)
		{
			_debugTimer += Time.deltaTime;
			if (!(_debugTimer < 1f))
			{
				_debugTimer = 0f;
				if (playerService != null && playerService.TryGetCameraTransform(out var cameraTransform))
				{
					_ = cameraTransform != null;
				}
				else
					_ = 0;
				if (pc != null)
				{
					_ = pc.sourceCount;
				}
				Quaternion.Angle(Quaternion.identity, Quaternion.Euler(_currentOffset));
			}
		}

		private void OnDisable()
		{
			_worldInitialized = false;
			_hasRemoteTarget = false;
			_remoteTargetOffset = Vector3.zero;
			_currentRemoteOffset = Quaternion.identity;
			_currentOffset = Vector3.zero;
			_sendTimer = 0f;
			_heartbeatTimer = 0f;
			ParentConstraint parentConstraint = ((_networkedTransform != null) ? _networkedTransform.ParentConstraint : null);
			if (IsEquippedNow && parentConstraint != null && parentConstraint.sourceCount > 0)
			{
				parentConstraint.SetRotationOffset(0, Vector3.zero);
			}
		}

		public FlashlightLookAtHandler()
		{
			_Mirror_SyncVarHookDelegate__syncedAimOffset = OnAimOffsetHookChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSyncAimOffset__Vector3(Vector3 offset)
		{
			Network_syncedAimOffset = offset;
			ApplyRemoteAim(offset);
			RpcReceiveAimOffset(offset);
		}

		protected static void InvokeUserCode_CmdSyncAimOffset__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSyncAimOffset called on client.");
			}
			else
			{
				((FlashlightLookAtHandler)obj).UserCode_CmdSyncAimOffset__Vector3(reader.ReadVector3());
			}
		}

		protected void UserCode_RpcReceiveAimOffset__Vector3(Vector3 offset)
		{
			if (!base.isServer)
			{
				ApplyRemoteAim(offset);
			}
		}

		protected static void InvokeUserCode_RpcReceiveAimOffset__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcReceiveAimOffset called on server.");
			}
			else
			{
				((FlashlightLookAtHandler)obj).UserCode_RpcReceiveAimOffset__Vector3(reader.ReadVector3());
			}
		}

		static FlashlightLookAtHandler()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(FlashlightLookAtHandler), "System.Void NomadDrive.Features.Tools.FlashlightLookAtHandler::CmdSyncAimOffset(UnityEngine.Vector3)", InvokeUserCode_CmdSyncAimOffset__Vector3, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(FlashlightLookAtHandler), "System.Void NomadDrive.Features.Tools.FlashlightLookAtHandler::RpcReceiveAimOffset(UnityEngine.Vector3)", InvokeUserCode_RpcReceiveAimOffset__Vector3);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteVector3(_syncedAimOffset);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteVector3(_syncedAimOffset);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _syncedAimOffset, _Mirror_SyncVarHookDelegate__syncedAimOffset, reader.ReadVector3());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedAimOffset, _Mirror_SyncVarHookDelegate__syncedAimOffset, reader.ReadVector3());
			}
		}
	}
}
