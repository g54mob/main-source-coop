using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public static class ExtCompute
	{
		public static ComputeBuffer CreateComputeBuffer<T>(this T[] me, ComputeBufferType type = ComputeBufferType.Structured) where T : struct
		{
			ComputeBuffer computeBuffer = new ComputeBuffer(me.Count(), Marshal.SizeOf(typeof(T)), type);
			computeBuffer.SetData(me);
			return computeBuffer;
		}

		public static ComputeBuffer SetAndCreateComputeBuffer<T>(this T[] me, ComputeShader cs, int kId, string name, ComputeBufferType type = ComputeBufferType.Structured)
		{
			ComputeBuffer computeBuffer = new ComputeBuffer(me.Length, Marshal.SizeOf(typeof(T)), type);
			computeBuffer.SetData(me);
			cs.SetBuffer(kId, name, computeBuffer);
			return computeBuffer;
		}

		public static ComputeBuffer SetAndCreateComputeBuffer<T>(this NativeArray<T> me, ComputeShader cs, int kId, string name, ComputeBufferType type = ComputeBufferType.Structured) where T : unmanaged
		{
			ComputeBuffer computeBuffer = new ComputeBuffer(me.Length, Marshal.SizeOf(typeof(T)), type);
			computeBuffer.SetData(me);
			cs.SetBuffer(kId, name, computeBuffer);
			return computeBuffer;
		}

		public static ComputeBuffer CreateComputeBuffer<T>(this List<T> me, ComputeBufferType type = ComputeBufferType.Structured) where T : struct
		{
			ComputeBuffer computeBuffer = new ComputeBuffer(me.Count, Marshal.SizeOf(typeof(T)), type);
			computeBuffer.SetData(me);
			return computeBuffer;
		}

		public static void PDispatch(this ComputeShader me, int kid, string threadGroupName, float x = 1f, float y = 1f, float z = 1f)
		{
			me.GetKernelThreadGroupSizes(kid, out var x2, out var y2, out var z2);
			int threadGroupsX = Mathf.CeilToInt(x / (float)x2);
			int threadGroupsY = Mathf.CeilToInt(y / (float)y2);
			int threadGroupsZ = Mathf.CeilToInt(z / (float)z2);
			me.SetVector(threadGroupName, new Vector3(x, y, z));
			me.Dispatch(kid, threadGroupsX, threadGroupsY, threadGroupsZ);
		}
	}
}
