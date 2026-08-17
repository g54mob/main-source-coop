using Cysharp.Threading.Tasks;
using NWH.VehiclePhysics2;
using NomadDrive.Features.Vehicle.Enums;
using NomadDrive.Features.Vehicle.Modules;
using NomadDrive.Managers.GameTime;
using ShadedTechnology.WindshieldRainAsset;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace NomadDrive.Features.Vehicle.Interactables
{
	public class WindshieldsModule : VehicleModule
	{
		[SerializeField]
		private Animator leftWiperAnimator;

		[SerializeField]
		private Animator rightWiperAnimator;

		[SerializeField]
		private string sweepStateName = "Sweep";

		[SerializeField]
		private float slowSpeedMultiplier = 1f;

		[SerializeField]
		private float fastSpeedMultiplier = 1.5f;

		[FormerlySerializedAs("WiperButton")]
		[SerializeField]
		public WindshieldButton windshieldButton;

		[SerializeField]
		private WindshieldRain windshieldRain;

		[SerializeField]
		private SimpleRainManager simpleRainManager;

		[SerializeField]
		private float speedReferenceMs = 30f;

		[SerializeField]
		private AnimationCurve intensityCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private float baseSpawnRate = 4000f;

		[SerializeField]
		private float baseSpawnAmount = 0.8f;

		[SerializeField]
		private float minDropletRadius = 0.5f;

		[SerializeField]
		private float baseMaxDropletRadius = 0.2f;

		[SerializeField]
		private float maxDropletRadiusAtFullIntensity = 0.5f;

		[SerializeField]
		private float minDropsInfluence = 0.3f;

		[SerializeField]
		private float maxDropsInfluence = 1f;

		[SerializeField]
		private float minTurbulanceImpact = 1f;

		[SerializeField]
		private float maxTurbulanceImpact = 2.5f;

		[SerializeField]
		private float minTurbulanceSpeedMul = 1f;

		[SerializeField]
		private float maxTurbulanceSpeedMul = 3f;

		[SerializeField]
		private float idleMovementY = 0.35f;

		[SerializeField]
		private float maxMovementY = 1.5f;

		[SerializeField]
		private float rainingStreakDecayRate = 0.5f;

		[SerializeField]
		private float dryStreakDecayRate = 8f;

		[SerializeField]
		private float bodyRainAddVelocityMin = 0.5f;

		[SerializeField]
		private float bodyRainAddVelocityMax = 1.2f;

		[SerializeField]
		private float bodyRainAccelScaleMin = 1f;

		[SerializeField]
		private float bodyRainAccelScaleMax = 2f;

		[SerializeField]
		private float minMovingSpeedMs = 0.5f;

		[SerializeField]
		private float bodyMaxRainAmount = 1f;

		[SerializeField]
		private string bodyRainAmountProperty = "_RainAmount";

		[SerializeField]
		private float wiperActiveDecayMultiplier = 2f;

		[Inject]
		private ITimeManager timeManager;

		private static readonly int SweepHash = Animator.StringToHash("Sweep");

		[SerializeField]
		private bool bladesSweeping;

		[SerializeField]
		private float currentSpeedMultiplier = 1f;

		private bool animatorSeeded;

		private bool wasRaining = true;

		private int bodyRainAmountPropertyId;

		private int sweepStateHash;

		protected override void Awake()
		{
			base.Awake();
			bodyRainAmountPropertyId = Shader.PropertyToID(bodyRainAmountProperty);
			sweepStateHash = Animator.StringToHash(sweepStateName);
			if (windshieldRain != null)
			{
				windshieldRain.m_SpawnRate = 0f;
				windshieldRain.m_SpawnAmount = 0f;
				windshieldRain.m_RainStreakDecayRate = dryStreakDecayRate;
				windshieldRain.Movement = Vector2.zero;
				if (windshieldRain.m_WipersEnabled && windshieldRain.m_WipersScript == null)
				{
					windshieldRain.m_WipersEnabled = false;
				}
				ClearWindshieldRainMaterialHeightmap();
			}
			if (!(simpleRainManager != null) || simpleRainManager.m_rainMaterials == null)
			{
				return;
			}
			Material[] rainMaterials = simpleRainManager.m_rainMaterials;
			foreach (Material material in rainMaterials)
			{
				if (!(material == null) && material.HasProperty(bodyRainAmountPropertyId))
				{
					material.SetFloat(bodyRainAmountPropertyId, 0f);
				}
			}
		}

		private void ClearWindshieldRainMaterialHeightmap()
		{
			if (!(windshieldRain == null) && !(windshieldRain.m_RainMaterial == null) && windshieldRain.m_RainMaterial.HasProperty("_HeightMap"))
			{
				windshieldRain.m_RainMaterial.SetTexture("_HeightMap", Texture2D.blackTexture);
			}
		}

		private async void Start()
		{
			VehicleBatteryModule module = base.VehicleManager.GetModule<VehicleBatteryModule>();
			if (module != null)
			{
				module.RegisterConsumer(IsWiperSlowActive, GetWiperSlowMultiplier);
				module.RegisterConsumer(IsWiperFastActive, GetWiperFastMultiplier);
			}
			await UniTask.NextFrame(base.destroyCancellationToken);
			if (!(this == null) && !(windshieldRain == null))
			{
				windshieldRain.ResetRain();
				windshieldRain.UpdateShaderValues();
			}
		}

		protected override void SubscribeEvents()
		{
			if (!(windshieldButton == null))
			{
				windshieldButton.OnWiperButtonOff.AddListener(OnWiperDeactivated);
				windshieldButton.OnWiperSlowButtonOn.AddListener(OnWiperSlowActivated);
				windshieldButton.OnWiperFastButtonOn.AddListener(OnWiperFastActivated);
				base.EventBus.OnBatteryDepleted += OnBatteryDepleted;
				base.EventBus.OnBatteryInstalled += OnBatteryInstalled;
			}
		}

		protected override void UnsubscribeEvents()
		{
			if (windshieldButton != null)
			{
				windshieldButton.OnWiperButtonOff.RemoveListener(OnWiperDeactivated);
				windshieldButton.OnWiperSlowButtonOn.RemoveListener(OnWiperSlowActivated);
				windshieldButton.OnWiperFastButtonOn.RemoveListener(OnWiperFastActivated);
			}
			base.EventBus.OnBatteryDepleted -= OnBatteryDepleted;
			base.EventBus.OnBatteryInstalled -= OnBatteryInstalled;
			VehicleBatteryModule module = base.VehicleManager.GetModule<VehicleBatteryModule>();
			if (module != null)
			{
				module.UnregisterConsumer(IsWiperSlowActive);
				module.UnregisterConsumer(IsWiperFastActive);
			}
		}

		private bool IsBatteryUsable()
		{
			VehicleBatteryModule module = base.VehicleManager.GetModule<VehicleBatteryModule>();
			if (module != null && module.IsBatteryInstalled)
			{
				return !module.IsBatteryBroken;
			}
			return false;
		}

		private void OnWiperSlowActivated(float speed)
		{
			RefreshWiperAnimation();
			KickBatteryDrain();
		}

		private void OnWiperFastActivated(float speed)
		{
			RefreshWiperAnimation();
			KickBatteryDrain();
		}

		private void OnWiperDeactivated()
		{
			RefreshWiperAnimation();
		}

		private void OnBatteryDepleted()
		{
			RefreshWiperAnimation();
		}

		private void OnBatteryInstalled()
		{
			RefreshWiperAnimation();
			KickBatteryDrain();
		}

		private bool IsWiperSlowActive()
		{
			if (windshieldButton != null)
			{
				return windshieldButton.WiperState.Equals(WiperState.Slow);
			}
			return false;
		}

		private bool IsWiperFastActive()
		{
			if (windshieldButton != null)
			{
				return windshieldButton.WiperState.Equals(WiperState.Fast);
			}
			return false;
		}

		private float GetWiperSlowMultiplier()
		{
			return (base.VehicleManager.GetModule<VehicleBatteryModule>()?.InstalledBattery?.batteryConfig?.wiperSlowUsageMultiplier).GetValueOrDefault();
		}

		private float GetWiperFastMultiplier()
		{
			return (base.VehicleManager.GetModule<VehicleBatteryModule>()?.InstalledBattery?.batteryConfig?.wiperFastUsageMultiplier).GetValueOrDefault();
		}

		private void RefreshWiperAnimation()
		{
			if (!(windshieldButton == null))
			{
				WiperState wiperState = windshieldButton.WiperState;
				bool flag = wiperState != WiperState.Off && IsBatteryUsable();
				float speed = ((wiperState == WiperState.Fast) ? fastSpeedMultiplier : slowSpeedMultiplier);
				bladesSweeping = flag;
				currentSpeedMultiplier = speed;
				if (flag)
				{
					ApplySweep(leftWiperAnimator, speed);
					ApplySweep(rightWiperAnimator, speed);
					animatorSeeded = true;
				}
				else if (wiperState == WiperState.Off)
				{
					Park(leftWiperAnimator);
					Park(rightWiperAnimator);
					animatorSeeded = false;
				}
				else
				{
					Freeze(leftWiperAnimator);
					Freeze(rightWiperAnimator);
				}
			}
		}

		private void ApplySweep(Animator animator, float speed)
		{
			if (!(animator == null))
			{
				animator.SetBool(SweepHash, value: true);
				if (!animatorSeeded)
				{
					animator.Play(sweepStateHash, 0, 0f);
				}
				animator.speed = speed;
			}
		}

		private void Park(Animator animator)
		{
			if (!(animator == null))
			{
				animator.SetBool(SweepHash, value: false);
				animator.speed = 1f;
			}
		}

		private void Freeze(Animator animator)
		{
			if (!(animator == null))
			{
				animator.speed = 0f;
			}
		}

		private void KickBatteryDrain()
		{
			if (!(windshieldButton == null) && windshieldButton.WiperState != WiperState.Off && IsBatteryUsable())
			{
				base.VehicleManager.GetModule<VehicleBatteryModule>()?.ConsumeBatteryCondition().Forget();
			}
		}

		private void Update()
		{
			ApplyRainParameters();
			ApplyBodyRainParameters();
		}

		private void ApplyRainParameters()
		{
			if (!(windshieldRain == null))
			{
				bool flag = timeManager?.IsRaining ?? false;
				float time = (flag ? Mathf.Clamp01(timeManager.RainIntensity) : 0f);
				float num = intensityCurve.Evaluate(time);
				float num2 = 0f;
				VehicleController vehicleController = ((base.VehicleManager != null) ? base.VehicleManager.VehicleController : null);
				if (vehicleController != null)
				{
					num2 = Mathf.Abs(vehicleController.Speed);
				}
				float num3 = Mathf.Clamp01(num2 / Mathf.Max(0.01f, speedReferenceMs));
				windshieldRain.m_SpawnRate = baseSpawnRate * num;
				windshieldRain.m_SpawnAmount = (flag ? (baseSpawnAmount * num) : 0f);
				windshieldRain.m_MinDropletRadius = minDropletRadius;
				windshieldRain.m_MaxDropletRadius = Mathf.Lerp(baseMaxDropletRadius, maxDropletRadiusAtFullIntensity, num);
				windshieldRain.m_DropsInfluenceProportion = Mathf.Lerp(minDropsInfluence, maxDropsInfluence, num);
				windshieldRain.m_TurbulanceImpact = Mathf.Lerp(minTurbulanceImpact, maxTurbulanceImpact, num3);
				windshieldRain.m_TurbulanceSpeedImpactMultiplier = Mathf.Lerp(minTurbulanceSpeedMul, maxTurbulanceSpeedMul, num3);
				float num4 = (flag ? rainingStreakDecayRate : dryStreakDecayRate);
				if (bladesSweeping)
				{
					num4 *= Mathf.Max(1f, wiperActiveDecayMultiplier);
				}
				windshieldRain.m_RainStreakDecayRate = num4;
				float y = (flag ? ((0f - (idleMovementY + (maxMovementY - idleMovementY) * num3)) * num) : 0f);
				windshieldRain.Movement = new Vector2(0f, y);
				windshieldRain.UpdateShaderValues();
				if (wasRaining && !flag)
				{
					windshieldRain.ResetRain();
					ClearWindshieldRainMaterialHeightmap();
				}
				wasRaining = flag;
			}
		}

		private void ApplyBodyRainParameters()
		{
			if (simpleRainManager == null)
			{
				return;
			}
			VehicleController vehicleController = ((base.VehicleManager != null) ? base.VehicleManager.VehicleController : null);
			ITimeManager obj = timeManager;
			float time = ((obj != null && obj.IsRaining) ? Mathf.Clamp01(timeManager.RainIntensity) : 0f);
			float num = intensityCurve.Evaluate(time);
			float value = bodyMaxRainAmount * num;
			if (simpleRainManager.m_rainMaterials != null)
			{
				Material[] rainMaterials = simpleRainManager.m_rainMaterials;
				foreach (Material material in rainMaterials)
				{
					if (!(material == null) && material.HasProperty(bodyRainAmountPropertyId))
					{
						material.SetFloat(bodyRainAmountPropertyId, value);
					}
				}
			}
			if (!(vehicleController == null))
			{
				float num2 = Mathf.Abs(vehicleController.Speed);
				float t = Mathf.Clamp01(num2 / Mathf.Max(0.01f, speedReferenceMs));
				bool flag = num2 > minMovingSpeedMs;
				simpleRainManager.m_SetAccelerationForcibly = flag;
				if (flag)
				{
					Vector3 vector = vehicleController.transform.forward * num2;
					simpleRainManager.m_ForcedAcceleration = -vector + simpleRainManager.m_gravityVector;
				}
				simpleRainManager.m_addVelocityFactor = Mathf.Lerp(bodyRainAddVelocityMin, bodyRainAddVelocityMax, t);
				simpleRainManager.m_accelerationScale = Mathf.Lerp(bodyRainAccelScaleMin, bodyRainAccelScaleMax, t);
			}
		}
	}
}
