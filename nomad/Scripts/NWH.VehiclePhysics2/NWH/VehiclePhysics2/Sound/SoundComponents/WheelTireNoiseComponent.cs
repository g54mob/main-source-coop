using System;
using NWH.VehiclePhysics2.GroundDetection;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class WheelTireNoiseComponent : SoundComponent
	{
		private float _prevPitch;

		private float _prevVolume;

		public override GameObject ContainerGO => vehicleController.soundManager.otherSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.surfaceNoiseMixerGroup;

		public override int Priority => 90;

		public override bool InitPlayOnAwake => true;

		public override bool InitLoop => true;

		public override bool InitializeWithNoClips => true;

		public override void VC_Update()
		{
			base.VC_Update();
			if (!vehicleController.groundDetection.state.isEnabled)
			{
				return;
			}
			float num = 0f;
			float num2 = 0f;
			SurfacePreset dominantSurfacePreset = vehicleController.groundDetection.DominantSurfacePreset;
			if (dominantSurfacePreset == null || dominantSurfacePreset.surfaceSoundClip == null || !dominantSurfacePreset.playSurfaceSounds)
			{
				Stop();
				return;
			}
			source.clip = dominantSurfacePreset.surfaceSoundClip;
			for (int i = 0; i < vehicleController.powertrain.wheelCount; i++)
			{
				WheelComponent wheelComponent = vehicleController.powertrain.wheels[i];
				if (wheelComponent.wheelUAPI.IsGrounded)
				{
					float num3 = 1f;
					if (dominantSurfacePreset.slipSensitiveSurfaceSound)
					{
						num3 = wheelComponent.wheelUAPI.NormalizedLateralSlip / vehicleController.longitudinalSlipThreshold;
						num3 = ((num3 < 0f) ? 0f : ((num3 > 1f) ? 1f : num3));
					}
					float num4 = vehicleController.Speed * 0.03f;
					num4 = ((num4 < 0f) ? 0f : ((num4 > 1f) ? 1f : num4));
					float num5 = dominantSurfacePreset.surfaceSoundVolume * num3 * num4;
					num5 = ((num5 < 0f) ? 0f : ((num5 > 1f) ? 1f : num5));
					num = Mathf.Max(num, num5);
					float b = dominantSurfacePreset.surfaceSoundPitch * 0.5f + num4;
					num2 = Mathf.Max(num2, b);
				}
			}
			num = Mathf.Lerp(_prevVolume, num, vehicleController.deltaTime * 20f);
			SetVolume(num);
			_prevVolume = num;
			num2 = Mathf.Lerp(_prevPitch, num2, vehicleController.deltaTime * 20f);
			SetPitch(num2);
			_prevPitch = num2;
			if (num < 0.01f && source.isPlaying)
			{
				Stop();
			}
			else if (!source.isPlaying)
			{
				Play();
			}
		}
	}
}
