using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	[ExecuteInEditMode]
	public class EnviroAudioModule : EnviroModule
	{
		public EnviroAudio Settings;

		public EnviroAudioModule preset;

		public float ambientVolumeModifier;

		public float weatherVolumeModifier;

		public float thunderVolumeModifier;

		public bool showAmbientSetupControls;

		public bool showWeatherSetupControls;

		public bool showThunderSetupControls;

		public bool showAudioControls;

		public override void Enable()
		{
			if (!(EnviroManager.instance == null))
			{
				CreateAudio();
			}
		}

		public override void Disable()
		{
			if (!(EnviroManager.instance == null))
			{
				Cleanup();
			}
		}

		private void Setup()
		{
		}

		private void Cleanup()
		{
			if (!(EnviroManager.instance == null) && EnviroManager.instance.Objects.audio != null)
			{
				EnviroHelper.DestroyExtended(EnviroManager.instance.Objects.audio);
			}
		}

		public override void UpdateModule()
		{
			if (active)
			{
				UpdateAudio();
			}
		}

		public void CreateAudio()
		{
			if (EnviroManager.instance.Objects.audio != null)
			{
				EnviroHelper.DestroyExtended(EnviroManager.instance.Objects.audio);
				EnviroManager.instance.Objects.audio = null;
			}
			if (EnviroManager.instance.Objects.audio == null)
			{
				EnviroManager.instance.Objects.audio = new GameObject();
				EnviroManager.instance.Objects.audio.hideFlags = HideFlags.DontSave;
				EnviroManager.instance.Objects.audio.name = "Audio";
				EnviroManager.instance.Objects.audio.transform.SetParent(EnviroManager.instance.transform);
				EnviroManager.instance.Objects.audio.transform.localPosition = Vector3.zero;
			}
			for (int i = 0; i < Settings.ambientClips.Count; i++)
			{
				if (Settings.ambientClips[i].myAudioSource != null)
				{
					EnviroHelper.DestroyExtended(Settings.ambientClips[i].myAudioSource.gameObject);
				}
				if (Settings.ambientClips[i].audioClip != null)
				{
					GameObject gameObject = new GameObject();
					gameObject.name = "Ambient - " + Settings.ambientClips[i].name;
					gameObject.transform.SetParent(EnviroManager.instance.Objects.audio.transform);
					Settings.ambientClips[i].myAudioSource = gameObject.AddComponent<AudioSource>();
					Settings.ambientClips[i].myAudioSource.clip = Settings.ambientClips[i].audioClip;
					Settings.ambientClips[i].myAudioSource.loop = Settings.ambientClips[i].loop;
					Settings.ambientClips[i].myAudioSource.volume = Settings.ambientClips[i].volume;
					Settings.ambientClips[i].myAudioSource.outputAudioMixerGroup = Settings.ambientClips[i].audioMixerGroup;
				}
			}
			for (int j = 0; j < Settings.weatherClips.Count; j++)
			{
				if (Settings.weatherClips[j].myAudioSource != null)
				{
					EnviroHelper.DestroyExtended(Settings.weatherClips[j].myAudioSource.gameObject);
				}
				if (Settings.weatherClips[j].audioClip != null)
				{
					GameObject gameObject2 = new GameObject();
					gameObject2.name = "Weather - " + Settings.weatherClips[j].name;
					gameObject2.transform.SetParent(EnviroManager.instance.Objects.audio.transform);
					Settings.weatherClips[j].myAudioSource = gameObject2.AddComponent<AudioSource>();
					Settings.weatherClips[j].myAudioSource.clip = Settings.weatherClips[j].audioClip;
					Settings.weatherClips[j].myAudioSource.loop = Settings.weatherClips[j].loop;
					Settings.weatherClips[j].myAudioSource.volume = Settings.weatherClips[j].volume;
					Settings.weatherClips[j].myAudioSource.outputAudioMixerGroup = Settings.weatherClips[j].audioMixerGroup;
				}
			}
			for (int k = 0; k < Settings.thunderClips.Count; k++)
			{
				if (Settings.thunderClips[k].myAudioSource != null)
				{
					EnviroHelper.DestroyExtended(Settings.thunderClips[k].myAudioSource.gameObject);
				}
				if (Settings.thunderClips[k].audioClip != null)
				{
					GameObject gameObject3 = new GameObject();
					gameObject3.name = "Thunder - " + Settings.thunderClips[k].name;
					gameObject3.transform.SetParent(EnviroManager.instance.Objects.audio.transform);
					Settings.thunderClips[k].myAudioSource = gameObject3.AddComponent<AudioSource>();
					Settings.thunderClips[k].myAudioSource.clip = Settings.thunderClips[k].audioClip;
					Settings.thunderClips[k].myAudioSource.loop = false;
					Settings.thunderClips[k].myAudioSource.playOnAwake = false;
					Settings.thunderClips[k].myAudioSource.volume = Settings.thunderClips[k].volume;
					Settings.thunderClips[k].myAudioSource.outputAudioMixerGroup = Settings.thunderClips[k].audioMixerGroup;
				}
			}
		}

		public void PlayRandomThunderSFX()
		{
			int index = UnityEngine.Random.Range(0, Settings.thunderClips.Count);
			if (Settings.thunderClips.Count > 0 && Settings.thunderClips[index] != null)
			{
				Settings.thunderClips[index].myAudioSource.volume = Settings.thunderClips[index].volume * Settings.thunderMasterVolume + thunderVolumeModifier;
				Settings.thunderClips[index].myAudioSource.PlayOneShot(Settings.thunderClips[index].myAudioSource.clip);
			}
		}

		public void UpdateAudio()
		{
			for (int i = 0; i < Settings.ambientClips.Count; i++)
			{
				UpdateEnviroAudioClip(Settings.ambientClips[i], Settings.ambientMasterVolume + ambientVolumeModifier);
			}
			for (int j = 0; j < Settings.weatherClips.Count; j++)
			{
				UpdateEnviroAudioClip(Settings.weatherClips[j], Settings.weatherMasterVolume + weatherVolumeModifier);
			}
		}

		private void UpdateEnviroAudioClip(EnviroAudioClip clip, float masterVolume)
		{
			if (!(clip.audioClip != null) || !(clip.myAudioSource != null))
			{
				return;
			}
			if (!Application.isPlaying)
			{
				clip.myAudioSource.Stop();
				return;
			}
			clip.myAudioSource.loop = clip.loop;
			switch (clip.playBackType)
			{
			case EnviroAudioClip.PlayBackType.Always:
				clip.myAudioSource.volume = clip.volume * masterVolume;
				break;
			case EnviroAudioClip.PlayBackType.BasedOnSun:
				clip.myAudioSource.volume = clip.volumeCurve.Evaluate(EnviroManager.instance.solarTime);
				clip.myAudioSource.volume *= clip.volume * masterVolume;
				break;
			case EnviroAudioClip.PlayBackType.BasedOnMoon:
				clip.myAudioSource.volume = clip.volumeCurve.Evaluate(EnviroManager.instance.lunarTime);
				clip.myAudioSource.volume *= clip.volume * masterVolume;
				break;
			}
			if (clip.myAudioSource.volume < 0.001f && clip.myAudioSource.isPlaying)
			{
				clip.myAudioSource.Stop();
			}
			if (clip.myAudioSource.volume > 0f && !clip.myAudioSource.isPlaying)
			{
				clip.myAudioSource.Play();
			}
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				Settings = JsonUtility.FromJson<EnviroAudio>(JsonUtility.ToJson(preset.Settings));
			}
			else
			{
				Debug.Log("Please assign a saved module to load from!");
			}
		}

		public void SaveModuleValues()
		{
		}

		public void SaveModuleValues(EnviroAudioModule module)
		{
			module.Settings = JsonUtility.FromJson<EnviroAudio>(JsonUtility.ToJson(Settings));
		}
	}
}
