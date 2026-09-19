using System;
using UnityEngine;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts.Implementation
{
	[Serializable]
	public class PathHolder
	{
		[SerializeField]
		private RuntimePlatform _platform;

		[SerializeField]
		private SavePathType _savePathType;

		[SerializeField]
		private string _customPath;

		public RuntimePlatform Platform => _platform;

		public string GetPath()
		{
			return _savePathType switch
			{
				SavePathType.ApplicationDataPath => Application.dataPath, 
				SavePathType.ApplicationPersistentDataPath => Application.persistentDataPath, 
				SavePathType.CustomPath => _customPath, 
				_ => throw new ArgumentOutOfRangeException(), 
			};
		}
	}
}
