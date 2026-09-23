using System.Collections.Generic;
using Mimicraft.Customization;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Networking
{
	public static class CharacterValidator
	{
		public readonly struct Notice
		{
			public readonly string Key;

			public readonly int First;

			public readonly int Second;

			public bool HasValue => !string.IsNullOrEmpty(Key);

			public Notice(string key, int first, int second)
			{
				Key = key;
				First = first;
				Second = second;
			}
		}

		public const int MaxPayloadBytes = 262144;

		public static bool Accepts(byte[] data, CharacterRigDefinition rig, out CharacterData character, out string rejection, out Notice notice)
		{
			character = null;
			rejection = null;
			notice = default(Notice);
			if (data == null || data.Length == 0)
			{
				return true;
			}
			if (data.Length > 262144)
			{
				rejection = "Reject.CharacterTooLarge";
				return false;
			}
			if (rig == null)
			{
				rejection = "Reject.NoCharacterRig";
				return false;
			}
			if (!CharacterCodec.TryDecode(data, rig.RigId, out var character2))
			{
				rejection = "Reject.CharacterUnreadable";
				return false;
			}
			CharacterData characterData = new CharacterData(rig.RigId);
			int num = 0;
			int trimmed = 0;
			int filled = 0;
			foreach (CharacterPartData part in character2.Parts)
			{
				if (part?.Grid != null)
				{
					CharacterPartDefinition characterPartDefinition = rig.Find(part.PartId);
					if (!(characterPartDefinition == null))
					{
						VoxelGrid voxelGrid = Conform(part.Grid, characterPartDefinition, ref trimmed, ref filled);
						num += voxelGrid.Count;
						characterData.Set(part.PartId, voxelGrid);
					}
				}
			}
			int effectiveMaxTotalVoxels = rig.EffectiveMaxTotalVoxels;
			if (effectiveMaxTotalVoxels > 0 && num > effectiveMaxTotalVoxels)
			{
				rejection = $"Karakter voxel sınırını aşıyor ({num} > {effectiveMaxTotalVoxels}).";
				return false;
			}
			if (trimmed > 0 || filled > 0)
			{
				notice = new Notice("Notice.CharacterFitted", trimmed, filled);
			}
			character = characterData;
			return true;
		}

		private static VoxelGrid Conform(VoxelGrid source, CharacterPartDefinition definition, ref int trimmed, ref int filled)
		{
			VoxelGrid voxelGrid = new VoxelGrid();
			Color32 color = new Color32(200, 200, 205, byte.MaxValue);
			bool flag = false;
			foreach (KeyValuePair<Vector3Int, VoxelData> voxel in source.Voxels)
			{
				if (!definition.IsInsideBox(voxel.Key))
				{
					trimmed++;
					continue;
				}
				voxelGrid.Set(voxel.Key, voxel.Value);
				if (!flag)
				{
					color = voxel.Value.Color;
					flag = true;
				}
			}
			foreach (KeyValuePair<(Vector3Int, int), Color32> faceColor in source.FaceColors)
			{
				if (voxelGrid.Contains(faceColor.Key.Item1))
				{
					voxelGrid.SetFaceColor(faceColor.Key.Item1, faceColor.Key.Item2, faceColor.Value);
				}
			}
			if (!definition.HasRequiredCore)
			{
				return voxelGrid;
			}
			Vector3Int requiredMin = definition.RequiredMin;
			Vector3Int vector3Int = requiredMin + definition.RequiredSize;
			for (int i = requiredMin.x; i < vector3Int.x; i++)
			{
				for (int j = requiredMin.y; j < vector3Int.y; j++)
				{
					for (int k = requiredMin.z; k < vector3Int.z; k++)
					{
						Vector3Int position = new Vector3Int(i, j, k);
						if (!voxelGrid.Contains(position))
						{
							voxelGrid.Set(position, new VoxelData(color));
							filled++;
						}
					}
				}
			}
			return voxelGrid;
		}
	}
}
