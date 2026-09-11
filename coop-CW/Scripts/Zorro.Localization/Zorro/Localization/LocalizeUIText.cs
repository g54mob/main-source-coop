using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace Zorro.Localization
{
	public class LocalizeUIText : MonoBehaviour
	{
		public LocalizedString String;

		[SerializeField]
		private bool m_extraStrings;

		[SerializeField]
		private LocalizedString[] m_extraLocalizedStrings;

		private TextMeshProUGUI m_text;

		private void Start()
		{
			m_text = GetComponent<TextMeshProUGUI>();
			String.StringChanged += OnStringChanged;
			OnStringChanged(String.GetLocalizedString());
		}

		private void OnStringChanged(string value)
		{
			string text = "";
			if (m_extraStrings)
			{
				LocalizedString[] extraLocalizedStrings = m_extraLocalizedStrings;
				foreach (LocalizedString localizedString in extraLocalizedStrings)
				{
					text = text + Environment.NewLine + localizedString.GetLocalizedString();
				}
			}
			if (!string.IsNullOrEmpty(value))
			{
				m_text.text = value + text;
			}
		}
	}
}
