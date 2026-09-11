using UnityEngine;

public class AmbienceHandler : MonoBehaviour
{
	public AudioClip[] clips;

	private AudioClip targetClip;

	private AudioSource source;

	private AmbienceTrigger[] triggers;

	private AmbienceTrigger currentTrigger;

	private void Start()
	{
		triggers = GetComponentsInChildren<AmbienceTrigger>(includeInactive: true);
		source = GetComponent<AudioSource>();
		TriggerChanged(null);
	}

	private void Update()
	{
		CheckForChange();
		PlayCurrentClip();
	}

	private void PlayCurrentClip()
	{
		if (source.clip != targetClip)
		{
			source.volume = Mathf.MoveTowards(source.volume, 0f, Time.deltaTime * 0.3f);
			if (source.volume < 0.001f)
			{
				PlayClip(targetClip);
			}
		}
		else if (!source.isPlaying)
		{
			PlayClip(GetRandomClip(currentTrigger));
		}
		else if (source.time > Mathf.Max(source.clip.length - 5f, source.clip.length * 0.9f))
		{
			source.volume = Mathf.MoveTowards(source.volume, 0f, Time.deltaTime * 0.3f);
			if (source.volume < 0.001f)
			{
				PlayClip(GetRandomClip(currentTrigger));
			}
		}
		else
		{
			source.volume = Mathf.MoveTowards(source.volume, 0.7f, Time.deltaTime * 0.3f);
		}
	}

	private AudioClip GetRandomClip(AmbienceTrigger targetTrigger)
	{
		if (targetTrigger != null)
		{
			return targetTrigger.clips[Random.Range(0, targetTrigger.clips.Length)];
		}
		return clips[Random.Range(0, clips.Length)];
	}

	private void PlayClip(AudioClip targetClip)
	{
		source.Stop();
		source.clip = targetClip;
		source.time = 0f;
		source.Play();
	}

	private void CheckForChange()
	{
		float num = float.MaxValue;
		AmbienceTrigger ambienceTrigger = null;
		for (int i = 0; i < triggers.Length; i++)
		{
			if (triggers[i].bounds.Contains(MainCamera.instance.transform.position))
			{
				float size = triggers[i].size;
				if (size < num)
				{
					num = size;
					ambienceTrigger = triggers[i];
				}
			}
		}
		if (currentTrigger != ambienceTrigger)
		{
			TriggerChanged(ambienceTrigger);
		}
	}

	private void TriggerChanged(AmbienceTrigger bestTrigger)
	{
		currentTrigger = bestTrigger;
		targetClip = GetRandomClip(currentTrigger);
	}
}
