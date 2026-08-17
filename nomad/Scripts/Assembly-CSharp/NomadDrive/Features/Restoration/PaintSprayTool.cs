using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.DynamicCasting;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Tools;
using PaintIn3D;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Restoration
{
	public class PaintSprayTool : DirectHeldItem, INetworkHitRelay
	{
		[Header("Tool Tip Configuration")]
		[SerializeField]
		private Transform _toolTip;

		[Header("Particle System")]
		[SerializeField]
		private ParticleSystem _particleSystem;

		[SerializeField]
		private CwHitParticles _hitParticles;

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
		[Range(-0.5f, 1f)]
		private float _aligningOffset = 0.35f;

		[SerializeField]
		[Range(-0.5f, 1f)]
		private float _snapOffset = 0.3f;

		[Header("Align Reference")]
		[Tooltip("Tool's surface contact point (e.g. spray nozzle tip). Hit point aligns to this transform.")]
		[SerializeField]
		private Transform _alignTransform;

		[SerializeField]
		private bool _useColliderBoundsForSnap = true;

		[SerializeField]
		private Vector3 _snapRotationOffset = Vector3.zero;

		[Inject]
		private ICastingManager _castingManager;

		private ToolSnapHelper _snapHelper;

		private IRaycastHandle _raycastHandle;

		private readonly ToolInteractionSuppressor _interactionSuppressor = new ToolInteractionSuppressor();

		private const float CastStillPositionEpsilonSqr = 1E-08f;

		private const float CastStillAngleEpsilonDegrees = 0.05f;

		private float _detectionTimer;

		private Vector3 _lastCastCamPos;

		private Quaternion _lastCastCamRot;

		private bool _hasLastCast;

		[Header("Paint Spheres")]
		[SerializeField]
		private CwPaintSphere _colorSphere;

		[SerializeField]
		private CwPaintSphere _maskSphere;

		[Header("Paint Spray Settings")]
		[SerializeField]
		private Color _paintColor = Color.red;

		[SerializeField]
		[Range(0.01f, 0.5f)]
		private float _sphereRadius = 0.08f;

		[SerializeField]
		[Range(0.001f, 20f)]
		private float _hardness = 1.5f;

		[SerializeField]
		[Range(0f, 1f)]
		private float _colorOpacity = 0.15f;

		[SerializeField]
		[Range(0f, 1f)]
		private float _maskOpacity = 0.1f;

		[Header("Spray Pattern")]
		[SerializeField]
		[Range(10f, 500f)]
		private float _emissionRate = 150f;

		[SerializeField]
		[Range(1f, 20f)]
		private float _particleSpeed = 6f;

		[SerializeField]
		[Range(0f, 45f)]
		private float _sprayAngle = 12f;

		[SerializeField]
		[Range(0.1f, 2f)]
		private float _particleLifetime = 0.4f;

		[Header("Target Groups")]
		[SerializeField]
		private int _paintColorGroupIndex = 200;

		[SerializeField]
		private int _paintMaskGroupIndex = 204;

		[Header("Visual Model")]
		[SerializeField]
		private Renderer visualModelRenderer;

		[SerializeField]
		private int visualModelMaterialIndex;

		[SerializeField]
		private string colorPropertyName = "_BaseColor";

		[Header("Audio")]
		[Tooltip("Looping AudioEntity (mark it Loop in BroAudio) played while actively spraying a surface.")]
		[SerializeField]
		private SoundID sprayEvent;

		[Header("Paint Capacity")]
		[SerializeField]
		[Range(0f, 100f)]
		private float _maxPaintCapacity = 100f;

		[SerializeField]
		[Min(0f)]
		private float _paintConsumptionRate = 5f;

		[SyncVar]
		private bool _syncedToolActive;

		[SyncVar]
		private Color _syncedPaintColor;

		[SyncVar(hook = "OnPaintCapacityChanged")]
		private float _paintCapacity;

		[Inject]
		private IGameUIManager _guiManager;

		private SprayPaintInfoPanel _sprayInfoPanel;

		private PaintSprayCore _core;

		private RestorationHitRelay _hitRelay;

		private AudioHandle _sprayInstance;

		private bool _sprayPlaying;

		private bool _useButtonHeld;

		private bool _panelActive;

		private MaterialPropertyBlock _visualPropertyBlock;

		private int _colorPropertyId;

		public Action<float, float> _Mirror_SyncVarHookDelegate__paintCapacity;

		public GameObject CurrentTarget => _snapHelper?.CurrentTarget;

		public bool IsSnapped => _snapHelper?.IsSnapped ?? false;

		public bool IsToolActive => _core?.IsToolActive ?? false;

		public float PaintCapacity => _paintCapacity;

		public float PaintCapacityRatio
		{
			get
			{
				if (!(_maxPaintCapacity > 0f))
				{
					return 0f;
				}
				return Mathf.Clamp01(_paintCapacity / _maxPaintCapacity);
			}
		}

		public bool HasPaint => _paintCapacity > 0f;

		public Color PaintColor
		{
			get
			{
				return _paintColor;
			}
			set
			{
				_paintColor = value;
				SyncToCore();
				_core.UpdatePaintColor();
				UpdateVisualModelColor();
				_sprayInfoPanel?.SetColor(_paintColor);
			}
		}

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
				GeneratedSyncVarSetter(value, ref _syncedToolActive, 512uL, null);
			}
		}

		public Color Network_syncedPaintColor
		{
			get
			{
				return _syncedPaintColor;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _syncedPaintColor, 1024uL, null);
			}
		}

		public float Network_paintCapacity
		{
			get
			{
				return _paintCapacity;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _paintCapacity, 2048uL, _Mirror_SyncVarHookDelegate__paintCapacity);
			}
		}

		[Inject]
		private void ResolveSprayInfoPanel(IObjectResolver resolver)
		{
			resolver.TryResolve<SprayPaintInfoPanel>(out _sprayInfoPanel);
		}

		protected override void Awake()
		{
			base.Awake();
			InitCore();
			_core.ValidateSetup(base.transform);
			InitSnapHelper();
			SetupHitRelay();
			DisableTool();
		}

		private void InitCore()
		{
			_core = new PaintSprayCore();
			SyncToCore();
		}

		private void SyncToCore()
		{
			_core.ToolTip = _toolTip;
			_core.ParticleSystem = _particleSystem;
			_core.HitParticles = _hitParticles;
			_core.ColorSphere = _colorSphere;
			_core.MaskSphere = _maskSphere;
			_core.PaintColor = _paintColor;
			_core.SphereRadius = _sphereRadius;
			_core.Hardness = _hardness;
			_core.ColorOpacity = _colorOpacity;
			_core.MaskOpacity = _maskOpacity;
			_core.EmissionRate = _emissionRate;
			_core.ParticleSpeed = _particleSpeed;
			_core.SprayAngle = _sprayAngle;
			_core.ParticleLifetime = _particleLifetime;
			_core.PaintColorGroupIndex = _paintColorGroupIndex;
			_core.PaintMaskGroupIndex = _paintMaskGroupIndex;
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
			SyncToCore();
			_core.ConfigureSpheres();
			_colorPropertyId = Shader.PropertyToID(colorPropertyName);
			UpdateVisualModelColor();
		}

		public void EnableTool()
		{
			if (HasPaint)
			{
				if (_snapHelper != null)
				{
					_snapHelper.SnapOffset = _snapOffset;
				}
				SyncToCore();
				_core.EnableToolLocal();
				if (_core.IsToolActive && base.isOwned)
				{
					StartSprayAudio();
					CmdEnableTool(_paintColor);
				}
				_ = _core.IsToolActive;
			}
		}

		public void DisableTool()
		{
			if (_snapHelper != null)
			{
				_snapHelper.SnapOffset = _aligningOffset;
			}
			SyncToCore();
			_core.DisableToolLocal();
			StopSprayAudio();
			if (base.netIdentity != null && base.isOwned)
			{
				CmdDisableTool();
			}
		}

		public void ToggleTool()
		{
			if (_core.IsToolActive)
			{
				DisableTool();
			}
			else
			{
				EnableTool();
			}
		}

		private void OnSpraySettingsChanged()
		{
			if (_core != null && Application.isPlaying)
			{
				SyncToCore();
				_core.ConfigureSpheres();
				_core.UpdatePaintColor();
				UpdateVisualModelColor();
			}
		}

		private void OnParticleSettingsChanged()
		{
			if (_core != null && Application.isPlaying)
			{
				SyncToCore();
				_core.ConfigureParticleSystem();
			}
		}

		public void SetColorRed()
		{
			PaintColor = new Color(0.8f, 0.1f, 0.1f);
		}

		public void SetColorBlue()
		{
			PaintColor = new Color(0.1f, 0.2f, 0.8f);
		}

		public void SetColorGreen()
		{
			PaintColor = new Color(0.1f, 0.6f, 0.1f);
		}

		public void SetColorWhite()
		{
			PaintColor = Color.white;
		}

		public void SetColorBlack()
		{
			PaintColor = new Color(0.05f, 0.05f, 0.05f);
		}

		public void SetColorYellow()
		{
			PaintColor = new Color(0.9f, 0.8f, 0.1f);
		}

		public void SetPresetUltraSmooth()
		{
			_sphereRadius = 0.12f;
			_hardness = 1f;
			_colorOpacity = 0.1f;
			_maskOpacity = 0.08f;
			_emissionRate = 200f;
			_particleSpeed = 5f;
			_sprayAngle = 10f;
			SyncAndUpdate();
		}

		public void SetPresetBalanced()
		{
			_sphereRadius = 0.08f;
			_hardness = 1.5f;
			_colorOpacity = 0.15f;
			_maskOpacity = 0.1f;
			_emissionRate = 150f;
			_particleSpeed = 6f;
			_sprayAngle = 12f;
			SyncAndUpdate();
		}

		public void SetPresetPerformance()
		{
			_sphereRadius = 0.05f;
			_hardness = 3f;
			_colorOpacity = 0.25f;
			_maskOpacity = 0.15f;
			_emissionRate = 80f;
			_particleSpeed = 8f;
			_sprayAngle = 15f;
			SyncAndUpdate();
		}

		public void SetPresetAirbrush()
		{
			_sphereRadius = 0.15f;
			_hardness = 0.5f;
			_colorOpacity = 0.05f;
			_maskOpacity = 0.03f;
			_emissionRate = 300f;
			_particleSpeed = 4f;
			_sprayAngle = 8f;
			SyncAndUpdate();
		}

		private void SyncAndUpdate()
		{
			SyncToCore();
			_core.ConfigureParticleSystem();
			_core.ConfigureSpheres();
		}

		private void FullAutoSetup()
		{
			SyncToCore();
			_core.FullAutoSetup(base.transform);
			_toolTip = _core.ToolTip;
			_particleSystem = _core.ParticleSystem;
			_hitParticles = _core.HitParticles;
			_colorSphere = _core.ColorSphere;
			_maskSphere = _core.MaskSphere;
		}

		private void UpdateParticleSettings()
		{
			SyncToCore();
			_core.ConfigureParticleSystem();
			_core.ConfigureSpheres();
		}

		private void FindComponents()
		{
			SyncToCore();
			_core.FindComponents(base.transform);
			_toolTip = _core.ToolTip;
			_particleSystem = _core.ParticleSystem;
			_hitParticles = _core.HitParticles;
			_colorSphere = _core.ColorSphere;
			_maskSphere = _core.MaskSphere;
		}

		public override void OnEquip()
		{
			base.OnEquip();
			RegisterSurfaceDetection();
			_panelActive = true;
			_sprayInfoPanel?.SetTool(_paintColor, PaintCapacityRatio);
			_guiManager?.ShowCanvasGroup(GameCanvasGroupName.SprayPaintInfo, interactable: false, blockRaycast: false);
			_sprayInfoPanel?.ClearWorldTarget();
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
			_panelActive = false;
			_sprayInfoPanel?.Clear();
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.SprayPaintInfo);
			_sprayInfoPanel?.ClearWorldTarget();
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			_panelActive = true;
			_sprayInfoPanel?.SetTool(_paintColor, PaintCapacityRatio);
			_guiManager?.ShowCanvasGroup(GameCanvasGroupName.SprayPaintInfo, interactable: false, blockRaycast: false);
			_sprayInfoPanel?.SetWorldTarget(base.gameObject);
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			if (!base.IsEquipped)
			{
				_panelActive = false;
				_sprayInfoPanel?.Clear();
				_guiManager?.HideCanvasGroup(GameCanvasGroupName.SprayPaintInfo);
				_sprayInfoPanel?.ClearWorldTarget();
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
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.PaintSprayTool::CmdRelayHitPoint(UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Int32)", -284928757, writer, 0);
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
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.PaintSprayTool::RpcRelayHitPoint(UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Int32)", -898542458, writer, 0, includeOwner: true);
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
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.PaintSprayTool::CmdRelayHitLine(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Boolean,System.Int32)", 1785199147, writer, 0);
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
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.PaintSprayTool::RpcRelayHitLine(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Boolean,System.Int32)", -2039718330, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		private void CmdEnableTool(Color paintColor)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteColor(paintColor);
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.PaintSprayTool::CmdEnableTool(UnityEngine.Color)", -926506301, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcEnableTool(Color paintColor)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteColor(paintColor);
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.PaintSprayTool::RpcEnableTool(UnityEngine.Color)", 1903799970, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		private void CmdDisableTool()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.PaintSprayTool::CmdDisableTool()", -29599036, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcDisableTool()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.PaintSprayTool::RpcDisableTool()", -1189447907, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		public void SetPaintColorNetworked(Color color)
		{
			_paintColor = color;
			SyncToCore();
			_core.UpdatePaintColor();
			UpdateVisualModelColor();
			_sprayInfoPanel?.SetColor(_paintColor);
			if (base.isOwned)
			{
				CmdSetPaintColor(color);
			}
		}

		[Command]
		private void CmdSetPaintColor(Color color)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteColor(color);
			SendCommandInternal("System.Void NomadDrive.Features.Restoration.PaintSprayTool::CmdSetPaintColor(UnityEngine.Color)", -1757609261, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcSetPaintColor(Color color)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteColor(color);
			SendRPCInternal("System.Void NomadDrive.Features.Restoration.PaintSprayTool::RpcSetPaintColor(UnityEngine.Color)", -428752188, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			Network_paintCapacity = _maxPaintCapacity;
		}

		private void Update()
		{
			TickSurfaceDetection();
			_snapHelper?.TickLerp(base.transform);
			if (base.isServer && _syncedToolActive && !(_paintCapacity <= 0f))
			{
				Network_paintCapacity = _paintCapacity - _paintConsumptionRate * Time.deltaTime;
				if (_paintCapacity <= 0f)
				{
					Network_paintCapacity = 0f;
					Network_syncedToolActive = false;
					RpcDisableTool();
				}
			}
		}

		private void OnPaintCapacityChanged(float oldValue, float newValue)
		{
			if (IsLateJoinCompleted)
			{
				if (_panelActive)
				{
					_sprayInfoPanel?.UpdateCapacity(PaintCapacityRatio);
				}
				if (newValue <= 0f && IsToolActive)
				{
					SyncToCore();
					_core.DisableToolLocal();
					StopSprayAudio();
				}
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (_syncedToolActive && !IsToolActive)
			{
				_paintColor = _syncedPaintColor;
				SyncToCore();
				_core.UpdatePaintColor();
				_core.ConfigureSpheres();
				_core.EnableToolLocal();
				UpdateVisualModelColor();
				StartSprayAudio();
			}
		}

		private void OnDestroy()
		{
			_interactionSuppressor.SetSuppressed(playerService, suppress: false);
			StopSprayAudio();
			_castingManager?.Unregister(_raycastHandle);
		}

		private void UpdateVisualModelColor()
		{
			if (!(visualModelRenderer == null))
			{
				if (_visualPropertyBlock == null)
				{
					_visualPropertyBlock = new MaterialPropertyBlock();
				}
				visualModelRenderer.GetPropertyBlock(_visualPropertyBlock, visualModelMaterialIndex);
				_visualPropertyBlock.SetColor(_colorPropertyId, _paintColor);
				visualModelRenderer.SetPropertyBlock(_visualPropertyBlock, visualModelMaterialIndex);
			}
		}

		private void StartSprayAudio()
		{
			if (AudioManager != null && sprayEvent.IsValid())
			{
				if (_sprayPlaying && !_sprayInstance.IsValid)
				{
					_sprayPlaying = false;
				}
				if (!_sprayPlaying)
				{
					_sprayInstance = AudioManager.PlayEventAttached(sprayEvent, base.gameObject);
					_sprayPlaying = true;
				}
			}
		}

		private void StopSprayAudio()
		{
			if (_sprayPlaying)
			{
				AudioManager?.StopEvent(_sprayInstance);
				_sprayPlaying = false;
			}
		}

		public PaintSprayTool()
		{
			_Mirror_SyncVarHookDelegate__paintCapacity = OnPaintCapacityChanged;
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
				((PaintSprayTool)obj).UserCode_CmdRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32(reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadFloat(), reader.ReadVarInt(), reader.ReadVarInt());
			}
		}

		protected void UserCode_RpcRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32(Vector3 position, Quaternion rotation, float pressure, int seed, int priority)
		{
			if (!base.isOwned)
			{
				if (_colorSphere != null)
				{
					_colorSphere.HandleHitPoint(preview: false, priority, pressure, seed, position, rotation);
				}
				if (_maskSphere != null)
				{
					_maskSphere.HandleHitPoint(preview: false, priority, pressure, seed, position, rotation);
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
				((PaintSprayTool)obj).UserCode_RpcRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32(reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadFloat(), reader.ReadVarInt(), reader.ReadVarInt());
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
				((PaintSprayTool)obj).UserCode_CmdRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32(reader.ReadVector3(), reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadFloat(), reader.ReadVarInt(), reader.ReadBool(), reader.ReadVarInt());
			}
		}

		protected void UserCode_RpcRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32(Vector3 position, Vector3 endPosition, Quaternion rotation, float pressure, int seed, bool clip, int priority)
		{
			if (!base.isOwned)
			{
				if (_colorSphere != null)
				{
					_colorSphere.HandleHitLine(preview: false, priority, pressure, seed, position, endPosition, rotation, clip);
				}
				if (_maskSphere != null)
				{
					_maskSphere.HandleHitLine(preview: false, priority, pressure, seed, position, endPosition, rotation, clip);
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
				((PaintSprayTool)obj).UserCode_RpcRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32(reader.ReadVector3(), reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadFloat(), reader.ReadVarInt(), reader.ReadBool(), reader.ReadVarInt());
			}
		}

		protected void UserCode_CmdEnableTool__Color(Color paintColor)
		{
			if (!(_paintCapacity <= 0f))
			{
				Network_syncedToolActive = true;
				Network_syncedPaintColor = paintColor;
				RpcEnableTool(paintColor);
			}
		}

		protected static void InvokeUserCode_CmdEnableTool__Color(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdEnableTool called on client.");
			}
			else
			{
				((PaintSprayTool)obj).UserCode_CmdEnableTool__Color(reader.ReadColor());
			}
		}

		protected void UserCode_RpcEnableTool__Color(Color paintColor)
		{
			if (!base.isOwned)
			{
				_paintColor = paintColor;
				SyncToCore();
				_core.UpdatePaintColor();
				_core.ConfigureSpheres();
				_core.EnableToolLocal();
				UpdateVisualModelColor();
				StartSprayAudio();
			}
		}

		protected static void InvokeUserCode_RpcEnableTool__Color(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcEnableTool called on server.");
			}
			else
			{
				((PaintSprayTool)obj).UserCode_RpcEnableTool__Color(reader.ReadColor());
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
				((PaintSprayTool)obj).UserCode_CmdDisableTool();
			}
		}

		protected void UserCode_RpcDisableTool()
		{
			if (!base.isOwned)
			{
				SyncToCore();
				_core.DisableToolLocal();
				StopSprayAudio();
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
				((PaintSprayTool)obj).UserCode_RpcDisableTool();
			}
		}

		protected void UserCode_CmdSetPaintColor__Color(Color color)
		{
			RpcSetPaintColor(color);
		}

		protected static void InvokeUserCode_CmdSetPaintColor__Color(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetPaintColor called on client.");
			}
			else
			{
				((PaintSprayTool)obj).UserCode_CmdSetPaintColor__Color(reader.ReadColor());
			}
		}

		protected void UserCode_RpcSetPaintColor__Color(Color color)
		{
			if (!base.isOwned)
			{
				_paintColor = color;
				SyncToCore();
				_core.UpdatePaintColor();
				_core.ConfigureSpheres();
				UpdateVisualModelColor();
				if (_panelActive)
				{
					_sprayInfoPanel?.SetColor(_paintColor);
				}
			}
		}

		protected static void InvokeUserCode_RpcSetPaintColor__Color(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcSetPaintColor called on server.");
			}
			else
			{
				((PaintSprayTool)obj).UserCode_RpcSetPaintColor__Color(reader.ReadColor());
			}
		}

		static PaintSprayTool()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(PaintSprayTool), "System.Void NomadDrive.Features.Restoration.PaintSprayTool::CmdRelayHitPoint(UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Int32)", InvokeUserCode_CmdRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PaintSprayTool), "System.Void NomadDrive.Features.Restoration.PaintSprayTool::CmdRelayHitLine(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Boolean,System.Int32)", InvokeUserCode_CmdRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PaintSprayTool), "System.Void NomadDrive.Features.Restoration.PaintSprayTool::CmdEnableTool(UnityEngine.Color)", InvokeUserCode_CmdEnableTool__Color, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PaintSprayTool), "System.Void NomadDrive.Features.Restoration.PaintSprayTool::CmdDisableTool()", InvokeUserCode_CmdDisableTool, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PaintSprayTool), "System.Void NomadDrive.Features.Restoration.PaintSprayTool::CmdSetPaintColor(UnityEngine.Color)", InvokeUserCode_CmdSetPaintColor__Color, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(PaintSprayTool), "System.Void NomadDrive.Features.Restoration.PaintSprayTool::RpcRelayHitPoint(UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Int32)", InvokeUserCode_RpcRelayHitPoint__Vector3__Quaternion__Single__Int32__Int32);
			RemoteProcedureCalls.RegisterRpc(typeof(PaintSprayTool), "System.Void NomadDrive.Features.Restoration.PaintSprayTool::RpcRelayHitLine(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Quaternion,System.Single,System.Int32,System.Boolean,System.Int32)", InvokeUserCode_RpcRelayHitLine__Vector3__Vector3__Quaternion__Single__Int32__Boolean__Int32);
			RemoteProcedureCalls.RegisterRpc(typeof(PaintSprayTool), "System.Void NomadDrive.Features.Restoration.PaintSprayTool::RpcEnableTool(UnityEngine.Color)", InvokeUserCode_RpcEnableTool__Color);
			RemoteProcedureCalls.RegisterRpc(typeof(PaintSprayTool), "System.Void NomadDrive.Features.Restoration.PaintSprayTool::RpcDisableTool()", InvokeUserCode_RpcDisableTool);
			RemoteProcedureCalls.RegisterRpc(typeof(PaintSprayTool), "System.Void NomadDrive.Features.Restoration.PaintSprayTool::RpcSetPaintColor(UnityEngine.Color)", InvokeUserCode_RpcSetPaintColor__Color);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_syncedToolActive);
				writer.WriteColor(_syncedPaintColor);
				writer.WriteFloat(_paintCapacity);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				writer.WriteBool(_syncedToolActive);
			}
			if ((syncVarDirtyBits & 0x400L) != 0L)
			{
				writer.WriteColor(_syncedPaintColor);
			}
			if ((syncVarDirtyBits & 0x800L) != 0L)
			{
				writer.WriteFloat(_paintCapacity);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _syncedToolActive, null, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _syncedPaintColor, null, reader.ReadColor());
				GeneratedSyncVarDeserialize(ref _paintCapacity, _Mirror_SyncVarHookDelegate__paintCapacity, reader.ReadFloat());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedToolActive, null, reader.ReadBool());
			}
			if ((num & 0x400L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _syncedPaintColor, null, reader.ReadColor());
			}
			if ((num & 0x800L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _paintCapacity, _Mirror_SyncVarHookDelegate__paintCapacity, reader.ReadFloat());
			}
		}
	}
}
