using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 256)]
	public readonly struct MetaConstant256 : IMetaConstant
	{
		public int ExpectedSize => 256;
	}
}
