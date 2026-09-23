using TMPro;
using UnityEngine;

namespace Mimicraft.Localization
{
	[DisallowMultipleComponent]
	public class LocalizedText : MonoBehaviour
	{
		[Tooltip("Metin tablosundaki anahtar - Resources/Localization/strings.csv'nin ilk sütunu. Boş bırakılırsa bu bileşen hiçbir şey yapmaz ve etikette yazan ne ise o kalır.")]
		[SerializeField]
		private string key;

		[Tooltip("Yazının konacağı etiket. Boş bırakılırsa bu objenin kendi TMP bileşeni aranır.")]
		[SerializeField]
		private TMP_Text label;

		private object[] args;

		public string Key
		{
			get
			{
				return key;
			}
			set
			{
				if (!(key == value))
				{
					key = value;
					Apply();
				}
			}
		}

		public void SetArgs(params object[] values)
		{
			args = values;
			Apply();
		}

		public static LocalizedText Attach(TMP_Text label, string key, params object[] values)
		{
			if (label == null)
			{
				return null;
			}
			LocalizedText localizedText = label.GetComponent<LocalizedText>();
			if (localizedText == null)
			{
				localizedText = label.gameObject.AddComponent<LocalizedText>();
			}
			localizedText.label = label;
			localizedText.key = key;
			localizedText.args = ((values != null && values.Length != 0) ? values : null);
			localizedText.Apply();
			return localizedText;
		}

		public static LocalizedText AttachIfUnset(TMP_Text label, string key)
		{
			if (label == null)
			{
				return null;
			}
			LocalizedText component = label.GetComponent<LocalizedText>();
			if (component != null && !string.IsNullOrEmpty(component.key))
			{
				return component;
			}
			return Attach(label, key);
		}

		private void Awake()
		{
			Resolve();
		}

		private void OnEnable()
		{
			Loc.Changed += Apply;
			Apply();
		}

		private void OnDisable()
		{
			Loc.Changed -= Apply;
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
			if (!(label == null) && !string.IsNullOrEmpty(key) && Loc.TryGet(key, out var text))
			{
				label.text = ((args == null || args.Length == 0) ? text : Loc.Format(key, args));
			}
		}
	}
}
