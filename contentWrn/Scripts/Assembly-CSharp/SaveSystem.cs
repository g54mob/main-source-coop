using System;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public static class SaveSystem
{
	public static string SAVE_LOCATION = Application.persistentDataPath + "/Saves/";

	public static readonly string BIN_EXTENSION = ".bin";

	public static readonly string CURRENT_VERSION = "2";

	private static readonly uint NUM_SAVES = 3u;

	public static Save[] SavesOnFile = new Save[NUM_SAVES];

	public static bool Inited = false;

	private static FileInfo MetaFileInfo => new FileInfo(Application.persistentDataPath + "/Saves/metaData.m");

	public static bool USING_SAVE { get; private set; }

	public static bool HaveCurrentSave => CurrentSave != null;

	public static Save CurrentSave { get; private set; }

	public static int LastSelectedIndex { get; private set; }

	public static void Init()
	{
		LoadSavesFromDisk();
		if (!Inited)
		{
			Inited = true;
			Debug.Log("SaveSystem Init, Save Location: " + SAVE_LOCATION);
		}
	}

	public static async void LoadSavesFromDisk()
	{
		SavesOnFile = new Save[NUM_SAVES];
		Debug.Log("Loading saves...");
		DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_LOCATION);
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
			return;
		}
		IEnumerable<FileInfo> saveFiles = SaveIO.GetSaveFiles();
		if (DeleteInvalidSaveFiles(saveFiles))
		{
			Modal.ShowError(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_BadSave_Title), LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_BadSave_Body));
			saveFiles = SaveIO.GetSaveFiles();
		}
		foreach (Save safe in GetSaves(saveFiles))
		{
			SavesOnFile[safe.SerializedSave.SaveIndex] = safe;
			Debug.Log("Loaded Save: " + safe.SerializedSave.SaveIndex);
		}
		static bool DeleteInvalidSaveFiles(IEnumerable<FileInfo> files)
		{
			bool result = false;
			foreach (FileInfo file in files)
			{
				string text = File.ReadAllText(file.FullName);
				if (!text.StartsWith("version:"))
				{
					result = true;
					Debug.Log("Deleting Invalid Save: " + file.FullName);
					file.Delete();
				}
				else
				{
					string text2 = text.Split("ersion:")[1].Split("\n")[0].Trim();
					if (text2 != CURRENT_VERSION)
					{
						result = true;
						Debug.Log("Deleting Invalid Save, wrong version: " + text2 + " instead of " + CURRENT_VERSION + ", " + file.FullName);
						file.Delete();
					}
				}
			}
			return result;
		}
		static int GetSaveIndex(FileInfo file)
		{
			for (int i = 0; i < SavesOnFile.Length; i++)
			{
				if (file.Name.Contains(i.ToString()))
				{
					return i;
				}
			}
			Debug.LogError("Failed to get save index from file: " + file.FullName);
			return -1;
		}
		static IEnumerable<Save> GetSaves(IEnumerable<FileInfo> files)
		{
			List<Save> list = new List<Save>();
			bool flag = false;
			foreach (FileInfo file2 in files)
			{
				int num = GetSaveIndex(file2);
				if (num >= 0)
				{
					Save save = null;
					try
					{
						save = SaveIO.LoadSave(file2, num);
					}
					catch (Exception ex)
					{
						Debug.LogError("Save Corrupted: " + ex.Message);
						flag = true;
						continue;
					}
					list.Add(save);
				}
			}
			if (flag)
			{
				Modal.ShowError(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_CorruptSave_Title), LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_CorruptSave_Body));
			}
			return list;
		}
	}

	public static void MakeNewSave()
	{
		CurrentSave = new Save(LastSelectedIndex);
		Debug.Log($"Making New Save for index{LastSelectedIndex} ");
	}

	public static void SaveToDisk()
	{
		if (!CanSave())
		{
			Debug.LogError("Not Ready For Save!");
			return;
		}
		SaveLoadHandler.FillSaveData(CurrentSave);
		if (CurrentSave.SerializedSave.SaveIndex == 255)
		{
			SaveIO.SaveTo(SAVE_LOCATION + "/DebugSave", CurrentSave);
		}
		else
		{
			SaveIO.SaveTo(SAVE_LOCATION + "/Save" + CurrentSave.SerializedSave.SaveIndex + BIN_EXTENSION, CurrentSave);
		}
	}

	private static bool CanSave()
	{
		if (PhotonNetwork.InRoom)
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				Debug.Log("Skipping Save From Non Master");
				return false;
			}
			if (CurrentSave == null)
			{
				Debug.LogError("Current Save is null");
				return false;
			}
			if (!USING_SAVE)
			{
				Debug.LogError("Save System Not Enabled");
				return false;
			}
			return true;
		}
		Debug.LogError("Cant Save Outside Of Room");
		return false;
	}

	public static void SelectCurrentSave(int saveIndex)
	{
		Save save = SavesOnFile[saveIndex];
		if (CanSave())
		{
			Debug.LogError("Trying to change current save during run? Not Allowed");
			return;
		}
		LastSelectedIndex = saveIndex;
		if (save != null)
		{
			VerboseDebug.Log("Changed Current Save to: " + save.ToString());
		}
		else
		{
			VerboseDebug.Log("Changed Current Save to Empty");
		}
		VerboseDebug.Log("Changed Current Save Index: " + LastSelectedIndex);
		CurrentSave = save;
	}

	public static void DeleteCurrentSave()
	{
		if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (CurrentSave != null)
		{
			FileInfo fileInfo = new FileInfo(SAVE_LOCATION + "/Save" + CurrentSave.SerializedSave.SaveIndex + BIN_EXTENSION);
			if (fileInfo.Exists)
			{
				File.Delete(fileInfo.FullName);
				Debug.Log("Deleting Current Save!");
			}
			else
			{
				Debug.LogError("Error Deleting Current Save");
			}
		}
		else
		{
			Debug.LogError("Save Is null!");
		}
		MakeNewSave();
	}

	public static void DeleteSave(int saveIndex)
	{
		if (!PhotonNetwork.InRoom || PhotonNetwork.IsMasterClient)
		{
			FileInfo fileInfo = new FileInfo(SAVE_LOCATION + "/Save" + saveIndex + BIN_EXTENSION);
			if (fileInfo.Exists)
			{
				File.Delete(fileInfo.FullName);
				Debug.Log("Deleting Current Save!");
			}
			else
			{
				Debug.LogError("Error Deleting Current Save");
			}
			SavesOnFile[saveIndex] = null;
		}
	}

	public static void UsingSave(bool b)
	{
		USING_SAVE = b;
	}

	public static void SaveMetaData(string serializedData)
	{
		if (MetaFileInfo.Exists)
		{
			File.Delete(MetaFileInfo.FullName);
			Debug.Log("Deleting MetaFile!");
		}
		File.WriteAllText(MetaFileInfo.FullName, serializedData);
	}

	public static void TryLoadMetaData(Action<bool, string> onDone)
	{
		if (MetaFileInfo.Exists)
		{
			string text = File.ReadAllText(MetaFileInfo.FullName);
			onDone?.Invoke(!string.IsNullOrEmpty(text), text);
		}
		else
		{
			Debug.Log("Found No MetaFile!");
			onDone?.Invoke(arg1: false, null);
		}
	}

	public static void DeleteMetaData()
	{
		if (MetaFileInfo.Exists)
		{
			File.Delete(MetaFileInfo.FullName);
			Debug.Log("Deleting MetaFile!");
		}
	}
}
