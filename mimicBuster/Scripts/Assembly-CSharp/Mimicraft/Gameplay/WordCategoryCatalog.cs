using System;
using System.Collections.Generic;
using System.IO;
using Mimicraft.Localization;
using Mimicraft.Networking;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public static class WordCategoryCatalog
	{
		private const string ResourceFolder = "WordCategories";

		private const string StreamingFolder = "WordCategories";

		private const string CustomIdPrefix = "custom:";

		private const int CustomSortOrder = 1000;

		private static WordCategory[] cached;

		public static IReadOnlyList<WordCategory> All
		{
			get
			{
				if (cached != null)
				{
					return cached;
				}
				List<WordCategory> list = new List<WordCategory>();
				WordCategory[] array = Resources.LoadAll<WordCategory>("WordCategories");
				foreach (WordCategory wordCategory in array)
				{
					if (!(wordCategory == null))
					{
						if (!wordCategory.IsUsable)
						{
							Debug.LogWarning("[WordCategoryCatalog] '" + wordCategory.name + "' eksik (Category Id dolu olmalı ve kelime listesi en az 3 kelime içermeli) - listeye alınmadı.");
						}
						else
						{
							list.Add(wordCategory);
						}
					}
				}
				list.AddRange(LoadCustomCategories());
				list.Sort(delegate(WordCategory a, WordCategory b)
				{
					int num = a.SortOrder.CompareTo(b.SortOrder);
					return (num == 0) ? string.CompareOrdinal(a.DisplayName, b.DisplayName) : num;
				});
				cached = list.ToArray();
				if (cached.Length == 0)
				{
					Debug.LogWarning("[WordCategoryCatalog] Kullanılabilir kategori yok - Gartic modu kelime dağıtamaz.");
				}
				return cached;
			}
		}

		public static WordCategory Default
		{
			get
			{
				if (All.Count <= 0)
				{
					return null;
				}
				return All[0];
			}
		}

		private static IEnumerable<WordCategory> LoadCustomCategories()
		{
			string path = Path.Combine(Application.streamingAssetsPath, "WordCategories");
			List<WordCategory> list = new List<WordCategory>();
			if (!Directory.Exists(path))
			{
				return list;
			}
			string[] files = Directory.GetFiles(path, "*.txt");
			foreach (string path2 in files)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path2);
				if (fileNameWithoutExtension.StartsWith("_"))
				{
					continue;
				}
				try
				{
					string[] array = WordCategory.ParseWords(File.ReadAllText(path2));
					if (array.Length < 3)
					{
						Debug.LogWarning("[WordCategoryCatalog] '" + fileNameWithoutExtension + ".txt' en az 3 kelime içermeli - atlandı.");
						continue;
					}
					WordCategory wordCategory = ScriptableObject.CreateInstance<WordCategory>();
					wordCategory.name = fileNameWithoutExtension;
					wordCategory.InitializeRuntime("custom:" + fileNameWithoutExtension.ToLowerInvariant(), fileNameWithoutExtension, array, LoadSprite(Path.ChangeExtension(path2, ".png")), 1000);
					list.Add(wordCategory);
				}
				catch (Exception ex)
				{
					Debug.LogWarning("[WordCategoryCatalog] '" + fileNameWithoutExtension + ".txt' okunamadı: " + ex.Message);
				}
			}
			return list;
		}

		private static Sprite LoadSprite(string path)
		{
			if (!File.Exists(path))
			{
				return null;
			}
			try
			{
				Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain: false);
				if (!texture2D.LoadImage(File.ReadAllBytes(path)))
				{
					return null;
				}
				return Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[WordCategoryCatalog] '" + Path.GetFileName(path) + "' yüklenemedi: " + ex.Message);
				return null;
			}
		}

		public static List<WordCategory> For(string language)
		{
			List<WordCategory> list = new List<WordCategory>();
			foreach (WordCategory item in All)
			{
				if (item != null && item.Supports(language))
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static List<string> Languages()
		{
			HashSet<string> hashSet = new HashSet<string>();
			List<string> list = new List<string>();
			foreach (WordCategory item in All)
			{
				if (item == null)
				{
					continue;
				}
				foreach (string language in item.Languages)
				{
					if (hashSet.Add(language))
					{
						list.Add(language);
					}
				}
			}
			list.Sort(StringComparer.Ordinal);
			return list;
		}

		public static string DefaultLanguage()
		{
			List<string> list = Languages();
			if (list.Count == 0)
			{
				return "";
			}
			string text = LobbyPrefs.GetString("Gartic.WordLanguage");
			if (!string.IsNullOrEmpty(text) && list.Contains(text))
			{
				return text;
			}
			string language = Loc.Language;
			if (!list.Contains(language))
			{
				return list[0];
			}
			return language;
		}

		public static WordCategory Find(string categoryId)
		{
			if (string.IsNullOrEmpty(categoryId))
			{
				return null;
			}
			foreach (WordCategory item in All)
			{
				if (item.CategoryId == categoryId)
				{
					return item;
				}
			}
			return null;
		}

		public static string DisplayName(string categoryId)
		{
			WordCategory wordCategory = Find(categoryId);
			if (wordCategory != null)
			{
				return wordCategory.DisplayName;
			}
			if (string.IsNullOrEmpty(categoryId))
			{
				return "Bilinmeyen kategori";
			}
			if (!categoryId.StartsWith("custom:"))
			{
				return categoryId;
			}
			return categoryId.Substring("custom:".Length);
		}

		public static void Invalidate()
		{
			cached = null;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			cached = null;
		}
	}
}
