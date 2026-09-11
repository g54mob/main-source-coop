using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFX_Player : MonoBehaviour
{
	[Serializable]
	public class SFX_Source
	{
		public AudioSource source;

		public SFX_Player player;

		public void StopPlaying()
		{
			source.Stop();
			source.clip = null;
			source.gameObject.SetActive(value: false);
		}

		public void StartPlaying()
		{
			source.time = 0f;
			source.Play();
		}

		public void ReEnable()
		{
			source.gameObject.SetActive(value: true);
		}
	}

	private GameObject defaultSource;

	private Dictionary<SFX_Instance, List<SFX_Source>> sources = new Dictionary<SFX_Instance, List<SFX_Source>>();

	public static SFX_Player instance;

	public AudioMixerGroup boostGroup;

	private int nrOfSoundsPlayed;

	private List<SFX_Source> availibleSources = new List<SFX_Source>();

	public Action<Vector3, float, int> playNoiseAction;

	[SerializeField]
	private AudioClip warmupClip;

	internal List<SoundBooster> soundBoosters = new List<SoundBooster>();

	public AnimationCurve defaultCurve;

	private void Start()
	{
		AudioSource componentInChildren = GetComponentInChildren<AudioSource>(includeInactive: true);
		defaultSource = componentInChildren.gameObject;
		instance = this;
	}

	public void PlaySFX(SFX_Instance SFX, Vector3 position, Transform followTransform = null, SFX_Settings overrideSettings = null, float volumeMultiplier = 1f, bool loop = false, bool local = false, bool isNoise = true, float stepNoiseMultiplier = 0f, int alerts = 0)
	{
		if (SFX == null)
		{
			Debug.LogError("Trying to play null sound >:I");
		}
		else if (SFX.clips.Length == 0)
		{
			Debug.LogError("Trying to play sound with no clips >:I");
		}
		else if (!(SFX.settings.spatialBlend > 0.75f) || !(Vector3.Distance(position, MainCamera.instance.transform.position) > SFX.settings.range))
		{
			if (SFX.settings.noiseDistance > 0 && isNoise)
			{
				playNoiseAction?.Invoke(position, (float)SFX.settings.noiseDistance * stepNoiseMultiplier, alerts);
			}
			if (!sources.ContainsKey(SFX))
			{
				sources.Add(SFX, new List<SFX_Source>());
			}
			else if (sources[SFX].Count >= 10)
			{
				return;
			}
			SFX.OnPlayed();
			SFX_Source availibleSource = GetAvailibleSource();
			availibleSource.player.StartCoroutine(IPlaySFX(availibleSource, SFX, position, followTransform, overrideSettings, volumeMultiplier, local));
		}
	}

	public void PlayNoise(Vector3 position, float distance = 15f, int alerts = 1)
	{
		playNoiseAction?.Invoke(position, distance, alerts);
	}

	private IEnumerator IPlaySFX(SFX_Source source, SFX_Instance SFX, Vector3 position, Transform followTransform, SFX_Settings overrideSettings, float volumeMultiplier, bool local)
	{
		sources[SFX].Add(source);
		source.source.gameObject.name = "SFX: " + SFX.name;
		AudioClip clip = SFX.GetClip();
		if (clip == null)
		{
			Debug.LogError("Trying to play null sound >:I");
			RemoveSource(source, SFX);
			yield break;
		}
		SFX_Settings settings = SFX.settings;
		if (overrideSettings != null)
		{
			settings = overrideSettings;
		}
		float c = 0f;
		float t = clip.length;
		float boostValue = GetBoostValue(position);
		float rangeFactor;
		float num = AudioObstructability.GetObstructionValue(position, settings.obstructability, out rangeFactor);
		if ((double)settings.spatialBlend < 0.5)
		{
			num = 1f;
			rangeFactor = 1f;
		}
		source.source.clip = clip;
		source.source.transform.position = position;
		source.source.volume = settings.volume * UnityEngine.Random.Range(1f - settings.volume_Variation, 1f) * volumeMultiplier * num;
		source.source.pitch = settings.pitch + UnityEngine.Random.Range((0f - settings.pitch_Variation) * 0.5f, settings.pitch_Variation * 0.5f);
		source.source.maxDistance = settings.range * boostValue * rangeFactor;
		source.source.minDistance = settings.minRange * boostValue * rangeFactor;
		if ((double)settings.spatialBlend > 0.3)
		{
			source.source.rolloffMode = AudioRolloffMode.Custom;
			source.source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, defaultCurve);
		}
		source.source.spatialBlend = settings.spatialBlend;
		source.source.dopplerLevel = settings.dopplerLevel;
		source.source.outputAudioMixerGroup = settings.mixerGroup;
		if (boostValue * num > 1.5f)
		{
			source.source.outputAudioMixerGroup = boostGroup;
		}
		if (local && settings.nonSpatializedForLocalPlayer)
		{
			source.source.spatialBlend = 0f;
		}
		Vector3 relativePos = Vector3.zero;
		if ((bool)followTransform)
		{
			relativePos = followTransform.InverseTransformPoint(position);
		}
		source.StartPlaying();
		while (c < t)
		{
			c += Time.deltaTime * settings.pitch;
			if ((bool)followTransform)
			{
				source.source.transform.position = followTransform.TransformPoint(relativePos);
			}
			yield return null;
		}
		RemoveSource(source, SFX);
	}

	private void RemoveSource(SFX_Source source, SFX_Instance sfx)
	{
		sources[sfx].Remove(source);
		source.StopPlaying();
		availibleSources.Add(source);
	}

	public static float GetBoostValue(Vector3 position)
	{
		float distance;
		return GetBoostValue(position, out distance);
	}

	public static float GetBoostValue(Vector3 position, out float distance)
	{
		float num = float.PositiveInfinity;
		SoundBooster soundBooster = null;
		float result = 1f;
		for (int i = 0; i < instance.soundBoosters.Count; i++)
		{
			if (HelperFunctions.InBoxRange(instance.soundBoosters[i].transform.position, position, 5))
			{
				float num2 = Vector3.Distance(instance.soundBoosters[i].transform.position, position);
				if (num2 < num)
				{
					num = num2;
					soundBooster = instance.soundBoosters[i];
				}
			}
		}
		if (soundBooster != null)
		{
			result = Mathf.InverseLerp(soundBooster.maxRange, soundBooster.minRange, num);
			result *= result;
			result = Mathf.Lerp(1f, 2f, result);
		}
		distance = num;
		return result;
	}

	private SFX_Source GetAvailibleSource()
	{
		if (availibleSources.Count > 0)
		{
			SFX_Source sFX_Source = availibleSources[0];
			availibleSources.RemoveAt(0);
			sFX_Source.ReEnable();
			return sFX_Source;
		}
		return CreateNewSource();
	}

	private SFX_Source CreateNewSource()
	{
		SFX_Source sFX_Source = new SFX_Source();
		GameObject gameObject = UnityEngine.Object.Instantiate(defaultSource, base.transform.position, base.transform.rotation, base.transform);
		sFX_Source.source = gameObject.GetComponent<AudioSource>();
		sFX_Source.player = this;
		sFX_Source.source.gameObject.SetActive(value: true);
		return sFX_Source;
	}
}
