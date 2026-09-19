using System.Collections.Generic;
using UnityEngine;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts.Implementation
{
	[CreateAssetMenu(fileName = "SavingSystemConfiguration_Default", menuName = "Configurations/SavingModule/SavingSystemConfiguration")]
	public class SavingSystemConfiguration : ScriptableObject
	{
		[SerializeField]
		private string _filesEncryptionKey;

		[SerializeField]
		private string _saveFolderName;

		[SerializeField]
		private List<PathHolder> _pathHolders = new List<PathHolder>();

		[SerializeField]
		private List<SaveFolderOverride> _saveFolderOverrides = new List<SaveFolderOverride>();

		public string FilesEncryptionKey => _filesEncryptionKey;

		public string SaveFolderName => _saveFolderName;

		public string GetSaveFolderForGroup(string groupName)
		{
			foreach (SaveFolderOverride saveFolderOverride in _saveFolderOverrides)
			{
				if (saveFolderOverride.GroupName == groupName)
				{
					return saveFolderOverride.SaveFolderName;
				}
			}
			return _saveFolderName;
		}

		public IReadOnlyCollection<string> GetAllSaveFolders()
		{
			HashSet<string> hashSet = new HashSet<string> { _saveFolderName };
			foreach (SaveFolderOverride saveFolderOverride in _saveFolderOverrides)
			{
				hashSet.Add(saveFolderOverride.SaveFolderName);
			}
			return hashSet;
		}

		public string GetRootFolderPathForPlatform(RuntimePlatform platform)
		{
			foreach (PathHolder pathHolder in _pathHolders)
			{
				if (pathHolder.Platform == platform)
				{
					return pathHolder.GetPath();
				}
			}
			return Application.persistentDataPath;
		}

		public string GetRootFolderPathForCurrentPlatform()
		{
			return GetRootFolderPathForPlatform(Application.platform);
		}
	}
}
