using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreenController : MonoBehaviour
{
	public VideoPlayer videoPlayer;

	public void Start()
	{
		if (videoPlayer == null)
		{
			Debug.LogError("VideoPlayer component is not assigned.");
		}
		else
		{
			videoPlayer.seekCompleted += OnVideoEnd;
		}
	}

	private void OnVideoEnd(VideoPlayer source)
	{
		SceneManager.LoadScene(1);
	}
}
