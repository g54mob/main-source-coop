using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace NWH.VehiclePhysics2.Effects
{
	[Serializable]
	public class EffectManager : ManagerVehicleComponent
	{
		public ExhaustFlash exhaustFlash = new ExhaustFlash();

		public ExhaustSmoke exhaustSmoke = new ExhaustSmoke();

		[FormerlySerializedAs("lights")]
		public LightsMananger lightsManager = new LightsMananger();

		[FormerlySerializedAs("skidmarks")]
		public SkidmarkManager skidmarkManager = new SkidmarkManager();

		public SurfaceParticleManager surfaceParticleManager = new SurfaceParticleManager();

		protected override void FillComponentList()
		{
			_components = new List<VehicleComponent> { exhaustFlash, exhaustSmoke, lightsManager, skidmarkManager, surfaceParticleManager };
		}
	}
}
