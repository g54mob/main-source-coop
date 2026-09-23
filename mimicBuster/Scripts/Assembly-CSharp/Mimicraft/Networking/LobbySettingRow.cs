using UnityEngine;

namespace Mimicraft.Networking
{
	public class LobbySettingRow : MonoBehaviour
	{
		[Tooltip("Bu satır hangi ayar(lar)a ait. Mod bunlardan HERHANGİ birini kullanıyorsa satır görünür.\n\nHiçbiri seçilmezse satır her modda görünür - bir başlık ya da ayırıcı için doğru olan da budur.\n\nLobi adı, şifre, Max. oyuncu ve Min. oyuncu satırlarına bu bileşeni HİÇ koyma: onlar lobinin kendi özellikleri, her modda varlar ve bir modun onları kapatabilmesi diye bir şey yok.")]
		[SerializeField]
		private LobbySettingFields fields;

		[Tooltip("Gizlenecek obje. Boş bırakılırsa bu objenin kendisi kullanılır - normal durum.\n\nSatırın kendisinden başka bir şeyi gizlemen gerekiyorsa (mesela satır bir düzen grubunun içindeyse ve grubun tamamı gitmeliyse) burayı doldur.")]
		[SerializeField]
		private GameObject target;

		public bool BelongsTo(GameModeDefinition mode)
		{
			if (fields == LobbySettingFields.None)
			{
				return true;
			}
			if (!(mode == null))
			{
				return mode.Uses(fields);
			}
			return true;
		}

		public bool Apply(GameModeDefinition mode)
		{
			GameObject gameObject = ((target != null) ? target : base.gameObject);
			bool flag = BelongsTo(mode);
			if (gameObject.activeSelf == flag)
			{
				return false;
			}
			gameObject.SetActive(flag);
			return true;
		}
	}
}
