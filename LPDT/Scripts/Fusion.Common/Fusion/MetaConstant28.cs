using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 28)]
	public readonly struct MetaConstant28 : IMetaConstant
	{
		public int ExpectedSize => 28;
	}
}
