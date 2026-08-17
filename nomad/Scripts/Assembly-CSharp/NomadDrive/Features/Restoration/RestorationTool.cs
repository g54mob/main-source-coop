using System.Runtime.InteropServices;
using EvilCore.DynamicCasting;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Tools;
using PaintIn3D;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Restoration
{
	public abstract class RestorationTool : ChargableDirectHeldItem, INetworkHitRelay
	{
		[Header("Tool Tip Configuration")]
		[SerializeField]
		protected Transform _toolTip;

		[SerializeField]
		protected CwHitBetween _hitBetween;

		[Header("Surface Detection")]
		[SerializeField]
		protected float _maxDetectionDistance = 3f;

		[Tooltip("How fast the tool glides to the aimed surface. Higher = snappier, lower = smoother/floatier. Drives a velocity-carrying SmoothDamp (position) + exponential approach (rotation), so motion stays smooth between throttled casts and is frame-rate independent.")]
		[SerializeField]
		[Range(1f, 50f)]
		protected float _snapSmoothness = 15f;

		[Tooltip("How often the surface-detection ray is cast (Hz). Lower = cheaper. The snap lerp still runs every frame, so the approach stays smooth.")]
		[SerializeField]
		[Min(1f)]
		protected float _detectionHz = 25f;

		[Tooltip("Skip the cast while the camera is essentially still (the last snap result stays valid). Near-free when holding aim steady.")]
		[SerializeField]
		protected bool _skipCastWhenStill = true;

		[Tooltip("When the aim briefly drifts off a small/curved spot, keep the tool snapped for this long before releasing.")]
		[SerializeField]
		[Range(0f, 0.5f)]
		protected float _snapGraceSeconds = 0.12f;

		[Header("Snap Offset")]
		[SerializeField]
		[Range(-0.5f, 0.5f)]
		protected float _aligningOffset = 0.03f;

		[SerializeField]
		[Range(-0.5f, 0.5f)]
		protected float _snapOffset;

		[Header("Align Reference")]
		[Tooltip("Tool's surface contact point (e.g. grinder disc center). Hit point aligns to this transform.")]
		[SerializeField]
		protected Transform _alignTransform;

		[SerializeField]
		protected bool _useColliderBoundsForSnap = true;

		[SerializeField]
		protected Vector3 _snapRotationOffset = Vector3.zero;

		[Header("Movement Rotation")]
		[SerializeField]
		protected bool _enableMovementRotation = true;

		[SerializeField]
		[Range(1f, 20f)]
		protected float _movementRotationSpeed = 8f;

		[SerializeField]
		[Range(0.001f, 0.1f)]
		protected float _movementThreshold = 0.005f;

		[Inject]
		protected ICastingManager _castingManager;

		protected ToolSnapHelper _snapHelper;

		protected IRaycastHandle _raycastHandle;

		private readonly ToolInteractionSuppressor _interactionSuppressor = new ToolInteractionSuppressor();

		private bool _useButtonHeld;

		private const float CastStillPositionEpsilonSqr = 1E-08f;

		private const float CastStillAngleEpsilonDegrees = 0.05f;

		private float _detectionTimer;

		private Vector3 _lastCastCamPos;

		private Quaternion _lastCastCamRot;

		private bool _hasLastCast;

		[Header("Hit Detection (Fast Movement)")]
		[SerializeField]
		[Range(-1f, 1f)]
		protected float _hitInterval;

		[SerializeField]
		[Range(1f, 3f)]
		protected float _fastMovementRadiusMultiplier = 1.5f;

		[Header("Paint Configuration")]
		[SerializeField]
		protected CwPaintSphere[] _paintSpheres;

		[SerializeField]
		[Range(0.01f, 1f)]
		protected float _activeRadius = 0.1f;

		[SerializeField]
		protected ParticleSystem _toolEffect;

		[Header("Pressure Sensitivity")]
		[SerializeField]
		protected bool _enablePressureResponse = true;

		[SerializeField]
		[Range(0.1f, 1f)]
		protected float _minPressureOpacity = 0.3f;

		[SerializeField]
		[Range(0.5f, 5f)]
		protected float _maxMovementSpeed = 2f;

		protected RestorationToolCore _core;

		private RestorationHitRelay _hitRelay;

		protected bool _isToolActive;

		[SyncVar]
		private bool _syncedToolActive;

		[SyncVar]
		private float _syncedRadius;

		[SyncVar]
		private float _syncedOpacity;

		public GameObject CurrentTarget => _snapHelper?.CurrentTarget;

		public bool IsSnapped => _snapHelper?.IsSnapped ?? false;

		public bool IsToolActive => _isToolActive;

		protected override bool IsActivelyConsuming => _isToolActive;

		public bool IsRelaySource => base.isOwned;

		public bool Network_syncedToolActive
		{
			get
			{
				return _syncedToolActive;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedToolActive, 2048uL, null);
			}
		}

		public float Network_syncedRadius
		{
			get
			{
				return _syncedRadius;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedRadius, 4096uL, null);
			}
		}

		public float Network_syncedOpacity
		{
			get
			{
				return _syncedOpacity;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedOpacity, 8192uL, null);
			}
		}

		protected override void OnBatteryDepleted()
		{
			DisableTool();
		}

		protected abstract void ConfigurePaintSpheres();

		protected virtual float GetToolRadius()
		{
			return _activeRadius;
		}

		protected virtual float GetBaseOpacity()
		{
			return 0.7f;
		}

		protected virtual bool CanActivate()
		{
			return base.HasCharge;
		}

		protected override void Awake()
		{
			base.Awake();
			InitCore();
			_core.ValidateSetup(base.transform, GetType().Name);
			InitSnapHelper();
			SetupHitRelay();
			DisableTool();
		}

		private void InitCore()
		{
			_core = new RestorationToolCore();
			SyncToCore();
		}

		private void SyncToCore()
		{
			_core.ToolTip = _toolTip;
			_core.HitBetween = _hitBetween;
			_core.PaintSpheres = _paintSpheres;
			_core.ToolEffect = _toolEffect;
			_core.HitInterval = _hitInterval;
			_core.FastMovementRadiusMultiplier = _fastMovementRadiusMultiplier;
			_core.ActiveRadius = _activeRadius;
			_core.EnablePressureResponse = _enablePressureResponse;
			_core.MinPressureOpacity = _minPressureOpacity;
			_core.MaxMovementSpeed = _maxMovementSpeed;
		}

		private void InitSnapHelper()
		{
			_snapHelper = new ToolSnapHelper
			{
				SnapSmoothness = _snapSmoothness,
				SnapOffset = _aligningOffset,
				AlignTransform = _alignTransform,
				UseColliderBoundsForSnap = _useColliderBoundsForSnap,
				SnapRotationOffset = _snapRotationOffset,
				EnableMovementRotation = _enableMovementRotation,
				MovementRotationSpeed = _movementRotationSpeed,
				MovementThreshold = _movementThreshold,
				SnapGraceSeconds = _snapGraceSeconds
			};
			_snapHelper.CacheSnapReferenceCollider();
		}

		private void SetupHitRelay()
		{
			if (!(_toolTip == null))
			{
				_hitRelay = _toolTip.gameObject.AddComponent<RestorationHitRelay>();
				_hitRelay.Initialize(this);
			}
		}

		protected override void Start()
		{
			base.Start();
			ConfigurePaintSpheres();
			_core.Initialize(GetBaseOpacity());
		}

		protected override void Update()
		{
			base.Update();
			TickSurfaceDetection();
			_snapHelper?.TickLerp(base.transform);
			if (_isToolActive && _enablePressureResponse && !(_toolTip == null))
			{
				SyncToCore();
				_core.UpdatePressureSensitivity();
			}
		}

		public virtual void EnableTool()
		{
			if (_isToolActive)
			{
				return;
			}
			if (_snapHelper != null)
			{
				_snapHelper.SnapOffset = _snapOffset;
			}
			if (!CanActivate())
			{
				return;
			}
			_isToolActive = true;
			SyncToCore();
			var (num, num2) = _core.EnableToolLocal();
			if (base.netIdentity != null)
			{
				if (base.isServer)
				{
					Network_syncedToolActive = true;
					Network_syncedRadius = num;
					Network_syncedOpacity = num2;
					RpcEnableTool(num, num2);
				}
				else if (base.isOwned)
				{
					CmdEnableTool(num, num2);
				}
			}
		}

		protected virtual void ApplyHitDetectionSettings()
		{
			_core.ApplyHitDetectionSettings();
		}

		protected void OnHitSettingsChanged()
		{
			SyncToCore();
			_core.OnHitSettingsChanged();
		}

		protected void OnRadiusMultiplierChanged()
		{
			SyncToCore();
			_core.OnRadiusMultiplierChanged();
		}

		protected void OnSnapSettingsChanged()
		{
			if (_snapHelper != null)
			{
				_snapHelper.SnapOffset = (_isToolActive ? _snapOffset : _aligningOffset);
				_snapHelper.SnapSmoothness = _snapSmoothness;
				_snapHelper.UseColliderBoundsForSnap = _useColliderBoundsForSnap;
				_snapHelper.SnapRotationOffset = _snapRotationOffset;
			}
		}

		public void SetHitPresetUltraSmooth()
		{
			SyncToCore();
			_core.SetHitPresetUltraSmooth();
			_hitInterval = _core.HitInterval;
			_fastMovementRadiusMultiplier = _core.FastMovementRadiusMultiplier;
		}

		public void SetHitPresetBalanced()
		{
			SyncToCore();
			_core.SetHitPresetBalanced();
			_hitInterval = _core.HitInterval;
			_fastMovementRadiusMultiplier = _core.FastMovementRadiusMultiplier;
		}

		public void SetHitPresetPrecision()
		{
			SyncToCore();
			_core.SetHitPresetPrecision();
			_hitInterval = _core.HitInterval;
			_fastMovementRadiusMultiplier = _core.FastMovementRadiusMultiplier;
		}

		public virtual void DisableTool()
		{
			_isToolActive = false;
			if (_snapHelper != null)
			{
				_snapHelper.SnapOffset = _aligningOffset;
			}
			SyncToCore();
			_core.DisableToolLocal();
			if (base.netIdentity != null)
			{
				if (base.isServer)
				{
					Network_syncedToolActive = false;
					RpcDisableTool();
				}
				else if (base.isOwned)
				{
					CmdDisableTool();
				}
			}
		}

		public void ToggleTool()
		{
			if (_isToolActive)
			{
				DisableTool();
			}
			else
			{
				EnableTool();
			}
		}

		protected virtual void FullAutoSetup()
		{
			SyncToCore();
			_core.FullAutoSetup(base.transform, GetType().Name, SetupPaintSpheresOnToolTip);
			_toolTip = _core.ToolTip;
			_hitBetween = _core.HitBetween;
			_paintSpheres = _core.PaintSpheres;
		}

		protected virtual void SetupPaintSpheresOnToolTip()
		{
		}

		protected void FindComponents()
		{
			SyncToCore();
			_core.FindComponents(base.transform, GetType().Name);
			_toolTip = _core.ToolTip;
			_hitBetween = _core.HitBetween;
			_paintSpheres = _core.PaintSpheres;
		}

		public override void OnEquip()
		{
			base.OnEquip();
			RegisterSurfaceDetection();
		}

		public override void OnUnequip()
		{
			base.OnUnequip();
			_useButtonHeld = false;
			DisableTool();
			UnregisterSurfaceDetection();
			if (_snapHelper.ForceRelease())
			{
				OnRelease();
			}
		}

		public override void StartPlacing()
		{
			if (_snapHelper.ForceRelease())
			{
				OnRelease();
			}
			base.StartPlacing();
		}

		public override void OnPlacementModeEnter()
		{
			base.OnPlacementModeEnter();
			_useButtonHeld = false;
			DisableTool();
			UnregisterSurfaceDetection();
			if (_snapHelper.ForceRelease())
			{
				OnRelease();
			}
		}

		public override void OnPlacementModeExit()
		{
			base.OnPlacementModeExit();
			if (base.IsEquipped)
			{
				RegisterSurfaceDetection();
			}
		}

		protected virtual void RegisterSurfaceDetection()
		{
			if (_castingManager != null && !(playerService?.CameraTransform == null))
			{
				CastRequest request = CastRequest.MeshRay(playerService.CameraTransform, _maxDetectionDistance, -1).RequireComponent<RestorableVehicleSurface>();
				request.UpdateMode = UpdateMode.Manual;
				_raycastHandle = _castingManager.Register(request, OnSurfaceDetected);
				_detectionTimer = 0f;
				_hasLastCast = false;
			}
		}

		protected virtual void UnregisterSurfaceDetection()
		{
			_castingManager?.Unregister(_raycastHandle);
			_raycastHandle = null;
		}

		protected void TickSurfaceDetection()
		{
			if (_raycastHandle == null || !base.isOwned || !base.IsEquipped)
			{
				return;
			}
			_detectionTimer -= Time.deltaTime;
			if (!(_detectionTimer > 0f))
			{
				_detectionTimer = 1f / Mathf.Max(1f, _detectionHz);
				Transform transform = playerService?.CameraTransform;
				if (!(transform == null) && (!_skipCastWhenStill || !_hasLastCast || !((transform.position - _lastCastCamPos).sqrMagnitude < 1E-08f) || !(Quaternion.Angle(transform.rotation, _lastCastCamRot) < 0.05f)))
				{
					_lastCastCamPos = transform.position;
					_lastCastCamRot = transform.rotation;
					_hasLastCast = true;
					_castingManager?.Process(_raycastHandle);
				}
			}
		}

		protected virtual void OnSurfaceDetected(CastResult result)
		{
			if (!base.isOwned || !base.IsEquipped)
			{
				return;
			}
			if (result.DidHit)
			{
				if (_snapHelper.ProcessHit(base.transform, result.HitPoint, result.HitNormal, result.HitGameObject))
				{
					OnSnap();
				}
				if (_snapHelper.IsSnapped)
				{
					SetSnapConstraintSuspended(suspend: true);
					StreamSnapPose(base.transform.position, base.transform.rotation);
				}
			}
			else if (_snapHelper.ProcessNoHit())
			{
				OnRelease();
			}
		}

		protected virtual void OnSnap()
		{
			if (_snapHelper != null)
			{
				_snapHelper.SnapOffset = _aligningOffset;
			}
			BeginSnapStream();
			playerService?.EquipmentManager?.OnGripPauseRequested?.Invoke();
			_interactionSuppressor.SetSuppressed(playerService, suppress: true);
			if (_useButtonHeld)
			{
				EnableTool();
			}
		}

		protected virtual void OnRelease()
		{
			DisableTool();
			EndSnapStream();
			SetSnapConstraintSuspended(suspend: false);
			playerService?.EquipmentManager?.OnGripResumeRequested?.Invoke();
			_interactionSuppressor.SetSuppressed(playerService, suppress: false);
		}

		public override void OnUseButtonDown()
		{
			base.OnUseButtonDown();
			_useButtonHeld = true;
			if (_snapHelper.IsSnapped)
			{
				EnableTool();
			}
		}

		public override void OnUseButtonUp()
		{
			base.OnUseButtonUp();
			_useButtonHeld = false;
			DisableTool();
		}

		public void RelayHitPoint(Vector3 position, Quaternion rotation, float pressure, int seed, int priority)
		{
			CmdRelayHitPoint(position, rotation, pressure, seed, priority);
		}

		public void RelayHitLine(Vector3 position, Vector3 endPosition, Quaternion rotation, float pressure, int seed, bool clip, int priority)
		{
			CmdRelayHitLine(position, endPosition, rotation, pressure, seed, clip, priority);
		}

		[Command]
		private void CmdRelayHitPoint(Vector3 position, Quaternion rotation, float pressure, int seed, int priority)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			writer.WriteFloat(pressure);
			writer.WriteVarInt(seed);
			writer.WriteVarInt(priority);
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.RestorationTool::CmdRelayHitPoint(UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Int32)", -1082317598, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcRelayHitPoint(Vector3 position, Quaternion rotation, float pressure, int seed, int priority)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			writer.WriteFloat(pressure);
			writer.WriteVarInt(seed);
			writer.WriteVarInt(priority);
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.RestorationTool::RpcRelayHitPoint(UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Int32)", -298914013, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		private void CmdRelayHitLine(Vector3 position, Vector3 endPosition, Quaternion rotation, float pressure, int seed, bool clip, int priority)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(position);
			writer.WriteVector3(endPosition);
			writer.WriteQuaternion(rotation);
			writer.WriteFloat(pressure);
			writer.WriteVarInt(seed);
			writer.WriteBool(clip);
			writer.WriteVarInt(priority);
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.RestorationTool::CmdRelayHitLine(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Boolean,System.Int32)", -1864329630, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcRelayHitLine(Vector3 position, Vector3 endPosition, Quaternion rotation, float pressure, int seed, bool clip, int priority)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(position);
			writer.WriteVector3(endPosition);
			writer.WriteQuaternion(rotation);
			writer.WriteFloat(pressure);
			writer.WriteVarInt(seed);
			writer.WriteBool(clip);
			writer.WriteVarInt(priority);
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.RestorationTool::RpcRelayHitLine(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Boolean,System.Int32)", -2109572013, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		private void CmdEnableTool(float radius, float opacity)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(radius);
			writer.WriteFloat(opacity);
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.RestorationTool::CmdEnableTool(System.Single,System.Single)", 1357846060, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcEnableTool(float radius, float opacity)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(radius);
			writer.WriteFloat(opacity);
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.RestorationTool::RpcEnableTool(System.Single,System.Single)", -2006011671, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		private void CmdDisableTool()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.RestorationTool::CmdDisableTool()", 873111857, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcDisableTool()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.RestorationTool::RpcDisableTool()", 1487519084, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (_syncedToolActive && !_isToolActive)
			{
				_isToolActive = true;
				SyncToCore();
				_core.ApplyToolStateToSpheres(_syncedRadius, _syncedOpacity);
				if (_hitBetween != null)
				{
					_hitBetween.enabled = false;
				}
				if (_toolEffect != null)
				{
					_toolEffect.Play();
				}
			}
		}

		protected virtual void OnDestroy()
		{
			_interactionSuppressor.SetSuppressed(playerService, suppress: false);
			_castingManager?.Unregister(_raycastHandle);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32(Vector3 position, Quaternion rotation, float pressure, int seed, int priority)
		{
			RpcRelayHitPoint(position, rotation, pressure, seed, priority);
		}

		protected static void InvokeUserCode_CmdRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRelayHitPoint called on client.");
			}
			else
			{
				((RestorationTool)obj).UserCode_CmdRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32(reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadFloat(), reader.ReadVarInt(), reader.ReadVarInt());
			}
		}

		protected void UserCode_RpcRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32(Vector3 position, Quaternion rotation, float pressure, int seed, int priority)
		{
			if (base.isOwned || _paintSpheres == null)
			{
				return;
			}
			CwPaintSphere[] paintSpheres = _paintSpheres;
			foreach (CwPaintSphere cwPaintSphere in paintSpheres)
			{
				if (cwPaintSphere != null)
				{
					cwPaintSphere.HandleHitPoint(preview: false, priority, pressure, seed, position, rotation);
				}
			}
		}

		protected static void InvokeUserCode_RpcRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcRelayHitPoint called on server.");
			}
			else
			{
				((RestorationTool)obj).UserCode_RpcRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32(reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadFloat(), reader.ReadVarInt(), reader.ReadVarInt());
			}
		}

		protected void UserCode_CmdRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32(Vector3 position, Vector3 endPosition, Quaternion rotation, float pressure, int seed, bool clip, int priority)
		{
			RpcRelayHitLine(position, endPosition, rotation, pressure, seed, clip, priority);
		}

		protected static void InvokeUserCode_CmdRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRelayHitLine called on client.");
			}
			else
			{
				((RestorationTool)obj).UserCode_CmdRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32(reader.ReadVector3(), reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadFloat(), reader.ReadVarInt(), reader.ReadBool(), reader.ReadVarInt());
			}
		}

		protected void UserCode_RpcRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32(Vector3 position, Vector3 endPosition, Quaternion rotation, float pressure, int seed, bool clip, int priority)
		{
			if (base.isOwned || _paintSpheres == null)
			{
				return;
			}
			CwPaintSphere[] paintSpheres = _paintSpheres;
			foreach (CwPaintSphere cwPaintSphere in paintSpheres)
			{
				if (cwPaintSphere != null)
				{
					cwPaintSphere.HandleHitLine(preview: false, priority, pressure, seed, position, endPosition, rotation, clip);
				}
			}
		}

		protected static void InvokeUserCode_RpcRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcRelayHitLine called on server.");
			}
			else
			{
				((RestorationTool)obj).UserCode_RpcRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32(reader.ReadVector3(), reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadFloat(), reader.ReadVarInt(), reader.ReadBool(), reader.ReadVarInt());
			}
		}

		protected void UserCode_CmdEnableTool__Single__Single(float radius, float opacity)
		{
			Network_syncedToolActive = true;
			Network_syncedRadius = radius;
			Network_syncedOpacity = opacity;
			RpcEnableTool(radius, opacity);
		}

		protected static void InvokeUserCode_CmdEnableTool__Single__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdEnableTool called on client.");
			}
			else
			{
				((RestorationTool)obj).UserCode_CmdEnableTool__Single__Single(reader.ReadFloat(), reader.ReadFloat());
			}
		}

		protected void UserCode_RpcEnableTool__Single__Single(float radius, float opacity)
		{
			if (!base.isOwned || !_isToolActive)
			{
				_isToolActive = true;
				SyncToCore();
				_core.ApplyToolStateToSpheres(radius, opacity);
				if (_hitBetween != null)
				{
					_hitBetween.enabled = false;
				}
				if (_toolEffect != null)
				{
					_toolEffect.Play();
				}
			}
		}

		protected static void InvokeUserCode_RpcEnableTool__Single__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcEnableTool called on server.");
			}
			else
			{
				((RestorationTool)obj).UserCode_RpcEnableTool__Single__Single(reader.ReadFloat(), reader.ReadFloat());
			}
		}

		protected void UserCode_CmdDisableTool()
		{
			Network_syncedToolActive = false;
			RpcDisableTool();
		}

		protected static void InvokeUserCode_CmdDisableTool(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdDisableTool called on client.");
			}
			else
			{
				((RestorationTool)obj).UserCode_CmdDisableTool();
			}
		}

		protected void UserCode_RpcDisableTool()
		{
			if (!base.isOwned || _isToolActive)
			{
				_isToolActive = false;
				SyncToCore();
				_core.ApplyToolStateToSpheres(0f, 0f);
				if (_toolEffect != null)
				{
					_toolEffect.Stop();
				}
			}
		}

		protected static void InvokeUserCode_RpcDisableTool(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcDisableTool called on server.");
			}
			else
			{
				((RestorationTool)obj).UserCode_RpcDisableTool();
			}
		}

		static RestorationTool()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(RestorationTool), "System.Void NomadDrive.Features.Restoration.RestorationTool::CmdRelayHitPoint(UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Int32)", InvokeUserCode_CmdRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(RestorationTool), "System.Void NomadDrive.Features.Restoration.RestorationTool::CmdRelayHitLine(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Boolean,System.Int32)", InvokeUserCode_CmdRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(RestorationTool), "System.Void NomadDrive.Features.Restoration.RestorationTool::CmdEnableTool(System.Single,System.Single)", InvokeUserCode_CmdEnableTool__Single__Single, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(RestorationTool), "System.Void NomadDrive.Features.Restoration.RestorationTool::CmdDisableTool()", InvokeUserCode_CmdDisableTool, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(RestorationTool), "System.Void NomadDrive.Features.Restoration.RestorationTool::RpcRelayHitPoint(UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Int32)", InvokeUserCode_RpcRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32);
			RemoteProcedureCalls.RegisterRpc(typeof(RestorationTool), "System.Void NomadDrive.Features.Restoration.RestorationTool::RpcRelayHitLine(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Boolean,System.Int32)", InvokeUserCode_RpcRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32);
			RemoteProcedureCalls.RegisterRpc(typeof(RestorationTool), "System.Void NomadDrive.Features.Restoration.RestorationTool::RpcEnableTool(System.Single,System.Single)", InvokeUserCode_RpcEnableTool__Single__Single);
			RemoteProcedureCalls.RegisterRpc(typeof(RestorationTool), "System.Void NomadDrive.Features.Restoration.RestorationTool::RpcDisableTool()", InvokeUserCode_RpcDisableTool);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_syncedToolActive);
				writer.WriteFloat(_syncedRadius);
				writer.WriteFloat(_syncedOpacity);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x800L) != 0L)
			{
				writer.WriteBool(_syncedToolActive);
			}
			if ((syncVarDirtyBits & 0x1000L) != 0L)
			{
				writer.WriteFloat(_syncedRadius);
			}
			if ((syncVarDirtyBits & 0x2000L) != 0L)
			{
				writer.WriteFloat(_syncedOpacity);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _syncedToolActive, null, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _syncedRadius, null, reader.ReadFloat());
				GeneratedSyncVarDeserialize(ref _syncedOpacity, null, reader.ReadFloat());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x800L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedToolActive, null, reader.ReadBool());
			}
			if ((num & 0x1000L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedRadius, null, reader.ReadFloat());
			}
			if ((num & 0x2000L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedOpacity, null, reader.ReadFloat());
			}
		}
	}
}
