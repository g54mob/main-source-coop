using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Fusion
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	[NetworkStructWeaved(4)]
	public struct NetworkPrefabRef : INetworkStruct, IEquatable<NetworkPrefabRef>, IComparable<NetworkPrefabRef>
	{
		public sealed class EqualityComparer : IEqualityComparer<NetworkPrefabRef>
		{
			public bool Equals(NetworkPrefabRef x, NetworkPrefabRef y)
			{
				return x.Equals(y);
			}

			public int GetHashCode(NetworkPrefabRef obj)
			{
				return obj.GetHashCode();
			}
		}

		public const int ALIGNMENT = 4;

		[FieldOffset(0)]
		public unsafe fixed long RawGuidValue[2];

		[FieldOffset(0)]
		[NonSerialized]
		private long _data0;

		[FieldOffset(8)]
		[NonSerialized]
		private long _data1;

		public const int SIZE = 16;

		public const int WORD_COUNT = 4;

		internal const int BYTE_OF_RAW_GUID_VALUE = 0;

		internal const int BYTE_COUNT_OF_RAW_GUID_VALUE = 16;

		internal const int BYTE_OF__DATA0 = 0;

		internal const int BYTE_COUNT_OF__DATA0 = 8;

		internal const int BYTE_OF__DATA1 = 8;

		internal const int BYTE_COUNT_OF__DATA1 = 8;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public static NetworkPrefabRef Empty => default(NetworkPrefabRef);

		public readonly bool IsValid => _data0 != 0L || _data1 != 0;

		public NetworkPrefabRef(string guid)
			: base(guid)
		{
		}

		public NetworkPrefabRef(long data0, long data1)
		{
			_data0 = data0;
			_data1 = data1;
		}

		public NetworkPrefabRef(byte[] guid)
		{
			_data0 = BitConverter.ToInt64(guid, 0);
			_data1 = BitConverter.ToInt64(guid, 8);
		}

		public unsafe NetworkPrefabRef(byte* guid)
		{
			_data0 = *(long*)guid;
			_data1 = ((long*)guid)[1];
		}

		public unsafe static implicit operator NetworkPrefabRef(Guid guid)
		{
			NetworkPrefabRef result = default(NetworkPrefabRef);
			NetworkObjectGuidUtils.CopyAndMangleGuid((byte*)(&guid), (byte*)(&result));
			return result;
		}

		public unsafe static implicit operator Guid(NetworkPrefabRef guid)
		{
			Guid result = default(Guid);
			NetworkObjectGuidUtils.CopyAndMangleGuid((byte*)(&guid), (byte*)(&result));
			return result;
		}

		public static bool TryParse(string str, out NetworkPrefabRef guid)
		{
			if (Guid.TryParse(str, out var result))
			{
				guid = result;
				return true;
			}
			guid = default(NetworkPrefabRef);
			return false;
		}

		public static NetworkPrefabRef Parse(string str)
		{
			return Guid.Parse(str);
		}

		public static bool operator ==(NetworkPrefabRef a, NetworkPrefabRef b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(NetworkPrefabRef a, NetworkPrefabRef b)
		{
			return !a.Equals(b);
		}

		public readonly bool Equals(NetworkPrefabRef other)
		{
			return _data0 == other._data0 && _data1 == other._data1;
		}

		public override readonly bool Equals(object obj)
		{
			return obj is NetworkPrefabRef other && Equals(other);
		}

		public override readonly int GetHashCode()
		{
			return ((Guid)this/*cast due to .constrained prefix*/).GetHashCode();
		}

		public override readonly string ToString()
		{
			return ((Guid)this/*cast due to .constrained prefix*/).ToString();
		}

		public readonly string ToUnityGuidString()
		{
			return ToString("N");
		}

		public readonly string ToString(string format)
		{
			return ((Guid)this).ToString(format);
		}

		public readonly int CompareTo(NetworkPrefabRef other)
		{
			long num = _data0 - other._data0;
			if (num == 0)
			{
				num = _data1 - other._data1;
				if (num == 0)
				{
					return 0;
				}
			}
			if (num < 0)
			{
				return -1;
			}
			return 1;
		}

		public unsafe static explicit operator NetworkObjectGuid(NetworkPrefabRef t)
		{
			return new NetworkObjectGuid((byte*)(&t));
		}
	}
}
