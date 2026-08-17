namespace Mirror.BouncyCastle.Math.EC.Rfc8032
{
	internal static class Codec
	{
		internal static uint Decode32(byte[] bs, int off)
		{
			return (uint)(bs[off] | (bs[++off] << 8) | (bs[++off] << 16) | (bs[++off] << 24));
		}

		internal static void Decode32(byte[] bs, int bsOff, uint[] n, int nOff, int nLen)
		{
			for (int i = 0; i < nLen; i++)
			{
				n[nOff + i] = Decode32(bs, bsOff + i * 4);
			}
		}
	}
}
