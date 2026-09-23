using Mimicraft.Localization;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewGameMode", menuName = "Mimicraft/Game Mode")]
public class GameModeDefinition : ScriptableObject
{
	[Tooltip("Stable identifier sent over the network. Must be unique and must NOT change once a build is out - it is what every client resolves back to this asset. Deliberately separate from the asset's file name so renaming the asset cannot break saved lobbies.")]
	[SerializeField]
	private string modeId = "";

	[Tooltip("Modun adı - string tablosundan. Content tablosunda Mode.*.Name anahtarları hazır.\n\nBoş bırakılırsa aşağıdaki yazılı ad kullanılır.")]
	[SerializeField]
	private LocalizedString localizedName;

	[Tooltip("Çevrilmemiş yedek ad. Yukarıdaki bağlanana kadar, ve hiç bağlanmazsa, görünen budur.")]
	[SerializeField]
	private string displayName = "";

	[Tooltip("Modun kart açıklaması - string tablosundan. Content tablosunda Mode.*.Description anahtarları hazır.\n\nBoş bırakılırsa aşağıdaki yazılı açıklama kullanılır.")]
	[SerializeField]
	private LocalizedString localizedDescription;

	[Tooltip("Çevrilmemiş yedek açıklama.")]
	[SerializeField]
	[TextArea(2, 4)]
	private string description = "";

	[Tooltip("Card artwork for the menu picker. Optional - a mode with none still gets a card.")]
	[SerializeField]
	private Sprite preview;

	[Tooltip("Menüdeki sıra. Küçük olan önce gelir. Eşit olanlar ada göre sıralanır.\n\nİlk kart açılışta seçili gelir, yani bu aynı zamanda varsayılan modu belirler.")]
	[SerializeField]
	private int sortOrder;

	[Tooltip("Scene loaded ADDITIVELY on top of Game when this mode starts. Holds the mode's round logic and its own HUD. Must be in Build Settings or it cannot be loaded at all.")]
	[AssetSelectorPopup("SceneAsset", false)]
	[SerializeField]
	private string modeSceneName = "";

	[Tooltip("Whether this mode is played on a map the LOBBY chooses. False hides and locks the map picker, and the mode brings its own world through Own Map Scene below instead.")]
	[SerializeField]
	private bool usesMap = true;

	[Tooltip("The world this mode plays in when Uses Map is OFF - its own fixed environment, chosen here instead of by the lobby. Ignored entirely while Uses Map is on.\n\nLeave empty for a mode that needs no world at all. Must be in Build Settings.")]
	[AssetSelectorPopup("SceneAsset", false)]
	[SerializeField]
	private string ownMapSceneName = "";

	[Tooltip("Avcıların Hazırlık boyunca bekledikleri oda - haritanın YANINDA, ayrı bir sahne olarak yüklenir. İçinde HunterRoomSpawn türünde MapMarker'lar olmalı; Av başlayınca Avcılar haritadaki HunterSpawn'lara ışınlanır.\n\nBoş bırakılırsa mod avcı odası kullanmaz ve Avcılar eskisi gibi doğrudan haritada başlar. Build Settings'te olmalı.")]
	[AssetSelectorPopup("SceneAsset", false)]
	[SerializeField]
	private string hunterRoomSceneName = "";

	[Tooltip("Avcı odası sahnesinin yükleme anında kaydırılacağı miktar. NORMALDE SIFIR BIRAK - odayı sahnede zaten olması gereken yerde kur.\n\nIşık bakelenmiş bir odada sıfırdan farklı bir değer çalışmaz: 'Batching Static' işaretli mesh'lerin köşe noktaları dünya uzayında tek bir mesh'e gömülüyor, o andan sonra transform artık çizimi sürmüyor. Oda çarpışma ve marker olarak aşağı iner ama görüntüsü yukarıda kalır. Işık probe'ları da dünya uzayında bakelendiği için oradaki oyuncuyu aydınlatan veriler yine eski konumda kalır.\n\nYani bu alan sadece static olmayan, bakelenmemiş bir oda için işe yarar.")]
	[SerializeField]
	private Vector3 hunterRoomOffset = Vector3.zero;

	[Tooltip("Whether this mode lets players build voxel models. When true the shared Editor scene is loaded alongside the mode scene; modes that never model simply never pay for that UI.")]
	[SerializeField]
	private bool usesVoxelEditor = true;

	[Tooltip("Bu içeriğin durumu.\n\nAvailable: oynanabilir.\nComing Soon: kartta 'Yakında' rozetiyle görünür, seçilemez.\nNot Available in Demo: demo sürümünde rozetle görünür ve seçilemez; diğer sürümlerde normal.\nInvisible: oyuncuya hiç listelenmez, seçilemez - Editor'de rozetle görünür.\n\nEditor HEPSİNİ listeler ama erişim kuralları build'dekiyle aynıdır: Coming Soon bir mod Editor'de de kilitlidir. Denemek için geçici olarak Available yap.")]
	[SerializeField]
	private ContentAvailability availability;

	[Tooltip("Lobi kartında bu mod için hangi ROUND ayarlarının sorulacağı.\n\nYazdığı gibi okunur: Nothing = bu modda hiç round ayarı yok, Everything = hepsi var (prop hunt ve deathmatch ayarlarının ikisi birden - büyük ihtimalle istediğin bu değil). Round işletmeyen bir mod (Guessr, Pratik) Nothing olur.\n\nLobi adı ve şifre burada YOK - onlar her modda var, oynanan şeyin değil lobinin özellikleri.\n\nKartın kendisi bunu okumaz; satırlara koyduğun LobbySettingRow bileşenleri okur. Yani burada işaretlemek yetmez, ilgili satırın da hangi ayar olduğunu söylemesi gerekir.")]
	[SerializeField]
	private LobbySettingFields lobbySettings = LobbySettingFields.PrepSeconds | LobbySettingFields.HuntSeconds | LobbySettingFields.RoundEndSeconds | LobbySettingFields.TauntInterval | LobbySettingFields.SelfDamage | LobbySettingFields.HunterShare;

	[Tooltip("Bulunan Modelci, birkaç saniye sonra AVCI olarak geri dönsün mü? Infestation modu budur ve Classic'ten tek farkı bu kutudur - aynı sahneyi, aynı RoundManager'ı, aynı kuralları kullanır.\n\nRound yine tüm Modelciler bulununca biter; fark, bulunanların kalan Modelcilerin peşine düşmesidir.")]
	[SerializeField]
	private bool convertsFoundHiders;

	[Tooltip("Bulunan Modelci'nin Avcı olarak ayağa kalkması için geçen süre (saniye). Ölüm gösterisinin (enkaz + ragdoll) bitmesine yetecek kadar uzun olmalı, yoksa oyuncu kendi enkazının içinde doğar.")]
	[SerializeField]
	[Min(0.5f)]
	private float hiderConversionSeconds = 5f;

	public string ModeId => modeId;

	public string DisplayName => Loc.Resolve(localizedName, string.IsNullOrWhiteSpace(displayName) ? base.name : displayName);

	public string Description => Loc.Resolve(localizedDescription, description);

	public Sprite Preview => preview;

	public int SortOrder => sortOrder;

	public string ModeSceneName => modeSceneName;

	public bool UsesMap => usesMap;

	public string OwnMapSceneName => ownMapSceneName;

	public bool UsesVoxelEditor => usesVoxelEditor;

	public string HunterRoomSceneName => hunterRoomSceneName;

	public Vector3 HunterRoomOffset => hunterRoomOffset;

	public bool UsesHunterRoom => !string.IsNullOrWhiteSpace(hunterRoomSceneName);

	public bool ConvertsFoundHiders => convertsFoundHiders;

	public float HiderConversionSeconds => hiderConversionSeconds;

	public bool IsUsable
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(modeId))
			{
				return !string.IsNullOrWhiteSpace(modeSceneName);
			}
			return false;
		}
	}

	public ContentAvailability Availability => availability;

	public LobbySettingFields LobbySettings => lobbySettings & (LobbySettingFields.PrepSeconds | LobbySettingFields.HuntSeconds | LobbySettingFields.RoundEndSeconds | LobbySettingFields.TauntInterval | LobbySettingFields.SelfDamage | LobbySettingFields.HunterShare | LobbySettingFields.ScoreLimit | LobbySettingFields.TimeLimit | LobbySettingFields.ExtraWarmup | LobbySettingFields.RespawnTime);

	public bool Uses(LobbySettingFields field)
	{
		return (LobbySettings & field) != 0;
	}
}
