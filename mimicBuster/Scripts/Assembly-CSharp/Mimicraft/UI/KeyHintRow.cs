using Mimicraft.Localization;
using Mimicraft.Settings;
using TMPro;
using UnityEngine;

namespace Mimicraft.UI
{
	public class KeyHintRow : MonoBehaviour
	{
		[Tooltip("Hangi komut. Listeden seç - elle yazılan bir id, oyuna girip boş bir tuş görene kadar fark edilmez.")]
		[CommandIdPopup]
		[SerializeField]
		private string commandId;

		[Tooltip("Tuşun yazılacağı yer - 'Tab', 'Left Ctrl'. Boş bırakılabilir.")]
		[SerializeField]
		private TextMeshProUGUI keyLabel;

		[Tooltip("Komutun adının yazılacağı yer. Boş bırakılırsa komut adı hiç yazılmaz - satırın yazısını kendin döşediysen bunu boş bırak.")]
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		[Tooltip("Komutun kendi adı yerine kullanılacak çeviri anahtarı. Boş bırakılırsa komutun tuş atamaları ekranında görünen adı kullanılır.\n\nBuradaki amaç aynı komutu farklı bir cümleyle anlatabilmek: tuş atamalarında 'Editör/Oyun' yazan komut, saklanan bir Modelciye 'Düzenlemeye geç' diye gösterilebilir.")]
		[SerializeField]
		private string nameKeyOverride;

		[Tooltip("Tuşun etrafına ne yazılacağı. {0} tuşun kendisi - '[{0}]' ya da '{0} tuşu' gibi.")]
		[SerializeField]
		private string keyFormat = "{0}";

		[Tooltip("Komut bulunamazsa ya da hiçbir tuşa bağlı değilse bu satırı tamamen gizler. Kapatırsan satır boş bir tuşla görünür.")]
		[SerializeField]
		private bool hideWhenUnbound = true;

		private bool warned;

		private void OnEnable()
		{
			GameInput.BindingsChanged += Apply;
			Loc.Changed += Apply;
			Apply();
		}

		private void OnDisable()
		{
			GameInput.BindingsChanged -= Apply;
			Loc.Changed -= Apply;
		}

		public void SetCommand(string id)
		{
			commandId = id;
			Apply();
		}

		private void Apply()
		{
			if (!GameInput.TryFind(commandId, out var command))
			{
				if (!string.IsNullOrEmpty(commandId) && !warned)
				{
					warned = true;
					Debug.LogWarning("[KeyHintRow] '" + base.name + "': '" + commandId + "' diye bir komut yok - yeniden adlandirilmis olabilir. Satir gizlendi.", this);
				}
				SetShown(!hideWhenUnbound);
				return;
			}
			string displayString = command.DisplayString;
			if (string.IsNullOrEmpty(displayString) && hideWhenUnbound)
			{
				SetShown(shown: false);
				return;
			}
			SetShown(shown: true);
			if (keyLabel != null)
			{
				keyLabel.text = (string.IsNullOrEmpty(keyFormat) ? displayString : string.Format(keyFormat, displayString));
			}
			if (nameLabel != null)
			{
				nameLabel.text = (string.IsNullOrEmpty(nameKeyOverride) ? command.Label : Loc.Get(nameKeyOverride));
			}
		}

		private void SetShown(bool shown)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				if (child.gameObject.activeSelf != shown)
				{
					child.gameObject.SetActive(shown);
				}
			}
			if (base.transform.childCount == 0)
			{
				if (keyLabel != null && keyLabel.enabled != shown)
				{
					keyLabel.enabled = shown;
				}
				if (nameLabel != null && nameLabel.enabled != shown)
				{
					nameLabel.enabled = shown;
				}
			}
		}
	}
}
