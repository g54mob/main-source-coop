using System.IO;
using UnityEngine;

namespace EvilCore.EvilSave
{
	[CreateAssetMenu(fileName = "EvilSaveSettings", menuName = "EvilSave/Settings")]
	public class EvilSaveSettings : ScriptableObject
	{
		[Header("General")]
		public int SaveVersion = 1;

		public string DefaultSlotId = "slot_0";

		public int MaxSaveSlots = 5;

		[Header("Storage")]
		public SaveLocation SaveLocation;

		public string CustomSavePath = "";

		[Header("Auto-Save")]
		public bool EnableAutoSave = true;

		public float AutoSaveIntervalSeconds = 600f;

		[Header("Backup")]
		public bool EnableBackups = true;

		public int MaxBackupsPerSlot = 5;

		[Header("Security")]
		public bool EnableEncryption;

		public string EncryptionPassword = "";

		public bool EnableCompression = true;

		[Header("Format")]
		public SaveFormat Format;

		[Header("PlayerPrefs")]
		public string PlayerPrefsPrefix = "EvilSave_";

		[Header("Debug")]
		public bool LogOperations = true;

		private static EvilSaveSettings _instance;

		public static EvilSaveSettings Instance
		{
			get
			{
				if (_instance != null)
				{
					return _instance;
				}
				_instance = Resources.Load<EvilSaveSettings>("EvilSaveSettings");
				if (_instance == null)
				{
					_instance = ScriptableObject.CreateInstance<EvilSaveSettings>();
				}
				return _instance;
			}
		}

		public string GetSaveRootPath()
		{
			return Path.Combine(SaveLocation switch
			{
				SaveLocation.PersistentDataPath => Application.persistentDataPath, 
				SaveLocation.DataPath => Application.dataPath, 
				SaveLocation.Custom => string.IsNullOrEmpty(CustomSavePath) ? Application.persistentDataPath : CustomSavePath, 
				_ => Application.persistentDataPath, 
			}, "EvilSave");
		}
	}
}
