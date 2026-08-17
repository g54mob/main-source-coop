using System.IO;
using UnityEngine;

namespace EvilCore.EvilSave
{
	[RequireComponent(typeof(SaveableId))]
	public class SaveableEntity : MonoBehaviour
	{
		private SaveableId _saveableId;

		public string UniqueId
		{
			get
			{
				if (_saveableId == null)
				{
					_saveableId = GetComponent<SaveableId>();
				}
				return _saveableId.Id;
			}
		}

		private void OnEnable()
		{
			EvilSaveManager evilSaveManager = Object.FindAnyObjectByType<EvilSaveManager>();
			if (evilSaveManager != null)
			{
				evilSaveManager.RegisterSaveable(this);
			}
		}

		private void OnDisable()
		{
			EvilSaveManager evilSaveManager = Object.FindAnyObjectByType<EvilSaveManager>();
			if (evilSaveManager != null)
			{
				evilSaveManager.UnregisterSaveable(this);
			}
		}

		public void CaptureState()
		{
			ISaveable[] components = GetComponents<ISaveable>();
			foreach (ISaveable saveable in components)
			{
				string key = UniqueId + "/" + saveable.SaveId;
				using MemoryStream memoryStream = new MemoryStream();
				using EvilWriter writer = new EvilWriter(memoryStream);
				saveable.OnSave(writer);
				EvilSave.SaveRaw(key, memoryStream.ToArray());
			}
		}

		public void RestoreState()
		{
			ISaveable[] components = GetComponents<ISaveable>();
			foreach (ISaveable saveable in components)
			{
				byte[] array = EvilSave.LoadRaw(UniqueId + "/" + saveable.SaveId);
				if (array == null)
				{
					continue;
				}
				using MemoryStream stream = new MemoryStream(array);
				using EvilReader reader = new EvilReader(stream);
				saveable.OnLoad(reader);
			}
		}
	}
}
