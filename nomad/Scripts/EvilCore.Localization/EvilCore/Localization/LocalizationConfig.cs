using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.Localization
{
	[CreateAssetMenu(menuName = "EvilCore/Localization/Config", fileName = "LocalizationConfig")]
	public class LocalizationConfig : ScriptableObject
	{
		[Serializable]
		public class TableMapping
		{
			public string category;

			public string tableName;
		}

		public enum TemperatureUnit
		{
			Celsius = 0,
			Fahrenheit = 1
		}

		[Header("General")]
		public string keyPrefix = "@";

		public string defaultTableName = "Default";

		public string fallbackLocaleCode = "en";

		[Header("Table Mappings")]
		public List<TableMapping> tableMappings = new List<TableMapping>();

		[Header("Cache")]
		public bool enableCaching = true;

		[Header("Fonts")]
		public LocaleFontConfig fontConfig;

		[Header("Formatting")]
		public TemperatureUnit defaultTemperatureUnit;

		[Header("Debug")]
		public bool logMissingKeys = true;

		public string missingKeyFormat = "[!{0}]";

		public bool showMissingKeyVisualIndicator = true;

		[Tooltip("When true, missing interactable name localizations render as empty (clean). When false, falls back to the english interactableName (verbose, for debugging).")]
		public bool hideMissingNames = true;

		public PseudoLocalizationMode defaultPseudoMode;

		[Header("Validation")]
		public bool enableLengthValidation;

		public int defaultMaxCharacters = 200;

		public float expansionWarningThreshold = 1.3f;

		public string GetTableName(string category)
		{
			foreach (TableMapping tableMapping in tableMappings)
			{
				if (string.Equals(tableMapping.category, category, StringComparison.OrdinalIgnoreCase))
				{
					return tableMapping.tableName;
				}
			}
			return null;
		}
	}
}
