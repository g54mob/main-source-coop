#define DEBUG
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Fusion
{
	public static class FusionUnsafe
	{
		private static class MemoryTracking
		{
			public static FusionUnsafeTrackingMode Mode;

			public static ConcurrentDictionary<nint, FusionUnsafeAllocInfo> Entries;
		}

		private const int MaxAllocSize = 1073741824;

		public const int DefaultAlignment = 8;

		public unsafe static int AllocAndClearBlock(int size0, int size1, out void* ptr0, out void* ptr1, int align = 8)
		{
			size0 = MakeAligned(size0, align);
			size1 = MakeAligned(size1, align);
			int num = size0 + size1;
			byte* ptr2 = (byte*)(ptr0 = AllocAndClear(num, 8, "Fusion\\Fusion.Common\\FusionUnsafe.Common.cs", 382)) + size0;
			ptr1 = ptr2;
			Assert.Check(IsAligned(ptr0, align), "IsAligned(ptr0, align)");
			Assert.Check(IsAligned(ptr1, align), "IsAligned(ptr1, align)");
			return num;
		}

		public unsafe static int AllocAndClearBlock(int size0, int size1, int size2, out void* ptr0, out void* ptr1, out void* ptr2, int align = 8)
		{
			size0 = MakeAligned(size0, align);
			size1 = MakeAligned(size1, align);
			size2 = MakeAligned(size2, align);
			int num = size0 + size1 + size2;
			byte* ptr3 = (byte*)(ptr1 = (byte*)(ptr0 = AllocAndClear(num, 8, "Fusion\\Fusion.Common\\FusionUnsafe.Common.cs", 406)) + size0) + size1;
			ptr2 = ptr3;
			Assert.Check(IsAligned(ptr0, align), "IsAligned(ptr0, align)");
			Assert.Check(IsAligned(ptr1, align), "IsAligned(ptr1, align)");
			Assert.Check(IsAligned(ptr2, align), "IsAligned(ptr2, align)");
			return num;
		}

		public unsafe static int AllocAndClearBlock(int size0, int size1, int size2, int size3, out void* ptr0, out void* ptr1, out void* ptr2, out void* ptr3, int align = 8)
		{
			size0 = MakeAligned(size0, align);
			size1 = MakeAligned(size1, align);
			size2 = MakeAligned(size2, align);
			size3 = MakeAligned(size3, align);
			int num = size0 + size1 + size2 + size3;
			byte* ptr4 = (byte*)(ptr2 = (byte*)(ptr1 = (byte*)(ptr0 = AllocAndClear(num, 8, "Fusion\\Fusion.Common\\FusionUnsafe.Common.cs", 434)) + size0) + size1) + size2;
			ptr3 = ptr4;
			Assert.Check(IsAligned(ptr0, align), "IsAligned(ptr0, align)");
			Assert.Check(IsAligned(ptr1, align), "IsAligned(ptr1, align)");
			Assert.Check(IsAligned(ptr2, align), "IsAligned(ptr2, align)");
			Assert.Check(IsAligned(ptr3, align), "IsAligned(ptr3, align)");
			return num;
		}

		public unsafe static int AllocAndClearBlock(int size0, int size1, int size2, int size3, int size4, out void* ptr0, out void* ptr1, out void* ptr2, out void* ptr3, out void* ptr4, int align = 8)
		{
			size0 = MakeAligned(size0, align);
			size1 = MakeAligned(size1, align);
			size2 = MakeAligned(size2, align);
			size3 = MakeAligned(size3, align);
			size4 = MakeAligned(size4, align);
			int num = size0 + size1 + size2 + size3 + size4;
			byte* ptr5 = (byte*)(ptr3 = (byte*)(ptr2 = (byte*)(ptr1 = (byte*)(ptr0 = AllocAndClear(num, 8, "Fusion\\Fusion.Common\\FusionUnsafe.Common.cs", 466)) + size0) + size1) + size2) + size3;
			ptr4 = ptr5;
			Assert.Check(IsAligned(ptr0, align), "IsAligned(ptr0, align)");
			Assert.Check(IsAligned(ptr1, align), "IsAligned(ptr1, align)");
			Assert.Check(IsAligned(ptr2, align), "IsAligned(ptr2, align)");
			Assert.Check(IsAligned(ptr3, align), "IsAligned(ptr3, align)");
			Assert.Check(IsAligned(ptr4, align), "IsAligned(ptr4, align)");
			return num;
		}

		public unsafe static int AllocAndClearBlock(int size0, int size1, int size2, int size3, int size4, int size5, out void* ptr0, out void* ptr1, out void* ptr2, out void* ptr3, out void* ptr4, out void* ptr5, int align = 8)
		{
			size0 = MakeAligned(size0, align);
			size1 = MakeAligned(size1, align);
			size2 = MakeAligned(size2, align);
			size3 = MakeAligned(size3, align);
			size4 = MakeAligned(size4, align);
			size5 = MakeAligned(size5, align);
			int num = size0 + size1 + size2 + size3 + size4 + size5;
			byte* ptr6 = (byte*)(ptr4 = (byte*)(ptr3 = (byte*)(ptr2 = (byte*)(ptr1 = (byte*)(ptr0 = AllocAndClear(num, 8, "Fusion\\Fusion.Common\\FusionUnsafe.Common.cs", 502)) + size0) + size1) + size2) + size3) + size4;
			ptr5 = ptr6;
			Assert.Check(IsAligned(ptr0, align), "IsAligned(ptr0, align)");
			Assert.Check(IsAligned(ptr1, align), "IsAligned(ptr1, align)");
			Assert.Check(IsAligned(ptr2, align), "IsAligned(ptr2, align)");
			Assert.Check(IsAligned(ptr3, align), "IsAligned(ptr3, align)");
			Assert.Check(IsAligned(ptr4, align), "IsAligned(ptr4, align)");
			Assert.Check(IsAligned(ptr5, align), "IsAligned(ptr5, align)");
			return num;
		}

		public unsafe static int AllocAndClearBlock(int size0, int size1, int size2, int size3, int size4, int size5, int size6, out void* ptr0, out void* ptr1, out void* ptr2, out void* ptr3, out void* ptr4, out void* ptr5, out void* ptr6, int align = 8)
		{
			size0 = MakeAligned(size0, align);
			size1 = MakeAligned(size1, align);
			size2 = MakeAligned(size2, align);
			size3 = MakeAligned(size3, align);
			size4 = MakeAligned(size4, align);
			size5 = MakeAligned(size5, align);
			size6 = MakeAligned(size6, align);
			int num = size0 + size1 + size2 + size3 + size4 + size5 + size6;
			byte* ptr7 = (byte*)(ptr5 = (byte*)(ptr4 = (byte*)(ptr3 = (byte*)(ptr2 = (byte*)(ptr1 = (byte*)(ptr0 = AllocAndClear(num, 8, "Fusion\\Fusion.Common\\FusionUnsafe.Common.cs", 542)) + size0) + size1) + size2) + size3) + size4) + size5;
			ptr6 = ptr7;
			Assert.Check(IsAligned(ptr0, align), "IsAligned(ptr0, align)");
			Assert.Check(IsAligned(ptr1, align), "IsAligned(ptr1, align)");
			Assert.Check(IsAligned(ptr2, align), "IsAligned(ptr2, align)");
			Assert.Check(IsAligned(ptr3, align), "IsAligned(ptr3, align)");
			Assert.Check(IsAligned(ptr4, align), "IsAligned(ptr4, align)");
			Assert.Check(IsAligned(ptr5, align), "IsAligned(ptr5, align)");
			Assert.Check(IsAligned(ptr6, align), "IsAligned(ptr6, align)");
			return num;
		}

		public unsafe static int AllocAndClearBlock(int size0, int size1, int size2, int size3, int size4, int size5, int size6, int size7, out void* ptr0, out void* ptr1, out void* ptr2, out void* ptr3, out void* ptr4, out void* ptr5, out void* ptr6, out void* ptr7, int align = 8)
		{
			size0 = MakeAligned(size0, align);
			size1 = MakeAligned(size1, align);
			size2 = MakeAligned(size2, align);
			size3 = MakeAligned(size3, align);
			size4 = MakeAligned(size4, align);
			size5 = MakeAligned(size5, align);
			size6 = MakeAligned(size6, align);
			size7 = MakeAligned(size7, align);
			int num = size0 + size1 + size2 + size3 + size4 + size5 + size6 + size7;
			byte* ptr8 = (byte*)(ptr6 = (byte*)(ptr5 = (byte*)(ptr4 = (byte*)(ptr3 = (byte*)(ptr2 = (byte*)(ptr1 = (byte*)(ptr0 = AllocAndClear(num, 8, "Fusion\\Fusion.Common\\FusionUnsafe.Common.cs", 586)) + size0) + size1) + size2) + size3) + size4) + size5) + size6;
			ptr7 = ptr8;
			Assert.Check(IsAligned(ptr0, align), "IsAligned(ptr0, align)");
			Assert.Check(IsAligned(ptr1, align), "IsAligned(ptr1, align)");
			Assert.Check(IsAligned(ptr2, align), "IsAligned(ptr2, align)");
			Assert.Check(IsAligned(ptr3, align), "IsAligned(ptr3, align)");
			Assert.Check(IsAligned(ptr4, align), "IsAligned(ptr4, align)");
			Assert.Check(IsAligned(ptr5, align), "IsAligned(ptr5, align)");
			Assert.Check(IsAligned(ptr6, align), "IsAligned(ptr6, align)");
			Assert.Check(IsAligned(ptr7, align), "IsAligned(ptr7, align)");
			return num;
		}

		public unsafe static int AllocAndClearBlock(int size0, int size1, int size2, int size3, int size4, int size5, int size6, int size7, int size8, out void* ptr0, out void* ptr1, out void* ptr2, out void* ptr3, out void* ptr4, out void* ptr5, out void* ptr6, out void* ptr7, out void* ptr8, int align = 8)
		{
			size0 = MakeAligned(size0, align);
			size1 = MakeAligned(size1, align);
			size2 = MakeAligned(size2, align);
			size3 = MakeAligned(size3, align);
			size4 = MakeAligned(size4, align);
			size5 = MakeAligned(size5, align);
			size6 = MakeAligned(size6, align);
			size7 = MakeAligned(size7, align);
			size8 = MakeAligned(size8, align);
			int num = size0 + size1 + size2 + size3 + size4 + size5 + size6 + size7 + size8;
			byte* ptr9 = (byte*)(ptr7 = (byte*)(ptr6 = (byte*)(ptr5 = (byte*)(ptr4 = (byte*)(ptr3 = (byte*)(ptr2 = (byte*)(ptr1 = (byte*)(ptr0 = AllocAndClear(num, 8, "Fusion\\Fusion.Common\\FusionUnsafe.Common.cs", 634)) + size0) + size1) + size2) + size3) + size4) + size5) + size6) + size7;
			ptr8 = ptr9;
			Assert.Check(IsAligned(ptr0, align), "IsAligned(ptr0, align)");
			Assert.Check(IsAligned(ptr1, align), "IsAligned(ptr1, align)");
			Assert.Check(IsAligned(ptr2, align), "IsAligned(ptr2, align)");
			Assert.Check(IsAligned(ptr3, align), "IsAligned(ptr3, align)");
			Assert.Check(IsAligned(ptr4, align), "IsAligned(ptr4, align)");
			Assert.Check(IsAligned(ptr5, align), "IsAligned(ptr5, align)");
			Assert.Check(IsAligned(ptr6, align), "IsAligned(ptr6, align)");
			Assert.Check(IsAligned(ptr7, align), "IsAligned(ptr7, align)");
			Assert.Check(IsAligned(ptr8, align), "IsAligned(ptr8, align)");
			return num;
		}

		public unsafe static int AllocAndClearBlock(int size0, int size1, int size2, int size3, int size4, int size5, int size6, int size7, int size8, int size9, out void* ptr0, out void* ptr1, out void* ptr2, out void* ptr3, out void* ptr4, out void* ptr5, out void* ptr6, out void* ptr7, out void* ptr8, out void* ptr9, int align = 8)
		{
			size0 = MakeAligned(size0, align);
			size1 = MakeAligned(size1, align);
			size2 = MakeAligned(size2, align);
			size3 = MakeAligned(size3, align);
			size4 = MakeAligned(size4, align);
			size5 = MakeAligned(size5, align);
			size6 = MakeAligned(size6, align);
			size7 = MakeAligned(size7, align);
			size8 = MakeAligned(size8, align);
			size9 = MakeAligned(size9, align);
			int num = size0 + size1 + size2 + size3 + size4 + size5 + size6 + size7 + size8 + size9;
			byte* ptr10 = (byte*)(ptr8 = (byte*)(ptr7 = (byte*)(ptr6 = (byte*)(ptr5 = (byte*)(ptr4 = (byte*)(ptr3 = (byte*)(ptr2 = (byte*)(ptr1 = (byte*)(ptr0 = AllocAndClear(num, 8, "Fusion\\Fusion.Common\\FusionUnsafe.Common.cs", 686)) + size0) + size1) + size2) + size3) + size4) + size5) + size6) + size7) + size8;
			ptr9 = ptr10;
			Assert.Check(IsAligned(ptr0, align), "IsAligned(ptr0, align)");
			Assert.Check(IsAligned(ptr1, align), "IsAligned(ptr1, align)");
			Assert.Check(IsAligned(ptr2, align), "IsAligned(ptr2, align)");
			Assert.Check(IsAligned(ptr3, align), "IsAligned(ptr3, align)");
			Assert.Check(IsAligned(ptr4, align), "IsAligned(ptr4, align)");
			Assert.Check(IsAligned(ptr5, align), "IsAligned(ptr5, align)");
			Assert.Check(IsAligned(ptr6, align), "IsAligned(ptr6, align)");
			Assert.Check(IsAligned(ptr7, align), "IsAligned(ptr7, align)");
			Assert.Check(IsAligned(ptr8, align), "IsAligned(ptr8, align)");
			Assert.Check(IsAligned(ptr9, align), "IsAligned(ptr9, align)");
			return num;
		}

		public unsafe static int AllocAndClearBlock(int size0, int size1, int size2, int size3, int size4, int size5, int size6, int size7, int size8, int size9, int size10, out void* ptr0, out void* ptr1, out void* ptr2, out void* ptr3, out void* ptr4, out void* ptr5, out void* ptr6, out void* ptr7, out void* ptr8, out void* ptr9, out void* ptr10, int align = 8)
		{
			size0 = MakeAligned(size0, align);
			size1 = MakeAligned(size1, align);
			size2 = MakeAligned(size2, align);
			size3 = MakeAligned(size3, align);
			size4 = MakeAligned(size4, align);
			size5 = MakeAligned(size5, align);
			size6 = MakeAligned(size6, align);
			size7 = MakeAligned(size7, align);
			size8 = MakeAligned(size8, align);
			size9 = MakeAligned(size9, align);
			size10 = MakeAligned(size10, align);
			int num = size0 + size1 + size2 + size3 + size4 + size5 + size6 + size7 + size8 + size9 + size10;
			byte* ptr11 = (byte*)(ptr9 = (byte*)(ptr8 = (byte*)(ptr7 = (byte*)(ptr6 = (byte*)(ptr5 = (byte*)(ptr4 = (byte*)(ptr3 = (byte*)(ptr2 = (byte*)(ptr1 = (byte*)(ptr0 = AllocAndClear(num, 8, "Fusion\\Fusion.Common\\FusionUnsafe.Common.cs", 742)) + size0) + size1) + size2) + size3) + size4) + size5) + size6) + size7) + size8) + size9;
			ptr10 = ptr11;
			Assert.Check(IsAligned(ptr0, align), "IsAligned(ptr0, align)");
			Assert.Check(IsAligned(ptr1, align), "IsAligned(ptr1, align)");
			Assert.Check(IsAligned(ptr2, align), "IsAligned(ptr2, align)");
			Assert.Check(IsAligned(ptr3, align), "IsAligned(ptr3, align)");
			Assert.Check(IsAligned(ptr4, align), "IsAligned(ptr4, align)");
			Assert.Check(IsAligned(ptr5, align), "IsAligned(ptr5, align)");
			Assert.Check(IsAligned(ptr6, align), "IsAligned(ptr6, align)");
			Assert.Check(IsAligned(ptr7, align), "IsAligned(ptr7, align)");
			Assert.Check(IsAligned(ptr8, align), "IsAligned(ptr8, align)");
			Assert.Check(IsAligned(ptr9, align), "IsAligned(ptr9, align)");
			Assert.Check(IsAligned(ptr10, align), "IsAligned(ptr10, align)");
			return num;
		}

		public unsafe static int AllocAndClearBlock(int size0, int size1, int size2, int size3, int size4, int size5, int size6, int size7, int size8, int size9, int size10, int size11, out void* ptr0, out void* ptr1, out void* ptr2, out void* ptr3, out void* ptr4, out void* ptr5, out void* ptr6, out void* ptr7, out void* ptr8, out void* ptr9, out void* ptr10, out void* ptr11, int align = 8)
		{
			size0 = MakeAligned(size0, align);
			size1 = MakeAligned(size1, align);
			size2 = MakeAligned(size2, align);
			size3 = MakeAligned(size3, align);
			size4 = MakeAligned(size4, align);
			size5 = MakeAligned(size5, align);
			size6 = MakeAligned(size6, align);
			size7 = MakeAligned(size7, align);
			size8 = MakeAligned(size8, align);
			size9 = MakeAligned(size9, align);
			size10 = MakeAligned(size10, align);
			size11 = MakeAligned(size11, align);
			int num = size0 + size1 + size2 + size3 + size4 + size5 + size6 + size7 + size8 + size9 + size10 + size11;
			byte* ptr12 = (byte*)(ptr10 = (byte*)(ptr9 = (byte*)(ptr8 = (byte*)(ptr7 = (byte*)(ptr6 = (byte*)(ptr5 = (byte*)(ptr4 = (byte*)(ptr3 = (byte*)(ptr2 = (byte*)(ptr1 = (byte*)(ptr0 = AllocAndClear(num, 8, "Fusion\\Fusion.Common\\FusionUnsafe.Common.cs", 802)) + size0) + size1) + size2) + size3) + size4) + size5) + size6) + size7) + size8) + size9) + size10;
			ptr11 = ptr12;
			Assert.Check(IsAligned(ptr0, align), "IsAligned(ptr0, align)");
			Assert.Check(IsAligned(ptr1, align), "IsAligned(ptr1, align)");
			Assert.Check(IsAligned(ptr2, align), "IsAligned(ptr2, align)");
			Assert.Check(IsAligned(ptr3, align), "IsAligned(ptr3, align)");
			Assert.Check(IsAligned(ptr4, align), "IsAligned(ptr4, align)");
			Assert.Check(IsAligned(ptr5, align), "IsAligned(ptr5, align)");
			Assert.Check(IsAligned(ptr6, align), "IsAligned(ptr6, align)");
			Assert.Check(IsAligned(ptr7, align), "IsAligned(ptr7, align)");
			Assert.Check(IsAligned(ptr8, align), "IsAligned(ptr8, align)");
			Assert.Check(IsAligned(ptr9, align), "IsAligned(ptr9, align)");
			Assert.Check(IsAligned(ptr10, align), "IsAligned(ptr10, align)");
			Assert.Check(IsAligned(ptr11, align), "IsAligned(ptr11, align)");
			return num;
		}

		public static void SetMemoryTrackingMode(FusionUnsafeTrackingMode mode)
		{
			if (mode == FusionUnsafeTrackingMode.None)
			{
				MemoryTracking.Entries = null;
			}
			else
			{
				if (MemoryTracking.Entries == null)
				{
					MemoryTracking.Entries = new ConcurrentDictionary<IntPtr, FusionUnsafeAllocInfo>();
				}
				MemoryTracking.Entries.Clear();
			}
			MemoryTracking.Mode = mode;
		}

		public static FusionUnsafeAllocResult GetMemoryTrackingResult()
		{
			return new FusionUnsafeAllocResult(MemoryTracking.Entries.Values.ToArray());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Set(nint ptr, byte value, nuint count)
		{
			Set((void*)ptr, value, (int)count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Copy(nint destination, nint source, nuint size)
		{
			Copy((void*)destination, (void*)source, (int)size);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Move(nint destination, nint source, nuint size)
		{
			Move((void*)destination, (void*)source, (int)size);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Compare(nint ptr1, nint ptr2, nuint size)
		{
			return Compare((void*)ptr1, (void*)ptr2, (int)size);
		}

		public unsafe static void Swap(void* a, void* b, int count)
		{
			byte* ptr = stackalloc byte[(int)(uint)count];
			Unsafe.CopyBlock(ptr, a, (uint)count);
			Unsafe.CopyBlock(a, b, (uint)count);
			Unsafe.CopyBlock(b, ptr, (uint)count);
		}

		public unsafe static void Swap<T>(T* a, T* b) where T : unmanaged
		{
			ref T reference = ref *a;
			T val = *b;
			T val2 = *a;
			reference = val;
			*b = val2;
		}

		public static void Swap<T>(ref T a, ref T b) where T : unmanaged
		{
			T val = b;
			T val2 = a;
			a = val;
			b = val2;
		}

		public unsafe static T Replace<T>(T* a, T value) where T : unmanaged
		{
			T result = *a;
			*a = value;
			return result;
		}

		public static T Replace<T>(ref T a, T value) where T : unmanaged
		{
			T result = a;
			a = value;
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void* Alloc(int size, int align = 8, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
		{
			ThrowIfSizeInvalid(size);
			void* ptr = AllocInternal(size, align);
			ConcurrentDictionary<nint, FusionUnsafeAllocInfo> entries = MemoryTracking.Entries;
			if (entries != null && !entries.TryAdd((nint)ptr, new FusionUnsafeAllocInfo((nint)ptr, size, align, callerFilePath, callerLineNumber, (MemoryTracking.Mode == FusionUnsafeTrackingMode.StackTrace) ? Environment.StackTrace : null)))
			{
				Assert.AlwaysFail($"Pointer {(nint)ptr} already tracked!");
			}
			return ptr;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static T* Alloc<T>(int align = 8, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0) where T : unmanaged
		{
			void* ptr = AllocInternal(sizeof(T), align);
			ConcurrentDictionary<nint, FusionUnsafeAllocInfo> entries = MemoryTracking.Entries;
			if (entries != null && !entries.TryAdd((nint)ptr, new FusionUnsafeAllocInfo((nint)ptr, sizeof(T), align, callerFilePath, callerLineNumber, (MemoryTracking.Mode == FusionUnsafeTrackingMode.StackTrace) ? Environment.StackTrace : null)))
			{
				Assert.AlwaysFail($"Pointer {(nint)ptr} already tracked!");
			}
			return (T*)ptr;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void* AllocAndClear(int size, int align = 8, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
		{
			ThrowIfSizeInvalid(size);
			void* ptr = AllocInternal(size, align);
			ConcurrentDictionary<nint, FusionUnsafeAllocInfo> entries = MemoryTracking.Entries;
			if (entries != null && !entries.TryAdd((nint)ptr, new FusionUnsafeAllocInfo((nint)ptr, size, align, callerFilePath, callerLineNumber, (MemoryTracking.Mode == FusionUnsafeTrackingMode.StackTrace) ? Environment.StackTrace : null)))
			{
				Assert.AlwaysFail($"Pointer {(nint)ptr} already tracked!");
			}
			Clear(ptr, size);
			return ptr;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static T* AllocAndClear<T>(int align = 8, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0) where T : unmanaged
		{
			void* ptr = AllocInternal(sizeof(T), align);
			ConcurrentDictionary<nint, FusionUnsafeAllocInfo> entries = MemoryTracking.Entries;
			if (entries != null && !entries.TryAdd((nint)ptr, new FusionUnsafeAllocInfo((nint)ptr, sizeof(T), align, callerFilePath, callerLineNumber, (MemoryTracking.Mode == FusionUnsafeTrackingMode.StackTrace) ? Environment.StackTrace : null)))
			{
				Assert.AlwaysFail($"Pointer {(nint)ptr} already tracked!");
			}
			Clear(ptr, sizeof(T));
			return (T*)ptr;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static T* AllocAndClearArray<T>(int count, int align = 8, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0) where T : unmanaged
		{
			int size = count * sizeof(T);
			void* ptr = AllocInternal(size, align);
			ConcurrentDictionary<nint, FusionUnsafeAllocInfo> entries = MemoryTracking.Entries;
			if (entries != null && !entries.TryAdd((nint)ptr, new FusionUnsafeAllocInfo((nint)ptr, size, align, callerFilePath, callerLineNumber, (MemoryTracking.Mode == FusionUnsafeTrackingMode.StackTrace) ? Environment.StackTrace : null)))
			{
				Assert.AlwaysFail($"Pointer {(nint)ptr} already tracked!");
			}
			Clear(ptr, size);
			return (T*)ptr;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void* AllocAndClearArray(int count, int elementSize, int align = 8, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
		{
			int size = count * elementSize;
			void* ptr = AllocInternal(size, align);
			ConcurrentDictionary<nint, FusionUnsafeAllocInfo> entries = MemoryTracking.Entries;
			if (entries != null && !entries.TryAdd((nint)ptr, new FusionUnsafeAllocInfo((nint)ptr, size, align, callerFilePath, callerLineNumber, (MemoryTracking.Mode == FusionUnsafeTrackingMode.StackTrace) ? Environment.StackTrace : null)))
			{
				Assert.AlwaysFail($"Pointer {(nint)ptr} already tracked!");
			}
			Clear(ptr, size);
			return ptr;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static T** AllocAndClearPtrArray<T>(int capacity, int align = 8, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0) where T : unmanaged
		{
			return (T**)AllocAndClearArray(capacity, sizeof(T*), align, callerFilePath, callerLineNumber);
		}

		public unsafe static void Copy<T>(T* destination, ReadOnlySpan<T> source) where T : unmanaged
		{
			Copy(new Span<T>(destination, source.Length), source);
		}

		public unsafe static void Copy<T>(Span<T> destination, T* source) where T : unmanaged
		{
			Copy(destination, new Span<T>(source, destination.Length));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Free(void* ptr)
		{
			if (ptr != null)
			{
				ConcurrentDictionary<nint, FusionUnsafeAllocInfo> entries = MemoryTracking.Entries;
				if (entries != null && !entries.TryRemove((nint)ptr, out var _))
				{
					Assert.AlwaysFail($"Pointer {(nint)ptr} not tracked!");
				}
				FreeInternal(ptr);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Free(ref void* ptr)
		{
			if (ptr != null)
			{
				ConcurrentDictionary<nint, FusionUnsafeAllocInfo> entries = MemoryTracking.Entries;
				if (entries != null && !entries.TryRemove((nint)ptr, out var _))
				{
					Assert.AlwaysFail($"Pointer {(nint)ptr} not tracked!");
				}
				void* memory = ptr;
				ptr = null;
				FreeInternal(memory);
			}
		}

		public unsafe static void Free<T>(ref T* ptr) where T : unmanaged
		{
			if (ptr != null)
			{
				ConcurrentDictionary<nint, FusionUnsafeAllocInfo> entries = MemoryTracking.Entries;
				if (entries != null && !entries.TryRemove((nint)ptr, out var _))
				{
					Assert.AlwaysFail($"Pointer {(nint)ptr} not tracked!");
				}
				T* memory = ptr;
				ptr = null;
				FreeInternal(memory);
			}
		}

		public unsafe static void Free<T>(ref T** ptr) where T : unmanaged
		{
			if (ptr != null)
			{
				ConcurrentDictionary<nint, FusionUnsafeAllocInfo> entries = MemoryTracking.Entries;
				if (entries != null && !entries.TryRemove((nint)ptr, out var _))
				{
					Assert.AlwaysFail($"Pointer {(nint)ptr} not tracked!");
				}
				T** memory = ptr;
				ptr = null;
				FreeInternal(memory);
			}
		}

		public unsafe static T* ExpandArray<T>(T* buffer, int currentSize, int newSize, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0) where T : unmanaged
		{
			return (T*)Expand(buffer, currentSize * sizeof(T), newSize * sizeof(T), callerFilePath, callerLineNumber);
		}

		public unsafe static T** ExpandPtrArray<T>(T** buffer, int currentSize, int newSize, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0) where T : unmanaged
		{
			return (T**)Expand(buffer, currentSize * sizeof(T*), newSize * sizeof(T*), callerFilePath, callerLineNumber);
		}

		public unsafe static void* Expand(void* buffer, int currentSize, int newSize, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
		{
			Assert.Always(newSize >= currentSize, "newSize >= currentSize");
			if (newSize == currentSize)
			{
				return buffer;
			}
			void* ptr = AllocAndClear(newSize, 8, callerFilePath, callerLineNumber);
			Copy(ptr, buffer, currentSize);
			Free(buffer);
			return ptr;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static nint MakeAligned(nint value, int align)
		{
			Assert.Always((align & (align - 1)) == 0, "(align & (align - 1)) == 0");
			return (value + align - 1) & ~(align - 1);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int MakeAligned(int value, int align)
		{
			Assert.Always((align & (align - 1)) == 0, "(align & (align - 1)) == 0");
			return (value + align - 1) & ~(align - 1);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int MakeAligned(int value)
		{
			return (value + 8 - 1) & -8;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAligned(nint value, int align = 8)
		{
			return (value & (align - 1)) == 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static bool IsAligned(void* value, int align = 8)
		{
			return IsAligned((nint)value, align);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetWordCount(int stride, int wordSize)
		{
			return MakeAligned(stride, wordSize) / wordSize;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static object Box<T>(T* ptr) where T : unmanaged
		{
			return Pointer.Box(ptr, typeof(T*));
		}

		internal static int GetArrayElementAlign(int elementSize)
		{
			Assert.Check(elementSize > 0, "elementSize > 0");
			if (elementSize % 8 == 0)
			{
				return 8;
			}
			return 4;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static bool TryFree(void* ptr)
		{
			FusionUnsafeAllocInfo allocInfo;
			return TryFree(ptr, out allocInfo);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static bool TryFree(void* ptr, out FusionUnsafeAllocInfo allocInfo)
		{
			if (ptr == null)
			{
				allocInfo = default(FusionUnsafeAllocInfo);
				return false;
			}
			ConcurrentDictionary<IntPtr, FusionUnsafeAllocInfo> entries = MemoryTracking.Entries;
			if (entries != null)
			{
				if (!entries.TryRemove((nint)ptr, out allocInfo))
				{
					return false;
				}
			}
			else
			{
				allocInfo = default(FusionUnsafeAllocInfo);
			}
			FreeInternal(ptr);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static bool TryGetAllocInfo(void* ptr, out FusionUnsafeAllocInfo allocInfo)
		{
			if (ptr == null)
			{
				allocInfo = default(FusionUnsafeAllocInfo);
				return false;
			}
			ConcurrentDictionary<IntPtr, FusionUnsafeAllocInfo> entries = MemoryTracking.Entries;
			if (entries != null)
			{
				return entries.TryGetValue((nint)ptr, out allocInfo);
			}
			allocInfo = default(FusionUnsafeAllocInfo);
			return false;
		}

		internal static (nint, FusionUnsafeAllocInfo)[] GetAllAllocs()
		{
			ConcurrentDictionary<IntPtr, FusionUnsafeAllocInfo> entries = MemoryTracking.Entries;
			if (entries == null || entries.Count == 0)
			{
				return Array.Empty<(IntPtr, FusionUnsafeAllocInfo)>();
			}
			List<(IntPtr, FusionUnsafeAllocInfo)> list = new List<(IntPtr, FusionUnsafeAllocInfo)>();
			foreach (var (item, item2) in entries)
			{
				list.Add((item, item2));
			}
			return list.ToArray();
		}

		private static void ThrowIfSizeInvalid(int size)
		{
			if (size <= 0)
			{
				throw new ArgumentOutOfRangeException("size", $"Trying to allocate < 0 bytes: {size}");
			}
			if (size > 1073741824)
			{
				throw new ArgumentOutOfRangeException("size", $"Trying to allocate very large block: {size} bytes");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool IsUnderStackAllocThreshold<T>(int count)
		{
			return count <= 1 + 511 / Unsafe.SizeOf<T>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Move([NotNull] void* destination, [NotNull] void* source, int size)
		{
			if (size != 0)
			{
				Assert.Always(destination != null && source != null && size >= 0 && size < 1073741824, (nint)destination, (nint)source, size);
				UnsafeUtility.MemMove(destination, source, size);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Copy([NotNull] void* destination, [NotNull] void* source, int size)
		{
			Assert.Check(size == 0 || (destination != null && source != null && size > 0 && size < 1073741824), (nint)destination, (nint)source, size);
			Unsafe.CopyBlockUnaligned(destination, source, (uint)size);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Clear([NotNull] void* ptr, int size)
		{
			if (size != 0)
			{
				Assert.Always(ptr != null && size >= 0 && size < 1073741824, (nint)ptr, size);
				UnsafeUtility.MemClear(ptr, size);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Set([NotNull] void* ptr, byte value, int size)
		{
			if (size != 0)
			{
				Assert.Always(ptr != null && size >= 0 && size < 1073741824, (nint)ptr, size);
				UnsafeUtility.MemSet(ptr, value, size);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int Compare([NotNull] void* ptr1, [NotNull] void* ptr2, int size)
		{
			if (size == 0)
			{
				return 0;
			}
			Assert.Always(ptr1 != null && ptr2 != null && size >= 0 && size < 1073741824, (nint)ptr1, (nint)ptr2, size);
			return UnsafeUtility.MemCmp(ptr1, ptr2, size);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Copy<T>(Span<T> destination, ReadOnlySpan<T> source) where T : unmanaged
		{
			Assert.Check(source.Length <= destination.Length, source.Length, destination.Length);
			if (source.Length != 0)
			{
				Unsafe.CopyBlockUnaligned(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(destination)), ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(source)), (uint)(source.Length * sizeof(T)));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void CopyToExact<T>(Span<T> destination, ReadOnlySpan<T> source) where T : unmanaged
		{
			Assert.Check(source.Length == destination.Length, source.Length, destination.Length);
			if (source.Length != 0)
			{
				Unsafe.CopyBlockUnaligned(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(destination)), ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(source)), (uint)(source.Length * sizeof(T)));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Clear<T>(Span<T> span) where T : unmanaged
		{
			span.Clear();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int SizeOf([NotNull] Type t)
		{
			Assert.Always(t != null, "t != null");
			return UnsafeUtility.SizeOf(t);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetFieldOffset([NotNull] FieldInfo fi)
		{
			Assert.Always(fi != null, "fi != null");
			return UnsafeUtility.GetFieldOffset(fi);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static void* AllocInternal(int size, int align)
		{
			return UnsafeUtility.Malloc(size, align, Allocator.Persistent);
		}

		private unsafe static void FreeInternal([NotNull] void* memory)
		{
			UnsafeUtility.Free(memory, Allocator.Persistent);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("This is likely to give code which will break with a moving GC. Prefer keeping ref or pinning.")]
		public unsafe static byte* ReferenceToPointer<T>(ref T obj) where T : unmanaged
		{
			return (byte*)Unsafe.AsPointer(ref obj);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T ReinterpretWords<T>(Span<int> words) where T : unmanaged
		{
			return ref MemoryMarshal.Cast<int, T>(words)[0];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T ReinterpretBytes<T>(Span<byte> bytes) where T : unmanaged
		{
			return ref MemoryMarshal.Cast<byte, T>(bytes)[0];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T ReinterpretWords<T>(Span<int> words, int offset) where T : unmanaged
		{
			return ref MemoryMarshal.Cast<int, T>(words.Slice(offset))[0];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int GetWordCount(int stride)
		{
			return MakeAligned(stride, 4) / 4;
		}
	}
}
