#define DEBUG
using System;

namespace Fusion
{
	public struct NetworkInput
	{
		private unsafe uint* _ptr;

		private int _wordCount;

		public readonly int WordCount => _wordCount;

		public unsafe readonly uint* Data => (_ptr == null) ? null : (_ptr + 1);

		public unsafe readonly bool IsValid => _ptr != null;

		internal unsafe readonly uint* Ptr => _ptr;

		internal unsafe int TypeKey
		{
			readonly get
			{
				return (_ptr == null) ? (-1) : ((int)(*_ptr));
			}
			set
			{
				if (_ptr != null)
				{
					*_ptr = (uint)value;
				}
			}
		}

		public readonly Type Type
		{
			get
			{
				int typeKey = TypeKey;
				if (typeKey == -1)
				{
					return null;
				}
				if (!NetworkTypesMeta.Instance.TryGetInputType(typeKey, out var type))
				{
					return null;
				}
				return type;
			}
		}

		internal unsafe static NetworkInput FromRaw(uint* ptr, int wordCount)
		{
			return new NetworkInput
			{
				_ptr = ptr,
				_wordCount = wordCount
			};
		}

		internal unsafe static NetworkInput FromRaw(int* ptr, int wordCount)
		{
			return new NetworkInput
			{
				_ptr = (uint*)ptr,
				_wordCount = wordCount
			};
		}

		public unsafe bool TryGet<T>(out T input) where T : unmanaged, INetworkInput
		{
			Assert.Check(IsValid, "IsValid");
			if (_ptr == null || !NetworkTypesMeta.Instance.TryGetInputTypeKey(typeof(T), out var key) || TypeKey != key)
			{
				input = default(T);
				return false;
			}
			input = *(T*)Data;
			return true;
		}

		public unsafe bool TrySet<T>(T input) where T : unmanaged, INetworkInput
		{
			Assert.Check(IsValid, "IsValid");
			if (_ptr == null || !NetworkTypesMeta.Instance.TryGetInputTypeKey(typeof(T), out var key) || TypeKey != key)
			{
				return false;
			}
			*(T*)Data = input;
			return true;
		}

		public unsafe T Get<T>() where T : unmanaged, INetworkInput
		{
			Assert.Check(IsValid, "IsValid");
			Convert<T>();
			return *(T*)Data;
		}

		public unsafe bool Set<T>(T value) where T : unmanaged, INetworkInput
		{
			Assert.Check(IsValid, "IsValid");
			bool result = Convert<T>();
			*(T*)Data = value;
			return result;
		}

		internal unsafe bool Set(Type type, void* value)
		{
			Assert.Check(IsValid, "IsValid");
			if (!NetworkTypesMeta.Instance.TryGetInputWordCount(type, out var wordCount))
			{
				throw new ArgumentException($"Invalid type: {type}", "type");
			}
			if (wordCount >= _wordCount)
			{
				throw new ArgumentException($"Expected max {_wordCount}, got: {wordCount}", "type");
			}
			bool flag = Convert(type);
			FusionUnsafe.Copy(Data, value, wordCount * 4);
			return true;
		}

		public bool Convert<T>() where T : unmanaged, INetworkInput
		{
			return Convert(typeof(T));
		}

		public unsafe bool Convert(Type type)
		{
			Assert.Check(IsValid, "IsValid");
			if (NetworkTypesMeta.Instance.TryGetInputTypeKey(type, out var key) && key != TypeKey)
			{
				FusionUnsafe.Clear(_ptr, _wordCount * 4);
				TypeKey = key;
				return true;
			}
			return false;
		}

		public bool Is<T>() where T : unmanaged, INetworkInput
		{
			Assert.Check(IsValid, "IsValid");
			int key;
			return NetworkTypesMeta.Instance.TryGetInputTypeKey(typeof(T), out key) && key == TypeKey;
		}
	}
}
