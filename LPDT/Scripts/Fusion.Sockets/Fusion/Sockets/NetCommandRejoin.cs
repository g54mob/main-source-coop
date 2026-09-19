using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Fusion.Sockets
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct NetCommandRejoin
	{
		public const int SIZE_BYTES = 518;

		public const int SIZE_BITS = 4144;

		public const int SESSION_MAX_LENGTH_BYTES = 512;

		public const int SESSION_MAX_LENGTH_CHARS = 200;

		[FieldOffset(0)]
		public NetCommandHeader Header;

		[FieldOffset(2)]
		public int SessionIDLength;

		[FieldOffset(6)]
		public unsafe fixed byte SessionName[512];

		private static int ClampLength(int tokenLength)
		{
			if (tokenLength >= 0)
			{
				if (tokenLength > 512)
				{
					InternalLogStreams.LogWarn?.Log($"Rejoin Session Name length too large, truncated to {512} bytes.");
				}
			}
			else
			{
				InternalLogStreams.LogWarn?.Log("Rejoin Session Name length can't be negative");
			}
			return Math.Min(tokenLength, 512);
		}

		public unsafe static NetCommandRejoin Create(string sessionID)
		{
			Assert.Always(sessionID.Length <= 200, "sessionID.Length <= SESSION_MAX_LENGTH_CHARS");
			byte[] bytes = Encoding.UTF8.GetBytes(sessionID);
			int num = ClampLength(bytes.Length);
			Assert.Always(num <= 512, "sessionIDLength <= SESSION_MAX_LENGTH_BYTES");
			NetCommandRejoin result = new NetCommandRejoin
			{
				Header = NetCommands.Rejoin,
				SessionIDLength = num
			};
			fixed (byte* source = bytes)
			{
				FusionUnsafe.Copy(result.SessionName, source, result.SessionIDLength);
			}
			return result;
		}

		public unsafe static string GetSessionId(NetCommandRejoin command)
		{
			int num = ClampLength(command.SessionIDLength);
			return (num == 0) ? null : Encoding.UTF8.GetString(command.SessionName, num);
		}
	}
}
