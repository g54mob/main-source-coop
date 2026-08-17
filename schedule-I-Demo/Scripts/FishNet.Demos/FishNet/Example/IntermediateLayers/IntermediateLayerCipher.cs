using System;
using FishNet.Managing.Transporting;

namespace FishNet.Example.IntermediateLayers
{
	public class IntermediateLayerCipher : IntermediateLayer
	{
		private const byte CIPHER_KEY = 5;

		public override ArraySegment<byte> HandleIncoming(ArraySegment<byte> src, bool fromServer)
		{
			byte[] array = src.Array;
			int count = src.Count;
			int offset = src.Offset;
			for (int i = src.Offset; i < offset + count; i++)
			{
				short num = (short)(array[i] - 5);
				if (num < 0)
				{
					num += 255;
				}
				array[i] = (byte)num;
			}
			return src;
		}

		public override ArraySegment<byte> HandleOutgoing(ArraySegment<byte> src, bool toServer)
		{
			byte[] array = src.Array;
			int count = src.Count;
			int offset = src.Offset;
			for (int i = offset; i < offset + count; i++)
			{
				short num = (short)(array[i] + 5);
				if (num > 255)
				{
					num -= 255;
				}
				array[i] = (byte)num;
			}
			return src;
		}
	}
}
