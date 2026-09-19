using System;
using UnityEngine;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts.Implementation
{
	[Serializable]
	public class SaveFolderOverride
	{
		[SerializeField]
		private string _groupName;

		[SerializeField]
		private string _saveFolderName;

		public string GroupName => _groupName;

		public string SaveFolderName => _saveFolderName;
	}
}
