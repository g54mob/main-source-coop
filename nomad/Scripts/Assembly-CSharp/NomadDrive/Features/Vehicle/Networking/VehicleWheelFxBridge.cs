using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using NWH.VehiclePhysics2;
using NWH.VehiclePhysics2.GroundDetection;
using NWH.VehiclePhysics2.Powertrain;
using NWH.VehiclePhysics2.Sound;
using NWH.VehiclePhysics2.Sound.SoundComponents;
using NWH.WheelController3D;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.Vehicle.Fx;
using NomadDrive.Features.Vehicle.WheelSystem;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Networking
{
	[DefaultExecutionOrder(115)]
	[RequireComponent(typeof(NetworkIdentity))]
	[RequireComponent(typeof(VehicleController))]
	public sealed class VehicleWheelFxBridge : NetworkBehaviour
	{
		private sealed class RemoteWheelFx
		{
			public readonly WheelLocation Location;

			public WheelController Wc;

			public WheelComponent Comp;

			public WheelSkidmarkEmitter Skid;

			public ParticleSystem Particles;

			public RemoteWheelFx(WheelLocation location)
			{
				Location = location;
			}
		}

		[Header("Remote ground raycast")]
		[SerializeField]
		private LayerMask groundMask = -1;

		[SerializeField]
		private float raycastExtra = 0.6f;

		[SerializeField]
		private float raycastMargin = 0.1f;

		[SerializeField]
		[Range(0f, 1f)]
		private float minGroundUpDot = 0.34f;

		[Header("Effect toggles")]
		[SerializeField]
		private bool enableSkidmarks = true;

		[SerializeField]
		private bool enableParticles = true;

		[Header("Sync")]
		[SerializeField]
		private bool logFxSync;

		private const float SyncInterval = 1f / 15f;

		private const float HeartbeatInterval = 0.5f;

		private const float SlipEpsilon = 0.03f;

		private const float SpeedEpsilon = 0.15f;

		private const float GlobalSkidmarkIntensity = 0.6f;

		private const float MaxSkidmarkAlpha = 0.6f;

		private const float SmokeSlipGate = 0.35f;

		private const float LateralSlipCoeff = 0.5f;

		private const float LongitudinalSlipCoeff = 0.5f;

		private VehicleController _vehicleController;

		private WheelsManager _wheelsManager;

		private bool _ready;

		[SyncVar(hook = "OnFxStateHookChanged")]
		private WheelFxState _fxStateSync;

		private WheelFxState _lastSentFx;

		private WheelFxState _appliedFx;

		private bool _hasReceived;

		private float _syncTimer;

		private float _heartbeatTimer;

		private readonly RemoteWheelFx[] _wheels = new RemoteWheelFx[4]
		{
			new RemoteWheelFx(WheelLocation.FrontLeft),
			new RemoteWheelFx(WheelLocation.FrontRight),
			new RemoteWheelFx(WheelLocation.RearLeft),
			new RemoteWheelFx(WheelLocation.RearRight)
		};

		private bool _wheelsResolved;

		private Transform _skidmarkContainer;

		private readonly RaycastHit[] _hitBuffer = new RaycastHit[8];

		private AudioSource _skidSource;

		private AudioSource _rollSource;

		private AudioSource _bumpSource;

		private bool _audioResolved;

		private float _skidVol;

		private float _rollVol;

		private float _rollPitch;

		private readonly bool[] _wheelWasGrounded = new bool[4];

		private const float SkidVolLerp = 10f;

		private const float RollVolLerp = 20f;

		public Action<WheelFxState, WheelFxState> _Mirror_SyncVarHookDelegate__fxStateSync;

		public WheelFxState Network_fxStateSync
		{
			get
			{
				return _fxStateSync;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _fxStateSync, 1uL, _Mirror_SyncVarHookDelegate__fxStateSync);
			}
		}

		private void Awake()
		{
			_vehicleController = GetComponent<VehicleController>();
			_wheelsManager = GetComponent<WheelsManager>();
			if (_vehicleController != null)
			{
				_vehicleController.onVehicleInitialized.AddListener(OnVehicleInitialized);
			}
		}

		private void OnVehicleInitialized()
		{
			_ready = true;
		}

		private void OnDestroy()
		{
			if (_vehicleController != null)
			{
				_vehicleController.onVehicleInitialized.RemoveListener(OnVehicleInitialized);
			}
			TeardownRemoteRenderers();
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			TeardownRemoteRenderers();
		}

		public override void OnStartAuthority()
		{
			base.OnStartAuthority();
			_lastSentFx = default(WheelFxState);
			_syncTimer = 0f;
			_heartbeatTimer = 0f;
			ResetRemote();
		}

		public override void OnStopAuthority()
		{
			base.OnStopAuthority();
			ResetRemote();
		}

		private void FixedUpdate()
		{
			if (!_ready || !base.isOwned || _wheelsManager == null)
			{
				return;
			}
			_syncTimer += Time.fixedDeltaTime;
			if (!(_syncTimer < 1f / 15f))
			{
				_syncTimer = 0f;
				_heartbeatTimer += 1f / 15f;
				EnsureWheelsResolved();
				WheelFxState a = Sample();
				bool num = FxChanged(in a, in _lastSentFx);
				bool flag = _heartbeatTimer >= 0.5f;
				if (num || flag)
				{
					_lastSentFx = a;
					_heartbeatTimer = 0f;
					CmdSyncFxState(a);
				}
			}
		}

		private void LateUpdate()
		{
			if (!_ready || base.isOwned || _wheelsManager == null || !_hasReceived)
			{
				return;
			}
			EnsureWheelsResolved();
			float deltaTime = Time.deltaTime;
			for (int i = 0; i < _wheels.Length; i++)
			{
				RemoteWheelFx remoteWheelFx = _wheels[i];
				bool num = IsAttached(i);
				GetWheel(in _appliedFx, i, out var grounded, out var latSlip, out var lonSlip, out var surf, out var _);
				if (!num || !grounded || remoteWheelFx.Wc == null || remoteWheelFx.Wc.wheel == null)
				{
					remoteWheelFx.Skid?.Break();
					StopParticles(remoteWheelFx);
					continue;
				}
				SurfacePreset surfacePreset = ResolveSurface(surf);
				Transform rotatingContainer = remoteWheelFx.Wc.wheel.rotatingContainer;
				if (rotatingContainer == null)
				{
					remoteWheelFx.Skid?.Break();
					StopParticles(remoteWheelFx);
					continue;
				}
				if (!RaycastContact(rotatingContainer.position, remoteWheelFx.Wc.wheel.radius, out var point, out var normal))
				{
					remoteWheelFx.Skid?.Break();
					StopParticles(remoteWheelFx);
					continue;
				}
				if (enableSkidmarks && surfacePreset != null && surfacePreset.drawSkidmarks)
				{
					float intensity = SkidIntensity(latSlip, lonSlip, surfacePreset);
					EnsureSkidEmitter(remoteWheelFx);
					remoteWheelFx.Skid.AddPoint(point, normal, intensity, surfacePreset.skidmarkMaterial, deltaTime);
				}
				else
				{
					remoteWheelFx.Skid?.Break();
				}
				if (enableParticles && surfacePreset != null && surfacePreset.emitParticles)
				{
					DriveParticles(remoteWheelFx, surfacePreset, latSlip, lonSlip, point);
				}
				else
				{
					StopParticles(remoteWheelFx);
				}
			}
			DriveRemoteSurfaceAudio(deltaTime);
		}

		private WheelFxState Sample()
		{
			SampleWheel(0, out var grounded, out var latSlip, out var lonSlip, out var surf, out var load);
			SampleWheel(1, out var grounded2, out var latSlip2, out var lonSlip2, out var surf2, out var load2);
			SampleWheel(2, out var grounded3, out var latSlip3, out var lonSlip3, out var surf3, out var load3);
			SampleWheel(3, out var grounded4, out var latSlip4, out var lonSlip4, out var surf4, out var load4);
			return new WheelFxState
			{
				GroundedFL = grounded,
				GroundedFR = grounded2,
				GroundedRL = grounded3,
				GroundedRR = grounded4,
				LatSlipFL = latSlip,
				LatSlipFR = latSlip2,
				LatSlipRL = latSlip3,
				LatSlipRR = latSlip4,
				LonSlipFL = lonSlip,
				LonSlipFR = lonSlip2,
				LonSlipRL = lonSlip3,
				LonSlipRR = lonSlip4,
				SurfFL = surf,
				SurfFR = surf2,
				SurfRL = surf3,
				SurfRR = surf4,
				LoadFL = load,
				LoadFR = load2,
				LoadRL = load3,
				LoadRR = load4,
				Speed = _vehicleController.Speed,
				AngularVelMag = _vehicleController.AngularVelocityMagnitude
			};
		}

		private void SampleWheel(int i, out bool grounded, out float latSlip, out float lonSlip, out int surf, out float load)
		{
			grounded = false;
			latSlip = 0f;
			lonSlip = 0f;
			surf = -1;
			load = 0f;
			RemoteWheelFx remoteWheelFx = _wheels[i];
			if (!(remoteWheelFx.Wc == null) && IsAttached(i))
			{
				grounded = remoteWheelFx.Wc.IsGrounded;
				latSlip = remoteWheelFx.Wc.NormalizedLateralSlip;
				lonSlip = remoteWheelFx.Wc.NormalizedLongitudinalSlip;
				float maxLoad = remoteWheelFx.Wc.MaxLoad;
				load = ((maxLoad > 0.0001f) ? Mathf.Clamp01(remoteWheelFx.Wc.Load / maxLoad) : 0f);
				if (remoteWheelFx.Comp != null)
				{
					surf = Mathf.Clamp(remoteWheelFx.Comp.surfaceMapIndex, -1, 126);
				}
			}
		}

		private static bool FxChanged(in WheelFxState a, in WheelFxState b)
		{
			if (a.GroundedFL == b.GroundedFL && a.GroundedFR == b.GroundedFR && a.GroundedRL == b.GroundedRL && a.GroundedRR == b.GroundedRR && a.SurfFL == b.SurfFL && a.SurfFR == b.SurfFR && a.SurfRL == b.SurfRL && a.SurfRR == b.SurfRR && !(Mathf.Abs(a.LatSlipFL - b.LatSlipFL) > 0.03f) && !(Mathf.Abs(a.LatSlipFR - b.LatSlipFR) > 0.03f) && !(Mathf.Abs(a.LatSlipRL - b.LatSlipRL) > 0.03f) && !(Mathf.Abs(a.LatSlipRR - b.LatSlipRR) > 0.03f) && !(Mathf.Abs(a.LonSlipFL - b.LonSlipFL) > 0.03f) && !(Mathf.Abs(a.LonSlipFR - b.LonSlipFR) > 0.03f) && !(Mathf.Abs(a.LonSlipRL - b.LonSlipRL) > 0.03f) && !(Mathf.Abs(a.LonSlipRR - b.LonSlipRR) > 0.03f) && !(Mathf.Abs(a.Speed - b.Speed) > 0.15f))
			{
				return Mathf.Abs(a.AngularVelMag - b.AngularVelMag) > 0.15f;
			}
			return true;
		}

		[Command(channel = 1)]
		private void CmdSyncFxState(WheelFxState state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelFxState(writer, state);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Networking.VehicleWheelFxBridge::CmdSyncFxState(NomadDrive.Features.Vehicle.Networking.WheelFxState)", -152623689, writer, 1);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(channel = 1, includeOwner = false)]
		private void RpcReceiveFxState(WheelFxState state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelFxState(writer, state);
			SendRPCInternal("System.Void NomadDrive.Features.Vehicle.Networking.VehicleWheelFxBridge::RpcReceiveFxState(NomadDrive.Features.Vehicle.Networking.WheelFxState)", 266828552, writer, 1, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnFxStateHookChanged(WheelFxState _, WheelFxState newValue)
		{
			if (_ready)
			{
				ApplyFxState(newValue);
			}
		}

		private void ApplyFxState(WheelFxState state)
		{
			if (!base.isOwned)
			{
				bool num = !_hasReceived;
				_appliedFx = state;
				_hasReceived = true;
				if (num)
				{
					_wheelWasGrounded[0] = state.GroundedFL;
					_wheelWasGrounded[1] = state.GroundedFR;
					_wheelWasGrounded[2] = state.GroundedRL;
					_wheelWasGrounded[3] = state.GroundedRR;
				}
				if (num)
				{
					_ = logFxSync;
				}
			}
		}

		private bool RaycastContact(Vector3 wheelCenter, float radius, out Vector3 point, out Vector3 normal)
		{
			point = default(Vector3);
			normal = Vector3.up;
			int num = Physics.RaycastNonAlloc(wheelCenter + Vector3.up * (radius + raycastMargin), maxDistance: 2f * radius + raycastExtra + raycastMargin, direction: Vector3.down, results: _hitBuffer, layerMask: groundMask, queryTriggerInteraction: QueryTriggerInteraction.Ignore);
			if (num <= 0)
			{
				return false;
			}
			float num2 = 3.4028235E+38f;
			int num3 = -1;
			for (int i = 0; i < num; i++)
			{
				Collider collider = _hitBuffer[i].collider;
				if (!(collider == null) && !collider.transform.IsChildOf(base.transform) && !(Vector3.Dot(_hitBuffer[i].normal, Vector3.up) < minGroundUpDot) && _hitBuffer[i].distance < num2)
				{
					num2 = _hitBuffer[i].distance;
					num3 = i;
				}
			}
			if (num3 < 0)
			{
				return false;
			}
			point = _hitBuffer[num3].point;
			normal = _hitBuffer[num3].normal;
			return true;
		}

		private SurfacePreset ResolveSurface(int index)
		{
			GroundDetectionPreset groundDetectionPreset = ((_vehicleController != null && _vehicleController.groundDetection != null) ? _vehicleController.groundDetection.groundDetectionPreset : null);
			if (groundDetectionPreset == null)
			{
				return null;
			}
			if (index >= 0 && index < groundDetectionPreset.surfaceMaps.Count)
			{
				SurfacePreset surfacePreset = groundDetectionPreset.surfaceMaps[index].surfacePreset;
				if (surfacePreset != null)
				{
					return surfacePreset;
				}
			}
			return groundDetectionPreset.fallbackSurfacePreset;
		}

		private float SkidIntensity(float latSlip, float lonSlip, SurfacePreset surface)
		{
			float num = Mathf.Max(0f, latSlip - _vehicleController.lateralSlipThreshold);
			float num2 = Mathf.Max(0f, lonSlip - _vehicleController.longitudinalSlipThreshold);
			float num3 = (num + num2) * surface.slipFactor;
			return Mathf.Clamp(Mathf.Clamp01(surface.skidmarkBaseIntensity + num3) * 0.6f, 0f, 0.6f);
		}

		private void EnsureSkidEmitter(RemoteWheelFx w)
		{
			if (w.Skid != null)
			{
				return;
			}
			if (_skidmarkContainer == null)
			{
				GameObject gameObject = new GameObject("VehicleSkidmarks_" + base.name);
				_skidmarkContainer = gameObject.transform;
				FloatingOriginManager instance = FloatingOriginManager.Instance;
				if (instance != null && instance.WorldContentRoot != null)
				{
					_skidmarkContainer.SetParent(instance.WorldContentRoot, worldPositionStays: true);
				}
			}
			float markWidth = ((w.Wc != null && w.Wc.wheel != null) ? w.Wc.wheel.width : 0.25f);
			w.Skid = new WheelSkidmarkEmitter(_skidmarkContainer, markWidth);
		}

		private void DriveParticles(RemoteWheelFx w, SurfacePreset surface, float latSlip, float lonSlip, Vector3 contactPoint)
		{
			EnsureParticles(w);
			if (w.Particles == null)
			{
				return;
			}
			w.Particles.transform.position = contactPoint;
			ParticleSystem.EmissionModule emission = w.Particles.emission;
			ParticleSystem.MainModule main = w.Particles.main;
			float speed = _appliedFx.Speed;
			float a;
			if (surface.particleType == SurfacePreset.ParticleType.Smoke)
			{
				if ((!(latSlip > 0.35f) && !(lonSlip > 0.35f)) || (speed < 0.5f && _appliedFx.AngularVelMag < 0.5f))
				{
					emission.rateOverDistance = 0f;
					emission.rateOverTime = 0f;
					return;
				}
				float num = ((latSlip > 0.35f) ? (latSlip * 0.5f) : 0f);
				float num2 = ((lonSlip > 0.35f) ? (lonSlip * 0.5f) : 0f);
				float num3 = Mathf.Clamp01(num + num2) * surface.maxParticleEmissionRateOverDistance;
				a = Mathf.Clamp01(num3) * surface.particleMaxAlpha;
				float num4 = Mathf.Clamp01(speed * 0.33f);
				emission.rateOverDistance = num4 * num3;
				emission.rateOverTime = (1f - num4) * num3;
			}
			else
			{
				float num3 = Mathf.Clamp01(speed * 0.125f - 0.05f) * surface.maxParticleEmissionRateOverDistance;
				a = Mathf.Clamp01(num3 * 2f) * surface.particleMaxAlpha;
				emission.rateOverTime = 0f;
				emission.rateOverDistance = num3;
			}
			Color particleColor = surface.particleColor;
			particleColor.a = a;
			main.startColor = particleColor;
			main.startSize = surface.particleSize;
			float num5 = Mathf.Max(0.5f, speed);
			main.startLifetime = Mathf.Clamp(surface.particleLifeDistance / num5, 0.5f, surface.maxParticleLifetime);
		}

		private void EnsureParticles(RemoteWheelFx w)
		{
			if (w.Particles != null)
			{
				return;
			}
			GroundDetectionPreset groundDetectionPreset = ((_vehicleController != null && _vehicleController.groundDetection != null) ? _vehicleController.groundDetection.groundDetectionPreset : null);
			if (!(groundDetectionPreset == null) && !(groundDetectionPreset.particlePrefab == null) && !(w.Wc == null))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(groundDetectionPreset.particlePrefab, w.Wc.transform, worldPositionStays: false);
				w.Particles = gameObject.GetComponent<ParticleSystem>();
				if (w.Particles != null)
				{
					w.Particles.Play();
				}
			}
		}

		private void StopParticles(RemoteWheelFx w)
		{
			if (!(w.Particles == null))
			{
				ParticleSystem.EmissionModule emission = w.Particles.emission;
				emission.rateOverDistance = 0f;
				emission.rateOverTime = 0f;
			}
		}

		private void DriveRemoteSurfaceAudio(float dt)
		{
			SoundManager soundManager = _vehicleController.soundManager;
			if (soundManager == null)
			{
				return;
			}
			EnsureRemoteAudioSources();
			if (_skidSource == null || _rollSource == null)
			{
				return;
			}
			float num = Mathf.Abs(_appliedFx.Speed);
			SurfacePreset surfacePreset = ComputeDominantSurface();
			float num2 = Mathf.Min(1f, num * 0.33f);
			float num3 = Mathf.Clamp01(num * 0.03f);
			float num4 = Mathf.Max(0.0001f, _vehicleController.longitudinalSlipThreshold);
			float lateralSlipThreshold = _vehicleController.lateralSlipThreshold;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			for (int i = 0; i < _wheels.Length; i++)
			{
				if (!IsAttached(i))
				{
					continue;
				}
				GetWheel(in _appliedFx, i, out var grounded, out var latSlip, out var lonSlip, out var _, out var load);
				if (grounded && !_wheelWasGrounded[i])
				{
					PlayBump(load);
				}
				_wheelWasGrounded[i] = grounded;
				if (!grounded || surfacePreset == null)
				{
					continue;
				}
				if (surfacePreset.playSkidSounds && surfacePreset.skidSoundClip != null && (latSlip > lateralSlipThreshold || lonSlip > num4))
				{
					float num8 = Mathf.Clamp01(latSlip + lonSlip) * surfacePreset.skidSoundVolume * num2;
					if (num8 > num5)
					{
						num5 = num8;
					}
				}
				if (surfacePreset.playSurfaceSounds && surfacePreset.surfaceSoundClip != null)
				{
					float num9 = (surfacePreset.slipSensitiveSurfaceSound ? Mathf.Clamp01(latSlip / num4) : 1f);
					float num10 = Mathf.Clamp01(surfacePreset.surfaceSoundVolume * num9 * num3);
					if (num10 > num6)
					{
						num6 = num10;
					}
					float num11 = surfacePreset.surfaceSoundPitch * 0.5f + num3;
					if (num11 > num7)
					{
						num7 = num11;
					}
				}
			}
			ApplyLoop(_skidSource, (surfacePreset != null) ? surfacePreset.skidSoundClip : null, ref _skidVol, num5, dt * 10f, soundManager.masterVolume);
			_rollPitch = Mathf.Lerp(_rollPitch, num7, Mathf.Clamp01(dt * 20f));
			if (_rollPitch > 0.01f)
			{
				_rollSource.pitch = _rollPitch;
			}
			ApplyLoop(_rollSource, (surfacePreset != null) ? surfacePreset.surfaceSoundClip : null, ref _rollVol, num6, dt * 20f, soundManager.masterVolume);
		}

		private void PlayBump(float loadRatio)
		{
			if (_bumpSource == null)
			{
				return;
			}
			SoundManager soundManager = _vehicleController.soundManager;
			SuspensionBumpComponent suspensionBumpComponent = soundManager?.suspensionBumpComponent;
			if (suspensionBumpComponent == null || suspensionBumpComponent.clips == null || suspensionBumpComponent.clips.Count == 0)
			{
				return;
			}
			AudioClip audioClip = suspensionBumpComponent.clips[UnityEngine.Random.Range(0, suspensionBumpComponent.clips.Count)];
			if (!(audioClip == null))
			{
				float num = suspensionBumpComponent.baseVolume * Mathf.Clamp01(loadRatio) * soundManager.masterVolume;
				if (!(num <= 0.0001f))
				{
					_bumpSource.pitch = UnityEngine.Random.Range(0.7f, 1.3f);
					_bumpSource.PlayOneShot(audioClip, num);
				}
			}
		}

		private SurfacePreset ComputeDominantSurface()
		{
			SurfacePreset result = null;
			int num = 0;
			for (int i = 0; i < _wheels.Length; i++)
			{
				if (!IsAttached(i))
				{
					continue;
				}
				GetWheel(in _appliedFx, i, out var grounded, out var latSlip, out var lonSlip, out var surf, out var load);
				if (!grounded)
				{
					continue;
				}
				SurfacePreset surfacePreset = ResolveSurface(surf);
				if (surfacePreset == null)
				{
					continue;
				}
				int num2 = 0;
				for (int j = 0; j < _wheels.Length; j++)
				{
					if (IsAttached(j))
					{
						GetWheel(in _appliedFx, j, out var grounded2, out load, out lonSlip, out var surf2, out latSlip);
						if (grounded2 && (object)ResolveSurface(surf2) == surfacePreset)
						{
							num2++;
						}
					}
				}
				if (num2 > num)
				{
					num = num2;
					result = surfacePreset;
				}
			}
			return result;
		}

		private void EnsureRemoteAudioSources()
		{
			if (!_audioResolved && _vehicleController.soundManager != null)
			{
				_skidSource = CreateRemoteSource("RemoteTireSkid", loop: true);
				_rollSource = CreateRemoteSource("RemoteTireRoll", loop: true);
				_bumpSource = CreateRemoteSource("RemoteSuspensionBump", loop: false);
				if (_bumpSource != null)
				{
					_bumpSource.volume = 1f;
				}
				_audioResolved = _skidSource != null && _rollSource != null;
			}
		}

		private AudioSource CreateRemoteSource(string label, bool loop)
		{
			SoundManager soundManager = _vehicleController.soundManager;
			if (soundManager == null)
			{
				return null;
			}
			Transform parent = ((soundManager.otherSourceGO != null) ? soundManager.otherSourceGO.transform : base.transform);
			GameObject obj = new GameObject(label + "_" + base.name);
			obj.transform.SetParent(parent, worldPositionStays: false);
			AudioSource audioSource = obj.AddComponent<AudioSource>();
			audioSource.outputAudioMixerGroup = soundManager.otherMixerGroup;
			audioSource.spatialBlend = soundManager.spatialBlend;
			audioSource.dopplerLevel = soundManager.dopplerLevel;
			audioSource.loop = loop;
			audioSource.playOnAwake = false;
			audioSource.volume = 0f;
			return audioSource;
		}

		private static void ApplyLoop(AudioSource src, AudioClip clip, ref float smoothedVol, float targetVol, float lerpT, float master)
		{
			if (!(src == null))
			{
				if (clip != null && src.clip != clip)
				{
					src.clip = clip;
				}
				smoothedVol = Mathf.Lerp(smoothedVol, targetVol, Mathf.Clamp01(lerpT));
				src.volume = smoothedVol * master;
				bool flag = src.clip != null && smoothedVol > 0.001f;
				if (flag && !src.isPlaying)
				{
					src.Play();
				}
				else if (!flag && src.isPlaying)
				{
					src.Stop();
				}
			}
		}

		private void EnsureWheelsResolved()
		{
			if (_wheelsResolved || _wheelsManager == null)
			{
				return;
			}
			bool wheelsResolved = false;
			for (int i = 0; i < _wheels.Length; i++)
			{
				RemoteWheelFx remoteWheelFx = _wheels[i];
				if (_wheelsManager.HasWheelLocation(remoteWheelFx.Location))
				{
					remoteWheelFx.Wc = _wheelsManager.GetWheelController(remoteWheelFx.Location);
					remoteWheelFx.Comp = FindWheelComponent(remoteWheelFx.Wc);
					if (remoteWheelFx.Wc != null)
					{
						wheelsResolved = true;
					}
				}
			}
			_wheelsResolved = wheelsResolved;
		}

		private WheelComponent FindWheelComponent(WheelController wc)
		{
			if (wc == null || _vehicleController == null || _vehicleController.powertrain == null)
			{
				return null;
			}
			List<WheelComponent> wheels = _vehicleController.powertrain.wheels;
			for (int i = 0; i < wheels.Count; i++)
			{
				if ((object)wheels[i].wheelUAPI == wc)
				{
					return wheels[i];
				}
			}
			return null;
		}

		private bool IsAttached(int index)
		{
			return index switch
			{
				0 => _wheelsManager.IsFrontLeftWheelAttached, 
				1 => _wheelsManager.IsFrontRightWheelAttached, 
				2 => _wheelsManager.IsRearLeftWheelAttached, 
				_ => _wheelsManager.IsRearRightWheelAttached, 
			};
		}

		private static void GetWheel(in WheelFxState s, int index, out bool grounded, out float latSlip, out float lonSlip, out int surf, out float load)
		{
			switch (index)
			{
			case 0:
				grounded = s.GroundedFL;
				latSlip = s.LatSlipFL;
				lonSlip = s.LonSlipFL;
				surf = s.SurfFL;
				load = s.LoadFL;
				break;
			case 1:
				grounded = s.GroundedFR;
				latSlip = s.LatSlipFR;
				lonSlip = s.LonSlipFR;
				surf = s.SurfFR;
				load = s.LoadFR;
				break;
			case 2:
				grounded = s.GroundedRL;
				latSlip = s.LatSlipRL;
				lonSlip = s.LonSlipRL;
				surf = s.SurfRL;
				load = s.LoadRL;
				break;
			default:
				grounded = s.GroundedRR;
				latSlip = s.LatSlipRR;
				lonSlip = s.LonSlipRR;
				surf = s.SurfRR;
				load = s.LoadRR;
				break;
			}
		}

		private void ResetRemote()
		{
			_hasReceived = false;
			_appliedFx = default(WheelFxState);
			_skidVol = 0f;
			_rollVol = 0f;
			_rollPitch = 0f;
			if (_skidSource != null && _skidSource.isPlaying)
			{
				_skidSource.Stop();
			}
			if (_rollSource != null && _rollSource.isPlaying)
			{
				_rollSource.Stop();
			}
			for (int i = 0; i < _wheelWasGrounded.Length; i++)
			{
				_wheelWasGrounded[i] = false;
			}
			RemoteWheelFx[] wheels = _wheels;
			foreach (RemoteWheelFx remoteWheelFx in wheels)
			{
				remoteWheelFx.Skid?.Break();
				StopParticles(remoteWheelFx);
			}
		}

		private void TeardownRemoteRenderers()
		{
			RemoteWheelFx[] wheels = _wheels;
			foreach (RemoteWheelFx remoteWheelFx in wheels)
			{
				remoteWheelFx.Skid?.Cleanup();
				remoteWheelFx.Skid = null;
				if (remoteWheelFx.Particles != null)
				{
					UnityEngine.Object.Destroy(remoteWheelFx.Particles.gameObject);
					remoteWheelFx.Particles = null;
				}
			}
			if (_skidmarkContainer != null)
			{
				UnityEngine.Object.Destroy(_skidmarkContainer.gameObject);
				_skidmarkContainer = null;
			}
			DestroyAudioSource(ref _skidSource);
			DestroyAudioSource(ref _rollSource);
			DestroyAudioSource(ref _bumpSource);
			_audioResolved = false;
		}

		private static void DestroyAudioSource(ref AudioSource src)
		{
			if (src != null)
			{
				UnityEngine.Object.Destroy(src.gameObject);
				src = null;
			}
		}

		public VehicleWheelFxBridge()
		{
			_Mirror_SyncVarHookDelegate__fxStateSync = OnFxStateHookChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSyncFxState__WheelFxState(WheelFxState state)
		{
			Network_fxStateSync = state;
			ApplyFxState(state);
			RpcReceiveFxState(state);
		}

		protected static void InvokeUserCode_CmdSyncFxState__WheelFxState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSyncFxState called on client.");
			}
			else
			{
				((VehicleWheelFxBridge)obj).UserCode_CmdSyncFxState__WheelFxState(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelFxState(reader));
			}
		}

		protected void UserCode_RpcReceiveFxState__WheelFxState(WheelFxState state)
		{
			if (!base.isServer)
			{
				ApplyFxState(state);
			}
		}

		protected static void InvokeUserCode_RpcReceiveFxState__WheelFxState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcReceiveFxState called on server.");
			}
			else
			{
				((VehicleWheelFxBridge)obj).UserCode_RpcReceiveFxState__WheelFxState(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelFxState(reader));
			}
		}

		static VehicleWheelFxBridge()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(VehicleWheelFxBridge), "System.Void NomadDrive.Features.Vehicle.Networking.VehicleWheelFxBridge::CmdSyncFxState(NomadDrive.Features.Vehicle.Networking.WheelFxState)", InvokeUserCode_CmdSyncFxState__WheelFxState, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(VehicleWheelFxBridge), "System.Void NomadDrive.Features.Vehicle.Networking.VehicleWheelFxBridge::RpcReceiveFxState(NomadDrive.Features.Vehicle.Networking.WheelFxState)", InvokeUserCode_RpcReceiveFxState__WheelFxState);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelFxState(writer, _fxStateSync);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelFxState(writer, _fxStateSync);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _fxStateSync, _Mirror_SyncVarHookDelegate__fxStateSync, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelFxState(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _fxStateSync, _Mirror_SyncVarHookDelegate__fxStateSync, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelFxState(reader));
			}
		}
	}
}
