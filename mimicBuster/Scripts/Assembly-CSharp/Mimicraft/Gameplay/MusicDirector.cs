using System;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Mimicraft.Gameplay
{
	public class MusicDirector : MonoBehaviour
	{
		[Serializable]
		public class Track
		{
			[Tooltip("Sadece Inspector'da listeyi okunur kılmak için. Kod bunu kullanmıyor.")]
			public string name = "";

			[Tooltip("Bu parçanın çaldığı an. Aynı ana birden fazla kayıt koyarsan ilki kullanılır.")]
			public MusicCue cue = MusicCue.MainMenu;

			[Tooltip("Bu ana ait parçalar. Birden fazlaysa, o ana HER girişte rastgele biri seçilir ve an bitene kadar döngüde kalır - aynı avı iki kez aynı müzikle oynamamak için.")]
			public AudioClip[] clips = new AudioClip[0];

			[Tooltip("Bu parçanın kendi ses seviyesi. Master'ları farklı seviyelerde gelen parçaları birbirine eşitlemek için - oyuncunun müzik ayarı bunun üstüne çarpılır.")]
			[Range(0f, 1f)]
			public float volume = 1f;

			public AudioClip Pick()
			{
				if (clips == null || clips.Length == 0)
				{
					return null;
				}
				return clips[UnityEngine.Random.Range(0, clips.Length)];
			}
		}

		public const string ResourcePath = "MusicDirector";

		[Tooltip("Müziğin çıkacağı mixer grubu - AudioMixer içindeki 'Music'. Boş bırakılırsa müzik ana ses üzerinden çalar ve ayarlardaki müzik kaydırıcısı onu etkilemez.")]
		[SerializeField]
		private AudioMixerGroup output;

		[Tooltip("Hangi an hangi parçayı çalar. Bir ana hiç parça atamazsan o anda müzik KESİLMEZ, o sırada çalan devam eder - eksik bir liste sessizlik değil, süreklilik demek.")]
		[SerializeField]
		private Track[] tracks = new Track[0];

		[Tooltip("İki parça arasındaki geçiş süresi (saniye). Biri kısılırken diğeri açılır; faz geçişleri bir müzik cümlesinin ortasına denk geldiği için kesme değil geçiş şart.")]
		[SerializeField]
		[Min(0.1f)]
		private float crossfadeSeconds = 1.5f;

		[Tooltip("Av fazının son kaç saniyesinde 'Closing' parçasına geçileceği. Fazın kendi uzunluğu lobiden ayarlandığı için bu bir ORAN değil, sondan sayılan sabit bir süre.")]
		[SerializeField]
		[Min(0f)]
		private float closingSeconds = 30f;

		[Tooltip("Ana menü sahnesinin adı. Bu sahnedeyken her zaman menü müziği çalar - oyun modu aranmaz, çünkü menüde mod yoktur.")]
		[SerializeField]
		private string menuSceneName = "Menu";

		[Header("Boğuk mod (splash)")]
		[Tooltip("Boğukken tizlerin kesildiği frekans (Hz). Düşürmek müziği daha uzağa, kapalı kapı ardına taşır: 700 civarı 'yan odadan duyuluyor', 2000 sadece hafif bir örtü.")]
		[SerializeField]
		[Range(200f, 5000f)]
		private float muffledCutoff = 700f;

		[Tooltip("Boğukken ses seviyesinin çarpanı. Kapalı kapı ardındaki müzik sadece tizini değil bir miktar gücünü de kaybeder; 1 bırakırsan yalnızca tizler kesilir.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float muffledVolume = 0.85f;

		[Tooltip("Boğukla normal arasındaki geçiş süresi (saniye). Splash'te tuşa basıldığı an başlar - panel geçişi ve kamera hareketiyle aynı anda açılması için onlara yakın tutulmalı.")]
		[SerializeField]
		[Min(0.05f)]
		private float muffleSeconds = 0.8f;

		private const float OpenCutoff = 22000f;

		private AudioSource primary;

		private AudioSource secondary;

		private AudioLowPassFilter primaryFilter;

		private AudioLowPassFilter secondaryFilter;

		private float muffle;

		private float muffleTarget;

		private static bool muffleRequested;

		private MusicCue appliedCue = (MusicCue)(-1);

		private bool inMenuScene;

		private float fade;

		private float primaryVolume = 1f;

		private float secondaryVolume = 1f;

		private bool routed;

		public static MusicDirector Instance { get; private set; }

		public static void Muffle(bool on)
		{
			muffleRequested = on;
			if (Instance != null)
			{
				Instance.muffleTarget = (on ? 1f : 0f);
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void EnsureExists()
		{
			if (!(Instance != null))
			{
				MusicDirector musicDirector = Resources.Load<MusicDirector>("MusicDirector");
				if (!(musicDirector == null))
				{
					UnityEngine.Object.Instantiate(musicDirector).name = musicDirector.name;
				}
			}
		}

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			primary = BuildSource("Music A", out primaryFilter);
			secondary = BuildSource("Music B", out secondaryFilter);
			muffleTarget = (muffleRequested ? 1f : 0f);
			muffle = muffleTarget;
			ApplyMuffle();
			SceneManager.activeSceneChanged += OnActiveSceneChanged;
			inMenuScene = SceneManager.GetActiveScene().name == menuSceneName;
		}

		private void OnDestroy()
		{
			SceneManager.activeSceneChanged -= OnActiveSceneChanged;
			if (Instance == this)
			{
				Instance = null;
			}
		}

		private void OnActiveSceneChanged(Scene from, Scene to)
		{
			inMenuScene = to.name == menuSceneName;
		}

		private AudioSource BuildSource(string sourceName, out AudioLowPassFilter filter)
		{
			GameObject gameObject = new GameObject(sourceName);
			gameObject.transform.SetParent(base.transform, worldPositionStays: false);
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			filter = gameObject.AddComponent<AudioLowPassFilter>();
			filter.cutoffFrequency = 22000f;
			audioSource.playOnAwake = false;
			audioSource.loop = true;
			audioSource.spatialBlend = 0f;
			audioSource.outputAudioMixerGroup = output;
			audioSource.ignoreListenerPause = true;
			return audioSource;
		}

		private void Update()
		{
			EnsureRouted();
			MusicCue musicCue = Resolve();
			if (musicCue != appliedCue)
			{
				Apply(musicCue);
			}
			TickFade();
			TickMuffle();
		}

		private void EnsureRouted()
		{
			if (!routed)
			{
				AudioMixerGroup audioMixerGroup = ((output != null) ? output : SettingsApplier.MusicGroup);
				if (!(audioMixerGroup == null))
				{
					primary.outputAudioMixerGroup = audioMixerGroup;
					secondary.outputAudioMixerGroup = audioMixerGroup;
					routed = true;
				}
			}
		}

		private void TickMuffle()
		{
			if (!Mathf.Approximately(muffle, muffleTarget))
			{
				muffle = Mathf.MoveTowards(muffle, muffleTarget, Time.unscaledDeltaTime / muffleSeconds);
				ApplyMuffle();
				WriteVolumes();
			}
		}

		private void ApplyMuffle()
		{
			float cutoffFrequency = Mathf.Exp(Mathf.Lerp(Mathf.Log(22000f), Mathf.Log(muffledCutoff), muffle));
			if (primaryFilter != null)
			{
				primaryFilter.cutoffFrequency = cutoffFrequency;
			}
			if (secondaryFilter != null)
			{
				secondaryFilter.cutoffFrequency = cutoffFrequency;
			}
		}

		private void WriteVolumes()
		{
			float num = Mathf.Lerp(1f, muffledVolume, muffle);
			primary.volume = primaryVolume * fade * num;
			secondary.volume = secondaryVolume * (1f - fade) * num;
		}

		private MusicCue Resolve()
		{
			if (inMenuScene)
			{
				return MusicCue.MainMenu;
			}
			GameModeController current = GameModeController.Current;
			if (current == null || !current.IsSpawned)
			{
				if (appliedCue != (MusicCue)(-1))
				{
					return appliedCue;
				}
				return MusicCue.None;
			}
			MusicCue music = current.Music;
			if (music == MusicCue.Hunt && closingSeconds > 0f && SecondsLeft(current) <= closingSeconds)
			{
				return MusicCue.Closing;
			}
			return music;
		}

		private static float SecondsLeft(GameModeController mode)
		{
			if (mode.PhaseEndServerTime.Value <= 0.0 || NetworkManager.Singleton == null)
			{
				return float.MaxValue;
			}
			return (float)(mode.PhaseEndServerTime.Value - NetworkManager.Singleton.ServerTime.Time);
		}

		private void Apply(MusicCue cue)
		{
			Track track = Find(cue);
			if (track == null && cue != MusicCue.None)
			{
				appliedCue = cue;
				return;
			}
			appliedCue = cue;
			AudioClip audioClip = track?.Pick();
			if (audioClip != null && primary.clip == audioClip && primary.isPlaying)
			{
				primaryVolume = track.volume;
				return;
			}
			AudioSource audioSource = secondary;
			AudioSource audioSource2 = primary;
			primary = audioSource;
			secondary = audioSource2;
			float num = track?.volume ?? 0f;
			float num2 = primaryVolume;
			primaryVolume = num;
			secondaryVolume = num2;
			primary.clip = audioClip;
			primary.volume = 0f;
			primary.time = 0f;
			if (audioClip != null)
			{
				primary.Play();
			}
			fade = 0f;
		}

		private Track Find(MusicCue cue)
		{
			Track[] array = tracks;
			foreach (Track track in array)
			{
				if (track != null && track.cue == cue && track.Pick() != null)
				{
					return track;
				}
			}
			return null;
		}

		private void TickFade()
		{
			if (!(fade >= 1f))
			{
				fade = Mathf.MoveTowards(fade, 1f, Time.unscaledDeltaTime / crossfadeSeconds);
				WriteVolumes();
				if (!(fade < 1f) && secondary.isPlaying)
				{
					secondary.Stop();
					secondary.clip = null;
				}
			}
		}
	}
}
