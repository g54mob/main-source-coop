using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.DynamicCasting;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Tools;
using PaintCore;
using PaintIn3D;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Restoration
{
	public class CleanTool : DirectHeldItem, INetworkHitRelay
	{
		[Header("Tool Tip Configuration")]
		[SerializeField]
		private Transform _toolTip;

		[SerializeField]
		private CwHitBetween _hitBetween;

		[SerializeField]
		private CwPaintSphere[] _paintSpheres;

		[SerializeField]
		[Range(0.01f, 1f)]
		private float _activeRadius = 0.35f;

		[SerializeField]
		private ParticleSystem _toolEffect;

		[Header("Surface Detection")]
		[SerializeField]
		private float _maxDetectionDistance = 3f;

		[SerializeField]
		[Range(1f, 50f)]
		private float _snapSmoothness = 15f;

		[Tooltip("How often the surface-detection ray is cast (Hz). Lower = cheaper. The snap lerp still runs every frame, so the approach stays smooth.")]
		[SerializeField]
		[Min(1f)]
		private float _detectionHz = 25f;

		[Tooltip("Skip the cast while the camera is essentially still (the last snap result stays valid). Near-free when holding aim steady.")]
		[SerializeField]
		private bool _skipCastWhenStill = true;

		[Tooltip("When the aim briefly drifts off a small/curved spot, keep the tool snapped for this long before releasing.")]
		[SerializeField]
		[Range(0f, 0.5f)]
		private float _snapGraceSeconds = 0.12f;

		[Header("Snap Offset")]
		[SerializeField]
		[Range(-0.5f, 0.5f)]
		private float _aligningOffset = 0.03f;

		[SerializeField]
		[Range(-0.5f, 0.5f)]
		private float _snapOffset;

		[Header("Align Reference")]
		[Tooltip("Tool's surface contact point (e.g. sponge center). Hit point aligns to this transform.")]
		[SerializeField]
		private Transform _alignTransform;

		[SerializeField]
		private bool _useColliderBoundsForSnap = true;

		[SerializeField]
		private Vector3 _snapRotationOffset = Vector3.zero;

		[Header("Movement Rotation")]
		[SerializeField]
		private bool _enableMovementRotation = true;

		[SerializeField]
		[Range(1f, 20f)]
		private float _movementRotationSpeed = 8f;

		[SerializeField]
		[Range(0.001f, 0.1f)]
		private float _movementThreshold = 0.005f;

		[Header("Clean Settings")]
		[SerializeField]
		[Range(1f, 20f)]
		private float _hardness = 5f;

		[SerializeField]
		[Range(0.01f, 0.2f)]
		private float _opacity = 0.05f;

		[Header("Rust Blur Effect")]
		[Tooltip("Enable soft blur effect on rust when cleaning")]
		[SerializeField]
		private bool _enableRustBlur = true;

		[Tooltip("Blur kernel size in pixels - higher = more spread")]
		[SerializeField]
		[Range(1f, 8f)]
		private float _rustBlurKernel = 2f;

		[Tooltip("Blur effect opacity - how much blur is applied per stroke")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _rustBlurOpacity = 0.15f;

		[Tooltip("Blur sphere hardness - softer edges for more natural effect")]
		[SerializeField]
		[Range(0.5f, 10f)]
		private float _rustBlurHardness = 3f;

		[Header("Target Groups")]
		[SerializeField]
		private int _dirtGroupIndex = 201;

		[SerializeField]
		private int _rustGroupIndex = 202;

		[Header("Sponge Degradation")]
		[SerializeField]
		[Range(0.01f, 0.1f)]
		private float _degradationSpeed = 0.02f;

		[SerializeField]
		private Renderer _spongeRenderer;

		[SerializeField]
		private Color _degradedColor = Color.black;

		[SerializeField]
		private int _spongeMaterialIndex;

		[Header("Audio")]
		[SerializeField]
		private SoundID scrubEvent;

		[Tooltip("Meters the tool must move across the surface to play one scrub one-shot.")]
		[SerializeField]
		[Min(0.01f)]
		private float scrubDistancePerSip = 0.12f;

		[Tooltip("Minimum seconds between scrub one-shots (caps the rate when moving fast).")]
		[SerializeField]
		[Min(0f)]
		private float scrubMinInterval = 0.1f;

		[SyncVar(hook = "OnConditionChanged")]
		private float _condition = 1f;

		[SyncVar]
		private bool _syncedToolActive;

		[SyncVar]
		private float _syncedRadius;

		[SyncVar]
		private float _syncedOpacity;

		[Inject]
		private ICastingManager _castingManager;

		private ToolSnapHelper _snapHelper;

		private IRaycastHandle _raycastHandle;

		private readonly ToolInteractionSuppressor _interactionSuppressor = new ToolInteractionSuppressor();

		private RestorationHitRelay _hitRelay;

		private bool _isToolActive;

		private bool _useButtonHeld;

		private const float CastStillPositionEpsilonSqr = 1E-08f;

		private const float CastStillAngleEpsilonDegrees = 0.05f;

		private float _detectionTimer;

		private Vector3 _lastCastCamPos;

		private Quaternion _lastCastCamRot;

		private bool _hasLastCast;

		private CwPaintSphere _dirtSphere;

		private CwPaintSphere _rustBlurSphere;

		private InteractionStateMachine<SpongeState> _spongeStateMachine;

		private Color _originalColor;

		private bool _originalColorCached;

		private MaterialPropertyBlock _spongePropertyBlock;

		private static readonly int BaseColorProperty;

		private Vector3 _lastScrubPos;

		private bool _hasLastScrubPos;

		private float _scrubDistance;

		private float _scrubCooldown;

		private bool _hitThisFrame;

		public Action<float, float> _Mirror_SyncVarHookDelegate__condition;

		public float Condition => _condition;

		public float ConditionRatio => Mathf.Clamp01(_condition);

		public bool IsDepleted => _condition <= 0f;

		public bool IsToolActive => _isToolActive;

		public bool IsSnapped => _snapHelper?.IsSnapped ?? false;

		public GameObject CurrentTarget => _snapHelper?.CurrentTarget;

		protected override bool UseStateMachine => true;

		protected override bool UseDefaultStateMachine => false;

		public bool IsRelaySource => base.isOwned;

		public float Network_condition
		{
			get
			{
				return _condition;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _condition, 512uL, _Mirror_SyncVarHookDelegate__condition);
			}
		}

		public bool Network_syncedToolActive
		{
			get
			{
				return _syncedToolActive;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedToolActive, 1024uL, null);
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
				GeneratedSyncVarSetter(value, ref _syncedRadius, 2048uL, null);
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
				GeneratedSyncVarSetter(value, ref _syncedOpacity, 4096uL, null);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			InitSnapHelper();
			SetupHitRelay();
			ValidateComponents();
			DisableTool();
		}

		protected override void Start()
		{
			base.Start();
			ConfigurePaintSpheres();
		}

		private void Update()
		{
			TickSurfaceDetection();
			_snapHelper?.TickLerp(base.transform);
			if (_isToolActive)
			{
				Vector3 vector = ((_toolTip != null) ? _toolTip.position : base.transform.position);
				if (!_hasLastScrubPos)
				{
					_lastScrubPos = vector;
					_hasLastScrubPos = true;
				}
				float magnitude = (vector - _lastScrubPos).magnitude;
				_lastScrubPos = vector;
				if (magnitude <= 0.5f)
				{
					_scrubDistance += magnitude;
				}
				if (_scrubCooldown > 0f)
				{
					_scrubCooldown -= Time.deltaTime;
				}
				if (_scrubDistance >= scrubDistancePerSip && _scrubCooldown <= 0f)
				{
					_scrubDistance = 0f;
					_scrubCooldown = scrubMinInterval;
					if (scrubEvent.IsValid())
					{
						AudioManager?.PlayOneShot(scrubEvent, base.transform.position);
					}
				}
			}
			else
			{
				_hasLastScrubPos = false;
				_scrubDistance = 0f;
				_scrubCooldown = 0f;
			}
			if (base.isServer && _hitThisFrame)
			{
				_hitThisFrame = false;
				Network_condition = _condition - _degradationSpeed * Time.deltaTime;
				if (_condition <= 0f)
				{
					Network_condition = 0f;
					DisableTool();
					UpdateState();
				}
			}
		}

		private void OnDestroy()
		{
			_interactionSuppressor.SetSuppressed(playerService, suppress: false);
			_castingManager?.Unregister(_raycastHandle);
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

		private void ValidateComponents()
		{
			if (_toolTip == null)
			{
				_toolTip = base.transform.Find("ToolTip");
				if (_toolTip == null)
				{
					EvilLogger.LogError("[CleanTool] ToolTip transform not found!", "ValidateComponents", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Restoration\\Scripts\\Networked\\CleanTool.cs", 247);
				}
			}
			if (_hitBetween == null && _toolTip != null)
			{
				_hitBetween = _toolTip.GetComponent<CwHitBetween>();
				if (_hitBetween == null)
				{
					EvilLogger.LogError("[CleanTool] CwHitBetween not found on ToolTip!", "ValidateComponents", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Restoration\\Scripts\\Networked\\CleanTool.cs", 254);
				}
			}
			if (_paintSpheres == null || _paintSpheres.Length == 0)
			{
				_paintSpheres = GetComponentsInChildren<CwPaintSphere>();
				if (_paintSpheres.Length == 0)
				{
					EvilLogger.LogError("[CleanTool] No CwPaintSphere components found!", "ValidateComponents", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Restoration\\Scripts\\Networked\\CleanTool.cs", 261);
				}
			}
		}

		protected override void InitializeStateMachine()
		{
			_spongeStateMachine = new InteractionStateMachine<SpongeState>(this);
			base.BaseStateMachine = _spongeStateMachine;
			ConfigureStates();
			_spongeStateMachine.Initialize(DetermineSpongeState());
		}

		protected override void ConfigureStates()
		{
			_spongeStateMachine.RegisterState(SpongeState.Idle, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(SpongeState.Equipped, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default)).RegisterState(SpongeState.Depleted, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default));
		}

		private SpongeState DetermineSpongeState()
		{
			if (!base.IsEquipped)
			{
				return SpongeState.Idle;
			}
			if (IsDepleted)
			{
				return SpongeState.Depleted;
			}
			return SpongeState.Equipped;
		}

		public override void UpdateState()
		{
			_spongeStateMachine?.TransitionTo(DetermineSpongeState());
		}

		private void RegisterSurfaceDetection()
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

		private void UnregisterSurfaceDetection()
		{
			_castingManager?.Unregister(_raycastHandle);
			_raycastHandle = null;
		}

		private void TickSurfaceDetection()
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

		private void OnSurfaceDetected(CastResult result)
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

		private void OnSnap()
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

		private void OnRelease()
		{
			DisableTool();
			EndSnapStream();
			SetSnapConstraintSuspended(suspend: false);
			playerService?.EquipmentManager?.OnGripResumeRequested?.Invoke();
			_interactionSuppressor.SetSuppressed(playerService, suppress: false);
		}

		public void EnableTool()
		{
			if (_isToolActive || IsDepleted)
			{
				return;
			}
			_isToolActive = true;
			if (_snapHelper != null)
			{
				_snapHelper.SnapOffset = _snapOffset;
			}
			float activeRadius = _activeRadius;
			float opacity = _opacity;
			ApplyToolStateToSpheres(activeRadius, opacity);
			if (_hitBetween != null)
			{
				_hitBetween.enabled = true;
			}
			if (_toolEffect != null)
			{
				_toolEffect.Play();
			}
			if (base.netIdentity != null)
			{
				if (base.isServer)
				{
					Network_syncedToolActive = true;
					Network_syncedRadius = activeRadius;
					Network_syncedOpacity = opacity;
					RpcEnableTool(activeRadius, opacity);
				}
				else if (base.isOwned)
				{
					CmdEnableTool(activeRadius, opacity);
				}
			}
		}

		public void DisableTool()
		{
			_isToolActive = false;
			if (_snapHelper != null)
			{
				_snapHelper.SnapOffset = _aligningOffset;
			}
			ApplyToolStateToSpheres(0f, 0f);
			if (_hitBetween != null)
			{
				_hitBetween.enabled = false;
			}
			if (_toolEffect != null)
			{
				_toolEffect.Stop();
			}
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

		private void ApplyToolStateToSpheres(float radius, float opacity)
		{
			if (_paintSpheres == null)
			{
				return;
			}
			CwPaintSphere[] paintSpheres = _paintSpheres;
			foreach (CwPaintSphere cwPaintSphere in paintSpheres)
			{
				if (!(cwPaintSphere == null))
				{
					cwPaintSphere.Radius = radius;
					if (opacity > 0f)
					{
						cwPaintSphere.Opacity = opacity;
					}
				}
			}
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

		public override void OnUseButtonDown()
		{
			_useButtonHeld = true;
			if (_snapHelper.IsSnapped)
			{
				EnableTool();
			}
		}

		public override void OnUseButtonUp()
		{
			_useButtonHeld = false;
			DisableTool();
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

		[Command]
		private void CmdEnableTool(float radius, float opacity)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(radius);
			writer.WriteFloat(opacity);
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.CleanTool::CmdEnableTool(System.Single,System.Single)", -1626401487, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcEnableTool(float radius, float opacity)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(radius);
			writer.WriteFloat(opacity);
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.CleanTool::RpcEnableTool(System.Single,System.Single)", 1066842760, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		private void CmdDisableTool()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.CleanTool::CmdDisableTool()", 2106398148, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcDisableTool()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.CleanTool::RpcDisableTool()", 946549277, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
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
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.CleanTool::CmdRelayHitPoint(UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Int32)", 649691147, writer, 0);
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
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.CleanTool::RpcRelayHitPoint(UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Int32)", 36077446, writer, 0, includeOwner: true);
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
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.CleanTool::CmdRelayHitLine(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Boolean,System.Int32)", -951968469, writer, 0);
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
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.CleanTool::RpcRelayHitLine(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Boolean,System.Int32)", -481918650, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void OnConditionChanged(float oldValue, float newValue)
		{
			if (IsLateJoinCompleted)
			{
				CacheOriginalColor();
				ApplyDegradationVisual(newValue);
				if (newValue <= 0f && _isToolActive)
				{
					DisableTool();
				}
				UpdateState();
			}
		}

		private void CacheOriginalColor()
		{
			if (!_originalColorCached && !(_spongeRenderer == null))
			{
				_spongePropertyBlock = new MaterialPropertyBlock();
				_spongeRenderer.GetPropertyBlock(_spongePropertyBlock, _spongeMaterialIndex);
				Material material = _spongeRenderer.sharedMaterials[_spongeMaterialIndex];
				_originalColor = ((material != null && material.HasProperty(BaseColorProperty)) ? material.GetColor(BaseColorProperty) : Color.white);
				_originalColorCached = true;
			}
		}

		private void ApplyDegradationVisual(float condition)
		{
			if (!(_spongeRenderer == null) && _originalColorCached)
			{
				Color value = Color.Lerp(_degradedColor, _originalColor, condition);
				_spongePropertyBlock.SetColor(BaseColorProperty, value);
				_spongeRenderer.SetPropertyBlock(_spongePropertyBlock, _spongeMaterialIndex);
			}
		}

		public void ResetCondition()
		{
			CmdResetCondition();
		}

		[Command(requiresAuthority = false)]
		private void CmdResetCondition()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.CleanTool::CmdResetCondition()", 1109281828, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ServerRestoreCondition(float amount)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Restoration.CleanTool::ServerRestoreCondition(System.Single)' called when server was not active");
			}
			else
			{
				Network_condition = Mathf.Clamp01(_condition + amount);
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			CacheOriginalColor();
			ApplyDegradationVisual(_condition);
			if (_syncedToolActive && !_isToolActive)
			{
				_isToolActive = true;
				ApplyToolStateToSpheres(_syncedRadius, _syncedOpacity);
				if (_hitBetween != null)
				{
					_hitBetween.enabled = false;
				}
				if (_toolEffect != null)
				{
					_toolEffect.Play();
				}
			}
			UpdateState();
		}

		private void ConfigurePaintSpheres()
		{
			if (_paintSpheres == null || _paintSpheres.Length < 1)
			{
				return;
			}
			_dirtSphere = _paintSpheres[0];
			if (_dirtSphere != null)
			{
				_dirtSphere.Group = new CwGroup(_dirtGroupIndex);
				_dirtSphere.Color = Color.white;
				_dirtSphere.Opacity = _opacity;
				_dirtSphere.Hardness = _hardness;
				_dirtSphere.BlendMode = CwBlendMode.Subtractive(new Vector4(1f, 0f, 0f, 0f));
			}
			if (_enableRustBlur && _paintSpheres.Length >= 2)
			{
				_rustBlurSphere = _paintSpheres[1];
				if (_rustBlurSphere != null)
				{
					_rustBlurSphere.Group = new CwGroup(_rustGroupIndex);
					_rustBlurSphere.Color = Color.white;
					_rustBlurSphere.Opacity = _rustBlurOpacity;
					_rustBlurSphere.Hardness = _rustBlurHardness;
					_rustBlurSphere.BlendMode = CwBlendMode.Blur(_rustBlurKernel, new Vector4(1f, 0f, 0f, 0f));
				}
			}
		}

		public void SetRustBlurKernel(float kernel)
		{
			_rustBlurKernel = Mathf.Clamp(kernel, 1f, 8f);
			if (_rustBlurSphere != null)
			{
				_rustBlurSphere.BlendMode = CwBlendMode.Blur(_rustBlurKernel, new Vector4(1f, 0f, 0f, 0f));
			}
		}

		public void SetRustBlurEnabled(bool enabled)
		{
			_enableRustBlur = enabled;
			if (_rustBlurSphere != null)
			{
				_rustBlurSphere.enabled = enabled;
			}
		}

		private void FullAutoSetup()
		{
			if (_toolTip == null)
			{
				_toolTip = base.transform.Find("ToolTip");
				if (_toolTip == null)
				{
					GameObject gameObject = new GameObject("ToolTip");
					gameObject.transform.SetParent(base.transform);
					gameObject.transform.localPosition = Vector3.zero;
					_toolTip = gameObject.transform;
				}
			}
			Transform transform = _toolTip.Find("PointA");
			if (transform == null)
			{
				GameObject obj = new GameObject("PointA");
				obj.transform.SetParent(_toolTip);
				obj.transform.localPosition = Vector3.back * 0.2f;
				transform = obj.transform;
			}
			Transform transform2 = _toolTip.Find("PointB");
			if (transform2 == null)
			{
				GameObject obj2 = new GameObject("PointB");
				obj2.transform.SetParent(_toolTip);
				obj2.transform.localPosition = Vector3.zero;
				transform2 = obj2.transform;
			}
			_hitBetween = _toolTip.GetComponent<CwHitBetween>();
			if (_hitBetween == null)
			{
				_hitBetween = _toolTip.gameObject.AddComponent<CwHitBetween>();
			}
			_hitBetween.PointA = transform;
			_hitBetween.PointB = transform2;
			SetupPaintSpheresOnToolTip();
			_paintSpheres = _toolTip.GetComponentsInChildren<CwPaintSphere>();
		}

		private void SetupPaintSpheresOnToolTip()
		{
			if (!(_toolTip == null))
			{
				CwPaintSphere cwPaintSphere = CreateOrFindSphere("Sphere_Clean");
				if (_enableRustBlur)
				{
					CwPaintSphere cwPaintSphere2 = CreateOrFindSphere("Sphere_RustBlur");
					_paintSpheres = new CwPaintSphere[2] { cwPaintSphere, cwPaintSphere2 };
				}
				else
				{
					_paintSpheres = new CwPaintSphere[1] { cwPaintSphere };
				}
				ConfigurePaintSpheres();
			}
		}

		private CwPaintSphere CreateOrFindSphere(string sphereName)
		{
			Transform transform = _toolTip.Find(sphereName);
			if (transform != null)
			{
				return transform.GetComponent<CwPaintSphere>() ?? transform.gameObject.AddComponent<CwPaintSphere>();
			}
			GameObject obj = new GameObject(sphereName);
			obj.transform.SetParent(_toolTip);
			obj.transform.localPosition = Vector3.zero;
			return obj.AddComponent<CwPaintSphere>();
		}

		private void FindComponents()
		{
			if (_toolTip == null)
			{
				_toolTip = base.transform.Find("ToolTip");
			}
			if (_toolTip != null && _hitBetween == null)
			{
				_hitBetween = _toolTip.GetComponent<CwHitBetween>();
			}
			_paintSpheres = GetComponentsInChildren<CwPaintSphere>();
		}

		public CleanTool()
		{
			_Mirror_SyncVarHookDelegate__condition = OnConditionChanged;
		}

		static CleanTool()
		{
			BaseColorProperty = Shader.PropertyToID("_BaseColor");
			RemoteProcedureCalls.RegisterCommand(typeof(CleanTool), "System.Void NomadDrive.Features.Restoration.CleanTool::CmdEnableTool(System.Single,System.Single)", InvokeUserCode_CmdEnableTool__Single__Single, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(CleanTool), "System.Void NomadDrive.Features.Restoration.CleanTool::CmdDisableTool()", InvokeUserCode_CmdDisableTool, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(CleanTool), "System.Void NomadDrive.Features.Restoration.CleanTool::CmdRelayHitPoint(UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Int32)", InvokeUserCode_CmdRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(CleanTool), "System.Void NomadDrive.Features.Restoration.CleanTool::CmdRelayHitLine(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Boolean,System.Int32)", InvokeUserCode_CmdRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(CleanTool), "System.Void NomadDrive.Features.Restoration.CleanTool::CmdResetCondition()", InvokeUserCode_CmdResetCondition, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(CleanTool), "System.Void NomadDrive.Features.Restoration.CleanTool::RpcEnableTool(System.Single,System.Single)", InvokeUserCode_RpcEnableTool__Single__Single);
			RemoteProcedureCalls.RegisterRpc(typeof(CleanTool), "System.Void NomadDrive.Features.Restoration.CleanTool::RpcDisableTool()", InvokeUserCode_RpcDisableTool);
			RemoteProcedureCalls.RegisterRpc(typeof(CleanTool), "System.Void NomadDrive.Features.Restoration.CleanTool::RpcRelayHitPoint(UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Int32)", InvokeUserCode_RpcRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32);
			RemoteProcedureCalls.RegisterRpc(typeof(CleanTool), "System.Void NomadDrive.Features.Restoration.CleanTool::RpcRelayHitLine(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Boolean,System.Int32)", InvokeUserCode_RpcRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32);
		}

		public override bool Weaved()
		{
			return true;
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
				((CleanTool)obj).UserCode_CmdEnableTool__Single__Single(reader.ReadFloat(), reader.ReadFloat());
			}
		}

		protected void UserCode_RpcEnableTool__Single__Single(float radius, float opacity)
		{
			if (!base.isOwned || !_isToolActive)
			{
				_isToolActive = true;
				ApplyToolStateToSpheres(radius, opacity);
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
				((CleanTool)obj).UserCode_RpcEnableTool__Single__Single(reader.ReadFloat(), reader.ReadFloat());
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
				((CleanTool)obj).UserCode_CmdDisableTool();
			}
		}

		protected void UserCode_RpcDisableTool()
		{
			if (!base.isOwned || _isToolActive)
			{
				_isToolActive = false;
				ApplyToolStateToSpheres(0f, 0f);
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
				((CleanTool)obj).UserCode_RpcDisableTool();
			}
		}

		protected void UserCode_CmdRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32(Vector3 position, Quaternion rotation, float pressure, int seed, int priority)
		{
			_hitThisFrame = true;
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
				((CleanTool)obj).UserCode_CmdRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32(reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadFloat(), reader.ReadVarInt(), reader.ReadVarInt());
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
				((CleanTool)obj).UserCode_RpcRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32(reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadFloat(), reader.ReadVarInt(), reader.ReadVarInt());
			}
		}

		protected void UserCode_CmdRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32(Vector3 position, Vector3 endPosition, Quaternion rotation, float pressure, int seed, bool clip, int priority)
		{
			_hitThisFrame = true;
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
				((CleanTool)obj).UserCode_CmdRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32(reader.ReadVector3(), reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadFloat(), reader.ReadVarInt(), reader.ReadBool(), reader.ReadVarInt());
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
				((CleanTool)obj).UserCode_RpcRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32(reader.ReadVector3(), reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadFloat(), reader.ReadVarInt(), reader.ReadBool(), reader.ReadVarInt());
			}
		}

		protected void UserCode_CmdResetCondition()
		{
			Network_condition = 1f;
		}

		protected static void InvokeUserCode_CmdResetCondition(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdResetCondition called on client.");
			}
			else
			{
				((CleanTool)obj).UserCode_CmdResetCondition();
			}
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteFloat(_condition);
				writer.WriteBool(_syncedToolActive);
				writer.WriteFloat(_syncedRadius);
				writer.WriteFloat(_syncedOpacity);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				writer.WriteFloat(_condition);
			}
			if ((syncVarDirtyBits & 0x400L) != 0L)
			{
				writer.WriteBool(_syncedToolActive);
			}
			if ((syncVarDirtyBits & 0x800L) != 0L)
			{
				writer.WriteFloat(_syncedRadius);
			}
			if ((syncVarDirtyBits & 0x1000L) != 0L)
			{
				writer.WriteFloat(_syncedOpacity);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _condition, _Mirror_SyncVarHookDelegate__condition, reader.ReadFloat());
				GeneratedSyncVarDeserialize(ref _syncedToolActive, null, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _syncedRadius, null, reader.ReadFloat());
				GeneratedSyncVarDeserialize(ref _syncedOpacity, null, reader.ReadFloat());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _condition, _Mirror_SyncVarHookDelegate__condition, reader.ReadFloat());
			}
			if ((num & 0x400L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedToolActive, null, reader.ReadBool());
			}
			if ((num & 0x800L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedRadius, null, reader.ReadFloat());
			}
			if ((num & 0x1000L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedOpacity, null, reader.ReadFloat());
			}
		}
	}
}
