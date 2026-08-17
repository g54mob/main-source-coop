using System;
using NWH.VehiclePhysics2.Effects;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class BlinkerComponent : SoundComponent
	{
		public override GameObject ContainerGO => vehicleController.soundManager.otherSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.otherMixerGroup;

		public override int Priority => 180;

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				if (vehicleController.effectsManager.lightsManager.leftBlinkers.lightSources.Count > 0)
				{
					LightSource lightSource = vehicleController.effectsManager.lightsManager.leftBlinkers.lightSources[0];
					lightSource.onLightTurnedOn.AddListener(PlayBlinkerOn);
					lightSource.onLightTurnedOff.AddListener(PlayBlinkerOff);
				}
				if (vehicleController.effectsManager.lightsManager.rightBlinkers.lightSources.Count > 0)
				{
					LightSource lightSource2 = vehicleController.effectsManager.lightsManager.rightBlinkers.lightSources[0];
					lightSource2.onLightTurnedOn.AddListener(PlayBlinkerOn);
					lightSource2.onLightTurnedOff.AddListener(PlayBlinkerOff);
				}
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				if (vehicleController.effectsManager.lightsManager.leftBlinkers.lightSources.Count > 0)
				{
					LightSource lightSource = vehicleController.effectsManager.lightsManager.leftBlinkers.lightSources[0];
					lightSource.onLightTurnedOn.RemoveListener(PlayBlinkerOn);
					lightSource.onLightTurnedOff.RemoveListener(PlayBlinkerOff);
				}
				if (vehicleController.effectsManager.lightsManager.rightBlinkers.lightSources.Count > 0)
				{
					LightSource lightSource2 = vehicleController.effectsManager.lightsManager.rightBlinkers.lightSources[0];
					lightSource2.onLightTurnedOn.RemoveListener(PlayBlinkerOn);
					lightSource2.onLightTurnedOff.RemoveListener(PlayBlinkerOff);
				}
				return true;
			}
			return false;
		}

		private void PlayBlinkerOn()
		{
			SetVolume(baseVolume);
			Play(0);
		}

		private void PlayBlinkerOff()
		{
			SetVolume(baseVolume);
			Play(1);
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			baseVolume = 0.8f;
			if (base.Clip == null)
			{
				AddDefaultClip("BlinkerOn");
				AddDefaultClip("BlinkerOff");
			}
		}
	}
}
