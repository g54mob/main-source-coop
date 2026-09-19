using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 250)]
	public readonly struct MetaConstant250 : IMetaConstant
	{
		public int ExpectedSize => 250;
	}
}
