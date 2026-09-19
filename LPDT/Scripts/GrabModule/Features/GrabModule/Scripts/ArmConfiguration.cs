using System;
using UnityEngine;

namespace Features.GrabModule.Scripts
{
	[Serializable]
	public class ArmConfiguration
	{
		public ConfigurableJointMotion ZMotion;

		public ConfigurableJointMotion AngularYMotion;
	}
}
