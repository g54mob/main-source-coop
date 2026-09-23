using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class MapMarker : MonoBehaviour
	{
		[Tooltip("Hangi oyun modlarına ait. Birden fazlası seçilebilir; Everything = her mod kullanabilir. Önce bunu seç, sonra aşağıdaki türü - hangi türlerin anlamlı olduğu moda göre değişir. Hiçbiri seçili değilse bu marker hiçbir modda kullanılmaz.")]
		[SerializeField]
		private MapMarkerModes modes = (MapMarkerModes)(-1);

		[Tooltip("Bu nokta ne işe yarıyor. Birden fazlası seçilebilir - aynı yer hem lobi hem Hider spawn'ı olabilir, ki çoğu haritada öyledir. Hunter/Hider/Lobby spawn'ları istediğin kadar olabilir; RoundManager aralarında sırayla dağıtıyor. HunterDoor'un en fazla bir tane olması bekleniyor.")]
		[SerializeField]
		private MapMarkerKind kinds = MapMarkerKind.HiderSpawn;

		[SerializeField]
		[HideInInspector]
		private int kind = -1;

		public MapMarkerModes Modes => modes;

		public MapMarkerKind Kind
		{
			get
			{
				Migrate();
				return kinds;
			}
		}

		public bool Is(MapMarkerKind purpose)
		{
			return (Kind & purpose) != 0;
		}

		private void Awake()
		{
			Migrate();
		}

		private void OnValidate()
		{
			Migrate();
		}

		private void Migrate()
		{
			if (kind >= 0)
			{
				kinds = kind switch
				{
					0 => MapMarkerKind.HunterSpawn, 
					1 => MapMarkerKind.HiderSpawn, 
					2 => MapMarkerKind.HunterDoor, 
					3 => MapMarkerKind.LobbySpawn, 
					4 => MapMarkerKind.ModelerSpawn, 
					_ => kinds, 
				};
				kind = -1;
			}
		}

		public bool AppliesTo(MapMarkerModes target)
		{
			return (modes & target) != 0;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = (Is(MapMarkerKind.HunterDoor) ? new Color(0.95f, 0.85f, 0.3f, 1f) : (Is(MapMarkerKind.HunterRoomSpawn) ? new Color(0.95f, 0.55f, 0.15f, 1f) : (Is(MapMarkerKind.HunterSpawn) ? new Color(0.95f, 0.4f, 0.3f, 1f) : (Is(MapMarkerKind.HiderSpawn) ? new Color(0.35f, 0.75f, 0.95f, 1f) : (Is(MapMarkerKind.ModelerSpawn) ? new Color(0.95f, 0.6f, 0.95f, 1f) : (Is(MapMarkerKind.LobbySpawn) ? new Color(0.5f, 0.95f, 0.5f, 1f) : new Color(0.6f, 0.6f, 0.6f, 1f)))))));
			if (Is(MapMarkerKind.HunterDoor))
			{
				Gizmos.DrawWireCube(base.transform.position, Vector3.one);
				return;
			}
			Gizmos.DrawWireSphere(base.transform.position + Vector3.up * 0.9f, 0.4f);
			Gizmos.DrawRay(base.transform.position + Vector3.up * 0.9f, base.transform.forward * 1.2f);
		}
	}
}
