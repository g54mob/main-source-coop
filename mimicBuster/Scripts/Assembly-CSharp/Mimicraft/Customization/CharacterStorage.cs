using System;
using System.Collections.Generic;
using System.IO;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Customization
{
	public static class CharacterStorage
	{
		private const string FolderName = "Characters";

		private static string Directory_ => Path.Combine(Application.persistentDataPath, "Characters");

		public static List<CharacterListEntry> List()
		{
			List<CharacterListEntry> list = new List<CharacterListEntry>();
			if (!Directory.Exists(Directory_))
			{
				return list;
			}
			string[] files = Directory.GetFiles(Directory_, "*.character");
			foreach (string filePath in files)
			{
				if (TryReadFileName(filePath, out var characterName))
				{
					list.Add(new CharacterListEntry(filePath, characterName));
				}
			}
			files = Directory.GetFiles(Directory_, "*.json");
			foreach (string filePath2 in files)
			{
				CharacterJson characterJson = TryRead(filePath2);
				if (characterJson != null)
				{
					list.Add(new CharacterListEntry(filePath2, characterJson.characterName));
				}
			}
			list.Sort((CharacterListEntry a, CharacterListEntry b) => string.Compare(a.CharacterName, b.CharacterName, StringComparison.OrdinalIgnoreCase));
			return list;
		}

		private static bool TryReadFileName(string filePath, out string characterName)
		{
			characterName = "";
			try
			{
				return CharacterFile.TryReadName(File.ReadAllBytes(filePath), out characterName);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CharacterStorage] '" + filePath + "' okunamadi: " + ex.Message);
				return false;
			}
		}

		private static CharacterJson TryRead(string filePath)
		{
			try
			{
				return JsonUtility.FromJson<CharacterJson>(File.ReadAllText(filePath));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CharacterStorage] '" + filePath + "' okunamadi: " + ex.Message);
				return null;
			}
		}

		public static CharacterData Load(string filePath, CharacterRigDefinition rig)
		{
			if (filePath != null && filePath.EndsWith(".character", StringComparison.OrdinalIgnoreCase))
			{
				return LoadBinary(filePath, rig);
			}
			CharacterJson characterJson = TryRead(filePath);
			if (characterJson == null)
			{
				return null;
			}
			CharacterData characterData = new CharacterData(characterJson.rigId);
			foreach (CharacterPartJson part in characterJson.parts)
			{
				if (part != null && !string.IsNullOrWhiteSpace(part.partId))
				{
					if (rig != null && rig.Find(part.partId) == null)
					{
						Debug.LogWarning("[CharacterStorage] '" + part.partId + "' bu rig'de yok - atlandi.");
					}
					else
					{
						characterData.Set(part.partId, ToGrid(part));
					}
				}
			}
			return characterData;
		}

		private static CharacterData LoadBinary(string filePath, CharacterRigDefinition rig)
		{
			byte[] bytes;
			try
			{
				bytes = File.ReadAllBytes(filePath);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CharacterStorage] '" + filePath + "' okunamadi: " + ex.Message);
				return null;
			}
			if (!CharacterFile.TryDecode(bytes, out var _, out var data, out var savedBoxes))
			{
				Debug.LogWarning("[CharacterStorage] '" + filePath + "' bir karakter dosyasi degil ya da bozuk.");
				return null;
			}
			if (rig == null)
			{
				return data;
			}
			CharacterBoxMigration.Apply(data, savedBoxes, rig);
			CharacterData characterData = new CharacterData(data.RigId)
			{
				VoiceType = data.VoiceType
			};
			foreach (CharacterPartData part in data.Parts)
			{
				if (part != null && !string.IsNullOrWhiteSpace(part.PartId))
				{
					if (rig.Find(part.PartId) == null)
					{
						Debug.LogWarning("[CharacterStorage] '" + part.PartId + "' bu rig'de yok - atlandi.");
					}
					else
					{
						characterData.Set(part.PartId, part.Grid);
					}
				}
			}
			return characterData;
		}

		public static string Save(string filePath, string characterName, CharacterData data, CharacterRigDefinition rig = null)
		{
			if (!Directory.Exists(Directory_))
			{
				Directory.CreateDirectory(Directory_);
			}
			if (string.IsNullOrEmpty(filePath) || !filePath.EndsWith(".character", StringComparison.OrdinalIgnoreCase))
			{
				filePath = Path.Combine(Directory_, string.Format("{0:N}{1}", Guid.NewGuid(), ".character"));
			}
			File.WriteAllBytes(filePath, CharacterFile.Encode(characterName, data, rig));
			return filePath;
		}

		public static void Delete(string filePath)
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}

		private static CharacterJson ToJson(string characterName, CharacterData data)
		{
			CharacterJson characterJson = new CharacterJson
			{
				characterName = characterName,
				rigId = (data?.RigId ?? "")
			};
			if (data == null)
			{
				return characterJson;
			}
			foreach (CharacterPartData part in data.Parts)
			{
				if (part?.Grid == null)
				{
					continue;
				}
				CharacterPartJson characterPartJson = new CharacterPartJson
				{
					partId = part.PartId
				};
				foreach (KeyValuePair<Vector3Int, VoxelData> voxel in part.Grid.Voxels)
				{
					characterPartJson.voxels.Add(new VoxelEntryData
					{
						x = voxel.Key.x,
						y = voxel.Key.y,
						z = voxel.Key.z,
						color = ToHex(voxel.Value.Color)
					});
				}
				if (part.Grid.FaceColorCount > 0)
				{
					characterPartJson.faces = new List<FaceEntryData>(part.Grid.FaceColorCount);
					foreach (KeyValuePair<(Vector3Int, int), Color32> faceColor in part.Grid.FaceColors)
					{
						characterPartJson.faces.Add(new FaceEntryData
						{
							x = faceColor.Key.Item1.x,
							y = faceColor.Key.Item1.y,
							z = faceColor.Key.Item1.z,
							face = faceColor.Key.Item2,
							color = ToHex(faceColor.Value)
						});
					}
				}
				characterJson.parts.Add(characterPartJson);
			}
			return characterJson;
		}

		private static VoxelGrid ToGrid(CharacterPartJson part)
		{
			VoxelGrid voxelGrid = new VoxelGrid();
			foreach (VoxelEntryData voxel in part.voxels)
			{
				voxelGrid.Set(new Vector3Int(voxel.x, voxel.y, voxel.z), new VoxelData(FromHex(voxel.color)));
			}
			if (part.faces == null)
			{
				return voxelGrid;
			}
			foreach (FaceEntryData face in part.faces)
			{
				if (face.face >= 0 && face.face < 6)
				{
					voxelGrid.SetFaceColor(new Vector3Int(face.x, face.y, face.z), face.face, FromHex(face.color));
				}
			}
			return voxelGrid;
		}

		private static string ToHex(Color32 c)
		{
			return $"#{c.r:X2}{c.g:X2}{c.b:X2}";
		}

		private static Color32 FromHex(string hex)
		{
			if (string.IsNullOrEmpty(hex))
			{
				return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
			hex = hex.TrimStart('#');
			if (hex.Length < 6)
			{
				return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
			return new Color32(Convert.ToByte(hex.Substring(0, 2), 16), Convert.ToByte(hex.Substring(2, 2), 16), Convert.ToByte(hex.Substring(4, 2), 16), byte.MaxValue);
		}
	}
}
