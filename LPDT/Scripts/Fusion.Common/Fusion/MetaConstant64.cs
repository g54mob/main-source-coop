using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 64)]
	public readonly struct MetaConstant64 : IMetaConstant
	{
		public int ExpectedSize => 64;
	}
}
