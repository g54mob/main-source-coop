using System;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class EngineStartStopComponent : SoundComponent
	{
		[Range(0.1f, 2f)]
		public float pitch = 1f;

		public override GameObject ContainerGO => vehicleController.soundManager.engineSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.engineMixerGroup;

		public override int Priority => 50;

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.powertrain.engine.onStart.AddListener(PlayStarting);
				vehicleController.powertrain.engine.onStop.AddListener(PlayStopping);
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				vehicleController.powertrain.engine.onStart.RemoveListener(PlayStarting);
				vehicleController.powertrain.engine.onStop.RemoveListener(PlayStopping);
				return true;
			}
			return false;
		}

		public override void VC_Update()
		{
			base.VC_Update();
			if (vehicleController.powertrain.engine.starterActive)
			{
				float value = vehicleController.powertrain.engine.OutputRPM / vehicleController.powertrain.engine.idleRPM;
				value = Mathf.Clamp01(value) * pitch;
				SetPitch(value);
			}
		}

		public virtual void PlayStarting()
		{
			SetVolume(baseVolume);
			SetPitch(pitch);
			vehicleController.StartCoroutine(PlayForDurationCoroutine(0, vehicleController.powertrain.engine.startDuration));
		}

		public virtual void PlayStopping()
		{
			if (vehicleController.powertrain.engine.IsRunning)
			{
				SetVolume(baseVolume);
				SetPitch(pitch);
				if (source.enabled)
				{
					Play(1);
				}
			}
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			baseVolume = 0.2f;
			if (base.Clip == null)
			{
				AddDefaultClip("EngineStart");
			}
		}
	}
}
