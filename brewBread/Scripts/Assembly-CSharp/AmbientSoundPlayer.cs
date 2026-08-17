using System.Collections;
using UnityEngine;

public class AmbientSoundPlayer : MonoBehaviour
{
	[SerializeField]
	private AudioClip[] windClips;

	[SerializeField]
	private Vector2 windFrequency;

	[SerializeField]
	private AudioClip[] birdsClips;

	[SerializeField]
	private Vector2 birdsFrequency;

	private AudioSource source;

	private void Start()
	{
		source = GetComponent<AudioSource>();
		StartCoroutine(PlayWind());
		StartCoroutine(PlayBird());
	}

	private IEnumerator PlayWind()
	{
		while (true)
		{
			float seconds = Random.Range(windFrequency.x, windFrequency.y);
			yield return new WaitForSeconds(seconds);
			int num = Random.Range(0, windClips.Length);
			source.PlayOneShot(windClips[num]);
		}
	}

	private IEnumerator PlayBird()
	{
		float seconds = Random.Range(birdsFrequency.x, birdsFrequency.y);
		yield return new WaitForSeconds(seconds);
		int num = Random.Range(0, birdsClips.Length);
		source.PlayOneShot(birdsClips[num]);
	}
}
