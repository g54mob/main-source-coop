using System.IO;
using UnityEngine;

namespace Zorro.Core
{
	public static class BinaryWriterExtensions
	{
		public static void Write(this BinaryWriter binaryWriter, Vector3 vec3)
		{
			binaryWriter.Write(vec3.x);
			binaryWriter.Write(vec3.y);
			binaryWriter.Write(vec3.z);
		}
	}
}
