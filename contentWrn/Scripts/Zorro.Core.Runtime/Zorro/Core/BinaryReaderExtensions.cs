using System.IO;
using UnityEngine;

namespace Zorro.Core
{
	public static class BinaryReaderExtensions
	{
		public static Quaternion ReadQuaternion(this BinaryReader binaryReader)
		{
			byte b = binaryReader.ReadByte();
			if (b >= 4)
			{
				return BinarySerializationUtility.ConstructQuaternion(b);
			}
			return BinarySerializationUtility.ConstructQuaternion(b, binaryReader.ReadBytes(6));
		}

		public static Vector3 ReadVector3(this BinaryReader binaryReader)
		{
			return new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
		}
	}
}
