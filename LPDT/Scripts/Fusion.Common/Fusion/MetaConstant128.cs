using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 128)]
	public readonly struct MetaConstant128 : IMetaConstant
	{
		public int ExpectedSize => 128;
	}
}
