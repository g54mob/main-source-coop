using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 1)]
	public readonly struct MetaConstant1 : IMetaConstant
	{
		public int ExpectedSize => 1;
	}
}
