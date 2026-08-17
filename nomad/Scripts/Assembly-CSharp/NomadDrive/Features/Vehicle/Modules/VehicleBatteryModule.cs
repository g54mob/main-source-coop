using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Vehicle.Parts.Battery;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleBatteryModule : VehicleModule
	{
		private const string BATTERY = "Battery";

		private const float BATTERY_CONSUMPTION_SPEED = 0.008f;

		[SerializeField]
		private BatterySlot _batterySlotRef;

		[SerializeField]
		public bool IsBatteryInstalled;

		[SerializeField]
		private Battery _installedBattery;

		[SerializeField]
		public UnityEvent onBatteryConditionZero;

		[SerializeField]
		private float _batteryMaxPower;

		[SerializeField]
		private float _batteryConditionConsumptionRate;

		private ConditionComponent _batteryCondition;

		private bool _isConsumingBattery;

		private readonly List<(Func<bool> isActive, Func<float> multiplier)> _consumers = new List<(Func<bool>, Func<float>)>();

		public BatterySlot BatterySlotRef => _batterySlotRef;

		public Battery InstalledBattery => _installedBattery;

		public bool IsBatteryBroken
		{
			get
			{
				if (_batteryCondition != null)
				{
					return _batteryCondition.IsBroken;
				}
				return false;
			}
		}

		public bool IsBatteryUseful
		{
			get
			{
				if (IsBatteryInstalled)
				{
					return !IsBatteryBroken;
				}
				return false;
			}
		}

		public void RegisterConsumer(Func<bool> isActive, Func<float> multiplier)
		{
			_consumers.Add((isActive, multiplier));
		}

		public void UnregisterConsumer(Func<bool> isActive)
		{
			_consumers.RemoveAll(((Func<bool> isActive, Func<float> multiplier) c) => c.isActive == isActive);
		}

		protected override void SubscribeEvents()
		{
			_batterySlotRef.OnBatteryInstalled.AddListener(OnBatteryInstalled);
			_batterySlotRef.OnBatteryRemoved.AddListener(OnBatteryRemoved);
			onBatteryConditionZero.AddListener(OnBatteryConditionZero);
			base.EventBus.OnUnderHoodPartsActivate += OnUnderHoodActivate;
			base.EventBus.OnUnderHoodPartsDeactivate += OnUnderHoodDeactivate;
		}

		protected override void UnsubscribeEvents()
		{
			_batterySlotRef.OnBatteryInstalled.RemoveListener(OnBatteryInstalled);
			_batterySlotRef.OnBatteryRemoved.RemoveListener(OnBatteryRemoved);
			onBatteryConditionZero.RemoveListener(OnBatteryConditionZero);
			base.EventBus.OnUnderHoodPartsActivate -= OnUnderHoodActivate;
			base.EventBus.OnUnderHoodPartsDeactivate -= OnUnderHoodDeactivate;
		}

		private void SetBatteryProperties(Battery battery)
		{
			if (battery == null)
			{
				_installedBattery = null;
				_batteryCondition = null;
				IsBatteryInstalled = false;
				_batteryMaxPower = 0f;
				_batteryConditionConsumptionRate = 0f;
			}
			else if (battery.batteryConfig == null)
			{
				EvilLogger.LogError("Battery config is null", "SetBatteryProperties", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Modules\\VehicleBatteryModule.cs", 81);
			}
			else
			{
				_installedBattery = battery;
				_batteryCondition = battery.GetComponent<ConditionComponent>();
				IsBatteryInstalled = true;
				_batteryMaxPower = battery.batteryConfig.maxPower;
				_batteryConditionConsumptionRate = battery.batteryConfig.conditionConsumptionRate;
			}
		}

		private void OnBatteryInstalled(Battery battery)
		{
			SetBatteryProperties(battery);
			base.EventBus.FireBatteryUsefulChanged(IsBatteryUseful);
			base.EventBus.FireBatteryInstalled();
		}

		private void OnBatteryRemoved()
		{
			base.EventBus.FireBatteryRemoved();
			base.EventBus.FireBatteryDepleted();
			SetBatteryProperties(null);
		}

		public async UniTask ConsumeBatteryCondition()
		{
			if (base.VehicleManager.NetworkSync == null || !base.VehicleManager.NetworkSync.isServer || !IsBatteryInstalled || _batteryCondition == null || _batteryCondition.Condition <= 0f || _isConsumingBattery)
			{
				return;
			}
			float conditionConsumptionRate = _batteryConditionConsumptionRate;
			_isConsumingBattery = true;
			while (IsBatteryInstalled && _batteryCondition != null && IsBatteryConsumptionActive())
			{
				float calculatedBatteryConsumptionRate = GetCalculatedBatteryConsumptionRate();
				_batteryCondition.ServerSetCondition(_batteryCondition.Condition - calculatedBatteryConsumptionRate * conditionConsumptionRate * 0.008f * Time.deltaTime);
				if (_batteryCondition.Condition <= 0f)
				{
					onBatteryConditionZero.Invoke();
					_batteryCondition.ServerSetCondition(0f);
					break;
				}
				await UniTask.NextFrame();
			}
			_isConsumingBattery = false;
		}

		private bool IsBatteryConsumptionActive()
		{
			foreach (var consumer in _consumers)
			{
				if (consumer.isActive())
				{
					return true;
				}
			}
			return false;
		}

		private float GetCalculatedBatteryConsumptionRate()
		{
			float num = 0f;
			foreach (var consumer in _consumers)
			{
				if (consumer.isActive())
				{
					num += consumer.multiplier();
				}
			}
			return num;
		}

		private void OnBatteryConditionZero()
		{
			base.EventBus.FireBatteryDepleted();
		}

		private void OnUnderHoodActivate()
		{
			_batterySlotRef.Activate();
		}

		private void OnUnderHoodDeactivate()
		{
			_batterySlotRef.Deactivate();
		}

		public override void OnFrontSeatsTaken()
		{
			_installedBattery?.IgnoreHovering();
		}

		public override void OnFrontSeatsVacated()
		{
			_installedBattery?.UnignoreHovering();
		}
	}
}
