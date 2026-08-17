using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
	[Serializable]
	public abstract class SoundComponent : VehicleComponent
	{
		[FormerlySerializedAs("volume")]
		[Range(0f, 1f)]
		[Tooltip("    Base volume of the sound component.")]
		public float baseVolume = 0.1f;

		[Tooltip("List of audio clips this component can use. Some components can use multiple clips in which case they will be chosen at random, and some components can use only one in which case only the first clip will be selected. Check manual for more details.")]
		public List<AudioClip> clips = new List<AudioClip>();

		[NonSerialized]
		[Tooltip("Audio sources for this component. Can be multiple (e.g. multiple wheels per SkidComponent)")]
		public AudioSource source;

		public virtual bool InitializeWithNoClips => false;

		public abstract GameObject ContainerGO { get; }

		public abstract AudioMixerGroup AudioMixerGroup { get; }

		public abstract int Priority { get; }

		public AudioClip Clip
		{
			get
			{
				if (clips.Count <= 0)
				{
					return null;
				}
				return clips[0];
			}
			set
			{
				if (clips.Count > 0)
				{
					clips[0] = value;
				}
				else
				{
					clips.Add(value);
				}
			}
		}

		public virtual bool InitLoop => false;

		public virtual AudioClip InitClip => Clip;

		public virtual float InitVolume => baseVolume;

		public virtual float InitSpatialBlend => vehicleController.soundManager.spatialBlend;

		public virtual float InitDopplerLevel => vehicleController.soundManager.dopplerLevel;

		public virtual bool InitPlayOnAwake => false;

		public AudioClip RandomClip => clips[UnityEngine.Random.Range(0, clips.Count)];

		protected override void VC_Initialize()
		{
			if (!InitializeWithNoClips && (clips == null || clips.Count == 0))
			{
				return;
			}
			source = CreateAndRegisterAudioSource(AudioMixerGroup, ContainerGO);
			if (source == null)
			{
				Debug.LogWarning("AudioSource could not be created on " + GetType().Name + "! Make sure that the Project Settings > Audio > Disable Unity Audio is not ticked and that the mixer (if assigned) has the required audio groups.");
				return;
			}
			source.priority = Priority;
			if (InitPlayOnAwake)
			{
				Play();
			}
			else
			{
				Stop();
			}
			base.VC_Initialize();
		}

		protected AudioSource CreateAndRegisterAudioSource(AudioMixerGroup mixerGroup, GameObject container)
		{
			if (mixerGroup == null)
			{
				Debug.LogError("Trying to setup an AudioSource with null mixer group on " + vehicleController.name + ".");
				return null;
			}
			if (container == null)
			{
				Debug.LogError("Trying to use a null container.");
				return null;
			}
			AudioSource audioSource = container.AddComponent<AudioSource>();
			if (audioSource == null)
			{
				Debug.LogError("Failed to create AudioSource.");
				return null;
			}
			audioSource.outputAudioMixerGroup = mixerGroup;
			audioSource.spatialBlend = InitSpatialBlend;
			audioSource.playOnAwake = InitPlayOnAwake;
			audioSource.loop = InitLoop;
			audioSource.volume = InitVolume * vehicleController.soundManager.masterVolume;
			audioSource.clip = InitClip;
			audioSource.priority = 100;
			audioSource.dopplerLevel = InitDopplerLevel;
			return audioSource;
		}

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				source.enabled = true;
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				Stop();
				source.enabled = false;
				return true;
			}
			return false;
		}

		public virtual void PlayRandomClip()
		{
			Play(UnityEngine.Random.Range(0, clips.Count));
		}

		public IEnumerator PlayForDurationCoroutine(int clipIndex, float duration)
		{
			Play(clipIndex);
			yield return new WaitForSeconds(duration);
			Stop();
		}

		public virtual void Play()
		{
			if (source.enabled && !source.isPlaying)
			{
				source.Play();
			}
		}

		public virtual void Play(int clipIndex)
		{
			if (clipIndex >= 0 && clipIndex < clips.Count)
			{
				source.clip = clips[clipIndex];
				Play();
			}
		}

		public virtual void SetPitch(float pitch)
		{
			pitch = ((pitch < 0f) ? 0f : ((pitch > 5f) ? 5f : pitch));
			source.pitch = pitch;
		}

		public virtual void SetVolume(float volume)
		{
			source.volume = volume * vehicleController.soundManager.masterVolume;
		}

		public virtual void Stop()
		{
			if (source.isPlaying)
			{
				source.Stop();
			}
		}

		public virtual void AddDefaultClip(string clipName)
		{
			Clip = Resources.Load("NWH Vehicle Physics 2/Defaults/Sound/" + clipName) as AudioClip;
			if (Clip == null)
			{
				Debug.LogWarning("Audio Clip for sound component " + GetType().Name + " could not be loaded from resources. Source will not play.Assign an AudioClip manually.");
			}
		}
	}
}
