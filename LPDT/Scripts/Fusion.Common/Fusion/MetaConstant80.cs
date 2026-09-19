using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 80)]
	public readonly struct MetaConstant80 : IMetaConstant
	{
		public int ExpectedSize => 80;
	}
}
