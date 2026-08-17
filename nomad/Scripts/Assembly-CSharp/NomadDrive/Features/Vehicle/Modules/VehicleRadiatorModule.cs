using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Vehicle.Parts.Radiator;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleRadiatorModule : VehicleModule
	{
		private const string RADIATOR = "Radiator";

		private const string DEBUG = "Radiator/Debug";

		[SerializeField]
		private RadiatorSlot radiatorSlotRef;

		[SerializeField]
		public bool IsRadiatorInstalled;

		[SerializeField]
		private Radiator _installedRadiator;

		private bool _isConsumingCoolant;

		private ConditionComponent _radiatorCondition;

		private float CoolantAmountDebug => (_installedRadiator?.LiquidContainer?.CurrentAmount).GetValueOrDefault();

		private float CoolantCapacityDebug => (_installedRadiator?.LiquidContainer?.Capacity).GetValueOrDefault();

		private float CoolantFillRatioDebug => CoolantFillRatio;

		private float EffectiveCoolantRatioDebug => EffectiveCoolantRatio;

		private RadiatorConfig Config => _installedRadiator?.radiatorConfig;

		public RadiatorSlot RadiatorSlotRef => radiatorSlotRef;

		public Radiator InstalledRadiator => _installedRadiator;

		public float CoolantFillRatio => (_installedRadiator?.LiquidContainer?.FillRatio).GetValueOrDefault();

		public float EffectiveCoolantRatio
		{
			get
			{
				if (!IsRadiatorInstalled || Config == null)
				{
					return 0f;
				}
				float time = ((_radiatorCondition != null) ? _radiatorCondition.ConditionRatio : 1f);
				float num = Config.conditionEfficiencyCurve.Evaluate(time);
				return Mathf.Clamp01(CoolantFillRatio * num);
			}
		}

		protected override void SubscribeEvents()
		{
			radiatorSlotRef.OnRadiatorInstalled.AddListener(OnRadiatorInstalled);
			radiatorSlotRef.OnRadiatorRemoved.AddListener(OnRadiatorRemoved);
			base.EventBus.OnUnderHoodPartsActivate += OnUnderHoodActivate;
			base.EventBus.OnUnderHoodPartsDeactivate += OnUnderHoodDeactivate;
			base.EventBus.OnEngineStarted += OnEngineStartedConsumeCoolant;
			base.EventBus.OnEngineStopped += OnEngineStoppedConsumeCoolant;
		}

		protected override void UnsubscribeEvents()
		{
			radiatorSlotRef.OnRadiatorInstalled.RemoveListener(OnRadiatorInstalled);
			radiatorSlotRef.OnRadiatorRemoved.RemoveListener(OnRadiatorRemoved);
			base.EventBus.OnUnderHoodPartsActivate -= OnUnderHoodActivate;
			base.EventBus.OnUnderHoodPartsDeactivate -= OnUnderHoodDeactivate;
			base.EventBus.OnEngineStarted -= OnEngineStartedConsumeCoolant;
			base.EventBus.OnEngineStopped -= OnEngineStoppedConsumeCoolant;
		}

		private void OnRadiatorInstalled(Radiator radiator)
		{
			if (radiator.radiatorConfig == null)
			{
				EvilLogger.LogError("Radiator config is null", "OnRadiatorInstalled", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Modules\\VehicleRadiatorModule.cs", 73);
				return;
			}
			_installedRadiator = radiator;
			_radiatorCondition = radiator.GetComponent<ConditionComponent>();
			IsRadiatorInstalled = true;
		}

		private void OnRadiatorRemoved()
		{
			_isConsumingCoolant = false;
			_installedRadiator = null;
			_radiatorCondition = null;
			IsRadiatorInstalled = false;
		}

		private async void OnEngineStartedConsumeCoolant()
		{
			if (!(base.VehicleManager.NetworkSync == null) && base.VehicleManager.NetworkSync.isServer)
			{
				await ConsumeCoolantLoop();
			}
		}

		private void OnEngineStoppedConsumeCoolant()
		{
			_isConsumingCoolant = false;
		}

		private async UniTask ConsumeCoolantLoop()
		{
			if (IsRadiatorInstalled && !(Config == null) && !(_installedRadiator.LiquidContainer == null) && !_isConsumingCoolant)
			{
				_isConsumingCoolant = true;
				await UniTask.NextFrame();
				VehicleIgnitionModule ignitionModule = base.VehicleManager.GetModule<VehicleIgnitionModule>();
				while (_isConsumingCoolant && IsRadiatorInstalled && _installedRadiator.LiquidContainer.CurrentAmount > 0f && ignitionModule != null && ignitionModule.IsIgnited)
				{
					_installedRadiator.LiquidContainer.Drain(Config.coolantConsumptionRate * Time.deltaTime);
					await UniTask.NextFrame();
				}
				_isConsumingCoolant = false;
			}
		}

		private void OnUnderHoodActivate()
		{
			radiatorSlotRef.Activate();
		}

		private void OnUnderHoodDeactivate()
		{
			radiatorSlotRef.Deactivate();
		}

		public override void OnFrontSeatsTaken()
		{
			_installedRadiator?.IgnoreHovering();
		}

		public override void OnFrontSeatsVacated()
		{
			_installedRadiator?.UnignoreHovering();
		}
	}
}
