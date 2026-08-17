using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class ReverseBeepComponent : SoundComponent
	{
		public bool beepOnNegativeVelocity = true;

		public bool beepOnReverseGear = true;

		private Coroutine _beepCoroutine;

		public override GameObject ContainerGO => vehicleController.soundManager.otherSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.otherMixerGroup;

		public override bool InitLoop => false;

		public override int Priority => 160;

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				_beepCoroutine = vehicleController.StartCoroutine(BeepCoroutine());
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				if (_beepCoroutine != null)
				{
					vehicleController.StopCoroutine(_beepCoroutine);
				}
				return true;
			}
			return false;
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			if (base.Clip == null)
			{
				AddDefaultClip("ReverseBeep");
			}
		}

		private IEnumerator BeepCoroutine()
		{
			while (true)
			{
				int gear = vehicleController.powertrain.transmission.Gear;
				bool num = beepOnReverseGear && gear < 0;
				bool flag = beepOnNegativeVelocity && vehicleController.LocalForwardVelocity < -0.2f;
				if (num || flag)
				{
					SetVolume(baseVolume);
					Play();
				}
				else
				{
					Stop();
				}
				yield return new WaitForSeconds(1f);
			}
		}
	}
}
