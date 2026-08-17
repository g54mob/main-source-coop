using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;

namespace EvilCore.EvilSave
{
	public static class EvilSave
	{
		public static class Prefs
		{
			public static void SetString(string key, string value)
			{
				PlayerPrefsBackend.SetString(key, value);
			}

			public static string GetString(string key, string defaultValue = "")
			{
				return PlayerPrefsBackend.GetString(key, defaultValue);
			}

			public static void SetInt(string key, int value)
			{
				PlayerPrefsBackend.SetInt(key, value);
			}

			public static int GetInt(string key, int defaultValue = 0)
			{
				return PlayerPrefsBackend.GetInt(key, defaultValue);
			}

			public static void SetFloat(string key, float value)
			{
				PlayerPrefsBackend.SetFloat(key, value);
			}

			public static float GetFloat(string key, float defaultValue = 0f)
			{
				return PlayerPrefsBackend.GetFloat(key, defaultValue);
			}

			public static void SetBool(string key, bool value)
			{
				PlayerPrefsBackend.SetBool(key, value);
			}

			public static bool GetBool(string key, bool defaultValue = false)
			{
				return PlayerPrefsBackend.GetBool(key, defaultValue);
			}

			public static void SetObject<T>(string key, T value)
			{
				PlayerPrefsBackend.SetObject(key, value);
			}

			public static T GetObject<T>(string key, T defaultValue = default(T))
			{
				return PlayerPrefsBackend.GetObject(key, defaultValue);
			}

			public static bool HasKey(string key)
			{
				return PlayerPrefsBackend.HasKey(key);
			}

			public static void DeleteKey(string key)
			{
				PlayerPrefsBackend.DeleteKey(key);
			}

			public static void DeleteAll()
			{
				PlayerPrefsBackend.DeleteAll();
			}

			public static string[] GetAllKeys()
			{
				return PlayerPrefsBackend.GetAllKeys();
			}
		}

		private static readonly Dictionary<string, SaveData> SlotCache = new Dictionary<string, SaveData>();

		private static IStorageBackend _storage;

		private static readonly List<IStreamProcessor> Processors = new List<IStreamProcessor>();

		private static string _activeSlot;

		public static string ActiveSlot
		{
			get
			{
				return _activeSlot ?? EvilSaveSettings.Instance.DefaultSlotId;
			}
			set
			{
				_activeSlot = value;
			}
		}

		internal static IStorageBackend Storage
		{
			get
			{
				if (_storage == null)
				{
					_storage = new FileStorageBackend();
				}
				return _storage;
			}
			set
			{
				_storage = value;
			}
		}

		internal static void RebuildProcessors()
		{
			Processors.Clear();
			EvilSaveSettings instance = EvilSaveSettings.Instance;
			if (instance.EnableCompression)
			{
				Processors.Add(new GZipCompressionProcessor());
			}
			if (instance.EnableEncryption && !string.IsNullOrEmpty(instance.EncryptionPassword))
			{
				Processors.Add(new AesEncryptionProcessor(instance.EncryptionPassword));
			}
		}

		private static SaveData GetOrCreateSlotData(string slotId)
		{
			if (slotId == null)
			{
				slotId = ActiveSlot;
			}
			if (SlotCache.TryGetValue(slotId, out var value))
			{
				return value;
			}
			value = new SaveData
			{
				Version = EvilSaveSettings.Instance.SaveVersion
			};
			SlotCache[slotId] = value;
			return value;
		}

		public static void Save<T>(string key, T value)
		{
			GetOrCreateSlotData(ActiveSlot).Set(key, value);
		}

		public static T Load<T>(string key, T defaultValue = default(T))
		{
			return GetOrCreateSlotData(ActiveSlot).Get(key, defaultValue);
		}

		public static bool HasKey(string key)
		{
			return GetOrCreateSlotData(ActiveSlot).HasKey(key);
		}

		public static void DeleteKey(string key)
		{
			GetOrCreateSlotData(ActiveSlot).Remove(key);
		}

		public static string[] GetKeys()
		{
			return GetOrCreateSlotData(ActiveSlot).GetKeys();
		}

		public static string[] GetSlotIds()
		{
			string saveRootPath = EvilSaveSettings.Instance.GetSaveRootPath();
			return Enumerable.Select(Storage.ListDirectories(saveRootPath), Path.GetFileName).ToArray();
		}

		public static SaveSlotMetadata GetSlotMetadata(string slotId)
		{
			string metaPath = GetMetaPath(slotId);
			byte[] array = Storage.Read(metaPath);
			if (array == null)
			{
				return null;
			}
			return SaveSlotMetadata.FromJson(Encoding.UTF8.GetString(array));
		}

		public static void CreateSlot(string slotId, string displayName = null)
		{
			EvilSaveSettings instance = EvilSaveSettings.Instance;
			SaveSlotMetadata saveSlotMetadata = SaveSlotMetadata.Create(slotId, displayName, instance.SaveVersion);
			string metaPath = GetMetaPath(slotId);
			Storage.Write(metaPath, Encoding.UTF8.GetBytes(saveSlotMetadata.ToJson()));
			SlotCache[slotId] = new SaveData
			{
				Version = instance.SaveVersion
			};
			Log("Slot created: " + slotId);
		}

		public static void DeleteSlot(string slotId)
		{
			string slotDirectory = GetSlotDirectory(slotId);
			if (Directory.Exists(slotDirectory))
			{
				Directory.Delete(slotDirectory, recursive: true);
			}
			SlotCache.Remove(slotId);
			Log("Slot deleted: " + slotId);
		}

		public static void CopySlot(string source, string target)
		{
			string slotDirectory = GetSlotDirectory(source);
			string slotDirectory2 = GetSlotDirectory(target);
			if (Directory.Exists(slotDirectory))
			{
				if (!Directory.Exists(slotDirectory2))
				{
					Directory.CreateDirectory(slotDirectory2);
				}
				string[] files = Directory.GetFiles(slotDirectory);
				foreach (string text in files)
				{
					string destFileName = Path.Combine(slotDirectory2, Path.GetFileName(text));
					File.Copy(text, destFileName, overwrite: true);
				}
				SaveSlotMetadata slotMetadata = GetSlotMetadata(target);
				if (slotMetadata != null)
				{
					slotMetadata.slotId = target;
					Storage.Write(GetMetaPath(target), Encoding.UTF8.GetBytes(slotMetadata.ToJson()));
				}
				Log("Slot copied: " + source + " -> " + target);
			}
		}

		public static bool SlotExists(string slotId)
		{
			if (!Storage.Exists(GetSavePath(slotId)))
			{
				return Storage.Exists(GetMetaPath(slotId));
			}
			return true;
		}

		public static void SaveToDisk(string slotId = null)
		{
			if (slotId == null)
			{
				slotId = ActiveSlot;
			}
			SaveData orCreateSlotData = GetOrCreateSlotData(slotId);
			byte[] array = SerializeSaveData(orCreateSlotData);
			string savePath = GetSavePath(slotId);
			Storage.Write(savePath, array);
			UpdateMetadata(slotId, orCreateSlotData);
			Log($"Saved to disk: {savePath} ({array.Length} bytes)");
		}

		public static void LoadFromDisk(string slotId = null)
		{
			if (slotId == null)
			{
				slotId = ActiveSlot;
			}
			string savePath = GetSavePath(slotId);
			byte[] array = Storage.Read(savePath);
			SaveData data;
			if (array == null)
			{
				Log("No save file found at: " + savePath);
			}
			else if (TryDeserializeSaveData(array, out data))
			{
				SlotCache[slotId] = data;
				Log($"Loaded from disk: {savePath} ({array.Length} bytes, {data.Entries.Count} entries)");
			}
			else
			{
				SlotCache[slotId] = RecoverCorruptedSlot(slotId, savePath);
			}
		}

		public static async UniTask SaveToDiskAsync(string slotId = null)
		{
			if (slotId == null)
			{
				slotId = ActiveSlot;
			}
			SaveData data = GetOrCreateSlotData(slotId);
			byte[] bytes = SerializeSaveData(data);
			string savePath = GetSavePath(slotId);
			await Storage.WriteAsync(savePath, bytes);
			UpdateMetadata(slotId, data);
			Log($"Saved to disk (async): {savePath} ({bytes.Length} bytes)");
		}

		public static async UniTask LoadFromDiskAsync(string slotId = null)
		{
			if (slotId == null)
			{
				slotId = ActiveSlot;
			}
			string savePath = GetSavePath(slotId);
			byte[] array = await Storage.ReadAsync(savePath);
			SaveData data;
			if (array == null)
			{
				Log("No save file found at: " + savePath);
			}
			else if (TryDeserializeSaveData(array, out data))
			{
				SlotCache[slotId] = data;
				Log($"Loaded from disk (async): {savePath} ({array.Length} bytes, {data.Entries.Count} entries)");
			}
			else
			{
				SlotCache[slotId] = RecoverCorruptedSlot(slotId, savePath);
			}
		}

		public static void CreateBackup(string slotId = null)
		{
			if (slotId == null)
			{
				slotId = ActiveSlot;
			}
			string savePath = GetSavePath(slotId);
			if (Storage.Exists(savePath))
			{
				EvilSaveSettings instance = EvilSaveSettings.Instance;
				string text = Path.Combine(GetSlotDirectory(slotId), "backup");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				string text2 = DateTime.Now.ToString("yyyyMMdd_HHmmss");
				string text3 = Path.Combine(text, "save_" + text2 + ".dat");
				byte[] data = Storage.Read(savePath);
				Storage.Write(text3, data);
				CleanOldBackups(text, instance.MaxBackupsPerSlot);
				Log("Backup created: " + text3);
			}
		}

		public static bool RestoreBackup(string slotId = null, int backupIndex = 0)
		{
			string[] backups = GetBackups(slotId);
			if (backupIndex < 0 || backupIndex >= backups.Length)
			{
				return false;
			}
			if (slotId == null)
			{
				slotId = ActiveSlot;
			}
			string savePath = GetSavePath(slotId);
			byte[] array = Storage.Read(backups[backupIndex]);
			if (array == null)
			{
				return false;
			}
			Storage.Write(savePath, array);
			SlotCache.Remove(slotId);
			LoadFromDisk(slotId);
			Log("Backup restored: " + backups[backupIndex]);
			return true;
		}

		public static string[] GetBackups(string slotId = null)
		{
			if (slotId == null)
			{
				slotId = ActiveSlot;
			}
			string path = Path.Combine(GetSlotDirectory(slotId), "backup");
			if (!Directory.Exists(path))
			{
				return Array.Empty<string>();
			}
			return (from f in Directory.GetFiles(path, "save_*.dat")
				orderby f descending
				select f).ToArray();
		}

		public static void SaveRaw(string key, byte[] data)
		{
			GetOrCreateSlotData(ActiveSlot).Entries[key] = data;
		}

		public static byte[] LoadRaw(string key)
		{
			if (!GetOrCreateSlotData(ActiveSlot).Entries.TryGetValue(key, out var value))
			{
				return null;
			}
			return value;
		}

		public static byte[] Serialize<T>(T value)
		{
			using MemoryStream memoryStream = new MemoryStream();
			using EvilWriter evilWriter = new EvilWriter(memoryStream);
			evilWriter.Write(value);
			return memoryStream.ToArray();
		}

		public static T Deserialize<T>(byte[] data)
		{
			using MemoryStream stream = new MemoryStream(data);
			using EvilReader evilReader = new EvilReader(stream);
			return evilReader.Read<T>();
		}

		public static void Clear(string slotId = null)
		{
			if (slotId == null)
			{
				slotId = ActiveSlot;
			}
			if (SlotCache.TryGetValue(slotId, out var value))
			{
				value.Entries.Clear();
			}
		}

		public static string GetSavePath(string slotId = null)
		{
			if (slotId == null)
			{
				slotId = ActiveSlot;
			}
			return Path.Combine(GetSlotDirectory(slotId), "save.dat");
		}

		public static string GetSlotDirectory(string slotId)
		{
			return Path.Combine(EvilSaveSettings.Instance.GetSaveRootPath(), slotId);
		}

		public static string GetMetaPath(string slotId)
		{
			return Path.Combine(GetSlotDirectory(slotId), "meta.json");
		}

		public static int GetCachedSlotCount()
		{
			return SlotCache.Count;
		}

		public static SaveData GetCachedSlotData(string slotId)
		{
			if (!SlotCache.TryGetValue(slotId, out var value))
			{
				return null;
			}
			return value;
		}

		private static byte[] SerializeSaveData(SaveData data)
		{
			byte[] array = ((EvilSaveSettings.Instance.Format != SaveFormat.Json) ? data.Serialize() : EvilJsonSerializer.SerializeToBytes(data));
			RebuildProcessors();
			foreach (IStreamProcessor processor in Processors)
			{
				array = processor.Process(array);
			}
			return array;
		}

		private static SaveData DeserializeSaveData(byte[] bytes)
		{
			RebuildProcessors();
			for (int num = Processors.Count - 1; num >= 0; num--)
			{
				bytes = Processors[num].Unprocess(bytes);
			}
			if (EvilSaveSettings.Instance.Format == SaveFormat.Json)
			{
				return EvilJsonSerializer.DeserializeFromBytes(bytes);
			}
			return SaveData.Deserialize(bytes);
		}

		private static bool TryDeserializeSaveData(byte[] bytes, out SaveData data)
		{
			try
			{
				data = DeserializeSaveData(bytes);
				return data != null;
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[EvilSave] Failed to deserialize save data: " + ex.Message, "TryDeserializeSaveData", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\EvilSave\\Scripts\\Core\\EvilSave.cs", 368);
				data = null;
				return false;
			}
		}

		private static SaveData RecoverCorruptedSlot(string slotId, string savePath)
		{
			EvilLogger.LogError("[EvilSave] Save file corrupted for slot '" + slotId + "' at " + savePath + ". Attempting backup recovery.", "RecoverCorruptedSlot", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\EvilSave\\Scripts\\Core\\EvilSave.cs", 379);
			string[] backups = GetBackups(slotId);
			foreach (string path in backups)
			{
				byte[] array = Storage.Read(path);
				if (array != null && TryDeserializeSaveData(array, out var data))
				{
					try
					{
						Storage.Write(savePath, array);
					}
					catch (Exception ex)
					{
						EvilLogger.LogError("[EvilSave] Failed to restore backup over corrupt save: " + ex.Message, "RecoverCorruptedSlot", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\EvilSave\\Scripts\\Core\\EvilSave.cs", 393);
					}
					return data;
				}
			}
			EvilLogger.LogError("[EvilSave] No valid backup for slot '" + slotId + "'. Starting a fresh slot to avoid a crash on load.", "RecoverCorruptedSlot", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\EvilSave\\Scripts\\Core\\EvilSave.cs", 400);
			return new SaveData
			{
				Version = EvilSaveSettings.Instance.SaveVersion
			};
		}

		private static void UpdateMetadata(string slotId, SaveData data)
		{
			string metaPath = GetMetaPath(slotId);
			byte[] array = Storage.Read(metaPath);
			SaveSlotMetadata saveSlotMetadata;
			if (array != null)
			{
				saveSlotMetadata = SaveSlotMetadata.FromJson(Encoding.UTF8.GetString(array));
				saveSlotMetadata.lastSavedAt = DateTime.UtcNow.ToString("o");
				saveSlotMetadata.saveVersion = data.Version;
			}
			else
			{
				saveSlotMetadata = SaveSlotMetadata.Create(slotId, slotId, data.Version);
			}
			Storage.Write(metaPath, Encoding.UTF8.GetBytes(saveSlotMetadata.ToJson()));
		}

		private static void CleanOldBackups(string backupDir, int maxBackups)
		{
			string[] array = (from f in Directory.GetFiles(backupDir, "save_*.dat")
				orderby f descending
				select f).ToArray();
			for (int num = maxBackups; num < array.Length; num++)
			{
				File.Delete(array[num]);
			}
		}

		private static void Log(string message)
		{
			_ = EvilSaveSettings.Instance.LogOperations;
		}
	}
}
