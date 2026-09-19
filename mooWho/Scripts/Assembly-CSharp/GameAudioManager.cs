using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
	[Header("Ağ Genelinde (tüm client'lar duyar — GameManager ClientRpc'lerinden çağrılır)")]
	public AudioClip GameStartedSFX;

	public AudioClip Last30SecondsSFX;

	public AudioClip GameOverSFX;

	[Header("Local (sadece bu client duyar — PlayerVoiceMonitor'dan çağrılır)")]
	public AudioClip BuzzingNotificationSFX;

	public AudioClip BuzzingClearedSFX;

	[Header("Hunter Release Geri Sayımı (local — PlayerHUD.UpdateHunterReleaseCountdown'dan çağrılır)")]
	[Tooltip("Hunter release geri sayımının (\"10 9 8 7...\") HER tam saniye düşüşünde tekrar çalınır (10, 9, 8, 7 ... 0'a kadar).")]
	public AudioClip CountdownClockSFX;

	[Tooltip("Geri sayım BİTTİĞİNDE (kapı açılınca) TEK SEFER çalınır (CountdownSfxSource.PlayOneShot).")]
	public AudioClip CountdownSnapSFX;

	[Header("Ses Kaynağı")]
	[Tooltip("Atanmazsa otomatik eklenir (2D, non-spatial — herkes aynı şekilde duyar)")]
	public AudioSource audioSource;

	[Header("Hunter Release Geri Sayımı — Ayrı Ses Kaynağı")]
	[Tooltip("CountdownClockSFX/CountdownSnapSFX BUNUN üzerinden çalınır (yukarıdaki paylaşılan audioSource'tan AYRI) — Clock uzun bir klip olarak Play() ile çalarken PlayGameStarted/PlayLast30Seconds gibi diğer PlayOneShot'ların üstüne binmesin/kesilmesin diye. Atanmazsa otomatik eklenir.")]
	public AudioSource CountdownSfxSource;

	public static GameAudioManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
		}
		if (audioSource == null)
		{
			audioSource = base.gameObject.AddComponent<AudioSource>();
		}
		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 0f;
		if (CountdownSfxSource == null)
		{
			CountdownSfxSource = base.gameObject.AddComponent<AudioSource>();
		}
		CountdownSfxSource.playOnAwake = false;
		CountdownSfxSource.spatialBlend = 0f;
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private void PlayClip(AudioClip clip)
	{
		if (!(clip == null) && !(audioSource == null))
		{
			audioSource.PlayOneShot(clip);
		}
	}

	public void PlayGameStarted()
	{
		PlayClip(GameStartedSFX);
	}

	public void PlayLast30Seconds()
	{
		PlayClip(Last30SecondsSFX);
	}

	public void PlayGameOver()
	{
		PlayClip(GameOverSFX);
	}

	public void PlayBuzzingNotification()
	{
		PlayClip(BuzzingNotificationSFX);
	}

	public void PlayBuzzingCleared()
	{
		PlayClip(BuzzingClearedSFX);
	}

	public void PlayCountdownClock()
	{
		if (!(CountdownClockSFX == null) && !(CountdownSfxSource == null))
		{
			CountdownSfxSource.PlayOneShot(CountdownClockSFX);
		}
	}

	public void PlayCountdownSnap()
	{
		if (!(CountdownSnapSFX == null) && !(CountdownSfxSource == null))
		{
			CountdownSfxSource.PlayOneShot(CountdownSnapSFX);
		}
	}
}
