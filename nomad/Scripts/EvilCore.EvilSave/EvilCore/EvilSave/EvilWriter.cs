using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace EvilCore.EvilSave
{
	public sealed class EvilWriter : IDisposable
	{
		private readonly BinaryWriter _bw;

		public EvilWriter(Stream stream)
		{
			_bw = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
		}

		public void Write<T>(T value)
		{
			TypeSerializers.WriteValue(_bw, value, typeof(T));
		}

		public void Write(bool value)
		{
			_bw.Write(value);
		}

		public void Write(byte value)
		{
			_bw.Write(value);
		}

		public void Write(sbyte value)
		{
			_bw.Write(value);
		}

		public void Write(short value)
		{
			_bw.Write(value);
		}

		public void Write(ushort value)
		{
			_bw.Write(value);
		}

		public void Write(int value)
		{
			_bw.Write(value);
		}

		public void Write(uint value)
		{
			_bw.Write(value);
		}

		public void Write(long value)
		{
			_bw.Write(value);
		}

		public void Write(ulong value)
		{
			_bw.Write(value);
		}

		public void Write(float value)
		{
			_bw.Write(value);
		}

		public void Write(double value)
		{
			_bw.Write(value);
		}

		public void Write(decimal value)
		{
			_bw.Write(value);
		}

		public void Write(char value)
		{
			_bw.Write(value);
		}

		public void Write(string value)
		{
			_bw.Write(value ?? string.Empty);
		}

		public void Write(Vector2 value)
		{
			_bw.Write(value.x);
			_bw.Write(value.y);
		}

		public void Write(Vector3 value)
		{
			_bw.Write(value.x);
			_bw.Write(value.y);
			_bw.Write(value.z);
		}

		public void Write(Vector4 value)
		{
			_bw.Write(value.x);
			_bw.Write(value.y);
			_bw.Write(value.z);
			_bw.Write(value.w);
		}

		public void Write(Vector2Int value)
		{
			_bw.Write(value.x);
			_bw.Write(value.y);
		}

		public void Write(Vector3Int value)
		{
			_bw.Write(value.x);
			_bw.Write(value.y);
			_bw.Write(value.z);
		}

		public void Write(Quaternion value)
		{
			_bw.Write(value.x);
			_bw.Write(value.y);
			_bw.Write(value.z);
			_bw.Write(value.w);
		}

		public void Write(Color value)
		{
			_bw.Write(value.r);
			_bw.Write(value.g);
			_bw.Write(value.b);
			_bw.Write(value.a);
		}

		public void Write(Color32 value)
		{
			_bw.Write(value.r);
			_bw.Write(value.g);
			_bw.Write(value.b);
			_bw.Write(value.a);
		}

		public void Write(Rect value)
		{
			_bw.Write(value.x);
			_bw.Write(value.y);
			_bw.Write(value.width);
			_bw.Write(value.height);
		}

		public void Write(RectInt value)
		{
			_bw.Write(value.x);
			_bw.Write(value.y);
			_bw.Write(value.width);
			_bw.Write(value.height);
		}

		public void Write(Bounds value)
		{
			Write(value.center);
			Write(value.size);
		}

		public void Write(BoundsInt value)
		{
			_bw.Write(value.position.x);
			_bw.Write(value.position.y);
			_bw.Write(value.position.z);
			_bw.Write(value.size.x);
			_bw.Write(value.size.y);
			_bw.Write(value.size.z);
		}

		public void Write(LayerMask value)
		{
			_bw.Write(value.value);
		}

		public void Write(byte[] value)
		{
			_bw.Write(value.Length);
			_bw.Write(value);
		}

		public void WriteCollection<T>(ICollection<T> collection)
		{
			_bw.Write(collection.Count);
			Type typeFromHandle = typeof(T);
			foreach (T item in collection)
			{
				TypeSerializers.WriteValue(_bw, item, typeFromHandle);
			}
		}

		public void WriteDictionary<TK, TV>(IDictionary<TK, TV> dict)
		{
			_bw.Write(dict.Count);
			Type typeFromHandle = typeof(TK);
			Type typeFromHandle2 = typeof(TV);
			foreach (KeyValuePair<TK, TV> item in dict)
			{
				TypeSerializers.WriteValue(_bw, item.Key, typeFromHandle);
				TypeSerializers.WriteValue(_bw, item.Value, typeFromHandle2);
			}
		}

		public void WriteNullable<T>(T? value) where T : struct
		{
			_bw.Write(value.HasValue);
			if (value.HasValue)
			{
				TypeSerializers.WriteValue(_bw, value.Value, typeof(T));
			}
		}

		public void Dispose()
		{
			_bw?.Dispose();
		}
	}
}
