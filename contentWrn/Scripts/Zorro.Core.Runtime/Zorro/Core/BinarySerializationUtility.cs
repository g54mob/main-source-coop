using System.IO;
using UnityEngine;

namespace Zorro.Core
{
	public static class BinarySerializationUtility
	{
		private static readonly float QUATERNION_PRECISION_MULT = 10000f;

		public static byte[] OptimizeQuaternion(Quaternion val)
		{
			byte b = 0;
			float num = Mathf.Abs(val.x);
			float num2 = 1f;
			if (Mathf.Abs(val.y) > num)
			{
				b = 1;
				num = val.y;
				num2 = ((val.y < 0f) ? (-1f) : 1f);
			}
			if (Mathf.Abs(val.z) > num)
			{
				b = 2;
				num = val.z;
				num2 = ((val.z < 0f) ? (-1f) : 1f);
			}
			if (Mathf.Abs(val.w) > num)
			{
				b = 3;
				num = val.w;
				num2 = ((val.w < 0f) ? (-1f) : 1f);
			}
			if (Mathf.Approximately(num, 1f))
			{
				b += 4;
				return new byte[1] { b };
			}
			short value;
			short value2;
			short value3;
			switch (b)
			{
			case 0:
				value = (short)(val.y * num2 * QUATERNION_PRECISION_MULT);
				value2 = (short)(val.z * num2 * QUATERNION_PRECISION_MULT);
				value3 = (short)(val.w * num2 * QUATERNION_PRECISION_MULT);
				break;
			case 1:
				value = (short)(val.x * num2 * QUATERNION_PRECISION_MULT);
				value2 = (short)(val.z * num2 * QUATERNION_PRECISION_MULT);
				value3 = (short)(val.w * num2 * QUATERNION_PRECISION_MULT);
				break;
			case 2:
				value = (short)(val.x * num2 * QUATERNION_PRECISION_MULT);
				value2 = (short)(val.y * num2 * QUATERNION_PRECISION_MULT);
				value3 = (short)(val.w * num2 * QUATERNION_PRECISION_MULT);
				break;
			default:
				value = (short)(val.x * num2 * QUATERNION_PRECISION_MULT);
				value2 = (short)(val.y * num2 * QUATERNION_PRECISION_MULT);
				value3 = (short)(val.z * num2 * QUATERNION_PRECISION_MULT);
				break;
			}
			byte[] array = new byte[7];
			using MemoryStream output = new MemoryStream(array);
			using BinaryWriter binaryWriter = new BinaryWriter(output);
			binaryWriter.Write(b);
			binaryWriter.Write(value);
			binaryWriter.Write(value2);
			binaryWriter.Write(value3);
			return array;
		}

		public static object DeserializeQuaternion(BinaryReader arg)
		{
			byte b = arg.ReadByte();
			if (b >= 4)
			{
				return ConstructQuaternion(b);
			}
			return ConstructQuaternion(b, arg.ReadBytes(6));
		}

		public static Quaternion ConstructQuaternion(byte maxIndex, byte[] data)
		{
			using MemoryStream input = new MemoryStream(data);
			using BinaryReader binaryReader = new BinaryReader(input);
			float num = (float)binaryReader.ReadInt16() / QUATERNION_PRECISION_MULT;
			float num2 = (float)binaryReader.ReadInt16() / QUATERNION_PRECISION_MULT;
			float num3 = (float)binaryReader.ReadInt16() / QUATERNION_PRECISION_MULT;
			float num4 = Mathf.Sqrt(1f - (num * num + num2 * num2 + num3 * num3));
			return maxIndex switch
			{
				0 => new Quaternion(num4, num, num2, num3), 
				1 => new Quaternion(num, num4, num2, num3), 
				2 => new Quaternion(num, num2, num4, num3), 
				_ => new Quaternion(num, num2, num3, num4), 
			};
		}

		public static Quaternion ConstructQuaternion(byte data)
		{
			float x = ((data == 4) ? 1f : 0f);
			float y = ((data == 5) ? 1f : 0f);
			float z = ((data == 6) ? 1f : 0f);
			float w = ((data == 7) ? 1f : 0f);
			return new Quaternion(x, y, z, w);
		}
	}
}
