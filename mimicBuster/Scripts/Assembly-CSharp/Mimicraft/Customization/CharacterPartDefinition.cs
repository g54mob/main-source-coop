using UnityEngine;

namespace Mimicraft.Customization
{
	[CreateAssetMenu(fileName = "NewCharacterPart", menuName = "Mimicraft/Character Part")]
	public class CharacterPartDefinition : ScriptableObject, IVoxelBox
	{
		[Tooltip("Kaydedilen karakterin bu parçayı bulmak için kullandığı sabit kimlik. Benzersiz olmalı ve bir build çıktıktan sonra DEĞİŞMEMELİ. Asset adından ayrı, böylece dosyayı yeniden adlandırmak kimsenin karakterini bozmuyor.")]
		[SerializeField]
		private string partId = "";

		[Tooltip("Özelleştirme menüsünde görünen ad.")]
		[SerializeField]
		private string displayName = "";

		[Tooltip("Bu parçanın asılacağı kemik. Humanoid avatar üzerinden bulunuyor - isimle değil, çünkü kemik isimleri modele göre değişiyor, HumanBodyBones değişmiyor.")]
		[SerializeField]
		private HumanBodyBones bone = HumanBodyBones.Head;

		[Tooltip("Parçanın VARSAYILAN kutusu, voxel cinsinden - hiç dokunulmamış bir karakterde bu parça bu boyutta doğar, ve voxel boyu da buradan hesaplanır (kutu uzvun boyuna otursun diye).\n\nOyuncunun ne kadar büyütebileceği ayrı bir alan: Box Size Limit.")]
		[SerializeField]
		private Vector3Int boxSize = new Vector3Int(8, 8, 8);

		[Tooltip("Kutunun ÇÖZÜNÜRLÜK çarpanı. 2 yazmak 8x8x8 bir kutuyu 16x16x16 yapar - parça aynı büyüklükte kalır, sadece voxelleri yarı boyuta iner, yani daha ince modellenebilir.\n\nBox Size'ı elle 16 yazmak yerine bunun olmasının sebebi: kayıtlı karakterler. Çarpanı büyüttüğünde eski kayıtlar yüklenirken voxelleri bloklar hâlinde büyütülür, yani kafa küçük kalmaz. Box Size'ın kendisi parçanın oranlarını anlatır ve dokunulmadan kalır.")]
		[SerializeField]
		[Min(1f)]
		private int boxSizeMultiplier = 1;

		[Tooltip("Varsayılan kutunun HER YÖNE kaç voxel büyüyebileceği. 2 yazmak, kutunun altı yönünün hepsinde 2 voxel fazladan yer açar - yani 8x8x8 bir kutu 12x12x12 olur ve varsayılan şekil tam ortasında kalır.\n\nBoyut değil PAYI olmasının sebebi: sadece 'daha büyük olabilir' demek kutuyu kendi (0,0,0) köşesinden büyütür, yani fazladan yerin hepsi tek tarafa gider ve parça simetrik olarak büyütülemez.\n\n0 = büyüyemez, parça doğduğu kutunun içinde kalır.")]
		[SerializeField]
		private Vector3Int boxSizeExtend = Vector3Int.zero;

		[Tooltip("Kapalıyken (varsayılan) voxel boyu, bu kemikteki mevcut mesh'in Box Size'a bölünmesiyle kendiliğinden bulunur - yani Box Size'ı yazmak yetiyor, kutu uzvun boyuna oturuyor.\n\nAçarsan aşağıdaki değer aynen kullanılır.")]
		[SerializeField]
		private bool overrideVoxelSize;

		[Tooltip("Bir voxelin kenar uzunluğu, metre. Minecraft ölçeğinde ~0.0625 (1/16). Yalnızca Override Voxel Size açıkken kullanılır; kapalıyken ölçüm başarısız olursa yedek değer olarak devreye girer.")]
		[SerializeField]
		[Min(0.001f)]
		private float voxelSize = 0.0625f;

		[Tooltip("Kutunun kemiğe göre yerleşimi.\n\nCharacterAssembler'da Centre On Bone Mesh AÇIKSA (varsayılan): kutu, mevcut mesh'in bu kemik etrafındaki hacminin tam ortasına oturtulur ve bu değer onun üstüne eklenen bir düzeltmedir - yani genelde sıfıra yakın kalır.\n\nKAPALIYSA: kutunun (0,0,0) köşesi doğrudan bu noktaya oturur.")]
		[SerializeField]
		private Vector3 anchorOffset = Vector3.zero;

		[Tooltip("Kutunun kemiğe göre dönüşü. Çoğu parçada sıfır; ters bakan bir kemik varsa burada düzeltilir.")]
		[SerializeField]
		private Vector3 anchorEuler = Vector3.zero;

		[Header("Zorunlu çekirdek")]
		[Tooltip("Asla silinemeyecek bölgenin kutu içindeki başlangıcı.")]
		[SerializeField]
		private Vector3Int requiredMin = Vector3Int.zero;

		[Tooltip("Asla silinemeyecek bölgenin boyutu. Bu hacim her zaman dolu kalır - vücudun saçma görünmesini engelleyen şey bu.\n\nSıfır bırakılırsa o parçanın zorunlu bölgesi yoktur ve tamamen boşaltılabilir.")]
		[SerializeField]
		private Vector3Int requiredSize = Vector3Int.zero;

		public string PartId => partId;

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

		public HumanBodyBones Bone => bone;

		public int BoxSizeMultiplier => Mathf.Max(1, boxSizeMultiplier);

		public Vector3Int BoxSize => boxSize * BoxSizeMultiplier;

		public Vector3Int AuthoredBoxSize => boxSize;

		public Vector3Int BoxExtend => new Vector3Int(Mathf.Max(0, boxSizeExtend.x), Mathf.Max(0, boxSizeExtend.y), Mathf.Max(0, boxSizeExtend.z)) * BoxSizeMultiplier;

		public Vector3Int LimitMin => -BoxExtend;

		public Vector3Int LimitSize => BoxSize + BoxExtend * 2;

		public bool OverrideVoxelSize => overrideVoxelSize;

		public float VoxelSize => voxelSize / (float)BoxSizeMultiplier;

		public Vector3 AnchorOffset => anchorOffset;

		public Quaternion AnchorRotation => Quaternion.Euler(anchorEuler);

		public Vector3Int RequiredMin => requiredMin * BoxSizeMultiplier;

		public Vector3Int RequiredSize => requiredSize * BoxSizeMultiplier;

		public bool HasRequiredCore
		{
			get
			{
				if (requiredSize.x > 0 && requiredSize.y > 0)
				{
					return requiredSize.z > 0;
				}
				return false;
			}
		}

		public bool IsUsable
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(partId) && boxSize.x > 0 && boxSize.y > 0)
				{
					return boxSize.z > 0;
				}
				return false;
			}
		}

		public bool IsInsideBox(Vector3Int cell)
		{
			Vector3Int limitMin = LimitMin;
			Vector3Int vector3Int = limitMin + LimitSize;
			if (cell.x >= limitMin.x && cell.y >= limitMin.y && cell.z >= limitMin.z && cell.x < vector3Int.x && cell.y < vector3Int.y)
			{
				return cell.z < vector3Int.z;
			}
			return false;
		}

		public bool IsRequired(Vector3Int cell)
		{
			if (!HasRequiredCore)
			{
				return false;
			}
			Vector3Int vector3Int = RequiredMin;
			Vector3Int vector3Int2 = vector3Int + RequiredSize;
			if (cell.x >= vector3Int.x && cell.x < vector3Int2.x && cell.y >= vector3Int.y && cell.y < vector3Int2.y && cell.z >= vector3Int.z)
			{
				return cell.z < vector3Int2.z;
			}
			return false;
		}

		private void OnValidate()
		{
			boxSize = Vector3Int.Max(boxSize, Vector3Int.one);
			requiredMin = Vector3Int.Max(requiredMin, Vector3Int.zero);
			requiredSize = Vector3Int.Max(requiredSize, Vector3Int.zero);
			if (HasRequiredCore)
			{
				requiredMin = Vector3Int.Min(requiredMin, boxSize - Vector3Int.one);
				requiredSize = Vector3Int.Min(requiredSize, boxSize - requiredMin);
			}
		}
	}
}
