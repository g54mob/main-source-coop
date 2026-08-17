using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace EvilCore.EvilSave
{
	public sealed class EvilReader : IDisposable
	{
		private readonly BinaryReader _br;

		public EvilReader(Stream stream)
		{
			_br = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);
		}

		public T Read<T>()
		{
			return (T)TypeSerializers.ReadValue(_br, typeof(T));
		}

		public bool ReadBool()
		{
			return _br.ReadBoolean();
		}

		public byte ReadByte()
		{
			return _br.ReadByte();
		}

		public sbyte ReadSByte()
		{
			return _br.ReadSByte();
		}

		public short ReadShort()
		{
			return _br.ReadInt16();
		}

		public ushort ReadUShort()
		{
			return _br.ReadUInt16();
		}

		public int ReadInt()
		{
			return _br.ReadInt32();
		}

		public uint ReadUInt()
		{
			return _br.ReadUInt32();
		}

		public long ReadLong()
		{
			return _br.ReadInt64();
		}

		public ulong ReadULong()
		{
			return _br.ReadUInt64();
		}

		public float ReadFloat()
		{
			return _br.ReadSingle();
		}

		public double ReadDouble()
		{
			return _br.ReadDouble();
		}

		public decimal ReadDecimal()
		{
			return _br.ReadDecimal();
		}

		public char ReadChar()
		{
			return _br.ReadChar();
		}

		public string ReadString()
		{
			return _br.ReadString();
		}

		public Vector2 ReadVector2()
		{
			return new Vector2(_br.ReadSingle(), _br.ReadSingle());
		}

		public Vector3 ReadVector3()
		{
			return new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
		}

		public Vector4 ReadVector4()
		{
			return new Vector4(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
		}

		public Vector2Int ReadVector2Int()
		{
			return new Vector2Int(_br.ReadInt32(), _br.ReadInt32());
		}

		public Vector3Int ReadVector3Int()
		{
			return new Vector3Int(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32());
		}

		public Quaternion ReadQuaternion()
		{
			return new Quaternion(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
		}

		public Color ReadColor()
		{
			return new Color(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
		}

		public Color32 ReadColor32()
		{
			return new Color32(_br.ReadByte(), _br.ReadByte(), _br.ReadByte(), _br.ReadByte());
		}

		public Rect ReadRect()
		{
			return new Rect(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
		}

		public RectInt ReadRectInt()
		{
			return new RectInt(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32());
		}

		public Bounds ReadBounds()
		{
			return new Bounds(ReadVector3(), ReadVector3());
		}

		public BoundsInt ReadBoundsInt()
		{
			return new BoundsInt(new Vector3Int(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()), new Vector3Int(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()));
		}

		public LayerMask ReadLayerMask()
		{
			return _br.ReadInt32();
		}

		public byte[] ReadBytes()
		{
			int count = _br.ReadInt32();
			return _br.ReadBytes(count);
		}

		public List<T> ReadList<T>()
		{
			int num = _br.ReadInt32();
			List<T> list = new List<T>(num);
			Type typeFromHandle = typeof(T);
			for (int i = 0; i < num; i++)
			{
				list.Add((T)TypeSerializers.ReadValue(_br, typeFromHandle));
			}
			return list;
		}

		public Dictionary<TK, TV> ReadDictionary<TK, TV>()
		{
			int num = _br.ReadInt32();
			Dictionary<TK, TV> dictionary = new Dictionary<TK, TV>(num);
			Type typeFromHandle = typeof(TK);
			Type typeFromHandle2 = typeof(TV);
			for (int i = 0; i < num; i++)
			{
				TK key = (TK)TypeSerializers.ReadValue(_br, typeFromHandle);
				TV value = (TV)TypeSerializers.ReadValue(_br, typeFromHandle2);
				dictionary[key] = value;
			}
			return dictionary;
		}

		public T? ReadNullable<T>() where T : struct
		{
			if (!_br.ReadBoolean())
			{
				return null;
			}
			return (T)TypeSerializers.ReadValue(_br, typeof(T));
		}

		public void Dispose()
		{
			_br?.Dispose();
		}
	}
}
