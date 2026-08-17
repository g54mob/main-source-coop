using System;
using NWH.VehiclePhysics2.GroundDetection;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class WheelSkidComponent : SoundComponent
	{
		private float _prevVolume;

		public override GameObject ContainerGO => vehicleController.soundManager.otherSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.surfaceNoiseMixerGroup;

		public override int Priority => 60;

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
			SurfacePreset dominantSurfacePreset = vehicleController.groundDetection.DominantSurfacePreset;
			if (dominantSurfacePreset != null && dominantSurfacePreset.skidSoundClip != null && dominantSurfacePreset.playSkidSounds)
			{
				source.clip = dominantSurfacePreset.skidSoundClip;
				for (int i = 0; i < vehicleController.powertrain.wheelCount; i++)
				{
					WheelComponent wheelComponent = vehicleController.powertrain.wheels[i];
					if (wheelComponent.wheelUAPI.IsGrounded && (wheelComponent.wheelUAPI.IsSkiddingLaterally || wheelComponent.wheelUAPI.IsSkiddingLongitudinally))
					{
						float num2 = Mathf.Clamp01(wheelComponent.wheelUAPI.NormalizedLateralSlip + wheelComponent.wheelUAPI.NormalizedLongitudinalSlip);
						float num3 = Mathf.Min(1f, vehicleController.Speed * 0.33f + wheelComponent.wheelUAPI.AngularVelocity * 0.05f);
						float num4 = num2 * dominantSurfacePreset.skidSoundVolume * num3;
						num = ((num > num4) ? num : num4);
					}
				}
				num = Mathf.Lerp(_prevVolume, num, vehicleController.deltaTime * 10f);
				SetVolume(num);
				_prevVolume = num;
				if (num < 0.01f && source.isPlaying)
				{
					Stop();
				}
				else if (!source.isPlaying)
				{
					Play();
				}
			}
			else
			{
				Stop();
			}
		}
	}
}
