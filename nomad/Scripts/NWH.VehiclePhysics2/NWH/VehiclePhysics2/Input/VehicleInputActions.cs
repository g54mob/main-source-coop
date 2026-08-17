using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace NWH.VehiclePhysics2.Input
{
	public class VehicleInputActions : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
	{
		public struct VehicleControlsActions
		{
			private VehicleInputActions m_Wrapper;

			public InputAction Steering => m_Wrapper.m_VehicleControls_Steering;

			public InputAction Throttle => m_Wrapper.m_VehicleControls_Throttle;

			public InputAction Brakes => m_Wrapper.m_VehicleControls_Brakes;

			public InputAction Clutch => m_Wrapper.m_VehicleControls_Clutch;

			public InputAction Handbrake => m_Wrapper.m_VehicleControls_Handbrake;

			public InputAction EngineStartStop => m_Wrapper.m_VehicleControls_EngineStartStop;

			public InputAction ShiftUp => m_Wrapper.m_VehicleControls_ShiftUp;

			public InputAction ShiftDown => m_Wrapper.m_VehicleControls_ShiftDown;

			public InputAction LeftBlinker => m_Wrapper.m_VehicleControls_LeftBlinker;

			public InputAction RightBlinker => m_Wrapper.m_VehicleControls_RightBlinker;

			public InputAction LowBeamLights => m_Wrapper.m_VehicleControls_LowBeamLights;

			public InputAction HighBeamLights => m_Wrapper.m_VehicleControls_HighBeamLights;

			public InputAction HazardLights => m_Wrapper.m_VehicleControls_HazardLights;

			public InputAction ExtraLights => m_Wrapper.m_VehicleControls_ExtraLights;

			public InputAction TrailerAttachDetach => m_Wrapper.m_VehicleControls_TrailerAttachDetach;

			public InputAction Horn => m_Wrapper.m_VehicleControls_Horn;

			public InputAction ShiftIntoR1 => m_Wrapper.m_VehicleControls_ShiftIntoR1;

			public InputAction ShiftInto0 => m_Wrapper.m_VehicleControls_ShiftInto0;

			public InputAction ShiftInto1 => m_Wrapper.m_VehicleControls_ShiftInto1;

			public InputAction ShiftInto2 => m_Wrapper.m_VehicleControls_ShiftInto2;

			public InputAction ShiftInto3 => m_Wrapper.m_VehicleControls_ShiftInto3;

			public InputAction ShiftInto4 => m_Wrapper.m_VehicleControls_ShiftInto4;

			public InputAction ShiftInto5 => m_Wrapper.m_VehicleControls_ShiftInto5;

			public InputAction ShiftInto6 => m_Wrapper.m_VehicleControls_ShiftInto6;

			public InputAction ShiftInto7 => m_Wrapper.m_VehicleControls_ShiftInto7;

			public InputAction ShiftInto8 => m_Wrapper.m_VehicleControls_ShiftInto8;

			public InputAction FlipOver => m_Wrapper.m_VehicleControls_FlipOver;

			public InputAction Boost => m_Wrapper.m_VehicleControls_Boost;

			public InputAction CruiseControl => m_Wrapper.m_VehicleControls_CruiseControl;

			public bool enabled => Get().enabled;

			public VehicleControlsActions(VehicleInputActions wrapper)
			{
				m_Wrapper = wrapper;
			}

			public InputActionMap Get()
			{
				return m_Wrapper.m_VehicleControls;
			}

			public void Enable()
			{
				Get().Enable();
			}

			public void Disable()
			{
				Get().Disable();
			}

			public static implicit operator InputActionMap(VehicleControlsActions set)
			{
				return set.Get();
			}

			public void AddCallbacks(IVehicleControlsActions instance)
			{
				if (instance != null && !m_Wrapper.m_VehicleControlsActionsCallbackInterfaces.Contains(instance))
				{
					m_Wrapper.m_VehicleControlsActionsCallbackInterfaces.Add(instance);
					Steering.started += instance.OnSteering;
					Steering.performed += instance.OnSteering;
					Steering.canceled += instance.OnSteering;
					Throttle.started += instance.OnThrottle;
					Throttle.performed += instance.OnThrottle;
					Throttle.canceled += instance.OnThrottle;
					Brakes.started += instance.OnBrakes;
					Brakes.performed += instance.OnBrakes;
					Brakes.canceled += instance.OnBrakes;
					Clutch.started += instance.OnClutch;
					Clutch.performed += instance.OnClutch;
					Clutch.canceled += instance.OnClutch;
					Handbrake.started += instance.OnHandbrake;
					Handbrake.performed += instance.OnHandbrake;
					Handbrake.canceled += instance.OnHandbrake;
					EngineStartStop.started += instance.OnEngineStartStop;
					EngineStartStop.performed += instance.OnEngineStartStop;
					EngineStartStop.canceled += instance.OnEngineStartStop;
					ShiftUp.started += instance.OnShiftUp;
					ShiftUp.performed += instance.OnShiftUp;
					ShiftUp.canceled += instance.OnShiftUp;
					ShiftDown.started += instance.OnShiftDown;
					ShiftDown.performed += instance.OnShiftDown;
					ShiftDown.canceled += instance.OnShiftDown;
					LeftBlinker.started += instance.OnLeftBlinker;
					LeftBlinker.performed += instance.OnLeftBlinker;
					LeftBlinker.canceled += instance.OnLeftBlinker;
					RightBlinker.started += instance.OnRightBlinker;
					RightBlinker.performed += instance.OnRightBlinker;
					RightBlinker.canceled += instance.OnRightBlinker;
					LowBeamLights.started += instance.OnLowBeamLights;
					LowBeamLights.performed += instance.OnLowBeamLights;
					LowBeamLights.canceled += instance.OnLowBeamLights;
					HighBeamLights.started += instance.OnHighBeamLights;
					HighBeamLights.performed += instance.OnHighBeamLights;
					HighBeamLights.canceled += instance.OnHighBeamLights;
					HazardLights.started += instance.OnHazardLights;
					HazardLights.performed += instance.OnHazardLights;
					HazardLights.canceled += instance.OnHazardLights;
					ExtraLights.started += instance.OnExtraLights;
					ExtraLights.performed += instance.OnExtraLights;
					ExtraLights.canceled += instance.OnExtraLights;
					TrailerAttachDetach.started += instance.OnTrailerAttachDetach;
					TrailerAttachDetach.performed += instance.OnTrailerAttachDetach;
					TrailerAttachDetach.canceled += instance.OnTrailerAttachDetach;
					Horn.started += instance.OnHorn;
					Horn.performed += instance.OnHorn;
					Horn.canceled += instance.OnHorn;
					ShiftIntoR1.started += instance.OnShiftIntoR1;
					ShiftIntoR1.performed += instance.OnShiftIntoR1;
					ShiftIntoR1.canceled += instance.OnShiftIntoR1;
					ShiftInto0.started += instance.OnShiftInto0;
					ShiftInto0.performed += instance.OnShiftInto0;
					ShiftInto0.canceled += instance.OnShiftInto0;
					ShiftInto1.started += instance.OnShiftInto1;
					ShiftInto1.performed += instance.OnShiftInto1;
					ShiftInto1.canceled += instance.OnShiftInto1;
					ShiftInto2.started += instance.OnShiftInto2;
					ShiftInto2.performed += instance.OnShiftInto2;
					ShiftInto2.canceled += instance.OnShiftInto2;
					ShiftInto3.started += instance.OnShiftInto3;
					ShiftInto3.performed += instance.OnShiftInto3;
					ShiftInto3.canceled += instance.OnShiftInto3;
					ShiftInto4.started += instance.OnShiftInto4;
					ShiftInto4.performed += instance.OnShiftInto4;
					ShiftInto4.canceled += instance.OnShiftInto4;
					ShiftInto5.started += instance.OnShiftInto5;
					ShiftInto5.performed += instance.OnShiftInto5;
					ShiftInto5.canceled += instance.OnShiftInto5;
					ShiftInto6.started += instance.OnShiftInto6;
					ShiftInto6.performed += instance.OnShiftInto6;
					ShiftInto6.canceled += instance.OnShiftInto6;
					ShiftInto7.started += instance.OnShiftInto7;
					ShiftInto7.performed += instance.OnShiftInto7;
					ShiftInto7.canceled += instance.OnShiftInto7;
					ShiftInto8.started += instance.OnShiftInto8;
					ShiftInto8.performed += instance.OnShiftInto8;
					ShiftInto8.canceled += instance.OnShiftInto8;
					FlipOver.started += instance.OnFlipOver;
					FlipOver.performed += instance.OnFlipOver;
					FlipOver.canceled += instance.OnFlipOver;
					Boost.started += instance.OnBoost;
					Boost.performed += instance.OnBoost;
					Boost.canceled += instance.OnBoost;
					CruiseControl.started += instance.OnCruiseControl;
					CruiseControl.performed += instance.OnCruiseControl;
					CruiseControl.canceled += instance.OnCruiseControl;
				}
			}

			private void UnregisterCallbacks(IVehicleControlsActions instance)
			{
				Steering.started -= instance.OnSteering;
				Steering.performed -= instance.OnSteering;
				Steering.canceled -= instance.OnSteering;
				Throttle.started -= instance.OnThrottle;
				Throttle.performed -= instance.OnThrottle;
				Throttle.canceled -= instance.OnThrottle;
				Brakes.started -= instance.OnBrakes;
				Brakes.performed -= instance.OnBrakes;
				Brakes.canceled -= instance.OnBrakes;
				Clutch.started -= instance.OnClutch;
				Clutch.performed -= instance.OnClutch;
				Clutch.canceled -= instance.OnClutch;
				Handbrake.started -= instance.OnHandbrake;
				Handbrake.performed -= instance.OnHandbrake;
				Handbrake.canceled -= instance.OnHandbrake;
				EngineStartStop.started -= instance.OnEngineStartStop;
				EngineStartStop.performed -= instance.OnEngineStartStop;
				EngineStartStop.canceled -= instance.OnEngineStartStop;
				ShiftUp.started -= instance.OnShiftUp;
				ShiftUp.performed -= instance.OnShiftUp;
				ShiftUp.canceled -= instance.OnShiftUp;
				ShiftDown.started -= instance.OnShiftDown;
				ShiftDown.performed -= instance.OnShiftDown;
				ShiftDown.canceled -= instance.OnShiftDown;
				LeftBlinker.started -= instance.OnLeftBlinker;
				LeftBlinker.performed -= instance.OnLeftBlinker;
				LeftBlinker.canceled -= instance.OnLeftBlinker;
				RightBlinker.started -= instance.OnRightBlinker;
				RightBlinker.performed -= instance.OnRightBlinker;
				RightBlinker.canceled -= instance.OnRightBlinker;
				LowBeamLights.started -= instance.OnLowBeamLights;
				LowBeamLights.performed -= instance.OnLowBeamLights;
				LowBeamLights.canceled -= instance.OnLowBeamLights;
				HighBeamLights.started -= instance.OnHighBeamLights;
				HighBeamLights.performed -= instance.OnHighBeamLights;
				HighBeamLights.canceled -= instance.OnHighBeamLights;
				HazardLights.started -= instance.OnHazardLights;
				HazardLights.performed -= instance.OnHazardLights;
				HazardLights.canceled -= instance.OnHazardLights;
				ExtraLights.started -= instance.OnExtraLights;
				ExtraLights.performed -= instance.OnExtraLights;
				ExtraLights.canceled -= instance.OnExtraLights;
				TrailerAttachDetach.started -= instance.OnTrailerAttachDetach;
				TrailerAttachDetach.performed -= instance.OnTrailerAttachDetach;
				TrailerAttachDetach.canceled -= instance.OnTrailerAttachDetach;
				Horn.started -= instance.OnHorn;
				Horn.performed -= instance.OnHorn;
				Horn.canceled -= instance.OnHorn;
				ShiftIntoR1.started -= instance.OnShiftIntoR1;
				ShiftIntoR1.performed -= instance.OnShiftIntoR1;
				ShiftIntoR1.canceled -= instance.OnShiftIntoR1;
				ShiftInto0.started -= instance.OnShiftInto0;
				ShiftInto0.performed -= instance.OnShiftInto0;
				ShiftInto0.canceled -= instance.OnShiftInto0;
				ShiftInto1.started -= instance.OnShiftInto1;
				ShiftInto1.performed -= instance.OnShiftInto1;
				ShiftInto1.canceled -= instance.OnShiftInto1;
				ShiftInto2.started -= instance.OnShiftInto2;
				ShiftInto2.performed -= instance.OnShiftInto2;
				ShiftInto2.canceled -= instance.OnShiftInto2;
				ShiftInto3.started -= instance.OnShiftInto3;
				ShiftInto3.performed -= instance.OnShiftInto3;
				ShiftInto3.canceled -= instance.OnShiftInto3;
				ShiftInto4.started -= instance.OnShiftInto4;
				ShiftInto4.performed -= instance.OnShiftInto4;
				ShiftInto4.canceled -= instance.OnShiftInto4;
				ShiftInto5.started -= instance.OnShiftInto5;
				ShiftInto5.performed -= instance.OnShiftInto5;
				ShiftInto5.canceled -= instance.OnShiftInto5;
				ShiftInto6.started -= instance.OnShiftInto6;
				ShiftInto6.performed -= instance.OnShiftInto6;
				ShiftInto6.canceled -= instance.OnShiftInto6;
				ShiftInto7.started -= instance.OnShiftInto7;
				ShiftInto7.performed -= instance.OnShiftInto7;
				ShiftInto7.canceled -= instance.OnShiftInto7;
				ShiftInto8.started -= instance.OnShiftInto8;
				ShiftInto8.performed -= instance.OnShiftInto8;
				ShiftInto8.canceled -= instance.OnShiftInto8;
				FlipOver.started -= instance.OnFlipOver;
				FlipOver.performed -= instance.OnFlipOver;
				FlipOver.canceled -= instance.OnFlipOver;
				Boost.started -= instance.OnBoost;
				Boost.performed -= instance.OnBoost;
				Boost.canceled -= instance.OnBoost;
				CruiseControl.started -= instance.OnCruiseControl;
				CruiseControl.performed -= instance.OnCruiseControl;
				CruiseControl.canceled -= instance.OnCruiseControl;
			}

			public void RemoveCallbacks(IVehicleControlsActions instance)
			{
				if (m_Wrapper.m_VehicleControlsActionsCallbackInterfaces.Remove(instance))
				{
					UnregisterCallbacks(instance);
				}
			}

			public void SetCallbacks(IVehicleControlsActions instance)
			{
				foreach (IVehicleControlsActions vehicleControlsActionsCallbackInterface in m_Wrapper.m_VehicleControlsActionsCallbackInterfaces)
				{
					UnregisterCallbacks(vehicleControlsActionsCallbackInterface);
				}
				m_Wrapper.m_VehicleControlsActionsCallbackInterfaces.Clear();
				AddCallbacks(instance);
			}
		}

		public interface IVehicleControlsActions
		{
			void OnSteering(InputAction.CallbackContext context);

			void OnThrottle(InputAction.CallbackContext context);

			void OnBrakes(InputAction.CallbackContext context);

			void OnClutch(InputAction.CallbackContext context);

			void OnHandbrake(InputAction.CallbackContext context);

			void OnEngineStartStop(InputAction.CallbackContext context);

			void OnShiftUp(InputAction.CallbackContext context);

			void OnShiftDown(InputAction.CallbackContext context);

			void OnLeftBlinker(InputAction.CallbackContext context);

			void OnRightBlinker(InputAction.CallbackContext context);

			void OnLowBeamLights(InputAction.CallbackContext context);

			void OnHighBeamLights(InputAction.CallbackContext context);

			void OnHazardLights(InputAction.CallbackContext context);

			void OnExtraLights(InputAction.CallbackContext context);

			void OnTrailerAttachDetach(InputAction.CallbackContext context);

			void OnHorn(InputAction.CallbackContext context);

			void OnShiftIntoR1(InputAction.CallbackContext context);

			void OnShiftInto0(InputAction.CallbackContext context);

			void OnShiftInto1(InputAction.CallbackContext context);

			void OnShiftInto2(InputAction.CallbackContext context);

			void OnShiftInto3(InputAction.CallbackContext context);

			void OnShiftInto4(InputAction.CallbackContext context);

			void OnShiftInto5(InputAction.CallbackContext context);

			void OnShiftInto6(InputAction.CallbackContext context);

			void OnShiftInto7(InputAction.CallbackContext context);

			void OnShiftInto8(InputAction.CallbackContext context);

			void OnFlipOver(InputAction.CallbackContext context);

			void OnBoost(InputAction.CallbackContext context);

			void OnCruiseControl(InputAction.CallbackContext context);
		}

		private readonly InputActionMap m_VehicleControls;

		private List<IVehicleControlsActions> m_VehicleControlsActionsCallbackInterfaces = new List<IVehicleControlsActions>();

		private readonly InputAction m_VehicleControls_Steering;

		private readonly InputAction m_VehicleControls_Throttle;

		private readonly InputAction m_VehicleControls_Brakes;

		private readonly InputAction m_VehicleControls_Clutch;

		private readonly InputAction m_VehicleControls_Handbrake;

		private readonly InputAction m_VehicleControls_EngineStartStop;

		private readonly InputAction m_VehicleControls_ShiftUp;

		private readonly InputAction m_VehicleControls_ShiftDown;

		private readonly InputAction m_VehicleControls_LeftBlinker;

		private readonly InputAction m_VehicleControls_RightBlinker;

		private readonly InputAction m_VehicleControls_LowBeamLights;

		private readonly InputAction m_VehicleControls_HighBeamLights;

		private readonly InputAction m_VehicleControls_HazardLights;

		private readonly InputAction m_VehicleControls_ExtraLights;

		private readonly InputAction m_VehicleControls_TrailerAttachDetach;

		private readonly InputAction m_VehicleControls_Horn;

		private readonly InputAction m_VehicleControls_ShiftIntoR1;

		private readonly InputAction m_VehicleControls_ShiftInto0;

		private readonly InputAction m_VehicleControls_ShiftInto1;

		private readonly InputAction m_VehicleControls_ShiftInto2;

		private readonly InputAction m_VehicleControls_ShiftInto3;

		private readonly InputAction m_VehicleControls_ShiftInto4;

		private readonly InputAction m_VehicleControls_ShiftInto5;

		private readonly InputAction m_VehicleControls_ShiftInto6;

		private readonly InputAction m_VehicleControls_ShiftInto7;

		private readonly InputAction m_VehicleControls_ShiftInto8;

		private readonly InputAction m_VehicleControls_FlipOver;

		private readonly InputAction m_VehicleControls_Boost;

		private readonly InputAction m_VehicleControls_CruiseControl;

		public InputActionAsset asset { get; }

		public InputBinding? bindingMask
		{
			get
			{
				return asset.bindingMask;
			}
			set
			{
				asset.bindingMask = value;
			}
		}

		public ReadOnlyArray<InputDevice>? devices
		{
			get
			{
				return asset.devices;
			}
			set
			{
				asset.devices = value;
			}
		}

		public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

		public IEnumerable<InputBinding> bindings => asset.bindings;

		public VehicleControlsActions VehicleControls => new VehicleControlsActions(this);

		public VehicleInputActions()
		{
			asset = InputActionAsset.FromJson("{\n    \"version\": 1,\n    \"name\": \"VehicleInputActions\",\n    \"maps\": [\n        {\n            \"name\": \"Vehicle Controls\",\n            \"id\": \"200a0048-834b-4c46-8e58-cb0180a3f09b\",\n            \"actions\": [\n                {\n                    \"name\": \"Steering\",\n                    \"type\": \"Value\",\n                    \"id\": \"4c14d84a-48f6-429e-9111-d009cff86527\",\n                    \"expectedControlType\": \"Axis\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Throttle\",\n                    \"type\": \"Value\",\n                    \"id\": \"067e3728-8c0e-4c68-8b07-765ef5a0b2ff\",\n                    \"expectedControlType\": \"Axis\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Brakes\",\n                    \"type\": \"Value\",\n                    \"id\": \"063dcbbf-0a3c-4282-90a5-ec46c6b1db95\",\n                    \"expectedControlType\": \"Axis\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Clutch\",\n                    \"type\": \"Value\",\n                    \"id\": \"036104a2-f1da-429a-b3bf-75c4e539a58d\",\n                    \"expectedControlType\": \"Axis\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Handbrake\",\n                    \"type\": \"Value\",\n                    \"id\": \"6502904b-df3b-4a12-b9ca-b365d43db960\",\n                    \"expectedControlType\": \"Axis\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"EngineStartStop\",\n                    \"type\": \"Button\",\n                    \"id\": \"aa0a9858-ed3f-472c-96f9-4fdf0346726d\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ShiftUp\",\n                    \"type\": \"Button\",\n                    \"id\": \"4fa6a7ca-d894-4cd6-8592-7e34c66a8190\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ShiftDown\",\n                    \"type\": \"Button\",\n                    \"id\": \"180cb808-2f04-48c7-9551-a2859fff6752\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"LeftBlinker\",\n                    \"type\": \"Button\",\n                    \"id\": \"8f13ab7c-233f-4736-bbfa-c5f202240ad1\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"RightBlinker\",\n                    \"type\": \"Button\",\n                    \"id\": \"62880158-789b-43bb-bd49-c3bf25e94c87\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"LowBeamLights\",\n                    \"type\": \"Button\",\n                    \"id\": \"6536d934-38cf-48a6-afa1-16d6ed8f421c\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"HighBeamLights\",\n                    \"type\": \"Button\",\n                    \"id\": \"8af605a2-5581-4c32-9e35-2f80d0250a3e\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"HazardLights\",\n                    \"type\": \"Button\",\n                    \"id\": \"4e80b995-afae-4eb7-bd57-c2685a0c4388\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ExtraLights\",\n                    \"type\": \"Button\",\n                    \"id\": \"47c64239-6f45-41be-ba39-f8b1966b4170\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"TrailerAttachDetach\",\n                    \"type\": \"Button\",\n                    \"id\": \"d1143207-7243-4236-95d0-54b07f8caaf1\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Horn\",\n                    \"type\": \"Button\",\n                    \"id\": \"2a4bc293-16f8-47c1-8532-bc82b3905f77\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ShiftIntoR1\",\n                    \"type\": \"Button\",\n                    \"id\": \"fdf654af-5894-4876-9565-8e64e1f53efa\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ShiftInto0\",\n                    \"type\": \"Button\",\n                    \"id\": \"2ee85004-4812-4a6a-bb27-7c535a276c1a\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ShiftInto1\",\n                    \"type\": \"Button\",\n                    \"id\": \"b6f6becb-e7c2-4a15-8288-797cf992242c\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ShiftInto2\",\n                    \"type\": \"Button\",\n                    \"id\": \"4e82356c-b972-494a-9c0b-6031cd291630\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ShiftInto3\",\n                    \"type\": \"Button\",\n                    \"id\": \"aae19f07-e299-427a-8033-23a590c791d2\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ShiftInto4\",\n                    \"type\": \"Button\",\n                    \"id\": \"4e5032a3-39df-4dc8-a307-485c7a996b50\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ShiftInto5\",\n                    \"type\": \"Button\",\n                    \"id\": \"93decc9e-67a4-4d2e-a2aa-02cae173ffbf\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ShiftInto6\",\n                    \"type\": \"Button\",\n                    \"id\": \"8a253467-ad3e-4c6c-b0dd-fc030cf1db5c\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ShiftInto7\",\n                    \"type\": \"Button\",\n                    \"id\": \"cf66f6fe-1e63-45fc-a7dc-732882ca95fa\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"ShiftInto8\",\n                    \"type\": \"Button\",\n                    \"id\": \"4e7aa765-fdc6-4098-b90d-a2017111fafd\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"FlipOver\",\n                    \"type\": \"Button\",\n                    \"id\": \"238902b2-609f-4842-bd46-b5b15a8bd829\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Boost\",\n                    \"type\": \"Button\",\n                    \"id\": \"6de7528e-5f55-46a1-8fc6-bd19214b263c\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"CruiseControl\",\n                    \"type\": \"Button\",\n                    \"id\": \"c2c193e5-0ba5-4cdc-a189-84ccae17d118\",\n                    \"expectedControlType\": \"\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"\",\n                    \"id\": \"35b97d96-fd20-4097-8fb3-e4a275703cfd\",\n                    \"path\": \"<Keyboard>/e\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"EngineStartStop\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"3cb1f792-d862-4891-8e4f-156d57a4829e\",\n                    \"path\": \"<Keyboard>/r\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ShiftUp\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"dc3a7a67-1f90-4e7c-b07c-18f1dc7d6902\",\n                    \"path\": \"<Gamepad>/rightShoulder\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ShiftUp\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"6c4a4e7e-d035-447e-aaae-a5fb5f586cce\",\n                    \"path\": \"<Keyboard>/f\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ShiftDown\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"b3d427b4-5535-4cde-93b8-39952f11604f\",\n                    \"path\": \"<Gamepad>/leftShoulder\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ShiftDown\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c80687f6-73d6-4d12-a7ec-bfd5f70b9c1f\",\n                    \"path\": \"<Keyboard>/z\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"LeftBlinker\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"685cee63-e7f3-4a07-a8e3-bf34b04f0b47\",\n                    \"path\": \"<Keyboard>/x\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"RightBlinker\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"d2ec8fbe-9683-4987-931d-57ea5713d264\",\n                    \"path\": \"<Keyboard>/l\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"LowBeamLights\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c49dc91a-be6d-4a78-ae0c-cfd55da21347\",\n                    \"path\": \"<Gamepad>/buttonNorth\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"LowBeamLights\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"ba07cb72-8b32-40a6-b7b7-49fda5a5696a\",\n                    \"path\": \"<Keyboard>/k\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"HighBeamLights\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"47963c3f-83fc-444f-be98-2c2ad7dd5898\",\n                    \"path\": \"<Keyboard>/j\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"HazardLights\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c0f9f750-4f64-4408-9d29-295d1fd9c54e\",\n                    \"path\": \"<Keyboard>/semicolon\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ExtraLights\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"95f029b5-d826-4702-888f-47f7e793f787\",\n                    \"path\": \"<Keyboard>/t\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"TrailerAttachDetach\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"cbddc214-430c-4d0b-8003-11f10876d005\",\n                    \"path\": \"<Gamepad>/buttonWest\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"TrailerAttachDetach\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"67c9fe69-ff40-4fe2-8412-6d8f476f5d93\",\n                    \"path\": \"<Keyboard>/h\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Horn\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"81844531-f08f-4aa7-8e3f-26f755bc62f3\",\n                    \"path\": \"<Keyboard>/minus\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ShiftIntoR1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c2489f77-0bdb-4561-a6b7-45708fd8b7dc\",\n                    \"path\": \"<Keyboard>/0\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ShiftInto0\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c57a7e33-e49e-4bd9-b5a1-a33861f506d9\",\n                    \"path\": \"<Keyboard>/1\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ShiftInto1\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"abcd6d75-1316-4eda-87e5-9644bd935300\",\n                    \"path\": \"<Keyboard>/2\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ShiftInto2\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"669d3a9c-c9fe-42f5-9057-8f0cb31e0b96\",\n                    \"path\": \"<Keyboard>/3\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ShiftInto3\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"d4d8b40a-ee7f-495f-be9a-20d75f9e8a04\",\n                    \"path\": \"<Keyboard>/4\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ShiftInto4\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"a2c47890-4ff7-4795-8d55-20fb2e40a543\",\n                    \"path\": \"<Keyboard>/5\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ShiftInto5\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"aeff3f22-e375-486e-a67e-b88f1bd384e5\",\n                    \"path\": \"<Keyboard>/6\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ShiftInto6\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"4e7e74b3-942e-4649-b2d3-8bb7d139bf5e\",\n                    \"path\": \"<Keyboard>/7\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ShiftInto7\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"ac654d38-d65d-40e4-9796-8c17eede2112\",\n                    \"path\": \"<Keyboard>/8\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"ShiftInto8\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"074547a7-a3ea-4b53-a88f-599e9a8004b0\",\n                    \"path\": \"<Keyboard>/m\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"FlipOver\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"76b93c66-3dac-4d90-ac1f-aae6a399a31e\",\n                    \"path\": \"<Keyboard>/leftShift\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Boost\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"68e6d03c-1dd8-4b1b-9a6e-bbe32edef279\",\n                    \"path\": \"<Gamepad>/buttonSouth\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Boost\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"86494961-cebe-49ae-bbfe-950e9a17c5f9\",\n                    \"path\": \"<Keyboard>/n\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"CruiseControl\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"Keyboard\",\n                    \"id\": \"1bff6693-e128-4d74-ad2f-ad4e229608a8\",\n                    \"path\": \"1DAxis\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Steering\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"Negative\",\n                    \"id\": \"6a4beedd-9d33-48f8-ab0b-2fa4835b56d5\",\n                    \"path\": \"<Keyboard>/a\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Steering\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Positive\",\n                    \"id\": \"3ce20f4e-6e04-4810-b8c6-829ced054390\",\n                    \"path\": \"<Keyboard>/d\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Steering\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Gamepad\",\n                    \"id\": \"acf664f4-dde0-4367-8e3a-7fbdf293bc0c\",\n                    \"path\": \"1DAxis\",\n                    \"interactions\": \"\",\n                    \"processors\": \"AxisDeadzone(min=0.005,max=1)\",\n                    \"groups\": \"\",\n                    \"action\": \"Steering\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"negative\",\n                    \"id\": \"0332ccc4-b818-41e6-8455-330ec56c13de\",\n                    \"path\": \"<Gamepad>/leftStick/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Steering\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"positive\",\n                    \"id\": \"89725913-86df-4c0b-893e-7dab71609463\",\n                    \"path\": \"<Gamepad>/leftStick/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Steering\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Keyboard\",\n                    \"id\": \"866afad4-aa6f-444a-8122-260b6292b1d2\",\n                    \"path\": \"1DAxis\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Handbrake\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"positive\",\n                    \"id\": \"1ca21924-c96b-474a-a54b-b585c2e71ec2\",\n                    \"path\": \"<Keyboard>/space\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Handbrake\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Gamepad\",\n                    \"id\": \"e484e077-2222-4195-b4c0-92d2608ae41c\",\n                    \"path\": \"1DAxis\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Handbrake\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"positive\",\n                    \"id\": \"2d33c1e7-80af-4289-b8fa-c30c539b46bf\",\n                    \"path\": \"<Gamepad>/buttonEast\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Handbrake\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Keyboard\",\n                    \"id\": \"68c44959-158f-4a90-a301-94ab2339beee\",\n                    \"path\": \"1DAxis\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Throttle\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"positive\",\n                    \"id\": \"8601f7ae-fd79-42d3-bd51-c0054d12b6f7\",\n                    \"path\": \"<Keyboard>/w\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Throttle\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Gamepad\",\n                    \"id\": \"405ba6f9-74dd-4b74-8687-7dda1bc905d3\",\n                    \"path\": \"1DAxis\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Throttle\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"positive\",\n                    \"id\": \"7d163169-2e04-49bb-8dad-3fa803f4201e\",\n                    \"path\": \"<Gamepad>/rightTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Throttle\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Keyboard\",\n                    \"id\": \"079d6648-edd9-46d8-ae9b-ab8fced71ba4\",\n                    \"path\": \"1DAxis\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Brakes\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"positive\",\n                    \"id\": \"8d444115-e1a0-4c2a-864c-fbd63db53c90\",\n                    \"path\": \"<Keyboard>/s\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Brakes\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"Gamepad\",\n                    \"id\": \"4c6d18cf-e9dc-4ffd-a439-ed257cb0c16a\",\n                    \"path\": \"1DAxis\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Brakes\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"positive\",\n                    \"id\": \"cf160faf-0e28-4998-94ee-ce086b4b76c5\",\n                    \"path\": \"<Gamepad>/leftTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Brakes\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                }\n            ]\n        }\n    ],\n    \"controlSchemes\": []\n}");
			m_VehicleControls = asset.FindActionMap("Vehicle Controls", throwIfNotFound: true);
			m_VehicleControls_Steering = m_VehicleControls.FindAction("Steering", throwIfNotFound: true);
			m_VehicleControls_Throttle = m_VehicleControls.FindAction("Throttle", throwIfNotFound: true);
			m_VehicleControls_Brakes = m_VehicleControls.FindAction("Brakes", throwIfNotFound: true);
			m_VehicleControls_Clutch = m_VehicleControls.FindAction("Clutch", throwIfNotFound: true);
			m_VehicleControls_Handbrake = m_VehicleControls.FindAction("Handbrake", throwIfNotFound: true);
			m_VehicleControls_EngineStartStop = m_VehicleControls.FindAction("EngineStartStop", throwIfNotFound: true);
			m_VehicleControls_ShiftUp = m_VehicleControls.FindAction("ShiftUp", throwIfNotFound: true);
			m_VehicleControls_ShiftDown = m_VehicleControls.FindAction("ShiftDown", throwIfNotFound: true);
			m_VehicleControls_LeftBlinker = m_VehicleControls.FindAction("LeftBlinker", throwIfNotFound: true);
			m_VehicleControls_RightBlinker = m_VehicleControls.FindAction("RightBlinker", throwIfNotFound: true);
			m_VehicleControls_LowBeamLights = m_VehicleControls.FindAction("LowBeamLights", throwIfNotFound: true);
			m_VehicleControls_HighBeamLights = m_VehicleControls.FindAction("HighBeamLights", throwIfNotFound: true);
			m_VehicleControls_HazardLights = m_VehicleControls.FindAction("HazardLights", throwIfNotFound: true);
			m_VehicleControls_ExtraLights = m_VehicleControls.FindAction("ExtraLights", throwIfNotFound: true);
			m_VehicleControls_TrailerAttachDetach = m_VehicleControls.FindAction("TrailerAttachDetach", throwIfNotFound: true);
			m_VehicleControls_Horn = m_VehicleControls.FindAction("Horn", throwIfNotFound: true);
			m_VehicleControls_ShiftIntoR1 = m_VehicleControls.FindAction("ShiftIntoR1", throwIfNotFound: true);
			m_VehicleControls_ShiftInto0 = m_VehicleControls.FindAction("ShiftInto0", throwIfNotFound: true);
			m_VehicleControls_ShiftInto1 = m_VehicleControls.FindAction("ShiftInto1", throwIfNotFound: true);
			m_VehicleControls_ShiftInto2 = m_VehicleControls.FindAction("ShiftInto2", throwIfNotFound: true);
			m_VehicleControls_ShiftInto3 = m_VehicleControls.FindAction("ShiftInto3", throwIfNotFound: true);
			m_VehicleControls_ShiftInto4 = m_VehicleControls.FindAction("ShiftInto4", throwIfNotFound: true);
			m_VehicleControls_ShiftInto5 = m_VehicleControls.FindAction("ShiftInto5", throwIfNotFound: true);
			m_VehicleControls_ShiftInto6 = m_VehicleControls.FindAction("ShiftInto6", throwIfNotFound: true);
			m_VehicleControls_ShiftInto7 = m_VehicleControls.FindAction("ShiftInto7", throwIfNotFound: true);
			m_VehicleControls_ShiftInto8 = m_VehicleControls.FindAction("ShiftInto8", throwIfNotFound: true);
			m_VehicleControls_FlipOver = m_VehicleControls.FindAction("FlipOver", throwIfNotFound: true);
			m_VehicleControls_Boost = m_VehicleControls.FindAction("Boost", throwIfNotFound: true);
			m_VehicleControls_CruiseControl = m_VehicleControls.FindAction("CruiseControl", throwIfNotFound: true);
		}

		~VehicleInputActions()
		{
		}

		public void Dispose()
		{
			UnityEngine.Object.Destroy(asset);
		}

		public bool Contains(InputAction action)
		{
			return asset.Contains(action);
		}

		public IEnumerator<InputAction> GetEnumerator()
		{
			return asset.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Enable()
		{
			asset.Enable();
		}

		public void Disable()
		{
			asset.Disable();
		}

		public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
		{
			return asset.FindAction(actionNameOrId, throwIfNotFound);
		}

		public int FindBinding(InputBinding bindingMask, out InputAction action)
		{
			return asset.FindBinding(bindingMask, out action);
		}
	}
}
