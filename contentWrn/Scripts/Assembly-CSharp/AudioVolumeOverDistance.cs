using UnityEngine;

public class AudioVolumeOverDistance : MonoBehaviour
{
	public AnimationCurve VolumeOverDistance = AnimationCurve.EaseInOut(0f, 1f, 100f, 0f);

	private AudioSource _audioSource;

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (!(_audioSource == null) && !(MainCamera.instance == null))
		{
			float time = Vector3.Distance(base.transform.position, MainCamera.instance.transform.position);
			_audioSource.volume = VolumeOverDistance.Evaluate(time);
		}
	}
}
