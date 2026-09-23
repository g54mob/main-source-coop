using System;
using System.Collections.Generic;
using Mimicraft.Localization;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWordCategory", menuName = "Mimicraft/Word Category")]
public class WordCategory : ScriptableObject
{
	[Serializable]
	public class LocalizedWordList
	{
		[Tooltip("Dil kodu - 'tr', 'en', 'zh-Hans'. Oyunun dil listesindekiyle birebir aynı olmalı.")]
		public string language = "";

		[Tooltip("O dildeki kelime listesi. Biçim aynı: her satırda bir kelime.")]
		public TextAsset words;
	}

	[Tooltip("Stable identifier sent over the network. Must be unique and must NOT change once a build is out. Deliberately separate from the asset's file name so renaming the asset cannot break a session in progress.")]
	[SerializeField]
	private string categoryId = "";

	[Tooltip("Content tablosundaki çeviri anahtarı. Oyuncunun eklediği kategorilerde boş kalır - o zaman aşağıdaki isim kullanılır.")]
	[SerializeField]
	private string nameKey = "";

	[Tooltip("Name shown in the host's settings panel.")]
	[SerializeField]
	private string displayName = "";

	[Tooltip("Yukarıdaki ana listenin dili. Boş bırakılırsa 'tr' varsayılır - bu projenin kategorileri Türkçe yazıldı.")]
	[SerializeField]
	private string authoredLanguage = "tr";

	[Tooltip("Kategorinin kartında gösterilecek görsel. Opsiyonel - görseli olmayan kategori yine listelenir, sadece düz renkli bir kart olur.")]
	[SerializeField]
	private Sprite preview;

	[Tooltip("Seçicideki sıra. Küçük olan önce gelir. Eşit olanlar ada göre sıralanır. İlk kart açılışta seçili gelir, yani bu aynı zamanda varsayılan kategoriyi belirler.")]
	[SerializeField]
	private int sortOrder;

	[Tooltip("Kelime listesi (.txt). Her satırda bir kelime. Boş satırlar ve # ile başlayan satırlar yok sayılır - açıklama ve gruplama için kullanılabilir.\n\nAşağıdaki dil listesinde karşılığı olmayan bir dil için bu liste kullanılır. Yani bu, kategorinin yazıldığı asıl dildeki listesi.")]
	[SerializeField]
	private TextAsset wordList;

	[Tooltip("Kategorinin diğer dillerdeki kelime listeleri.\n\nBir dilin listesi yoksa kategori o dilde SEÇİLEMEZ - listelenmez bile. Yarım çevrilmiş bir kategoriyi göstermek, oyuncuya kendi dilinde tahmin edemeyeceği kelimeler vermek demek olurdu.")]
	[SerializeField]
	private LocalizedWordList[] translations = new LocalizedWordList[0];

	private readonly Dictionary<string, string[]> parsedByLanguage = new Dictionary<string, string[]>();

	private string[] parsedWords;

	public const int MaxWordLength = 20;

	public string CategoryId => categoryId;

	public string DisplayName
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(nameKey) && Loc.TryGet(nameKey, out var text))
			{
				return text;
			}
			if (!string.IsNullOrWhiteSpace(displayName))
			{
				return displayName;
			}
			return base.name;
		}
	}

	public Sprite Preview => preview;

	public int SortOrder => sortOrder;

	public string[] Words => parsedWords ?? (parsedWords = ParseWords((wordList != null) ? wordList.text : ""));

	public IEnumerable<string> Languages
	{
		get
		{
			if (!string.IsNullOrEmpty(authoredLanguage) && Words.Length >= 3)
			{
				yield return authoredLanguage;
			}
			if (translations == null)
			{
				yield break;
			}
			LocalizedWordList[] array = translations;
			foreach (LocalizedWordList localizedWordList in array)
			{
				if (localizedWordList != null && !string.IsNullOrWhiteSpace(localizedWordList.language) && localizedWordList.language != authoredLanguage && Supports(localizedWordList.language))
				{
					yield return localizedWordList.language;
				}
			}
		}
	}

	public bool IsUsable
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(categoryId))
			{
				return Words.Length >= 3;
			}
			return false;
		}
	}

	public string[] WordsFor(string language)
	{
		if (string.IsNullOrEmpty(language))
		{
			return Words;
		}
		if (parsedByLanguage.TryGetValue(language, out var value))
		{
			return value;
		}
		TextAsset textAsset = FileFor(language);
		string[] array = ((textAsset != null) ? ParseWords(textAsset.text) : Array.Empty<string>());
		parsedByLanguage[language] = array;
		return array;
	}

	public bool Supports(string language)
	{
		return WordsFor(language).Length >= 3;
	}

	private TextAsset FileFor(string language)
	{
		if (language == authoredLanguage)
		{
			return wordList;
		}
		if (translations == null)
		{
			return null;
		}
		LocalizedWordList[] array = translations;
		foreach (LocalizedWordList localizedWordList in array)
		{
			if (localizedWordList != null && localizedWordList.language == language)
			{
				return localizedWordList.words;
			}
		}
		return null;
	}

	public static string[] ParseWords(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return Array.Empty<string>();
		}
		List<string> list = new List<string>();
		HashSet<string> hashSet = new HashSet<string>();
		string[] array = text.Split('\n');
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = array[i].Trim().TrimEnd('\r');
			if (text2.Length != 0 && !text2.StartsWith("#") && hashSet.Add(text2.ToLowerInvariant()))
			{
				if (text2.Length > 20)
				{
					Debug.LogWarning($"[WordCategory] '{text2}' {text2.Length} harf - en fazla {20} " + "olabilir, atlandi.");
				}
				else
				{
					list.Add(text2);
				}
			}
		}
		return list.ToArray();
	}

	public void InitializeRuntime(string id, string display, string[] words, Sprite art, int order, string language = "")
	{
		categoryId = id;
		displayName = display;
		parsedWords = words;
		preview = art;
		sortOrder = order;
		authoredLanguage = (string.IsNullOrEmpty(language) ? authoredLanguage : language);
		parsedByLanguage.Clear();
	}
}
