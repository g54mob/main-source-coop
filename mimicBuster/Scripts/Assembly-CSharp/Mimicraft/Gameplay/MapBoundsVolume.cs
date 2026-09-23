using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[DisallowMultipleComponent]
	public class MapBoundsVolume : MonoBehaviour
	{
		[Header("Hacim (yerel uzay)")]
		[Tooltip("Kutunun merkezi - bu objenin yerel uzayında.")]
		[SerializeField]
		private Vector3 center;

		[Tooltip("Kutunun boyutu, metre. Sahnede sarı kutunun köşelerinden de sürükleyebilirsin.")]
		[SerializeField]
		private Vector3 size = new Vector3(64f, 32f, 64f);

		[Tooltip("Sığdır düğmelerinin geometrinin çevresine bıraktığı pay, metre. Yukarısı önemli: çatıdaki bir Modelci ya da zıplamasının tepesindeki bir Avcı hâlâ haritadadır.")]
		[SerializeField]
		private float fitMargin = 6f;

		[Header("Bake ayarları")]
		[Tooltip("Hücre boyutu, metre. Küçültmek ince geçitleri ve çatlakları daha iyi yakalar ama bake süresi de veri boyutu da bunun KÜPÜ ile büyür.")]
		[SerializeField]
		private float cellSize = 0.5f;

		[Tooltip("Hangi katmanlar dolu sayılsın. Oyuncular, prop'lar, FPS eli ve Studio sahnesi varsayılan olarak dışarıda - onlar harita değil.")]
		[SerializeField]
		private LayerMask blockingLayers = -1957;

		public const int MapLayers = -1957;

		[Tooltip("Dolgunun başlayacağı noktalar. Boş bırakılırsa hacmin içindeki MapMarker'lar kullanılır - normal olan budur. Doldurursan SADECE bunlar kullanılır.")]
		[SerializeField]
		private Transform[] seeds;

		[Tooltip("Hangi tür MapMarker dolgunun başlangıcı sayılsın. Dolgu binadan taşıp bütün kutuyu dolduruyorsa sebep genelde budur: binanın dışında duran bir marker dışarıyı tohumluyordur.")]
		[SerializeField]
		private MapMarkerKind seedKinds = (MapMarkerKind)(-1);

		[Tooltip("Bir oyuncunun geçebileceği en dar aralık. Bake dolguyu iki kez çalıştırır - biri normal, biri sadece bu genişlikteki yerlerden - ve aradaki fark sızıntıdır. Varsayılan 0.8 = oyuncu genişliği (CharacterController yarıçapı 0.4).")]
		[SerializeField]
		private float narrowThreshold = 0.8f;

		[SerializeField]
		[HideInInspector]
		private Vector3 bakedOrigin;

		[SerializeField]
		[HideInInspector]
		private float bakedCellSize;

		[SerializeField]
		[HideInInspector]
		private Vector3Int bakedSize;

		[SerializeField]
		[HideInInspector]
		private byte[] cells;

		[SerializeField]
		[HideInInspector]
		private byte[] narrow;

		[SerializeField]
		[HideInInspector]
		private Bounds bakedBox;

		[SerializeField]
		[HideInInspector]
		private Vector3[] escapePath;

		[SerializeField]
		[HideInInspector]
		private Vector3 escapePoint;

		[SerializeField]
		[HideInInspector]
		private float escapeWidth;

		[SerializeField]
		[HideInInspector]
		private string bakedAt;

		[SerializeField]
		[HideInInspector]
		private int playCellCount;

		[SerializeField]
		[HideInInspector]
		private int narrowCellCount;

		private static readonly List<MapBoundsVolume> active = new List<MapBoundsVolume>();

		public static IReadOnlyList<MapBoundsVolume> Active => active;

		public bool IsBaked
		{
			get
			{
				if (cells != null && cells.Length != 0 && bakedSize.x > 0 && bakedSize.y > 0)
				{
					return bakedSize.z > 0;
				}
				return false;
			}
		}

		public string BakedAt => bakedAt;

		public bool HasEscapePath
		{
			get
			{
				if (escapePath != null)
				{
					return escapePath.Length > 1;
				}
				return false;
			}
		}

		public Vector3 EscapePoint => base.transform.TransformPoint(escapePoint);

		public float EscapeWidth => escapeWidth;

		public int PlayCellCount => playCellCount;

		public int NarrowCellCount => narrowCellCount;

		public Vector3Int BakedSize => bakedSize;

		public float BakedCellSize => bakedCellSize;

		public int DataBytes
		{
			get
			{
				if (cells == null)
				{
					return 0;
				}
				return cells.Length + ((narrow != null) ? narrow.Length : 0);
			}
		}

		public Bounds LocalBox
		{
			get
			{
				return new Bounds(center, size);
			}
			set
			{
				center = value.center;
				size = value.size;
			}
		}

		public float FitMargin => fitMargin;

		public float CellSize => cellSize;

		public LayerMask BlockingLayers => blockingLayers;

		public Transform[] Seeds => seeds;

		public MapMarkerKind SeedKinds => seedKinds;

		public float NarrowThreshold => narrowThreshold;

		public bool IsStale
		{
			get
			{
				if (IsBaked)
				{
					if (!(bakedBox.center != center) && !(bakedBox.size != size))
					{
						return !Mathf.Approximately(bakedCellSize, cellSize);
					}
					return true;
				}
				return false;
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			active.Clear();
		}

		private void OnEnable()
		{
			active.Add(this);
			MapBoundsWarden.Ensure();
		}

		private void OnDisable()
		{
			active.Remove(this);
		}

		public void UseMapLayers()
		{
			blockingLayers = -1957;
		}

		public void CollectSeeds(List<Vector3> into)
		{
			into.Clear();
			if (seeds != null && seeds.Length != 0)
			{
				Transform[] array = seeds;
				foreach (Transform transform in array)
				{
					if (transform != null)
					{
						into.Add(transform.position);
					}
				}
				if (into.Count > 0)
				{
					return;
				}
			}
			Bounds localBox = LocalBox;
			MapMarker[] array2 = UnityEngine.Object.FindObjectsByType<MapMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (MapMarker mapMarker in array2)
			{
				if ((mapMarker.Kind & seedKinds) != MapMarkerKind.None && localBox.Contains(base.transform.InverseTransformPoint(mapMarker.transform.position)))
				{
					into.Add(mapMarker.transform.position);
				}
			}
		}

		public bool Contains(Vector3 worldPoint)
		{
			if (!IsBaked)
			{
				return false;
			}
			Vector3 vector = base.transform.InverseTransformPoint(worldPoint) - bakedOrigin;
			float num = bakedCellSize;
			if (vector.x < 0f - num || vector.y < 0f - num || vector.z < 0f - num || vector.x > (float)bakedSize.x * bakedCellSize + num || vector.y > (float)bakedSize.y * bakedCellSize + num || vector.z > (float)bakedSize.z * bakedCellSize + num)
			{
				return false;
			}
			int num2 = Mathf.FloorToInt(vector.x / bakedCellSize);
			int num3 = Mathf.FloorToInt(vector.y / bakedCellSize);
			int num4 = Mathf.FloorToInt(vector.z / bakedCellSize);
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					for (int k = -1; k <= 1; k++)
					{
						if (IsPlayArea(num2 + k, num3 + j, num4 + i))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private bool IsPlayArea(int x, int y, int z)
		{
			if (x < 0 || y < 0 || z < 0 || x >= bakedSize.x || y >= bakedSize.y || z >= bakedSize.z)
			{
				return false;
			}
			return GetBit(cells, CellIndex(x, y, z));
		}

		private int CellIndex(int x, int y, int z)
		{
			return (z * bakedSize.y + y) * bakedSize.x + x;
		}

		private static bool GetBit(byte[] set, int index)
		{
			if (set != null)
			{
				return (set[index >> 3] & (1 << (index & 7))) != 0;
			}
			return false;
		}

		public void ApplyEscape(Vector3[] path, Vector3 tightest, float width)
		{
			escapePath = path;
			escapePoint = tightest;
			escapeWidth = width;
		}

		public void ApplyBake(Vector3 origin, float bakedCells, Vector3Int gridSize, byte[] playArea, byte[] narrowCells, int playCells, int narrowCount)
		{
			bakedOrigin = origin;
			bakedBox = LocalBox;
			bakedCellSize = bakedCells;
			bakedSize = gridSize;
			cells = playArea;
			narrow = narrowCells;
			playCellCount = playCells;
			narrowCellCount = narrowCount;
			bakedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
			InvalidateGizmoMeshes();
		}

		public void ClearBake()
		{
			bakedOrigin = default(Vector3);
			bakedBox = default(Bounds);
			bakedCellSize = 0f;
			bakedSize = default(Vector3Int);
			cells = null;
			narrow = null;
			playCellCount = 0;
			narrowCellCount = 0;
			bakedAt = null;
			escapePath = null;
			escapePoint = default(Vector3);
			escapeWidth = 0f;
			InvalidateGizmoMeshes();
		}

		private void InvalidateGizmoMeshes()
		{
		}
	}
}
