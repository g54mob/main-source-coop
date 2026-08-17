using System;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class EngineFanComponent : SoundComponent
	{
		[Tooltip("Starting sound pitch at idle RPM.")]
		public float basePitch = 1f;

		[Range(0f, 4f)]
		[Tooltip("Pitch range, redline pitch equals basePitch + pitchRange.")]
		public float pitchRange = 0.5f;

		public override GameObject ContainerGO => vehicleController.soundManager.engineSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.engineMixerGroup;

		public override int Priority => 100;

		public override bool InitLoop => true;

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.powertrain.engine.onStart.AddListener(Play);
				vehicleController.powertrain.engine.onStop.AddListener(Stop);
				if (vehicleController.powertrain.engine.IsRunning)
				{
					Play();
				}
				else
				{
					Stop();
				}
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				vehicleController.powertrain.engine.onStart.RemoveListener(Play);
				vehicleController.powertrain.engine.onStop.RemoveListener(Stop);
				return true;
			}
			return false;
		}

		public override void VC_Update()
		{
			base.VC_Update();
			float rPMPercent = vehicleController.powertrain.engine.RPMPercent;
			SetVolume(rPMPercent * rPMPercent * baseVolume);
			SetPitch(basePitch + pitchRange * rPMPercent);
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			baseVolume = 0.05f;
			if (base.Clip == null)
			{
				AddDefaultClip("EngineFan");
			}
		}
	}
}
