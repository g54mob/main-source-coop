using System;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public class ExhaustPopComponent : SoundComponent
	{
		public enum PopSource
		{
			RevLimiter = 0,
			ExhaustFlash = 1
		}

		[Tooltip("The source for the pop trigger. \r\nIf ExhaustFlash is selected, ExhaustFlash effect needs to be set up for this to work.")]
		public PopSource popSource = PopSource.ExhaustFlash;

		[Tooltip("Each time there is an exhaust flash or rev limiter is hit, what is the chance of exhaust pop?")]
		public float popChance = 0.1f;

		[Tooltip("Should pops happen randomly when the vehicle is decelerating with throttle released.")]
		public bool popOnDeceleration = true;

		[Tooltip("The amount of pops under deceleration.")]
		public float decelerationPopChanceCoeff = 1f;

		public override GameObject ContainerGO => vehicleController.soundManager.exhaustSourceGO;

		public override AudioMixerGroup AudioMixerGroup => vehicleController.soundManager.engineMixerGroup;

		public override bool InitLoop => false;

		public override int Priority => 160;

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				if (popSource == PopSource.RevLimiter)
				{
					vehicleController.powertrain.engine.onRevLimiter.AddListener(Pop);
				}
				else if (popSource == PopSource.ExhaustFlash)
				{
					vehicleController.effectsManager.exhaustFlash.onFlash.AddListener(Pop);
				}
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				vehicleController.powertrain.engine.onRevLimiter.RemoveListener(Pop);
				vehicleController.effectsManager.exhaustFlash.onFlash.RemoveListener(Pop);
				return true;
			}
			return false;
		}

		public override void VC_FixedUpdate()
		{
			base.VC_FixedUpdate();
			if (popOnDeceleration && vehicleController.powertrain.engine.ThrottlePosition < 0.02f && vehicleController.powertrain.engine.RPMPercent > 0.4f && UnityEngine.Random.Range(0f, 1f) < popChance * decelerationPopChanceCoeff * vehicleController.fixedDeltaTime)
			{
				SetVolume(baseVolume * 0.5f + vehicleController.powertrain.engine.RPMPercent * 0.5f);
				Pop();
			}
		}

		public void Pop()
		{
			if (!(UnityEngine.Random.Range(0f, 1f) > popChance))
			{
				Stop();
				SetVolume(UnityEngine.Random.Range(baseVolume * 0.5f, baseVolume * 1.5f));
				SetPitch(UnityEngine.Random.Range(0.7f, 1.3f));
				PlayRandomClip();
			}
		}

		public override void VC_SetDefaults()
		{
			base.VC_SetDefaults();
			baseVolume = 0.3f;
			if (base.Clip == null)
			{
				AddDefaultClip("ExhaustPop");
			}
		}
	}
}
