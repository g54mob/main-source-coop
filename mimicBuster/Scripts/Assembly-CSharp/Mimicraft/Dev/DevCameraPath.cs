using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;

namespace Mimicraft.Dev
{
	public class DevCameraPath
	{
		public struct Key
		{
			public Vector3 Position;

			public Quaternion Rotation;

			public float FieldOfView;
		}

		public const string Extension = ".campath";

		private const string Magic = "MCP";

		private const byte Version = 1;

		private const int MaxKeys = 432000;

		public string Name = "";

		public float TickRate = 60f;

		public readonly List<Key> Keys = new List<Key>();

		public float Duration
		{
			get
			{
				if (!(TickRate > 0f))
				{
					return 0f;
				}
				return (float)Keys.Count / TickRate;
			}
		}

		public static string Directory => Path.Combine(Application.persistentDataPath, "CameraPaths");

		public Key Sample(float seconds)
		{
			if (Keys.Count == 0)
			{
				return default(Key);
			}
			float num = Mathf.Clamp(seconds, 0f, Duration) * TickRate;
			int num2 = Mathf.Clamp(Mathf.FloorToInt(num), 0, Keys.Count - 1);
			int index = Mathf.Min(num2 + 1, Keys.Count - 1);
			float t = num - (float)num2;
			Key key = Keys[num2];
			Key key2 = Keys[index];
			return new Key
			{
				Position = Vector3.Lerp(key.Position, key2.Position, t),
				Rotation = Quaternion.Slerp(key.Rotation, key2.Rotation, t),
				FieldOfView = Mathf.Lerp(key.FieldOfView, key2.FieldOfView, t)
			};
		}

		public void Smooth(int window)
		{
			if (Keys.Count < 3 || window < 1)
			{
				return;
			}
			List<Key> list = new List<Key>(Keys.Count);
			for (int i = 0; i < Keys.Count; i++)
			{
				Vector3 zero = Vector3.zero;
				float num = 0f;
				int num2 = 0;
				for (int j = -window; j <= window; j++)
				{
					int index = Mathf.Clamp(i + j, 0, Keys.Count - 1);
					zero += Keys[index].Position;
					num += Keys[index].FieldOfView;
					num2++;
				}
				Quaternion rotation = Keys[i].Rotation;
				rotation = Quaternion.Slerp(rotation, Keys[Mathf.Max(0, i - window)].Rotation, 0.25f);
				rotation = Quaternion.Slerp(rotation, Keys[Mathf.Min(Keys.Count - 1, i + window)].Rotation, 0.25f);
				list.Add(new Key
				{
					Position = zero / num2,
					Rotation = rotation,
					FieldOfView = num / (float)num2
				});
			}
			Keys.Clear();
			Keys.AddRange(list);
		}

		public static string PathFor(string name)
		{
			return Path.Combine(Directory, DevRecording.SanitizeName(name) + ".campath");
		}

		public static List<string> ListNames()
		{
			List<string> list = new List<string>();
			if (!System.IO.Directory.Exists(Directory))
			{
				return list;
			}
			string[] files = System.IO.Directory.GetFiles(Directory, "*.campath");
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
			binaryWriter.Write(Encoding.ASCII.GetBytes("MCP"));
			binaryWriter.Write((byte)1);
			binaryWriter.Write(Name ?? "");
			binaryWriter.Write(TickRate);
			binaryWriter.Write(Keys.Count);
			foreach (Key key in Keys)
			{
				binaryWriter.Write(key.Position.x);
				binaryWriter.Write(key.Position.y);
				binaryWriter.Write(key.Position.z);
				binaryWriter.Write(key.Rotation.x);
				binaryWriter.Write(key.Rotation.y);
				binaryWriter.Write(key.Rotation.z);
				binaryWriter.Write(key.Rotation.w);
				binaryWriter.Write(key.FieldOfView);
			}
		}

		public static bool TryLoad(string path, out DevCameraPath loaded)
		{
			loaded = null;
			try
			{
				using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
				using DeflateStream input = new DeflateStream(stream, CompressionMode.Decompress);
				using BinaryReader binaryReader = new BinaryReader(input, Encoding.UTF8);
				if (Encoding.ASCII.GetString(binaryReader.ReadBytes(3)) != "MCP" || binaryReader.ReadByte() != 1)
				{
					return false;
				}
				DevCameraPath devCameraPath = new DevCameraPath
				{
					Name = binaryReader.ReadString(),
					TickRate = binaryReader.ReadSingle()
				};
				int num = binaryReader.ReadInt32();
				if (num < 0 || num > 432000)
				{
					return false;
				}
				for (int i = 0; i < num; i++)
				{
					devCameraPath.Keys.Add(new Key
					{
						Position = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()),
						Rotation = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()),
						FieldOfView = binaryReader.ReadSingle()
					});
				}
				loaded = devCameraPath;
				return true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[DevCameraPath] '" + path + "' okunamadi: " + ex.Message);
				return false;
			}
		}
	}
}
