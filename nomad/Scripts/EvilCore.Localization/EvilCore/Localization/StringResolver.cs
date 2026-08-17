using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace EvilCore.Localization
{
	internal class StringResolver
	{
		private readonly Dictionary<string, string> _cache = new Dictionary<string, string>();

		private readonly LocalizationConfig _config;

		private Locale _fallbackLocale;

		public StringResolver(LocalizationConfig config)
		{
			_config = config;
		}

		public void PreloadAllTables(Action onComplete = null)
		{
			if (LocalizationSettings.Instance == null)
			{
				onComplete?.Invoke();
				return;
			}
			Locale fallbackLocale = GetFallbackLocale();
			List<AsyncOperationHandle> list = new List<AsyncOperationHandle>();
			foreach (LocalizationConfig.TableMapping tableMapping in _config.tableMappings)
			{
				if (!string.IsNullOrEmpty(tableMapping.tableName))
				{
					TryAddTableOp(list, tableMapping.tableName, null);
					if (fallbackLocale != null)
					{
						TryAddTableOp(list, tableMapping.tableName, fallbackLocale);
					}
				}
			}
			int pending = list.Count;
			if (pending == 0)
			{
				onComplete?.Invoke();
				return;
			}
			bool fired = false;
			foreach (AsyncOperationHandle item in list)
			{
				AsyncOperationHandle current2 = item;
				if (current2.IsDone)
				{
					Done();
					continue;
				}
				current2.Completed += delegate
				{
					Done();
				};
			}
			void Done()
			{
				if (!fired && --pending <= 0)
				{
					fired = true;
					onComplete?.Invoke();
				}
			}
		}

		private static void TryAddTableOp(List<AsyncOperationHandle> ops, string tableName, Locale locale)
		{
			try
			{
				AsyncOperationHandle<StringTable> asyncOperationHandle = ((locale == null) ? LocalizationSettings.StringDatabase.GetTableAsync(tableName) : LocalizationSettings.StringDatabase.GetTableAsync(tableName, locale));
				ops.Add(asyncOperationHandle);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[StringResolver] Failed to load table '" + tableName + "': " + ex.Message);
			}
		}

		public string Resolve(string key)
		{
			if (_config.enableCaching && _cache.TryGetValue(key, out var value))
			{
				return value;
			}
			(string tableName, string entryKey) tuple = SplitKey(key);
			string item = tuple.tableName;
			string item2 = tuple.entryKey;
			string fromLocale = GetFromLocale(item, item2, null);
			if (string.IsNullOrEmpty(fromLocale))
			{
				fromLocale = GetFromLocale(item, item2, GetFallbackLocale());
			}
			if (string.IsNullOrEmpty(fromLocale))
			{
				return null;
			}
			if (_config.enableCaching)
			{
				_cache[key] = fromLocale;
			}
			return fromLocale;
		}

		public bool HasEntry(string key)
		{
			if (_cache.ContainsKey(key))
			{
				return true;
			}
			if (LocalizationSettings.Instance == null)
			{
				return false;
			}
			var (tableName, entryKey) = SplitKey(key);
			if (!TableHasEntry(tableName, entryKey, null))
			{
				return TableHasEntry(tableName, entryKey, GetFallbackLocale());
			}
			return true;
		}

		private (string tableName, string entryKey) SplitKey(string key)
		{
			string text = key;
			if (!string.IsNullOrEmpty(_config.keyPrefix) && key.StartsWith(_config.keyPrefix))
			{
				text = key.Substring(_config.keyPrefix.Length);
			}
			string item = _config.defaultTableName;
			string item2 = text;
			int num = text.IndexOf('.');
			if (num > 0)
			{
				string category = text.Substring(0, num);
				string tableName = _config.GetTableName(category);
				if (tableName != null)
				{
					item = tableName;
					item2 = text.Substring(num + 1);
				}
			}
			return (tableName: item, entryKey: item2);
		}

		public void InvalidateCache()
		{
			_cache.Clear();
		}

		private static string GetFromLocale(string tableName, string entryKey, Locale locale)
		{
			if (LocalizationSettings.Instance == null)
			{
				return null;
			}
			try
			{
				StringTable stringTable = ((locale == null) ? LocalizationSettings.StringDatabase.GetTable(tableName) : LocalizationSettings.StringDatabase.GetTable(tableName, locale));
				if (stringTable != null)
				{
					StringTableEntry entry = stringTable.GetEntry(entryKey);
					if (entry != null)
					{
						string localizedValue = entry.LocalizedValue;
						return string.IsNullOrEmpty(localizedValue) ? null : localizedValue;
					}
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		private static bool TableHasEntry(string tableName, string entryKey, Locale locale)
		{
			try
			{
				StringTable stringTable = ((locale == null) ? LocalizationSettings.StringDatabase.GetTable(tableName) : LocalizationSettings.StringDatabase.GetTable(tableName, locale));
				if (stringTable == null)
				{
					return false;
				}
				StringTableEntry entry = stringTable.GetEntry(entryKey);
				return entry != null && !string.IsNullOrEmpty(entry.LocalizedValue);
			}
			catch (Exception)
			{
				return false;
			}
		}

		private Locale GetFallbackLocale()
		{
			if (_fallbackLocale != null)
			{
				return _fallbackLocale;
			}
			if (LocalizationSettings.Instance == null)
			{
				return null;
			}
			string fallbackLocaleCode = _config.fallbackLocaleCode;
			if (string.IsNullOrEmpty(fallbackLocaleCode))
			{
				return null;
			}
			foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
			{
				if (locale.Identifier.Code == fallbackLocaleCode || locale.Identifier.Code.StartsWith(fallbackLocaleCode))
				{
					_fallbackLocale = locale;
					return locale;
				}
			}
			return null;
		}
	}
}
