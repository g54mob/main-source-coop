using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 255)]
	public readonly struct MetaConstant255 : IMetaConstant
	{
		public int ExpectedSize => 255;
	}
}
