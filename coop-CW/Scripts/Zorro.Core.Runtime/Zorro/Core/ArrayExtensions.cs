using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Zorro.Core
{
	public static class ArrayExtensions
	{
		public static Vector3 GetRandomPoint(this Vector3[] array)
		{
			int num = UnityEngine.Random.Range(0, array.Length);
			return array[num];
		}

		public static Vector3 GetRandomPoint(this Transform[] array)
		{
			int num = UnityEngine.Random.Range(0, array.Length);
			return array[num].position;
		}

		public static NativeArray<float3> ToFloat3NativeArray(this Vector3[] array, Allocator allocator)
		{
			NativeArray<float3> result = new NativeArray<float3>(array.Length, allocator);
			for (int i = 0; i < array.Length; i++)
			{
				result[i] = array[i];
			}
			return result;
		}

		public static NativeArray<T> ToNativeArray<T>(this T[] array, Allocator allocator) where T : struct
		{
			NativeArray<T> result = new NativeArray<T>(array.Length, allocator);
			for (int i = 0; i < array.Length; i++)
			{
				result[i] = array[i];
			}
			return result;
		}

		public static T GetRandom<T>(this T[] array)
		{
			int num = UnityEngine.Random.Range(0, array.Length);
			return array[num];
		}
	}
}
