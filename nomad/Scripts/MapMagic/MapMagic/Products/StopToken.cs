using System;
using System.Runtime.InteropServices;

namespace MapMagic.Products
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class StopToken
	{
		public bool stop;

		public bool restart;
	}
}
