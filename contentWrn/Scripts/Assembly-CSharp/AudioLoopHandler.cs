using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Zorro.Core;

[DefaultExecutionOrder(-2000)]
public class AudioLoopHandler : Singleton<AudioLoopHandler>
{
	public GameObject m_sourcePrefab;

	public AudioMixerGroup boosted;

	public List<AudioLoop> audioLoops = new List<AudioLoop>();

	public Dictionary<AudioLoop, AudioSource> audioSources = new Dictionary<AudioLoop, AudioSource>();

	public AnimationCurve m_falloffCurve;

	public static void RegisterAudioLoop(AudioLoop audioLoop)
	{
		if (Singleton<AudioLoopHandler>.Instance == null)
		{
			Debug.LogError("AudioLoopHandler not found!");
		}
		else
		{
			Singleton<AudioLoopHandler>.Instance.audioLoops.Add(audioLoop);
		}
	}

	public static void UnregisterAudioLoop(AudioLoop audioLoop)
	{
		if (!(Singleton<AudioLoopHandler>.Instance == null))
		{
			Singleton<AudioLoopHandler>.Instance.audioLoops.Remove(audioLoop);
			if (Singleton<AudioLoopHandler>.Instance.audioSources.ContainsKey(audioLoop))
			{
				Singleton<AudioLoopHandler>.Instance.RemoveSource(audioLoop);
			}
		}
	}

	private void LateUpdate()
	{
		for (int i = 0; i < audioLoops.Count; i++)
		{
			if (audioLoops[i] == null)
			{
				audioLoops.RemoveAt(i);
				i--;
				continue;
			}
			if (audioLoops[i].clip == null)
			{
				Debug.LogError("AudioLoop " + audioLoops[i].name + " has no clip!");
				continue;
			}
			bool flag = Vector3.Distance(audioLoops[i].transform.position, MainCamera.instance.transform.position) < audioLoops[i].maxDistance * 0.7f && audioLoops[i].volume > 0.01f;
			bool flag2 = audioSources.ContainsKey(audioLoops[i]);
			if (flag && !flag2)
			{
				AssignSource(audioLoops[i]);
			}
			else if (!flag && flag2)
			{
				RemoveSource(audioLoops[i]);
			}
			if (!audioSources.ContainsKey(audioLoops[i]))
			{
				continue;
			}
			AudioSource audioSource = audioSources[audioLoops[i]];
			audioSource.transform.position = audioLoops[i].transform.position;
			audioSource.pitch = audioLoops[i].pitch;
			float rangeFactor;
			float obstructionValue = AudioObstructability.GetObstructionValue(audioLoops[i].transform.position, audioLoops[i].obstrability, out rangeFactor);
			audioSource.volume = audioLoops[i].volume * obstructionValue;
			float num;
			float num2;
			if (audioSource.spatialBlend < 0.5f)
			{
				audioSource.volume = audioLoops[i].volume;
				num = audioLoops[i].maxDistance;
				num2 = audioLoops[i].minDistance;
			}
			else
			{
				audioSource.volume = audioLoops[i].volume * obstructionValue;
				num = audioLoops[i].maxDistance * rangeFactor;
				num2 = audioLoops[i].minDistance * rangeFactor;
			}
			if (audioLoops[i].CheckBoost() && SFX_Player.instance != null)
			{
				float boostValue = SFX_Player.GetBoostValue(audioSource.transform.position);
				num *= boostValue;
				num2 *= boostValue;
				if (boostValue > 1.5f)
				{
					audioSource.outputAudioMixerGroup = boosted;
				}
				else
				{
					audioSource.outputAudioMixerGroup = audioLoops[i].mixerGroup;
				}
			}
			audioSource.maxDistance = num;
			audioSource.minDistance = num2;
		}
	}

	private void RemoveSource(AudioLoop audioLoop)
	{
		VerboseDebug.Log("Removing source from " + audioLoop.name);
		AudioSource audioSource = audioSources[audioLoop];
		if (audioSource != null)
		{
			Object.Destroy(audioSource.gameObject);
		}
		audioSources.Remove(audioLoop);
		if (audioSources.ContainsKey(audioLoop))
		{
			audioSources.Remove(audioLoop);
		}
	}

	private void AssignSource(AudioLoop audioLoop)
	{
		VerboseDebug.Log("Assigning source to " + audioLoop.name);
		GameObject obj = Object.Instantiate(m_sourcePrefab, audioLoop.transform.position, Quaternion.identity);
		obj.name = "LOOP: " + audioLoop.name + " - " + audioLoop.clip.name;
		AudioSource component = obj.GetComponent<AudioSource>();
		component.clip = audioLoop.clip;
		component.volume = audioLoop.volume;
		component.pitch = audioLoop.pitch;
		component.spatialBlend = audioLoop.blend;
		component.outputAudioMixerGroup = audioLoop.mixerGroup;
		if (audioLoop.TryGetOverrideTime(out var f))
		{
			component.time = f % component.clip.length;
		}
		component.maxDistance = audioLoop.maxDistance;
		component.minDistance = audioLoop.minDistance;
		component.rolloffMode = AudioRolloffMode.Custom;
		component.SetCustomCurve(AudioSourceCurveType.CustomRolloff, m_falloffCurve);
		component.loop = true;
		component.Play();
		audioSources.Add(audioLoop, component);
	}
}
