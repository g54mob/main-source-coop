using System;
using System.Collections.Generic;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.ModuleTemplate
{
	[Serializable]
	public class ModuleTemplate : VehicleComponent
	{
		[Range(0f, 1f)]
		[Tooltip("    Example float field.")]
		public float floatExample;

		[Tooltip("    Example list field.")]
		public List<int> listExample = new List<int>();

		protected override void VC_Initialize()
		{
			base.VC_Initialize();
		}

		public override void VC_Update()
		{
			base.VC_Update();
		}

		public override void VC_FixedUpdate()
		{
			base.VC_FixedUpdate();
		}
	}
}
