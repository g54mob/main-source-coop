using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	internal static class Unsafe
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T Add<T>(ref T source, int elementOffset)
		{
			return ref Unsafe.Add(ref source, elementOffset);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref TTo As<TFrom, TTo>(ref TFrom source)
		{
			return ref Unsafe.As<TFrom, TTo>(ref source);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("This is likely to give code which will break with a moving GC. Prefer keeping ref or pinning.")]
		public unsafe static void* AsPointer<T>(ref T value)
		{
			return Unsafe.AsPointer(ref value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static ref T AsRef<T>(void* source)
		{
			return ref *(T*)source;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T AsRef<T>(in T source)
		{
			return ref source;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static T Read<T>(void* source)
		{
			return Unsafe.Read<T>(source);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static T ReadUnaligned<T>(void* source)
		{
			return Unsafe.ReadUnaligned<T>(source);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T ReadUnaligned<T>(ref byte source)
		{
			return Unsafe.ReadUnaligned<T>(ref source);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Write<T>(void* destination, T value)
		{
			Unsafe.Write(destination, value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteUnaligned<T>(void* destination, T value)
		{
			Unsafe.WriteUnaligned(destination, value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteUnaligned<T>(ref byte destination, T value)
		{
			Unsafe.WriteUnaligned(ref destination, value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SkipInit<T>(out T value)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void InitBlock(void* startAddress, byte value, uint byteCount)
		{
			// IL initblk instruction
			Unsafe.InitBlock(startAddress, value, byteCount);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void InitBlockUnaligned(void* startAddress, byte value, uint byteCount)
		{
			// IL initblk instruction
			Unsafe.InitBlockUnaligned(startAddress, value, byteCount);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void CopyBlock(void* destination, void* source, uint byteCount)
		{
			// IL cpblk instruction
			Unsafe.CopyBlock(destination, source, byteCount);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CopyBlock(ref byte destination, [In][RequiresLocation] ref byte source, uint byteCount)
		{
			// IL cpblk instruction
			Unsafe.CopyBlock(ref destination, ref source, byteCount);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void CopyBlockUnaligned(void* destination, void* source, uint byteCount)
		{
			// IL cpblk instruction
			Unsafe.CopyBlockUnaligned(destination, source, byteCount);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CopyBlockUnaligned(ref byte destination, [In][RequiresLocation] ref byte source, uint byteCount)
		{
			// IL cpblk instruction
			Unsafe.CopyBlockUnaligned(ref destination, ref source, byteCount);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int SizeOf<T>()
		{
			return Unsafe.SizeOf<T>();
		}
	}
}
