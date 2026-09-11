using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffectInstance", menuName = "Landfall/SoundEffectInstance")]
public class SFX_Instance : ScriptableObject
{
	public AudioClip[] clips;

	public SFX_Settings settings;

	internal float lastTimePlayed;

	public AudioClip GetClip()
	{
		return clips[Random.Range(0, clips.Length)];
	}

	public void Play(Vector3 pos = default(Vector3), bool local = false, float volumeMultiplier = 1f, Transform followTransform = null)
	{
		SFX_Player.instance.PlaySFX(this, pos, followTransform, null, volumeMultiplier, loop: false, local);
	}

	internal float GetPitch()
	{
		return settings.pitch * Random.Range(1f - settings.pitch_Variation, 1f);
	}

	internal float GetVolume()
	{
		return settings.volume * Random.Range(1f - settings.volume_Variation, 1f);
	}

	internal void OnPlayed()
	{
		lastTimePlayed = Time.unscaledTime;
	}

	internal bool ReadyToPlay()
	{
		if (lastTimePlayed > Time.unscaledTime + settings.cooldown)
		{
			return true;
		}
		return lastTimePlayed + settings.cooldown < Time.unscaledTime;
	}
}
