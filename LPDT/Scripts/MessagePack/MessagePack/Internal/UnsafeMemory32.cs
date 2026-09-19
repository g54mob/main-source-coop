using System;
using System.Runtime.CompilerServices;

namespace MessagePack.Internal
{
	public static class UnsafeMemory32
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw4(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw5(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)1u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)1u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw6(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)2u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)2u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw7(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)3u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)3u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw8(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw9(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)5u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)5u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw10(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)6u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)6u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw11(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)7u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)7u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw12(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw13(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)9u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)9u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw14(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)10u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)10u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw15(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)11u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)11u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw16(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw17(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)13u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)13u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw18(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)14u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)14u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw19(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)15u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)15u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw20(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)16u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)16u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw21(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)16u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)16u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)17u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)17u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw22(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)16u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)16u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)18u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)18u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw23(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)16u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)16u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)19u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)19u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw24(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)16u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)16u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)20u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)20u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw25(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)16u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)16u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)20u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)20u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)21u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)21u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw26(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)16u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)16u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)20u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)20u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)22u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)22u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw27(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)16u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)16u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)20u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)20u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)23u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)23u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw28(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)16u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)16u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)20u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)20u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)24u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)24u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw29(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)16u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)16u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)20u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)20u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)24u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)24u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)25u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)25u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw30(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)16u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)16u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)20u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)20u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)24u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)24u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)26u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)26u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw31(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(int*)ptr2 = *(int*)ptr;
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)4u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)4u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)8u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)8u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)12u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)12u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)16u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)16u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)20u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)20u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)24u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)24u);
					*(int*)checked(unchecked((nuint)ptr2) + (nuint)27u) = *(int*)checked(unchecked((nuint)ptr) + (nuint)27u);
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw1(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*ptr2 = *ptr;
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw2(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*(short*)ptr2 = *(short*)ptr;
				}
			}
			writer.Advance(src.Length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteRaw3(ref MessagePackWriter writer, ReadOnlySpan<byte> src)
		{
			Span<byte> span = writer.GetSpan(src.Length);
			fixed (byte* ptr = &src[0])
			{
				fixed (byte* ptr2 = &span[0])
				{
					*ptr2 = *ptr;
					*(short*)checked(unchecked((nuint)ptr2) + (nuint)1u) = *(short*)checked(unchecked((nuint)ptr) + (nuint)1u);
				}
			}
			writer.Advance(src.Length);
		}
	}
}
