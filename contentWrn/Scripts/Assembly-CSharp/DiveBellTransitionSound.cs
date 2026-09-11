using System.Collections;
using UnityEngine;

public class DiveBellTransitionSound : MonoBehaviour
{
	public void Init(DiveBellSFX sfx)
	{
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		audioSource.transform.position = sfx.transform.position;
		audioSource.clip = sfx.loopSound;
		audioSource.loop = true;
		audioSource.volume = 0.15f;
		audioSource.outputAudioMixerGroup = sfx.diveBellStartGoingDown.settings.mixerGroup;
		audioSource.Play();
		audioSource.spatialBlend = 0f;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public static void Remove()
	{
		DiveBellTransitionSound diveBellTransitionSound = Object.FindObjectOfType<DiveBellTransitionSound>();
		if (diveBellTransitionSound != null)
		{
			diveBellTransitionSound.StartCoroutine(diveBellTransitionSound.FadeOut());
		}
	}

	private IEnumerator FadeOut()
	{
		float startVolume = GetComponent<AudioSource>().volume;
		float t = 0f;
		while (t < 1f)
		{
			t += Time.deltaTime;
			GetComponent<AudioSource>().volume = Mathf.Lerp(startVolume, 0f, t);
			yield return null;
		}
		Object.Destroy(base.gameObject);
	}
}
