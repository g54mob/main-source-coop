using UnityEngine;

namespace Features.JacuzziBeachInteractableModule.Scripts.VisualMusic
{
	public class BlinkVisualMusicBehaviour : VisualMusicBehaviour
	{
		[SerializeField]
		private float _minIntensity = 0.1f;

		[SerializeField]
		private float _maxIntensity = 4f;

		[SerializeField]
		private Vector2 _blinkSpeedRange = new Vector2(1.5f, 5f);

		private float[] _speeds;

		private float[] _phases;

		private float _time;

		protected override void OnEffectStarted()
		{
			if (_speeds == null || _speeds.Length != base.Lights.Length)
			{
				_speeds = new float[base.Lights.Length];
				_phases = new float[base.Lights.Length];
			}
			_time = 0f;
			for (int i = 0; i < base.Lights.Length; i++)
			{
				_speeds[i] = Random.Range(_blinkSpeedRange.x, _blinkSpeedRange.y);
				_phases[i] = Random.Range(0f, 100f);
			}
		}

		protected override void OnEffectTick(float deltaTime)
		{
			_time += deltaTime;
			for (int i = 0; i < base.Lights.Length; i++)
			{
				float t = Mathf.PerlinNoise(_phases[i], _time * _speeds[i]);
				float b = Mathf.Lerp(_minIntensity, _maxIntensity, t);
				base.Lights[i].intensity = Mathf.Lerp(base.DefaultIntensities[i], b, base.Blend);
			}
		}

		protected override void OnEffectStopped()
		{
		}
	}
}
