using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private AudioClip[] tracks;

	[SerializeField]
	private bool randomFirstTrack = true;

	[SerializeField]
	private float delayBetweenTracks = 10f;

	private int index;

	private bool waitingForNext;

	private void Start()
	{
		if (tracks.Length != 0)
		{
			index = (randomFirstTrack ? Random.Range(0, tracks.Length) : 0);
			PlayNext();
		}
	}

	private void Update()
	{
		if (!source.isPlaying && !waitingForNext)
		{
			StartCoroutine(PlayNextAfterDelay());
		}
	}

	private IEnumerator PlayNextAfterDelay()
	{
		waitingForNext = true;
		yield return new WaitForSeconds(delayBetweenTracks);
		PlayNext();
		waitingForNext = false;
	}

	private void PlayNext()
	{
		source.clip = tracks[index];
		source.Play();
		index = (index + 1) % tracks.Length;
	}
}
