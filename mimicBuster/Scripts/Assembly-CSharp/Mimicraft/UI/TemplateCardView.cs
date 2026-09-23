using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class TemplateCardView : MonoBehaviour
	{
		[Tooltip("Şablonun adının yazılacağı metin.")]
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		[Tooltip("Şablonu yükleyen buton.")]
		[SerializeField]
		private Button loadButton;

		[Tooltip("Şablonu silen buton.")]
		[SerializeField]
		private Button deleteButton;

		[Tooltip("Şablonun bir kopyasını çıkaran buton. Kopya '<ad> (2)' olarak kaydedilir ve kendi görseli çekilir.")]
		[SerializeField]
		private Button duplicateButton;

		[Tooltip("Şablonun görselinin çizileceği Image. Görsel yoksa kapatılıyor, yani altına koyacağın bir placeholder ikon kendiliğinden ortaya çıkar.\n\nİsteğe bağlı: bağlanmazsa kart yalnızca ismi gösterir.")]
		[SerializeField]
		private Image thumbnail;

		[Tooltip("Kart seçiliyken açılacak obje - çerçeve, parlama, tik, ne çizdiysen. Kapalı başlaması gerekmiyor; kütüphane her çizimde doğru duruma getiriyor.\n\nİsteğe bağlı: bağlanmazsa seçim, kartın kendi Button renkleriyle gösterilir.")]
		[SerializeField]
		private GameObject selectionIndicator;

		public TextMeshProUGUI NameLabel => nameLabel;

		public Button LoadButton => loadButton;

		public Button DeleteButton => deleteButton;

		public Button DuplicateButton => duplicateButton;

		public GameObject SelectionIndicator => selectionIndicator;

		public Image Thumbnail => thumbnail;

		public bool IsUsable => nameLabel != null;
	}
}
