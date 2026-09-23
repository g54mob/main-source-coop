using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartupSceneManager : MonoBehaviour
{
	public string nextSceneName = "Menu";

	public VideoPlayer videoPlayer;

	[Tooltip("Sırayla oynatılacak videolar - ör. önce yayıncının, sonra stüdyonun. Hepsi bitince sahne geçer. Boş bırakılırsa Video Player'a atanmış tek klip oynatılır (eski davranış).")]
	[SerializeField]
	private VideoClip[] videos = new VideoClip[0];

	[Tooltip("Any key or click cuts the intro short. Leave on - an intro that cannot be skipped is a tax on every launch after the first.")]
	[SerializeField]
	private bool skippable = true;

	[Tooltip("Açıkken bir tuşa basmak yalnızca O ANKİ videoyu geçer ve sıradaki başlar. Kapalıyken tek basış bütün introyu atlayıp menüye geçer.")]
	[SerializeField]
	private bool skipAdvancesOneVideo = true;

	[Tooltip("Safety net for a video that never reports finishing (a stall, or a source with no known length). Added on top of the clip's own length when there is one.")]
	[SerializeField]
	[Min(1f)]
	private float extraTimeoutSeconds = 5f;

	private const float FallbackLengthSeconds = 15f;

	private readonly List<VideoClip> queue = new List<VideoClip>();

	private int current = -1;

	private int lastAdvanceFrame = -1;

	private AsyncOperation menuLoad;

	private float deadline;

	private bool continuing;

	private void Start()
	{
		BeginPreloadingMenu();
		if (videoPlayer == null)
		{
			Debug.LogWarning("[StartupSceneManager] Video Player bağlanmamış - doğrudan menüye geçiliyor.");
			Continue();
			return;
		}
		videoPlayer.isLooping = false;
		videoPlayer.loopPointReached += OnVideoFinished;
		videoPlayer.errorReceived += OnVideoError;
		BuildQueue();
		if (queue.Count == 0)
		{
			Debug.LogWarning("[StartupSceneManager] Oynatılacak video yok - doğrudan menüye geçiliyor.");
			Continue();
		}
		else
		{
			PlayNext();
		}
	}

	private void BuildQueue()
	{
		queue.Clear();
		if (videos != null)
		{
			VideoClip[] array = videos;
			foreach (VideoClip videoClip in array)
			{
				if (videoClip != null)
				{
					queue.Add(videoClip);
				}
			}
		}
		if (queue.Count == 0 && videoPlayer.clip != null)
		{
			queue.Add(videoPlayer.clip);
		}
	}

	private void OnDestroy()
	{
		if (!(videoPlayer == null))
		{
			videoPlayer.loopPointReached -= OnVideoFinished;
			videoPlayer.errorReceived -= OnVideoError;
		}
	}

	private void Update()
	{
		if (continuing)
		{
			return;
		}
		if (skippable && SkipPressed())
		{
			if (skipAdvancesOneVideo)
			{
				Advance();
			}
			else
			{
				Continue();
			}
		}
		else if (Time.unscaledTime >= deadline)
		{
			Debug.LogWarning("[StartupSceneManager] Video zamanında bitmedi - sıradakine geçiliyor.");
			Advance();
		}
	}

	private static bool SkipPressed()
	{
		if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
		{
			return true;
		}
		if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
		{
			return true;
		}
		if (Gamepad.current != null)
		{
			return Gamepad.current.startButton.wasPressedThisFrame;
		}
		return false;
	}

	private void OnVideoFinished(VideoPlayer source)
	{
		Advance();
	}

	private void OnVideoError(VideoPlayer source, string message)
	{
		Debug.LogWarning("[StartupSceneManager] Video oynatılamadı (" + message + ") - sıradakine geçiliyor.");
		Advance();
	}

	private void Advance()
	{
		if (!continuing && lastAdvanceFrame != Time.frameCount)
		{
			lastAdvanceFrame = Time.frameCount;
			PlayNext();
		}
	}

	private void PlayNext()
	{
		current++;
		if (current >= queue.Count)
		{
			Continue();
			return;
		}
		VideoClip videoClip = queue[current];
		if (current != 0 || !(videoPlayer.clip == videoClip) || !videoPlayer.isPlaying)
		{
			videoPlayer.source = VideoSource.VideoClip;
			videoPlayer.clip = videoClip;
			videoPlayer.Play();
		}
		double length = videoClip.length;
		deadline = Time.unscaledTime + (float)((length > 0.0) ? length : 15.0) + extraTimeoutSeconds;
	}

	private void BeginPreloadingMenu()
	{
		if (string.IsNullOrWhiteSpace(nextSceneName))
		{
			Debug.LogError("[StartupSceneManager] Next Scene Name boş - geçilecek sahne yok.");
			return;
		}
		menuLoad = SceneManager.LoadSceneAsync(nextSceneName);
		if (menuLoad == null)
		{
			Debug.LogError("[StartupSceneManager] '" + nextSceneName + "' yüklenemedi - File > Build Profiles içindeki sahne listesinde olduğundan emin ol.");
		}
		else
		{
			menuLoad.allowSceneActivation = false;
		}
	}

	private void Continue()
	{
		if (!continuing)
		{
			continuing = true;
			if (videoPlayer != null && videoPlayer.isPlaying)
			{
				videoPlayer.Stop();
			}
			if (menuLoad != null)
			{
				menuLoad.allowSceneActivation = true;
			}
			else
			{
				Debug.LogError("[StartupSceneManager] Menü sahnesi yüklenemediği için intro'da kalındı.");
			}
		}
	}
}
