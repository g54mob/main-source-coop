using UnityEngine;

namespace EvilCore.Localization
{
	public class LocalizationKeyAttribute : PropertyAttribute
	{
		public string CategoryFilter { get; }

		public LocalizationKeyAttribute(string categoryFilter = null)
		{
			CategoryFilter = categoryFilter;
		}
	}
}
