using System;
using System.Collections.Generic;
using System.Text;
using Mimicraft.Customization;
using Mimicraft.Gameplay;
using UnityEngine;
using UnityEngine.Audio;

namespace Mimicraft.UI
{
	public class AudioLibrary : MonoBehaviour
	{
		[Serializable]
		public struct ClipVolume
		{
			public AudioClip clip;

			[Range(0f, 1f)]
			public float volume;
		}

		[Header("Extrude")]
		[Tooltip("Played on each step the extrude drag grows the selection outward.")]
		public AudioClip extrudeStepIncreaseClip;

		[Tooltip("Played on each step the extrude drag shrinks/insets the selection.")]
		public AudioClip extrudeStepDecreaseClip;

		[Tooltip("Minimum seconds between two Extrude step sounds - a fast drag can step several times within one frame-to-frame gap, and without this they would overlap into a buzz instead of a series of distinct clicks.")]
		public float extrudeStepInterval = 0.05f;

		[Header("Paint")]
		[Tooltip("Played while Brush-painting - rate-limited by paintBrushInterval, same reasoning as Extrude's own interval.")]
		public AudioClip paintBrushClip;

		public float paintBrushInterval = 0.08f;

		[Tooltip("Played once per Bucket flood-fill click.")]
		public AudioClip paintBucketClip;

		[Tooltip("Played once per Pattern flood-fill click.")]
		public AudioClip paintPatternClip;

		[Header("Round")]
		public AudioClip prepBeginClip;

		public AudioClip huntBeginClip;

		[Tooltip("Played on Round Sonu for whichever role actually won this round.")]
		public AudioClip roundSuccessClip;

		[Tooltip("Tutorial'da bir adım tamamlanınca çalan kısa kutlama sesi - bir 'ding', bir jingle. Boşsa tur başarı sesi kullanılır.\n\nOn dört adımda on dört kez duyulacak: kısa ve hafif tut.")]
		public AudioClip tutorialStepClip;

		[Tooltip("Played on Round Sonu for whichever role actually lost this round.")]
		public AudioClip roundFailClip;

		[Tooltip("Played once per second in the last 5 seconds of Hazırlık or Av.")]
		public AudioClip timerTickClip;

		[Header("Standart - Silah")]
		[Tooltip("Ateş edildiğinde. DÜNYA sesi - atan da yakındakiler de duyar, ateşin nereden geldiği duyulabilir olmalı.")]
		public AudioClip shotClip;

		[Tooltip("Işınlar hiçbir şeye isabet etmediğinde çalan ceza sesi. Yalnızca ateş edene çalar - kendi ıskasını duyman geri bildirim, başkasınınkini duyman istihbarat olurdu.")]
		public AudioClip missPenaltyClip;

		[Tooltip("Mermi duvara/zemine çarptığında, çarpma noktasında.")]
		public AudioClip wallHitClip;

		[Tooltip("Deathmatch'te birini ÖLDÜRÜNCE atanın kulağında çalan onay sesi. Orada isabet/ıska sesi yok, yalnızca bu. Boşsa isabet tonu çalar.")]
		public AudioClip killConfirmClip;

		[Tooltip("Vurulan (mermi yiyen) karakterin acı sesleri - vurulan kendi kulağında, diğerleri onun olduğu yerden (efor sesi gibi kısık ve boğuk) duyar. Boşsa yumruk yeme sesleri çalar.")]
		public AudioClip[] shotHurtClips = new AudioClip[0];

		[Header("Standart - Hareket")]
		[Tooltip("Yürüyüş adım sesleri. Birden fazla varyant koy - her adımda rastgele biri seçilir. Tek klip de çalışır, ama aynı sesin tekrarı birkaç adımda fark edilir hale gelir.")]
		public AudioClip[] footstepClips = new AudioClip[0];

		[Tooltip("Adımlar arası minimum süre (saniye). Koşarken bu süre kısalır.")]
		public float footstepInterval = 0.45f;

		[Tooltip("Zıplama anındaki efor sesleri (nefes/gerilme). Birden fazla varyant koyabilirsin.")]
		public AudioClip[] jumpEffortClips = new AudioClip[0];

		[Tooltip("Yere iniş sesleri.")]
		public AudioClip[] landClips = new AudioClip[0];

		[Tooltip("Havadaki ikinci zıplamanın efor sesleri. Yerdekinden ayrı, çünkü oyuncunun bunu duyduğunda anlaması gereken şey farklı: ekstra bir hakkı harcadı.")]
		public AudioClip[] doubleJumpClips = new AudioClip[0];

		[Header("Dövüş")]
		[Tooltip("Yumruk SAVURURKEN çıkan efor sesi (nefes/homurtu). Yumruğun isabet edip etmediğinden bağımsız çalar - savuran kişinin kendi sesi.")]
		public AudioClip[] punchEffortClips = new AudioClip[0];

		[Tooltip("Yumruk İSABET ETTİĞİNDE, temas noktasında çalan darbe sesi. Efor sesinden ayrı, çünkü ıskalanan bir yumrukta bu duyulmamalı - ıskaladığını duymak oyunun bilgisi.")]
		public AudioClip[] punchHitClips = new AudioClip[0];

		[Tooltip("Yumruk YİYENİN çıkardığı acı sesi, yiyenin üzerinden. Darbe sesinden ayrı: biri temasın sesi, diğeri canlının tepkisi, ve ikisi aynı anda farklı yerlerden gelir.")]
		public AudioClip[] punchHurtClips = new AudioClip[0];

		[Tooltip("Bir Modelci koşarak bir insana ÇARPIP onu yere düşürdüğünde, ikisinin arasında çalan darbe sesi. Yumruk darbesinden ayrı: bu bir sandığın bir bedene tam hızla girmesi - daha ağır ve daha gövdeli olmalı.\n\nBirden fazla varyant koyarsan sırayla çalar. Boşsa yumruk darbe sesi kullanılır.")]
		public AudioClip[] chargeImpactClips = new AudioClip[0];

		[Header("Standart - Ragdoll")]
		[Tooltip("Yere serilmiş bir vücudun bir yere çarpma sesi. Çarpma HIZINA göre yüksekliği ve perdesi ayarlanarak çalınır (bkz. RagdollImpactFeedback), yani tek bir sertlik için değil orta bir çarpma için ses seç - hafifi kısık ve tiz, sertisi yüksek ve kalın gelir.\n\nBirden fazla varyant koy: bir düşüş art arda birkaç çarpmadır ve aynı klibin tekrarlanması dinleyicinin fark ettiği tek şeydir.")]
		public AudioClip[] ragdollImpactClips = new AudioClip[0];

		[Header("Standart - Ölüm")]
		[Tooltip("Bir Avcı elendiğinde, elendiği yerde. Birden fazla varyant koyabilirsin.")]
		public AudioClip[] hunterDeathClips = new AudioClip[0];

		[Tooltip("Bir Modelci bulunduğunda, bulunduğu yerde.")]
		public AudioClip[] modelerDeathClips = new AudioClip[0];

		[Header("Standart - Modelci sesi")]
		[Tooltip("Modelcinin T ile KENDİ isteğiyle çıkardığı ses, modelin bulunduğu yerde. Avcıyı yanlış tarafa çekmek için kullanılır, yani oyuncunun bilerek harcadığı bir sestir.\n\nBoş bırakılırsa kod içinde üretilen iki tonlu 'çip' sesi çalar - yani bu alanı doldurmadan da her şey çalışır.")]
		public AudioClip tauntClip;

		[Tooltip("Av boyunca belli aralıklarla TÜM Modelcilerin zorla çıkardığı ses (bkz. RoundManager'ın taunt aralığı). Oyuncunun seçimi değil, herkese aynı anda gelen bir an.\n\nYukarıdakinden FARKLI bir ses koy: biri 'bir Modelci bir şey yaptı', diğeri 'saat doldu' demek, ve Avcının ikisini kulaktan ayırabilmesi ikisinin de işe yaramasını sağlayan şey.\n\nBoş bırakılırsa aynı üretilen ses çalar.")]
		public AudioClip forcedTauntClip;

		[Header("Kadın Ses Tipi")]
		[Tooltip("Aşağıdakiler, karakteri Customization'da KADIN ses tipine ayarlanmış oyuncuların çıkardığı sesler. Her biri yukarıdaki erkek karşılığının yerine çalar.\n\nBoş bırakılan her liste için erkek sesi çalar - yani bunları doldurmadan da her şey çalışır, sadece herkes erkek sesiyle konuşur.")]
		public AudioClip[] femaleJumpEffortClips = new AudioClip[0];

		[Tooltip("Havadaki ikinci zıplamanın kadın efor sesleri. Boşsa erkek ses.")]
		public AudioClip[] femaleDoubleJumpClips = new AudioClip[0];

		[Tooltip("Yumruk savururken çıkan kadın efor sesleri. Boşsa erkek ses.")]
		public AudioClip[] femalePunchEffortClips = new AudioClip[0];

		[Tooltip("Yumruk yiyen kadın karakterin acı sesleri. Boşsa erkek ses.")]
		public AudioClip[] femalePunchHurtClips = new AudioClip[0];

		[Tooltip("Vurulan (mermi yiyen) kadın karakterin acı sesleri. Boşsa erkek ses; o da boşsa kadın yumruk yeme sesleri.")]
		public AudioClip[] femaleShotHurtClips = new AudioClip[0];

		[Tooltip("Kadın ses tipli bir Avcı elendiğinde. Boşsa erkek ses.\n\nModelci ölümünün kadın versiyonu yok: bulunan bir Modelci bir insan değil, bir sandık - o ses tek tiptir (yukarıdaki Modeler Death Clips).")]
		public AudioClip[] femaleHunterDeathClips = new AudioClip[0];

		[Header("Standart - Round Sonu")]
		[Tooltip("MVP açıklandığında.")]
		public AudioClip mvpClip;

		[Tooltip("Deathmatch'te maç bitince KAZANANA çalınır ('oh yeah'). Boşsa üstteki MVP sesi kullanılır.")]
		public AudioClip deathmatchMvpClip;

		[Tooltip("Round sonu sineması başlarken.")]
		public AudioClip cinematicInClip;

		[Tooltip("Sinema bitip kamera oyuncuya dönerken.")]
		public AudioClip cinematicOutClip;

		[Header("Guessr")]
		[Tooltip("Tahmin yakın olduğunda - yalnızca tahmin edene.")]
		public AudioClip guessCloseClip;

		[Tooltip("Doğru tahmin. Bilen kişide çalar.")]
		public AudioClip guessCorrectClip;

		[Tooltip("Tur, kimse bilemeden bittiğinde.")]
		public AudioClip guessFailedClip;

		[Tooltip("Sıra sana geldi, üç kelime ekrana düştü.")]
		public AudioClip wordChoiceClip;

		[Tooltip("Oyun bitti - kazanan sensin.")]
		public AudioClip gameWonClip;

		[Tooltip("Oyun bitti - kazanan başkası.")]
		public AudioClip gameLostClip;

		[Header("Edit Fail")]
		[Tooltip("Played whenever an edit is refused - entering Movement mode inside solid geometry, a rejected multiplayer body submission, or a body limit/cohesion violation.")]
		public AudioClip editDeniedClip;

		[Header("Diğer Oyuncuların Sesleri")]
		[Tooltip("BAŞKA bir oyuncunun efor seslerinin (zıplama, çift zıplama, yumruk eforu, yumruk yeme) ses çarpanı. Kendi seslerin etkilenmez. 1 = kendi sesin kadar yüksek.")]
		[Range(0f, 1f)]
		public float otherPlayerEffortVolume = 0.55f;

		[Tooltip("Başka bir oyuncunun efor seslerini boğuklaştıran low-pass filtrenin kesim frekansı, Hz. Tiz ve kulak tırmalayan kısmı keser - ses uzaktan, bir odanın öbür ucundan geliyormuş gibi olur. 2500-4000 arası iyi bir başlangıç; düşük = daha boğuk, 22000 = filtre kapalı. Adım sesleri etkilenmez - Avcı onlarla iz sürüyor.")]
		[Range(500f, 22000f)]
		public float otherPlayerEffortCutoff = 3000f;

		[Header("Mixer")]
		[Tooltip("Efekt seslerinin bagli oldugu AudioMixer grubu. Ayarlar ekranindaki 'Efekt sesleri' kaydiraci bu grubu kisar - atanmazsa efekt sesleri yalnizca ana sesten etkilenir. Her sahnedeki kendi AudioLibrary'sine ayri ayri atanir, kliplerin atandigi gibi.")]
		public AudioMixerGroup sfxGroup;

		[HideInInspector]
		public List<ClipVolume> clipVolumes = new List<ClipVolume>();

		private AudioSource source;

		private float lastExtrudeStepTime = float.NegativeInfinity;

		private float lastPaintBrushTime = float.NegativeInfinity;

		private Dictionary<AudioClip, float> volumeLookup;

		private static int ragdollImpactIndex = -1;

		private static int jumpIndex = -1;

		private static int landIndex = -1;

		private static int doubleJumpIndex = -1;

		private static int hunterDeathIndex = -1;

		private static int modelerDeathIndex = -1;

		private static int footstepIndex = -1;

		private static int punchEffortIndex = -1;

		private static int punchHitIndex = -1;

		private static int punchHurtIndex = -1;

		private static int shotHurtIndex = -1;

		private static int femaleShotHurtIndex = -1;

		private static int chargeImpactIndex = -1;

		private static int femaleJumpIndex = -1;

		private static int femaleDoubleJumpIndex = -1;

		private static int femalePunchEffortIndex = -1;

		private static int femalePunchHurtIndex = -1;

		private static int femaleHunterDeathIndex = -1;

		public static AudioLibrary Instance { get; private set; }

		public static AudioMixerGroup SfxGroup
		{
			get
			{
				if (!(Instance != null))
				{
					return null;
				}
				return Instance.sfxGroup;
			}
		}

		public static float FootstepInterval
		{
			get
			{
				if (!(Instance != null))
				{
					return 0.45f;
				}
				return Instance.footstepInterval;
			}
		}

		private void Awake()
		{
			Instance = this;
			source = base.gameObject.AddComponent<AudioSource>();
			source.playOnAwake = false;
			source.spatialBlend = 0f;
			source.outputAudioMixerGroup = sfxGroup;
			SpatialAudio.FlushPendingRoutes();
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public static void PlayOneShotClip(AudioClip clip)
		{
			if (!(Instance == null) && !(clip == null))
			{
				Instance.source.PlayOneShot(clip, Instance.LookupVolume(clip));
			}
		}

		public static string OutputReport()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (Instance == null)
			{
				stringBuilder.AppendLine("AudioLibrary.Instance: NULL - nothing can play.");
				return stringBuilder.ToString();
			}
			stringBuilder.AppendLine("AudioLibrary: '" + Instance.name + "' in scene '" + Instance.gameObject.scene.name + "'");
			AudioSource audioSource = Instance.source;
			if (audioSource == null)
			{
				stringBuilder.AppendLine("  source: NULL");
			}
			else
			{
				stringBuilder.AppendLine($"  source: enabled={audioSource.enabled} mute={audioSource.mute} volume={audioSource.volume:0.00} " + $"spatialBlend={audioSource.spatialBlend:0.00} (1 = 3D, would need the listener nearby)");
				stringBuilder.AppendLine("  mixer group: " + ((audioSource.outputAudioMixerGroup != null) ? audioSource.outputAudioMixerGroup.name : "none (master)"));
			}
			stringBuilder.AppendLine($"  AudioListener.volume: {AudioListener.volume:0.00}   paused: {AudioListener.pause}");
			AudioListener[] array = UnityEngine.Object.FindObjectsByType<AudioListener>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			stringBuilder.AppendLine($"  AudioListeners in scenes: {array.Length}");
			AudioListener[] array2 = array;
			foreach (AudioListener audioListener in array2)
			{
				stringBuilder.AppendLine($"    '{audioListener.name}' enabled={audioListener.enabled} " + $"activeInHierarchy={audioListener.gameObject.activeInHierarchy} " + "scene='" + audioListener.gameObject.scene.name + "'");
			}
			if (array.Length == 0)
			{
				stringBuilder.AppendLine("    NONE AWAKE - this alone makes the whole game silent.");
			}
			return stringBuilder.ToString();
		}

		public static float VolumeOf(AudioClip clip)
		{
			if (!(Instance != null) || !(clip != null))
			{
				return 1f;
			}
			return Instance.LookupVolume(clip);
		}

		private float LookupVolume(AudioClip clip)
		{
			if (volumeLookup == null)
			{
				volumeLookup = new Dictionary<AudioClip, float>();
				foreach (ClipVolume clipVolume in clipVolumes)
				{
					if (clipVolume.clip != null)
					{
						volumeLookup[clipVolume.clip] = Mathf.Clamp01(clipVolume.volume);
					}
				}
			}
			if (!volumeLookup.TryGetValue(clip, out var value))
			{
				return 1f;
			}
			return value;
		}

		private void OnValidate()
		{
			volumeLookup = null;
		}

		public static void PlayExtrudeStep(bool increased)
		{
			if (!(Instance == null) && !(Time.unscaledTime - Instance.lastExtrudeStepTime < Instance.extrudeStepInterval))
			{
				Instance.lastExtrudeStepTime = Time.unscaledTime;
				PlayOneShotClip(increased ? Instance.extrudeStepIncreaseClip : Instance.extrudeStepDecreaseClip);
			}
		}

		public static AudioClip NextFootstepClip()
		{
			if (!(Instance != null))
			{
				return null;
			}
			return PickVariant(Instance.footstepClips, ref footstepIndex);
		}

		private static AudioClip PickVariant(AudioClip[] clips, ref int lastIndex)
		{
			if (clips == null || clips.Length == 0)
			{
				return null;
			}
			if (clips.Length == 1)
			{
				return clips[0];
			}
			int num = UnityEngine.Random.Range(0, clips.Length - 1);
			if (num >= lastIndex)
			{
				num++;
			}
			lastIndex = num;
			return clips[num];
		}

		public static AudioClip NextJumpEffortClip(CharacterVoice voice = CharacterVoice.Male)
		{
			if (!(Instance != null))
			{
				return null;
			}
			return Voiced(voice, Instance.femaleJumpEffortClips, ref femaleJumpIndex, Instance.jumpEffortClips, ref jumpIndex);
		}

		public static AudioClip NextLandClip()
		{
			if (!(Instance != null))
			{
				return null;
			}
			return PickVariant(Instance.landClips, ref landIndex);
		}

		public static AudioClip NextDoubleJumpClip(CharacterVoice voice = CharacterVoice.Male)
		{
			if (!(Instance != null))
			{
				return null;
			}
			return Voiced(voice, Instance.femaleDoubleJumpClips, ref femaleDoubleJumpIndex, Instance.doubleJumpClips, ref doubleJumpIndex);
		}

		public static AudioClip NextPunchEffortClip(CharacterVoice voice = CharacterVoice.Male)
		{
			if (!(Instance != null))
			{
				return null;
			}
			return Voiced(voice, Instance.femalePunchEffortClips, ref femalePunchEffortIndex, Instance.punchEffortClips, ref punchEffortIndex);
		}

		public static AudioClip NextPunchHitClip()
		{
			if (!(Instance != null))
			{
				return null;
			}
			return PickVariant(Instance.punchHitClips, ref punchHitIndex);
		}

		public static AudioClip NextShotHurtClip(CharacterVoice voice = CharacterVoice.Male)
		{
			if (Instance == null)
			{
				return null;
			}
			if ((Instance.shotHurtClips == null || Instance.shotHurtClips.Length == 0) && (voice != CharacterVoice.Female || Instance.femaleShotHurtClips == null || Instance.femaleShotHurtClips.Length == 0))
			{
				return NextPunchHurtClip(voice);
			}
			return Voiced(voice, Instance.femaleShotHurtClips, ref femaleShotHurtIndex, Instance.shotHurtClips, ref shotHurtIndex);
		}

		public static AudioClip NextPunchHurtClip(CharacterVoice voice = CharacterVoice.Male)
		{
			if (!(Instance != null))
			{
				return null;
			}
			return Voiced(voice, Instance.femalePunchHurtClips, ref femalePunchHurtIndex, Instance.punchHurtClips, ref punchHurtIndex);
		}

		public static AudioClip NextChargeImpactClip()
		{
			if (!(Instance != null))
			{
				return null;
			}
			return PickVariant(Instance.chargeImpactClips, ref chargeImpactIndex);
		}

		public static AudioClip NextRagdollImpactClip()
		{
			if (!(Instance != null))
			{
				return null;
			}
			return PickVariant(Instance.ragdollImpactClips, ref ragdollImpactIndex);
		}

		public static AudioClip PickDeath(bool wasHunter, CharacterVoice voice = CharacterVoice.Male)
		{
			if (Instance == null)
			{
				return null;
			}
			if (!wasHunter)
			{
				return PickVariant(Instance.modelerDeathClips, ref modelerDeathIndex);
			}
			return Voiced(voice, Instance.femaleHunterDeathClips, ref femaleHunterDeathIndex, Instance.hunterDeathClips, ref hunterDeathIndex);
		}

		private static AudioClip Voiced(CharacterVoice voice, AudioClip[] female, ref int femaleIndex, AudioClip[] male, ref int maleIndex)
		{
			if (voice == CharacterVoice.Female && female != null && female.Length != 0)
			{
				return PickVariant(female, ref femaleIndex);
			}
			return PickVariant(male, ref maleIndex);
		}

		private static void PlayFrom(AudioClip[] clips, ref int lastIndex)
		{
			PlayOneShotClip(PickVariant(clips, ref lastIndex));
		}

		public static AudioClip Or(AudioClip preferred, AudioClip fallback)
		{
			if (!(preferred != null))
			{
				return fallback;
			}
			return preferred;
		}

		public static void PlayPaintBrush()
		{
			if (!(Instance == null) && !(Time.unscaledTime - Instance.lastPaintBrushTime < Instance.paintBrushInterval))
			{
				Instance.lastPaintBrushTime = Time.unscaledTime;
				PlayOneShotClip(Instance.paintBrushClip);
			}
		}
	}
}
