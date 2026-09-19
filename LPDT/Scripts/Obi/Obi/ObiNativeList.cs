using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	public class ObiNativeList<T> : IEnumerable<T>, IEnumerable, IDisposable, ISerializationCallbackReceiver where T : struct
	{
		public T[] serializedContents;

		protected unsafe void* m_AlignedPtr = null;

		protected int m_Stride;

		protected int m_Capacity;

		protected int m_Count;

		[SerializeField]
		protected int m_AlignBytes = 16;

		protected GraphicsBuffer.Target m_ComputeBufferType;

		protected GraphicsBuffer m_ComputeBuffer;

		protected GraphicsBuffer m_CountBuffer;

		protected bool computeBufferDirty;

		protected AsyncGPUReadbackRequest m_AsyncRequest;

		protected AsyncGPUReadbackRequest m_CounterAsyncRequest;

		public int count
		{
			get
			{
				return m_Count;
			}
			set
			{
				if (value != m_Count)
				{
					EnsureCapacity(m_Count);
					m_Count = Mathf.Min(m_Capacity, value);
					if (m_ComputeBuffer != null && m_ComputeBuffer.IsValid() && m_ComputeBufferType == GraphicsBuffer.Target.Counter)
					{
						m_ComputeBuffer.SetCounterValue((uint)m_Count);
					}
				}
			}
		}

		public int capacity => m_Capacity;

		public int stride => m_Stride;

		public unsafe bool isCreated => m_AlignedPtr != null;

		public bool noReadbackInFlight
		{
			get
			{
				if (m_AsyncRequest.done)
				{
					if (m_ComputeBufferType == GraphicsBuffer.Target.Counter)
					{
						return m_CounterAsyncRequest.done;
					}
					return true;
				}
				return false;
			}
		}

		public GraphicsBuffer computeBuffer => m_ComputeBuffer;

		public unsafe T this[int index]
		{
			get
			{
				return UnsafeUtility.ReadArrayElementWithStride<T>(m_AlignedPtr, index, m_Stride);
			}
			set
			{
				UnsafeUtility.WriteArrayElementWithStride(m_AlignedPtr, index, m_Stride, value);
				computeBufferDirty = true;
			}
		}

		protected unsafe ObiNativeList()
		{
			m_Stride = UnsafeUtility.SizeOf<T>();
		}

		public unsafe ObiNativeList(int capacity = 8, int alignment = 16)
		{
			m_Stride = UnsafeUtility.SizeOf<T>();
			m_AlignBytes = alignment;
			ChangeCapacity(capacity);
		}

		~ObiNativeList()
		{
			Dispose(disposing: false);
		}

		protected unsafe void Dispose(bool disposing)
		{
			DisposeOfComputeBuffer();
			if (isCreated)
			{
				UnsafeUtility.Free(m_AlignedPtr, Allocator.Persistent);
				m_AlignedPtr = null;
				m_Count = (m_Capacity = 0);
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}

		public void DisposeOfComputeBuffer()
		{
			if (m_ComputeBuffer != null)
			{
				WaitForReadback();
				m_ComputeBuffer.Dispose();
				m_ComputeBuffer = null;
			}
			if (m_CountBuffer != null)
			{
				m_CountBuffer.Dispose();
				m_CountBuffer = null;
			}
		}

		public unsafe void OnBeforeSerialize()
		{
			if (isCreated)
			{
				serializedContents = new T[m_Count];
				UnsafeUtility.MemCpy(UnsafeUtility.PinGCArrayAndGetDataAddress(serializedContents, out var gcHandle), m_AlignedPtr, m_Count * m_Stride);
				UnsafeUtility.ReleaseGCObject(gcHandle);
			}
		}

		public unsafe void OnAfterDeserialize()
		{
			if (serializedContents != null)
			{
				ResizeUninitialized(serializedContents.Length);
				ulong gcHandle;
				void* source = UnsafeUtility.PinGCArrayAndGetDataAddress(serializedContents, out gcHandle);
				UnsafeUtility.MemCpy(m_AlignedPtr, source, m_Count * m_Stride);
				UnsafeUtility.ReleaseGCObject(gcHandle);
			}
		}

		public NativeArray<U> AsNativeArray<U>() where U : struct
		{
			return AsNativeArray<U>(m_Count);
		}

		public NativeArray<T> AsNativeArray()
		{
			return AsNativeArray<T>(m_Count);
		}

		public unsafe NativeArray<U> AsNativeArray<U>(int arrayLength) where U : struct
		{
			NativeArray<U> result = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<U>(m_AlignedPtr, Mathf.Min(arrayLength, m_Capacity), Allocator.None);
			computeBufferDirty = true;
			return result;
		}

		public GraphicsBuffer SafeAsComputeBuffer<U>(GraphicsBuffer.Target bufferType = GraphicsBuffer.Target.Structured) where U : struct
		{
			return AsComputeBuffer<U>(Mathf.Max(1, m_Count), bufferType);
		}

		public GraphicsBuffer AsComputeBuffer<U>(GraphicsBuffer.Target bufferType = GraphicsBuffer.Target.Structured) where U : struct
		{
			return AsComputeBuffer<U>(m_Count, bufferType);
		}

		public GraphicsBuffer AsComputeBuffer<U>(int arrayLength, GraphicsBuffer.Target bufferType = GraphicsBuffer.Target.Structured) where U : struct
		{
			DisposeOfComputeBuffer();
			if (arrayLength > 0)
			{
				m_ComputeBufferType = bufferType;
				m_ComputeBuffer = new GraphicsBuffer(bufferType, arrayLength, UnsafeUtility.SizeOf<U>());
				m_ComputeBuffer.SetData(AsNativeArray<U>(arrayLength));
				if (bufferType == GraphicsBuffer.Target.Counter)
				{
					m_Count = 0;
					m_ComputeBuffer.SetCounterValue((uint)m_Count);
					m_CountBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, 1, 4);
					GraphicsBuffer.CopyCount(m_ComputeBuffer, m_CountBuffer, 0);
				}
				return m_ComputeBuffer;
			}
			return null;
		}

		public void Readback<U>(int readcount, bool async) where U : struct
		{
			if (m_ComputeBuffer == null || !m_ComputeBuffer.IsValid() || !noReadbackInFlight)
			{
				return;
			}
			NativeArray<U> output = AsNativeArray<U>(readcount);
			if (output.Length >= readcount && readcount > 0)
			{
				m_AsyncRequest = AsyncGPUReadback.RequestIntoNativeArray(ref output, m_ComputeBuffer, readcount * UnsafeUtility.SizeOf<U>(), 0);
			}
			if (m_ComputeBufferType == GraphicsBuffer.Target.Counter)
			{
				GraphicsBuffer.CopyCount(m_ComputeBuffer, m_CountBuffer, 0);
				m_CounterAsyncRequest = AsyncGPUReadback.Request(m_CountBuffer, m_CountBuffer.stride, 0, delegate(AsyncGPUReadbackRequest request)
				{
					if (!request.hasError)
					{
						m_Count = Mathf.Min(m_Capacity, request.GetData<int>()[0]);
					}
				});
			}
			if (!async)
			{
				WaitForReadback();
			}
		}

		public void Readback(bool async = true)
		{
			if (m_ComputeBuffer != null)
			{
				Readback<T>(m_ComputeBuffer.count, async);
			}
		}

		public void Readback(int readcount, bool async = true)
		{
			Readback<T>(readcount, async);
		}

		public void Upload<U>(int length, bool force = false) where U : struct
		{
			if ((computeBufferDirty || force) && m_ComputeBuffer != null && m_ComputeBuffer.IsValid())
			{
				m_ComputeBuffer.SetData(AsNativeArray<U>(length));
			}
			computeBufferDirty = false;
		}

		public void Upload(bool force = false)
		{
			Upload<T>(m_Count, force);
		}

		public void UploadFullCapacity()
		{
			Upload<T>(m_Capacity, force: true);
		}

		public void WaitForReadback()
		{
			if (isCreated)
			{
				m_AsyncRequest.WaitForCompletion();
				m_CounterAsyncRequest.WaitForCompletion();
			}
		}

		protected unsafe void ChangeCapacity(int newCapacity)
		{
			DisposeOfComputeBuffer();
			m_Stride = UnsafeUtility.SizeOf<T>();
			void* ptr = UnsafeUtility.Malloc(newCapacity * m_Stride, m_AlignBytes, Allocator.Persistent);
			if (isCreated)
			{
				UnsafeUtility.MemCpy(ptr, m_AlignedPtr, Mathf.Min(newCapacity, m_Capacity) * m_Stride);
				UnsafeUtility.Free(m_AlignedPtr, Allocator.Persistent);
			}
			m_AlignedPtr = ptr;
			m_Capacity = newCapacity;
		}

		public unsafe bool Compare(ObiNativeList<T> other)
		{
			if (other == null || !isCreated || !other.isCreated)
			{
				throw new ArgumentNullException();
			}
			if (m_Count != other.m_Count)
			{
				return false;
			}
			return UnsafeUtility.MemCmp(m_AlignedPtr, other.m_AlignedPtr, m_Count * m_Stride) == 0;
		}

		public unsafe void CopyFrom(ObiNativeList<T> source)
		{
			if (source == null || !isCreated || !source.isCreated)
			{
				throw new ArgumentNullException();
			}
			if (m_Count < source.m_Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			UnsafeUtility.MemCpy(m_AlignedPtr, source.m_AlignedPtr, source.count * m_Stride);
		}

		public unsafe void CopyFrom(ObiNativeList<T> source, int sourceIndex, int destIndex, int length)
		{
			if (source == null || !isCreated || !source.isCreated)
			{
				throw new ArgumentNullException();
			}
			if (length > 0 && source.m_Count != 0)
			{
				if (sourceIndex >= source.m_Count || sourceIndex < 0 || destIndex >= m_Count || destIndex < 0 || sourceIndex + length > source.m_Count || destIndex + length > m_Count)
				{
					throw new ArgumentOutOfRangeException();
				}
				void* source2 = source.AddressOfElement(sourceIndex);
				UnsafeUtility.MemCpy(AddressOfElement(destIndex), source2, length * m_Stride);
			}
		}

		public unsafe void CopyFrom<U>(NativeArray<U> source, int sourceIndex, int destIndex, int length) where U : struct
		{
			if (!isCreated || !source.IsCreated || UnsafeUtility.SizeOf<U>() != m_Stride)
			{
				throw new ArgumentNullException();
			}
			if (length > 0 && source.Length != 0)
			{
				if (sourceIndex >= source.Length || sourceIndex < 0 || destIndex >= m_Count || destIndex < 0 || sourceIndex + length > source.Length || destIndex + length > m_Count)
				{
					throw new ArgumentOutOfRangeException();
				}
				void* source2 = (byte*)source.GetUnsafePtr() + sourceIndex * m_Stride;
				UnsafeUtility.MemCpy(AddressOfElement(destIndex), source2, length * m_Stride);
			}
		}

		public unsafe void CopyFrom(T[] source, int sourceIndex, int destIndex, int length)
		{
			if (source == null || !isCreated)
			{
				throw new ArgumentNullException();
			}
			if (length > 0 && source.Length != 0)
			{
				if (sourceIndex < 0 || destIndex < 0 || sourceIndex + length > source.Length || destIndex + length > m_Count)
				{
					throw new ArgumentOutOfRangeException();
				}
				ulong gcHandle;
				void* source2 = UnsafeUtility.PinGCArrayAndGetDataAddress(source, out gcHandle);
				UnsafeUtility.MemCpy(UnsafeUtility.AddressOf(ref UnsafeUtility.ArrayElementAsRef<T>(m_AlignedPtr, destIndex)), source2, length * m_Stride);
				UnsafeUtility.ReleaseGCObject(gcHandle);
			}
		}

		public unsafe void CopyReplicate(T value, int destIndex, int length)
		{
			if (length > 0)
			{
				if (!isCreated)
				{
					throw new ArgumentNullException();
				}
				if (destIndex >= m_Count || destIndex < 0 || destIndex + length > m_Count)
				{
					throw new ArgumentOutOfRangeException();
				}
				void* source = UnsafeUtility.AddressOf(ref value);
				UnsafeUtility.MemCpyReplicate(AddressOfElement(destIndex), source, m_Stride, length);
			}
		}

		public unsafe void CopyTo(T[] dest, int sourceIndex, int length)
		{
			if (length > 0)
			{
				if (dest == null || !isCreated)
				{
					throw new ArgumentNullException();
				}
				if (sourceIndex < 0 || sourceIndex >= m_Count || sourceIndex + length > m_Count || length > dest.Length)
				{
					throw new ArgumentOutOfRangeException();
				}
				void* source = AddressOfElement(sourceIndex);
				UnsafeUtility.MemCpy(UnsafeUtility.PinGCArrayAndGetDataAddress(dest, out var gcHandle), source, length * m_Stride);
				UnsafeUtility.ReleaseGCObject(gcHandle);
			}
		}

		public void Clear()
		{
			m_Count = 0;
		}

		public void Add(T item)
		{
			EnsureCapacity(m_Count + 1);
			computeBufferDirty = true;
			this[m_Count++] = item;
		}

		public void AddReplicate(T value, int times)
		{
			int destIndex = m_Count;
			ResizeUninitialized(m_Count + times);
			CopyReplicate(value, destIndex, times);
		}

		public void AddRange(T[] array)
		{
			AddRange(array, array.Length);
		}

		public void AddRange(T[] array, int length)
		{
			AddRange(array, 0, length);
		}

		public void AddRange(T[] array, int start, int length)
		{
			int destIndex = m_Count;
			ResizeUninitialized(m_Count + length);
			CopyFrom(array, start, destIndex, length);
		}

		public void AddRange(ObiNativeList<T> array, int length)
		{
			int destIndex = m_Count;
			ResizeUninitialized(m_Count + length);
			CopyFrom(array, 0, destIndex, length);
		}

		public void AddRange(ObiNativeList<T> array, int start, int length)
		{
			int destIndex = m_Count;
			ResizeUninitialized(m_Count + length);
			CopyFrom(array, start, destIndex, length);
		}

		public void AddRange(ObiNativeList<T> array)
		{
			AddRange(array, array.count);
		}

		public void AddRange(IEnumerable<T> enumerable)
		{
			if (enumerable is ICollection<T> { Count: >0 } collection)
			{
				EnsureCapacity(m_Count + collection.Count);
			}
			foreach (T item in enumerable)
			{
				Add(item);
			}
		}

		public void RemoveRange(int index, int count)
		{
			if (index < 0 || count < 0 || index + count > m_Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			for (int i = index; i < m_Count - count; i++)
			{
				this[i] = this[i + count];
			}
			m_Count -= count;
		}

		public void RemoveAt(int index)
		{
			if (index < 0 || index >= count)
			{
				throw new ArgumentOutOfRangeException();
			}
			for (int i = index; i < m_Count - 1; i++)
			{
				this[i] = this[i + 1];
			}
			m_Count--;
		}

		public bool ResizeUninitialized(int newCount)
		{
			newCount = Mathf.Max(0, newCount);
			bool result = EnsureCapacity(newCount);
			m_Count = newCount;
			return result;
		}

		public unsafe bool ResizeInitialized(int newCount, T value = default(T))
		{
			newCount = Mathf.Max(0, newCount);
			bool num = newCount >= m_Capacity || !isCreated;
			bool result = EnsureCapacity(newCount);
			if (num)
			{
				void* source = UnsafeUtility.AddressOf(ref value);
				UnsafeUtility.MemCpyReplicate(AddressOfElement(m_Count), source, m_Stride, m_Capacity - m_Count);
			}
			m_Count = newCount;
			return result;
		}

		public bool EnsureCapacity(int min)
		{
			if (min >= m_Capacity || !isCreated)
			{
				ChangeCapacity(min * 2);
				return true;
			}
			return false;
		}

		public unsafe void WipeToZero()
		{
			if (isCreated)
			{
				UnsafeUtility.MemClear(m_AlignedPtr, count * m_Stride);
				computeBufferDirty = true;
			}
		}

		public unsafe void WipeToValue(T value)
		{
			if (isCreated)
			{
				void* source = UnsafeUtility.AddressOf(ref value);
				UnsafeUtility.MemCpyReplicate(m_AlignedPtr, source, m_Stride, count);
				computeBufferDirty = true;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('[');
			for (int i = 0; i < m_Count; i++)
			{
				stringBuilder.Append(this[i].ToString());
				if (i < m_Count - 1)
				{
					stringBuilder.Append(',');
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		public unsafe void* AddressOfElement(int index)
		{
			return (byte*)m_AlignedPtr + m_Stride * index;
		}

		public NativeReference<int> GetCountReference(Allocator alloc)
		{
			return new NativeReference<int>(m_Count, alloc);
		}

		public unsafe IntPtr GetIntPtr()
		{
			if (isCreated)
			{
				return new IntPtr(m_AlignedPtr);
			}
			return IntPtr.Zero;
		}

		public void Swap(int index1, int index2)
		{
			if (index1 >= 0 && index1 < count && index2 >= 0 && index2 < count)
			{
				T value = this[index1];
				this[index1] = this[index2];
				this[index2] = value;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			int i = 0;
			while (i < count)
			{
				yield return this[i];
				int num = i + 1;
				i = num;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
