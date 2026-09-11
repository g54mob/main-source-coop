using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Zorro.Core;

namespace DefaultNamespace
{
	[CreateAssetMenu(fileName = "LostFootageDatabase", menuName = "LostFootageDatabase")]
	public class LostFootageDatabase : SingletonAsset<LostFootageDatabase>
	{
		[Serializable]
		public class FootageRarityPair
		{
			public FileInfo fileInfo;

			public int score;

			public string name;

			public int rarity;

			public static bool FromFileInfo(FileInfo info, out FootageRarityPair pair)
			{
				pair = null;
				string text = info.Name.Replace(info.Extension, "");
				Debug.Log("noext: " + text);
				string[] array = text.Split("Rarity");
				if (array.Length != 2)
				{
					Debug.LogError("RaritySplit failed " + info.FullName + " has invalid name " + info.Name);
					return false;
				}
				string text2 = array[0];
				string[] array2 = array[1].Split("Score");
				if (array2.Length != 2)
				{
					Debug.LogError("ScoreSplit Failed " + info.FullName + " has invalid name " + info.Name);
					return false;
				}
				string text3 = array2[0];
				string text4 = array2[1];
				int num = -1;
				try
				{
					num = int.Parse(text3);
				}
				catch (Exception value)
				{
					Debug.Log("Invalid rarity " + text3 + " /n " + info.FullName + " ");
					Console.WriteLine(value);
					return false;
				}
				if (num < 0 || num > 1000)
				{
					Debug.LogError("inforrectRarity");
					return false;
				}
				int num2 = -1;
				try
				{
					num2 = int.Parse(text4);
				}
				catch (Exception value2)
				{
					Debug.Log("Invalid score " + text4 + " /n " + info.FullName + " ");
					Console.WriteLine(value2);
					return false;
				}
				text2 = text2.Replace('_', ' ');
				text2 = text2.Trim();
				Debug.Log($"Name: {text2} Rarity{num}");
				pair = new FootageRarityPair
				{
					fileInfo = info,
					name = text2,
					rarity = num,
					score = num2
				};
				return true;
			}
		}

		public List<FootageRarityPair> footageRarityPairs;

		public void Init()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, "LostFootage"));
			if (!directoryInfo.Exists)
			{
				Debug.LogError("Cant find lost footage folder! " + directoryInfo.FullName);
			}
			footageRarityPairs = new List<FootageRarityPair>();
			FileInfo[] array = (from x in directoryInfo.GetFiles()
				where x.Extension == ".webm"
				select x).ToArray();
			for (int num = 0; num < array.Length; num++)
			{
				if (FootageRarityPair.FromFileInfo(array[num], out var pair))
				{
					footageRarityPairs.Add(pair);
				}
			}
		}

		public void PrintChances()
		{
			int num = 100;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int i = 0; i < num; i++)
			{
				int randomLostFootageIndex = GetRandomLostFootageIndex();
				if (!dictionary.ContainsKey(randomLostFootageIndex))
				{
					dictionary[randomLostFootageIndex] = 1;
				}
				else
				{
					dictionary[randomLostFootageIndex]++;
				}
			}
			foreach (KeyValuePair<int, int> item in dictionary)
			{
				GetFootageByIndex(item.Key, out var footage);
				Debug.Log($"{footage.name} Chance: {(float)item.Value / (float)num}");
			}
		}

		public bool GetFootageByIndex(int i, out FootageRarityPair footage)
		{
			footage = null;
			if (i < 0 || i >= footageRarityPairs.Count)
			{
				return false;
			}
			footage = footageRarityPairs[i];
			return true;
		}

		public int GetRandomLostFootageIndex()
		{
			int num = 0;
			foreach (FootageRarityPair footageRarityPair2 in footageRarityPairs)
			{
				num += footageRarityPair2.rarity;
			}
			int num2 = UnityEngine.Random.Range(0, num);
			for (int i = 0; i < footageRarityPairs.Count; i++)
			{
				FootageRarityPair footageRarityPair = footageRarityPairs[i];
				num2 -= footageRarityPair.rarity;
				if (num2 <= 0)
				{
					return i;
				}
			}
			Debug.LogError("Failed to get random footage index");
			return -1;
		}

		public static bool TryGetLostFootage(LostFootageHandle handle, out FootageRarityPair footage)
		{
			footage = null;
			if (handle.index >= 1 && SingletonAsset<LostFootageDatabase>.Instance.GetFootageByIndex(handle.index, out footage))
			{
				return true;
			}
			LostFootageHandle lostFootageHandle = handle;
			Debug.LogError("Failed to get footage by index: " + lostFootageHandle.ToString());
			return false;
		}
	}
}
