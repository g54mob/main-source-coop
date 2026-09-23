using System;
using System.Collections.Generic;
using Mimicraft.Customization;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[CreateAssetMenu(fileName = "NewWeapon", menuName = "Mimicraft/Weapon")]
	public class WeaponDefinition : ScriptableObject
	{
		[Tooltip("Ağ üzerinde gönderilen sabit kimlik. Benzersiz olmalı ve bir build çıktıktan sonra DEĞİŞMEMELİ - her istemci bu asset'e bununla ulaşıyor. Asset'in dosya adından ayrı, böylece dosyayı yeniden adlandırmak kayıtlı hiçbir şeyi bozmuyor.")]
		[SerializeField]
		private string weaponId = "";

		[Tooltip("Oyuncuya gösterilen ad.")]
		[SerializeField]
		private string displayName = "";

		[Header("Balans")]
		[Tooltip("Saniyede kaç atış. 2 = yarım saniyede bir. Sunucu bunu ateş bekleme süresine çevirip zorluyor, yani istemci daha hızlı ateş edemiyor.")]
		[SerializeField]
		[Min(0.05f)]
		private float fireRate = 2f;

		[Tooltip("Bir saçmanın Modelci'ye verdiği hasar. Modelci canı 100. Saçma sayısıyla birlikte düşün: 8 saçma x 34 hasar anında öldürür.")]
		[SerializeField]
		[Min(0f)]
		private int damagePerProjectile = 34;

		[Tooltip("KAFAYA isabet eden saçmanın hasar çarpanı. 1 = fark yok. Vücut her zaman 1 - bkz. PlayerHitZones. Yalnızca insan karakterlerde; voxel model giyen bir Modelci'nin kafası yok.")]
		[SerializeField]
		[Min(0f)]
		private float headshotMultiplier = 2f;

		[Tooltip("Kola ya da bacağa isabet eden saçmanın hasar çarpanı.")]
		[SerializeField]
		[Min(0f)]
		private float limbMultiplier = 0.75f;

		[Tooltip("Delme gücü. 0 = hiçbir şeyi delmez.\n\nMermi, üzerinde PenetrableSurface bileşeni olan bir yüzeye çarpınca yüzeyin Geçirmezlik değeri bu güçten düşülür ve mermi kalanla devam eder; kalan güç bir sonraki yüzeyin değerinden azsa mermi orada durur. PenetrableSurface OLMAYAN yüzeyler bu değer ne olursa olsun delinmez. Geçilen her yüzey hasarı kendi çarpanıyla düşürür.")]
		[SerializeField]
		[Min(0f)]
		private float penetration;

		[Tooltip("Saçmaların nişan çizgisinden sapabileceği en büyük açı (derece). 0 = sapma yok. Küçük hareketli hedefleri vurulabilir yapan asıl ayar bu.")]
		[SerializeField]
		[Min(0f)]
		private float spreadDegrees;

		[Tooltip("Tek atışta çıkan saçma sayısı. 1 = klasik tek çizgi hitscan.")]
		[SerializeField]
		[Min(1f)]
		private int projectilesPerShot = 1;

		[Tooltip("Açıksa sol tık BASILI TUTULDUĞU sürece ateş eder; kapalıysa her atış için ayrı bir tık gerekir.\n\nAtış hızını değiştirmez - iki durumda da Fire Rate neyse o. Değiştirdiği tek şey, tetiğin kendi kendine geri gelip gelmediği.")]
		[SerializeField]
		private bool automatic;

		[Tooltip("Boşa ateş edildiğinde ATANIN kendi canından giden hasar. Avcı canı 100.\n\nSilah başına, çünkü ıskalamanın bedeli silahın kendisiyle birlikte düşünülmesi gereken bir şey: dokuz saçmalı bir pompalı zaten zor ıskalar, tek kurşunluk bir keskin nişancı ıskalamayı asıl riski yapabilir.")]
		[SerializeField]
		[Min(0f)]
		private int missSelfDamage = 10;

		[Tooltip("Oyuncu DURURKEN saçılmanın çarpanı. 1 = Saçılma değeri aynen geçerli, 0 = dururken hiç saçılma yok.")]
		[SerializeField]
		[Min(0f)]
		private float movementSpreadMinMultiplier = 1f;

		[Tooltip("Oyuncu TAM HIZLA koşarken saçılmanın çarpanı. Aradaki her hız ikisi arasında orantılı olarak dağılır.\n\nMin ile eşit yaparsan hareket saçılmayı hiç etkilemez - hareketsiz bir nişancı silahı için doğru olan bu olabilir.")]
		[SerializeField]
		[Min(0f)]
		private float movementSpreadMaxMultiplier = 2.5f;

		[Header("Dürbün")]
		[Tooltip("Açıksa sağ tık BASILI TUTULDUĞU sürece dürbüne girilir: kamera yakınlaşır ve ScopeView ekrana gelir. Kapalıysa sağ tık bu silahta hiçbir şey yapmaz.")]
		[SerializeField]
		private bool scope;

		[Tooltip("Dürbündeyken kameranın görüş açısı. Küçük değer = daha çok yakınlaşma. Normal görüş açısı kameranın kendi ayarı; buraya sadece dürbünlü hâli yazılır.")]
		[SerializeField]
		[Range(5f, 70f)]
		private float scopeFieldOfView = 25f;

		[Tooltip("Dürbündeyken fare hassasiyetinin çarpanı. 1 = değişmez, 0.4 = dürbünde iki buçuk kat daha yavaş.\n\nSilah başına, çünkü doğru değer yakınlaşma miktarına bağlı: dar görüş açısında aynı fare hareketi ekranda çok daha fazla yol kat ediyor, yani tek bir çarpan 4x ve 10x dürbünlerin ikisine birden doğru gelemez.")]
		[SerializeField]
		[Range(0.05f, 2f)]
		private float scopeSensitivityMultiplier = 0.4f;

		[Tooltip("Iskalama hasarını yerken çıkan acı sesinin yüksekliği. 1 = normal, 0 = sessiz.\n\nTarayan silahlarda kısmak için: saniyede birkaç kez tekrarlanan bir acı sesi tek seferlik olandan çok daha yorucu.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float missFeedbackVolume = 1f;

		[Tooltip("Iskalama hasarını yerken ekranın kızarma şiddeti. 1 = normal, 0 = hiç kızarmaz.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float missFeedbackFlash = 1f;

		[Header("Geri tepme")]
		[Tooltip("Her atışta silahın geriye kaçtığı mesafe (metre, yerel). WeaponSway'in kendi üst sınırı hâlâ geçerli - o sınır silaha ait değil, view model'in ekrandan çıkmadan ne kadar gidebileceği.")]
		[SerializeField]
		[Min(0f)]
		private float recoilKickBack = 0.05f;

		[Tooltip("Her atışta namlunun yukarı kalktığı açı (derece).")]
		[SerializeField]
		[Min(0f)]
		private float recoilKickUpDegrees = 6f;

		[Tooltip("Geri tepmenin sönme hızı. 0 = bu silah bir şey söylemiyor, WeaponSway'in kendi ayarı kullanılır. Yükseltmek toparlanmayı hızlandırır.")]
		[SerializeField]
		[Min(0f)]
		private float recoilRecoverySpeed;

		[Tooltip("Ateş ederken kameranın sarsılma miktarı. Geri tepmeden ayrı: biri silahın modelini oynatır, öteki nişanı bozar, ve ikisi aynı silahta farklı olabilir.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float shootTrauma = 0.15f;

		[Header("Geri tepme - kamera (Counter-Strike tarzı sabit desen)")]
		[Tooltip("Namlunun her atışta kameraya verdiği tepmenin BÜYÜKLÜĞÜ, derece. 0 = kamera tepmesi yok, sadece silah modeli tepiyor.\n\nBu bir hasar değil bir sapma: nişan açın değişmiyor, üstüne geçici bir sapma biniyor. Tetiği bıraktığında görüş tam olarak nişan aldığın yere geri yaylanıyor.")]
		[SerializeField]
		[Min(0f)]
		private float recoilMagnitude = 1.6f;

		[Tooltip("Tepme büyüklüğünün atıştan atışa değişimi, derece. Deseni tekdüze olmaktan çıkarır.")]
		[SerializeField]
		[Min(0f)]
		private float recoilMagnitudeVariance = 0.4f;

		[Tooltip("Tepmenin YÖNÜ, derece. 0 = tam yukarı, pozitif = sağa yatık.")]
		[SerializeField]
		[Range(-90f, 90f)]
		private float recoilAngle;

		[Tooltip("Yönün atıştan atışa sapması, derece. Deseni sola-sağa gezdiren şey bu; yatay bileşen sin(açı) olduğu için birkaç derece hiçbir şey yapmaz, 20-40 arası anlamlı.")]
		[SerializeField]
		[Range(0f, 90f)]
		private float recoilAngleVariance = 28f;

		[Tooltip("Deseni üreten tohum. Aynı tohum her zaman aynı deseni verir - oyuncunun ezberleyip karşı çekebilmesini sağlayan şey bu. Şeklini beğenene kadar değiştir, sonra bir daha dokunma: değiştirmek herkesin ezberini siler.")]
		[SerializeField]
		private int recoilSeed;

		[Header("Ses")]
		[Tooltip("Bu silahın ateş sesi. Birden fazla koyarsan her atışta rastgele biri seçilir.\n\nBoş bırakılırsa AudioLibrary'deki ortak silah sesine düşülür - yani doldurmadığın silahlar eskisi gibi aynı sesi verir.")]
		[SerializeField]
		private AudioClip[] fireClips;

		[Tooltip("Ateş sesinin ne kadar uzaktan duyulacağını etkilemez - sadece yüksekliği. Mesafe eğrisi her 3D ses gibi SpatialAudio'dan gelir.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float fireVolume = 0.9f;

		[Tooltip("SUSTURUCULU skinlerin ateş sesi. Oyuncu silah özelleştirmede ses tipini Susturuculu seçtiğinde bunlar çalar.\n\nBoş bırakılırsa yukarıdaki normal sesler kullanılır, sadece daha kısık. Yani ayrı klip vermesen de susturuculu seçenek çalışır, bir sonraki alana bak.")]
		[SerializeField]
		private AudioClip[] suppressedFireClips;

		[Tooltip("Susturuculu sesin yüksekliği. Normalden düşük tutulur - susturucunun tek yaptığı şey bu, hasar ve menzil değişmez (bkz. WeaponSoundKind).")]
		[SerializeField]
		[Range(0f, 1f)]
		private float suppressedFireVolume = 0.45f;

		[Header("Görsel")]
		[Tooltip("BİRİNCİ ŞAHIS görünüm modeli - kollar dahil, WeaponPivot köklü, rig'e ait RigTransform'ları olan tam takım. Sadece WeaponParent'ın altına gider ve yalnızca sahibi görür.\n\nBunu üçüncü şahıs eline KOYMA: içinde ikinci bir çift kol var ve karakterin kemikleriyle aynı isimleri taşıyor.")]
		[SerializeField]
		private GameObject heldPrefab;

		[Header("Voxel modeli")]
		[Tooltip("Oyuncunun bu silahı modelleyeceği kutu, voxel cinsinden. Bir ekseni SIFIR bırakırsan o eksen silahın kendi modeline oturacak şekilde Voxel Size'dan hesaplanır; yazdığın sayı her zaman kazanır.")]
		[SerializeField]
		private Vector3Int skinBoxSize;

		[Tooltip("Bir voxelin kenar uzunluğu, metre. BÜTÜN silahlarda aynı değeri kullan: farklı değerler, tabancanın blokları ile tüfeğin blokları farklı boyda demek.")]
		[SerializeField]
		[Min(0.001f)]
		private float skinVoxelSize = 0.03f;

		[Tooltip("Oyuncunun bu silahı oyarken inebileceği EN AZ voxel sayısı. Altına inen bir oyma geri alınır - kutu içinde ne kalacağı serbest, ama silah yok olamaz. 0 = sınır yok.")]
		[SerializeField]
		[Min(0f)]
		private int skinMinimumVoxels = 24;

		[Tooltip("Kutunun ModelRoot icindeki konumuna eklenen duzeltme, metre. Kutu varsayilan olarak ARKASI ModelRoot'un orijinine gelecek sekilde, X ve Y'de ortalanmis olarak one dogru uzaniyor - boylece her silah govdeye ayni noktadan baslar ve sadece uzadikca one tasar. Bu alan o kuralin uzerine eklenen kucuk bir kaydirma.")]
		[SerializeField]
		private Vector3 skinAnchorOffset;

		[Tooltip("Bu silahin varsayilan voxel modeli - projeye attigin bir .weapons dosyasi. Oyuncu sifirladiginda buna donuyor. Bos birakilirsa silah kendi mesh'ine doner.")]
		[SerializeField]
		private WeaponSkinAsset defaultSkin;

		[Tooltip("Bu silah icin hazir modeller - oyuncunun customization ekranindan secip uzerine calisabilecegi baslangic noktalari. Default Skin ile ayni turden .weapons dosyalari; fark, birinin SIFIRLAMA hedefi, bunlarin ise SECENEK olmasi. Bos birakilabilir - o zaman o silahin preset listesi cikmaz.")]
		[SerializeField]
		private WeaponSkinDefinition[] skinPresets;

		[Tooltip("ÜÇÜNCÜ ŞAHIS modeli - sadece silahın kendisi. Kol yok, WeaponPivot yok, RigTransform yok. Karakterin eline takılır ve HERKES bunu görür.\n\nBoş bırakılırsa karakterin eli boş görünür; birinci şahıs modeli oraya konmaz.")]
		[SerializeField]
		private GameObject tpsPrefab;

		private RecoilPattern pattern;

		public string WeaponId => weaponId;

		public string DisplayName
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(displayName))
				{
					return displayName;
				}
				return base.name;
			}
		}

		public double FireCooldownSeconds => 1.0 / (double)Mathf.Max(fireRate, 0.05f);

		public int DamagePerProjectile => damagePerProjectile;

		public float Penetration => penetration;

		public float SpreadDegrees => spreadDegrees;

		public int ProjectilesPerShot => projectilesPerShot;

		public bool Automatic => automatic;

		public int MissSelfDamage => missSelfDamage;

		public float MissFeedbackVolume => missFeedbackVolume;

		public float MissFeedbackFlash => missFeedbackFlash;

		public bool Scope => scope;

		public float ScopeFieldOfView => scopeFieldOfView;

		public float ScopeSensitivityMultiplier => scopeSensitivityMultiplier;

		public RecoilPattern RecoilPattern
		{
			get
			{
				if (recoilMagnitude <= 0f)
				{
					return null;
				}
				return pattern ?? (pattern = RecoilPattern.Build(recoilSeed, recoilAngle, recoilAngleVariance, recoilMagnitude, recoilMagnitudeVariance));
			}
		}

		public float RecoilKickBack => recoilKickBack;

		public float RecoilKickUpDegrees => recoilKickUpDegrees;

		public float RecoilRecoverySpeed => recoilRecoverySpeed;

		public float ShootTrauma => shootTrauma;

		public GameObject HeldPrefab => heldPrefab;

		public Vector3Int SkinBoxSize => skinBoxSize;

		public float SkinVoxelSize => skinVoxelSize;

		public int SkinMinimumVoxels => skinMinimumVoxels;

		public Vector3 SkinAnchorOffset => skinAnchorOffset;

		public WeaponSkinAsset DefaultSkin => defaultSkin;

		public IReadOnlyList<WeaponSkinDefinition> SkinPresets
		{
			get
			{
				if (skinPresets == null || skinPresets.Length == 0)
				{
					return Array.Empty<WeaponSkinDefinition>();
				}
				List<WeaponSkinDefinition> list = new List<WeaponSkinDefinition>(skinPresets.Length);
				WeaponSkinDefinition[] array = skinPresets;
				foreach (WeaponSkinDefinition weaponSkinDefinition in array)
				{
					if (weaponSkinDefinition != null && weaponSkinDefinition.isActive)
					{
						list.Add(weaponSkinDefinition);
					}
				}
				return list;
			}
		}

		public GameObject TpsPrefab => tpsPrefab;

		public float FireVolume => fireVolume;

		public bool IsUsable => !string.IsNullOrWhiteSpace(weaponId);

		public float DamageMultiplier(HitZone zone)
		{
			return zone switch
			{
				HitZone.Head => headshotMultiplier, 
				HitZone.Limb => limbMultiplier, 
				_ => 1f, 
			};
		}

		public float MovementSpreadMultiplier(float speed01)
		{
			return Mathf.LerpUnclamped(movementSpreadMinMultiplier, movementSpreadMaxMultiplier, Mathf.Clamp01(speed01));
		}

		private void OnValidate()
		{
			pattern = null;
		}

		public float FireVolumeFor(WeaponSoundKind sound)
		{
			if (sound != WeaponSoundKind.Suppressed)
			{
				return fireVolume;
			}
			return suppressedFireVolume;
		}

		public AudioClip PickFireClip()
		{
			return PickFireClip(WeaponSoundKind.Normal);
		}

		public AudioClip PickFireClip(WeaponSoundKind sound)
		{
			AudioClip[] array = ((sound == WeaponSoundKind.Suppressed && suppressedFireClips != null && suppressedFireClips.Length != 0) ? suppressedFireClips : fireClips);
			if (array == null || array.Length == 0)
			{
				return null;
			}
			return array[UnityEngine.Random.Range(0, array.Length)];
		}
	}
}
