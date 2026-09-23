using System;
using System.Collections.Generic;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewMap", menuName = "Mimicraft/Map")]
public class MapScriptableObject : ScriptableObject
{
	[Tooltip("Stable identifier sent over the network. Must be unique and must NOT change once a build is out - it is what every client resolves back to this asset. Deliberately separate from the asset's file name so renaming the asset cannot break saved lobbies.")]
	[SerializeField]
	private string mapId = "";

	[Tooltip("Haritanın adı - string tablosundan. Content tablosunda Map.*.Name anahtarları hazır.\n\nBoş bırakılırsa aşağıdaki yazılı ad kullanılır.")]
	[SerializeField]
	private LocalizedString localizedName;

	[Tooltip("Çevrilmemiş yedek ad. Yukarıdaki bağlanana kadar, ve hiç bağlanmazsa, görünen budur.")]
	[SerializeField]
	private string displayName = "";

	[Tooltip("Haritanın kart açıklaması - string tablosundan. Content tablosunda Map.*.Description anahtarları hazır.\n\nKart üzerinde 'DescriptionLabel' adında bir çocuk varsa oraya yazılır; yoksa hiçbir yerde görünmez ve bu alanı boş bırakmak da geçerlidir.")]
	[SerializeField]
	private LocalizedString localizedDescription;

	[Tooltip("Çevrilmemiş yedek açıklama.")]
	[SerializeField]
	[TextArea(2, 4)]
	private string description = "";

	[Tooltip("Bu haritanin kendi sahnesi. Doluysa harita additive olarak YUKLENIR (prefab yok sayilir) ve bake edilmis lightmap'leri ile isik ayarlari beraberinde gelir. Sahne Build Settings'te ekli olmali. Bos birakilirsa asagidaki prefab instantiate edilir.")]
	[AssetSelectorPopup("SceneAsset", false)]
	[SerializeField]
	private string sceneName = "";

	[Tooltip("Instantiated once when the Game scene opens. Must contain MapMarker components for the Hunter/Hider spawn points, and optionally one for the Hunter door. Sahne yolu kullaniliyorsa gerekmez - o zaman prefab sahnenin ICINDE durur.")]
	[SerializeField]
	private GameObject prefab;

	[Tooltip("Shown on the map's card in the lobby picker. Optional - a map with none still gets a card, just a plain coloured one.")]
	[SerializeField]
	private Sprite preview;

	[Tooltip("Hunter Room'daki projektorun duvara yansittigi gorseller - bir isigin Cookie'si olarak sirayla atanirlar (bkz. ProjectorCookieChanger). Bu haritanin plani, adi, ipuclari, ne istersen; sira buradaki siradir ve her oyuncuda ayni anda ayni kare gorunur. Preview'dan AYRI, cunku ikisi farkli yerlerde farkli isler yapiyor: preview lobide kucuk bir kart gorseli ve Sprite olmak zorunda, bunlar ise bir spot isigin icinden gecen maskeler. Ayni resmi ikisine de atayabilirsin, ama ayni sey olduklari icin degil. Bos birakilirsa projektor sahnede authored edilmis cookie'sinde kalir.")]
	[SerializeField]
	private Texture[] projectorCookies = new Texture[0];

	[Tooltip("Skybox/ambient/fog for this map. Every part is opt-in - a map that overrides nothing leaves the Game scene exactly as authored.")]
	[SerializeField]
	private MapEnvironment environment = new MapEnvironment();

	[Tooltip("Bu harita sadece Editor'de ve development build'de listelenir. Yarım kalmış, greybox ya da test haritaları için. Build'den ÇIKARMAZ - sadece seçilemez yapar.")]
	[SerializeField]
	private bool developmentOnly;

	[Tooltip("Bu içeriğin durumu.\n\nAvailable: oynanabilir.\nComing Soon: kartta 'Yakında' rozetiyle görünür, seçilemez.\nNot Available in Demo: demo sürümünde rozetle görünür ve seçilemez; diğer sürümlerde normal.\nInvisible: oyuncuya hiç listelenmez, seçilemez - Editor'de rozetle görünür.\n\nEditor HEPSİNİ listeler ama erişim kuralları build'dekiyle aynıdır: Coming Soon bir mod Editor'de de kilitlidir. Denemek için geçici olarak Available yap.")]
	[SerializeField]
	private ContentAvailability availability;

	public string MapId => mapId;

	public string DisplayName => Loc.Resolve(localizedName, string.IsNullOrWhiteSpace(displayName) ? base.name : displayName);

	public string Description => Loc.Resolve(localizedDescription, description);

	public GameObject Prefab => prefab;

	public string SceneName => sceneName;

	public bool UsesScene => !string.IsNullOrWhiteSpace(sceneName);

	public Sprite Preview => preview;

	public IReadOnlyList<Texture> ProjectorCookies
	{
		get
		{
			IReadOnlyList<Texture> readOnlyList = projectorCookies;
			return readOnlyList ?? Array.Empty<Texture>();
		}
	}

	public MapEnvironment Environment => environment ?? (environment = new MapEnvironment());

	public bool DevelopmentOnly => developmentOnly;

	public ContentAvailability Availability => availability;

	public bool IsUsable
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(mapId))
			{
				if (!UsesScene)
				{
					return prefab != null;
				}
				return true;
			}
			return false;
		}
	}
}
