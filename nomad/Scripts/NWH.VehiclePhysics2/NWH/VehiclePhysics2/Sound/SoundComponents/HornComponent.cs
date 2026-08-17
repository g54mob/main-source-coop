using System;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class HornComponent : SoundComponent
	{
		public override GameObject ContainerGO => vehicleController.soundManager.otherSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.otherMixerGroup;

		public override bool InitLoop => true;

		public override int Priority => 200;

		public override void VC_Update()
		{
			base.VC_Update();
			if (vehicleController.input.Horn)
			{
				SetVolume(baseVolume);
				if (!source.isPlaying)
				{
					Play();
				}
			}
			else if (source.isPlaying)
			{
				Stop();
			}
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			if (base.Clip == null)
			{
				AddDefaultClip("Horn");
			}
		}
	}
}
