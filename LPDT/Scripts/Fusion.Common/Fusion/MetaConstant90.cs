using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 90)]
	public readonly struct MetaConstant90 : IMetaConstant
	{
		public int ExpectedSize => 90;
	}
}
