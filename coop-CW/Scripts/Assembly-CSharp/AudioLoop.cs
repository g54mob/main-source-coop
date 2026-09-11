using UnityEngine;
using UnityEngine.Audio;
using Zorro.Core;

public class AudioLoop : MonoBehaviour
{
	public AudioClip clip;

	public AudioMixerGroup mixerGroup;

	public float volume = 1f;

	public float pitch = 1f;

	public float minDistance = 1f;

	public float maxDistance = 100f;

	public float obstrability = 0.8f;

	public float blend = 1f;

	private Optionable<float> overrideStartTime = Optionable<float>.None;

	private int checkID;

	private void Start()
	{
		checkID = Random.Range(0, 15);
	}

	private void OnEnable()
	{
		AudioLoopHandler.RegisterAudioLoop(this);
	}

	private void OnDisable()
	{
		AudioLoopHandler.UnregisterAudioLoop(this);
	}

	public void SetTime(float timeInSong)
	{
		overrideStartTime = Optionable<float>.Some(timeInSong);
	}

	public bool TryGetOverrideTime(out float f)
	{
		if (overrideStartTime.IsSome)
		{
			f = overrideStartTime.Value;
			return true;
		}
		f = 0f;
		return false;
	}

	internal bool CheckBoost()
	{
		if (checkID >= 15)
		{
			checkID = 0;
			return true;
		}
		checkID++;
		return false;
	}
}
