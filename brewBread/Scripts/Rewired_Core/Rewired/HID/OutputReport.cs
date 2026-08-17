using System;
using System.Runtime.InteropServices;

namespace Rewired.HID
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal struct OutputReport
	{
		public IntPtr buffer;

		public int bufferLength;

		public int reportLength;

		public OutputReportOptions options;

		public bool IsValid
		{
			get
			{
				if (buffer != IntPtr.Zero && bufferLength > 0)
				{
					return reportLength > 0;
				}
				return false;
			}
		}

		public OutputReport(IntPtr P_0, int P_1, int P_2)
		{
			buffer = P_0;
			bufferLength = P_1;
			reportLength = P_2;
			options = OutputReportOptions.None;
		}

		public void Clear()
		{
			buffer = IntPtr.Zero;
			bufferLength = 0;
			reportLength = 0;
			options = OutputReportOptions.None;
		}

		public override string ToString()
		{
			string text = "OutputReport:\n";
			text = text + "buffer = " + ((buffer == IntPtr.Zero) ? "NULL" : ("Is Valid (" + buffer + ")")) + "\n";
			text = text + "bufferLength = " + bufferLength + "\n";
			text = text + "reportLength = " + reportLength + "\n";
			string text2 = text;
			int num = (int)options;
			text = text2 + "options = " + num + "\n";
			if (buffer != IntPtr.Zero)
			{
				text += "Buffer data:\n";
				for (int i = 0; i < reportLength; i++)
				{
					text += Marshal.ReadByte(buffer, i).ToString("X2");
					if (i < reportLength - 1)
					{
						text += ", ";
					}
				}
			}
			return text;
		}
	}
}
