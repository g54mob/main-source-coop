using NWH.Common.Vehicles;
using NWH.VehiclePhysics2.Modules.ABS;
using NWH.VehiclePhysics2.Modules.TCS;
using UnityEngine;
using UnityEngine.Serialization;

namespace NWH.VehiclePhysics2.VehicleGUI
{
	[RequireComponent(typeof(Canvas))]
	public class DashGUIController : MonoBehaviour
	{
		public enum DataSource
		{
			VehicleController = 0,
			VehicleChanger = 1
		}

		[FormerlySerializedAs("ABS")]
		public DashLight absDashLight;

		public AnalogGauge analogRpmGauge;

		public AnalogGauge analogSpeedGauge;

		[FormerlySerializedAs("checkEngine")]
		public DashLight checkEngineDashLight;

		public DataSource dataSource;

		public DigitalGauge digitalGearGauge;

		public DigitalGauge digitalRpmGauge;

		public DigitalGauge digitalSpeedGauge;

		[FormerlySerializedAs("highBeam")]
		public DashLight highBeamDashLight;

		[FormerlySerializedAs("leftBlinker")]
		public DashLight leftBlinkerDashLight;

		[FormerlySerializedAs("lowBeam")]
		public DashLight lowBeamDashLight;

		[FormerlySerializedAs("rightBlinker")]
		public DashLight rightBlinkerDashLight;

		[FormerlySerializedAs("TCS")]
		public DashLight tcsDashLight;

		public bool useAbsDashLight;

		public bool useAnalogRpmGauge;

		public bool useAnalogSpeedGauge;

		public bool useCheckEngineDashLight;

		public bool useDigitalGearGauge;

		public bool useDigitalRpmGauge;

		public bool useDigitalSpeedGauge;

		public bool useHighBeamDashLight;

		public bool useLeftBlinkerDashLight;

		public bool useLowBeamDashLight;

		public bool useRightBlinkerDashLight;

		public bool useTcsDashLight;

		public VehicleController vehicleController;

		private Canvas _canvas;

		private VehicleController _prevVc;

		private ABSModule _absModule;

		private TCSModule _tcsModule;

		private bool _update = true;

		private void Awake()
		{
			_canvas = GetComponent<Canvas>();
			if (vehicleController != null)
			{
				vehicleController.onEnable.AddListener(OnVehicleEnable);
				vehicleController.onDisable.AddListener(OnVehicleDisable);
			}
		}

		private void Update()
		{
			vehicleController = Vehicle.ActiveVehicle as VehicleController;
			if (_prevVc != vehicleController && vehicleController != null)
			{
				_tcsModule = vehicleController.GetComponent<TCSModuleWrapper>()?.module;
				_absModule = vehicleController.GetComponent<ABSModuleWrapper>()?.module;
			}
			if (vehicleController != null && _update)
			{
				if (useAnalogRpmGauge)
				{
					analogRpmGauge.Value = vehicleController.powertrain.engine.OutputRPM;
				}
				if (useDigitalRpmGauge)
				{
					digitalRpmGauge.numericalValue = vehicleController.powertrain.engine.OutputRPM;
				}
				if (useAnalogSpeedGauge)
				{
					analogSpeedGauge.Value = vehicleController.Speed * 3.6f;
				}
				if (useDigitalSpeedGauge)
				{
					digitalSpeedGauge.numericalValue = vehicleController.Speed * 3.6f;
				}
				if (useDigitalGearGauge)
				{
					digitalGearGauge.stringValue = vehicleController.powertrain.transmission.GearName;
				}
				if (useLeftBlinkerDashLight)
				{
					leftBlinkerDashLight.Active = vehicleController.effectsManager.lightsManager.leftBlinkers.On;
				}
				if (useRightBlinkerDashLight)
				{
					rightBlinkerDashLight.Active = vehicleController.effectsManager.lightsManager.rightBlinkers.On;
				}
				if (useLowBeamDashLight)
				{
					lowBeamDashLight.Active = vehicleController.effectsManager.lightsManager.lowBeamLights.On;
				}
				if (useHighBeamDashLight)
				{
					highBeamDashLight.Active = vehicleController.effectsManager.lightsManager.highBeamLights.On;
				}
				if (useTcsDashLight && _tcsModule != null)
				{
					tcsDashLight.Active = _tcsModule.active;
				}
				if (useAbsDashLight && _absModule != null)
				{
					absDashLight.Active = _absModule.active;
				}
				if (useCheckEngineDashLight)
				{
					checkEngineDashLight.Active = vehicleController.powertrain.engine.Damage > 0.9999f;
				}
			}
			else
			{
				if (useAnalogRpmGauge)
				{
					analogRpmGauge.Value = 0f;
				}
				if (useAnalogSpeedGauge)
				{
					analogSpeedGauge.Value = 0f;
				}
				if (useDigitalSpeedGauge)
				{
					digitalSpeedGauge.numericalValue = 0f;
				}
				if (useDigitalGearGauge)
				{
					digitalGearGauge.stringValue = "";
				}
				if (useLeftBlinkerDashLight)
				{
					leftBlinkerDashLight.Active = false;
				}
				if (useRightBlinkerDashLight)
				{
					rightBlinkerDashLight.Active = false;
				}
				if (useLowBeamDashLight)
				{
					lowBeamDashLight.Active = false;
				}
				if (useHighBeamDashLight)
				{
					highBeamDashLight.Active = false;
				}
				if (useTcsDashLight)
				{
					tcsDashLight.Active = false;
				}
				if (useAbsDashLight)
				{
					absDashLight.Active = false;
				}
				if (useCheckEngineDashLight)
				{
					checkEngineDashLight.Active = false;
				}
			}
			_prevVc = vehicleController;
		}

		private void OnVehicleEnable()
		{
			_canvas.enabled = true;
			_update = true;
		}

		private void OnVehicleDisable()
		{
			_canvas.enabled = false;
			_update = false;
		}
	}
}
