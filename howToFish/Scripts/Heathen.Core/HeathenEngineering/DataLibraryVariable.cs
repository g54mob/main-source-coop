using System.Collections.Generic;
using System.IO;
using HeathenEngineering.Serializable;
using UnityEngine;

namespace HeathenEngineering
{
	[CreateAssetMenu(menuName = "System Core/Application/Data Library")]
	public class DataLibraryVariable : ScriptableObject
	{
		public List<KeyedVariable> Library = new List<KeyedVariable>();

		public KeyedDataLibrary CreateNewKeyedDataLibrary(bool applyDefaults)
		{
			KeyedDataLibrary keyedDataLibrary = new KeyedDataLibrary();
			foreach (KeyedVariable item in Library)
			{
				keyedDataLibrary[item.Key] = item.Default.ObjectValue;
				if (applyDefaults)
				{
					item.Variable.ObjectValue = item.Default.ObjectValue;
				}
			}
			return keyedDataLibrary;
		}

		public void ApplyDefaults()
		{
			foreach (KeyedVariable item in Library)
			{
				item.Variable.ObjectValue = item.Default.ObjectValue;
			}
		}

		public void SyncToFile(string path, bool createDirectory)
		{
			if (createDirectory)
			{
				CreateDirectoryIfRequired(path);
			}
			SyncToBuffer(out var buffer);
			FileStream fileStream = new FileStream(path, FileMode.Create);
			fileStream.Write(buffer, 0, buffer.Length);
			fileStream.Close();
		}

		public void SyncToFilePath(string path)
		{
			SyncToFile(path, createDirectory: false);
		}

		public void SyncToFilePathWithCreate(string path)
		{
			SyncToFile(path, createDirectory: true);
		}

		public void SyncToReferenceFile(StringVariable path, bool createDirectory)
		{
			SyncToFile(path.Value, createDirectory);
		}

		public void SyncToReferencePath(StringVariable path)
		{
			SyncToFile(path.Value, createDirectory: false);
		}

		public void SyncToReferencePathWithCreate(StringVariable path)
		{
			SyncToFile(path.Value, createDirectory: true);
		}

		public void SyncToKeyedLibrary(KeyedDataLibrary keyedLibrary)
		{
			if (keyedLibrary == null)
			{
				return;
			}
			foreach (KeyedVariable item in Library)
			{
				keyedLibrary[item.Key] = item.Variable.ObjectValue;
			}
		}

		public void SyncToBuffer(out byte[] buffer)
		{
			KeyedDataLibrary keyedDataLibrary = new KeyedDataLibrary();
			SyncToKeyedLibrary(keyedDataLibrary);
			buffer = KeyedDataLibrary.Serialize(keyedDataLibrary);
		}

		public void SyncFromFile(string path)
		{
			SyncFromBuffer(File.ReadAllBytes(path));
		}

		public void SyncFromReferenceFile(StringVariable path)
		{
			SyncFromFile(path.Value);
		}

		public void SyncFromKeyedLibrary(KeyedDataLibrary keyedLibrary)
		{
			if (keyedLibrary == null)
			{
				return;
			}
			foreach (KeyedVariable item in Library)
			{
				if (keyedLibrary.Contains(item.Key))
				{
					item.Variable.ObjectValue = keyedLibrary.GetValue(item.Key);
				}
			}
		}

		public void CreateDirectoryIfRequired(string filePath)
		{
			string text = filePath.Replace("\\", "/");
			if (!File.Exists(text) && !Directory.Exists(text.Substring(0, text.LastIndexOf('/'))))
			{
				Directory.CreateDirectory(filePath.Substring(0, text.LastIndexOf('/')));
			}
		}

		public void CreateReferencedDirectoryIfRequired(StringVariable filePath)
		{
			CreateDirectoryIfRequired(filePath.Value);
		}

		public void SyncFromBuffer(byte[] buffer)
		{
			KeyedDataLibrary keyedLibrary = KeyedDataLibrary.Deserialize(buffer);
			SyncFromKeyedLibrary(keyedLibrary);
		}
	}
}
