using System;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class CrashComponent : SoundComponent
	{
		[Range(0f, 0.5f)]
		[Tooltip("Different random pitch in range [basePitch + (1 +- pitchRandomness)] is set each time a collision happens.")]
		public float pitchRandomness = 0.4f;

		[Range(0f, 5f)]
		[Tooltip("    Higher values result in collisions getting louder for the given collision velocity magnitude.")]
		public float velocityMagnitudeEffect = 1f;

		public override GameObject ContainerGO => vehicleController.soundManager.crashSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.otherMixerGroup;

		public override int Priority => 80;

		protected override void VC_Initialize()
		{
			CreateAndRegisterAudioSource(vehicleController.soundManager.otherMixerGroup, vehicleController.soundManager.crashSourceGO);
			base.VC_Initialize();
		}

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.onCollision.AddListener(PlayCollisionSound);
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				vehicleController.onCollision.RemoveListener(PlayCollisionSound);
				return true;
			}
			return false;
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			baseVolume = 0.4f;
			if (base.Clip == null)
			{
				AddDefaultClip("Crash");
			}
		}

		public void PlayCollisionSound(Collision collision)
		{
			if (base.IsActive && collision != null && collision.contacts.Length != 0)
			{
				vehicleController.soundManager.crashSourceGO.transform.position = collision.contacts[0].point;
				float value = Mathf.Clamp01(collision.relativeVelocity.magnitude * 0.2f * velocityMagnitudeEffect) * baseVolume;
				value = Mathf.Clamp01(value);
				float pitch = UnityEngine.Random.Range(1f - pitchRandomness, 1f + pitchRandomness);
				SetVolume(value);
				SetPitch(pitch);
				PlayRandomClip();
			}
		}
	}
}
