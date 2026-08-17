using System;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class TurboFlutterComponent : SoundComponent
	{
		public override GameObject ContainerGO => vehicleController.soundManager.engineSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.engineMixerGroup;

		public override int Priority => 120;

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.powertrain.engine.forcedInduction.onWastegateRelease.AddListener(PlayFlutterSound);
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				vehicleController.powertrain.engine.forcedInduction.onWastegateRelease.RemoveListener(PlayFlutterSound);
				return true;
			}
			return false;
		}

		private void PlayFlutterSound(float wastegateBoost)
		{
			if (!source.isPlaying)
			{
				float value = baseVolume * wastegateBoost * wastegateBoost * UnityEngine.Random.Range(0.7f, 1.3f);
				SetVolume(Mathf.Clamp01(value));
				PlayRandomClip();
			}
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			baseVolume = 0.006f;
			if (base.Clip == null)
			{
				AddDefaultClip("TurboFlutter");
			}
		}
	}
}
