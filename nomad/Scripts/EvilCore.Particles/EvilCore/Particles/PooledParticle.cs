using UnityEngine;

namespace EvilCore.Particles
{
	public class PooledParticle : MonoBehaviour
	{
		private bool _isOneShot;

		private ParticlesManager _manager;

		private float _originalStartLifetimeMultiplier;

		private float _originalStartSpeedMultiplier;

		private float _originalSimulationSpeed;

		private Color _originalStartColor;

		private Vector3 _originalScale;

		public int ActiveId { get; private set; }

		public string Key { get; private set; }

		public ParticleSystem ParticleSystem { get; private set; }

		public void Initialize(string key, ParticlesManager manager)
		{
			Key = key;
			_manager = manager;
			ParticleSystem = GetComponent<ParticleSystem>();
			ParticleSystem.MainModule main = ParticleSystem.main;
			main.stopAction = ParticleSystemStopAction.Callback;
			_originalStartLifetimeMultiplier = main.startLifetimeMultiplier;
			_originalStartSpeedMultiplier = main.startSpeedMultiplier;
			_originalSimulationSpeed = main.simulationSpeed;
			_originalStartColor = main.startColor.color;
			_originalScale = base.transform.localScale;
		}

		public void Activate(int id, bool isOneShot, ParticleOverrides overrides = default(ParticleOverrides))
		{
			ActiveId = id;
			_isOneShot = isOneShot;
			ApplyOverrides(overrides);
			base.gameObject.SetActive(value: true);
			ParticleSystem.Play(withChildren: true);
		}

		public void Deactivate()
		{
			ActiveId = 0;
			ParticleSystem.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
			RestoreOriginalValues();
			base.gameObject.SetActive(value: false);
		}

		private void OnParticleSystemStopped()
		{
			if (_isOneShot && ActiveId != 0)
			{
				_manager.ReturnToPool(this);
			}
		}

		private void ApplyOverrides(ParticleOverrides overrides)
		{
			ParticleSystem.MainModule main = ParticleSystem.main;
			if (overrides.ScaleMultiplier.HasValue)
			{
				base.transform.localScale = _originalScale * overrides.ScaleMultiplier.Value;
			}
			if (overrides.StartColor.HasValue)
			{
				main.startColor = overrides.StartColor.Value;
			}
			if (overrides.StartLifetimeMultiplier.HasValue)
			{
				main.startLifetimeMultiplier = _originalStartLifetimeMultiplier * overrides.StartLifetimeMultiplier.Value;
			}
			if (overrides.StartSpeedMultiplier.HasValue)
			{
				main.startSpeedMultiplier = _originalStartSpeedMultiplier * overrides.StartSpeedMultiplier.Value;
			}
			if (overrides.SimulationSpeed.HasValue)
			{
				main.simulationSpeed = overrides.SimulationSpeed.Value;
			}
		}

		private void RestoreOriginalValues()
		{
			ParticleSystem.MainModule main = ParticleSystem.main;
			main.startLifetimeMultiplier = _originalStartLifetimeMultiplier;
			main.startSpeedMultiplier = _originalStartSpeedMultiplier;
			main.simulationSpeed = _originalSimulationSpeed;
			main.startColor = _originalStartColor;
			base.transform.localScale = _originalScale;
		}
	}
}
