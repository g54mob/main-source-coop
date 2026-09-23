using Mimicraft.Customization;
using Mimicraft.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class WeaponChoiceButton : MonoBehaviour
	{
		[Tooltip("Bu butonun verdiği silah.")]
		[SerializeField]
		private WeaponDefinition weapon;

		[Tooltip("Tıklanan buton. Boşsa bu objedeki Button.")]
		[SerializeField]
		private Button button;

		[Tooltip("Silahın adının yazılacağı metin. İsteğe bağlı - boşsa yazıya dokunulmaz, kendin yazarsın.")]
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		[Tooltip("Silahın görselinin konacağı Image. Oyuncunun O SİLAH İÇİN seçtiği kayıtlı skin'in görseli kullanılır - yani tezgâhta ne yaptıysan burada onu görürsün.\n\nİsteğe bağlı. Görsel yoksa (hiç kaydedilmemiş ya da resmi daha çekilmemiş bir silah) Image kapatılır, aşağıdaki yedek obje varsa o açılır.")]
		[SerializeField]
		private Image previewImage;

		[Tooltip("Görsel yokken açılacak yer tutucu - kutu ikonu, soru işareti. İsteğe bağlı.")]
		[SerializeField]
		private GameObject previewPlaceholder;

		[Tooltip("Bu silahın kısayol tuşunun yazılacağı metin - '1', '2'. İsteğe bağlı; metin kontrol şemasından gelir, yani tuşu değiştirirsen burası da değişir.")]
		[SerializeField]
		private TextMeshProUGUI hotkeyLabel;

		[Tooltip("Bu silah elindeyken AÇILACAK obje - çerçeve, tik işareti. İsteğe bağlı.")]
		[SerializeField]
		private GameObject selectedIndicator;

		[Tooltip("Seçiliyken / değilken rengi değişecek grafik. İsteğe bağlı - boşsa renk değişmez.")]
		[SerializeField]
		private Graphic tintTarget;

		[SerializeField]
		private Color selectedColor = new Color(0.18f, 0.35f, 0.31f, 0.95f);

		[SerializeField]
		private Color unselectedColor = new Color(0.16f, 0.19f, 0.19f, 0.95f);

		private bool? shownSelected;

		public WeaponDefinition Weapon => weapon;

		public void Bind(UnityAction onClick)
		{
			if (button == null)
			{
				button = GetComponent<Button>();
			}
			if (button != null)
			{
				button.onClick.RemoveListener(onClick);
				button.onClick.AddListener(onClick);
			}
			if (nameLabel != null && weapon != null)
			{
				nameLabel.text = weapon.DisplayName;
			}
		}

		public void RefreshPreview()
		{
			if (!(previewImage == null))
			{
				Sprite sprite = ((weapon != null) ? SavedThumbnails.Load(WeaponSkinStorage.WornPath(weapon.WeaponId)) : null);
				if (previewImage.sprite != sprite)
				{
					previewImage.sprite = sprite;
				}
				if (previewImage.enabled != (sprite != null))
				{
					previewImage.enabled = sprite != null;
				}
				if (previewPlaceholder != null && previewPlaceholder.activeSelf != (sprite == null))
				{
					previewPlaceholder.SetActive(sprite == null);
				}
			}
		}

		public void SetHotkey(string display)
		{
			if (!(hotkeyLabel == null))
			{
				if (hotkeyLabel.text != display)
				{
					hotkeyLabel.text = display;
				}
				bool flag = !string.IsNullOrEmpty(display);
				if (hotkeyLabel.gameObject.activeSelf != flag)
				{
					hotkeyLabel.gameObject.SetActive(flag);
				}
			}
		}

		public void SetSelected(bool selected)
		{
			if (shownSelected != selected)
			{
				shownSelected = selected;
				if (selectedIndicator != null && selectedIndicator.activeSelf != selected)
				{
					selectedIndicator.SetActive(selected);
				}
				if (tintTarget != null)
				{
					tintTarget.color = (selected ? selectedColor : unselectedColor);
				}
			}
		}
	}
}
