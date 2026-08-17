using System;
using System.Collections.Generic;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;

namespace NWH.VehiclePhysics2.Effects
{
	[Serializable]
	public class SurfaceParticleManager : Effect
	{
		[Range(0f, 5f)]
		[Tooltip("How much will lateral slip contribute to the particle emission.\r\nIgnored when particle type for the surface is set to other than Smoke.")]
		public float lateralSlipParticleCoeff = 1f;

		[Range(0f, 5f)]
		[Tooltip("How much will longitudinal slip contribute to the particle emission.\r\nIgnored when particle type for the surface is set to other than Smoke.")]
		public float longitudinalSlipParticleCoeff = 1f;

		[Range(0f, 2f)]
		[Tooltip("Particle size multiplier specific to this vehicle.\r\nUse to adjust particle size on per-vehicle basis.\r\nFor global particle size adjustment for individual surfaces check SurfacePresets.")]
		public float particleSizeCoeff = 1f;

		[Range(0f, 2f)]
		[Tooltip("Emission rate multiplier specific to this vehicle.\r\nUse to adjust emission on per-vehicle basis.\r\nFor global emission adjustment for individual surfaces check SurfacePresets.")]
		public float emissionRateCoeff = 1f;

		[Tooltip("When enabled the particle system will either emit or not emit, with no in-between. Also removes any smoothing.")]
		public bool binaryEmission;

		[SerializeField]
		private List<SurfaceParticleSystem> particleSystems = new List<SurfaceParticleSystem>();

		protected override void VC_Initialize()
		{
			for (int i = 0; i < vehicleController.powertrain.wheels.Count; i++)
			{
				WheelComponent wheelComponent = vehicleController.powertrain.wheels[i];
				SurfaceParticleSystem surfaceParticleSystem = new SurfaceParticleSystem();
				surfaceParticleSystem.Initialize(vehicleController, wheelComponent);
				particleSystems.Add(surfaceParticleSystem);
			}
			base.VC_Initialize();
		}

		public override void VC_Update()
		{
			base.VC_Update();
			int count = particleSystems.Count;
			for (int i = 0; i < count; i++)
			{
				SurfaceParticleSystem surfaceParticleSystem = particleSystems[i];
				surfaceParticleSystem.longitudinalSlipCoeff = longitudinalSlipParticleCoeff;
				surfaceParticleSystem.lateralSlipCoeff = lateralSlipParticleCoeff;
				surfaceParticleSystem.particleSizeCoeff = particleSizeCoeff;
				surfaceParticleSystem.emissionRateCoeff = emissionRateCoeff;
				surfaceParticleSystem.binaryEmission = binaryEmission;
			}
		}

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				int count = particleSystems.Count;
				for (int i = 0; i < count; i++)
				{
					particleSystems[i].Enable();
				}
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				int count = particleSystems.Count;
				for (int i = 0; i < count; i++)
				{
					particleSystems[i].Disable();
				}
				return true;
			}
			return false;
		}
	}
}
