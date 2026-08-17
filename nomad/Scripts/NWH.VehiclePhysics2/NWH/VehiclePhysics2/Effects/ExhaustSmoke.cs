using System;
using System.Collections.Generic;
using UnityEngine;

namespace NWH.VehiclePhysics2.Effects
{
	[Serializable]
	public class ExhaustSmoke : Effect
	{
		[Range(0f, 1f)]
		public float sootIntensity = 0.4f;

		[Range(1f, 5f)]
		public float maxSpeedMultiplier = 1.4f;

		[Range(1f, 5f)]
		public float maxSizeMultiplier = 1.2f;

		[Tooltip("    Normal particle start color. Used when there is no throttle - engine is under no load.")]
		public Color normalColor = new Color(0.6f, 0.6f, 0.6f, 0.3f);

		[Tooltip("    Soot particle start color. Used under heavy throttle - engine is under load.")]
		public Color sootColor = new Color(0.1f, 0.1f, 0.8f);

		[Tooltip("    List of exhaust particle systems.")]
		public List<ParticleSystem> particleSystems = new List<ParticleSystem>();

		private float _initStartSpeedMin;

		private float _initStartSpeedMax;

		private float _initStartSizeMin;

		private float _initStartSizeMax;

		private float _sootAmount;

		private ParticleSystem.EmissionModule _emissionModule;

		private ParticleSystem.MainModule _mainModule;

		private ParticleSystem.MinMaxCurve _minMaxCurve;

		private float _vehicleSpeed;

		protected override void VC_Initialize()
		{
			foreach (ParticleSystem particleSystem in particleSystems)
			{
				if (particleSystem == null)
				{
					Debug.LogError("One or more of the exhaust ParticleSystems on the vehicle " + vehicleController.name + " is null.");
				}
			}
			if (particleSystems != null && particleSystems.Count != 0)
			{
				_emissionModule = particleSystems[0].emission;
				_mainModule = particleSystems[0].main;
				_initStartSpeedMin = _mainModule.startSpeed.constantMin;
				_initStartSpeedMax = _mainModule.startSpeed.constantMax;
				_initStartSizeMin = _mainModule.startSize.constantMin;
				_initStartSizeMax = _mainModule.startSize.constantMax;
				maxSizeMultiplier = Mathf.Clamp(maxSizeMultiplier, 1f, 1f / 0f);
				maxSpeedMultiplier = Mathf.Clamp(maxSpeedMultiplier, 1f, 1f / 0f);
				base.VC_Initialize();
			}
		}

		public override void VC_Update()
		{
			base.VC_Update();
			if (vehicleController.powertrain.IsActive && vehicleController.powertrain.engine.IsRunning)
			{
				_vehicleSpeed = vehicleController.Speed;
				{
					foreach (ParticleSystem particleSystem in particleSystems)
					{
						if (!particleSystem.isPlaying)
						{
							particleSystem.Play();
						}
						_emissionModule = particleSystem.emission;
						_mainModule = particleSystem.main;
						float load = vehicleController.powertrain.engine.Load;
						float rPMPercent = vehicleController.powertrain.engine.RPMPercent;
						if (!_emissionModule.enabled)
						{
							_emissionModule.enabled = true;
						}
						_sootAmount = load * sootIntensity;
						_mainModule.startColor = Color.Lerp(_mainModule.startColor.color, Color.Lerp(normalColor, sootColor, _sootAmount), Time.deltaTime * 7f);
						float num = maxSpeedMultiplier - 1f;
						_minMaxCurve = _mainModule.startSpeed;
						_minMaxCurve.constantMin = _initStartSpeedMin + rPMPercent * num;
						_minMaxCurve.constantMax = _initStartSpeedMax + rPMPercent * num;
						_mainModule.startSpeed = _minMaxCurve;
						float num2 = maxSizeMultiplier - 1f;
						_minMaxCurve = _mainModule.startSize;
						_minMaxCurve.constantMin = _initStartSizeMin + rPMPercent * num2;
						_minMaxCurve.constantMax = _initStartSizeMax + rPMPercent * num2;
						_mainModule.startSize = _minMaxCurve;
					}
					return;
				}
			}
			foreach (ParticleSystem particleSystem2 in particleSystems)
			{
				if (particleSystem2.isPlaying)
				{
					particleSystem2.Stop();
				}
				ParticleSystem.EmissionModule emission = particleSystem2.emission;
				emission.enabled = false;
			}
		}

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				foreach (ParticleSystem particleSystem in particleSystems)
				{
					particleSystem.Play();
				}
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				foreach (ParticleSystem particleSystem in particleSystems)
				{
					_ = particleSystem.emission;
					particleSystem.Stop();
				}
				return true;
			}
			return false;
		}
	}
}
