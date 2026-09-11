using System;
using System.Runtime.CompilerServices;

namespace Zorro.Core
{
	public static class EnumFlagExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static bool HasFlagUnsafe<TEnum>(this TEnum lhs, TEnum rhs) where TEnum : unmanaged, Enum
		{
			return sizeof(TEnum) switch
			{
				1 => (*(byte*)(&lhs) & *(byte*)(&rhs)) > 0, 
				2 => (*(ushort*)(&lhs) & *(ushort*)(&rhs)) > 0, 
				4 => (*(uint*)(&lhs) & *(uint*)(&rhs)) != 0, 
				8 => (*(long*)(&lhs) & *(long*)(&rhs)) != 0, 
				_ => throw new Exception("Size does not match a known Enum backing type."), 
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static TEnum AddFlag<TEnum>(this TEnum lhs, TEnum rhs) where TEnum : unmanaged, Enum
		{
			switch (sizeof(TEnum))
			{
			case 1:
			{
				int num4 = *(byte*)(&lhs) | *(byte*)(&rhs);
				return *(TEnum*)(&num4);
			}
			case 2:
			{
				int num3 = *(ushort*)(&lhs) | *(ushort*)(&rhs);
				return *(TEnum*)(&num3);
			}
			case 4:
			{
				uint num2 = *(uint*)(&lhs) | *(uint*)(&rhs);
				return *(TEnum*)(&num2);
			}
			case 8:
			{
				ulong num = (ulong)(*(long*)(&lhs) | *(long*)(&rhs));
				return *(TEnum*)(&num);
			}
			default:
				throw new Exception("Size does not match a known Enum backing type.");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static TEnum RemoveFlag<TEnum>(this TEnum lhs, TEnum rhs) where TEnum : unmanaged, Enum
		{
			switch (sizeof(TEnum))
			{
			case 1:
			{
				int num4 = *(byte*)(&lhs) & ~(*(byte*)(&rhs));
				return *(TEnum*)(&num4);
			}
			case 2:
			{
				int num3 = *(ushort*)(&lhs) & ~(*(ushort*)(&rhs));
				return *(TEnum*)(&num3);
			}
			case 4:
			{
				uint num2 = *(uint*)(&lhs) & ~(*(uint*)(&rhs));
				return *(TEnum*)(&num2);
			}
			case 8:
			{
				ulong num = (ulong)(*(long*)(&lhs) & ~(*(long*)(&rhs)));
				return *(TEnum*)(&num);
			}
			default:
				throw new Exception("Size does not match a known Enum backing type.");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void SetFlag<TEnum>(this ref TEnum lhs, TEnum rhs) where TEnum : unmanaged, Enum
		{
			fixed (TEnum* ptr = &lhs)
			{
				switch (sizeof(TEnum))
				{
				case 1:
				{
					int num4 = *(byte*)ptr | *(byte*)(&rhs);
					*ptr = *(TEnum*)(&num4);
					break;
				}
				case 2:
				{
					int num3 = *(ushort*)ptr | *(ushort*)(&rhs);
					*ptr = *(TEnum*)(&num3);
					break;
				}
				case 4:
				{
					uint num2 = *(uint*)ptr | *(uint*)(&rhs);
					*ptr = *(TEnum*)(&num2);
					break;
				}
				case 8:
				{
					ulong num = (ulong)(*(long*)ptr | *(long*)(&rhs));
					*ptr = *(TEnum*)(&num);
					break;
				}
				default:
					throw new Exception("Size does not match a known Enum backing type.");
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void ClearFlag<TEnum>(this ref TEnum lhs, TEnum rhs) where TEnum : unmanaged, Enum
		{
			fixed (TEnum* ptr = &lhs)
			{
				switch (sizeof(TEnum))
				{
				case 1:
				{
					int num4 = *(byte*)ptr & ~(*(byte*)(&rhs));
					*ptr = *(TEnum*)(&num4);
					break;
				}
				case 2:
				{
					int num3 = *(ushort*)ptr & ~(*(ushort*)(&rhs));
					*ptr = *(TEnum*)(&num3);
					break;
				}
				case 4:
				{
					uint num2 = *(uint*)ptr & ~(*(uint*)(&rhs));
					*ptr = *(TEnum*)(&num2);
					break;
				}
				case 8:
				{
					ulong num = (ulong)(*(long*)ptr & ~(*(long*)(&rhs)));
					*ptr = *(TEnum*)(&num);
					break;
				}
				default:
					throw new Exception("Size does not match a known Enum backing type.");
				}
			}
		}
	}
}
