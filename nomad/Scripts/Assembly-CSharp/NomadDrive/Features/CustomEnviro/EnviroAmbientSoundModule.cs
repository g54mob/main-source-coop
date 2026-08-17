using System;
using Ami.BroAudio;
using Enviro;
using EvilCore.Audio;
using UnityEngine;

namespace NomadDrive.Features.CustomEnviro
{
	[Serializable]
	[CreateAssetMenu(fileName = "EnviroAmbientSoundModule", menuName = "Enviro3/Ambient Sound Module")]
	public class EnviroAmbientSoundModule : EnviroModule
	{
		public EnviroAmbientSoundSettings Settings = new EnviroAmbientSoundSettings();

		public EnviroAmbientSoundModule preset;

		private IAudioManager _audioManager;

		private GameObject _attachTarget;

		private AudioHandle[] _handles;

		private float[] _smoothed;

		private float[] _lastApplied;

		public void Initialize(IAudioManager audioManager, GameObject attachTarget)
		{
			_audioManager = audioManager;
			_attachTarget = attachTarget;
		}

		public override void Enable()
		{
			int valueOrDefault = (Settings?.layers?.Count).GetValueOrDefault();
			_handles = new AudioHandle[valueOrDefault];
			_smoothed = new float[valueOrDefault];
			_lastApplied = new float[valueOrDefault];
			for (int i = 0; i < valueOrDefault; i++)
			{
				_handles[i] = AudioHandle.Invalid;
				_smoothed[i] = 0f;
				_lastApplied[i] = -1f;
			}
		}

		public override void Disable()
		{
			if (_handles != null && _audioManager != null)
			{
				for (int i = 0; i < _handles.Length; i++)
				{
					if (_handles[i].IsValid)
					{
						_audioManager.StopEvent(_handles[i], AudioStopMode.Immediate);
					}
					_handles[i] = AudioHandle.Invalid;
				}
			}
			_handles = null;
			_smoothed = null;
			_lastApplied = null;
		}

		public override void UpdateModule()
		{
			if (!active || _audioManager == null || EnviroManager.instance == null || Settings?.layers == null || _handles == null || _handles.Length != Settings.layers.Count)
			{
				return;
			}
			float solarTime = EnviroManager.instance.solarTime;
			float t = Time.deltaTime * Settings.transitionSpeed;
			for (int i = 0; i < Settings.layers.Count; i++)
			{
				AmbientLayer ambientLayer = Settings.layers[i];
				if (ambientLayer == null || !ambientLayer.sound.IsValid())
				{
					continue;
				}
				float b = Mathf.Clamp01(((ambientLayer.volumeOverDay != null) ? ambientLayer.volumeOverDay.Evaluate(solarTime) : 0f) * ambientLayer.maxVolume);
				_smoothed[i] = Mathf.Lerp(_smoothed[i], b, t);
				if (_smoothed[i] <= Settings.silenceThreshold)
				{
					if (_handles[i].IsValid)
					{
						_audioManager.StopEvent(_handles[i], AudioStopMode.AllowFadeout, 0.5f);
						_handles[i] = AudioHandle.Invalid;
						_lastApplied[i] = -1f;
					}
					continue;
				}
				if (!_handles[i].IsValid)
				{
					_handles[i] = _audioManager.PlayEventAttached(ambientLayer.sound, _attachTarget);
					_lastApplied[i] = -1f;
				}
				if (_handles[i].IsValid && Mathf.Abs(_smoothed[i] - _lastApplied[i]) > Settings.volumeEpsilon)
				{
					_audioManager.SetParameter(_handles[i], "volume", _smoothed[i]);
					_lastApplied[i] = _smoothed[i];
				}
			}
		}

		public void SaveModuleValues(EnviroAmbientSoundModule module)
		{
			module.Settings = JsonUtility.FromJson<EnviroAmbientSoundSettings>(JsonUtility.ToJson(Settings));
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				Settings = JsonUtility.FromJson<EnviroAmbientSoundSettings>(JsonUtility.ToJson(preset.Settings));
			}
			if (Settings != null && Settings.layers != null)
			{
				_ = Settings.layers.Count;
			}
		}
	}
}
