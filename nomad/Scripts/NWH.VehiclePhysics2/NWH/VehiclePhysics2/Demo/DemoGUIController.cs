using NWH.Common.Input;
using NWH.Common.SceneManagement;
using NWH.Common.Vehicles;
using NWH.VehiclePhysics2.Damage;
using NWH.VehiclePhysics2.Modules.ABS;
using NWH.VehiclePhysics2.Modules.Aerodynamics;
using NWH.VehiclePhysics2.Modules.AirSteer;
using NWH.VehiclePhysics2.Modules.ArcadeModule;
using NWH.VehiclePhysics2.Modules.ESC;
using NWH.VehiclePhysics2.Modules.FlipOver;
using NWH.VehiclePhysics2.Modules.TCS;
using NWH.VehiclePhysics2.Modules.Trailer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NWH.VehiclePhysics2.Demo
{
	[RequireComponent(typeof(Canvas))]
	public class DemoGUIController : MonoBehaviour
	{
		public static Color disabledColor = new Color32(66, 66, 66, 255);

		public static Color enabledColor = new Color32(76, 175, 80, 255);

		public Text promptText;

		public GameObject helpWindow;

		public GameObject settingsWindow;

		public GameObject telemetryWindow;

		public Button absButton;

		public Button tcsButton;

		public Button escButton;

		public Button aeroButton;

		public Button damageButton;

		public Button repairButton;

		public Button arcadeButton;

		public Button airSteerButton;

		public Button resetButton;

		public Button helpButton;

		public Button settingsButton;

		public Button telemetryButton;

		public Slider throttleSlider;

		public Slider brakeSlider;

		public Slider clutchSlider;

		public Slider handbrakeSlider;

		public Slider horizontalLeftSlider;

		public Slider horizontalRightSlider;

		public Slider damageSlider;

		public Text turboBoostTitle;

		public Text turboBoostReadout;

		private VehicleController _vc;

		private VehicleController _prevVc;

		private TrailerHitchModule _trailerHitchModule;

		private FlipOverModule _flipOverModule;

		private ABSModule _absModule;

		private TCSModule _tcsModule;

		private ESCModule _escModule;

		private AerodynamicsModule _aeroModule;

		private AirSteerModule _airSteerModule;

		private ArcadeModule _arcadeModule;

		private ColorBlock _colorBlock;

		private Canvas _canvas;

		private bool _toggleGUI;

		private void Start()
		{
			absButton.onClick.AddListener(ToggleABS);
			tcsButton.onClick.AddListener(ToggleTCS);
			escButton.onClick.AddListener(ToggleESC);
			aeroButton.onClick.AddListener(ToggleAero);
			airSteerButton.onClick.AddListener(ToggleAirSteer);
			arcadeButton.onClick.AddListener(ToggleArcade);
			damageButton.onClick.AddListener(ToggleDamage);
			repairButton.onClick.AddListener(RepairDamage);
			helpButton.onClick.AddListener(ToggleHelpWindow);
			telemetryButton.onClick.AddListener(ToggleTelemetryWindow);
			settingsButton.onClick.AddListener(ToggleSettingsWindow);
			resetButton.onClick.AddListener(ResetScene);
			_canvas = GetComponent<Canvas>();
		}

		private void Update()
		{
			_vc = Vehicle.ActiveVehicle as VehicleController;
			promptText.text = "";
			_toggleGUI = InputProvider.CombinedInput((SceneInputProviderBase i) => i.ToggleGUI());
			if (_toggleGUI)
			{
				_canvas.enabled = !_canvas.enabled;
			}
			if (VehicleChanger.Instance != null)
			{
				if (VehicleChanger.Instance.location == VehicleChanger.CharacterLocation.Near)
				{
					promptText.text += "Press V / Select (Xbox/PS) to enter the vehicle.\r\n";
				}
				else if (VehicleChanger.Instance.location == VehicleChanger.CharacterLocation.Inside && _vc.Speed < VehicleChanger.Instance.maxEnterExitVehicleSpeed)
				{
					promptText.text += "Press V / Select (Xbox/PS) to exit/change the vehicle.\r\n";
				}
			}
			if (_vc == null)
			{
				return;
			}
			if (_vc != _prevVc)
			{
				_trailerHitchModule = _vc.GetComponent<TrailerHitchModuleWrapper>()?.GetModule() as TrailerHitchModule;
				_flipOverModule = _vc.GetComponent<FlipOverModuleWrapper>()?.GetModule() as FlipOverModule;
				_absModule = _vc.GetComponent<ABSModuleWrapper>()?.GetModule() as ABSModule;
				_tcsModule = _vc.GetComponent<TCSModuleWrapper>()?.GetModule() as TCSModule;
				_escModule = _vc.GetComponent<ESCModuleWrapper>()?.GetModule() as ESCModule;
				_aeroModule = _vc.GetComponent<AerodynamicsModuleWrapper>()?.GetModule() as AerodynamicsModule;
				_tcsModule = _vc.GetComponent<TCSModuleWrapper>()?.GetModule() as TCSModule;
				_escModule = _vc.GetComponent<ESCModuleWrapper>()?.GetModule() as ESCModule;
				_arcadeModule = _vc.GetComponent<ArcadeModuleWrapper>()?.GetModule() as ArcadeModule;
				_airSteerModule = _vc.GetComponent<AirSteerModuleWrapper>()?.GetModule() as AirSteerModule;
			}
			if (!_vc.IsInitialized)
			{
				return;
			}
			throttleSlider.value = Mathf.Clamp01(_vc.input.InputSwappedThrottle);
			brakeSlider.value = Mathf.Clamp01(_vc.input.InputSwappedBrakes);
			clutchSlider.value = Mathf.Clamp01(_vc.powertrain.clutch.clutchInput);
			handbrakeSlider.value = Mathf.Clamp01(_vc.input.states.handbrake);
			horizontalLeftSlider.value = Mathf.Clamp01(0f - _vc.input.Steering);
			horizontalRightSlider.value = Mathf.Clamp01(_vc.input.Steering);
			TrailerHitchModule trailerHitchModule = _trailerHitchModule;
			if (trailerHitchModule != null && trailerHitchModule.trailerInRange && !trailerHitchModule.attached)
			{
				promptText.text += "Press T / X (Xbox) / Square (PS) to attach the trailer.\r\n";
			}
			FlipOverModule flipOverModule = _flipOverModule;
			if (flipOverModule != null && flipOverModule.flipOverActivation == FlipOverModule.FlipOverActivation.Manual && flipOverModule.flippedOver)
			{
				promptText.text += "Press M to recover the vehicle.\r\n";
			}
			if (_absModule != null)
			{
				absButton.targetGraphic.color = (_absModule.state.isEnabled ? enabledColor : disabledColor);
			}
			if (_tcsModule != null)
			{
				tcsButton.targetGraphic.color = (_tcsModule.state.isEnabled ? enabledColor : disabledColor);
			}
			if (_escModule != null)
			{
				escButton.targetGraphic.color = (_escModule.state.isEnabled ? enabledColor : disabledColor);
			}
			if (_aeroModule != null)
			{
				aeroButton.targetGraphic.color = (_aeroModule.state.isEnabled ? enabledColor : disabledColor);
			}
			if (_airSteerModule != null)
			{
				airSteerButton.targetGraphic.color = (_airSteerModule.state.isEnabled ? enabledColor : disabledColor);
			}
			if (_arcadeModule != null)
			{
				arcadeButton.targetGraphic.color = (_arcadeModule.state.isEnabled ? enabledColor : disabledColor);
			}
			if (turboBoostTitle != null)
			{
				if (_vc.powertrain.engine.forcedInduction.useForcedInduction)
				{
					turboBoostTitle.text = "Turbo Boost";
					turboBoostReadout.text = (_vc.powertrain.engine.forcedInduction.boost * 100f).ToString("F0") + " %";
				}
				else
				{
					turboBoostTitle.text = "";
					turboBoostReadout.text = "";
				}
			}
			DamageHandler component = _vc.GetComponent<DamageHandler>();
			if (component != null)
			{
				damageButton.targetGraphic.color = (component.enabled ? enabledColor : disabledColor);
				damageSlider.value = component.Damage;
			}
			_prevVc = _vc;
		}

		public void ToggleDamage()
		{
			DamageHandler component = _vc.GetComponent<DamageHandler>();
			if (component != null)
			{
				component.enabled = !component.enabled;
			}
		}

		public void RepairDamage()
		{
			DamageHandler component = _vc.GetComponent<DamageHandler>();
			if (component != null && _vc != null && component.enabled)
			{
				component.Repair();
			}
		}

		public void ToggleAero()
		{
			if (_aeroModule != null)
			{
				_aeroModule.state.lodIndex = -1;
				_aeroModule.ToggleState();
			}
		}

		public void ToggleABS()
		{
			if (_absModule != null)
			{
				_absModule.state.lodIndex = -1;
				_absModule.ToggleState();
			}
		}

		public void ToggleTCS()
		{
			if (_tcsModule != null)
			{
				_tcsModule.state.lodIndex = -1;
				_tcsModule.ToggleState();
			}
		}

		public void ToggleESC()
		{
			if (_escModule != null)
			{
				_escModule.state.lodIndex = -1;
				_escModule.ToggleState();
			}
		}

		public void ToggleArcade()
		{
			if (_arcadeModule != null)
			{
				_arcadeModule.ToggleState();
			}
		}

		public void ToggleAirSteer()
		{
			if (_airSteerModule != null)
			{
				_airSteerModule.ToggleState();
			}
		}

		public void ToggleHelpWindow()
		{
			helpWindow.SetActive(!helpWindow.activeInHierarchy);
			settingsWindow.SetActive(value: false);
			telemetryWindow.SetActive(value: false);
		}

		public void ToggleSettingsWindow()
		{
			settingsWindow.SetActive(!settingsWindow.activeInHierarchy);
			helpWindow.SetActive(value: false);
			telemetryWindow.SetActive(value: false);
		}

		public void ToggleTelemetryWindow()
		{
			telemetryWindow.SetActive(!telemetryWindow.activeInHierarchy);
			settingsWindow.SetActive(value: false);
			helpWindow.SetActive(value: false);
		}

		public void ResetScene()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
