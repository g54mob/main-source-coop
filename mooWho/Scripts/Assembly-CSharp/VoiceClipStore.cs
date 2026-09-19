using System.Collections.Generic;
using UnityEngine;

public class VoiceClipStore : MonoBehaviour
{
	private readonly Dictionary<AnimalType, AudioClip> _clips = new Dictionary<AnimalType, AudioClip>();

	public static VoiceClipStore Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	public void SetClip(AnimalType type, AudioClip clip)
	{
		_clips[type] = clip;
	}

	public AudioClip GetClip(AnimalType type)
	{
		if (!_clips.TryGetValue(type, out var value))
		{
			return null;
		}
		return value;
	}

	public bool HasClip(AnimalType type)
	{
		return _clips.ContainsKey(type);
	}

	public void RemoveClip(AnimalType type)
	{
		_clips.Remove(type);
	}

	public void ClearAll()
	{
		_clips.Clear();
	}

	public void SetClipFromPcm(AnimalType type, byte[] pcm, int sampleRate, int channels)
	{
		AudioClip clip = MicRecorder.PcmToAudioClip(pcm, sampleRate, channels, $"Voice_{type}");
		SetClip(type, clip);
		Debug.Log($"[VoiceClipStore] {type} için kayıt depolandı ({pcm.Length} byte).");
	}
}
