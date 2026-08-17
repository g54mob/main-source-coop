using Enviro;
using NomadDrive.Managers.GameTime;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.GameTime
{
	public class WeatherAudioBridge : MonoBehaviour
	{
		[Tooltip("Multiplier applied to RainIntensity (0..1) before writing weatherMasterVolume.")]
		[SerializeField]
		[Range(0f, 2f)]
		private float weatherVolumeScale = 1f;

		[Tooltip("Master ambient volume (day/night, wildlife). Static — set once.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float ambientMasterVolume = 1f;

		[Tooltip("Thunder one-shot volume. Static — set once.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float thunderMasterVolume = 1f;

		[Inject]
		private ITimeManager _timeManager;

		private EnviroManager _enviroManager;

		private bool _initializedStaticVolumes;

		private void Update()
		{
			if (_enviroManager == null)
			{
				_enviroManager = EnviroManager.instance;
				if (_enviroManager == null)
				{
					return;
				}
			}
			if (!(_enviroManager.Audio == null) && _enviroManager.Audio.Settings != null)
			{
				if (!_initializedStaticVolumes)
				{
					_enviroManager.Audio.Settings.ambientMasterVolume = ambientMasterVolume;
					_enviroManager.Audio.Settings.thunderMasterVolume = thunderMasterVolume;
					_initializedStaticVolumes = true;
				}
				if (_timeManager != null)
				{
					float weatherMasterVolume = Mathf.Clamp01(_timeManager.RainIntensity * weatherVolumeScale);
					_enviroManager.Audio.Settings.weatherMasterVolume = weatherMasterVolume;
				}
			}
		}

		[ContextMenu("Log Current Weather Volume")]
		private void LogCurrentVolume()
		{
			if (!(_enviroManager == null))
			{
				_ = _enviroManager.Audio == null;
			}
		}
	}
}
