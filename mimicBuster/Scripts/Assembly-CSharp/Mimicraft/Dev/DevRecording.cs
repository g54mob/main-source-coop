using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Mimicraft.Gameplay;
using UnityEngine;

namespace Mimicraft.Dev
{
	public class DevRecording
	{
		public class Actor
		{
			public ulong Id;

			public string DisplayName = "";

			public byte Role;

			public string WeaponId = "";

			public byte[] CharacterBytes = Array.Empty<byte>();

			public byte[] BodyBytes = Array.Empty<byte>();

			public byte[] WeaponSkinBytes = Array.Empty<byte>();

			public string[] CharacterBoxes = Array.Empty<string>();

			public string[] Parameters = Array.Empty<string>();

			public int LayerCount;

			public readonly List<Sample> Samples = new List<Sample>();
		}

		public struct LayerState
		{
			public int Hash;

			public float NormalizedTime;

			public float Weight;
		}

		public struct ImpactEvent
		{
			public int Frame;

			public byte Kind;

			public Vector3 Position;

			public Vector3 Normal;

			public ulong Actor;

			public byte Weapon;
		}

		public struct BodyChange
		{
			public int Frame;

			public ulong Actor;

			public byte[] Bytes;
		}

		public struct Reconstruct
		{
			public int Frame;

			public ulong Actor;

			public float Seconds;
		}

		public struct BonePose
		{
			public Vector3 Position;

			public Quaternion Rotation;
		}

		public struct Sample
		{
			public bool Present;

			public Vector3 Position;

			public Quaternion Rotation;

			public float[] Parameters;

			public LayerState[] Layers;

			public bool Running;

			public BonePose[] Bones;

			public byte Weapon;
		}

		public const string Extension = ".mrec";

		private const string Magic = "MRC";

		private const byte Version = 9;

		private const byte VersionWithoutCharacterBoxes = 7;

		private const byte VersionWithoutReconstructs = 8;

		private const int MaxActors = 64;

		private const int MaxParameters = 32;

		private const int MaxLayers = 16;

		private const int MaxBones = 128;

		private const int MaxEvents = 200000;

		private const int MaxFrames = 216000;

		public string Name = "";

		public string MapId = "";

		public float TickRate = 30f;

		public readonly List<Actor> Actors = new List<Actor>();

		public readonly List<string> WeaponIds = new List<string> { "" };

		public readonly List<ImpactEvent> Events = new List<ImpactEvent>();

		public readonly List<BodyChange> BodyChanges = new List<BodyChange>();

		public readonly List<Reconstruct> Reconstructs = new List<Reconstruct>();

		public int FrameCount { get; private set; }

		public float Duration
		{
			get
			{
				if (!(TickRate > 0f))
				{
					return 0f;
				}
				return (float)FrameCount / TickRate;
			}
		}

		public static string Directory => Path.Combine(Application.persistentDataPath, "Recordings");

		public byte WeaponIndex(string weaponId)
		{
			if (string.IsNullOrEmpty(weaponId))
			{
				return 0;
			}
			for (int i = 0; i < WeaponIds.Count; i++)
			{
				if (WeaponIds[i] == weaponId)
				{
					return (byte)i;
				}
			}
			if (WeaponIds.Count >= 255)
			{
				return 0;
			}
			WeaponIds.Add(weaponId);
			return (byte)(WeaponIds.Count - 1);
		}

		public string WeaponIdAt(int index)
		{
			if (index <= 0 || index >= WeaponIds.Count)
			{
				return "";
			}
			return WeaponIds[index];
		}

		public WeaponDefinition WeaponAt(int index)
		{
			string text = WeaponIdAt(index);
			if (!string.IsNullOrEmpty(text))
			{
				return WeaponCatalog.Find(text);
			}
			return null;
		}

		public bool TryGetReconstruct(ulong actor, out Reconstruct found)
		{
			foreach (Reconstruct reconstruct in Reconstructs)
			{
				if (reconstruct.Actor == actor)
				{
					found = reconstruct;
					return true;
				}
			}
			found = default(Reconstruct);
			return false;
		}

		public void SetReconstruct(ulong actor, int frame, float seconds)
		{
			ClearReconstruct(actor);
			Reconstructs.Add(new Reconstruct
			{
				Actor = actor,
				Frame = Mathf.Max(0, frame),
				Seconds = Mathf.Clamp(seconds, 0.1f, 600f)
			});
		}

		public bool ClearReconstruct(ulong actor)
		{
			for (int num = Reconstructs.Count - 1; num >= 0; num--)
			{
				if (Reconstructs[num].Actor == actor)
				{
					Reconstructs.RemoveAt(num);
					return true;
				}
			}
			return false;
		}

		public void NoteFrame(int frameCount)
		{
			FrameCount = frameCount;
		}

		public Actor Find(ulong id)
		{
			foreach (Actor actor in Actors)
			{
				if (actor.Id == id)
				{
					return actor;
				}
			}
			return null;
		}

		public static string PathFor(string name)
		{
			return Path.Combine(Directory, SanitizeName(name) + ".mrec");
		}

		public static string SanitizeName(string name)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = (name ?? "").Trim();
			foreach (char c in text)
			{
				if (char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == ' ')
				{
					stringBuilder.Append(c);
				}
			}
			if (stringBuilder.Length <= 0)
			{
				return "kayit";
			}
			return stringBuilder.ToString();
		}

		public static List<string> ListNames()
		{
			List<string> list = new List<string>();
			if (!System.IO.Directory.Exists(Directory))
			{
				return list;
			}
			string[] files = System.IO.Directory.GetFiles(Directory, "*.mrec");
			foreach (string path in files)
			{
				list.Add(Path.GetFileNameWithoutExtension(path));
			}
			return list;
		}

		public void Save(string path)
		{
			System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path) ?? Directory);
			using FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
			using DeflateStream output = new DeflateStream(stream, System.IO.Compression.CompressionLevel.Optimal);
			using BinaryWriter binaryWriter = new BinaryWriter(output, Encoding.UTF8);
			binaryWriter.Write(Encoding.ASCII.GetBytes("MRC"));
			binaryWriter.Write((byte)9);
			binaryWriter.Write(Name ?? "");
			binaryWriter.Write(MapId ?? "");
			binaryWriter.Write(TickRate);
			binaryWriter.Write(FrameCount);
			binaryWriter.Write(WeaponIds.Count);
			foreach (string weaponId in WeaponIds)
			{
				binaryWriter.Write(weaponId ?? "");
			}
			binaryWriter.Write(Actors.Count);
			foreach (Actor actor in Actors)
			{
				binaryWriter.Write(actor.Id);
				binaryWriter.Write(actor.DisplayName ?? "");
				binaryWriter.Write(actor.Role);
				binaryWriter.Write(actor.WeaponId ?? "");
				WriteBytes(binaryWriter, actor.CharacterBytes);
				WriteBytes(binaryWriter, actor.BodyBytes);
				WriteBytes(binaryWriter, actor.WeaponSkinBytes);
				binaryWriter.Write(actor.CharacterBoxes.Length);
				string[] characterBoxes = actor.CharacterBoxes;
				foreach (string text in characterBoxes)
				{
					binaryWriter.Write(text ?? "");
				}
				binaryWriter.Write(actor.Parameters.Length);
				characterBoxes = actor.Parameters;
				foreach (string text2 in characterBoxes)
				{
					binaryWriter.Write(text2 ?? "");
				}
				binaryWriter.Write(actor.LayerCount);
			}
			binaryWriter.Write(Events.Count);
			foreach (ImpactEvent @event in Events)
			{
				binaryWriter.Write(@event.Frame);
				binaryWriter.Write(@event.Kind);
				binaryWriter.Write(@event.Position.x);
				binaryWriter.Write(@event.Position.y);
				binaryWriter.Write(@event.Position.z);
				binaryWriter.Write(@event.Normal.x);
				binaryWriter.Write(@event.Normal.y);
				binaryWriter.Write(@event.Normal.z);
				binaryWriter.Write(@event.Actor);
				binaryWriter.Write(@event.Weapon);
			}
			binaryWriter.Write(BodyChanges.Count);
			foreach (BodyChange bodyChange in BodyChanges)
			{
				binaryWriter.Write(bodyChange.Frame);
				binaryWriter.Write(bodyChange.Actor);
				WriteBytes(binaryWriter, bodyChange.Bytes);
			}
			binaryWriter.Write(Reconstructs.Count);
			foreach (Reconstruct reconstruct in Reconstructs)
			{
				binaryWriter.Write(reconstruct.Frame);
				binaryWriter.Write(reconstruct.Actor);
				binaryWriter.Write(reconstruct.Seconds);
			}
			for (int j = 0; j < FrameCount; j++)
			{
				foreach (Actor actor2 in Actors)
				{
					Sample sample = ((j < actor2.Samples.Count) ? actor2.Samples[j] : default(Sample));
					binaryWriter.Write(sample.Present);
					if (sample.Present)
					{
						binaryWriter.Write(sample.Position.x);
						binaryWriter.Write(sample.Position.y);
						binaryWriter.Write(sample.Position.z);
						binaryWriter.Write(sample.Rotation.x);
						binaryWriter.Write(sample.Rotation.y);
						binaryWriter.Write(sample.Rotation.z);
						binaryWriter.Write(sample.Rotation.w);
						binaryWriter.Write(sample.Weapon);
						binaryWriter.Write(sample.Running);
						for (int k = 0; k < actor2.Parameters.Length; k++)
						{
							binaryWriter.Write((sample.Parameters != null && k < sample.Parameters.Length) ? sample.Parameters[k] : 0f);
						}
						for (int l = 0; l < actor2.LayerCount; l++)
						{
							LayerState layerState = ((sample.Layers != null && l < sample.Layers.Length) ? sample.Layers[l] : default(LayerState));
							binaryWriter.Write(layerState.Hash);
							binaryWriter.Write(layerState.NormalizedTime);
							binaryWriter.Write(layerState.Weight);
						}
						int num = ((sample.Bones != null) ? sample.Bones.Length : 0);
						binaryWriter.Write((ushort)num);
						for (int m = 0; m < num; m++)
						{
							BonePose bonePose = sample.Bones[m];
							binaryWriter.Write(bonePose.Position.x);
							binaryWriter.Write(bonePose.Position.y);
							binaryWriter.Write(bonePose.Position.z);
							binaryWriter.Write(bonePose.Rotation.x);
							binaryWriter.Write(bonePose.Rotation.y);
							binaryWriter.Write(bonePose.Rotation.z);
							binaryWriter.Write(bonePose.Rotation.w);
						}
					}
				}
			}
		}

		public static bool TryLoad(string path, out DevRecording recording)
		{
			recording = null;
			try
			{
				using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
				using DeflateStream input = new DeflateStream(stream, CompressionMode.Decompress);
				using BinaryReader binaryReader = new BinaryReader(input, Encoding.UTF8);
				if (Encoding.ASCII.GetString(binaryReader.ReadBytes(3)) != "MRC")
				{
					return false;
				}
				byte b = binaryReader.ReadByte();
				if (b != 9 && b != 8 && b != 7)
				{
					return false;
				}
				DevRecording devRecording = new DevRecording
				{
					Name = binaryReader.ReadString(),
					MapId = binaryReader.ReadString(),
					TickRate = binaryReader.ReadSingle()
				};
				int num = binaryReader.ReadInt32();
				int num2 = binaryReader.ReadInt32();
				if (num2 < 1 || num2 > 255)
				{
					return false;
				}
				devRecording.WeaponIds.Clear();
				for (int i = 0; i < num2; i++)
				{
					devRecording.WeaponIds.Add(binaryReader.ReadString());
				}
				int num3 = binaryReader.ReadInt32();
				if (num < 0 || num > 216000 || num3 < 0 || num3 > 64)
				{
					return false;
				}
				for (int j = 0; j < num3; j++)
				{
					Actor actor = new Actor
					{
						Id = binaryReader.ReadUInt64(),
						DisplayName = binaryReader.ReadString(),
						Role = binaryReader.ReadByte(),
						WeaponId = binaryReader.ReadString(),
						CharacterBytes = ReadBytes(binaryReader),
						BodyBytes = ReadBytes(binaryReader),
						WeaponSkinBytes = ReadBytes(binaryReader),
						CharacterBoxes = ((b > 7) ? ReadStrings(binaryReader) : Array.Empty<string>())
					};
					int num4 = binaryReader.ReadInt32();
					if (num4 < 0 || num4 > 32)
					{
						return false;
					}
					actor.Parameters = new string[num4];
					for (int k = 0; k < num4; k++)
					{
						actor.Parameters[k] = binaryReader.ReadString();
					}
					actor.LayerCount = binaryReader.ReadInt32();
					if (actor.LayerCount < 0 || actor.LayerCount > 16)
					{
						return false;
					}
					devRecording.Actors.Add(actor);
				}
				int num5 = binaryReader.ReadInt32();
				if (num5 < 0 || num5 > 200000)
				{
					return false;
				}
				for (int l = 0; l < num5; l++)
				{
					devRecording.Events.Add(new ImpactEvent
					{
						Frame = binaryReader.ReadInt32(),
						Kind = binaryReader.ReadByte(),
						Position = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()),
						Normal = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()),
						Actor = binaryReader.ReadUInt64(),
						Weapon = binaryReader.ReadByte()
					});
				}
				int num6 = binaryReader.ReadInt32();
				if (num6 < 0 || num6 > 200000)
				{
					return false;
				}
				for (int m = 0; m < num6; m++)
				{
					devRecording.BodyChanges.Add(new BodyChange
					{
						Frame = binaryReader.ReadInt32(),
						Actor = binaryReader.ReadUInt64(),
						Bytes = ReadBytes(binaryReader)
					});
				}
				if (b > 8)
				{
					int num7 = binaryReader.ReadInt32();
					if (num7 < 0 || num7 > 200000)
					{
						return false;
					}
					for (int n = 0; n < num7; n++)
					{
						devRecording.Reconstructs.Add(new Reconstruct
						{
							Frame = binaryReader.ReadInt32(),
							Actor = binaryReader.ReadUInt64(),
							Seconds = binaryReader.ReadSingle()
						});
					}
				}
				for (int num8 = 0; num8 < num; num8++)
				{
					foreach (Actor actor2 in devRecording.Actors)
					{
						Sample item = new Sample
						{
							Present = binaryReader.ReadBoolean()
						};
						if (item.Present)
						{
							item.Position = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
							item.Rotation = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
							item.Weapon = binaryReader.ReadByte();
							item.Running = binaryReader.ReadBoolean();
							item.Parameters = new float[actor2.Parameters.Length];
							for (int num9 = 0; num9 < item.Parameters.Length; num9++)
							{
								item.Parameters[num9] = binaryReader.ReadSingle();
							}
							item.Layers = new LayerState[actor2.LayerCount];
							for (int num10 = 0; num10 < item.Layers.Length; num10++)
							{
								item.Layers[num10] = new LayerState
								{
									Hash = binaryReader.ReadInt32(),
									NormalizedTime = binaryReader.ReadSingle(),
									Weight = binaryReader.ReadSingle()
								};
							}
							int num11 = binaryReader.ReadUInt16();
							if (num11 > 128)
							{
								return false;
							}
							if (num11 > 0)
							{
								item.Bones = new BonePose[num11];
								for (int num12 = 0; num12 < num11; num12++)
								{
									item.Bones[num12] = new BonePose
									{
										Position = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()),
										Rotation = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle())
									};
								}
							}
						}
						actor2.Samples.Add(item);
					}
				}
				devRecording.FrameCount = num;
				recording = devRecording;
				return true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[DevRecording] '" + path + "' okunamadi: " + ex.Message);
				return false;
			}
		}

		private static void WriteBytes(BinaryWriter writer, byte[] bytes)
		{
			if (bytes == null)
			{
				bytes = Array.Empty<byte>();
			}
			writer.Write(bytes.Length);
			writer.Write(bytes);
		}

		private static string[] ReadStrings(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			if (num <= 0 || num > 4096)
			{
				return Array.Empty<string>();
			}
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadString();
			}
			return array;
		}

		private static byte[] ReadBytes(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			if (num < 0 || num > 8388608)
			{
				throw new InvalidDataException($"Gecersiz uzunluk: {num}");
			}
			return reader.ReadBytes(num);
		}
	}
}
