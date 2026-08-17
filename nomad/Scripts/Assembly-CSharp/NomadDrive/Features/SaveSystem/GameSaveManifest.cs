using System;
using System.Collections.Generic;
using System.IO;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.EvilSave;
using UnityEngine;

namespace NomadDrive.Features.SaveSystem
{
	public class GameSaveManifest
	{
		public const int CurrentFormatVersion = 5;

		public const string ManifestKey = "game.manifest";

		public int FormatVersion = 5;

		public DateTime SavedAtUtc = DateTime.UtcNow;

		public int Seed;

		public Vector3 PlayerSpawnPoint;

		public float TimeOfDay;

		public PlayerRecord HostPlayer;

		public readonly List<PlayerRecord> RemotePlayers = new List<PlayerRecord>();

		public readonly List<ObjectRecord> DynamicObjects = new List<ObjectRecord>();

		public readonly List<string> BlobRefs = new List<string>();

		public readonly List<int> SpawnedLootSeeds = new List<int>();

		public byte[] Serialize()
		{
			using MemoryStream memoryStream = new MemoryStream();
			using (EvilWriter evilWriter = new EvilWriter(memoryStream))
			{
				evilWriter.Write(FormatVersion);
				evilWriter.Write(SavedAtUtc.ToBinary());
				evilWriter.Write(Seed);
				evilWriter.Write(PlayerSpawnPoint);
				evilWriter.Write(TimeOfDay);
				evilWriter.Write(HostPlayer != null);
				if (HostPlayer != null)
				{
					WritePlayer(evilWriter, HostPlayer);
				}
				evilWriter.Write(DynamicObjects.Count);
				foreach (ObjectRecord dynamicObject in DynamicObjects)
				{
					WriteObject(evilWriter, dynamicObject);
				}
				evilWriter.Write(BlobRefs.Count);
				foreach (string blobRef in BlobRefs)
				{
					evilWriter.Write(blobRef ?? string.Empty);
				}
				evilWriter.Write(RemotePlayers.Count);
				foreach (PlayerRecord remotePlayer in RemotePlayers)
				{
					WritePlayer(evilWriter, remotePlayer);
				}
				evilWriter.Write(SpawnedLootSeeds.Count);
				foreach (int spawnedLootSeed in SpawnedLootSeeds)
				{
					evilWriter.Write(spawnedLootSeed);
				}
			}
			return memoryStream.ToArray();
		}

		public static GameSaveManifest Deserialize(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
			{
				return null;
			}
			try
			{
				using MemoryStream stream = new MemoryStream(bytes);
				using EvilReader evilReader = new EvilReader(stream);
				GameSaveManifest gameSaveManifest = new GameSaveManifest
				{
					FormatVersion = evilReader.ReadInt(),
					SavedAtUtc = DateTime.FromBinary(evilReader.ReadLong()),
					Seed = evilReader.ReadInt(),
					PlayerSpawnPoint = evilReader.ReadVector3()
				};
				if (gameSaveManifest.FormatVersion >= 2)
				{
					gameSaveManifest.TimeOfDay = evilReader.ReadFloat();
				}
				_ = gameSaveManifest.FormatVersion;
				_ = 5;
				if (evilReader.ReadBool())
				{
					gameSaveManifest.HostPlayer = ReadPlayer(evilReader, gameSaveManifest.FormatVersion);
				}
				int num = evilReader.ReadInt();
				for (int i = 0; i < num; i++)
				{
					gameSaveManifest.DynamicObjects.Add(ReadObject(evilReader));
				}
				int num2 = evilReader.ReadInt();
				for (int j = 0; j < num2; j++)
				{
					gameSaveManifest.BlobRefs.Add(evilReader.ReadString());
				}
				if (gameSaveManifest.FormatVersion >= 3)
				{
					int num3 = evilReader.ReadInt();
					for (int k = 0; k < num3; k++)
					{
						gameSaveManifest.RemotePlayers.Add(ReadPlayer(evilReader, gameSaveManifest.FormatVersion));
					}
				}
				if (gameSaveManifest.FormatVersion >= 5)
				{
					int num4 = evilReader.ReadInt();
					for (int l = 0; l < num4; l++)
					{
						gameSaveManifest.SpawnedLootSeeds.Add(evilReader.ReadInt());
					}
				}
				return gameSaveManifest;
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[GameSaveManifest] Failed to deserialize manifest: " + ex.Message, "Deserialize", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\SaveSystem\\GameSaveManifest.cs", 166);
				return null;
			}
		}

		private static void WritePlayer(EvilWriter w, PlayerRecord p)
		{
			w.Write(p.Puid ?? string.Empty);
			w.Write(p.DisplayName ?? string.Empty);
			w.Write(p.Position);
			w.Write(p.EulerAngles);
			w.Write(p.SurvivalBlob ?? Array.Empty<byte>());
			w.Write(p.EquippedItemGuid ?? string.Empty);
			w.Write(p.SeatGuid ?? string.Empty);
			w.Write(p.IsDowned);
		}

		private static PlayerRecord ReadPlayer(EvilReader r, int formatVersion)
		{
			PlayerRecord playerRecord = new PlayerRecord
			{
				Puid = r.ReadString(),
				DisplayName = r.ReadString(),
				Position = r.ReadVector3(),
				EulerAngles = r.ReadVector3(),
				SurvivalBlob = r.ReadBytes(),
				EquippedItemGuid = r.ReadString(),
				SeatGuid = r.ReadString()
			};
			if (formatVersion >= 4)
			{
				playerRecord.IsDowned = r.ReadBool();
			}
			return playerRecord;
		}

		private static void WriteObject(EvilWriter w, ObjectRecord record)
		{
			w.Write(record.Guid ?? string.Empty);
			w.Write(record.AddressableGuid ?? string.Empty);
			w.Write(record.SpawnSeed);
			w.Write(record.Position);
			w.Write(record.Rotation);
			w.Write(record.ParentLink.ParentGuid ?? string.Empty);
			w.Write((byte)record.ParentLink.LinkKind);
			w.Write(record.ParentLink.SubIndex);
			w.Write(record.Contributors.Count);
			foreach (KeyValuePair<string, byte[]> contributor in record.Contributors)
			{
				w.Write(contributor.Key ?? string.Empty);
				w.Write(contributor.Value ?? Array.Empty<byte>());
			}
		}

		private static ObjectRecord ReadObject(EvilReader r)
		{
			ObjectRecord objectRecord = new ObjectRecord
			{
				Guid = r.ReadString(),
				AddressableGuid = r.ReadString(),
				SpawnSeed = r.ReadInt(),
				Position = r.ReadVector3(),
				Rotation = r.ReadQuaternion(),
				ParentLink = new SaveParentLink
				{
					ParentGuid = r.ReadString(),
					LinkKind = (SaveLinkKind)r.ReadByte(),
					SubIndex = r.ReadByte()
				}
			};
			int num = r.ReadInt();
			for (int i = 0; i < num; i++)
			{
				string key = r.ReadString();
				byte[] value = r.ReadBytes();
				objectRecord.Contributors[key] = value;
			}
			return objectRecord;
		}
	}
}
