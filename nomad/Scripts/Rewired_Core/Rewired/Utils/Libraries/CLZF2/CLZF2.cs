using System;

namespace Rewired.Utils.Libraries.CLZF2
{
	public sealed class CLZF2
	{
		private enum VdBClpseeZvhihQkfIGnHWNsrhvi
		{
			Success = 0,
			OutputBufferTooSmall = 1,
			Einval = 2,
			ZeroSize = 3
		}

		private const uint rFofgkgstvlHqQzMDmuxSxODWwubA = 14u;

		private const uint IYpANdAwuEWzzwEtCGYKcprMBbLx = 16384u;

		private const uint KOYAZWYjictaVMmLDdthNwRrrbxs = 32u;

		private const uint PMBaoxdUbrVesryvlnlXDCboVPQI = 8192u;

		private const uint VCGXQbConNDjHJHegyDPzGfxmeNu = 264u;

		private readonly long[] ZoNwsCtaHUqmVjwoNLkEyDqQkBnE = new long[16384];

		public byte[] Compress(byte[] inputBytes)
		{
			int num = inputBytes.Length * 2;
			byte[] src = new byte[num];
			int num2;
			for (num2 = LdewzAEZWpaDupMkCIKlkpfTOVZG(inputBytes, ref src); num2 == 0; num2 = LdewzAEZWpaDupMkCIKlkpfTOVZG(inputBytes, ref src))
			{
				num *= 2;
				src = new byte[num];
			}
			byte[] array = new byte[num2];
			Buffer.BlockCopy(src, 0, array, 0, num2);
			return array;
		}

		public byte[] Decompress(byte[] inputBytes)
		{
			int num = inputBytes.Length * 2;
			byte[] array = new byte[num];
			VdBClpseeZvhihQkfIGnHWNsrhvi vdBClpseeZvhihQkfIGnHWNsrhvi;
			int num2 = AIadtabBQVaErxmugnFNbVyrxHDq(inputBytes, array, out vdBClpseeZvhihQkfIGnHWNsrhvi);
			while (num2 == 0 && vdBClpseeZvhihQkfIGnHWNsrhvi == VdBClpseeZvhihQkfIGnHWNsrhvi.OutputBufferTooSmall)
			{
				num *= 2;
				array = new byte[num];
				num2 = AIadtabBQVaErxmugnFNbVyrxHDq(inputBytes, array, out vdBClpseeZvhihQkfIGnHWNsrhvi);
			}
			if (vdBClpseeZvhihQkfIGnHWNsrhvi == VdBClpseeZvhihQkfIGnHWNsrhvi.Success)
			{
				byte[] array2 = new byte[num2];
				Buffer.BlockCopy(array, 0, array2, 0, num2);
				return array2;
			}
			return new byte[0];
		}

		private int LdewzAEZWpaDupMkCIKlkpfTOVZG(byte[] P_0, ref byte[] P_1)
		{
			int num = P_0.Length;
			int num2 = P_1.Length;
			Array.Clear(ZoNwsCtaHUqmVjwoNLkEyDqQkBnE, 0, 16384);
			uint num3 = 0u;
			uint num4 = 0u;
			uint num5 = (uint)((P_0[num3] << 8) | P_0[num3 + 1]);
			int num6 = 0;
			while (true)
			{
				if (num3 < num - 2)
				{
					num5 = (num5 << 8) | P_0[num3 + 2];
					long num7 = ((num5 ^ (num5 << 5)) >> (int)(10 - num5 * 5)) & 0x3FFF;
					long num8 = ZoNwsCtaHUqmVjwoNLkEyDqQkBnE[num7];
					ZoNwsCtaHUqmVjwoNLkEyDqQkBnE[num7] = num3;
					long num9;
					if ((num9 = num3 - num8 - 1) < 8192 && num3 + 4 < num && num8 > 0 && P_0[num8] == P_0[num3] && P_0[num8 + 1] == P_0[num3 + 1] && P_0[num8 + 2] == P_0[num3 + 2])
					{
						uint num10 = 2u;
						uint num11 = (uint)(num - (int)num3) - num10;
						num11 = ((num11 > 264) ? 264u : num11);
						if (num4 + num6 + 1 + 3 >= num2)
						{
							return 0;
						}
						do
						{
							num10++;
						}
						while (num10 < num11 && P_0[num8 + num10] == P_0[num3 + num10]);
						if (num6 != 0)
						{
							P_1[num4++] = (byte)(num6 - 1);
							num6 = -num6;
							do
							{
								P_1[num4++] = P_0[num3 + num6];
							}
							while (++num6 != 0);
						}
						num10 -= 2;
						num3++;
						if (num10 < 7)
						{
							P_1[num4++] = (byte)((num9 >> 8) + (num10 << 5));
						}
						else
						{
							P_1[num4++] = (byte)((num9 >> 8) + 224);
							P_1[num4++] = (byte)(num10 - 7);
						}
						P_1[num4++] = (byte)num9;
						num3 += num10 - 1;
						num5 = (uint)((P_0[num3] << 8) | P_0[num3 + 1]);
						num5 = (num5 << 8) | P_0[num3 + 2];
						ZoNwsCtaHUqmVjwoNLkEyDqQkBnE[((num5 ^ (num5 << 5)) >> (int)(10 - num5 * 5)) & 0x3FFF] = num3;
						num3++;
						num5 = (num5 << 8) | P_0[num3 + 2];
						ZoNwsCtaHUqmVjwoNLkEyDqQkBnE[((num5 ^ (num5 << 5)) >> (int)(10 - num5 * 5)) & 0x3FFF] = num3;
						num3++;
						continue;
					}
				}
				else if (num3 == num)
				{
					break;
				}
				num6++;
				num3++;
				if ((long)num6 == 32)
				{
					if (num4 + 1 + 32 >= num2)
					{
						return 0;
					}
					P_1[num4++] = 31;
					num6 = -num6;
					do
					{
						P_1[num4++] = P_0[num3 + num6];
					}
					while (++num6 != 0);
				}
			}
			if (num6 != 0)
			{
				if (num4 + num6 + 1 >= num2)
				{
					return 0;
				}
				P_1[num4++] = (byte)(num6 - 1);
				num6 = -num6;
				do
				{
					P_1[num4++] = P_0[num3 + num6];
				}
				while (++num6 != 0);
			}
			return (int)num4;
		}

		private int AIadtabBQVaErxmugnFNbVyrxHDq(byte[] P_0, byte[] P_1, out VdBClpseeZvhihQkfIGnHWNsrhvi P_2)
		{
			int num = P_0.Length;
			int num2 = P_1.Length;
			uint num3 = 0u;
			uint num4 = 0u;
			do
			{
				uint num5 = P_0[num3++];
				if (num5 < 32)
				{
					num5++;
					if (num4 + num5 > num2)
					{
						P_2 = VdBClpseeZvhihQkfIGnHWNsrhvi.OutputBufferTooSmall;
						return 0;
					}
					do
					{
						P_1[num4++] = P_0[num3++];
					}
					while (--num5 != 0);
					continue;
				}
				uint num6 = num5 >> 5;
				int num7 = (int)(num4 - ((num5 & 0x1F) << 8) - 1);
				if (num6 == 7)
				{
					num6 += P_0[num3++];
				}
				num7 -= P_0[num3++];
				if (num4 + num6 + 2 > num2)
				{
					P_2 = VdBClpseeZvhihQkfIGnHWNsrhvi.OutputBufferTooSmall;
					return 0;
				}
				if (num7 < 0)
				{
					P_2 = VdBClpseeZvhihQkfIGnHWNsrhvi.Einval;
					return 0;
				}
				P_1[num4++] = P_1[num7++];
				P_1[num4++] = P_1[num7++];
				do
				{
					P_1[num4++] = P_1[num7++];
				}
				while (--num6 != 0);
			}
			while (num3 < num);
			P_2 = ((num4 == 0) ? VdBClpseeZvhihQkfIGnHWNsrhvi.ZeroSize : VdBClpseeZvhihQkfIGnHWNsrhvi.Success);
			return (int)num4;
		}
	}
}
