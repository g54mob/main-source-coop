using System.Runtime.CompilerServices;

namespace MessagePack.Internal
{
	internal static class UnsafeRefDeserializeHelper
	{
		internal static int Deserialize(ref byte input, int length, ref bool output)
		{
			for (int i = 0; i < length; i = checked(i + 1))
			{
				switch (Unsafe.Add(ref input, i))
				{
				case 195:
					Unsafe.Add(ref output, i) = true;
					break;
				case 194:
					Unsafe.Add(ref output, i) = false;
					break;
				default:
					return i;
				}
			}
			return -1;
		}
	}
}
