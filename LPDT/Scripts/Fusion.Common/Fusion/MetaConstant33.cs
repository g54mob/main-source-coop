using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 33)]
	public readonly struct MetaConstant33 : IMetaConstant
	{
		public int ExpectedSize => 33;
	}
}
