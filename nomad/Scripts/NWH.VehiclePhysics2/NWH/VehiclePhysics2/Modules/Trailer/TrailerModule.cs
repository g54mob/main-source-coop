using System;
using UnityEngine;
using UnityEngine.Events;

namespace NWH.VehiclePhysics2.Modules.Trailer
{
	[Serializable]
	public class TrailerModule : VehicleComponent
	{
		[Tooltip("True if object is trailer and is attached to a towing vehicle and also true if towing vehicle and has trailer\r\nattached.")]
		public bool attached;

		[Tooltip("If the vehicle is a trailer, this is the object placed at the point at which it will connect to the towing vehicle. If the vehicle is towing, this is the object placed at point at which trailer will be coneected.")]
		public Transform attachmentPoint;

		public int attachmentLayer;

		public float attachmentTriggerRadius = 0.2f;

		public UnityEvent onAttach = new UnityEvent();

		public UnityEvent onDetach = new UnityEvent();

		[Tooltip("    Should the trailer input states be reset when trailer is detached?")]
		public bool resetInputStatesOnDetach = true;

		[Tooltip("If enabled the trailer will keep in same gear as the tractor, assuming powertrain on trailer is enabled.")]
		public bool synchronizeGearShifts;

		[Tooltip("    Object that will be disabled when trailer is attached and disabled when trailer is detached.")]
		public GameObject trailerStand;

		[NonSerialized]
		public TrailerHitchModule trailerHitch;

		protected override void VC_Initialize()
		{
			vehicleController.input.autoSetInput = false;
			base.VC_Initialize();
		}

		public override void VC_FixedUpdate()
		{
			base.VC_FixedUpdate();
			if (trailerHitch != null && attached)
			{
				vehicleController.powertrain.transmission.Gear = trailerHitch.vehicleController.powertrain.transmission.Gear;
				if (synchronizeGearShifts)
				{
					vehicleController.powertrain.transmission.ShiftInto(trailerHitch.vehicleController.powertrain.transmission.Gear);
				}
			}
		}

		public void OnAttach(TrailerHitchModule trailerHitch)
		{
			this.trailerHitch = trailerHitch;
			this.trailerHitch.vehicleController.onEnable.AddListener(EnableTrailer);
			this.trailerHitch.vehicleController.onDisable.AddListener(DisableTrailer);
			if (trailerHitch.vehicleController.enabled)
			{
				EnableTrailer();
			}
			else
			{
				DisableTrailer();
			}
			vehicleController.input.autoSetInput = false;
			if (trailerStand != null)
			{
				trailerStand.SetActive(value: false);
			}
			attached = true;
			onAttach.Invoke();
		}

		public void OnDetach()
		{
			if (resetInputStatesOnDetach)
			{
				vehicleController.input.states.Reset();
			}
			vehicleController.input.autoSetInput = false;
			if (trailerStand != null)
			{
				trailerStand.SetActive(value: true);
			}
			trailerHitch.vehicleController.onEnable.RemoveListener(EnableTrailer);
			trailerHitch.vehicleController.onDisable.RemoveListener(DisableTrailer);
			trailerHitch = null;
			attached = false;
			onDetach.Invoke();
			DisableTrailer();
		}

		private void EnableTrailer()
		{
			if (vehicleController != null)
			{
				vehicleController.enabled = true;
			}
		}

		private void DisableTrailer()
		{
			if (vehicleController != null)
			{
				vehicleController.enabled = false;
			}
		}
	}
}
