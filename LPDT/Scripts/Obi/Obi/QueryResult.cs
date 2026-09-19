using System.Runtime.InteropServices;
using UnityEngine;

namespace Obi
{
	[StructLayout(LayoutKind.Sequential, Size = 64)]
	public struct QueryResult
	{
		public Vector4 simplexBary;

		public Vector4 queryPoint;

		public Vector4 normal;

		public float distance;

		public float distanceAlongRay;

		public int simplexIndex;

		public int queryIndex;
	}
}
