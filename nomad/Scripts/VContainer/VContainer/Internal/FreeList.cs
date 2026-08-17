using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace VContainer.Internal
{
	internal class FreeList<T> where T : class
	{
		private readonly object gate = new object();

		private T[] values;

		private int lastIndex = -1;

		public bool IsDisposed => lastIndex == -2;

		public int Length => lastIndex + 1;

		public T this[int index] => values[index];

		public FreeList(int initialCapacity)
		{
			values = new T[initialCapacity];
		}

		public ReadOnlySpan<T> AsSpan()
		{
			if (lastIndex < 0)
			{
				return ReadOnlySpan<T>.Empty;
			}
			return values.AsSpan(0, lastIndex + 1);
		}

		public void Add(T item)
		{
			lock (gate)
			{
				CheckDispose();
				int num = FindNullIndex(values);
				if (num == -1)
				{
					int num2 = values.Length;
					T[] destinationArray = new T[num2 + num2 / 2];
					Array.Copy(values, destinationArray, num2);
					values = destinationArray;
					num = num2;
				}
				values[num] = item;
				if (lastIndex < num)
				{
					lastIndex = num;
				}
			}
		}

		public void RemoveAt(int index)
		{
			lock (gate)
			{
				if (index < values.Length)
				{
					ref T reference = ref values[index];
					if (reference == null)
					{
						throw new KeyNotFoundException($"key index {index} is not found.");
					}
					reference = null;
					if (index == lastIndex)
					{
						lastIndex = FindLastNonNullIndex(values, index);
					}
				}
			}
		}

		public bool Remove(T value)
		{
			lock (gate)
			{
				if (lastIndex < 0)
				{
					return false;
				}
				int num = -1;
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i] == value)
					{
						num = i;
						break;
					}
				}
				if (num != -1)
				{
					RemoveAt(num);
					return true;
				}
			}
			return false;
		}

		public void Clear()
		{
			lock (gate)
			{
				if (lastIndex > 0)
				{
					Array.Clear(values, 0, lastIndex + 1);
					lastIndex = -1;
				}
			}
		}

		public void Dispose()
		{
			lock (gate)
			{
				lastIndex = -2;
			}
		}

		private void CheckDispose()
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
		}

		private unsafe static int FindNullIndex(T[] target)
		{
			fixed (IntPtr* ptr = &UnsafeUtility.As<T, IntPtr>(ref MemoryMarshal.GetReference(target.AsSpan())))
			{
				void* pointer = ptr;
				return new ReadOnlySpan<IntPtr>(pointer, target.Length).IndexOf(IntPtr.Zero);
			}
		}

		private unsafe static int FindLastNonNullIndex(T[] target, int lastIndex)
		{
			fixed (IntPtr* ptr = &UnsafeUtility.As<T, IntPtr>(ref MemoryMarshal.GetReference(target.AsSpan())))
			{
				void* pointer = ptr;
				ReadOnlySpan<IntPtr> readOnlySpan = new ReadOnlySpan<IntPtr>(pointer, lastIndex);
				for (int num = readOnlySpan.Length - 1; num >= 0; num--)
				{
					if (readOnlySpan[num] != IntPtr.Zero)
					{
						return num;
					}
				}
				return -1;
			}
		}
	}
}
