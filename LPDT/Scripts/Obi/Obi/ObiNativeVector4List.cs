using System;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiNativeVector4List : ObiNativeList<Vector4>
	{
		public ObiNativeVector4List()
		{
		}

		public ObiNativeVector4List(int capacity = 8, int alignment = 16)
			: base(capacity, alignment)
		{
			for (int i = 0; i < capacity; i++)
			{
				base[i] = Vector4.zero;
			}
		}

		public unsafe Vector3 GetVector3(int index)
		{
			return *(Vector3*)((byte*)m_AlignedPtr + index * sizeof(Vector4));
		}

		public unsafe void SetVector3(int index, Vector3 value)
		{
			*(Vector3*)((byte*)m_AlignedPtr + index * sizeof(Vector4)) = value;
		}
	}
}
