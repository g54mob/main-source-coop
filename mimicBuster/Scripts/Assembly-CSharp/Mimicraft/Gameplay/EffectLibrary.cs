using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class EffectLibrary : MonoBehaviour
	{
		[Tooltip("Spawned where a shot strikes level geometry. Aligned to the surface normal.")]
		[SerializeField]
		private GameObject wallHitPrefab;

		[Tooltip("Spawned where a shot strikes a Modelci.")]
		[SerializeField]
		private GameObject modelHitPrefab;

		[Tooltip("Spawned when a Modelci is destroyed.")]
		[SerializeField]
		private GameObject killPrefab;

		[Tooltip("Spawned under a Hunter's feet on their mid-air second jump. Faces UP, unlike the impact effects, so author it as a puff spreading along its own +Z.")]
		[SerializeField]
		private GameObject doubleJumpPrefab;

		[Tooltip("Spawned where a punch connects. Faces the puncher, so author it as a puff spreading along its own +Z - the same convention the double jump puff uses.")]
		[SerializeField]
		private GameObject hitDustPrefab;

		[Tooltip("Mermi duvarda bıraktığı iz - delik decal'i. Duvara isabet eden her atışta, toz efektinin YANINDA doğar; ikisi ayrı şeyler, biri anlık patlama diğeri kalıcı iz.\n\nKendi +Z'si DUVARIN İÇİNE bakacak şekilde doğuruluyor - URP Decal Projector'ın projeksiyon yönü budur. Bunun yerine düz bir quad kullanıyorsan prefabin içinde Y'de 180 döndürmen yeterli.")]
		[SerializeField]
		private GameObject bulletHolePrefab;

		[Tooltip("Ragdoll halindeki bir vucudun bir yere carptigi noktada dogar - toz, kir, ne istersen. Carpma HIZINA gore olceklenerek doguruluyor (bkz. RagdollImpactFeedback), yani prefabi orta siddetteki bir carpma icin ayarla; hafifi kucuk, serti buyuk gelir.")]
		[SerializeField]
		private GameObject ragdollImpactPrefab;

		[Tooltip("Modelci bir duvara/tavana YAPIŞTIĞI anda, modelle yüzeyin tam temas noktasında doğar. +Z'si yüzeyden DIŞARI bakar - duvar toz efektiyle aynı yön kuralı. Boş bırakılırsa kod kendi küçük toz patlamasını kullanır.")]
		[SerializeField]
		private GameObject wallClingPrefab;

		[Tooltip("Modelci tutunduğu yüzeyi BIRAKTIĞI anda (zıplama, kenardan çıkma, yere inme) aynı noktada doğar. Boşsa yapışma prefabı kullanılır; ikisi aynı efekt olabilir.")]
		[SerializeField]
		private GameObject wallReleasePrefab;

		[Tooltip("Yapışma/bırakma efektinin yarıçap çarpanı.\n\nYarıçap modelin kendi boyutundan hesaplanıyor (duvara bakan yüzünün ortalama yarı genişliği), bu sayı da onun üstüne çarpılıyor. 1 = tam modelin genişliği, 2 = iki katı.\n\nPrefabdaki Shape > Radius değeri her doğuşta ÜZERİNE yazılıyor, yani orayı büyütmek işe yaramaz - ayar buradan yapılır.")]
		[SerializeField]
		[Min(0f)]
		private float clingRadiusScale = 1f;

		[Tooltip("Bir Modelci koşarak bir insana ÇARPIP onu yere düşürdüğünde, ikisinin arasında doğan darbe efekti. +Z'si Modelci'ye doğru bakar - yani efekt çarpılan kişiden geri, çarpanın geldiği yöne püskürür.\n\nModelci'nin boyutuyla ölçekleniyor (0.75x-2x), yani prefabı varsayılan boyuttaki bir Modelci için ayarla. Boşsa kod kendi toz patlamasını kullanır.")]
		[SerializeField]
		private GameObject chargeImpactPrefab;

		[Tooltip("Tutorial'da bir adım tamamlanınca modelin TEPESİNDE doğan kutlama - konfeti, kıvılcım. +Z'si yukarı bakar; çift zıplama efektiyle aynı yön kuralı. Boşsa kod kendi altın rengi patlamasını kullanır.")]
		[SerializeField]
		private GameObject tutorialCelebrationPrefab;

		[Header("Yumruk - yüzeyler")]
		[Tooltip("Bir yumruk KİMSEYE değil de bir şeye isabet ettiğinde ne olacağı, yüzey yüzey. Plastiğe vurmakla metale vurmak farklı ses çıkarsın diye katman bazlı: her kayda kendi katmanlarını, efektini ve seslerini veriyorsun.\n\nSıra önemli - yukarıdan aşağı bakılır, eşleşen ilk kayıt kullanılır. Hiçbiri eşleşmezse aşağıdaki varsayılan kayıt devreye girer, yani listeyi boş bırakmak da geçerli bir kurulum: her şey aynı sesi çıkarır.")]
		[SerializeField]
		private PunchSurface[] punchSurfaces = new PunchSurface[0];

		[Tooltip("Yukarıdaki listede hiçbir katman eşleşmediğinde kullanılan yüzey. Tanımlanmamış her duvarın sesi budur - bunu doldurmak, listeyi doldurmaktan daha önemli.")]
		[SerializeField]
		private PunchSurface defaultPunchSurface = new PunchSurface
		{
			name = "Varsayılan"
		};

		[Tooltip("How long a spawned effect lives before it is destroyed. Long enough for the longest particle in the prefab to finish.")]
		[SerializeField]
		[Min(0.1f)]
		private float effectLifetime = 3f;

		[Tooltip("Mermi izinin ne kadar süre duvarda kalacağı, saniye. Diğer efektlerden ayrı bir süre, çünkü ayrı bir şey ölçüyor: toz efektinin ömrü animasyonunun bitmesi demek, izin ömrü ise oyuncunun geçmiş atışları ne kadar süre görebileceği - bir tercih.\n\nUzun tutmak bedava değil: her atış bir obje bırakıyor ve hiçbiri süresi dolmadan gitmiyor, yani sayı ateş hızı çarpı bu süre kadar oluyor.")]
		[SerializeField]
		[Min(0.1f)]
		private float bulletHoleLifetime = 20f;

		[Header("İzli mermi")]
		[Tooltip("Her saçma için namludan çarptığı yere uçan iz. Kapatınca hiç çizilmez.")]
		[SerializeField]
		private bool tracersEnabled = true;

		[Tooltip("İzin prefab'ı - üzerinde bir LineRenderer olmalı (BulletTracer eklenir). Boşsa aşağıdaki renk ve kalınlıkla düz bir çizgi kodda oluşturulur.")]
		[SerializeField]
		private GameObject tracerPrefab;

		[Tooltip("İzin uçuş hızı, m/sn.")]
		[SerializeField]
		[Min(1f)]
		private float tracerSpeed = 250f;

		[Tooltip("Parlak izin uzunluğu, metre.")]
		[SerializeField]
		[Min(0.05f)]
		private float tracerLength = 1.5f;

		[Tooltip("Kodda oluşturulan çizginin kalınlığı, metre. Prefab varsa kullanılmaz.")]
		[SerializeField]
		[Min(0.001f)]
		private float tracerWidth = 0.02f;

		[Tooltip("Kodda oluşturulan çizginin rengi. Prefab varsa kullanılmaz.")]
		[SerializeField]
		private Color tracerColor = new Color(1f, 0.85f, 0.5f, 1f);

		private const float MarkSurfaceOffset = 0.01f;

		public static EffectLibrary Instance { get; private set; }

		public GameObject WallHitPrefab => wallHitPrefab;

		public GameObject ModelHitPrefab => modelHitPrefab;

		public GameObject KillPrefab => killPrefab;

		public GameObject DoubleJumpPrefab => doubleJumpPrefab;

		public GameObject HitDustPrefab => hitDustPrefab;

		public GameObject BulletHolePrefab => bulletHolePrefab;

		public GameObject RagdollImpactPrefab => ragdollImpactPrefab;

		public GameObject WallClingPrefab => wallClingPrefab;

		public GameObject WallReleasePrefab => wallReleasePrefab;

		public GameObject ChargeImpactPrefab => chargeImpactPrefab;

		public GameObject TutorialCelebrationPrefab => tutorialCelebrationPrefab;

		public static float ClingRadiusScale
		{
			get
			{
				if (!(Instance != null))
				{
					return 1f;
				}
				return Instance.clingRadiusScale;
			}
		}

		public float EffectLifetime => effectLifetime;

		public float BulletHoleLifetime => bulletHoleLifetime;

		public bool TracersEnabled => tracersEnabled;

		public GameObject TracerPrefab => tracerPrefab;

		public float TracerSpeed => tracerSpeed;

		public float TracerLength => tracerLength;

		public float TracerWidth => tracerWidth;

		public Color TracerColor => tracerColor;

		public static PunchSurface FindPunchSurface(int layer)
		{
			if (Instance == null)
			{
				return null;
			}
			PunchSurface[] array = Instance.punchSurfaces;
			foreach (PunchSurface punchSurface in array)
			{
				if (punchSurface != null && punchSurface.Matches(layer))
				{
					return punchSurface;
				}
			}
			return Instance.defaultPunchSurface;
		}

		private void Awake()
		{
			Instance = this;
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public static bool TrySpawn(GameObject prefab, Vector3 position, Vector3 normal, float scale = 1f)
		{
			GameObject spawned;
			return TrySpawn(prefab, position, normal, out spawned, scale);
		}

		public static bool TrySpawn(GameObject prefab, Vector3 position, Vector3 normal, out GameObject spawned, float scale = 1f)
		{
			spawned = null;
			if (prefab == null)
			{
				return false;
			}
			Quaternion rotation = ((normal.sqrMagnitude > 0.001f) ? Quaternion.LookRotation(normal) : Quaternion.identity);
			spawned = Object.Instantiate(prefab, position, rotation);
			if (!Mathf.Approximately(scale, 1f))
			{
				spawned.transform.localScale *= scale;
			}
			ImpactEffects.Adopt(spawned);
			Object.Destroy(spawned, (Instance != null) ? Instance.EffectLifetime : 3f);
			return true;
		}

		public static bool TrySpawnMark(GameObject prefab, Vector3 position, Vector3 normal, float lifetime, float rotation = 0f)
		{
			if (prefab == null)
			{
				return false;
			}
			bool num = normal.sqrMagnitude > 0.001f;
			Quaternion rotation2 = (num ? (Quaternion.LookRotation(-normal.normalized) * Quaternion.Euler(0f, 0f, rotation)) : Quaternion.Euler(0f, 0f, rotation));
			Vector3 position2 = (num ? (position + normal.normalized * 0.01f) : position);
			GameObject obj = Object.Instantiate(prefab, position2, rotation2);
			ImpactEffects.Adopt(obj);
			Object.Destroy(obj, Mathf.Max(0.1f, lifetime));
			return true;
		}
	}
}
