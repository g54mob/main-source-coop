using System;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class TurboWhistleComponent : SoundComponent
	{
		[Range(0f, 5f)]
		[Tooltip("    Pitch range that will be added to the base pitch depending on turbos's RPM.")]
		public float pitchRange = 0.9f;

		public override GameObject ContainerGO => vehicleController.soundManager.engineSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.engineMixerGroup;

		public override int Priority => 40;

		public override bool InitLoop => true;

		public override void VC_Update()
		{
			base.VC_Update();
			if (vehicleController.powertrain.engine.IsRunning && vehicleController.powertrain.engine.forcedInduction.useForcedInduction)
			{
				SetVolume(Mathf.Clamp01(baseVolume * vehicleController.powertrain.engine.forcedInduction.boost * vehicleController.powertrain.engine.forcedInduction.boost));
				SetPitch(pitchRange * vehicleController.powertrain.engine.forcedInduction.boost);
				Play();
			}
			else if (source != null)
			{
				SetVolume(0f);
				Stop();
			}
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			baseVolume = 0.004f;
			pitchRange = 1f;
			if (base.Clip == null)
			{
				AddDefaultClip("TurboWhistle");
			}
		}
	}
}
