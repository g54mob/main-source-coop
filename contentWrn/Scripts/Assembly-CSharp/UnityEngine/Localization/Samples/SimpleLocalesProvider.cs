using System.Collections.Generic;
using UnityEngine.Localization.Settings;

namespace UnityEngine.Localization.Samples
{
	public class SimpleLocalesProvider : ILocalesProvider
	{
		public List<Locale> Locales { get; } = new List<Locale>();

		public Locale GetLocale(LocaleIdentifier id)
		{
			return Locales.Find((Locale l) => l.Identifier == id);
		}

		public void AddLocale(Locale locale)
		{
			Locales.Add(locale);
		}

		public bool RemoveLocale(Locale locale)
		{
			return Locales.Remove(locale);
		}
	}
}
