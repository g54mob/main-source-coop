using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.EvilSave;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Objectives;
using NomadDrive.Features.SaveSystem;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Digging
{
	public class BuriedTreasure : NetworkBehaviour, INetworkSaveable, IFloatingOriginShiftable
	{
		private const byte MaxStage = 4;

		private static readonly HashSet<BuriedTreasure> Registry;

		[Header("Per-Treasure")]
		[Tooltip("World-space offset added to the detected surface for EACH dig stage (index 0 = stage 1 ... index 3 = stage 4). Usually negative Y so the loot starts below the decal and emerges as it's dug; the last entry is the rest pose. Length should be 4 (MaxStage). Stage 0 (untouched) stays at the underground spawn.")]
		[SerializeField]
		private Vector3[] stageSurfaceOffsets = new Vector3[4]
		{
			new Vector3(0f, -0.4f, 0f),
			new Vector3(0f, -0.25f, 0f),
			new Vector3(0f, -0.12f, 0f),
			new Vector3(0f, 0f, 0f)
		};

		[Tooltip("Euler rotation offset applied at EACH dig stage, RELATIVE to the buried/spawn rotation (index 0 = stage 1 ... index 3 = stage 4). Lets every step add a different small twist as the loot emerges; the last entry is the final rest rotation. Length should be 4 (MaxStage). Stage 0 (untouched) keeps the spawn rotation. All-zero = rise straight, no twist.")]
		[SerializeField]
		private Vector3[] stageRotationOffsets = new Vector3[4]
		{
			new Vector3(0f, 6f, 3f),
			new Vector3(0f, -4f, -2f),
			new Vector3(0f, 8f, 4f),
			new Vector3(0f, 0f, 0f)
		};

		[Tooltip("How long (seconds) the root TWEENS into EACH dig stage (index 0 = stage 1 ... index 3 = stage 4) — the per-step emerge speed for both position and rotation. Length should be 4 (MaxStage). Stage 0 only snaps on init, so its value is unused.")]
		[SerializeField]
		private float[] stageMoveDurations = new float[4] { 0.1f, 0.1f, 0.1f, 0.1f };

		[Header("Events")]
		public UnityEvent OnSurfaced;

		[SyncVar]
		private Vector3 _buriedPosition;

		[SyncVar]
		private Quaternion _buriedRotation = Quaternion.identity;

		[SyncVar]
		private Vector3 _surfacePosition;

		[SyncVar]
		private Vector3 _surfaceNormal = Vector3.up;

		[SyncVar(hook = "OnStageChanged")]
		private byte _stage;

		private Interactable _loot;

		private HeldItem _heldLoot;

		private readonly List<Interactable> _networkedInteractables = new List<Interactable>();

		private readonly List<StaticInteractable> _staticInteractables = new List<StaticInteractable>();

		private GameObject _decalInstance;

		private GameObject[] _decalSteps;

		private Tween _moveTween;

		private Tween _rotTween;

		private bool _initialized;

		private bool _suppressRootPose;

		private bool _surfaceConfirmed;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__stage;

		public byte Stage => _stage;

		public bool CanAdvance => _stage < 4;

		public bool IsSurfaced => _stage >= 4;

		public string ContributorKey => "treasure";

		public Vector3 Network_buriedPosition
		{
			get
			{
				return _buriedPosition;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _buriedPosition, 1uL, null);
			}
		}

		public Quaternion Network_buriedRotation
		{
			get
			{
				return _buriedRotation;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _buriedRotation, 2uL, null);
			}
		}

		public Vector3 Network_surfacePosition
		{
			get
			{
				return _surfacePosition;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _surfacePosition, 4uL, null);
			}
		}

		public Vector3 Network_surfaceNormal
		{
			get
			{
				return _surfaceNormal;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _surfaceNormal, 8uL, null);
			}
		}

		public byte Network_stage
		{
			get
			{
				return _stage;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _stage, 16uL, _Mirror_SyncVarHookDelegate__stage);
			}
		}

		public static BuriedTreasure FindNearestUndug(Vector3 originPos, float maxPlanarDistance)
		{
			BuriedTreasure result = null;
			float num = maxPlanarDistance * maxPlanarDistance;
			foreach (BuriedTreasure item in Registry)
			{
				if (!(item == null) && item._stage == 0)
				{
					Vector3 position = item.transform.position;
					float num2 = originPos.x - position.x;
					float num3 = originPos.z - position.z;
					float num4 = num2 * num2 + num3 * num3;
					if (num4 < num)
					{
						num = num4;
						result = item;
					}
				}
			}
			return result;
		}

		private void OnEnable()
		{
			Registry.Add(this);
			FloatingOriginManager.RegisterShiftable(this);
		}

		private void OnDisable()
		{
			Registry.Remove(this);
			FloatingOriginManager.UnregisterShiftable(this);
		}

		private void Awake()
		{
			_loot = GetComponent<Interactable>();
			if (_loot == null)
			{
				EvilLogger.LogError("[BuriedTreasure] No Interactable on '" + base.name + "'. Add this next to a loot's Interactable/HeldItem.", "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Digging\\Scripts\\BuriedTreasure.cs", 139);
				return;
			}
			_heldLoot = _loot as HeldItem;
			_networkedInteractables.AddRange(GetComponentsInChildren<Interactable>(includeInactive: true));
			_staticInteractables.AddRange(GetComponentsInChildren<StaticInteractable>(includeInactive: true));
			SetHoverGate(ignore: true);
			if (_heldLoot != null)
			{
				_heldLoot.OnEquipped.AddListener(OnLootEquipped);
			}
		}

		public override void OnStartServer()
		{
			CaptureBuriedAndSurface();
			InitOnce();
		}

		public override void OnStartClient()
		{
			if (!base.isServer)
			{
				InitOnce();
			}
		}

		[Server]
		private void CaptureBuriedAndSurface()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Digging.BuriedTreasure::CaptureBuriedAndSurface()' called when server was not active");
				return;
			}
			DiggingConfig instance = DiggingConfig.Instance;
			Vector3 position = base.transform.position;
			Quaternion rotation = base.transform.rotation;
			Network_buriedPosition = FloatingOriginManager.ToTrueWorld(position);
			Network_buriedRotation = rotation;
			float num = ((instance != null) ? instance.surfaceProbeHeight : 50f);
			float num2 = ((instance != null) ? instance.fallbackRiseHeight : 1.5f);
			LayerMask layerMask = ((instance != null) ? instance.terrainMask : ((LayerMask)(-1)));
			Vector3 renderPos = position + Vector3.up * num2;
			Vector3 network_surfaceNormal = Vector3.up;
			if (Physics.Raycast(position + Vector3.up * num, Vector3.down, out var hitInfo, num * 2f, layerMask, QueryTriggerInteraction.Ignore))
			{
				renderPos = new Vector3(position.x, hitInfo.point.y, position.z);
				network_surfaceNormal = hitInfo.normal;
			}
			Network_surfacePosition = FloatingOriginManager.ToTrueWorld(renderPos);
			Network_surfaceNormal = network_surfaceNormal;
		}

		private void InitOnce()
		{
			if (!_initialized)
			{
				BuildDecal();
				ApplyStage(_stage, animate: false);
				_initialized = true;
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdAdvanceStage(Vector3 digSurfacePoint, Vector3 digSurfaceNormal)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(digSurfacePoint);
			writer.WriteVector3(digSurfaceNormal);
			SendCommandInternal("System.Void NomadDrive.Features.Digging.BuriedTreasure::CmdAdvanceStage(UnityEngine.Vector3,UnityEngine.Vector3)", -707806732, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		private void RefreshSurfaceAnchor(Vector3 digSurfacePoint, Vector3 digSurfaceNormal)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Digging.BuriedTreasure::RefreshSurfaceAnchor(UnityEngine.Vector3,UnityEngine.Vector3)' called when server was not active");
			}
			else if (!_surfaceConfirmed)
			{
				_surfaceConfirmed = true;
				DiggingConfig instance = DiggingConfig.Instance;
				float num = ((instance != null) ? instance.surfaceProbeHeight : 50f);
				LayerMask layerMask = ((instance != null) ? instance.terrainMask : ((LayerMask)(-1)));
				Vector3 position = base.transform.position;
				_ = _surfacePosition;
				if (Physics.Raycast(new Vector3(position.x, position.y + num, position.z), Vector3.down, out var hitInfo, num * 2f, layerMask, QueryTriggerInteraction.Ignore))
				{
					Network_surfacePosition = FloatingOriginManager.ToTrueWorld(new Vector3(position.x, hitInfo.point.y, position.z));
					Network_surfaceNormal = hitInfo.normal;
				}
				else
				{
					Network_surfacePosition = FloatingOriginManager.ToTrueWorld(new Vector3(position.x, digSurfacePoint.y, position.z));
					Network_surfaceNormal = digSurfaceNormal;
				}
			}
		}

		private void OnStageChanged(byte _, byte newStage)
		{
			if (_initialized)
			{
				ApplyStage(newStage, animate: true);
				if (newStage >= 4)
				{
					OnSurfaced?.Invoke();
					ObjectivesEventBus.Raise(ObjectiveSignal.TreasureSurfaced, this);
				}
			}
		}

		private void ApplyStage(byte stage, bool animate)
		{
			ApplyDecals(stage);
			RepositionDecal();
			ApplyRootPose(stage, animate);
			ApplyHoverGate(stage);
		}

		private void ApplyDecals(byte stage)
		{
			if (_decalSteps == null)
			{
				return;
			}
			for (int i = 0; i < _decalSteps.Length; i++)
			{
				if (_decalSteps[i] != null)
				{
					_decalSteps[i].SetActive(stage > 0 && i == stage - 1);
				}
			}
		}

		private void ApplyRootPose(byte stage, bool animate)
		{
			if (_suppressRootPose)
			{
				return;
			}
			DiggingConfig instance = DiggingConfig.Instance;
			Ease ease = ((instance != null) ? instance.riseEase : Ease.OutCubic);
			float duration = ((instance != null) ? instance.riseDuration : 0.1f);
			if (stage > 0 && stageMoveDurations != null && stageMoveDurations.Length != 0)
			{
				duration = stageMoveDurations[Mathf.Clamp(stage - 1, 0, stageMoveDurations.Length - 1)];
			}
			Vector3 truePos;
			if (stage == 0)
			{
				truePos = _buriedPosition;
			}
			else
			{
				Vector3 vector = Vector3.zero;
				if (stageSurfaceOffsets != null && stageSurfaceOffsets.Length != 0)
				{
					vector = stageSurfaceOffsets[Mathf.Clamp(stage - 1, 0, stageSurfaceOffsets.Length - 1)];
				}
				truePos = _surfacePosition + vector;
			}
			Quaternion quaternion;
			if (stage == 0)
			{
				quaternion = _buriedRotation;
			}
			else
			{
				Vector3 euler = Vector3.zero;
				if (stageRotationOffsets != null && stageRotationOffsets.Length != 0)
				{
					euler = stageRotationOffsets[Mathf.Clamp(stage - 1, 0, stageRotationOffsets.Length - 1)];
				}
				quaternion = _buriedRotation * Quaternion.Euler(euler);
			}
			truePos = FloatingOriginManager.ToRenderWorld(truePos);
			_moveTween.Stop();
			_rotTween.Stop();
			if (animate)
			{
				_moveTween = Tween.Position(base.transform, truePos, duration, ease);
				_rotTween = Tween.Rotation(base.transform, quaternion, duration, ease);
			}
			else
			{
				base.transform.SetPositionAndRotation(truePos, quaternion);
			}
		}

		private void ApplyHoverGate(byte stage)
		{
			SetHoverGate(stage < 4);
		}

		private void SetHoverGate(bool ignore)
		{
			foreach (Interactable networkedInteractable in _networkedInteractables)
			{
				if (!(networkedInteractable == null))
				{
					if (ignore)
					{
						networkedInteractable.IgnoreHovering();
					}
					else
					{
						networkedInteractable.UnignoreHovering();
					}
				}
			}
			foreach (StaticInteractable staticInteractable in _staticInteractables)
			{
				if (!(staticInteractable == null))
				{
					if (ignore)
					{
						staticInteractable.IgnoreHovering();
					}
					else
					{
						staticInteractable.UnignoreHovering();
					}
				}
			}
		}

		private void BuildDecal()
		{
			DiggingConfig instance = DiggingConfig.Instance;
			if (!(instance == null) && !(instance.sandDecalPrefab == null))
			{
				Vector3 position = FloatingOriginManager.ToRenderWorld(_surfacePosition) + _surfaceNormal * instance.decalSurfaceOffset;
				Quaternion rotation = instance.sandDecalPrefab.transform.rotation;
				_decalInstance = UnityEngine.Object.Instantiate(instance.sandDecalPrefab, position, rotation);
				int num = Mathf.Min(4, _decalInstance.transform.childCount);
				_decalSteps = new GameObject[num];
				for (int i = 0; i < num; i++)
				{
					_decalSteps[i] = _decalInstance.transform.GetChild(i).gameObject;
				}
			}
		}

		private void RepositionDecal()
		{
			if (!(_decalInstance == null))
			{
				DiggingConfig instance = DiggingConfig.Instance;
				float num = ((instance != null) ? instance.decalSurfaceOffset : 0.1f);
				_decalInstance.transform.position = FloatingOriginManager.ToRenderWorld(_surfacePosition) + _surfaceNormal * num;
			}
		}

		public void OnOriginShift(Vector3 delta)
		{
			if (_decalInstance != null)
			{
				_decalInstance.transform.position += delta;
			}
		}

		private void OnLootEquipped()
		{
			_moveTween.Stop();
			_rotTween.Stop();
		}

		private void OnDestroy()
		{
			Registry.Remove(this);
			_moveTween.Stop();
			_rotTween.Stop();
			if (_heldLoot != null)
			{
				_heldLoot.OnEquipped.RemoveListener(OnLootEquipped);
			}
			if (_decalInstance != null)
			{
				UnityEngine.Object.Destroy(_decalInstance);
			}
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			writer.Write(_stage);
			writer.Write(_buriedPosition);
			writer.Write(_buriedRotation);
			writer.Write(_surfacePosition);
			writer.Write(_surfaceNormal);
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			byte b = reader.ReadByte();
			Vector3 network_buriedPosition = reader.ReadVector3();
			Quaternion network_buriedRotation = reader.ReadQuaternion();
			Vector3 network_surfacePosition = reader.ReadVector3();
			Vector3 network_surfaceNormal = reader.ReadVector3();
			if (NetworkServer.active)
			{
				Network_buriedPosition = network_buriedPosition;
				Network_buriedRotation = network_buriedRotation;
				Network_surfacePosition = network_surfacePosition;
				Network_surfaceNormal = network_surfaceNormal;
				if (b >= 4)
				{
					_suppressRootPose = true;
					DestroyDecal();
				}
				Network_stage = b;
			}
		}

		private void DestroyDecal()
		{
			if (_decalInstance != null)
			{
				UnityEngine.Object.Destroy(_decalInstance);
			}
			_decalInstance = null;
			_decalSteps = null;
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
		}

		public BuriedTreasure()
		{
			_Mirror_SyncVarHookDelegate__stage = OnStageChanged;
		}

		static BuriedTreasure()
		{
			Registry = new HashSet<BuriedTreasure>();
			RemoteProcedureCalls.RegisterCommand(typeof(BuriedTreasure), "System.Void NomadDrive.Features.Digging.BuriedTreasure::CmdAdvanceStage(UnityEngine.Vector3,UnityEngine.Vector3)", InvokeUserCode_CmdAdvanceStage__Vector3__Vector3, requiresAuthority: false);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdAdvanceStage__Vector3__Vector3(Vector3 digSurfacePoint, Vector3 digSurfaceNormal)
		{
			RefreshSurfaceAnchor(digSurfacePoint, digSurfaceNormal);
			if (_stage < 4)
			{
				Network_stage = (byte)(_stage + 1);
			}
		}

		protected static void InvokeUserCode_CmdAdvanceStage__Vector3__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdAdvanceStage called on client.");
			}
			else
			{
				((BuriedTreasure)obj).UserCode_CmdAdvanceStage__Vector3__Vector3(reader.ReadVector3(), reader.ReadVector3());
			}
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteVector3(_buriedPosition);
				writer.WriteQuaternion(_buriedRotation);
				writer.WriteVector3(_surfacePosition);
				writer.WriteVector3(_surfaceNormal);
				NetworkWriterExtensions.WriteByte(writer, _stage);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteVector3(_buriedPosition);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteQuaternion(_buriedRotation);
			}
			if ((syncVarDirtyBits & 4L) != 0L)
			{
				writer.WriteVector3(_surfacePosition);
			}
			if ((syncVarDirtyBits & 8L) != 0L)
			{
				writer.WriteVector3(_surfaceNormal);
			}
			if ((syncVarDirtyBits & 0x10L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _stage);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _buriedPosition, null, reader.ReadVector3());
				GeneratedSyncVarDeserialize(ref _buriedRotation, null, reader.ReadQuaternion());
				GeneratedSyncVarDeserialize(ref _surfacePosition, null, reader.ReadVector3());
				GeneratedSyncVarDeserialize(ref _surfaceNormal, null, reader.ReadVector3());
				GeneratedSyncVarDeserialize(ref _stage, _Mirror_SyncVarHookDelegate__stage, NetworkReaderExtensions.ReadByte(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _buriedPosition, null, reader.ReadVector3());
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _buriedRotation, null, reader.ReadQuaternion());
			}
			if ((num & 4L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _surfacePosition, null, reader.ReadVector3());
			}
			if ((num & 8L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _surfaceNormal, null, reader.ReadVector3());
			}
			if ((num & 0x10L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _stage, _Mirror_SyncVarHookDelegate__stage, NetworkReaderExtensions.ReadByte(reader));
			}
		}
	}
}
