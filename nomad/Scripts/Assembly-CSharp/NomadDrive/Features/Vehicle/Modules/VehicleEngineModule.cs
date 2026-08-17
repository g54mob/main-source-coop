using Ami.BroAudio;
using Cysharp.Threading.Tasks;
using EvilCore.Audio;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Particles;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Vehicle.Parts.Engine;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleEngineModule : VehicleModule
	{
		private const string ENGINE = "Engine";

		[SerializeField]
		private EngineSlot _engineSlotRef;

		[SerializeField]
		public bool IsEngineInstalled;

		[SerializeField]
		private Engine _installedEngine;

		[SerializeField]
		public UnityEvent onEngineConditionZero;

		[SerializeField]
		private float _engineMaxPower;

		[SerializeField]
		private float _engineFuelEfficiency;

		[SerializeField]
		private float _engineConditionConsumptionRate;

		[SerializeField]
		private float _engineIdleConditionConsumptionRate;

		[SerializeField]
		private float _engineRunningConditionConsumptionRate;

		[SerializeField]
		private float _engineConsumptionSpeed;

		[SerializeField]
		private SoundID suspensionImpactSound;

		private bool _engineOverheatParticleCached;

		private bool _steamEffectPlaying;

		private ParticleHandle _steamHandle;

		private string _engineOverheatParticleKey;

		private ParticleSystem[] _cachedSteamSystems;

		private ParticleSystemRenderer[] _cachedSteamRenderers;

		private MaterialPropertyBlock _steamPropertyBlock;

		private static readonly int SteamColorId = Shader.PropertyToID("_Color");

		private static readonly int SteamBaseColorId = Shader.PropertyToID("_BaseColor");

		[Inject]
		private IAudioManager _audioManager;

		[Inject]
		private IParticlesManager _particlesManager;

		private VehicleRadiatorModule _radiatorModule;

		private VehicleIgnitionModule _ignitionModule;

		private ConditionComponent _engineCondition;

		private EngineHeatComponent _engineHeat;

		private bool _lastOverheated;

		private const float SteamStartRatio = 0.7f;

		private float CurrentHeatDebug
		{
			get
			{
				if (!(_installedEngine != null) || !(_installedEngine.HeatComponent != null))
				{
					return 0f;
				}
				return _installedEngine.HeatComponent.HeatLevel;
			}
		}

		private bool IsOverheatedDebug
		{
			get
			{
				if (_installedEngine != null && _installedEngine.HeatComponent != null)
				{
					return _installedEngine.HeatComponent.IsOverheated;
				}
				return false;
			}
		}

		public EngineSlot EngineSlotRef => _engineSlotRef;

		public Engine InstalledEngine => _installedEngine;

		public bool IsEngineBroken
		{
			get
			{
				if (_engineCondition != null)
				{
					return _engineCondition.IsBroken;
				}
				return false;
			}
		}

		public bool IsEngineUseful
		{
			get
			{
				if (IsEngineInstalled)
				{
					return !IsEngineBroken;
				}
				return false;
			}
		}

		public float CurrentHeatLevel => (_installedEngine?.HeatComponent?.HeatLevel).GetValueOrDefault();

		public bool IsOverheated
		{
			get
			{
				if (_installedEngine != null && _installedEngine.HeatComponent != null)
				{
					return _installedEngine.HeatComponent.IsOverheated;
				}
				return false;
			}
		}

		private void Start()
		{
			_radiatorModule = base.VehicleManager.GetModule<VehicleRadiatorModule>();
			_ignitionModule = base.VehicleManager.GetModule<VehicleIgnitionModule>();
		}

		protected override void SubscribeEvents()
		{
			_engineSlotRef.onEngineInstalled.AddListener(OnEngineInstalled);
			_engineSlotRef.onEngineRemoved.AddListener(OnEngineRemoved);
			onEngineConditionZero.AddListener(OnEngineConditionZero);
			base.EventBus.OnUnderHoodPartsActivate += OnUnderHoodActivate;
			base.EventBus.OnUnderHoodPartsDeactivate += OnUnderHoodDeactivate;
			base.EventBus.OnEngineStarted += OnEngineStartedConsumeCondition;
		}

		protected override void UnsubscribeEvents()
		{
			_engineSlotRef.onEngineInstalled.RemoveListener(OnEngineInstalled);
			_engineSlotRef.onEngineRemoved.RemoveListener(OnEngineRemoved);
			onEngineConditionZero.RemoveListener(OnEngineConditionZero);
			base.EventBus.OnUnderHoodPartsActivate -= OnUnderHoodActivate;
			base.EventBus.OnUnderHoodPartsDeactivate -= OnUnderHoodDeactivate;
			base.EventBus.OnEngineStarted -= OnEngineStartedConsumeCondition;
		}

		private void SetEngineProperties(Engine engine)
		{
			if (engine == null)
			{
				_installedEngine = null;
				_engineCondition = null;
				IsEngineInstalled = false;
				_engineMaxPower = 0f;
				_engineFuelEfficiency = 0f;
				_engineConditionConsumptionRate = 0f;
				_engineIdleConditionConsumptionRate = 0f;
				_engineRunningConditionConsumptionRate = 0f;
				_engineConsumptionSpeed = 0f;
				base.VehicleManager.VehicleController.powertrain.engine.maxPower = 0f;
				base.VehicleManager.FuelModuleWrapper.module.efficiency = 0f;
				base.VehicleManager.FuelModuleWrapper.module.consumptionMultiplier = 1f;
			}
			else if (engine.engineConfig == null)
			{
				EvilLogger.LogError("Engine config is null", "SetEngineProperties", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Modules\\VehicleEngineModule.cs", 109);
			}
			else
			{
				_installedEngine = engine;
				_engineCondition = engine.GetComponent<ConditionComponent>();
				IsEngineInstalled = true;
				_engineMaxPower = engine.engineConfig.maxPower;
				_engineFuelEfficiency = engine.engineConfig.fuelEfficiency;
				_engineConditionConsumptionRate = engine.engineConfig.conditionConsumptionRate;
				_engineIdleConditionConsumptionRate = engine.engineConfig.idleConditionConsumptionRate;
				_engineRunningConditionConsumptionRate = engine.engineConfig.runningConditionConsumptionRate;
				_engineConsumptionSpeed = engine.engineConfig.engineConsumptionSpeed;
				base.VehicleManager.VehicleController.powertrain.engine.maxPower = engine.engineConfig.maxPower;
				base.VehicleManager.FuelModuleWrapper.module.efficiency = engine.engineConfig.fuelEfficiency;
				base.VehicleManager.FuelModuleWrapper.module.consumptionMultiplier = engine.engineConfig.fuelConsumptionMultiplier;
			}
		}

		private void OnEngineInstalled(Engine engine)
		{
			SetEngineProperties(engine);
			CacheSteamEffect();
			_engineHeat = engine.HeatComponent;
			if (_engineHeat != null)
			{
				_engineHeat.OnOverheatedChanged.AddListener(OnEngineHeatOverheatedChanged);
				if (base.VehicleManager.NetworkSync != null && base.VehicleManager.NetworkSync.isServer)
				{
					_engineHeat.ServerSetControlled(controlled: true);
					_lastOverheated = _engineHeat.IsOverheated;
				}
			}
			base.EventBus.FireEngineUsefulChanged(IsEngineUseful);
			base.VehicleManager.VehiclePhysiscsManager.ImpactForceAtPosition(Vector3.down, engine.transform.position, 150f);
			if (base.VehicleManager.WheelsManager.IsFrontLeftWheelAttached && base.VehicleManager.WheelsManager.IsFrontRightWheelAttached && suspensionImpactSound.IsValid())
			{
				_audioManager?.PlayOneShot(suspensionImpactSound, engine.transform.position);
			}
		}

		private void OnEngineRemoved()
		{
			base.VehicleManager.VehiclePhysiscsManager.ImpactForceAtPosition(Vector3.up, _installedEngine.transform.position, 50f);
			if (base.VehicleManager.WheelsManager.IsFrontLeftWheelAttached && base.VehicleManager.WheelsManager.IsFrontRightWheelAttached && suspensionImpactSound.IsValid())
			{
				_audioManager?.PlayOneShot(suspensionImpactSound, _installedEngine.transform.position);
			}
			if (_engineHeat != null)
			{
				_engineHeat.OnOverheatedChanged.RemoveListener(OnEngineHeatOverheatedChanged);
				if (base.VehicleManager.NetworkSync != null && base.VehicleManager.NetworkSync.isServer)
				{
					_engineHeat.ServerSetControlled(controlled: false);
				}
				_engineHeat = null;
			}
			_lastOverheated = false;
			StopSteamEffect();
			_engineOverheatParticleCached = false;
			_engineOverheatParticleKey = null;
			SetEngineProperties(null);
			StopEngine();
			base.EventBus.FireEngineUsefulChanged(IsEngineUseful);
		}

		public void StartEngine()
		{
			base.VehicleManager.VehicleController.powertrain.engine.StartEngine();
		}

		public void StopEngine()
		{
			base.VehicleManager.VehicleController.powertrain.engine.StopEngine();
		}

		private async void OnEngineStartedConsumeCondition()
		{
			if (!(base.VehicleManager.NetworkSync == null) && base.VehicleManager.NetworkSync.isServer)
			{
				await ConsumeEngineCondition();
			}
		}

		private async UniTask ConsumeEngineCondition()
		{
			if (!IsEngineInstalled || _engineCondition == null || _engineCondition.Condition <= 0f)
			{
				return;
			}
			float conditionConsumptionRate = _engineConditionConsumptionRate;
			await UniTask.NextFrame();
			VehicleIgnitionModule ignitionModule = base.VehicleManager.GetModule<VehicleIgnitionModule>();
			while (ignitionModule != null && ignitionModule.IsIgnited)
			{
				float num = ((base.VehicleManager.VehicleController.powertrain.engine.generatedPower > 0f) ? _engineRunningConditionConsumptionRate : _engineIdleConditionConsumptionRate);
				_engineCondition.ServerSetCondition(_engineCondition.Condition - num * conditionConsumptionRate * _engineConsumptionSpeed * Time.deltaTime);
				if (_engineCondition.Condition <= 0f)
				{
					_engineCondition.ServerSetCondition(0f);
					onEngineConditionZero.Invoke();
					break;
				}
				await UniTask.NextFrame();
			}
			await UniTask.Yield();
		}

		private void OnEngineConditionZero()
		{
			StopEngine();
		}

		private void OnUnderHoodActivate()
		{
			_engineSlotRef.Activate();
		}

		private void OnUnderHoodDeactivate()
		{
			_engineSlotRef.Deactivate();
		}

		public override void OnFrontSeatsTaken()
		{
			_installedEngine?.IgnoreHovering();
		}

		public override void OnFrontSeatsVacated()
		{
			_installedEngine?.UnignoreHovering();
		}

		private void Update()
		{
			if (!(base.VehicleManager.NetworkSync == null) && base.VehicleManager.NetworkSync.isServer && IsEngineInstalled && !(_engineHeat == null))
			{
				_engineHeat.ServerTick(GatherHeatInputs());
				bool isOverheated = _engineHeat.IsOverheated;
				if (isOverheated && !_lastOverheated)
				{
					StopEngine();
				}
				_lastOverheated = isOverheated;
			}
		}

		private EngineHeatInputs GatherHeatInputs()
		{
			bool num = _ignitionModule != null && _ignitionModule.IsIgnited;
			float loadFactor = 0f;
			if (num)
			{
				float generatedPower = base.VehicleManager.VehicleController.powertrain.engine.generatedPower;
				loadFactor = ((_engineMaxPower > 0f) ? Mathf.Clamp01(generatedPower / _engineMaxPower) : 0f);
			}
			bool flag = _radiatorModule != null && _radiatorModule.IsRadiatorInstalled;
			float coolantRatio = (flag ? _radiatorModule.EffectiveCoolantRatio : 0f);
			return new EngineHeatInputs(num, loadFactor, flag, coolantRatio);
		}

		private void OnEngineHeatOverheatedChanged(bool overheated)
		{
			if (overheated)
			{
				base.EventBus.FireEngineOverheated();
			}
			else
			{
				base.EventBus.FireEngineCooledDown();
			}
		}

		private void CacheSteamEffect()
		{
			_engineOverheatParticleKey = _installedEngine?.EngineOverheatParticleKey;
			_engineOverheatParticleCached = !string.IsNullOrEmpty(_engineOverheatParticleKey);
			_steamEffectPlaying = false;
			if (_engineOverheatParticleCached)
			{
				_particlesManager?.Warmup(_engineOverheatParticleKey, 1);
			}
		}

		private void LateUpdate()
		{
			if (_particlesManager != null && _engineOverheatParticleCached && IsEngineInstalled)
			{
				UpdateSteamEffect();
			}
		}

		private void UpdateSteamEffect()
		{
			EngineHeatComponent engineHeatComponent = ((_installedEngine != null) ? _installedEngine.HeatComponent : null);
			float value = ((engineHeatComponent != null) ? engineHeatComponent.HeatRatio : 0f);
			float num = Mathf.InverseLerp(0.7f, 1f, value);
			if (num <= 0f)
			{
				if (_steamEffectPlaying)
				{
					StopSteamEffect();
				}
				return;
			}
			if (!_steamEffectPlaying)
			{
				StartSteamEffect();
			}
			ApplySteamVisuals(num);
		}

		private void StartSteamEffect()
		{
			_steamHandle = _particlesManager.PlayAttached(_engineOverheatParticleKey, _installedEngine.transform);
			if (_steamHandle.IsValid)
			{
				_steamEffectPlaying = true;
				CacheSteamRenderers();
			}
		}

		private void StopSteamEffect()
		{
			if (_steamHandle.IsValid)
			{
				_particlesManager.Stop(_steamHandle);
			}
			_steamHandle = ParticleHandle.Invalid;
			_steamEffectPlaying = false;
			_cachedSteamSystems = null;
			_cachedSteamRenderers = null;
		}

		private void CacheSteamRenderers()
		{
			ParticleSystem particleSystem = _steamHandle.ParticleSystem;
			if (particleSystem == null)
			{
				_cachedSteamSystems = null;
				_cachedSteamRenderers = null;
				return;
			}
			_cachedSteamSystems = particleSystem.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
			_cachedSteamRenderers = new ParticleSystemRenderer[_cachedSteamSystems.Length];
			for (int i = 0; i < _cachedSteamSystems.Length; i++)
			{
				_cachedSteamRenderers[i] = _cachedSteamSystems[i].GetComponent<ParticleSystemRenderer>();
			}
			if (_steamPropertyBlock == null)
			{
				_steamPropertyBlock = new MaterialPropertyBlock();
			}
		}

		private void ApplySteamVisuals(float steamRatio)
		{
			if (_cachedSteamSystems == null)
			{
				return;
			}
			float num = Mathf.Lerp(0.5f, 1.5f, steamRatio);
			_steamHandle.Transform.localScale = Vector3.one * num;
			Color color = Color.Lerp(new Color(0.9f, 0.9f, 0.9f, 0.4f), new Color(0.15f, 0.15f, 0.15f, 1f), steamRatio);
			float rateOverTimeMultiplier = Mathf.Lerp(5f, 30f, steamRatio);
			for (int i = 0; i < _cachedSteamSystems.Length; i++)
			{
				ParticleSystem obj = _cachedSteamSystems[i];
				ParticleSystem.MainModule main = obj.main;
				main.startColor = color;
				ParticleSystem.EmissionModule emission = obj.emission;
				emission.rateOverTimeMultiplier = rateOverTimeMultiplier;
				ParticleSystemRenderer particleSystemRenderer = _cachedSteamRenderers[i];
				if (!(particleSystemRenderer == null))
				{
					particleSystemRenderer.GetPropertyBlock(_steamPropertyBlock);
					_steamPropertyBlock.SetColor(SteamColorId, color);
					_steamPropertyBlock.SetColor(SteamBaseColorId, color);
					particleSystemRenderer.SetPropertyBlock(_steamPropertyBlock);
				}
			}
		}
	}
}
