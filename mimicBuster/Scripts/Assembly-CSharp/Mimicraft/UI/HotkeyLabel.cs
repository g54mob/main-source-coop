using Mimicraft.Settings;
using TMPro;
using UnityEngine;

namespace Mimicraft.UI
{
	[DisallowMultipleComponent]
	public class HotkeyLabel : MonoBehaviour
	{
		[Tooltip("Hangi komutun tuşu yazılacak. Listeden seç - elle yazılan bir id, oyuna girip boş bir etiket görene kadar fark edilmez.")]
		[CommandIdPopup]
		[SerializeField]
		private string command;

		[Tooltip("Yazının konacağı etiket. Boş bırakılırsa bu objenin kendi TMP bileşeni aranır.")]
		[SerializeField]
		private TMP_Text label;

		[Tooltip("Tuşun etrafına yazılacak kalıp. {0} tuşun kendisidir - \"[{0}]\" gibi bir şey yazabilirsin. Boş bırakılırsa sadece tuş yazılır.")]
		[SerializeField]
		private string format = "{0}";

		[Tooltip("Tuş adını büyük harfe çevir. Kapalıysa girdi sisteminin verdiği hâliyle yazılır.")]
		[SerializeField]
		private bool upperCase = true;

		[Tooltip("Komut hiçbir tuşa bağlı değilse etiketi tamamen gizle. Kapalıysa boş bir etiket kalır - düzeni bozmaması gereken yerlerde bunu kapalı bırak.")]
		[SerializeField]
		private bool hideWhenUnbound = true;

		public string Command
		{
			get
			{
				return command;
			}
			set
			{
				if (!(command == value))
				{
					command = value;
					Apply();
				}
			}
		}

		private void Awake()
		{
			Resolve();
		}

		private void OnEnable()
		{
			GameInput.BindingsChanged += Apply;
			Apply();
		}

		private void OnDisable()
		{
			GameInput.BindingsChanged -= Apply;
		}

		private void Resolve()
		{
			if (label == null)
			{
				label = GetComponent<TMP_Text>();
			}
		}

		private void Apply()
		{
			Resolve();
			if (label == null || string.IsNullOrEmpty(command))
			{
				return;
			}
			string text = GameInput.DisplayFor(command);
			if (string.IsNullOrEmpty(text))
			{
				label.text = "";
				if (hideWhenUnbound && label.gameObject != base.gameObject)
				{
					label.gameObject.SetActive(value: false);
				}
				else if (hideWhenUnbound)
				{
					label.enabled = false;
				}
				return;
			}
			if (hideWhenUnbound)
			{
				if (label.gameObject != base.gameObject)
				{
					label.gameObject.SetActive(value: true);
				}
				label.enabled = true;
			}
			if (upperCase)
			{
				text = text.ToUpperInvariant();
			}
			label.text = (string.IsNullOrEmpty(format) ? text : string.Format(format, text));
		}
	}
}
