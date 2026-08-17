using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.Trailer
{
	[Serializable]
	[DisallowMultipleComponent]
	public class TrailerModuleWrapper : ModuleWrapper
	{
		public TrailerModule module = new TrailerModule();

		private VehicleController _vehicleController;

		private void Awake()
		{
			_vehicleController = GetComponentInParent<VehicleController>();
			if (_vehicleController == null)
			{
				Debug.LogError("No VehicleController attached to the TrailerModule " + base.name);
				return;
			}
			if (module.attachmentPoint == null)
			{
				Debug.LogError(base.name + ": TrailerModule attachmentPoint is null.");
				return;
			}
			if (module.attachmentPoint.GetComponent<SphereCollider>() == null)
			{
				SphereCollider sphereCollider = module.attachmentPoint.gameObject.AddComponent<SphereCollider>();
				sphereCollider.radius = module.attachmentTriggerRadius;
				sphereCollider.isTrigger = true;
				sphereCollider.gameObject.layer = module.attachmentLayer;
			}
			module.vehicleController = _vehicleController;
		}

		public override VehicleComponent GetModule()
		{
			return module;
		}

		public override void SetModule(VehicleComponent module)
		{
			this.module = module as TrailerModule;
		}

		private void OnJointBreak(float breakForce)
		{
			Debug.Log("Trailer: Trailer hitch joint broke with force: " + breakForce);
			module.trailerHitch?.DetachTrailer(_vehicleController);
		}

		public void OnDrawGizmos()
		{
			Gizmos.DrawSphere(module.attachmentPoint.position, 0.1f);
		}
	}
}
