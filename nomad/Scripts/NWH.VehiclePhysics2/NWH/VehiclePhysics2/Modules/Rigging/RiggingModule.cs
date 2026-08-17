using System;
using System.Collections.Generic;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.Rigging
{
	[Serializable]
	public class RiggingModule : VehicleComponent
	{
		public List<Bone> bones = new List<Bone>();

		protected override void VC_Initialize()
		{
			foreach (Bone bone in bones)
			{
				bone.Initialize();
			}
			base.VC_Initialize();
		}

		public override void VC_Update()
		{
			base.VC_Update();
			Vector3 forward = vehicleController.vehicleTransform.forward;
			Vector3 up = vehicleController.vehicleTransform.up;
			foreach (Bone bone in bones)
			{
				bone.Update(forward, up);
			}
		}
	}
}
