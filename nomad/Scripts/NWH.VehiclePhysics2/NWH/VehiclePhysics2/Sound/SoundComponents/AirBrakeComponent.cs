using System;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class AirBrakeComponent : SoundComponent
	{
		[Tooltip("    Minimum time between two plays.")]
		public float minInterval = 4f;

		private float _timer;

		public override GameObject ContainerGO => vehicleController.soundManager.otherSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.otherMixerGroup;

		public override int Priority => 150;

		public override void VC_Update()
		{
			base.VC_Update();
			_timer += Time.deltaTime;
		}

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.brakes.onBrakesDeactivate.AddListener(PlayBrakeHiss);
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				vehicleController.brakes.onBrakesDeactivate.RemoveListener(PlayBrakeHiss);
				return true;
			}
			return false;
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			baseVolume = 0.1f;
			if (base.Clip == null)
			{
				AddDefaultClip("AirBrakes");
			}
		}

		public void PlayBrakeHiss()
		{
			if (!(_timer < minInterval) && vehicleController.powertrain.engine.IsRunning)
			{
				SetVolume(UnityEngine.Random.Range(0.8f, 1.2f) * baseVolume);
				if (!source.isPlaying)
				{
					PlayRandomClip();
				}
				_timer = 0f;
			}
		}
	}
}
