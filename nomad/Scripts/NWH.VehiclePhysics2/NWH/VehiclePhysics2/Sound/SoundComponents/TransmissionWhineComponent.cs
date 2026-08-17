using System;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class TransmissionWhineComponent : SoundComponent
	{
		[Tooltip("Maximum speed value [m/s] of the vehicle at which the pitch will be at the top end of the pitchRange.")]
		public float maxSpeed = 80f;

		[Range(0f, 1f)]
		[Tooltip("    Volume coefficient when transmission is not under load.")]
		public float volumeRange = 0.2f;

		[Tooltip("    Starting pitch value.")]
		public float basePitch = 0.2f;

		[Range(0f, 5f)]
		[Tooltip("    Pitch range that will be added to the base pitch depending on transmission state.")]
		public float pitchRange = 0.7f;

		public override GameObject ContainerGO => vehicleController.soundManager.transmissionSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.transmissionMixerGroup;

		public override bool InitPlayOnAwake => false;

		public override int Priority => 90;

		public override bool InitLoop => true;

		public override void VC_Update()
		{
			base.VC_Update();
			float speed = vehicleController.Speed;
			float num = basePitch;
			if (vehicleController.powertrain.transmission.Gear != 0)
			{
				num += Mathf.Clamp01(speed / maxSpeed) * pitchRange;
			}
			SetPitch(num);
			float num2 = Mathf.Clamp01(Mathf.Abs(speed) * 0.8f);
			float num3 = baseVolume + vehicleController.powertrain.engine.Load * volumeRange;
			num3 *= num2;
			SetVolume(num3);
			if (num3 > 0.01f)
			{
				Play();
			}
			else
			{
				Stop();
			}
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			baseVolume = 0.005f;
			volumeRange = 0.005f;
			if (base.Clip == null)
			{
				AddDefaultClip("TransmissionWhine");
			}
		}
	}
}
