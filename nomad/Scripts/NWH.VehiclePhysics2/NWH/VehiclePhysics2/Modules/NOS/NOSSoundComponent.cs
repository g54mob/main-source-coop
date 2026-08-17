using System;
using NWH.VehiclePhysics2.Sound.SoundComponents;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Modules.NOS
{
	[Serializable]
	public class NOSSoundComponent : SoundComponent
	{
		[NonSerialized]
		public NOSModule nosModule;

		public override GameObject ContainerGO => vehicleController.soundManager.engineSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.engineMixerGroup;

		public override int Priority => 90;

		public override bool InitLoop => true;

		public override void VC_Update()
		{
			base.VC_Update();
			if (nosModule.IsUsingNOS)
			{
				SetVolume(baseVolume);
				if (!source.isPlaying)
				{
					Play();
				}
			}
			else
			{
				Stop();
			}
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			baseVolume = 0.2f;
			if (base.Clip == null)
			{
				base.Clip = Resources.Load("NWH Vehicle Physics 2/Defaults/Sound/NOS") as AudioClip;
				if (base.Clip == null)
				{
					Debug.LogWarning("Audio Clip for sound component " + GetType().Name + "  from resources. Source will not play.");
				}
			}
		}
	}
}
