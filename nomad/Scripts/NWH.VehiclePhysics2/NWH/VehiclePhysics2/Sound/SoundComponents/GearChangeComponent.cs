using System;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class GearChangeComponent : SoundComponent
	{
		public override GameObject ContainerGO => vehicleController.soundManager.transmissionSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.transmissionMixerGroup;

		public override int Priority => 160;

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.powertrain.transmission.onShift.AddListener(PlayShiftSound);
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				vehicleController.powertrain.transmission.onShift.RemoveListener(PlayShiftSound);
				return true;
			}
			return false;
		}

		private void PlayShiftSound()
		{
			if (vehicleController.powertrain.transmission.Gear != 0)
			{
				SetVolume(baseVolume);
				PlayRandomClip();
			}
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			baseVolume = 0.16f;
			if (base.Clip == null)
			{
				AddDefaultClip("GearChange");
			}
		}
	}
}
