using System;
using System.Collections.Generic;
using NWH.Common.Vehicles;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class SuspensionBumpComponent : SoundComponent
	{
		private List<bool> wheelWasGrounded = new List<bool>();

		public override GameObject ContainerGO => vehicleController.soundManager.otherSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.otherMixerGroup;

		public override int Priority => 160;

		protected override void VC_Initialize()
		{
			wheelWasGrounded = new List<bool>();
			foreach (WheelComponent wheel in vehicleController.powertrain.wheels)
			{
				_ = wheel;
				wheelWasGrounded.Add(item: true);
			}
			base.VC_Initialize();
		}

		public override void VC_FixedUpdate()
		{
			base.VC_FixedUpdate();
			if (vehicleController.realtimeSinceStartup < 2f)
			{
				return;
			}
			for (int i = 0; i < vehicleController.powertrain.wheelCount; i++)
			{
				bool isGrounded = vehicleController.powertrain.wheels[i].wheelUAPI.IsGrounded;
				bool flag = wheelWasGrounded[i];
				if (isGrounded && !flag)
				{
					PlayBumpSound(vehicleController.powertrain.wheels[i].wheelUAPI);
				}
				wheelWasGrounded[i] = isGrounded;
			}
		}

		private void PlayBumpSound(WheelUAPI wheel)
		{
			float pitch = UnityEngine.Random.Range(0.7f, 1.3f);
			float volume = baseVolume * Mathf.Clamp01(wheel.Load / wheel.MaxLoad);
			SetPitch(pitch);
			SetVolume(volume);
			PlayRandomClip();
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			baseVolume = 0.4f;
			if (base.Clip == null)
			{
				AddDefaultClip("SuspensionBump");
			}
		}
	}
}
