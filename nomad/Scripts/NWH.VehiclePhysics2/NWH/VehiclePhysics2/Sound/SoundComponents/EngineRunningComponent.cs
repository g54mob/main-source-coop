using System;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class EngineRunningComponent : SoundComponent
	{
		[Range(0f, 1f)]
		[Tooltip("    Distortion at maximum engine load.")]
		public float maxDistortion = 0.4f;

		[Range(0f, 4f)]
		[Tooltip("    Pitch added to the base engine pitch depending on engine RPM.")]
		public float pitchRange = 2f;

		public float pitchOffset = 0.2f;

		[Range(0f, 1f)]
		[Tooltip("    Smoothing of engine volume.")]
		public float smoothing = 0.05f;

		[Range(0f, 1f)]
		[Tooltip("    Volume added to the base engine volume depending on engine state.")]
		public float volumeRange = 0.1f;

		private float _volume;

		private float _volumeVelocity;

		private float _distortion;

		private float _distortionVelocity;

		public override GameObject ContainerGO => vehicleController.soundManager.engineSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.engineMixerGroup;

		public override int Priority => 10;

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
				Stop();
				return true;
			}
			return false;
		}

		public override void VC_Update()
		{
			base.VC_Update();
			EngineComponent engine = vehicleController.powertrain.engine;
			float pitch = engine.RPMPercent * pitchRange + pitchOffset;
			SetPitch(pitch);
			float target = (vehicleController.powertrain.engine.revLimiterActive ? 0.5f : engine.ThrottlePosition) * maxDistortion;
			_distortion = Mathf.SmoothDamp(_distortion, target, ref _distortionVelocity, smoothing);
			source.outputAudioMixerGroup.audioMixer.SetFloat("engineDistortion", _distortion);
			float num = baseVolume;
			num += engine.Load * volumeRange;
			num -= _distortion;
			num = Mathf.Clamp(num, baseVolume, 2f);
			_volume = Mathf.SmoothDamp(_volume, num, ref _volumeVelocity, smoothing);
			SetVolume(_volume);
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			baseVolume = 0.5f;
			volumeRange = 0.4f;
			pitchRange = 1.8f;
			if (base.Clip == null)
			{
				AddDefaultClip("EngineRunning");
			}
		}
	}
}
