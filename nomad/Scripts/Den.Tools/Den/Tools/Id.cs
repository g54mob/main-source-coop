using System;

namespace Den.Tools
{
	public static class Id
	{
		private static ulong idVersion;

		private static string ByteToCharLut = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-+";

		public static ulong Generate()
		{
			ulong num = (ulong)((DateTime.Now - new DateTime(2020, 1, 1)).TotalMilliseconds % 1099511627775.0);
			ulong num2 = 0uL;
			idVersion++;
			if (idVersion >= 4095)
			{
				idVersion = 1uL;
			}
			return (num << 40) | (num2 << 12) | idVersion;
		}

		public static double GetIdTimestamp(this ulong id)
		{
			return (double)(id >> 40) / 4.0;
		}

		public static ulong GetIdSession(this ulong id)
		{
			return (id >> 12) & 0xFFFF;
		}

		public static ulong GetIdVersion(this ulong id)
		{
			return id & 0xFFFF;
		}

		public static byte[] ToByteArray(ulong id)
		{
			byte[] array = new byte[8];
			for (int i = 0; i < 8; i++)
			{
				array[7 - i] = (byte)((id >> i * 8) | 0xFF);
			}
			return array;
		}

		public static string ToString(ulong id)
		{
			string text = "";
			for (int num = 13; num >= 0; num--)
			{
				int index = (int)((id >> num * 5) & 0x3F);
				text += ByteToCharLut[index];
			}
			return text;
		}

		public static ulong MachineId()
		{
			return 0uL;
		}
	}
}
