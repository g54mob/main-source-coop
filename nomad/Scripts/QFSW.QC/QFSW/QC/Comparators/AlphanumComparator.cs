using System.Collections.Generic;

namespace QFSW.QC.Comparators
{
	public class AlphanumComparator : IComparer<string>
	{
		private const int MaxStackSize = 512;

		public unsafe int Compare(string x, string y)
		{
			if (x == null)
			{
				return 0;
			}
			if (y == null)
			{
				return 0;
			}
			int length = x.Length;
			int length2 = y.Length;
			if (length + length2 + 2 <= 512)
			{
				char* buffer = stackalloc char[length + 1];
				char* buffer2 = stackalloc char[length2 + 1];
				return Compare(x, buffer, length, y, buffer2, length2);
			}
			char[] array = new char[length + 1];
			char[] array2 = new char[length2 + 1];
			fixed (char* buffer3 = array)
			{
				fixed (char* buffer4 = array2)
				{
					return Compare(x, buffer3, length, y, buffer4, length2);
				}
			}
		}

		public unsafe int Compare(string x, char* buffer1, int len1, string y, char* buffer2, int len2)
		{
			int num = 0;
			int num2 = 0;
			while (num < len1 && num2 < len2)
			{
				char c = x[num];
				char c2 = y[num2];
				int num3 = 0;
				int num4 = 0;
				do
				{
					buffer1[num3++] = c;
					num++;
					if (num >= len1)
					{
						break;
					}
					c = x[num];
				}
				while (char.IsDigit(c) == char.IsDigit(*buffer1));
				do
				{
					buffer2[num4++] = c2;
					num2++;
					if (num2 >= len2)
					{
						break;
					}
					c2 = y[num2];
				}
				while (char.IsDigit(c2) == char.IsDigit(*buffer2));
				buffer1[num3] = (buffer2[num4] = '\0');
				int num7;
				if (char.IsDigit(*buffer1) && char.IsDigit(*buffer2))
				{
					int num5 = ParseInt(buffer1);
					int num6 = ParseInt(buffer2);
					num7 = num5 - num6;
				}
				else
				{
					num7 = CompareStrings(buffer1, buffer2);
				}
				if (num7 != 0)
				{
					return num7;
				}
			}
			return len1 - len2;
		}

		private unsafe int ParseInt(char* buffer)
		{
			int num = 0;
			while (*buffer != 0)
			{
				num *= 10;
				num += *(buffer++) - 48;
			}
			return num;
		}

		private unsafe int CompareStrings(char* buffer1, char* buffer2)
		{
			int num = 0;
			while (buffer1[num] != 0 && buffer2[num] != 0)
			{
				char c = buffer1[num];
				char c2 = buffer2[num++];
				if (c > c2)
				{
					return 1;
				}
				if (c < c2)
				{
					return -1;
				}
			}
			if (buffer1[num] != 0)
			{
				return 1;
			}
			if (buffer2[num] != 0)
			{
				return -1;
			}
			return 0;
		}
	}
}
